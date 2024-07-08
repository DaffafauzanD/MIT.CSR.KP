using AutoMapper;
using DocumentFormat.OpenXml.ExtendedProperties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Core.Helper;
using MIT.ECSR.Core.Media.Query;
using MIT.ECSR.Core.Response;
using MIT.ECSR.Data;
using MIT.ECSR.Data.Model;
using MIT.ECSR.Shared.Attributes;
using WonderKid.DAL.Interface;

namespace MIT.ECSR.Core.ProgresProgram.Query
{
    public class GetPublicHistoryProgresProgramListExternalRequest : IRequest<ListResponse<PublicProgressDetailProgramExternalItemObject>>
    {
        public Guid IdPerusahaan { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public Guid IdProgram { get; set; }
    }

    internal class GetPublicHistoryProgresProgramListHandler : IRequestHandler<GetPublicHistoryProgresProgramListExternalRequest, ListResponse<PublicProgressDetailProgramExternalItemObject>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public GetPublicHistoryProgresProgramListHandler(
            ILogger<GetProgresProgramListExternalHandler> logger,
            IMapper mapper,
            IMediator mediator,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _context = context;
        }

        public async Task<ListResponse<PublicProgressDetailProgramExternalItemObject>> Handle(GetPublicHistoryProgresProgramListExternalRequest request, CancellationToken cancellationToken)
        {
            ListResponse<PublicProgressDetailProgramExternalItemObject> result = new ListResponse<PublicProgressDetailProgramExternalItemObject>();
            try
            {
                var data = await _context.Entity<MstPerusahaan>()
                    .Include(x => x.TrsPenawaran)
                    .ThenInclude(x => x.TrsPenawaranItem)
                    .ThenInclude(x => x.IdProgramItemNavigation)
                    .ThenInclude(x => x.TrsPenawaranItem)
                    .ThenInclude(x => x.IdPenawaranNavigation)
                    .Include(x => x.TrsPenawaran)
                    .ThenInclude(x => x.TrsPenawaranItem)
                    .ThenInclude(x => x.IdProgramItemNavigation)
                    .ThenInclude(x => x.TrsProgresProgram)
                    .ThenInclude(x => x.IdPerusahaanNavigation).ToListAsync();

                var resultData = data
                    .Where(x => x.TrsPenawaran.Count > 0 && x.TrsPenawaran.Any(z => z.TrsPenawaranItem.Any(d => d.IdProgramItemNavigation.IdProgram == request.IdProgram)))
                    .ToList()
                    .SelectMany(x => x.TrsPenawaran.SelectMany(z => z.TrsPenawaranItem.Select(c => c.IdProgramItemNavigation)))
                    .Where(x => x.IdProgram == request.IdProgram)
                    .GroupBy(x => x.Id).Select(x => x.FirstOrDefault());

                result.List = new List<PublicProgressDetailProgramExternalItemObject>();
                foreach (var item in resultData)
                {
                    result.List.AddRange(item.TrsProgresProgram
                        .Where(x => x.Status == (int)ProgressStatusEnum.WAITING || x.Status == (int)ProgressStatusEnum.APPROVE)
                        .Select(x =>
                    {
                        var itemResult = _mapper.Map<PublicProgressDetailProgramExternalItemObject>(x);
                        itemResult.Lampiran = _mediator.Send(new GetMediaUrlListRequest() { Modul = new List<string> { x.Id.ToString() }, Tipe = "PROGRESS" }).GetAwaiter().GetResult()?.List?.Select(d => d.Media)?.ToList();
                        itemResult.ProgramItemName = item.Nama;
                        itemResult.Unit = item.TrsPenawaranItem.Where(z => z.IdPenawaranNavigation.IdPerusahaan == x.IdPerusahaan).FirstOrDefault()?.Jumlah
                         ?? 0;
                        itemResult.Satuan = item.SatuanUnit;
                        itemResult.Perusahaan = x.IdPerusahaanNavigation.NamaPerusahaan;
                        return itemResult;
                    }));
                }

                result.Count = result.List.Count();

                var dataList = result.List.OrderByDescending(d => d.TglProgress)
                                    .Skip((request.Start - 1) * request.Length).Take(request.Length).ToList();
                result.List = dataList;
                result.Filtered = dataList.Count;
                result.OK();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Get List History ProgresProgram", request);
                result.Error("Failed Get List History ProgresProgram", ex.Message);
            }
            return result;
        }
    }
}
