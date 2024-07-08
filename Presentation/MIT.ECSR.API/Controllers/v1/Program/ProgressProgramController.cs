using Microsoft.AspNetCore.Mvc;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.ProgresProgram.Query;
using MIT.ECSR.Core.Request;
using MIT.ECSR.Core.ProgresProgram.Command;
using Microsoft.AspNetCore.Authorization;

namespace MIT.ECSR.API.Controllers
{
    public partial class ProgressProgramController : BaseController<ProgressProgramController>
    {
        [AllowAnonymous]
        [HttpGet(template: "get/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Wrapper(await _mediator.Send(new GetProgresProgramByIdRequest() { Id = id }));
        }

        [AllowAnonymous]
        [HttpPost(template: "list")]
        public async Task<IActionResult> List([FromBody] ListRequest request)
        {
            var list_request = _mapper.Map<GetProgresProgramListRequest>(request);
            return Wrapper(await _mediator.Send(list_request));
        }
        [HttpGet(template: "list_external")]
        public async Task<IActionResult> List(int start, int length)
        {
            return Wrapper(await _mediator.Send(new GetProgresProgramListExternalRequest()
            {
                IdPerusahaan = Token.User.Company.Id,
                Length = length,
                Start = start
            }));
        }

        [HttpGet(template: "list_history_external")]
        public async Task<IActionResult> List(int start, int length, Guid idProgram)
        {
            return Wrapper(await _mediator.Send(new GetHistoryProgresProgramListExternalRequest()
            {
                IdPerusahaan = Token.User.Company.Id,
                Length = length,
                Start = start,
                IdProgram = idProgram
            }));
        }

        [AllowAnonymous]
        [HttpGet(template: "list_history_public_external")]
        public async Task<IActionResult> ListHistoryPublic(int start, int length, Guid idProgram)
        {
            return Wrapper(await _mediator.Send(new GetPublicHistoryProgresProgramListExternalRequest()
            {
                Length = length,
                Start = start,
                IdProgram = idProgram
            }));
        }

        [HttpPost(template: "add")]
        public async Task<IActionResult> Add([FromBody] ProgresProgramRequest request)
        {
            if (Token.User.Company == null)
            {
                StatusResponse result = new StatusResponse();
                result.BadRequest("Harus pelaku usaha yang melakukan Add!");
                return Wrapper(result);
            }
            var add_request = _mapper.Map<AddProgresProgramRequest>(request);
            add_request.Inputer = Token.User.Username;
            add_request.IdPerusahaan = Token.User.Company.Id;
            return Wrapper(await _mediator.Send(add_request));
        }

        [HttpPost(template: "approve")]
        public async Task<IActionResult> Approve([FromBody] ApproveRequest request)
        {
            var add_request = _mapper.Map<ApprovalProgresProgramRequest>(request);
            add_request.Fullname = Token.User.FullName;
            return Wrapper(await _mediator.Send(add_request));
        }

        [HttpPost(template: "submit")]
        public async Task<IActionResult> Submit([FromBody] ApproveRequest request)
        {
            var add_request = _mapper.Map<SubmitProgresProgramRequest>(request);
            add_request.Fullname = Token.User.FullName;
            return Wrapper(await _mediator.Send(add_request));
        }

    }
}

