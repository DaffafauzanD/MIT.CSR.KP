using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Core.Program.Object;
using MIT.ECSR.Data;
using MIT.ECSR.Data.Model;
using MIT.ECSR.Shared.Attributes;
using WonderKid.DAL.Interface;

namespace MIT.ECSR.Core.Program.Query
{
    public class GetProgramYearDashboardRequest : IRequest<ObjectResponse<ProgramYearDashboardResponse>>
    {
    }

    internal class GetProgramYearDashboardHandler : IRequestHandler<GetProgramYearDashboardRequest, ObjectResponse<ProgramYearDashboardResponse>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public GetProgramYearDashboardHandler(
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

        public async Task<ObjectResponse<ProgramYearDashboardResponse>> Handle(GetProgramYearDashboardRequest request, CancellationToken cancellationToken)
        {
            var result = new ObjectResponse<ProgramYearDashboardResponse>();
            try
            {
                var listStatus = new List<int> { 2, 3, 4 };
                var programYear = _context.Entity<TrsProgram>()
                    .Where(x => listStatus.Any(z => z == x.Status))
                    .GroupBy(x => x.StartTglPelaksanaan.Year).Select(x => x.Key).ToList();
                result.Data = new ProgramYearDashboardResponse
                {
                    Year = programYear
                };
                result.OK();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Get Year Dashboard Program");
                result.Error("Failed Get Year Dashboard Program", ex.Message);
            }
            return result;
        }
    }
}
