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

namespace MIT.ECSR.Core.Penawaran.Command
{

    #region Request
    public class ApprovalPenawaranMapping : Profile
    {
        public ApprovalPenawaranMapping()
        {
            CreateMap<ApprovalPenawaranRequest, ApproveRequest>().ReverseMap();
        }
    }
    public class ApprovalPenawaranRequest:ApproveRequest, IRequest<StatusResponse>
    {
        [Required]
        public string Fullname { get; set; }
    }
    #endregion

    internal class ApprovalPenawaranHandler : IRequestHandler<ApprovalPenawaranRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IWrapperHelper _wrapper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public ApprovalPenawaranHandler(
            ILogger<PengajuanPenawaranHandler> logger,
            IWrapperHelper wrapper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _wrapper = wrapper;
            _context = context;
        }
        public async Task<StatusResponse> Handle(ApprovalPenawaranRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                var penawaran = await _context.Entity<TrsPenawaranItem>().Where(d => d.Id == request.Id).Include(d=>d.IdProgramItemNavigation).FirstOrDefaultAsync();
                if(penawaran!=null)
                {
                    penawaran.Notes = request.Notes;
                    penawaran.ApprovedBy = request.Fullname;
                    penawaran.ApprovedAt = DateTime.Now;
                    if (request.IsApprove)
                    {
                        penawaran.Status = (int)PenawaranStatusEnum.SUBMIT;
                        var save = await _context.UpdateSave(penawaran);
                        if (save.Success)
                        {
                            result.OK();
                            var program = await _context.Entity<TrsProgram>().Where(d => d.Id == penawaran.IdProgramItemNavigation.IdProgram).FirstOrDefaultAsync();
                            program.Status = (int)ProgramStatusEnum.ON_PROGRESS;
                            result = _wrapper.Response(await _context.UpdateSave(program));
                        }
                        else
                            result.BadRequest(save.Message);
                    }
                    else
                    {
                        var save = await _context.DeleteSave(penawaran);
                        if (save.Success)
                            result.OK();
                        else
                            result.BadRequest(save.Message);
                    }
                }
                else
                    result.NotFound("penawaran tidak ditemukan!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Approve Penawaran", request);
                result.Error("Failed Approve Penawaran", ex.Message);
            }
            return result;
        }
    }
}

