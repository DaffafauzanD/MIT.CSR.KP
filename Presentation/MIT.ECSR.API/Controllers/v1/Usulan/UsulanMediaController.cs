using Microsoft.AspNetCore.Mvc;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.Media.Query;
using MIT.ECSR.Core.Request;
using MIT.ECSR.Core.Media.Command;
using MIT.ECSR.Core.Usulan.Command;

namespace MIT.ECSR.API.Controllers
{
    public partial class UsulanMediaController : BaseController<UsulanMediaController>
    {
        [HttpGet(template: "list/{id_usulan}")]
        public async Task<IActionResult> List(Guid id_usulan, int? start, int? length)
        {
            return Wrapper(await _mediator.Send(new GetMediaListRequest()
            {
                Filter = new List<FilterRequest>()
                {
                    new FilterRequest()
                    {
                        Field = "modul",
                        Search = id_usulan.ToString()
                    },
                    new FilterRequest()
                    {
                        Field = "tipe",
                        Search = "USULAN_LAMPIRAN"
                    },
                },
                Sort = new SortRequest()
                {
                    Field = "createdate",
                    Type = SortTypeEnum.DESC
                },
                Start = start,
                Length = length,
            }));
        }

        [HttpPost(template: "upload/{id_usulan}")]
        public async Task<IActionResult> Upload(Guid id_usulan, [FromBody] FileObject request)
        {
            return Wrapper(await _mediator.Send(new UploadMediaRequest()
            {
                File = request,
                Inputer = Token.User.Username,
                Modul = id_usulan.ToString(),
                Tipe = "USULAN_LAMPIRAN"
            }));
        }
        [HttpPost(template: "photo/{id_usulan}")]
        public async Task<IActionResult> UploadPhoto(Guid id_usulan, [FromBody] FileObject request)
        {
            return Wrapper(await _mediator.Send(new UploadPhotoUsulanRequest()
            {
                Id = id_usulan,
                Photo = request,
                Inputer = Token.User.Username
            }));
        }

        [HttpDelete(template: "delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Wrapper(await _mediator.Send(new DeleteMediaRequest() { Id = id }));
        }

        [HttpGet(template: "download/{id}")]
        public async Task<IActionResult> Download(Guid id)
        {
            return Wrapper(await _mediator.Send(new DownloadMediaRequest() { Id = id }));
        }
        [HttpGet(template: "url/{tipe}/{modul}")]
        public async Task<IActionResult> GetUrl(string tipe, string modul)
        {
            return Wrapper(await _mediator.Send(new GetMediaUrlRequest() { Modul = modul, Tipe = tipe }));
        }
    }
}

