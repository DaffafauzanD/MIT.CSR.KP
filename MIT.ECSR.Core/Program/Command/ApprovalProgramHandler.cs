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
using MIT.ECSR.Shared.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderKid.DAL.Interface;

namespace MIT.ECSR.Core.Program.Command
{

    #region Request
    public class ApprovalProgramMapping : Profile
    {
        public ApprovalProgramMapping()
        {
            CreateMap<ApprovalProgramRequest, ApproveRequest>().ReverseMap();
        }
    }
    public class ApprovalProgramRequest : ApproveRequest, IRequest<StatusResponse>
    {
        [Required]
        public string Fullname { get; set; }
    }
    #endregion

    internal class ApprovalProgramHandler : IRequestHandler<ApprovalProgramRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IWrapperHelper _wrapper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        private readonly IMediator _mediator;
        public ApprovalProgramHandler(
            ILogger<ApprovalProgramHandler> logger,
            IWrapperHelper wrapper,
            IUnitOfWork<ApplicationDBContext> context,
            IMediator mediator)
        {
            _logger = logger;
            _wrapper = wrapper;
            _context = context;
            _mediator = mediator;
        }
        public async Task<StatusResponse> Handle(ApprovalProgramRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                var Program = await _context.Entity<TrsProgram>().Include(x => x.NamaProgramNavigation).Where(d => d.Id == request.Id).FirstOrDefaultAsync();
                if (Program != null)
                {
                    if (Program.Status != (int)ProgramStatusEnum.WAITING_VERIFIKASI && Program.Status != (int)ProgramStatusEnum.WAITING_APPROVAL)
                    {
                        result.BadRequest("Cannot Be Approve/Reject because Status is " + ((ProgramStatusEnum)Program.Status).ToString());
                        return result;
                    }

                    Program.Notes = request.Notes;
                    Program.ApprovedBy = request.Fullname;
                    Program.ApprovedAt = DateTime.Now;
                    Program.UpdateDate = DateTime.Now;
                    if (request.IsApprove)
                    {
                        Program.Status = Program.Status == (short)ProgramStatusEnum.WAITING_VERIFIKASI ? (short)ProgramStatusEnum.WAITING_APPROVAL : (short)ProgramStatusEnum.OPEN;
                        if (Program.Status == (short)ProgramStatusEnum.WAITING_APPROVAL)
                        {
                            var rolesOpd = await _context.Entity<SetRole>().Where(x => x.Name.Contains(RoleName.BAPPEDA.ToString()))
                                .Include(x => x.SetUser).SelectMany(x => x.SetUser).ToListAsync();

                            foreach (var item in rolesOpd)
                            {
                                await _mediator.Send(new AddNotificationRequest
                                {
                                    Description = $"PROGRAM {Program.NamaProgramNavigation.Name} menunggu untuk di approve",
                                    IdUser = item.Id,
                                    Inputer = request.Fullname,
                                    IsOpen = false,
                                    Subject = "MENUNGGU PERSETUJUAN",
                                    Navigation = "/Program"
                                });
                            }
                        }
                        else
                        {
                            var rolesOpd = await _context.Entity<SetRole>().Where(x => x.Name.Contains(RoleName.FORUM.ToString()))
                                .Include(x => x.SetUser).SelectMany(x => x.SetUser).ToListAsync();

                            foreach (var item in rolesOpd)
                            {
                                await _mediator.Send(new AddNotificationRequest
                                {
                                    Description = $"PROGRAM {Program.NamaProgramNavigation.Name} sudah di approve",
                                    IdUser = item.Id,
                                    Inputer = request.Fullname,
                                    IsOpen = false,
                                    Subject = "PROGRAM BARU TELAH DI TAMBAHKAN",
                                    Navigation = "/Penawaran"
                                });
                            }
                        }
                    }
                    else
                    {
                        Program.Status = Program.Status == (short)ProgramStatusEnum.WAITING_VERIFIKASI ? (short)ProgramStatusEnum.REJECT_VERIFIKASI : (short)ProgramStatusEnum.REJECT_APPROVAL;
                    }
                    
                    var save = await _context.UpdateSave(Program);
                    if (save.Success)
                        result.OK();
                    else
                        result.BadRequest(save.Message);
                }
                else
                    result.NotFound("Program tidak ditemukan!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Approve Program", request);
                result.Error("Failed Approve Program", ex.Message);
            }
            return result;
        }
    }
}
