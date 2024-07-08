using Microsoft.AspNetCore.Mvc;
using MIT.ECSR.Web.Helper;

namespace MIT.ECSR.WEB.Controllers
{
    public class MonitoringController : BaseController<MonitoringController>
    {
        private readonly IRestAPIHelper _apiHelper;
        public MonitoringController(IRestAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Monitoring/DetailEksternal/{id?}")]
        public async Task<IActionResult> DetailEksternal(Guid id)
        {
            var detailProgram = await _apiHelper.DetailProgram(id);
            if (detailProgram.Succeeded)
            {
                var jenis = await _apiHelper.ListJenisProgram();
                var dati = await _apiHelper.ListDati();
                ViewBag.JenisProgram = jenis.List;
                ViewBag.Dati = dati.List;
                return View(detailProgram.Data);
            }
            else
                return RedirectToAction("Index", "Home");
        }
    }
}
