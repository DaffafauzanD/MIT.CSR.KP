using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Core.Request;
using MIT.ECSR.Data;
using MIT.ECSR.Data.Model;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Shared.Interface;
using WonderKid.DAL.Interface;

namespace MIT.ECSR.Core.General.User.Command
{
    #region Request
    public class UserProfileRequest : IRequest<StatusResponse>
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public PerusahaanRequest Perusahaan { get; set; }
    }
    #endregion
    internal class UserProfileHandler : IRequestHandler<UserProfileRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        private readonly IGeneralHelper _helper;

        public UserProfileHandler(ILogger<UserProfileHandler> logger, IUnitOfWork<ApplicationDBContext> context, IGeneralHelper helper)
        {
            _logger = logger;
            _context = context;
            _helper = helper;
        }

        public async Task<StatusResponse> Handle(UserProfileRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                var user = await _context.Entity<SetUser>().FirstOrDefaultAsync(x => x.Id == request.Id);
                if (user != null)
                {
                    user.Username = request.Username;
                    user.Fullname = request.Fullname;
                    user.Mail = request.Mail;
                    user.PhoneNumber = request.PhoneNumber;
                    user.UpdateDate = DateTime.Now;

                    if (!string.IsNullOrEmpty(request.Password))
                    {
                        user.Password = _helper.PasswordEncrypt(request.Password);
                    }
                    _context.Update(user);

                    var company = await _context.Entity<MstPerusahaan>().FirstOrDefaultAsync(x => x.IdUser == user.Id);
                    if (company != null)
                    {
                        company.NamaPerusahaan = request.Perusahaan.NamaPerusahaan;
                        company.Nib = request.Perusahaan.Nib;
                        company.JenisPerseroan = request.Perusahaan.JenisPerseroan;
                        company.Npwp = request.Perusahaan.Npwp;
                        company.Alamat = request.Perusahaan.Alamat;
                        company.BidangUsaha = request.Perusahaan.BidangUsaha;
                        company.UpdateDate = DateTime.Now;
                        _context.Update(company);
                    }

                    var commit = await _context.Commit();
                    if (commit.Success)
                        result.OK();
                }
                else
                {
                    result.NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Update Profile", request);
                result.Error("Failed Update Profile", ex.Message);
            }

            return result;
        }
    }
}
