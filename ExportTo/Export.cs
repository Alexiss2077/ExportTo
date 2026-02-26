using System.Data;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace ExportTo
{
    // ── Font Resolver 
    internal class WindowsFontResolver : IFontResolver
    {
        public string DefaultFontName => "Arial";

        public byte[] GetFont(string faceName)
        {
            string fontsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
            string file = faceName switch
            {
                "Arial-Bold" => Path.Combine(fontsFolder, "arialbd.ttf"),
                _ => Path.Combine(fontsFolder, "arial.ttf")
            };
            return File.ReadAllBytes(file);
        }

        public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
            => new FontResolverInfo(bold ? "Arial-Bold" : "Arial");
    }

    // ── Export 
    internal class Export
    {
        static Export()
        {
            if (GlobalFontSettings.FontResolver == null)
                GlobalFontSettings.FontResolver = new WindowsFontResolver();
        }

        public static void ExportToPdf(DataTable table, string filePath)
        {
            if (table == null || table.Columns.Count == 0) return;

            using var doc = new PdfDocument();
            doc.Info.Title = "Exportacion";

            var headerFont = new XFont("Arial", 10, XFontStyleEx.Bold);
            var cellFont = new XFont("Arial", 9, XFontStyleEx.Regular);
            var headerBrush = new XSolidBrush(XColor.FromArgb(52, 73, 94));
            var altBrush = new XSolidBrush(XColor.FromArgb(236, 240, 241));
            var borderPen = new XPen(XColor.FromArgb(189, 195, 199), 0.5);

            const double marginLeft = 30;
            const double marginTop = 30;
            const double rowHeight = 20;
            const double padding = 5;

            PdfPage page = doc.AddPage();
            page.Orientation = table.Columns.Count > 5
                ? PdfSharp.PageOrientation.Landscape
                : PdfSharp.PageOrientation.Portrait;

            double colWidth = (page.Width - marginLeft * 2) / table.Columns.Count;
            int rowsPerPage = (int)Math.Floor((page.Height - marginTop * 2 - rowHeight) / rowHeight);

            int rowIndex = 0;
            int pageIndex = 0;

            while (pageIndex == 0 || rowIndex < table.Rows.Count)
            {
                if (pageIndex > 0)
                {
                    page = doc.AddPage();
                    page.Orientation = table.Columns.Count > 5
                        ? PdfSharp.PageOrientation.Landscape
                        : PdfSharp.PageOrientation.Portrait;
                }

                using var gfx = XGraphics.FromPdfPage(page);
                double y = marginTop;

                // Cabecera
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    double x = marginLeft + i * colWidth;
                    gfx.DrawRectangle(headerBrush, x, y, colWidth, rowHeight);
                    gfx.DrawRectangle(borderPen, x, y, colWidth, rowHeight);
                    gfx.DrawString(table.Columns[i].ColumnName, headerFont, XBrushes.White,
                        new XRect(x + padding, y, colWidth - padding * 2, rowHeight),
                        XStringFormats.CenterLeft);
                }
                y += rowHeight;

                // Filas
                bool alternate = false;
                int drawn = 0;

                while (rowIndex < table.Rows.Count && drawn < rowsPerPage)
                {
                    DataRow row = table.Rows[rowIndex];
                    var bg = alternate ? (XBrush)altBrush : XBrushes.White;

                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        double x = marginLeft + i * colWidth;
                        string val = row[i]?.ToString() ?? "";
                        gfx.DrawRectangle(bg, x, y, colWidth, rowHeight);
                        gfx.DrawRectangle(borderPen, x, y, colWidth, rowHeight);
                        gfx.DrawString(val, cellFont, XBrushes.Black,
                            new XRect(x + padding, y, colWidth - padding * 2, rowHeight),
                            XStringFormats.CenterLeft);
                    }

                    y += rowHeight;
                    rowIndex++;
                    drawn++;
                    alternate = !alternate;
                }

                pageIndex++;
                if (rowIndex >= table.Rows.Count) break;
            }

            doc.Save(filePath);
        }

        public static void ImportCsv(DataGridView dgv, string filePath)
        {
            var lines = File.ReadAllLines(filePath, System.Text.Encoding.UTF8);
            if (lines.Length == 0) return;

            var dt = new DataTable();

            // Primera línea → columnas
            var headers = SplitCsvLine(lines[0]);
            foreach (var header in headers)
                dt.Columns.Add(header.Trim());

            // Resto → filas
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                var fields = SplitCsvLine(lines[i]);

                while (fields.Count < dt.Columns.Count)
                    fields.Add(string.Empty);

                dt.Rows.Add(fields.Take(dt.Columns.Count).ToArray<object>());
            }

            // Asignar el DataTable como fuente del DGV
            dgv.DataSource = dt;
        }

        private static List<string> SplitCsvLine(string line)
        {
            var fields = new List<string>();
            var current = new System.Text.StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    { current.Append('"'); i++; }
                    else { inQuotes = !inQuotes; }
                }
                else if (c == ',' && !inQuotes)
                { fields.Add(current.ToString()); current.Clear(); }
                else { current.Append(c); }
            }

            fields.Add(current.ToString());
            return fields;
        }
    }
}