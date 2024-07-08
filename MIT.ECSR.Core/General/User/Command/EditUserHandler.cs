using AutoMapper;
using MediatR;
using WonderKid.DAL.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Data;
using MIT.ECSR.Data.Model;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.Helper;
using MIT.ECSR.Core.Request;
using MIT.ECSR.Core.Media.Command;
using MIT.ECSR.Shared.Interface;

namespace MIT.ECSR.Core.User.Command
{

    #region Request
    public class EditUserMapping : Profile
    {
        public EditUserMapping()
        {
            CreateMap<EditUserRequest, UserRequest>().ReverseMap();
        }
    }
    public class EditUserRequest : UserRequest, IMapRequest<SetUser, EditUserRequest>, IRequest<StatusResponse>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Inputer { get; set; }
        public void Mapping(IMappingExpression<EditUserRequest, SetUser> map)
        {
        }
    }
    #endregion

    internal class EditUserHandler : IRequestHandler<EditUserRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IGeneralHelper _helper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public EditUserHandler(
            ILogger<EditUserHandler> logger,
            IMediator mediator,
            IGeneralHelper helper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mediator = mediator;
            _helper = helper;
            _context = context;
        }
        public async Task<StatusResponse> Handle(EditUserRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                var user = await _context.Entity<SetUser>().Where(d => d.Id == request.Id).Include(d => d.IdRoleNavigation).FirstOrDefaultAsync();
                if (user != null)
                {
                    if (user.Username.ToLower() != request.Username.ToLower())
                    {
                        if (await _context.Entity<SetUser>().AnyAsync(d => d.Username.ToLower() == request.Username.ToLower()))
                        {
                            result.BadRequest("Username sudah ada!");
                            return result;
                        }
                    }
                    var role = await _context.Entity<SetRole>().Where(d => d.Id == request.IdRole).FirstOrDefaultAsync();
                    if (role == null)
                    {
                        result.BadRequest("Role Not Found!");
                        return result;
                    }

                    user.Username = request.Username;
                    user.Fullname = request.Fullname;
                    user.IdRole = request.IdRole;
                    user.Mail = request.Mail;
                    user.PhoneNumber = request.PhoneNumber;
                    user.UpdateBy = request.Inputer;
                    user.UpdateDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(request.Password))
                        user.Password = _helper.PasswordEncrypt(request.Password);

                    _context.Update(user);

                    var company = await _context.Entity<MstPerusahaan>().Where(d => d.IdUser == request.Id).FirstOrDefaultAsync();
                    if (role.Name.ToUpper().Contains(RoleName.FORUM.ToString()))
                    {
                        if (company == null && request.Perusahaan == null)
                        {
                            result.BadRequest("Perusahaan harus diisi!");
                            return result;
                        }
                        if (company != null && request.Perusahaan != null)
                        {
                            company.NamaPerusahaan = request.Perusahaan.NamaPerusahaan;
                            company.Alamat = request.Perusahaan.Alamat;
                            company.BidangUsaha = request.Perusahaan.BidangUsaha;
                            company.UpdateBy = user.Username;
                            company.UpdateDate = DateTime.Now;
                            company.JenisPerseroan = request.Perusahaan.JenisPerseroan;
                            company.Nib = request.Perusahaan.Nib;
                            company.Npwp = request.Perusahaan.Npwp;

                            _context.Update(company);
                        }
                        else if (company == null && request.Perusahaan != null)
                        {
                            _context.Add(new MstPerusahaan()
                            {
                                JenisPerseroan = request.Perusahaan.JenisPerseroan,
                                Nib = request.Perusahaan.Nib,
                                Npwp = request.Perusahaan.Npwp,
                                Alamat = request.Perusahaan.Alamat,
                                BidangUsaha = request.Perusahaan.BidangUsaha,
                                CreateBy = user.Username,
                                CreateDate = DateTime.Now,
                                Id = Guid.NewGuid(),
                                IdUser = user.Id,
                                NamaPerusahaan = request.Perusahaan.NamaPerusahaan,
                                UpdateBy = user.Username,
                                UpdateDate = DateTime.Now,
                            });
                        }
                    }
                    else
                    {
                        if (company != null)
                            _context.Delete(company);
                    }
                    var save = await _context.Commit();
                    if (save.Success)
                    {
                        result.OK();
                        if (request.Photo != null)
                        {
                            if (_helper.IsImage(request.Photo.Filename))
                            {
                                result = await _mediator.Send(new EditMediaRequest()
                                {
                                    File = request.Photo,
                                    Inputer = user.Username,
                                    Modul = user.Id.ToString(),
                                    Tipe = "PHOTO_USER",
                                    Height = 150,
                                    Width = 150
                                });
                            }
                        }
                    }
                    else
                        result.BadRequest(save.Message);

                    return result;
                }
                else
                    result.NotFound($"Id User {request.Id} Tidak Ditemukan");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Edit User", request);
                result.Error("Failed Edit User", ex.Message);
            }
            return result;
        }
    }
}

