using Microsoft.AspNetCore.Mvc;
using MIT.ECSR.Web.Helper;

namespace MIT.ECSR.WEB.Controllers
{
    public class SettingController : BaseController<SettingController>
    {
        private readonly IRestAPIHelper _apiHelper;
        public SettingController(IRestAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public IActionResult User()
        {
            return View();
        }
        public IActionResult UserAdd()
        {
            return View();
        }
        [Route("Setting/UserEdit/{id?}")]
        public async Task<IActionResult> UserEdit(Guid id)
        {
            var detail = await _apiHelper.DetailUser(id);
            if (detail.Succeeded)
            {
                return View(detail.Data);
            }
            else
                return RedirectToAction("Index", "Home");
        }
        public IActionResult JenisProgram()
        {
            return View();
        }
        public IActionResult Banner()
        {
            return View();
        }
    }
}
