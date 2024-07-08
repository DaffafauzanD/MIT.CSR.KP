using MediatR;
using WonderKid.DAL.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Data;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Shared.Interface;
using Microsoft.Extensions.Options;
using MIT.ECSR.Core.User.Command;
using AutoMapper;
using MIT.ECSR.Data.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Dynamic.Core.Tokenizer;
using MIT.ECSR.Core.Media.Query;

namespace MIT.ECSR.Core.User.Command
{

    #region Request
    public class LoginRequest : IRequest<ObjectResponse<TokenObject>>
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
    #endregion

    internal class LoginUserHandler : IRequestHandler<LoginRequest, ObjectResponse<TokenObject>>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IWrapperHelper _wrapper;
        private readonly IGeneralHelper _helper;
        private readonly ApplicationConfig _config;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        private readonly IMapper _mapper;


        public LoginUserHandler(
            ILogger<LoginUserHandler> logger,
            IMediator mediator,
            IWrapperHelper wrapper,
            IGeneralHelper helper,
            IOptions<ApplicationConfig> config,
            IUnitOfWork<ApplicationDBContext> context,
            IMapper mapper
            )
        {
            _logger = logger;
            _mediator = mediator;
            _wrapper = wrapper;
            _helper = helper;
            _config = config.Value;
            _context = context;
            _mapper = mapper;
        }
        public async Task<ObjectResponse<TokenObject>> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            ObjectResponse<TokenObject> result = new ObjectResponse<TokenObject>();
            try
            {
                string _hash = _helper.PasswordEncrypt(request.Password);
                var user = await _context.Entity<Data.Model.SetUser>()
                            .Where(d => d.Username.ToLower() == request.Username.ToLower() && d.Password.ToLower() == _hash)
                            .Include(d => d.IdRoleNavigation).Include(d => d.MstPerusahaan).FirstOrDefaultAsync();

                if (user != null)
                {
                    if (!user.Active)
                    {
                        result.Forbidden($"User not active please call administrator for verified user!");
                        return result;
                    }
                    var photo = await _mediator.Send(new GetMediaUrlRequest() { Modul = user.Id.ToString(), Tipe = "PHOTO_USER" });
                    var generateToken = await _mediator.Send(new GenerateTokenRequest()
                    {
                        Id = user.Id,
                        Username = user.Username,
                        FullName = user.Fullname,
                        Mail = user.Mail,
                        Role = new RoleObject()
                        {
                            Id = user.IdRole,
                            Name = user.IdRoleNavigation.Name
                        },
                        Company = user.MstPerusahaan != null && user.MstPerusahaan.Count() > 0 ? user.MstPerusahaan.Select(x => new CompanyObject()
                        {
                            Id = x.Id,
                            Name = x.NamaPerusahaan,
                            Nib = x.Nib,
                            JenisPerseroan = x.JenisPerseroan,
                            Npwp = x.Npwp,
                            BidangUsaha = x.BidangUsaha,
                            Alamat = x.Alamat
                        }).FirstOrDefault() : null,
                        Photos = photo.Succeeded ? photo.List.FirstOrDefault() : null
                    });
                    if (generateToken.Succeeded)
                    {
                        result.Data = generateToken.Data;
                        user.Token = result.Data.RefreshToken;
                        result.OK();
                    }
                    else
                        result = generateToken;
                }
                else
                {
                    result.NotFound();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Login User", request);
                result.Error("Failed Add User", ex.Message);
            }
            return result;
        }
    }
}

