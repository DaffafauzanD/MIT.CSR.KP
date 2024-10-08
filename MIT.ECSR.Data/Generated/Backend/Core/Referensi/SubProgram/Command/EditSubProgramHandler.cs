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

namespace MIT.ECSR.Core.SubProgram.Command
{

    #region Request
    public class EditSubProgramMapping: Profile
    {
        public EditSubProgramMapping()
        {
            CreateMap<EditSubProgramRequest, SubProgramRequest>().ReverseMap();
        }
    }
    public class EditSubProgramRequest :SubProgramRequest, IMapRequest<MIT.ECSR.Data.Model.RefSubProgram, EditSubProgramRequest>,IRequest<StatusResponse>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Inputer { get; set; }
        public void Mapping(IMappingExpression<EditSubProgramRequest, MIT.ECSR.Data.Model.RefSubProgram> map)
        {
            //use this for mapping
            //map.ForMember(d => d.EF_COLUMN, opt => opt.MapFrom(s => s.Object));
        }
    }
    #endregion

    internal class EditSubProgramHandler : IRequestHandler<EditSubProgramRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public EditSubProgramHandler(
            ILogger<EditSubProgramHandler> logger,
            IMapper mapper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }
        public async Task<StatusResponse> Handle(EditSubProgramRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                var existingItems = await _context.Entity<MIT.ECSR.Data.Model.RefSubProgram>().Where(d => d.Id == request.Id).FirstOrDefaultAsync();
                if (existingItems != null)
                {
                    var item = _mapper.Map(request, existingItems);
                    item.UpdateBy = request.Inputer;
                    item.UpdateDate = DateTime.Now;
                    var update = await _context.UpdateSave(item);
                    if (update.Success)
                        result.OK();
                    else
                        result.BadRequest(update.Message);

                    return result;
                }
                else
                    result.NotFound($"Id SubProgram {request.Id} Tidak Ditemukan");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Edit SubProgram", request);
                result.Error("Failed Edit SubProgram", ex.Message);
            }
            return result;
        }
    }
}

