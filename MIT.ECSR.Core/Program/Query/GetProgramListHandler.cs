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
using DocumentFormat.OpenXml.Presentation;

namespace MIT.ECSR.Core.Program.Query
{
    public class GetProgramListRequest : ListRequest,IListRequest<GetProgramListRequest>,IRequest<ListResponse<ProgramResponse>>
    {
    }
    internal class GetProgramListHandler : IRequestHandler<GetProgramListRequest, ListResponse<ProgramResponse>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
		private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public GetProgramListHandler(
            ILogger<GetProgramListHandler> logger,
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

        public async Task<ListResponse<ProgramResponse>> Handle(GetProgramListRequest request, CancellationToken cancellationToken)
        {
            ListResponse<ProgramResponse> result = new ListResponse<ProgramResponse>();
            try
            {
				var query = _context.Entity<MIT.ECSR.Data.Model.TrsProgram>().Include(d => d.IdJenisProgramNavigation)
					.Include(d=>d.TrsProgramItem)
					.Include(d=>d.NamaProgramNavigation)
					.Include(d=>d.LokasiNavigation)
					.AsQueryable();
				
				#region Filter
				Expression<Func<MIT.ECSR.Data.Model.TrsProgram, object>> column_sort = null;
				List<Expression<Func<MIT.ECSR.Data.Model.TrsProgram, bool>>> where = new List<Expression<Func<MIT.ECSR.Data.Model.TrsProgram, bool>>>();
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
                    var photos = await _mediator.Send(new GetMediaUrlListRequest() { Modul = data_list.Select(d => d.Id.ToString()).ToList(), Tipe = "PHOTO_PROGRAM" });
                    result.List = new List<ProgramResponse>();
                    foreach (var d in data_list)
                    {
                        var obj = _mapper.Map<ProgramResponse>(d);
                        obj.Photo = photos.Succeeded ? photos.List.Where(x => x.Modul == d.Id.ToString()).Select(d => d.Media).FirstOrDefault() : null;
                        result.List.Add(obj);
                    }
                }
                result.Filtered = data_list.Count();
                result.Count = await query_count.CountAsync();
                result.OK();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Get List Program", request);
                result.Error("Failed Get List Program", ex.Message);
            }
            return result;
        }

        #region List Utility
		private (Expression<Func<MIT.ECSR.Data.Model.TrsProgram, bool>> where, Expression<Func<MIT.ECSR.Data.Model.TrsProgram, object>> order) ListExpression(string search, string field, bool is_where)
		{
			Expression<Func<MIT.ECSR.Data.Model.TrsProgram, object>> result_order = null;
			Expression<Func<MIT.ECSR.Data.Model.TrsProgram, bool>> result_where = null;
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
					case "endprogramkerja" : 
						if(is_where){
							if (DateTime.TryParse(search, out var _EndProgramKerja))
								result_where = (d=>d.EndProgramKerja == _EndProgramKerja);
						}
						else
							result_order = (d => d.EndProgramKerja);
					break;
					case "tglpelaksanaan" : 
						if(is_where){
							var splitDate = search.Split("|");
							DateTime.TryParse(splitDate[0], out var _StartTglPelaksanaan);
							DateTime.TryParse(splitDate[1], out var _EndTglPelaksanaan);
                            result_where = (d => d.StartTglPelaksanaan.AddDays(1) >= _StartTglPelaksanaan && d.EndTglPelaksanaan <= _EndTglPelaksanaan);
                        }
						else
							result_order = (d => d.EndTglPelaksanaan);
					break;
					case "idjenisprogram" : 
						if(is_where){
							if (int.TryParse(search, out var _IdJenisProgram))
								result_where = (d=>d.IdJenisProgram == _IdJenisProgram);
						}
						else
							result_order = (d => d.IdJenisProgram);
					break;
					case "namaprogram" : 
						if(is_where){
							if (int.TryParse(search, out var _NamaProgram))
								result_where = (d => d.NamaProgram == _NamaProgram);
						}
						else
							result_order = (d => d.NamaProgram);
					break;
					case "status" : 
						if(is_where){
							if (short.TryParse(search, out var _Status))
                            {
								if(_Status == 0)
                                {
									List<int> all = new List<int>() { (int)ProgramStatusEnum.ON_PROGRESS, (int)ProgramStatusEnum.CLOSED,(int)ProgramStatusEnum.OPEN };
                                    result_where = (d => all.Contains(d.Status));
                                }else
                                    result_where = (d => d.Status == _Status);
                            }
						}
						else
							result_order = (d => d.Status);
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

