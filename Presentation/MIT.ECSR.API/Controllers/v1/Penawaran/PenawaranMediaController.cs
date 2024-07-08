using Microsoft.AspNetCore.Mvc;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.Media.Query;
using MIT.ECSR.Core.Request;
using MIT.ECSR.Core.Media.Command;

namespace MIT.ECSR.API.Controllers
{
    public partial class PenawaranMediaController : BaseController<PenawaranMediaController>
    {
        [HttpGet(template: "list/{id_penawaran}")]
        public async Task<IActionResult> List(Guid id_penawaran, int? start, int? length)
        {
            return Wrapper(await _mediator.Send(new GetMediaListRequest()
            {
                Filter = new List<FilterRequest>()
                {
                    new FilterRequest()
                    {
                        Field = "modul",
                        Search = id_penawaran.ToString()
                    },
                    new FilterRequest()
                    {
                        Field = "tipe",
                        Search = "PENAWARAN_LAMPIRAN"
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

        [HttpPost(template: "upload/{id_penawaran}")]
        public async Task<IActionResult> Upload(Guid id_penawaran, [FromBody] FileObject request)
        {
            return Wrapper(await _mediator.Send(new UploadMediaRequest()
            {
                File = request,
                Inputer = Token.User.Username,
                Modul = id_penawaran.ToString(),
                Tipe = "PENAWARAN_LAMPIRAN"
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

