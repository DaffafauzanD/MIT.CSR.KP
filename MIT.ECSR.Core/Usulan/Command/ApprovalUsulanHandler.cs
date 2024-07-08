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
using SixLabors.ImageSharp.Formats;

namespace MIT.ECSR.Core.Usulan.Command
{

    #region Request
    public class ApprovalUsulanMapping : Profile
    {
        public ApprovalUsulanMapping()
        {
            CreateMap<ApprovalUsulanRequest, ApproveRequest>().ReverseMap();
        }
    }
    public class ApprovalUsulanRequest : ApproveRequest, IRequest<StatusResponse>
    {
        [Required]
        public string Fullname { get; set; }
    }
    #endregion

    internal class ApprovalUsulanHandler : IRequestHandler<ApprovalUsulanRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public ApprovalUsulanHandler(
            ILogger<ApprovalUsulanHandler> logger,
            IMediator mediator,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mediator = mediator;
            _context = context;
        }
        public async Task<StatusResponse> Handle(ApprovalUsulanRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                var item = await _context.Entity<MIT.ECSR.Data.Model.TrsUsulan>().Where(d => d.Id == request.Id).FirstOrDefaultAsync();
                if (item != null)
                {
                    if (item.Status != (int)UsulanStatusEnum.WAITING)
                    {
                        result.BadRequest("Cannot Be Edited because Status is " + ((UsulanStatusEnum)item.Status).ToString());
                        return result;
                    }
                    item.ApprovedAt = DateTime.Now;
                    item.ApprovedBy = request.Fullname;
                    item.Notes = request.Notes;
                    item.Status = request.IsApprove ? (short)UsulanStatusEnum.APPROVE : (short)UsulanStatusEnum.REJECT;
                    var save = await _context.UpdateSave(item);
                    //todo: masukan ke program dan penawaran dengan perusahaan itu dengan status approve
                    if (save.Success)
                    {
                        result.OK();
                    }
                    else
                        result.BadRequest(save.Message);

                    return result;
                }
                else
                    result.NotFound();
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

