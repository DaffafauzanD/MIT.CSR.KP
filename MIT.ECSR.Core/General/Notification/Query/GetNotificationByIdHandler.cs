//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using AutoMapper;
using MediatR;
using WonderKid.DAL.Interface;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Data;
using MIT.ECSR.Data.Model;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.Response;

namespace MIT.ECSR.Core.Notification.Query
{

    public class GetNotificationByIdRequest : IRequest<ObjectResponse<NotificationResponse>>
    {
        [Required]
        public Guid Id { get; set; }
    }
    internal class GetNotificationByIdHandler : IRequestHandler<GetNotificationByIdRequest, ObjectResponse<NotificationResponse>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public GetNotificationByIdHandler(
            ILogger<GetNotificationByIdHandler> logger,
            IMapper mapper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }
        public async Task<ObjectResponse<NotificationResponse>> Handle(GetNotificationByIdRequest request, CancellationToken cancellationToken)
        {
            ObjectResponse<NotificationResponse> result = new ObjectResponse<NotificationResponse>();
            try
            {
                var item = await _context.Entity<MIT.ECSR.Data.Model.TrsNotification>().Where(d => d.Id == request.Id).FirstOrDefaultAsync();
                if (item != null)
                {
                    result.Data = _mapper.Map<NotificationResponse>(item);
                    result.OK();
                }
                else
                    result.NotFound($"Id Notification {request.Id} Tidak Ditemukan");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Get Detail Notification", request.Id);
                result.Error("Failed Get Detail Notification", ex.Message);
            }
            return result;
        }
    }
}

