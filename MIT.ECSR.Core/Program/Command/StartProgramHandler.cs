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
    public class StartProgramRequest : IRequest<StatusResponse>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Inputer { get; set; }
    }
    #endregion

    internal class StartProgramHandler : IRequestHandler<StartProgramRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IGeneralHelper _helper;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public StartProgramHandler(
            ILogger<StartProgramHandler> logger,
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
        public async Task<StatusResponse> Handle(StartProgramRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                var data = await _context.Entity<MIT.ECSR.Data.Model.TrsProgram>().Where(d => d.Id == request.Id).Include(d=>d.TrsProgramItem).FirstOrDefaultAsync();
                if (data != null)
                {
                    if (data.TrsProgramItem.Count()==0)
                    {
                        result.BadRequest("Cannot Be Start because Kegiatan harus diisi minimal 1 kegiatan!");
                        return result;
                    }

                    if (data.Status != (int)ProgramStatusEnum.OPEN)
                    {
                        result.BadRequest("Cannot Be Start because Status is " + ((ProgramStatusEnum)data.Status).ToString());
                        return result;
                    }
                    data.Status = (int)ProgramStatusEnum.ON_PROGRESS;
                    data.UpdateBy = request.Inputer;
                    data.UpdateDate = DateTime.Now;
                    var save = await _context.UpdateSave(data);
                    if (save.Success)
                        result.OK();
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

