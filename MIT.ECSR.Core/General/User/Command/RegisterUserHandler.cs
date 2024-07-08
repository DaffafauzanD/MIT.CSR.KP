using AutoMapper;
using MediatR;
using WonderKid.DAL.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Data;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.Helper;
using MIT.ECSR.Shared.Interface;
using Microsoft.Extensions.Options;
using MIT.ECSR.Core.Request;
using MIT.ECSR.Data.Model;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Spreadsheet;
using MIT.ECSR.Core.Media.Command;

namespace MIT.ECSR.Core.User.Command
{

    #region Request
    public class RegisterUserMapping : Profile
    {
        public RegisterUserMapping()
        {
            CreateMap<RegisterUserRequest, RegisterRequest>().ReverseMap();
        }
    }
    public class RegisterUserRequest : RegisterRequest, IMapRequest<Data.Model.SetUser, RegisterUserRequest>, IRequest<StatusResponse>
    {
        [Required]
        public string Inputer { get; set; }
        public void Mapping(IMappingExpression<RegisterUserRequest, Data.Model.SetUser> map)
        {
        }
    }
    #endregion

    internal class RegisterUserHandler : IRequestHandler<RegisterUserRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IGeneralHelper _helper;
        private readonly ApplicationConfig _config;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public RegisterUserHandler(
            ILogger<RegisterUserHandler> logger,
            IGeneralHelper helper,
            IOptions<ApplicationConfig> config,
            IMediator mediator,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _helper = helper;
            _mediator = mediator;
            _config = config.Value;
            _context = context;
        }
        public async Task<StatusResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                if (!_context.Entity<Data.Model.SetUser>().Any(d => d.Username.ToLower() == request.Username.ToLower()))
                {
                    string _hash_default_password = _helper.PasswordEncrypt(request.Password);
                    var role = await _context.Entity<SetRole>().Where(d => d.Id == request.IdRole).FirstOrDefaultAsync();
                    if (role == null)
                    {
                        result.UnAuthorized("Role Not Found!");
                        return result;
                    }
                    if (role.Name.ToUpper().Contains(RoleName.FORUM.ToString()) && request.Perusahaan == null)
                    {
                        result.UnAuthorized("Perusahaan Harus Diisi!");
                        return result;
                    }
                    SetUser data = new SetUser()
                    {
                        Fullname = request.Fullname,
                        Id = Guid.NewGuid(),
                        IdRole = request.IdRole,
                        Mail = request.Mail,
                        PhoneNumber = request.PhoneNumber,
                        Password = _hash_default_password,
                        Username = request.Username,
                        Active = true,
                        CreateBy = request.Inputer,
                        CreateDate = DateTime.Now,
                        UpdateBy = request.Inputer,
                        UpdateDate = DateTime.Now
                    };
                    _context.Add(data);
                    if (role.Name.ToUpper().Contains(RoleName.FORUM.ToString()))
                    {
                        _context.Add(new MstPerusahaan()
                        {
                            JenisPerseroan = request.Perusahaan.JenisPerseroan,
                            Nib = request.Perusahaan.Nib,
                            Npwp = request.Perusahaan.Npwp,
                            Alamat = request.Perusahaan.Alamat,
                            BidangUsaha = request.Perusahaan.BidangUsaha,
                            Id = Guid.NewGuid(),
                            IdUser = data.Id,
                            NamaPerusahaan = request.Perusahaan.NamaPerusahaan,
                            CreateBy = request.Inputer,
                            CreateDate = DateTime.Now,
                            UpdateBy = request.Inputer,
                            UpdateDate = DateTime.Now
                        });
                    }
                    var save = await _context.Commit();
                    if (save.Success)
                    {
                        result.OK();
                        if (request.Photo != null)
                        {
                            if (_helper.IsImage(request.Photo.Filename))
                            {
                                result = await _mediator.Send(new UploadMediaRequest()
                                {
                                    File = request.Photo,
                                    Inputer = request.Inputer,
                                    Modul = data.Id.ToString(),
                                    Tipe = "PHOTO_USER",
                                    Height = 150,
                                    Width = 150
                                });
                            }
                        }
                    }
                    else
                        result.BadRequest(save.Message);
                }
                else
                    result.UnAuthorized("Username Sudah Terdaftar!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Register User", request);
                result.Error("Failed Register User", ex.Message);
            }
            return result;
        }
    }
}

