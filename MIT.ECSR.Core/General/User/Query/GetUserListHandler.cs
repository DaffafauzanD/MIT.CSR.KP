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
using DocumentFormat.OpenXml.Office2010.Excel;

namespace MIT.ECSR.Core.User.Query
{
    public class GetUserListRequest : ListRequest,IListRequest<GetUserListRequest>,IRequest<ListResponse<UserResponse>>
    {
    }
    internal class GetUserListHandler : IRequestHandler<GetUserListRequest, ListResponse<UserResponse>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public GetUserListHandler(
            ILogger<GetUserListHandler> logger,
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

        public async Task<ListResponse<UserResponse>> Handle(GetUserListRequest request, CancellationToken cancellationToken)
        {
            ListResponse<UserResponse> result = new ListResponse<UserResponse>();
            try
            {
				var query = _context.Entity<MIT.ECSR.Data.Model.SetUser>().Include(d=>d.IdRoleNavigation).Include(d=>d.MstPerusahaan).AsQueryable();

				#region Filter
				Expression<Func<MIT.ECSR.Data.Model.SetUser, object>> column_sort = null;
				List<Expression<Func<MIT.ECSR.Data.Model.SetUser, bool>>> where = new List<Expression<Func<MIT.ECSR.Data.Model.SetUser, bool>>>();
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
                if(data_list!= null)
                {
                    var photos = await _mediator.Send(new GetMediaUrlListRequest() { Modul = data_list.Select(d=>d.Id.ToString()).ToList(), Tipe = "PHOTO_USER" });
                    result.List = new List<UserResponse>();
                    foreach(var d in data_list)
                    {
                        var user = _mapper.Map<UserResponse>(d);
                        user.Photo = photos.Succeeded ? photos.List.Where(x => x.Modul == d.Id.ToString()).Select(d => d.Media).FirstOrDefault():null;
                        result.List.Add(user);
                    }
                    result.Filtered = data_list.Count();
                    result.Count = await query_count.CountAsync();
                }
				result.OK();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Get List User", request);
                result.Error("Failed Get List User", ex.Message);
            }
            return result;
        }

        #region List Utility
        private (Expression<Func<MIT.ECSR.Data.Model.SetUser, bool>> where, Expression<Func<MIT.ECSR.Data.Model.SetUser, object>> order) ListExpression(string search, string field, bool is_where)
		{
			Expression<Func<MIT.ECSR.Data.Model.SetUser, object>> result_order = null;
			Expression<Func<MIT.ECSR.Data.Model.SetUser, bool>> result_where = null;
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
                    case "createdate":
                        if (is_where)
                        {
                            if (DateTime.TryParse(search, out var _createDate))
                                result_where = (d => d.CreateDate == _createDate);
                        }
                        else
                            result_order = (d => d.CreateDate);
                    break;
                    case "username" : 
						if(is_where){
							result_where = (d=>d.Username.Trim().ToLower().Contains(search));
						}
						else
							result_order = (d => d.Username);
					break;
                    case "fullname":
                        if (is_where)
                        {
                            result_where = (d => d.Fullname.Trim().ToLower().Contains(search));
                        }
                        else
                            result_order = (d => d.Fullname);
                        break;
                    case "mail":
                        if (is_where)
                        {
                            result_where = (d => d.Mail.Trim().ToLower().Contains(search));
                        }
                        else
                            result_order = (d => d.Mail);
                        break;
                    case "phonenumber":
                        if (is_where)
                        {
                            result_where = (d => d.PhoneNumber.Trim().ToLower().Contains(search));
                        }
                        else
                            result_order = (d => d.PhoneNumber);
                        break;
					case "status" : 
						if(is_where){
                            if(search.ToLower() == "not active")
                                result_where = (d => !d.Active);
                            if (search.ToLower() == "active")
                                result_where = (d => d.Active);
						}
						else
							result_order = (d => d.UpdateDate);
					break;
                    case "role":
                        if (is_where)
                        {
                            result_where = (d => d.IdRoleNavigation.Name.Trim().ToLower().Contains(search));
                        }
                        else
                            result_order = (d => d.IdRoleNavigation.Name);
                        break;
                }
            }
            return (result_where, result_order);
        }
        #endregion
    }
}

