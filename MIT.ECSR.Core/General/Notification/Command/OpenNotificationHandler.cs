using AutoMapper;
using MediatR;
using WonderKid.DAL.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Data;
using MIT.ECSR.Shared.Attributes;

namespace MIT.ECSR.Core.Notification.Command
{

    #region Request
    public class OpenNotificatioRequest : IRequest<StatusResponse>
    {
        [Required]
        public Guid Id { get; set; }
        public Guid? IdUser { get; set; }
    }
    #endregion

    internal class OpenNotificationHandler : IRequestHandler<OpenNotificatioRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public OpenNotificationHandler(
            ILogger<OpenNotificationHandler> logger,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _context = context;
        }
        public async Task<StatusResponse> Handle(OpenNotificatioRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                if (request.Id != default)
                {
                    var item = await _context.Entity<Data.Model.TrsNotification>().Where(d => d.Id == request.Id).FirstOrDefaultAsync();
                    if (item != null)
                    {
                        item.IsOpen = true;
                        var update = await _context.UpdateSave(item);
                        if (update.Success)
                            result.OK();
                        else
                            result.BadRequest(update.Message);

                        return result;
                    }
                    else
                        result.NotFound($"Id Notification {request.Id} Tidak Ditemukan");
                }
                else
                {
                    var item = await _context.Entity<Data.Model.TrsNotification>().Where(d => d.IdUser == request.IdUser).ToListAsync();
                    foreach (var notif in item)
                    {
                        notif.IsOpen = true;
                        _context.Update(notif);
                    }
                    var update = await _context.Commit();
                    if (update.Success)
                        result.OK();
                    else
                        result.BadRequest(update.Message);

                    return result;
                }
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Edit Notification", request);
                result.Error("Failed Edit Notification", ex.Message);
            }
            return result;
        }
    }
}

