using AutoMapper;
using MediatR;
using WonderKid.DAL.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Data;
using MIT.ECSR.Data.Model;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.Response;
using MIT.ECSR.Core.Helper;
using System.Linq.Dynamic.Core;
using MIT.ECSR.Core.Media.Query;
using static Humanizer.On;

namespace MIT.ECSR.Core.Penawaran.Query
{
    public class GetPenawaranListRequest : IRequest<ListResponse<PenawaranResponse>>
    {
        public string Search { get; set; }
        public Guid? IdPerusahaan { get; set; }
        public int? IdJenisProgram { get; set; }
        public int? IdKegiatan { get; set; }
        public string? TglPelaksanaan { get; set; }
        public int? Start { get; set; }
        public int? Length { get; set; }
    }
    internal class GetPenawaranListHandler : IRequestHandler<GetPenawaranListRequest, ListResponse<PenawaranResponse>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public GetPenawaranListHandler(
            ILogger<GetPenawaranListHandler> logger,
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

        public async Task<ListResponse<PenawaranResponse>> Handle(GetPenawaranListRequest request, CancellationToken cancellationToken)
        {
            ListResponse<PenawaranResponse> result = new ListResponse<PenawaranResponse>();
            try
            {
                var query = _context.Entity<TrsProgram>().Where(d => d.Status == (int)ProgramStatusEnum.OPEN || d.Status == (int)ProgramStatusEnum.ON_PROGRESS)
                            .Include(d => d.IdJenisProgramNavigation)
                            .Include(d => d.LokasiNavigation)
                            .Include(d => d.NamaProgramNavigation)
                            .Include(d => d.TrsProgramItem)
                            .ThenInclude(d => d.TrsPenawaranItem)
                            .ThenInclude(d => d.IdPenawaranNavigation)
                            .ThenInclude(d => d.IdPerusahaanNavigation)
                            .AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Search))
                    query = query.Where(d => d.NamaProgramNavigation.Name.ToLower().Contains(request.Search.ToLower()) || d.TrsProgramItem.Any(z => z.TrsPenawaranItem.Any(z => z.IdPenawaranNavigation.IdPerusahaanNavigation.NamaPerusahaan.ToLower().Contains(request.Search.ToLower()))));
                if (request.IdJenisProgram.HasValue)
                    query = query.Where(d => d.IdJenisProgram == request.IdJenisProgram);
                if (request.IdKegiatan.HasValue)
                    query = query.Where(d => d.IdJenisProgramNavigation.IdSubProgram == request.IdKegiatan);
                if (!string.IsNullOrEmpty(request.TglPelaksanaan))
                {
                    var splitDate = request.TglPelaksanaan.Split("|");
                    DateTime.TryParse(splitDate[0], out var _StartTglPelaksanaan);
                    DateTime.TryParse(splitDate[1], out var _EndTglPelaksanaan);
                    query = query.Where(d => d.StartTglPelaksanaan.AddDays(1) >= _StartTglPelaksanaan && d.EndTglPelaksanaan <= _EndTglPelaksanaan);
                }
                    

                var count = await query.CountAsync();
                if (request.IdPerusahaan.HasValue)
                {
                    query = query.OrderByDescending(d => d.UpdateDate);
                }
                else
                {
                    var test = query.ToList().Select(x => new
                    {
                        Id = x.Id,
                        Name = x.NamaProgramNavigation.Name,
                        JumlahProgramItem = x.TrsProgramItem.Sum(z => z.Jumlah),
                        JumlahPenawaran = x.TrsProgramItem.Sum(z => z.TrsPenawaranItem.Sum(x => x.Jumlah)),
                        Sisa = x.TrsProgramItem.Sum(z => z.Jumlah) - x.TrsProgramItem.Sum(z => z.TrsPenawaranItem.Sum(x => x.Jumlah))
                    });

                    query = query.ToList().OrderByDescending(d => d.TrsProgramItem.Sum(z => z.Jumlah) - d.TrsProgramItem.Sum(z => z.TrsPenawaranItem.Sum(x => x.Jumlah))).AsQueryable();
                }

                if (request.Start.HasValue && request.Length.HasValue && request.Length > 0)
                    query = query.Skip((request.Start.Value - 1) * request.Length.Value).Take(request.Length.Value);

                var data_list = query.ToList();
                if (data_list.Count() > 0)
                {
                    result.List = new List<PenawaranResponse>();
                    List<Guid> id_program_item = new List<Guid>();
                    foreach (var data in data_list)
                    {
                        id_program_item.AddRange(data.TrsProgramItem.Select(d => d.Id).ToList());
                    }
                    var total_penawaran = await _context.Entity<TrsPenawaranItem>().Where(d => id_program_item.Contains(d.IdProgramItem))
                                        .GroupBy(d => d.IdProgramItem)
                                        .Select(d => new
                                        {
                                            Total = d.Count(),
                                            IdProgramItem = d.Key,
                                            Jumlah = d.Sum(e => e.Jumlah)
                                        }).ToListAsync();
                    var query_penawaran = _context.Entity<TrsPenawaranItem>().Where(d => id_program_item.Contains(d.IdProgramItem)).AsQueryable();
                    if (request.IdPerusahaan.HasValue)
                        query_penawaran = query_penawaran.Where(d => d.IdPenawaranNavigation.IdPerusahaan == request.IdPerusahaan.Value).AsQueryable();

                    var penawaran = await query_penawaran.GroupBy(d => d.IdProgramItemNavigation.IdProgram).Select(d => new
                    {
                        IdProgram= d.Key,
                        Total = d.Count()
                    }).ToListAsync();

                    foreach (var data in data_list)
                    {
                        var obj = _mapper.Map<PenawaranResponse>(data);
                        obj.Items = new List<PenawaranKegiatanItemResponse>();
                        foreach (var item in data.TrsProgramItem)
                        {
                            obj.Items.Add(new PenawaranKegiatanItemResponse()
                            {
                                Kegiatan = item.Nama,
                                Satuan = item.SatuanUnit,
                                Jumlah = item.Jumlah,
                                Rupiah = item.Rupiah,
                                Sisa = item.Jumlah- total_penawaran.Where(d=>d.IdProgramItem==item.Id).Select(d=>d.Jumlah).FirstOrDefault()
                            });
                            obj.Penawaran += total_penawaran.Where(d => d.IdProgramItem == item.Id).Select(d => d.Total).FirstOrDefault();
                        }
                        obj.WaitingPenawaran = penawaran.Where(d => d.IdProgram == data.Id).Select(d => d.Total).FirstOrDefault();
                        result.List.Add(obj);
                    }
                    result.Filtered = result.List.Count();
                }

                result.Count = count;
                result.OK();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Get List Penawaran", request);
                result.Error("Failed Get List Penawaran", ex.Message);
            }
            return result;
        }
    }
}

