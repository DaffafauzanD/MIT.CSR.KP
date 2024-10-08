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
using System.ComponentModel.DataAnnotations;

namespace MIT.ECSR.Core.Monitoring.Query
{
    public class GetProgresProgramListRequest : IRequest<ListResponse<ProgressResponse>>
    {
        [Required]
        public Guid IdProgram { get; set; }
        public string Search { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Start { get; set; }
        public int? Length { get; set; }
    }
    internal class GetProgresProgramListHandler : IRequestHandler<GetProgresProgramListRequest, ListResponse<ProgressResponse>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public GetProgresProgramListHandler(
            ILogger<GetProgresProgramListHandler> logger,
            IMapper mapper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ListResponse<ProgressResponse>> Handle(GetProgresProgramListRequest request, CancellationToken cancellationToken)
        {
            ListResponse<ProgressResponse> result = new ListResponse<ProgressResponse>();
            try
            {
				var query = _context.Entity<MIT.ECSR.Data.Model.TrsProgresProgram>()
                            .Where(d=>d.IdProgramItemNavigation.IdProgram == request.IdProgram)
							.Include(d=>d.IdProgramItemNavigation).ThenInclude(d=>d.IdSatuanJenisNavigation)
                            .Include(d => d.TrsProgresProgramMedia).ThenInclude(d => d.IdMediaNavigation)
							.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Search))
                {
                    query = query.Where(d => d.IdProgramItemNavigation.Nama.Trim().ToLower().Contains(request.Search.Trim().ToLower())
                    || d.Deskripsi.Trim().ToLower().Contains(request.Search.Trim().ToLower())).AsQueryable();
                }
                if (request.StartDate.HasValue)
                    query = query.Where(d => d.CreateDate >= request.StartDate.Value).AsQueryable();

                if (request.EndDate.HasValue)
                    query = query.Where(d => d.CreateDate <= request.EndDate.Value).AsQueryable();

                var query_count = query;
                if (request.Start.HasValue && request.Length.HasValue && request.Length > 0)
                    query = query.Skip((request.Start.Value - 1) * request.Length.Value).Take(request.Length.Value);

                var data_list = await query.ToListAsync();

				result.List = _mapper.Map<List<ProgressResponse>>(data_list);
				result.Filtered = data_list.Count();
				result.Count = await query_count.CountAsync();
				result.OK();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Get List ProgresProgram", request);
                result.Error("Failed Get List ProgresProgram", ex.Message);
            }
            return result;
        }    
    }
}

