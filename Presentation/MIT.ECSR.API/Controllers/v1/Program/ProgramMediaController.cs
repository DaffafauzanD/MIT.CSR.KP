using Microsoft.AspNetCore.Mvc;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.Media.Query;
using MIT.ECSR.Core.Request;
using MIT.ECSR.Core.Media.Command;
using MIT.ECSR.Core.Program.Command;
using Microsoft.AspNetCore.Authorization;

namespace MIT.ECSR.API.Controllers
{
    public partial class ProgramMediaController : BaseController<ProgramMediaController>
    {
        [AllowAnonymous]
        [HttpGet(template: "list/{id_program}")]
        public async Task<IActionResult> List(Guid id_program, int? start, int? length)
        {
            return Wrapper(await _mediator.Send(new GetMediaListRequest()
            {
                Filter = new List<FilterRequest>()
                {
                    new FilterRequest()
                    {
                        Field = "modul",
                        Search = id_program.ToString()
                    },
                    new FilterRequest()
                    {
                        Field = "tipe",
                        Search = "PROGRAM_LAMPIRAN"
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

        [HttpPost(template: "upload/{id_program}")]
        public async Task<IActionResult> Upload(Guid id_program, [FromBody] FileObject request)
        {
            return Wrapper(await _mediator.Send(new UploadMediaRequest()
            {
                File = request,
                Inputer = Token.User.Username,
                Modul = id_program.ToString(),
                Tipe = "PROGRAM_LAMPIRAN"
            }));
        }
        [HttpPost(template: "photo/{id_program}")]
        public async Task<IActionResult> UploadPhoto(Guid id_program, [FromBody] FileObject request)
        {
            return Wrapper(await _mediator.Send(new UploadPhotoProgramRequest()
            {
                Id = id_program,
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

