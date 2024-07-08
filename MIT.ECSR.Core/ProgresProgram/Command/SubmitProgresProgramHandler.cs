using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Core.Helper;
using MIT.ECSR.Core.Notification.Command;
using MIT.ECSR.Core.Request;
using MIT.ECSR.Data;
using MIT.ECSR.Data.Model;
using MIT.ECSR.Shared.Attributes;
using System.ComponentModel.DataAnnotations;
using WonderKid.DAL.Interface;

namespace MIT.ECSR.Core.ProgresProgram.Command
{
    #region Request
    public class SubmitProgresProgramMapping : Profile
    {
        public SubmitProgresProgramMapping()
        {
            CreateMap<SubmitProgresProgramRequest, ApproveRequest>().ReverseMap();
        }
    }
    public class SubmitProgresProgramRequest : ApproveRequest, IRequest<StatusResponse>
    {
        [Required]
        public string Fullname { get; set; }
    }
    #endregion
    internal class SubmitProgresProgramHandler : IRequestHandler<SubmitProgresProgramRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public SubmitProgresProgramHandler(
            ILogger<ApprovalProgresProgramHandler> logger,
            IMediator mediator,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mediator = mediator;
            _context = context;
        }

        public async Task<StatusResponse> Handle(SubmitProgresProgramRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                var progress = await _context.Entity<TrsProgresProgram>().Include(x => x.IdProgramItemNavigation).Where(d => d.Id == request.Id).FirstOrDefaultAsync();
                if (progress != null)
                {
                    if (request.IsApprove)
                    {
                        progress.Status = (int)ProgressStatusEnum.WAITING;
                        progress.ApprovedBy = request.Fullname;
                        progress.ApprovedAt = DateTime.Now;
                        progress.Notes = request.Notes;
                        _context.Update(progress);
                        if (progress.Progress >= 100)
                        {
                            var rolesOpd = await _context.Entity<SetRole>().Where(x => x.Name.Contains(RoleName.DPMPTSPP.ToString()))
                                .Include(x => x.SetUser).SelectMany(x => x.SetUser).ToListAsync();

                            foreach (var item in rolesOpd)
                            {
                                await _mediator.Send(new AddNotificationRequest
                                {
                                    Description = $"SUB KEGIATAN {progress.IdProgramItemNavigation.Nama} meunggu approval",
                                    IdUser = item.Id,
                                    Inputer = request.Fullname,
                                    IsOpen = false,
                                    Subject = "MENUNGGU PERSETUJUAN PROGRESS 100%",
                                    Navigation = "/Monitoring"
                                });
                            }
                        }
                    }
                    else
                    {
                        _context.Delete(progress);
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
                _logger.LogError(ex, "Failed Submit progress", request);
                result.Error("Failed Submit progress", ex.Message);
            }
            return result;
        }
    }
}
