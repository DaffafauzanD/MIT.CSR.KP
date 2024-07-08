using AutoMapper;
using MediatR;
using WonderKid.DAL.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Data;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.Helper;
using MIT.ECSR.Core.Request;
using MIT.ECSR.Data.Model;
using MIT.ECSR.Core.Media.Command;
using MIT.ECSR.Shared.Interface;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MIT.ECSR.Core.Usulan.Command
{

    #region Request
    public class AddUsulanMapping : Profile
    {
        public AddUsulanMapping()
        {
            CreateMap<AddUsulanRequest, UsulanRequest>().ReverseMap();
        }
    }
    public class AddUsulanRequest : UsulanRequest, IMapRequest<TrsUsulan, AddUsulanRequest>, IRequest<ObjectResponse<Guid>>
    {
        [Required]
        public string Inputer { get; set; }
        [Required]
        public Guid IdPerusahaan { get; set; }
        public void Mapping(IMappingExpression<AddUsulanRequest, TrsUsulan> map)
        {
            map.ForMember(d => d.EndTglPelaksanaan, opt => opt.MapFrom(s => s.EndPelaksanaan))
                .ForMember(d => d.StartTglPelaksanaan, opt => opt.MapFrom(s => s.StartPelaksanaan));
        }
    }
    #endregion

    internal class AddUsulanHandler : IRequestHandler<AddUsulanRequest, ObjectResponse<Guid>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IGeneralHelper _helper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public AddUsulanHandler(
            ILogger<AddUsulanHandler> logger,
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
        public async Task<ObjectResponse<Guid>> Handle(AddUsulanRequest request, CancellationToken cancellationToken)
        {
            ObjectResponse<Guid> result = new ObjectResponse<Guid>();
            try
            {
                var data = _mapper.Map<TrsUsulan>(request);
                data.Id = Guid.NewGuid();
                data.Status = (short)UsulanStatusEnum.DRAFT;
                data.CreateDate = DateTime.Now;
                data.CreateBy = request.Inputer;
                data.IdPerusahaan = request.IdPerusahaan;

                var save = await _context.AddSave(data);
                if (save.Success)
                {
                    result.OK();
                    result.Data = data.Id;
                    if (request.Photo != null)
                    {
                        if (_helper.IsImage(request.Photo.Filename))
                        {
                            result = await _mediator.Send(new UploadMediaRequest()
                            {
                                File = request.Photo,
                                Inputer = request.Inputer,
                                Modul = data.Id.ToString(),
                                Tipe = "PHOTO_USULAN",
                                Height = 300,
                                Width = 300
                            });
                        }
                    }
                }
                else
                    result.BadRequest(save.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Add Usulan", request);
                result.Error("Failed Add Usulan", ex.Message);
            }
            return result;
        }
    }
}

