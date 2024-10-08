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
using Microsoft.Extensions.Options;
using System.Linq.Dynamic.Core;

namespace MIT.ECSR.Core.Media.Query
{
    public class GetMediaUrlRequest : IRequest<ListResponse<MediaUrl>>
    {
        public string Modul { get; set; }
        public string Tipe { get; set; }
    }
    internal class GetMediaUrlHandler : IRequestHandler<GetMediaUrlRequest, ListResponse<MediaUrl>>
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public GetMediaUrlHandler(
            ILogger<GetMediaListHandler> logger,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _context = context;
        }

        public async Task<ListResponse<MediaUrl>> Handle(GetMediaUrlRequest request, CancellationToken cancellationToken)
        {
            ListResponse<MediaUrl> result = new ListResponse<MediaUrl>();
            try
            {
                var data = await _context.Entity<MIT.ECSR.Data.Model.TrsMedia>().Where(d => d.Modul.ToLower() == request.Modul.ToLower() && d.Tipe.ToLower() == request.Tipe.ToLower())
                    .Select(d => new MediaUrl
                    {
                        Id = d.Id,
                        Original = ConstantApplication.BaseUrl + d.OriginalPath,
                        Resize = ConstantApplication.BaseUrl + d.ResizePath,
                        Filename = d.FileName
                    }).ToListAsync();
                if (data != null)
                {
                    result.List = data;
                    result.Count = data.Count;
                    result.Filtered = data.Count;
                    result.OK();
                }
                else
                    result.NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Get List Media", request);
                result.Error("Failed Get List Media", ex.Message);
            }
            return result;
        }

        #region List Utility
        private (Expression<Func<MIT.ECSR.Data.Model.TrsMedia, bool>> where, Expression<Func<MIT.ECSR.Data.Model.TrsMedia, object>> order) ListExpression(string search, string field, bool is_where)
        {
            Expression<Func<MIT.ECSR.Data.Model.TrsMedia, object>> result_order = null;
            Expression<Func<MIT.ECSR.Data.Model.TrsMedia, bool>> result_where = null;
            if (!string.IsNullOrWhiteSpace(search) && !string.IsNullOrWhiteSpace(field))
            {
                field = field.Trim().ToLower();
                search = search.Trim().ToLower();
                switch (field)
                {
                    case "id":
                        if (is_where)
                        {
                            if (Guid.TryParse(search, out var _Id))
                                result_where = (d => d.Id == _Id);
                        }
                        else
                            result_order = (d => d.Id);
                        break;
                    case "createby":
                        if (is_where)
                        {
                            result_where = (d => d.CreateBy.Trim().ToLower().Contains(search));
                        }
                        else
                            result_order = (d => d.CreateBy);
                        break;
                    case "createdate":
                        if (is_where)
                        {
                            if (DateTime.TryParse(search, out var _CreateDate))
                                result_where = (d => d.CreateDate == _CreateDate);
                        }
                        else
                            result_order = (d => d.CreateDate);
                        break;
                    case "extension":
                        if (is_where)
                        {
                            result_where = (d => d.Extension.Trim().ToLower().Contains(search));
                        }
                        else
                            result_order = (d => d.Extension);
                        break;
                    case "filename":
                        if (is_where)
                        {
                            result_where = (d => d.FileName.Trim().ToLower().Contains(search));
                        }
                        else
                            result_order = (d => d.FileName);
                        break;
                    case "modul":
                        if (is_where)
                        {
                            result_where = (d => d.Modul.Trim().ToLower().Contains(search));
                        }
                        else
                            result_order = (d => d.Modul);
                        break;
                    case "tipe":
                        if (is_where)
                        {
                            result_where = (d => d.Tipe.Trim().ToLower().Contains(search));
                        }
                        else
                            result_order = (d => d.Tipe);
                        break;

                }
            }
            return (result_where, result_order);
        }
        #endregion
    }
}

