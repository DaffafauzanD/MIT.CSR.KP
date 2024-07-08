using Microsoft.AspNetCore.Mvc;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.Usulan.Query;
using MIT.ECSR.Core.Request;
using MIT.ECSR.Core.Usulan.Command;
using Microsoft.AspNetCore.Authorization;

namespace MIT.ECSR.API.Controllers
{
    public partial class UsulanController : BaseController<UsulanController>
    {
        [AllowAnonymous]
        [HttpGet(template: "get/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Wrapper(await _mediator.Send(new GetUsulanByIdRequest() { Id = id }));
        }

        [AllowAnonymous]
        [HttpPost(template: "list")]
        public async Task<IActionResult> List([FromBody] ListRequest request)
        {
            var list_request = _mapper.Map<GetUsulanListRequest>(request);
            return Wrapper(await _mediator.Send(list_request));
        }

        [HttpPost(template: "add")]
        public async Task<IActionResult> Add([FromBody] UsulanRequest request)
        {
            var add_request = _mapper.Map<AddUsulanRequest>(request);
            add_request.Inputer = Token.User.Username;
            add_request.IdPerusahaan = Token.User.Company.Id;
            return Wrapper(await _mediator.Send(add_request));
        }

        [HttpPut(template: "edit/{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] UsulanRequest request)
        {
            var edit_request = _mapper.Map<EditUsulanRequest>(request);
            edit_request.Id = id;
            edit_request.Inputer = Token.User.Username;
            return Wrapper(await _mediator.Send(edit_request));
        }

        [HttpDelete(template: "delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Wrapper(await _mediator.Send(new DeleteUsulanRequest() { Id = id, Inputer = Token.User.Username }));
        }

        [HttpPost(template: "submit/{id_usulan}")]
        public async Task<IActionResult> Submit(Guid id_usulan)
        {
            return Wrapper(await _mediator.Send(new SubmitUsulanRequest()
            {
                Id = id_usulan,
                Inputer = Token.User.Username
            }));
        }

        [HttpPost(template: "approve")]
        public async Task<IActionResult> Approve([FromBody] ApproveRequest request)
        {
            var add_request = _mapper.Map<ApprovalUsulanRequest>(request);
            add_request.Fullname = Token.User.FullName;
            return Wrapper(await _mediator.Send(add_request));
        }

    }
}

