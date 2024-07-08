//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using AutoMapper;
using MediatR;
using WonderKid.DAL.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Data;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.Helper;
using MIT.ECSR.Core.Request;

namespace MIT.ECSR.Core.PenawaranItem.Command
{

    #region Request
    public class AddPenawaranItemMapping: Profile
    {
        public AddPenawaranItemMapping()
        {
            CreateMap<AddPenawaranItemRequest, PenawaranItemRequest>().ReverseMap();
        }
    }
    public class AddPenawaranItemRequest :PenawaranItemRequest, IMapRequest<MIT.ECSR.Data.Model.TrsPenawaranItem, AddPenawaranItemRequest>,IRequest<StatusResponse>
    {
        [Required]
        public string Inputer { get; set; }
        public void Mapping(IMappingExpression<AddPenawaranItemRequest, MIT.ECSR.Data.Model.TrsPenawaranItem> map)
        {
            //use this for mapping
            //map.ForMember(d => d.EF_COLUMN, opt => opt.MapFrom(s => s.Object));
        }
    }
    #endregion

    internal class AddPenawaranItemHandler : IRequestHandler<AddPenawaranItemRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public AddPenawaranItemHandler(
            ILogger<AddPenawaranItemHandler> logger,
            IMapper mapper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }
        public async Task<StatusResponse> Handle(AddPenawaranItemRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                var data = _mapper.Map<MIT.ECSR.Data.Model.TrsPenawaranItem>(request);
                data.CreateBy = request.Inputer;
                data.CreateDate = DateTime.Now;
                var add = await _context.AddSave(data);
                if (add.Success)
                    result.OK();
                else
                    result.BadRequest(add.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Add PenawaranItem", request);
                result.Error("Failed Add PenawaranItem", ex.Message);
            }
            return result;
        }
    }
}

