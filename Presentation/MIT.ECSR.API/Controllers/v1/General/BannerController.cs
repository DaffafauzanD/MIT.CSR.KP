using Microsoft.AspNetCore.Mvc;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.User.Query;
using MIT.ECSR.Core.User.Command;
using Microsoft.AspNetCore.Authorization;
using MIT.ECSR.Core.Request;
using MIT.ECSR.Core.Role.Query;
using MIT.ECSR.Core.Media.Command;
using MIT.ECSR.Core.Media.Query;

namespace MIT.ECSR.API.Controllers
{
    public partial class BannerController : BaseController<BannerController>
    {
        [AllowAnonymous]
        [HttpGet(template: "list")]
        public async Task<IActionResult> List(int? start, int? length)
        {
            return Wrapper(await _mediator.Send(new GetMediaListRequest()
            {
                Filter = new List<FilterRequest>()
                {
                    new FilterRequest()
                    {
                        Field = "modul",
                        Search = "BANNER"
                    },
                    new FilterRequest()
                    {
                        Field = "tipe",
                        Search = "BANNER"
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

        [HttpPost(template: "upload")]
        public async Task<IActionResult> Upload([FromBody] FileObject request)
        {
            return Wrapper(await _mediator.Send(new UploadMediaRequest()
            {
                File = request,
                Inputer = Token.User.Username,
                Modul = "BANNER",
                Tipe = "BANNER",
                Height = 500,
                Width = 1500
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
    }
}

