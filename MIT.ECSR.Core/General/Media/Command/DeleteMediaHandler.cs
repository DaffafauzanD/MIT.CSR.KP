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
    public class DeleteMediaRequest : IRequest<StatusResponse>
    {
        [Required]
        public Guid Id { get; set; }
    }
    #endregion

    internal class DeleteMediaHandler : IRequestHandler<DeleteMediaRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public DeleteMediaHandler(
            ILogger<DeleteMediaHandler> logger,
            IMapper mapper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }
        public async Task<StatusResponse> Handle(DeleteMediaRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                var item = await _context.Entity<Data.Model.TrsMedia>().Where(d => d.Id == request.Id).FirstOrDefaultAsync();
                if (item != null)
                {
                    string directory = $"Media/{item.Tipe}";
                    if (File.Exists(item.OriginalPath))
                        File.Delete(item.OriginalPath);
                    if (!string.IsNullOrWhiteSpace(item.ResizePath) && File.Exists(item.ResizePath))
                        File.Delete(item.ResizePath);

                    var delete = await _context.DeleteSave(item);
                    if (delete.Success)
                        result.OK();
                    else
                        result.BadRequest(delete.Message);

                    return result;
                }
                else
                    result.NotFound($"Id Media {request.Id} Tidak Ditemukan");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Delete Media", request.Id);
                result.Error("Failed Delete Media", ex.Message);
            }
            return result;
        }
    }
}

