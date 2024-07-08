using Microsoft.AspNetCore.Mvc;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.User.Query;
using MIT.ECSR.Core.User.Command;
using Microsoft.AspNetCore.Authorization;
using MIT.ECSR.Core.Request;
using MIT.ECSR.Core.Role.Query;
using MIT.ECSR.Core.General.User.Command;

namespace MIT.ECSR.API.Controllers
{
    public partial class UserController : BaseController<UserController>
    {
        [AllowAnonymous]
        [HttpPost(template: "login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            return Wrapper(await _mediator.Send(request));
        }

        [HttpGet(template: "get/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Wrapper(await _mediator.Send(new GetUserByIdRequest() { Id = id }));
        }

        [HttpPost(template: "list")]
        public async Task<IActionResult> List([FromBody] ListRequest request)
        {
            var list_request = _mapper.Map<GetUserListRequest>(request);
            return Wrapper(await _mediator.Send(list_request));
        }
        [HttpPost(template: "role")]
        public async Task<IActionResult> RoleList([FromBody] ListRequest request)
        {
            var list_request = _mapper.Map<GetRoleListRequest>(request);
            return Wrapper(await _mediator.Send(list_request));
        }
        [HttpPost(template: "register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var regist = _mapper.Map<RegisterUserRequest>(request);
            regist.Inputer = Token.User.Username;
            return Wrapper(await _mediator.Send(regist));
        }

        [HttpPost(template: "logoff")]
        public async Task<IActionResult> Logoff()
        {
            return Wrapper(await _mediator.Send(new LogoffRequest() { Token = Token.RefreshToken }));
        }


        [HttpPut(template: "edit/{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] UserRequest request)
        {
            var edit = _mapper.Map<EditUserRequest>(request);
            edit.Id = id;
            edit.Inputer = Token.User.Username;
            return Wrapper(await _mediator.Send(edit));
        }
        [HttpPut(template: "active/{id}/{value}")]
        public async Task<IActionResult> Active(Guid id, bool value)
        {
            return Wrapper(await _mediator.Send(new ActiveUserRequest() { Id = id, Active = value, Inputer = Token.User.Username }));
        }

        [HttpPost(template: "change_password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            return Wrapper(await _mediator.Send(request));
        }

        [HttpPost(template: "refresh_token")]
        public async Task<IActionResult> RefreshToken()
        {
            return Wrapper(await _mediator.Send(new RefreshTokenRequest() { Token = Token.RefreshToken }));
        }

        [HttpPut(template: "profile")]
        public async Task<IActionResult> Profile([FromBody] UserProfileRequest request)
        {
            request.Id = Token.User.Id;
            return Wrapper(await _mediator.Send(request));
        }
    }
}

