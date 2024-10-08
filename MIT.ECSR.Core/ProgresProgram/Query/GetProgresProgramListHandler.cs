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
using System.Globalization;

namespace MIT.ECSR.Core.ProgresProgram.Query
{
    public class GetProgresProgramListRequest : ListRequest,IListRequest<GetProgresProgramListRequest>,IRequest<ListResponse<ProgresProgramResponse>>
    {
    }
    internal class GetProgresProgramListHandler : IRequestHandler<GetProgresProgramListRequest, ListResponse<ProgresProgramResponse>>
    {
		private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public GetProgresProgramListHandler(
            ILogger<GetProgresProgramListHandler> logger,
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

        public async Task<ListResponse<ProgresProgramResponse>> Handle(GetProgresProgramListRequest request, CancellationToken cancellationToken)
        {
            ListResponse<ProgresProgramResponse> result = new ListResponse<ProgresProgramResponse>();
            try
            {
				var query = _context.Entity<MIT.ECSR.Data.Model.TrsProgresProgram>()
                    .Include(d => d.IdProgramItemNavigation)
                    .ThenInclude(d => d.IdProgramNavigation)
                    .ThenInclude(d => d.IdJenisProgramNavigation)
                    .Include(d => d.IdPerusahaanNavigation)
                    .Include(d => d.IdProgramItemNavigation)
                    .ThenInclude(d => d.IdProgramNavigation)
                    .ThenInclude(d => d.NamaProgramNavigation)
                    .Where(x => x.Progress >= 100 && x.Status == (int)ProgressStatusEnum.WAITING);

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
                if (data_list != null)
                {
                    var lampiran = await _mediator.Send(new GetMediaUrlListRequest() { Modul = data_list.Select(d => d.Id.ToString()).ToList(), Tipe = "PROGRESS" });
                    result.List = new List<ProgresProgramResponse>();
                    foreach (var d in data_list)
                    {
						var obj = _mapper.Map<ProgresProgramResponse>(d);
						obj.Lampiran = lampiran.Succeeded ? lampiran.List.Where(x => x.Modul == d.Id.ToString()).Select(d => d.Media).ToList() : null;
                        result.List.Add(obj);
                    }
                }
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
					case "createby" : 
						if(is_where){
							result_where = (d=>d.CreateBy.Trim().ToLower().Contains(search));
						}
						else
							result_order = (d => d.CreateBy);
					break;
                    case "companyname":
                        if (is_where)
                        {
                            result_where = (d => d.IdPerusahaanNavigation.NamaPerusahaan.Trim().ToLower().Contains(search));
                        }
                        else
                            result_order = (d => d.IdPerusahaanNavigation.NamaPerusahaan);
                        break;
                    case "jenisprogram":
                        if (is_where)
                        {
                            if (int.TryParse(search, out var _jenisProgram))
                            {
                                result_where = (d => d.IdProgramItemNavigation.IdProgramNavigation.IdJenisProgram == _jenisProgram);
                            }
                        }
                        else
                            result_order = (d => d.IdProgramItemNavigation.IdProgramNavigation.IdJenisProgram);
                        break;
                    case "namaprogram":
                        if (is_where)
                        {
                            result_where = (d => d.IdProgramItemNavigation.IdProgramNavigation.NamaProgramNavigation.Name.Trim().ToLower().Contains(search));
                        }
                        else
                            result_order = (d => d.IdProgramItemNavigation.IdProgramNavigation.NamaProgram);
                        break;
                    case "createdate" : 
						if(is_where){
							if (DateTime.TryParse(search, out var _CreateDate))
								result_where = (d=>d.CreateDate == _CreateDate);
						}
						else
							result_order = (d => d.CreateDate);
					break;
                    case "tglprogress":
                        if (is_where)
                        {
                            if (DateTime.TryParse(search, out var _tglprogress))
                                result_where = (d => d.TglProgress == _tglprogress);
                        }
                        else
                            result_order = (d => d.TglProgress);
                        break;
                    case "deskripsi" : 
						if(is_where){
							result_where = (d=>d.Deskripsi.Trim().ToLower().Contains(search));
						}
						else
							result_order = (d => d.Deskripsi);
					break;
					case "idprogramitem" : 
						if(is_where){
							if (Guid.TryParse(search, out var _IdProgramItem))
								result_where = (d=>d.IdProgramItem == _IdProgramItem);
						}
						else
							result_order = (d => d.IdProgramItem);
					break;
                    case "idprogram":
                        if (is_where)
                        {
                            if (Guid.TryParse(search, out var _idprogram))
                                result_where = (d => d.IdProgramItemNavigation.IdProgram == _idprogram);
                        }
                        else
                            result_order = (d => d.IdProgramItemNavigation.IdProgram);
                        break;
                    case "status":
                        if (is_where)
                        {
                            if (int.TryParse(search, out var _status))
                            {
                                result_where = (d => d.Status == _status);

                            }
                        }
                        else
                            result_order = (d => d.Status);
                        break;
                    case "progress" : 
						if(is_where){
							if (int.TryParse(search, out var _Progress))
								result_where = (d=>d.Progress == _Progress);
						}
						else
							result_order = (d => d.Progress);
					break;
                    case "itemname":
                        if (is_where)
                        {
                            result_where = (d => d.IdProgramItemNavigation.Nama.Trim().ToLower().Contains(search));
                        }
                        else
                            result_order = (d => d.IdProgramItemNavigation.Nama);
                        break;

                }
            }
            return (result_where, result_order);
        }
        #endregion
    }
}

