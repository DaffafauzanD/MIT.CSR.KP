using AutoMapper;
using MediatR;
using WonderKid.DAL.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Data;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.Helper;
using MIT.ECSR.Shared.Interface;
using Microsoft.Extensions.Options;

namespace MIT.ECSR.Core.User.Command
{
    #region Request
    public class ResetPasswordRequest : IRequest<StatusResponse>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Inputer { get; set; }
    }
    #endregion

    internal class ResetPasswordHandler : IRequestHandler<ResetPasswordRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IWrapperHelper _wrapper;
        private readonly IGeneralHelper _helper;
        private readonly ApplicationConfig _config;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public ResetPasswordHandler(
            ILogger<ResetPasswordHandler> logger,
            IWrapperHelper wrapper,
            IGeneralHelper helper,
            IOptions<ApplicationConfig> config,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _wrapper = wrapper;
            _helper = helper;
            _config = config.Value;
            _context = context;
        }
        public async Task<StatusResponse> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                var user = await _context.Single(
                                    _context.Entity<Data.Model.SetUser>()
                                    .Where(d => d.Id == request.Id)
                                );
                string _hash_default_password = _helper.PasswordEncrypt(_config.DefaultPassword) ;
                if (user != null)
                {
                    user.Password = _hash_default_password;
                    user.UpdateDate = DateTime.Now;
                    user.UpdateBy = request.Inputer;
                    result = _wrapper.Response(await _context.UpdateSave(user));
                }
                else
                    result.NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Reset Password", request);
                result.Error("Failed Reset Password", ex.Message);
            }
            return result;
        }
    }
}

