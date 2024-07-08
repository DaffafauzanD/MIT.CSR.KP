using Microsoft.AspNetCore.Mvc;
using MIT.ECSR.Web.Helper;

namespace MIT.ECSR.WEB.Controllers
{
    public class UsulanController : BaseController<UsulanController>
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
