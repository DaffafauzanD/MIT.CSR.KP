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
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Data;
using MIT.ECSR.Data.Model;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.Response;

namespace MIT.ECSR.Core.Status.Query
{

    public class GetStatusByIdRequest : IRequest<ObjectResponse<StatusResponse>>
    {
        [Required]
        public int Id { get; set; }
    }
    internal class GetStatusByIdHandler : IRequestHandler<GetStatusByIdRequest, ObjectResponse<StatusResponse>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public GetStatusByIdHandler(
            ILogger<GetStatusByIdHandler> logger,
            IMapper mapper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }
        public async Task<ObjectResponse<StatusResponse>> Handle(GetStatusByIdRequest request, CancellationToken cancellationToken)
        {
            ObjectResponse<StatusResponse> result = new ObjectResponse<StatusResponse>();
            try
            {
                var item = await _context.Entity<MIT.ECSR.Data.Model.RefStatus>().Where(d => d.Id == request.Id).FirstOrDefaultAsync();
                if (item != null)
                {
                    result.Data = _mapper.Map<StatusResponse>(item);
                    result.OK();
                }
                else
                    result.NotFound($"Id MIT.ECSR.Data.Model.RefStatus {request.Id} Tidak Ditemukan");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Get Detail Status", request.Id);
                result.Error("Failed Get Detail Status", ex.Message);
            }
            return result;
        }
    }
}

