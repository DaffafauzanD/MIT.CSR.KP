using AutoMapper;
using MediatR;
using WonderKid.DAL.Interface;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Data;
using MIT.ECSR.Shared.Attributes;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;

namespace MIT.ECSR.Core.User.Command
{

    #region Request
    public class GenerateTokenMapping : Profile
    {
        public GenerateTokenMapping()
        {
            CreateMap<GenerateTokenRequest, TokenUserObject>().ReverseMap();
        }
    }
    internal class GenerateTokenRequest : TokenUserObject, IRequest<ObjectResponse<TokenObject>>
    {
    }
    #endregion

    internal class GenerateTokenHandler : IRequestHandler<GenerateTokenRequest, ObjectResponse<TokenObject>>
    {
        private readonly ILogger _logger;
        private readonly ApplicationConfig _config;
        public GenerateTokenHandler(
            ILogger<GenerateTokenHandler> logger,
            IOptions<ApplicationConfig> config
            )
        {
            _logger = logger;
            _config = config.Value;
        }
        public Task<ObjectResponse<TokenObject>> Handle(GenerateTokenRequest request, CancellationToken cancellationToken)
        {
            ObjectResponse <TokenObject> result = new ObjectResponse<TokenObject>();
            try
            {
                var refresh_token = Convert.ToBase64String(Encoding.Unicode.GetBytes(Guid.NewGuid().ToString()));
                var token_handler = new JwtSecurityTokenHandler();
                var claims = new List<Claim> {
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Sub, request.Id.ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, request.Username),
                            new Claim(JwtRegisteredClaimNames.GivenName, request.FullName),
                            new Claim(JwtRegisteredClaimNames.Email, request.Mail),
                            new Claim(ClaimTypes.Role, $"{(int)request.Role.Id}-{request.Role.Name}"),
                            new Claim("token" , refresh_token)
                            };

                if (request.Company != null)
                {
                    claims.Add(new Claim("company_id", request.Company.Id.ToString()));
                    claims.Add(new Claim("company_name", request.Company.Name ?? string.Empty));
                    claims.Add(new Claim("company_nib", request.Company.Nib ?? string.Empty));
                    claims.Add(new Claim("company_jenis", request.Company.JenisPerseroan ?? string.Empty));
                    claims.Add(new Claim("company_npwp", request.Company.Npwp ?? string.Empty));
                    claims.Add(new Claim("company_bidang", request.Company.BidangUsaha ?? string.Empty));
                    claims.Add(new Claim("company_alamat", request.Company.Alamat ?? string.Empty));
                }
                if (request.Photos != null)
                {
                    claims.Add(new Claim("img_ori",request.Photos.Original));
                    claims.Add(new Claim("img_sm", request.Photos.Resize));
                }
                var key = Encoding.ASCII.GetBytes(_config.SecretKey);
                DateTime expired = DateTime.Now.AddMinutes(_config.TokenExpired);
                var token = new JwtSecurityToken(issuer: _config.Issuer,
                                audience: _config.Audience,
                                claims: claims,
                                expires: expired,
                                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                            );

                result.Data = new TokenObject()
                {
                    ExpiredAt = expired,
                    RefreshToken = refresh_token,
                    RawToken = new JwtSecurityTokenHandler().WriteToken(token),
                    User = request
                };
                result.OK();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Generate Token", request);
                result.Error("Failed Generate Token", ex.Message);
            }
            return Task.FromResult(result);
        }
    }
}

