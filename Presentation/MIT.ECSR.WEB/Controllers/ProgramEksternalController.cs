using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MIT.ECSR.WEB.Controllers
{
    public class ProgramEksternalController : BaseController<ProgramEksternalController>
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
