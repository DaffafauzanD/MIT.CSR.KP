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
using MIT.ECSR.Data.Model;
using DocumentFormat.OpenXml.Spreadsheet;
using MIT.ECSR.Core.Media.Command;
using MIT.ECSR.Shared.Interface;

namespace MIT.ECSR.Core.Program.Command
{

    #region Request
    public class EditProgramMapping: Profile
    {
        public EditProgramMapping()
        {
            CreateMap<EditProgramRequest, ProgramRequest>().ReverseMap();
        }
    }
    public class EditProgramRequest :ProgramRequest, IMapRequest<MIT.ECSR.Data.Model.TrsProgram, EditProgramRequest>,IRequest<StatusResponse>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Inputer { get; set; }
        public void Mapping(IMappingExpression<EditProgramRequest, MIT.ECSR.Data.Model.TrsProgram> map)
        {
            map.ForMember(d => d.EndTglPelaksanaan, opt => opt.MapFrom(s => s.EndPelaksanaan))
                .ForMember(d => d.StartTglPelaksanaan, opt => opt.MapFrom(s => s.StartPelaksanaan))
                .ForMember(d => d.EndProgramKerja, opt => opt.MapFrom(s => s.BatasWaktuProgram));
        }
    }
    #endregion

    internal class EditProgramHandler : IRequestHandler<EditProgramRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IGeneralHelper _helper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public EditProgramHandler(
            ILogger<EditProgramHandler> logger,
            IMapper mapper,
            IMediator mediator,
            IGeneralHelper helper,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _helper = helper;
            _context = context;
        }
        public async Task<StatusResponse> Handle(EditProgramRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                var existingItems = await _context.Entity<MIT.ECSR.Data.Model.TrsProgram>().Where(d => d.Id == request.Id).FirstOrDefaultAsync();
                if (existingItems != null)
                {
                    if (existingItems.Status != (int)ProgramStatusEnum.DRAFT)
                    {
                        result.BadRequest("Cannot Be Edited because Status is " + ((ProgramStatusEnum)existingItems.Status).ToString());
                        return result;
                    }

                    var item = _mapper.Map(request, existingItems);
                    item.UpdateDate = DateTime.Now;
                    item.UpdateBy = request.Inputer;
                    var save = await _context.UpdateSave(item);
                    if (save.Success)
                    {
                        result.OK();
                        if (request.Photo != null)
                        {
                            if (_helper.IsImage(request.Photo.Filename))
                            {
                                result = await _mediator.Send(new EditMediaRequest()
                                {
                                    File = request.Photo,
                                    Inputer = request.Inputer,
                                    Modul = item.Id.ToString(),
                                    Tipe = "PHOTO_PROGRAM",
                                    Height = 300,
                                    Width = 300
                                });
                            }
                        }
                    }
                    else
                        result.BadRequest(save.Message);

                    return result;
                }
                else
                    result.NotFound($"Id Program {request.Id} Tidak Ditemukan");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Edit Program", request);
                result.Error("Failed Edit Program", ex.Message);
            }
            return result;
        }
    }
}

