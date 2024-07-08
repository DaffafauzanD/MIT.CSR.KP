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

namespace MIT.ECSR.Core.Media.Command
{

    #region Request
    public class DeleteMediaByModulRequest : IRequest<StatusResponse>
    {
        [Required]
        public string Modul { get; set; }
        [Required]
        public string Tipe { get; set; }
    }
    #endregion

    internal class DeleteMediaByModulHandler : IRequestHandler<DeleteMediaByModulRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public DeleteMediaByModulHandler(
            ILogger<DeleteMediaByModulHandler> logger,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _context = context;
        }
        public async Task<StatusResponse> Handle(DeleteMediaByModulRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                var item = await _context.Entity<Data.Model.TrsMedia>().Where(d => d.Modul == request.Modul && d.Tipe == request.Tipe).ToListAsync();
                if (item != null)
                {
                    foreach(var d in item)
                    {
                        string directory = $"Media/{d.Tipe}";
                        if (File.Exists(d.OriginalPath))
                            File.Delete(d.OriginalPath);
                        if (!string.IsNullOrWhiteSpace(d.ResizePath) && File.Exists(d.ResizePath))
                            File.Delete(d.ResizePath);
                    }

                    var delete = await _context.DeleteSave(item);
                    if (delete.Success)
                        result.OK();
                    else
                        result.BadRequest(delete.Message);

                    return result;
                }
                else
                    result.NotFound($"Id Media {request.Modul} Tidak Ditemukan");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Delete Media", request.Modul);
                result.Error("Failed Delete Media", ex.Message);
            }
            return result;
        }
    }
}

