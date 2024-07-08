using AutoMapper;
using MediatR;
using WonderKid.DAL.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Data;
using MIT.ECSR.Shared.Attributes;
using Microsoft.Extensions.Configuration;
using MIT.ECSR.Core.Helper;
using MIT.ECSR.Shared.Interface;

namespace MIT.ECSR.Core.Media.Command
{

    #region Request
    public class UploadMediaRequest : IRequest<ObjectResponse<Guid>>
    {
        [Required]
        public string Tipe { get; set; }
        [Required]
        public string Modul { get; set; }
        [Required]
        public FileObject File { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        [Required]
        public string Inputer { get; set; }
    }
    #endregion

    internal class UploadMediaHandler : IRequestHandler<UploadMediaRequest, ObjectResponse<Guid>>
    {
        private readonly ILogger _logger;
        private readonly IGeneralHelper _helper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public UploadMediaHandler(
            ILogger<UploadMediaHandler> logger,
            IGeneralHelper helper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _helper = helper;
            _context = context;
        }
        public async Task<ObjectResponse<Guid>> Handle(UploadMediaRequest request, CancellationToken cancellationToken)
        {
            ObjectResponse<Guid> result = new ObjectResponse<Guid>();
            try
            {
                if (!_helper.IsFile(request.File.Base64))
                {
                    result.BadRequest("File not valid format Base 64!");
                    return result;
                }
                var id = Guid.NewGuid();
                string directory = $"Media/{request.Tipe}";
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                bool is_image = _helper.IsImage(request.File.Filename);
                string original_path = "";
                string resize_path = "";
                if (is_image && request.Width.HasValue && request.Height.HasValue)
                {
                    var save_image = _helper.SaveImage(directory, id, request.File, request.Width.Value, request.Height.Value);
                    if (!save_image.Success)
                    {
                        result.BadRequest("Failed Save Image " + save_image.Message);
                        return result;
                    }
                    original_path = save_image.Original;
                    resize_path = save_image.Resize;
                }
                else
                {
                    original_path = $"{directory}/{id.ToString().ToLowerInvariant()}{Path.GetExtension(request.File.Filename).ToLowerInvariant()}";
                    File.WriteAllBytes(original_path, Convert.FromBase64String(request.File.Base64));
                }

                var add = await _context.AddSave(new Data.Model.TrsMedia()
                {
                    CreateDate = DateTime.Now,
                    Extension = Path.GetExtension(request.File.Filename),
                    Id = id,
                    Tipe = request.Tipe,
                    Modul = request.Modul,
                    FileName = request.File.Filename,
                    CreateBy = request.Inputer,
                    IsImage = is_image,
                    OriginalPath = original_path,
                    ResizePath = resize_path
                });
                result.Data = id;
                if (add.Success)
                    result.OK();
                else
                    result.BadRequest(add.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Upload Media", request);
                result.Error("Failed Upload Media", ex.Message);
            }
            return result;
        }
    }
}

