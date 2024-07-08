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
using Hangfire;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace MIT.ECSR.Core.Program.Command
{

    #region Request
    public class AddProgramMapping : Profile
    {
        public AddProgramMapping()
        {
            CreateMap<AddProgramRequest, ProgramRequest>().ReverseMap();
        }
    }
    public class AddProgramRequest : ProgramRequest, IMapRequest<TrsProgram, AddProgramRequest>, IRequest<ObjectResponse<Guid>>
    {
        [Required]
        public string Inputer { get; set; }
        public void Mapping(IMappingExpression<AddProgramRequest, TrsProgram> map)
        {
            map.ForMember(d => d.EndTglPelaksanaan, opt => opt.MapFrom(s => s.EndPelaksanaan))
                .ForMember(d => d.StartTglPelaksanaan, opt => opt.MapFrom(s => s.StartPelaksanaan))
                .ForMember(d => d.EndProgramKerja, opt => opt.MapFrom(s => s.BatasWaktuProgram));
        }
    }
    #endregion

    internal class AddProgramHandler : IRequestHandler<AddProgramRequest, ObjectResponse<Guid>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IGeneralHelper _helper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        private readonly IBackgroundJobClient _job;

        public AddProgramHandler(
            ILogger<AddProgramHandler> logger,
            IMapper mapper,
            IMediator mediator,
            IGeneralHelper helper,
            IUnitOfWork<ApplicationDBContext> context,
            IBackgroundJobClient job)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _helper = helper;
            _context = context;
            _job = job;
        }
        public async Task<ObjectResponse<Guid>> Handle(AddProgramRequest request, CancellationToken cancellationToken)
        {
            ObjectResponse<Guid> result = new ObjectResponse<Guid>();
            try
            {
                var data = _mapper.Map<TrsProgram>(request);
                data.Id = Guid.NewGuid();
                data.Status = (short)ProgramStatusEnum.DRAFT;
                data.CreateDate = DateTime.Now;
                data.UpdateDate = DateTime.Now;
                data.CreateBy = request.Inputer;
                data.UpdateBy = request.Inputer;
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
                                Tipe = "PHOTO_PROGRAM",
                                Height = 300,
                                Width = 300
                            });

                            if (result.Succeeded)
                                result.Data = data.Id;
                        }
                    }
                }
                else
                    result.BadRequest(save.Message);

                if (result.Succeeded)
                {
                    _job.Schedule(() => UpdateProgramDate(data.Id), data.EndProgramKerja);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Add Program", request);
                result.Error("Failed Add Program", ex.Message);
            }
            return result;
        }

        public void UpdateProgramDate(Guid id)
        {
            var program = _context.Entity<TrsProgram>().FirstOrDefault(x => x.Id == id);
            if (program != null)
            {
                if (program.Status == (short)ProgramStatusEnum.OPEN)
                {
                    program.StartTglPelaksanaan = program.StartTglPelaksanaan.AddYears(1);
                    program.EndTglPelaksanaan = program.EndTglPelaksanaan.AddYears(1);
                    program.EndProgramKerja = program.EndProgramKerja.AddYears(1);
                    _context.UpdateSave(program);
                }
                else if (program.Status == (short)ProgramStatusEnum.ON_PROGRESS)
                {
                    var listProgramItem = _context.Entity<TrsProgramItem>().Where(x => x.IdProgram == program.Id)
                        .Include(x => x.TrsPenawaranItem).ToList();
                    var countJumlahProgram = listProgramItem.Sum(x => x.Jumlah);
                    var countSubmitProgram = listProgramItem.Where(x => x.TrsPenawaranItem.Any(z => z.Status == (int)PenawaranStatusEnum.SUBMIT)).Select(x => x.TrsPenawaranItem.Sum(z => z.Jumlah)).Sum(x => x);
                    if ((countJumlahProgram - countSubmitProgram) > 0)
                    {
                        program.StartTglPelaksanaan = program.StartTglPelaksanaan.AddYears(1);
                        program.EndTglPelaksanaan = program.EndTglPelaksanaan.AddYears(1);
                        program.EndProgramKerja = program.EndProgramKerja.AddYears(1);
                        _context.UpdateSave(program);
                    }
                }
            }
        }
    }
}

