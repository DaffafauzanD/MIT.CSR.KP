using Microsoft.AspNetCore.Mvc;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.Notification.Query;
using MIT.ECSR.Core.Request;
using MIT.ECSR.Core.Notification.Command;

namespace MIT.ECSR.API.Controllers
{
    public partial class NotificationController : BaseController<NotificationController>
    {
        [HttpGet(template: "get/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Wrapper(await _mediator.Send(new GetNotificationByIdRequest() { Id = id }));
        }

        [HttpPost(template: "list")]
        public async Task<IActionResult> List([FromBody] ListRequest request)
        {
            var list_request = _mapper.Map<GetNotificationListRequest>(request);
            list_request.IdUser = Token?.User?.Id;
            return Wrapper(await _mediator.Send(list_request));
        }

        [HttpPut(template: "open/{id}")]
        public async Task<IActionResult> Open(Guid id)
        {
            return Wrapper(await _mediator.Send(new OpenNotificatioRequest() { Id = id }));
        }

        [HttpPut(template: "open_all")]
        public async Task<IActionResult> OpenAll()
        {
            return Wrapper(await _mediator.Send(new OpenNotificatioRequest() { Id = default, IdUser  = Token?.User?.Id }));
        }
    }
}

