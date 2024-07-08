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
using System.Windows.Markup;

namespace MIT.ECSR.Core.Monitoring.Query
{
    public class GetMonitoringListRequest : IRequest<ListResponse<MonitoringResponse>>
    {
		public int? IdJenisProgram { get; set; }
        public int? Status { get; set; }
        public string Search { get; set; }
        public int? Start { get; set; }
        public int? Length { get; set; }
        public Guid? IdPerusahaan { get; set; }
    }
    internal class GetMonitoringListHandler : IRequestHandler<GetMonitoringListRequest, ListResponse<MonitoringResponse>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public GetMonitoringListHandler(
            ILogger<GetMonitoringListHandler> logger,
            IMapper mapper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ListResponse<MonitoringResponse>> Handle(GetMonitoringListRequest request, CancellationToken cancellationToken)
        {
            ListResponse<MonitoringResponse> result = new ListResponse<MonitoringResponse>();
            try
            {
                List<int> status = new List<int>() { (int)ProgramStatusEnum.ON_PROGRESS, (int)ProgramStatusEnum.CLOSED };
                IQueryable<TrsPenawaran> query = _context.Entity<TrsPenawaran>().Where(d=> status.Contains(d.IdProgramNavigation.Status))
                                                .Include(d => d.IdProgramNavigation).ThenInclude(d => d.IdDatiNavigation)
                                                .Include(d => d.IdProgramNavigation).ThenInclude(d => d.IdJenisProgramNavigation)
                                                .Include(d => d.IdPerusahaanNavigation)
                                                .Include(d => d.IdProgramNavigation).ThenInclude(d=>d.TrsProgramItem).ThenInclude(d=>d.IdSatuanJenisNavigation)
                                                .Include(d => d.IdProgramNavigation).ThenInclude(d => d.TrsProgramItem).ThenInclude(d => d.TrsProgresProgram).ThenInclude(d=>d.TrsProgresProgramMedia)
                                                .AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Search))
                {
                    if (request.IdPerusahaan.HasValue)
                        query = query.Where(d => d.IdProgramNavigation.NamaProgram.Trim().ToLower().Contains(request.Search.Trim().ToLower())).AsQueryable();
                    else
                        query = query.Where(d => d.IdProgramNavigation.NamaProgram.Trim().ToLower().Contains(request.Search.Trim().ToLower())
                                            || d.IdPerusahaanNavigation.NamaPerusahaan.Trim().ToLower().Contains(request.Search.Trim().ToLower())
                        ).AsQueryable();
                }
                
                if (request.IdJenisProgram.HasValue)
                    query = query.Where(d => d.IdProgramNavigation.IdJenisProgram == request.IdJenisProgram.Value).AsQueryable();

                if (request.IdPerusahaan.HasValue)
                    query = query.Where(d => d.IdPerusahaan == request.IdPerusahaan.Value).AsQueryable();

                if (request.Status.HasValue)
                    query = query.Where(d => d.Status == request.Status.Value).AsQueryable();

                var query_count = query;
				if (request.Start.HasValue && request.Length.HasValue && request.Length > 0)
					query = query.Skip((request.Start.Value - 1) * request.Length.Value).Take(request.Length.Value);

				var data_list = await query.ToListAsync();

				var list = _mapper.Map<List<MonitoringResponse>>(data_list);
                result.List = new List<MonitoringResponse>();
                foreach(var d in data_list)
                {
                    var obj = _mapper.Map<MonitoringResponse>(d);
                    var item = d.IdProgramNavigation.TrsProgramItem.ToList();
                    List<int> values = new List<int>();
                    foreach(var i in item)
                    {
                        values.Add(i.TrsProgresProgram != null ? i.TrsProgresProgram.Select(d => d.Progress).OrderByDescending(d => d).FirstOrDefault() : 0);
                    }
                    obj.TotalProgress = values.Count()>0?values.Average():0;
                    obj.Action = new List<string>()
                    {
                        "DETAIL"
                    };
                    if(obj.Penawaran.Program.Status != (int)ProgramStatusEnum.ON_PROGRESS)
                    {
                        if (obj.TotalProgress < 100)
                            obj.Action.Add("UPDATE");
                        else
                            obj.Action.Add("CLOSING");
                    }

                    result.List.Add(obj);
                }
                result.Filtered = data_list.Count();
				result.Count = await query_count.CountAsync();
				result.OK();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Get List Monitoring", request);
                result.Error("Failed Get List Monitoring", ex.Message);
            }
            return result;
        }

        
    }
}

