//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.Perusahaan.Query;
using MIT.ECSR.Core.Request;
using Microsoft.AspNetCore.Authorization;

namespace MIT.ECSR.API.Controllers
{
    [AllowAnonymous]
    public partial class PerusahaanController : BaseController<PerusahaanController>
    {
        [HttpGet(template: "get/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Wrapper(await _mediator.Send(new GetPerusahaanByIdRequest() { Id = id }));
        }

        [HttpPost(template: "list")]
        public async Task<IActionResult> List([FromBody] ListRequest request)
        {
            var list_request = _mapper.Map<GetPerusahaanListRequest>(request);
            return Wrapper(await _mediator.Send(list_request));
        }

        [HttpPost(template: "list_mini")]
        public async Task<IActionResult> ListMini([FromBody] ListRequest request)
        {
            var list_request = _mapper.Map<GetPerusahaanListMiniRequest>(request);
            return Wrapper(await _mediator.Send(list_request));
        }
    }
}

