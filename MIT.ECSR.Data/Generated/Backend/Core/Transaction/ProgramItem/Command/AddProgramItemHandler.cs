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

namespace MIT.ECSR.Core.ProgramItem.Command
{

    #region Request
    public class AddProgramItemMapping: Profile
    {
        public AddProgramItemMapping()
        {
            CreateMap<AddProgramItemRequest, ProgramItemRequest>().ReverseMap();
        }
    }
    public class AddProgramItemRequest :ProgramItemRequest, IMapRequest<MIT.ECSR.Data.Model.TrsProgramItem, AddProgramItemRequest>,IRequest<StatusResponse>
    {
        [Required]
        public string Inputer { get; set; }
        public void Mapping(IMappingExpression<AddProgramItemRequest, MIT.ECSR.Data.Model.TrsProgramItem> map)
        {
            //use this for mapping
            //map.ForMember(d => d.EF_COLUMN, opt => opt.MapFrom(s => s.Object));
        }
    }
    #endregion

    internal class AddProgramItemHandler : IRequestHandler<AddProgramItemRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public AddProgramItemHandler(
            ILogger<AddProgramItemHandler> logger,
            IMapper mapper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }
        public async Task<StatusResponse> Handle(AddProgramItemRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                var data = _mapper.Map<MIT.ECSR.Data.Model.TrsProgramItem>(request);
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
                _logger.LogError(ex, "Failed Add ProgramItem", request);
                result.Error("Failed Add ProgramItem", ex.Message);
            }
            return result;
        }
    }
}

