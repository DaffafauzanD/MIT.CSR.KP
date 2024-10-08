using MediatR;
using WonderKid.DAL.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Data;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Shared.Interface;

namespace MIT.ECSR.Core.User.Command
{

    #region Request
    public class LogoffRequest : IRequest<StatusResponse>
    {
        [Required]
        public string Token { get; set; }
    }
    #endregion

    internal class LogoffHandler : IRequestHandler<LogoffRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IWrapperHelper _wrapper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public LogoffHandler(
            ILogger<LogoffHandler> logger,
            IWrapperHelper wrapper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _wrapper = wrapper;
            _context = context;
        }
        public async Task<StatusResponse> Handle(LogoffRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                var user = await _context.Single(
                                    _context.Entity<Data.Model.SetUser>()
                                    .Where(d => d.Token == request.Token)
                                );
                if (user != null)
                {
                    user.Token = null;
                    user.UpdateDate = DateTime.Now;
                    user.UpdateBy = user.Username;
                    result = _wrapper.Response(await _context.UpdateSave(user));
                }
                else
                    result.NotFound();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Logoff", request);
                result.Error("Failed Logoff", ex.Message);
            }
            return result;
        }
    }
}

