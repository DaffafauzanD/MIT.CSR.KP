//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using AutoMapper;
using MediatR;
using WonderKid.DAL.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MIT.ECSR.Data;
using MIT.ECSR.Shared.Attributes;

namespace MIT.ECSR.Core.Notification.Command
{

    #region Request
    public class DeleteNotificationRequest : IRequest<StatusResponse>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Inputer { get; set; }
    }
    #endregion

    internal class DeleteNotificationHandler : IRequestHandler<DeleteNotificationRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public DeleteNotificationHandler(
            ILogger<DeleteNotificationHandler> logger,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _context = context;
        }
        public async Task<StatusResponse> Handle(DeleteNotificationRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                var item = await _context.Entity<MIT.ECSR.Data.Model.TrsNotification>().Where(d => d.Id == request.Id).FirstOrDefaultAsync();
                if (item != null)
                {
                    var delete = await _context.DeleteSave(item);
                    if (delete.Success)
                        result.OK();
                    else
                        result.BadRequest(delete.Message);
                    return result;
                }
                else
                    result.NotFound($"Id Notification {request.Id} Tidak Ditemukan");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Delete Notification", request.Id);
                result.Error("Failed Delete Notification", ex.Message);
            }
            return result;
        }
    }
}

