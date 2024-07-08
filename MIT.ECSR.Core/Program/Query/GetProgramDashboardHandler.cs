using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Core.Helper;
using MIT.ECSR.Core.Program.Object;
using MIT.ECSR.Data;
using MIT.ECSR.Data.Model;
using MIT.ECSR.Shared.Attributes;
using System.ComponentModel.DataAnnotations;
using WonderKid.DAL.Interface;

namespace MIT.ECSR.Core.Program.Query
{
    public class GetProgramDashboardRequest : IRequest<ObjectResponse<ProgramDashboardResponse>>
    {
        [Required]
        public int Year { get; set; }
    }

    internal class GetProgramDashboardHandler : IRequestHandler<GetProgramDashboardRequest, ObjectResponse<ProgramDashboardResponse>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public GetProgramDashboardHandler(
            ILogger<GetProgramDashboardHandler> logger,
            IMapper mapper,
            IMediator mediator,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _context = context;
        }

        public async Task<ObjectResponse<ProgramDashboardResponse>> Handle(GetProgramDashboardRequest request, CancellationToken cancellationToken)
        {
            var result = new ObjectResponse<ProgramDashboardResponse>();
            try
            {
                result.Data = new ProgramDashboardResponse();
                var listStatus = new List<int> { 2, 3, 4 };

                var program = await _context.Entity<TrsProgram>().Where(x => x.StartTglPelaksanaan.Year == request.Year && listStatus.Any(z => z == x.Status)).ToListAsync();

                result.Data.DataPage = program.GroupBy(x => x.StartTglPelaksanaan.Month)
                    .OrderBy(x => x.Key)
                    .Select(x => x.FirstOrDefault()?.StartTglPelaksanaan.ToString("MMMM", new System.Globalization.CultureInfo("id-ID"))).ToList();

                result.Data.Items = program.GroupBy(x => x.Status).Select(x =>
                {
                    var item = new ProgramItemDashboardResponse
                    {
                        Name = ((ProgramStatusEnum)x.Key).ToString(),
                        Type = "bar",
                        Data = new List<int>()
                    };
                    foreach (var month in program.GroupBy(x => x.StartTglPelaksanaan.Month))
                    {
                        item.Data.Add(x.Count(d => d.StartTglPelaksanaan.Month == month.Key));
                    }
                    return item;
                }).ToList();
                result.OK();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Get Dashboard Program");
                result.Error("Failed Get Dashboard Program", ex.Message);
            }
            return result;
        }
    }
}
