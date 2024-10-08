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

namespace MIT.ECSR.Core.ProgresProgram.Query
{
    public class GetProgresProgramListRequest : ListRequest,IListRequest<GetProgresProgramListRequest>,IRequest<ListResponse<ProgresProgramResponse>>
    {
    }
    internal class GetProgresProgramListHandler : IRequestHandler<GetProgresProgramListRequest, ListResponse<ProgresProgramResponse>>
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

        public async Task<ListResponse<ProgresProgramResponse>> Handle(GetProgresProgramListRequest request, CancellationToken cancellationToken)
        {
            ListResponse<ProgresProgramResponse> result = new ListResponse<ProgresProgramResponse>();
            try
            {
				var query = _context.Entity<MIT.ECSR.Data.Model.TrsProgresProgram>().AsQueryable();

				#region Filter
				Expression<Func<MIT.ECSR.Data.Model.TrsProgresProgram, object>> column_sort = null;
				List<Expression<Func<MIT.ECSR.Data.Model.TrsProgresProgram, bool>>> where = new List<Expression<Func<MIT.ECSR.Data.Model.TrsProgresProgram, bool>>>();
				if (request.Filter != null && request.Filter.Count > 0)
				{
					foreach (var f in request.Filter)
					{
						var obj = ListExpression(f.Search, f.Field, true);
						if (obj.where != null)
							where.Add(obj.where);
					}
				}
				if (where != null && where.Count() > 0)
				{
					foreach (var w in where)
					{
						query = query.Where(w);
					}
				}
				if (request.Sort != null)
                {
					column_sort = ListExpression(request.Sort.Field, request.Sort.Field, false).order!;
					if(column_sort != null)
						query = request.Sort.Type == SortTypeEnum.ASC ? query.OrderBy(column_sort) : query.OrderByDescending(column_sort);
					else
						query = query.OrderBy(d=>d.Id);
				}
				#endregion

				var query_count = query;
				if (request.Start.HasValue && request.Length.HasValue && request.Length > 0)
					query = query.Skip((request.Start.Value - 1) * request.Length.Value).Take(request.Length.Value);
				var data_list = await query.ToListAsync();

				result.List = _mapper.Map<List<ProgresProgramResponse>>(data_list);
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

        #region List Utility
		private (Expression<Func<MIT.ECSR.Data.Model.TrsProgresProgram, bool>> where, Expression<Func<MIT.ECSR.Data.Model.TrsProgresProgram, object>> order) ListExpression(string search, string field, bool is_where)
		{
			Expression<Func<MIT.ECSR.Data.Model.TrsProgresProgram, object>> result_order = null;
			Expression<Func<MIT.ECSR.Data.Model.TrsProgresProgram, bool>> result_where = null;
            if (!string.IsNullOrWhiteSpace(search) && !string.IsNullOrWhiteSpace(field))
            {
                field = field.Trim().ToLower();
                search = search.Trim().ToLower();
                switch (field)
                {
					case "id" : 
						if(is_where){
							if (Guid.TryParse(search, out var _Id))
								result_where = (d=>d.Id == _Id);
						}
						else
							result_order = (d => d.Id);
					break;
					case "approvedat" : 
						if(is_where){
							if (DateTime.TryParse(search, out var _ApprovedAt))
								result_where = (d=>d.ApprovedAt == _ApprovedAt);
						}
						else
							result_order = (d => d.ApprovedAt);
					break;
					case "approvedby" : 
						if(is_where){
							result_where = (d=>d.ApprovedBy.Trim().ToLower().Contains(search));
						}
						else
							result_order = (d => d.ApprovedBy);
					break;
					case "createby" : 
						if(is_where){
							result_where = (d=>d.CreateBy.Trim().ToLower().Contains(search));
						}
						else
							result_order = (d => d.CreateBy);
					break;
					case "createdate" : 
						if(is_where){
							if (DateTime.TryParse(search, out var _CreateDate))
								result_where = (d=>d.CreateDate == _CreateDate);
						}
						else
							result_order = (d => d.CreateDate);
					break;
					case "deskripsi" : 
						if(is_where){
							result_where = (d=>d.Deskripsi.Trim().ToLower().Contains(search));
						}
						else
							result_order = (d => d.Deskripsi);
					break;
					case "idperusahaan" : 
						if(is_where){
							if (Guid.TryParse(search, out var _IdPerusahaan))
								result_where = (d=>d.IdPerusahaan == _IdPerusahaan);
						}
						else
							result_order = (d => d.IdPerusahaan);
					break;
					case "idprogramitem" : 
						if(is_where){
							if (Guid.TryParse(search, out var _IdProgramItem))
								result_where = (d=>d.IdProgramItem == _IdProgramItem);
						}
						else
							result_order = (d => d.IdProgramItem);
					break;
					case "notes" : 
						if(is_where){
							result_where = (d=>d.Notes.Trim().ToLower().Contains(search));
						}
						else
							result_order = (d => d.Notes);
					break;
					case "progress" : 
						if(is_where){
							if (int.TryParse(search, out var _Progress))
								result_where = (d=>d.Progress == _Progress);
						}
						else
							result_order = (d => d.Progress);
					break;
					case "status" : 
						if(is_where){
							if (int.TryParse(search, out var _Status))
								result_where = (d=>d.Status == _Status);
						}
						else
							result_order = (d => d.Status);
					break;
					case "tglprogress" : 
						if(is_where){
							if (DateTime.TryParse(search, out var _TglProgress))
								result_where = (d=>d.TglProgress == _TglProgress);
						}
						else
							result_order = (d => d.TglProgress);
					break;

                }
            }
            return (result_where, result_order);
        }
        #endregion
    }
}

