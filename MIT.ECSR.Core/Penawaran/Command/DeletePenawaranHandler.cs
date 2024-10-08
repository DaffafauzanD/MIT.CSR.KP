//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using AutoMapper;
using MediatR;
using WonderKid.DAL.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MIT.ECSR.Data;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Core.Helper;
using MIT.ECSR.Core.Media.Command;
using MIT.ECSR.Data.Model;

namespace MIT.ECSR.Core.Penawaran.Command
{

    #region Request
    public class DeletePenawaranRequest : IRequest<StatusResponse>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Inputer { get; set; }
    }
    #endregion

    internal class DeletePenawaranHandler : IRequestHandler<DeletePenawaranRequest, StatusResponse>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public DeletePenawaranHandler(
            ILogger<DeletePenawaranHandler> logger,
            IMediator mediator,
            IUnitOfWork<ApplicationDBContext> context
            )
        {
            _logger = logger;
            _mediator = mediator;
            _context = context;
        }
        public async Task<StatusResponse> Handle(DeletePenawaranRequest request, CancellationToken cancellationToken)
        {
            StatusResponse result = new StatusResponse();
            try
            {
                var item = await _context.Entity<MIT.ECSR.Data.Model.TrsPenawaranItem>().Where(d => d.Id == request.Id).FirstOrDefaultAsync();
                if (item != null)
                {
                    bool is_single = await _context.Entity<MIT.ECSR.Data.Model.TrsPenawaranItem>().Where(d => d.Id == item.IdPenawaran).CountAsync() == 1;
                    if (item.Status != (int)PenawaranStatusEnum.SUBMIT)
                    {
                        result.BadRequest("Tidak dapat menghapus penawaran yang sudah di approve!");
                        return result;
                    }
                    if (is_single)
                    {
                        var penawaran = await _context.Entity<TrsPenawaran>().Where(d => d.Id == item.IdPenawaran).FirstOrDefaultAsync();
                        var delete = await _context.DeleteSave(penawaran);
                        if (delete.Success)
                        {
                            await _mediator.Send(new DeleteMediaByModulRequest() { Modul = item.IdPenawaran.ToString(), Tipe = "PENAWARAN_LAMPIRAN" });
                            result.OK();
                        }
                        else
                            result.BadRequest(delete.Message);
                    }
                    else
                    {
                        var delete = await _context.DeleteSave(item);
                        if (delete.Success)
                            result.OK();
                        else
                            result.BadRequest(delete.Message);
                    }
                }
                else
                    result.NotFound($"Id Penawaran {request.Id} Tidak Ditemukan");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Delete Penawaran", request.Id);
                result.Error("Failed Delete Penawaran", ex.Message);
            }
            return result;
        }
    }
}

