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

namespace MIT.ECSR.Core.Perusahaan.Query
{
    public class GetPerusahaanListRequest : ListRequest,IListRequest<GetPerusahaanListRequest>,IRequest<ListResponse<PerusahaanResponse>>
    {
    }
    internal class GetPerusahaanListHandler : IRequestHandler<GetPerusahaanListRequest, ListResponse<PerusahaanResponse>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public GetPerusahaanListHandler(
            ILogger<GetPerusahaanListHandler> logger,
            IMapper mapper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ListResponse<PerusahaanResponse>> Handle(GetPerusahaanListRequest request, CancellationToken cancellationToken)
        {
            ListResponse<PerusahaanResponse> result = new ListResponse<PerusahaanResponse>();
            try
            {
				var query = _context.Entity<MIT.ECSR.Data.Model.MstPerusahaan>().AsQueryable();

				#region Filter
				Expression<Func<MIT.ECSR.Data.Model.MstPerusahaan, object>> column_sort = null;
				List<Expression<Func<MIT.ECSR.Data.Model.MstPerusahaan, bool>>> where = new List<Expression<Func<MIT.ECSR.Data.Model.MstPerusahaan, bool>>>();
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

				result.List = _mapper.Map<List<PerusahaanResponse>>(data_list);
				result.Filtered = data_list.Count();
				result.Count = await query_count.CountAsync();
				result.OK();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Get List Perusahaan", request);
                result.Error("Failed Get List Perusahaan", ex.Message);
            }
            return result;
        }

        #region List Utility
		private (Expression<Func<MIT.ECSR.Data.Model.MstPerusahaan, bool>> where, Expression<Func<MIT.ECSR.Data.Model.MstPerusahaan, object>> order) ListExpression(string search, string field, bool is_where)
		{
			Expression<Func<MIT.ECSR.Data.Model.MstPerusahaan, object>> result_order = null;
			Expression<Func<MIT.ECSR.Data.Model.MstPerusahaan, bool>> result_where = null;
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
					case "alamat" : 
						if(is_where){
							result_where = (d=>d.Alamat.Trim().ToLower().Contains(search));
						}
						else
							result_order = (d => d.Alamat);
					break;
					case "bidangusaha" : 
						if(is_where){
							result_where = (d=>d.BidangUsaha.Trim().ToLower().Contains(search));
						}
						else
							result_order = (d => d.BidangUsaha);
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
					case "iduser" : 
						if(is_where){
							if (Guid.TryParse(search, out var _IdUser))
								result_where = (d=>d.IdUser == _IdUser);
						}
						else
							result_order = (d => d.IdUser);
					break;
					case "jenisperseroan" : 
						if(is_where){
							result_where = (d=>d.JenisPerseroan.Trim().ToLower().Contains(search));
						}
						else
							result_order = (d => d.JenisPerseroan);
					break;
					case "namaperusahaan" : 
						if(is_where){
							result_where = (d=>d.NamaPerusahaan.Trim().ToLower().Contains(search));
						}
						else
							result_order = (d => d.NamaPerusahaan);
					break;
					case "nib" : 
						if(is_where){
							result_where = (d=>d.Nib.Trim().ToLower().Contains(search));
						}
						else
							result_order = (d => d.Nib);
					break;
					case "npwp" : 
						if(is_where){
							result_where = (d=>d.Npwp.Trim().ToLower().Contains(search));
						}
						else
							result_order = (d => d.Npwp);
					break;
					case "updateby" : 
						if(is_where){
							result_where = (d=>d.UpdateBy.Trim().ToLower().Contains(search));
						}
						else
							result_order = (d => d.UpdateBy);
					break;
					case "updatedate" : 
						if(is_where){
							if (DateTime.TryParse(search, out var _UpdateDate))
								result_where = (d=>d.UpdateDate == _UpdateDate);
						}
						else
							result_order = (d => d.UpdateDate);
					break;

                }
            }
            return (result_where, result_order);
        }
        #endregion
    }
}

