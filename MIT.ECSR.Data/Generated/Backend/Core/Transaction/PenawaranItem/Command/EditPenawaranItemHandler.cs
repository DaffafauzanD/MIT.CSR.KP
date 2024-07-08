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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Data;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.Helper;
using MIT.ECSR.Core.Request;

namespace MIT.ECSR.Core.PenawaranItem.Command
{

    #region Request
    public class EditPenawaranItemMapping: Profile
    {
        public EditPenawaranItemMapping()
        {
            CreateMap<EditPenawaranItemRequest, PenawaranItemRequest>().ReverseMap();
        }
    }
    public class EditPenawaranItemRequest :PenawaranItemRequest, IMapRequest<MIT.ECSR.Data.Model.TrsPenawaranItem, EditPenawaranItemRequest>,IRequest<StatusResponse>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Inputer { get; set; }
        public void Mapping(IMappingExpression<EditPenawaranItemRequest, MIT.ECSR.Data.Model.TrsPenawaranItem> map)
        {
            //use this for mapping
            //map.ForMember(d => d.EF_COLUMN, opt => opt.MapFrom(s => s.Object));
        }
    }
    #endregion

    internal class EditPenawaranItemHandler : IRequestHandler<EditPenawaranItemRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public EditPenawaranItemHandler(
            ILogger<EditPenawaranItemHandler> logger,
            IMapper mapper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }
        public async Task<StatusResponse> Handle(EditPenawaranItemRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                var existingItems = await _context.Entity<MIT.ECSR.Data.Model.TrsPenawaranItem>().Where(d => d.Id == request.Id).FirstOrDefaultAsync();
                if (existingItems != null)
                {
                    var item = _mapper.Map(request, existingItems);
                    
                    
                    var update = await _context.UpdateSave(item);
                    if (update.Success)
                        result.OK();
                    else
                        result.BadRequest(update.Message);

                    return result;
                }
                else
                    result.NotFound($"Id PenawaranItem {request.Id} Tidak Ditemukan");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Edit PenawaranItem", request);
                result.Error("Failed Edit PenawaranItem", ex.Message);
            }
            return result;
        }
    }
}

