using MediatR;
using WonderKid.DAL.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Data;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Shared.Interface;

namespace MIT.ECSR.Core.User.Command
{

    #region Request
    public class ChangePasswordRequest : IRequest<StatusResponse>
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
    #endregion

    internal class ChangePasswordHandler : IRequestHandler<ChangePasswordRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IGeneralHelper _helper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public ChangePasswordHandler(
            ILogger<ChangePasswordHandler> logger,
            IGeneralHelper helper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _helper = helper;
            _context = context;
        }
        public async Task<StatusResponse> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                string _old_password = _helper.PasswordEncrypt(request.Password);
                string _new_password = _helper.PasswordEncrypt(request.NewPassword);
                if (!_helper.ValidatePassword(request.NewPassword))
                {
                    result.Confirmation($"Password Must 8 Character Length and Contains Upper Case, Symbol and Numeric!");
                    return result;
                }

                var user = await _context.Entity<Data.Model.SetUser>().Where(d => d.Username.Trim().ToLower() == request.Username.Trim().ToLower())
                                .OrderByDescending(d => d.CreateDate).FirstOrDefaultAsync();
                if (user != null)
                {
                    if(user.Password != _old_password)
                    {
                        result.Confirmation("Your old Password is incorrect!");
                        return result;
                    }
                    user.Password = _new_password;
                    user.UpdateBy = $"{user.Id}|{user.Fullname}";
                    user.UpdateDate = DateTime.Now;
                    var save = await _context.UpdateSave(user);
                    if (save.Success)
                        result.OK();
                    else
                        result.BadRequest(save.Message);

                }
                else
                    result.Confirmation("User Not Found!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Change Password", request);
                result.Error("Failed Change Password", ex.Message);
            }
            return result;
        }
    }
}

