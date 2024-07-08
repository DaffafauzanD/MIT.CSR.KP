using Microsoft.AspNetCore.Mvc;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.Program.Query;
using MIT.ECSR.Core.Request;
using MIT.ECSR.Core.Program.Command;
using Microsoft.AspNetCore.Authorization;

namespace MIT.ECSR.API.Controllers
{
    public partial class ProgramController : BaseController<ProgramController>
    {
        [AllowAnonymous]
        [HttpGet(template: "get/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Wrapper(await _mediator.Send(new GetProgramByIdRequest() { Id = id }));
        }

        [AllowAnonymous]
        [HttpPost(template: "list")]
        public async Task<IActionResult> List([FromBody] ListRequest request)
        {
            var list_request = _mapper.Map<GetProgramListRequest>(request);
            return Wrapper(await _mediator.Send(list_request));
        }

        [HttpPost(template: "add")]
        public async Task<IActionResult> Add([FromBody] ProgramRequest request)
        {
            var add_request = _mapper.Map<AddProgramRequest>(request);
            add_request.Inputer = Token.User.Username;
            return Wrapper(await _mediator.Send(add_request));
        }

        [HttpPut(template: "edit/{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] ProgramRequest request)
        {
            var edit_request = _mapper.Map<EditProgramRequest>(request);
            edit_request.Id = id;
            edit_request.Inputer = Token.User.Username;
            return Wrapper(await _mediator.Send(edit_request));
        }

        [HttpDelete(template: "delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Wrapper(await _mediator.Send(new DeleteProgramRequest() { Id = id, Inputer = Token.User.Username }));
        }

        [HttpPost(template: "submit/{id_program}")]
        public async Task<IActionResult> Submit(Guid id_program)
        {
            return Wrapper(await _mediator.Send(new SubmitProgramRequest()
            {
                Id = id_program,
                Inputer = Token.User.Username,
                RoleName = Token.User.Role?.Name
            }));
        }

        [HttpPost(template: "start/{id_program}")]
        public async Task<IActionResult> Start(Guid id_program)
        {
            return Wrapper(await _mediator.Send(new StartProgramRequest()
            {
                Id = id_program,
                Inputer = Token.User.Username
            }));
        }

        [HttpPost(template: "approve")]
        public async Task<IActionResult> Approve([FromBody] ApproveRequest request)
        {
            var add_request = _mapper.Map<ApprovalProgramRequest>(request);
            add_request.Fullname = Token.User.FullName;
            return Wrapper(await _mediator.Send(add_request));
        }

        [HttpPost(template: "export")]
        public async Task<IActionResult> Export([FromBody] ListRequest request)
        {
            var list_request = _mapper.Map<ExportProgramRequest>(request);
            return Wrapper(await _mediator.Send(list_request));
        }

        [HttpGet(template: "dashboard")]
        public async Task<IActionResult> Dashboard(int year)
        {
            return Wrapper(await _mediator.Send(new GetProgramDashboardRequest { 
                 Year = year
            }));
        }

        [HttpGet(template: "list-year-dashboard")]
        public async Task<IActionResult> ListYearDashboard()
        {
            return Wrapper(await _mediator.Send(new GetProgramYearDashboardRequest
            {
            }));
        }

    }
}

