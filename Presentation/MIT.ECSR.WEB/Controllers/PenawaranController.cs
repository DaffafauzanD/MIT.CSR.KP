using Microsoft.AspNetCore.Mvc;
using MIT.ECSR.Web.Helper;

namespace MIT.ECSR.WEB.Controllers
{
    public class PenawaranController : BaseController<PenawaranController>
    {
        private readonly IRestAPIHelper _apiHelper;
        public PenawaranController(IRestAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }

        [Route("Penawaran/Detail/{id?}")]
        public async Task<IActionResult> Detail(Guid id)
        {
            var detail = await _apiHelper.DetailPenawaran(id, HelperClient.RawToken(HttpContext.Request));
            if (detail.Succeeded)
            {
                return View(detail.Data);
            }
            else
                return RedirectToAction("Index", "Home");

        }
    }
}
