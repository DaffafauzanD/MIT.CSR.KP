using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MIT.ECSR.Core.Report.Query;

namespace MIT.ECSR.API.Controllers
{
    public partial class ReportController : BaseController<ReportController>
    {
        [AllowAnonymous]
        [HttpGet(template: "top_company")]
        public async Task<IActionResult> TopCompany()
        {
            return Wrapper(await _mediator.Send(new GetTopCompanyRequest()));
        }
        [AllowAnonymous]
        [HttpGet(template: "rekap_home")]
        public async Task<IActionResult> RekapHome()
        {
            return Wrapper(await _mediator.Send(new GetProgramRekapRequest()));
        }
    }
}

