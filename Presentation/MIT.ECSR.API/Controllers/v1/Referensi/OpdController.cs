//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.Opd.Query;
using MIT.ECSR.Core.Request;
using MIT.ECSR.Core.Opd.Command;

namespace MIT.ECSR.API.Controllers
{
    public partial class OpdController : BaseController<OpdController>
    {
        [HttpGet(template: "get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Wrapper(await _mediator.Send(new GetOpdByIdRequest() { Id = id }));
        }

        [HttpPost(template: "list")]
        public async Task<IActionResult> List([FromBody] ListRequest request)
        {
            var list_request = _mapper.Map<GetOpdListRequest>(request);
            return Wrapper(await _mediator.Send(list_request));
        }

        [HttpPost(template: "add")]
        public async Task<IActionResult> Add([FromBody] OpdRequest request)
        {
            var add_request = _mapper.Map<AddOpdRequest>(request);
            add_request.Inputer = Token.User.Username;
            return Wrapper(await _mediator.Send(add_request));
        }

        [HttpPut(template: "edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] OpdRequest request)
        {
            var edit_request = _mapper.Map<EditOpdRequest>(request);
            edit_request.Id = id;
            edit_request.Inputer = Token.User.Username;
            return Wrapper(await _mediator.Send(edit_request));
        }

        [HttpDelete(template: "delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Wrapper(await _mediator.Send(new DeleteOpdRequest() { Id = id, Inputer = Token.User.Username }));
        }

        
        [HttpPut(template: "active/{id}/{value}")]
        public async Task<IActionResult> Active(int id, bool value)
        {
            return Wrapper(await _mediator.Send(new ActiveOpdRequest() { Id = id, Active = value, Inputer = Token.User.Username }));
        }
        
    }
}

