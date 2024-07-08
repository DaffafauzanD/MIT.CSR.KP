//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
using MIT.ECSR.Core.Media.Query;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MIT.ECSR.Core.Penawaran.Query
{
    public class GetPenawaranListInternalRequest : IRequest<ListResponse<PenawaranInternalResponse>>
    {
        public Guid IdProgram { get; set; }
        public string Search { get; set; }
        public int? Start { get; set; }
        public int? Length { get; set; }
    }
    internal class GetPenawaranListInternalHandler : IRequestHandler<GetPenawaranListInternalRequest, ListResponse<PenawaranInternalResponse>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public GetPenawaranListInternalHandler(
            ILogger<GetPenawaranListInternalHandler> logger,
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

        public async Task<ListResponse<PenawaranInternalResponse>> Handle(GetPenawaranListInternalRequest request, CancellationToken cancellationToken)
        {
            ListResponse<PenawaranInternalResponse> result = new ListResponse<PenawaranInternalResponse>();
            try
            {
                var query = _context.Entity<MIT.ECSR.Data.Model.TrsProgramItem>().Where(d=>d.IdProgram == request.IdProgram)
                            .Include(d=>d.TrsPenawaranItem)
                            .ThenInclude(d=>d.IdPenawaranNavigation)
                            .ThenInclude(d=>d.IdPerusahaanNavigation)
                            .AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Search))
                    query = query.Where(d => d.Nama.ToLower().Contains(request.Search.ToLower()));

                result.Count = await query.CountAsync();
                if (request.Start.HasValue && request.Length.HasValue && request.Length > 0)
                    query = query.OrderBy(d => d.IdProgramNavigation.StartTglPelaksanaan).Skip((request.Start.Value - 1) * request.Length.Value).Take(request.Length.Value);

                var data_list = await query.ToListAsync();

                if (data_list.Count() > 0)
                {
                    List<Guid> id_user = new List<Guid>();
                    List<Guid> id_penawaran = new List<Guid>();
                    foreach(var data in data_list)
                    {
                        id_user.AddRange(data.TrsPenawaranItem
                                    .Where(d => d.Status == (int)PenawaranStatusEnum.SUBMIT && d.IdPenawaranNavigation.IdPerusahaanNavigation.IdUser.HasValue)
                                    .Select(d => d.IdPenawaranNavigation.IdPerusahaanNavigation.IdUser.Value).ToList());
                        id_penawaran.AddRange(data.TrsPenawaranItem
                                    .Where(d => d.Status == (int)PenawaranStatusEnum.SUBMIT)
                                    .Select(d => d.IdPenawaran).ToList());
                    }
                    ListResponse<MediaUrlResponse> photos = null;
                    ListResponse<MediaUrlResponse> lampiran = null;
                    if (id_user.Count()>0)
                        photos = await _mediator.Send(new GetMediaUrlListRequest() { Modul = id_user.Distinct().Select(d => d.ToString()).ToList(), Tipe = "PHOTO_USER" });
                    if (id_penawaran.Count() > 0)
                        lampiran = await _mediator.Send(new GetMediaUrlListRequest() { Modul = id_penawaran.Distinct().Select(d => d.ToString()).ToList(), Tipe = "PENAWARAN_LAMPIRAN" });

                    result.List = new List<PenawaranInternalResponse>();
                    foreach (var data in data_list)
                    {
                        var obj = new PenawaranInternalResponse()
                        {
                            Item = _mapper.Map<ProgramItemResponse>(data),
                            Penawaran = new List<PenawaranInternalItemResponse>()
                        };
                        var penawaran_waiting = data.TrsPenawaranItem.Where(d => d.Status == (int)PenawaranStatusEnum.SUBMIT).ToList();

                        foreach (var penawaran in penawaran_waiting)
                        {
                            var id_user_perusahaan = penawaran.IdPenawaranNavigation.IdPerusahaanNavigation.IdUser;
                            obj.Penawaran.Add(new PenawaranInternalItemResponse()
                            {
                                Id = penawaran.Id,
                                Deskripsi = penawaran.IdPenawaranNavigation.Deskripsi,
                                IdPerusahaan = penawaran.IdPenawaranNavigation.IdPerusahaan,
                                Jumlah = penawaran.Jumlah,
                                Perusahaan = penawaran.IdPenawaranNavigation.IdPerusahaanNavigation.NamaPerusahaan,
                                Rupiah = penawaran.Rupiah,
                                Photos = photos != null && photos.Succeeded && id_user_perusahaan.HasValue ? photos.List.Where(x => x.Modul == id_user_perusahaan.Value.ToString()).Select(d => d.Media).FirstOrDefault() : null,
                                Lampiran = lampiran != null && lampiran.Succeeded ? lampiran.List.Where(d => d.Modul == penawaran.IdPenawaran.ToString()).Select(d => d.Media).ToList() : null
                            });
                        }
                        result.List.Add(obj);
                    }
                    result.Filtered = data_list.Count();
                }
                result.OK();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Get List PenawaranItem", request);
                result.Error("Failed Get List PenawaranItem", ex.Message);
            }
            return result;
        }

    }
}

