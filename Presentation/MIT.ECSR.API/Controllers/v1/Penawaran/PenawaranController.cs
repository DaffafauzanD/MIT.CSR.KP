//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.Penawaran.Query;
using MIT.ECSR.Core.Request;
using MIT.ECSR.Core.Program.Query;
using MIT.ECSR.Core.Penawaran.Command;
using MIT.ECSR.Core.Penawaran.Object;
using Microsoft.AspNetCore.Authorization;
//using MIT.ECSR.Core.Penawaran.Command;

namespace MIT.ECSR.API.Controllers
{
    public partial class PenawaranController : BaseController<PenawaranController>
    {
        [HttpGet(template: "list_program")]
        public async Task<IActionResult> List(string search, int? id_jenis_program, int? start, int? length, int? id_kegiatan, string? tgl_pelaksanaan)
        {
            Guid? id_perusahaan = Token != null && Token.User.Company != null ? Token.User.Company.Id : null;
            return Wrapper(await _mediator.Send(new GetPenawaranListRequest()
            {
                IdJenisProgram = id_jenis_program,
                Length = length,
                Search = search,
                Start = start,
                IdPerusahaan = id_perusahaan,
                IdKegiatan = id_kegiatan,
                TglPelaksanaan = tgl_pelaksanaan
            }));
        }

        [AllowAnonymous]
        [HttpGet(template: "list_internal/{id_program}")]
        public async Task<IActionResult> ListInternal(Guid id_program, string search, int? start, int? length)
        {
            return Wrapper(await _mediator.Send(new GetPenawaranListInternalRequest()
            {
                IdProgram = id_program,
                Length = length,
                Search = search,
                Start = start
            }));
        }
        [HttpGet(template: "get/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Wrapper(await _mediator.Send(new GetPenawaranByIdRequest() { Id = id }));
        }
        [HttpPost(template: "submit")]
        public async Task<IActionResult> Submit([FromBody] PenawaranRequest request)
        {
            if (Token.User.Company == null)
            {
                StatusResponse result = new StatusResponse();
                result.BadRequest("Harus pelaku usaha yang melakukan Submit!");
                return Wrapper(result);
            }
            var submit_request = _mapper.Map<PengajuanPenawaranRequest>(request);
            submit_request.Inputer = Token.User.Username;
            submit_request.IdPerusahaan = Token.User.Company.Id;
            return Wrapper(await _mediator.Send(submit_request));
        }
        [HttpPost(template: "list_penawaran")]
        public async Task<IActionResult> List([FromBody] PenawaranListRequest request)
        {
            Guid? id_perusahaan = Token != null && Token.User.Company != null ? Token.User.Company.Id : null;
            var list_request = _mapper.Map<GetPenawaranListLogRequest>(request);
            list_request.IdPerusahaan = id_perusahaan;
            return Wrapper(await _mediator.Send(list_request));
        }

        [HttpPost(template: "approve")]
        public async Task<IActionResult> Approve([FromBody] ApproveRequest request)
        {
            var add_request = _mapper.Map<ApprovalPenawaranRequest>(request);
            add_request.Fullname = Token.User.FullName;
            return Wrapper(await _mediator.Send(add_request));
        }

        [HttpDelete(template: "delete/{id_penawaran_item}")]
        public async Task<IActionResult> Delete(Guid id_penawaran_item)
        {
            return Wrapper(await _mediator.Send(new DeletePenawaranRequest() { Id = id_penawaran_item, Inputer = Token.User.Username }));
        }
    }
}

