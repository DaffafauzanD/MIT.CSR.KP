using AutoMapper;
using MediatR;
using WonderKid.DAL.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Data;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.Helper;
using MIT.ECSR.Core.Request;
using MIT.ECSR.Data.Model;
using MIT.ECSR.Core.Media.Command;
using MIT.ECSR.Shared.Interface;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MIT.ECSR.Core.Penawaran.Command
{

    #region Request
    public class PengajuanPenawaranMapping : Profile
    {
        public PengajuanPenawaranMapping()
        {
            CreateMap<PengajuanPenawaranRequest, PenawaranRequest>().ReverseMap();
        }
    }
    public class PengajuanPenawaranRequest : PenawaranRequest, IRequest<StatusResponse>
    {
        [Required]
        public Guid IdPerusahaan { get; set; }
        [Required]
        public string Inputer { get; set; }
    }
    #endregion

    internal class PengajuanPenawaranHandler : IRequestHandler<PengajuanPenawaranRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public PengajuanPenawaranHandler(
            ILogger<PengajuanPenawaranHandler> logger,
            IMediator mediator,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mediator = mediator;
            _context = context;
        }
        public async Task<StatusResponse> Handle(PengajuanPenawaranRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                if(request.Items.Count()>0 && request.Items.Any(d => d.Value > 0))
                {
                    var penawaran = new TrsPenawaran()
                    {
                        CreateBy = request.Inputer,
                        CreateDate = DateTime.Now,
                        Id = Guid.NewGuid(),
                        IdPerusahaan = request.IdPerusahaan,
                        Deskripsi = string.Empty
                    };
                    var list = request.Items.Where(d => d.Value > 0).Select(d => new TrsPenawaranItem()
                    {
                        CreateBy = request.Inputer,
                        CreateDate = DateTime.Now,
                        Id = Guid.NewGuid(),
                        IdPenawaran = penawaran.Id,
                        IdProgramItem = d.IdProgramItem,
                        Jumlah =  d.Value,
                        Rupiah = d.Anggaran,
                        Status = (int)PenawaranStatusEnum.DRAFT
                    }).ToList();
                    _context.Add(penawaran);
                    _context.Add(list);
                    var save = await _context.Commit();
                    if (save.Success)
                    {
                        result.OK();
                    }
                    else
                        result.BadRequest(save.Message);
                }
                else
                    result.BadRequest("Items pengajuan harus terisi!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Add Penawaran", request);
                result.Error("Failed Add Penawaran", ex.Message);
            }
            return result;
        }
    }
}

