using AutoMapper;
using MediatR;
using WonderKid.DAL.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Data;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.Helper;
using MIT.ECSR.Core.Request;
using MIT.ECSR.Data.Model;
using MIT.ECSR.Core.Media.Command;
using MIT.ECSR.Shared.Interface;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;

namespace MIT.ECSR.Core.ProgresProgram.Command
{

    #region Request
    public class ApprovalProgresProgramMapping : Profile
    {
        public ApprovalProgresProgramMapping()
        {
            CreateMap<ApprovalProgresProgramRequest, ApproveRequest>().ReverseMap();
        }
    }
    public class ApprovalProgresProgramRequest : ApproveRequest, IRequest<StatusResponse>
    {
        [Required]
        public string Fullname { get; set; }
    }
    #endregion

    internal class ApprovalProgresProgramHandler : IRequestHandler<ApprovalProgresProgramRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public ApprovalProgresProgramHandler(
            ILogger<ApprovalProgresProgramHandler> logger,
            IMediator mediator,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mediator = mediator;
            _context = context;
        }
        public async Task<StatusResponse> Handle(ApprovalProgresProgramRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                var progress = await _context.Entity<TrsProgresProgram>().Include(x => x.IdProgramItemNavigation).Where(d => d.Id == request.Id).FirstOrDefaultAsync();
                if (progress != null)
                {
                    progress.Status = request.IsApprove ? (int)ProgressStatusEnum.APPROVE : (int)ProgressStatusEnum.REJECT;
                    progress.ApprovedBy = request.Fullname;
                    progress.ApprovedAt = DateTime.Now;
                    progress.Notes = request.Notes;
                    _context.Update(progress);
                    if(request.IsApprove)
                    {
                        var item = await _context.Entity<TrsProgramItem>().Where(d => d.Id == progress.IdProgramItem).FirstOrDefaultAsync();
                        item.Progress = progress.Progress;
                        _context.Update(item);

                        if (item.Progress >= 100)
                        {
                            var penawaran = await _context.Entity<TrsPenawaranItem>().Include(x => x.IdPenawaranNavigation).Where(d => d.IdProgramItem == progress.IdProgramItem && d.IdPenawaranNavigation.IdPerusahaan == progress.IdPerusahaan).FirstOrDefaultAsync();
                            penawaran.Status = (int)PenawaranStatusEnum.CLOSED;
                            _context.Update(penawaran);
                        }

                        var idProgram = progress.IdProgramItemNavigation.IdProgram;
                        var listProgramItem = _context.Entity<TrsProgramItem>().Where(x => x.IdProgram == idProgram).Include(x => x.TrsPenawaranItem).ToList();
                        var countJumlahProgram = listProgramItem.Sum(x => x.Jumlah);
                        var countClosedProgram = listProgramItem.Where(x => x.TrsPenawaranItem.Any(z => z.Status == (int)PenawaranStatusEnum.CLOSED)).Select(x => x.TrsPenawaranItem.Sum(z => z.Jumlah)).Sum(x => x);
                        if ((countJumlahProgram - countClosedProgram) < 1)
                        {
                            var program = _context.Entity<TrsProgram>().FirstOrDefault(x => x.Id == progress.IdProgramItemNavigation.IdProgram);
                            program.Status= (int)ProgramStatusEnum.CLOSED;
                            _context.Update(program);
                        }
                    }
                    var save = await _context.Commit();
                    if (save.Success)
                        result.OK();
                    else
                        result.BadRequest(save.Message);
                }
                else
                    result.NotFound("progress tidak ditemukan!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Approve progress", request);
                result.Error("Failed Approve progress", ex.Message);
            }
            return result;
        }
    }
}

