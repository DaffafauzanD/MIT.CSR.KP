using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MIT.ECSR.Core.Helper;
using MIT.ECSR.Core.Program.Query;
using MIT.ECSR.Core.Response;
using MIT.ECSR.Data;
using MIT.ECSR.Data.Model;
using MIT.ECSR.Shared.Attributes;
using MIT.ECSR.Shared.Interface;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Style.XmlAccess;
using WonderKid.DAL.Interface;

namespace MIT.ECSR.Core.Program.Command
{
    public class ExportProgramRequest : ListRequest, IListRequest<ExportProgramRequest>, IRequest<ObjectResponse<byte[]>>
    {

    }
    internal class ExportProgramHandler : IRequestHandler<ExportProgramRequest, ObjectResponse<byte[]>>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        private readonly IGeneralHelper _helper;

        public ExportProgramHandler(
            ILogger<ExportProgramHandler> logger,
            IMediator mediator,
            IUnitOfWork<ApplicationDBContext> context,
            IGeneralHelper helper)
        {
            _logger = logger;
            _mediator = mediator;
            _context = context;
            _helper = helper;
        }
        public async Task<ObjectResponse<byte[]>> Handle(ExportProgramRequest request, CancellationToken cancellationToken)
        {
            var result = new ObjectResponse<byte[]>();
            
            try
            {
                var listProgram = await _mediator.Send(new GetProgramListRequest
                {
                    Filter = request.Filter,
                    Sort = request.Sort,
                    Length = int.MaxValue,
                    Start = 1
                });

                var idsProgram = JsonConvert.DeserializeObject<ListResponse<ProgramResponse>>(JsonConvert.SerializeObject(listProgram));
                if (idsProgram.Succeeded)
                {
                    var listIdsProgram = idsProgram.List.Select(x => x.Id).ToList();
                    var itemsProgram = _context.Entity<TrsProgram>()
                         .Include(x => x.LokasiNavigation)
                         .Include(x => x.NamaProgramNavigation)
                         .Include(x => x.IdJenisProgramNavigation)
                         .ThenInclude(x => x.IdSubProgramNavigation)
                         .ThenInclude(x => x.IdOpdNavigation)
                         .Include(x => x.TrsProgramItem)
                         .ThenInclude(x => x.TrsPenawaranItem)
                         .ThenInclude(x => x.IdPenawaranNavigation)
                         .ThenInclude(x => x.IdPerusahaanNavigation)
                         .Include(x => x.TrsProgramItem)
                         .ThenInclude(x => x.TrsProgresProgram)
                         .ThenInclude(x => x.IdPerusahaanNavigation)
                         .ToList()
                         .Where(x => listIdsProgram.Any(z => z == x.Id))
                         .GroupBy(x => x.IdJenisProgramNavigation.IdSubProgramNavigation.IdOpdNavigation.Id)
                         .Select(x => new
                         {
                             Opd = x.FirstOrDefault().IdJenisProgramNavigation.IdSubProgramNavigation.IdOpdNavigation.Name,
                             Items = x.ToList()
                         })
                         .ToList();

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage Ep = new ExcelPackage();

                    ExcelNamedStyleXml ns = Ep.Workbook.Styles.CreateNamedStyle("General");
                    ns.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ns.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ns.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ns.Style.Border.Top.Style = ExcelBorderStyle.Thin;

                    ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("PROGRAM");
                    Sheet.Cells["A1"].Value = "NO";
                    Sheet.Cells["B1"].Value = "BULAN";
                    Sheet.Cells["C1"].Value = "SUB KEGIATAN";
                    Sheet.Cells["D1"].Value = "LOKASI PENYALURAN";
                    Sheet.Cells["E1"].Value = "VOLUME";
                    Sheet.Cells["F1"].Value = "PAGU ANGGARAN";
                    Sheet.Cells["G1"].Value = "NAMA PERUSAHAAN";
                    Sheet.Cells["H1"].Value = "VOLUME BOOKING";
                    Sheet.Cells["I1"].Value = "AKTUAL ANGGARAN";
                    Sheet.Cells["J1"].Value = "KETERANGAN";
                    Sheet.Cells["A1:J1"].StyleName = "General";
                    Sheet.Cells["A1:J1"].AutoFitColumns();

                    int index = 2;
                    double totalAnggaran = 0;
                    double totalAnggaranPerusahaan = 0;
                    for (int i = 0; i < itemsProgram.Count(); i++)
                    {
                        var item = itemsProgram[i];
                        Sheet.Cells[$"A{index}"].Value = item.Opd;
                        Sheet.Cells[$"A{index}"].StyleName = "General";
                        Sheet.Cells[$"A{index}:J{index}"].StyleName = "General";
                        Sheet.Cells[$"A{index}:J{index}"].AutoFitColumns();
                        index++;

                        double sumPagu = 0;
                        double sumAktual = 0;
                        for (int x = 0; x < item.Items.Count; x++)
                        {
                            var program = item.Items[x];
                            Sheet.Cells[$"B{index}"].Value = program.NamaProgramNavigation.Name;
                            Sheet.Cells[$"B{index}"].StyleName = "General";
                            Sheet.Cells[$"A{index}:J{index}"].StyleName = "General";
                            Sheet.Cells[$"A{index}:J{index}"].AutoFitColumns();
                            index++;
                            for (int y = 0; y < program.TrsProgramItem.Count; y++)
                            {
                                var programItem = program.TrsProgramItem.ToList()[y];
                                for (int z = 0; z < programItem.TrsPenawaranItem.Count; z++)
                                {
                                    var penawaranItem = programItem.TrsPenawaranItem.ToList()[z];
                                    var progress = programItem.TrsProgresProgram?.Where(d => d.IdPerusahaan == penawaranItem.IdPenawaranNavigation.IdPerusahaan)?.OrderByDescending(d => d.Progress)?.FirstOrDefault()?.Progress ?? 0;

                                    Sheet.Cells[$"B{index}"].Value = programItem.StartTglPelaksanaan.ToString("MMMM-yyyy", new System.Globalization.CultureInfo("id-ID"));
                                    Sheet.Cells[$"C{index}"].Value = programItem.Nama;
                                    Sheet.Cells[$"D{index}"].Value = $"{program.LokasiNavigation.NamaDati4} - {programItem.Lokasi}";
                                    Sheet.Cells[$"E{index}"].Value = $"{programItem.Jumlah}";
                                    Sheet.Cells[$"F{index}"].Value = $"{_helper.DoubleToRupiah(programItem.Rupiah)}";
                                    sumPagu += programItem.Rupiah;
                                    Sheet.Cells[$"G{index}"].Value = $"{penawaranItem.IdPenawaranNavigation.IdPerusahaanNavigation.NamaPerusahaan}";
                                    Sheet.Cells[$"H{index}"].Value = $"{penawaranItem.Jumlah}";
                                    Sheet.Cells[$"I{index}"].Value = $"{_helper.DoubleToRupiah(penawaranItem.Rupiah ?? 0)}";
                                    sumAktual += penawaranItem.Rupiah ?? 0;
                                    Sheet.Cells[$"J{index}"].Value = $"{(ProgramStatusEnum)program.Status} - {progress}%";
                                    Sheet.Cells[$"A{index}:J{index}"].StyleName = "General";
                                    Sheet.Cells[$"A{index}:J{index}"].AutoFitColumns();
                                    totalAnggaran += programItem.Rupiah;
                                    totalAnggaranPerusahaan += penawaranItem.Rupiah ?? 0;
                                    index++;
                                }
                            }
                        }

                        Sheet.Cells[$"B{index}"].Value = "Sub Total OPD";
                        Sheet.Cells[$"F{index}"].Value = _helper.DoubleToRupiah(sumPagu);
                        Sheet.Cells[$"I{index}"].Value = _helper.DoubleToRupiah(sumAktual);
                        Sheet.Cells[$"A{index}:J{index}"].StyleName = "General";
                        Sheet.Cells[$"A{index}:J{index}"].AutoFitColumns();
                        index++;
                    }

                    Sheet.Cells[$"B{index}"].Value = "Sub Total";
                    Sheet.Cells[$"B{index}"].StyleName = "General";

                    Sheet.Cells[$"F{index}"].Value = $"{_helper.DoubleToRupiah(totalAnggaran)}";
                    Sheet.Cells[$"F{index}"].StyleName = "General";

                    Sheet.Cells[$"I{index}"].Value = $"{_helper.DoubleToRupiah(totalAnggaranPerusahaan)}";
                    Sheet.Cells[$"I{index}"].StyleName = "General";

                    Sheet.Cells[$"A{index}:J{index}"].StyleName = "General";
                    Sheet.Cells[$"A{index}:J{index}"].AutoFitColumns();

                    result.Data = Ep.GetAsByteArray();
                    result.OK();
                }
                else
                    result.BadRequest("Failed Export Program");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Export Program", request);
                result.Error("Failed Export Program", ex.Message);
            }

            return result;
        }
    }
}
