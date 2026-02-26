using iText.Kernel.Pdf;
using iText.Kernel.Geom;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Borders;

namespace ExportTo
{
    internal class Export
    {
        /// <summary>
        /// Exporta el contenido de un DataGridView a un archivo PDF en la ruta indicada.
        /// </summary>
        public static void ExportToPdf(DataGridView dgv, string filePath)
        {
            PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            using var writer = new PdfWriter(filePath);
            using var pdf = new PdfDocument(writer);

            var pageSize = dgv.Columns.Count > 5 ? PageSize.A4.Rotate() : PageSize.A4;

            // Nombre completo para evitar ambigüedad con System.Reflection.Metadata.Document
            using var document = new iText.Layout.Document(pdf, pageSize);
            document.SetMargins(30, 30, 30, 30);

            var visibleColumns = dgv.Columns
                .Cast<DataGridViewColumn>()
                .Where(c => c.Visible)
                .OrderBy(c => c.DisplayIndex)
                .ToList();

            var table = new Table(UnitValue.CreatePercentArray(visibleColumns.Count))
                .UseAllAvailableWidth();

            // ── Cabeceras ──────────────────────────────────────────────
            foreach (var col in visibleColumns)
            {
                table.AddHeaderCell(
                    new Cell()
                        .SetBackgroundColor(new DeviceRgb(52, 73, 94))
                        .SetFontColor(ColorConstants.WHITE)
                        .SetFont(boldFont)           // negrita via fuente, no SetBold()
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetPadding(6)
                        .Add(new Paragraph(col.HeaderText))
                );
            }

            // ── Filas ──────────────────────────────────────────────────
            bool alternate = false;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                var bg = alternate
                    ? new DeviceRgb(236, 240, 241)
                    : ColorConstants.WHITE;

                foreach (var col in visibleColumns)
                {
                    var cellValue = row.Cells[col.Index].Value?.ToString() ?? string.Empty;
                    table.AddCell(
                        new Cell()
                            .SetBackgroundColor(bg)
                            .SetFont(normalFont)
                            .SetPadding(5)
                            .SetBorder(new SolidBorder(new DeviceRgb(189, 195, 199), 0.5f))
                            .Add(new Paragraph(cellValue))
                    );
                }

                alternate = !alternate;
            }

            document.Add(table);
        }
    }
}