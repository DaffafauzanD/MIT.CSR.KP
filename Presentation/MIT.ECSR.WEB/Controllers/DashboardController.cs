using Microsoft.AspNetCore.Mvc;
using MIT.ECSR.Web.Helper;

namespace MIT.ECSR.WEB.Controllers
{
    public class DashboardController : BaseController<DashboardController>
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult tutor()
        {
            return View();
        }
    }
}
