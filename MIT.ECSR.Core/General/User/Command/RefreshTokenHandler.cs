using AutoMapper;
using MediatR;
using WonderKid.DAL.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Data;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Shared.Interface;
using MIT.ECSR.Core.User.Command;
using Microsoft.EntityFrameworkCore;
using MIT.ECSR.Core.Media.Query;

namespace MIT.ECSR.Core.User.Command
{

    #region Request
    public class RefreshTokenRequest : IRequest<ObjectResponse<TokenObject>>
    {
        [Required]
        public string Token { get; set; }
    }
    #endregion

    internal class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, ObjectResponse<TokenObject>>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IWrapperHelper _wrapper;
        private readonly IGeneralHelper _helper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public RefreshTokenHandler(
            ILogger<RefreshTokenHandler> logger,
            IMediator mediator,
            IWrapperHelper wrapper,
            IGeneralHelper helper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mediator = mediator;
            _wrapper = wrapper;
            _helper = helper;
            _context = context;
        }
        public async Task<ObjectResponse<TokenObject>> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            ObjectResponse<TokenObject> result = new ObjectResponse<TokenObject>();
            try
            {
                var user = await   _context.Entity<Data.Model.SetUser>().Where(d => d.Token == request.Token)
                                      .Include(d=>d.MstPerusahaan).Include(d => d.IdRoleNavigation).FirstOrDefaultAsync();
                                
                if (user != null)
                {
                    user.UpdateDate = DateTime.Now;
                    user.UpdateBy = user.Username;
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
                            Name = x.NamaPerusahaan
                        }).FirstOrDefault() : null,
                        Photos = photo.Succeeded ? photo.List.FirstOrDefault() : null
                    });
                    if (generateToken.Succeeded)
                    {
                        result.Data = generateToken.Data;
                        user.Token = result.Data.RefreshToken;
                    }
                    else
                        result = generateToken;

                }
                else
                    result.NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Refresh Token", request);
                result.Error("Failed Refresh Token", ex.Message);
            }
            return result;
        }
    }
}

