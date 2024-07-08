using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Core.Media.Query;
using MIT.ECSR.Core.Response;
using MIT.ECSR.Data;
using MIT.ECSR.Data.Model;
using MIT.ECSR.Shared.Attributes;
using WonderKid.DAL.Interface;

namespace MIT.ECSR.Core.ProgresProgram.Query
{
    public class GetHistoryProgresProgramListExternalRequest : IRequest<ListResponse<ProgressDetailProgramExternalItemObject>>
    {
        public Guid IdPerusahaan { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public Guid IdProgram { get; set; }
    }
    internal class GetHistoryProgresProgramListHandler : IRequestHandler<GetHistoryProgresProgramListExternalRequest, ListResponse<ProgressDetailProgramExternalItemObject>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public GetHistoryProgresProgramListHandler(
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

        public async Task<ListResponse<ProgressDetailProgramExternalItemObject>> Handle(GetHistoryProgresProgramListExternalRequest request, CancellationToken cancellationToken)
        {
            ListResponse<ProgressDetailProgramExternalItemObject> result = new ListResponse<ProgressDetailProgramExternalItemObject>();
            try
            {
                var data = _context.Entity<TrsProgresProgram>()
                    .Include(x => x.IdProgramItemNavigation)
                    .ThenInclude(x => x.TrsPenawaranItem)
                    .Where(d => d.IdProgramItemNavigation.IdProgram == request.IdProgram && d.IdPerusahaan == request.IdPerusahaan);
                result.Count = data.Count();

                var dataList = await data.OrderByDescending(d => d.CreateDate)
                                    .Skip((request.Start - 1) * request.Length).Take(request.Length)
                                    .ToListAsync();
                result.List = dataList.Select(x =>
                {
                    var item = _mapper.Map<ProgressDetailProgramExternalItemObject>(x);
                    item.Lampiran = _mediator.Send(new GetMediaUrlListRequest() { Modul = new List<string> { x.Id.ToString() }, Tipe = "PROGRESS" }).GetAwaiter().GetResult()?.List?.Select(d => d.Media)?.ToList();
                    item.ProgramItemName = x.IdProgramItemNavigation.Nama;
                    item.Unit = x.IdProgramItemNavigation.Jumlah;
                    item.Satuan = x.IdProgramItemNavigation.SatuanUnit;
                    item.Booking = x.IdProgramItemNavigation.TrsPenawaranItem.Sum(d => d.Jumlah);
                    return item;
                }).ToList();
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
