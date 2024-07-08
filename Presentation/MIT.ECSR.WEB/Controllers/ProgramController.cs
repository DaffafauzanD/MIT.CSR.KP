using Microsoft.AspNetCore.Mvc;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Web.Helper;
using MIT.ECSR.Web.Model;

namespace MIT.ECSR.WEB.Controllers
{
    public class ProgramController : BaseController<ProgramController>
    {
        private readonly IRestAPIHelper _apiHelper;
        public ProgramController(IRestAPIHelper apiHelper)
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
        [Route("Program/Edit/{id?}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var detail = await _apiHelper.DetailProgram(id);
            if (detail.Succeeded)
            {
                var jenis = await _apiHelper.ListJenisProgram();
                var dati = await _apiHelper.ListDati();
                ViewBag.JenisProgram = jenis.List;
                ViewBag.Dati = dati.List;
                return View(detail.Data);
            }
            else
                return RedirectToAction("Index", "Home");

        }
        [Route("Program/Detail/{id?}")]
        public async Task<IActionResult> Detail(Guid id, int? tipe)
        {
            var detail = await _apiHelper.DetailProgram(id);
            ListResponse<object> items = new ListResponse<object>();
            ListResponse<ProgressProgramResponse> progress = new ListResponse<ProgressProgramResponse>();
            if (!tipe.HasValue)//public
            {
                items = await _apiHelper.ItemProgram(id, true, false);
                progress = await _apiHelper.ProgressProgram(id, false);
            }
            else
            {
                if (tipe == 1)//internal penawran
                {
                    items = await _apiHelper.ItemProgram(id, false, false);
                    progress = await _apiHelper.ProgressProgram(id, false);
                }
                if (tipe == 2)//external penawaran
                {
                    items = await _apiHelper.ItemProgram(id, false, true);
                    progress = await _apiHelper.ProgressProgram(id, true);
                }
            }
            if (detail.Succeeded && items.Succeeded)
            {
                var jenis = await _apiHelper.ListJenisProgram();
                var dati = await _apiHelper.ListDati();
                ViewBag.IsPublic = !tipe.HasValue;
                ViewBag.Item = items.List;
                ViewBag.Progress = progress.List;
                ViewBag.JenisProgram = jenis.List;
                ViewBag.Dati = dati.List;
                return View(detail.Data);
            }
            else
                    return RedirectToAction("Index", "Home");
         }
    }
}
