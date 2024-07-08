using AutoMapper;
using MediatR;
using WonderKid.DAL.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MIT.ECSR.Data;
using MIT.ECSR.Shared.Attributes;
using Microsoft.Extensions.Configuration;
using MIT.ECSR.Core.Helper;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.IO;
using DocumentFormat.OpenXml.Bibliography;
using MIT.ECSR.Shared.Interface;

namespace MIT.ECSR.Core.Media.Command
{

    #region Request
    public class EditMediaRequest : IRequest<StatusResponse>
    {
        [Required]
        public string Tipe { get; set; }
        [Required]
        public string Modul { get; set; }
        [Required]
        public FileObject File { get; set; }
        [Required]
        public string Inputer { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
    }
    #endregion

    internal class EditMediaHandler : IRequestHandler<EditMediaRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IGeneralHelper _helper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public EditMediaHandler(
            ILogger<EditMediaHandler> logger,
            IGeneralHelper helper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _helper = helper;
            _context = context;
        }
        public async Task<StatusResponse> Handle(EditMediaRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                if (!_helper.IsFile(request.File.Base64))
                {
                    result.BadRequest("File not valid format Base 64!");
                    return result;
                }
                bool edit = false;
                var item = await _context.Entity<Data.Model.TrsMedia>().Where(d => d.Tipe == request.Tipe && d.Modul == request.Modul).FirstOrDefaultAsync();
                if (item != null)
                {
                    if (File.Exists(item.OriginalPath))
                        File.Delete(item.OriginalPath);
                    if (!string.IsNullOrWhiteSpace(item.ResizePath) && File.Exists(item.ResizePath))
                        File.Delete(item.ResizePath);

                    edit = true;
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
                if (!edit)
                    item = new Data.Model.TrsMedia() { Id = Guid.NewGuid() };

                item.Tipe = request.Tipe;
                item.Modul = request.Modul;
                item.OriginalPath = original_path;
                item.ResizePath = resize_path;
                item.IsImage = is_image;
                item.CreateDate = DateTime.Now;
                item.CreateBy = request.Inputer;
                item.FileName = request.File.Filename;
                item.Extension = Path.GetExtension(request.File.Filename);

                if (edit)
                {
                    var save = await _context.UpdateSave(item);
                    if (save.Success)
                        result.OK();
                    else
                        result.BadRequest(save.Message);
                }
                else
                {
                    var save = await _context.AddSave(item);
                    if (save.Success)
                        result.OK();
                    else
                        result.BadRequest(save.Message);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Update Media", request);
                result.Error("Failed Delete Media", ex.Message);
            }
            return result;
        }
    }
}

