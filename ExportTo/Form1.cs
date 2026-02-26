namespace ExportTo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button1.Click += Button1_Click;
            buttonImport.Click += ButtonImport_Click;
        }

        private void ButtonImport_Click(object sender, EventArgs e)
        {
            using var openDialog = new OpenFileDialog
            {
                Title = "Seleccionar CSV",
                Filter = "Archivos CSV (*.csv)|*.csv|Todos los archivos (*.*)|*.*",
                DefaultExt = "csv"
            };

            if (openDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                Export.ImportCsv(dataGridView1, openDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al importar el CSV:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            // Obtener el DataTable desde el DataSource del DGV
            var table = dataGridView1.DataSource as System.Data.DataTable;

            if (table == null)
            {
                MessageBox.Show("No hay datos para exportar.\nAsegúrate de cargar un CSV primero.",
                    "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var saveDialog = new SaveFileDialog
            {
                Title = "Guardar PDF",
                Filter = "Archivos PDF (*.pdf)|*.pdf",
                FileName = "exportacion.pdf",
                DefaultExt = "pdf"
            };

            if (saveDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                Export.ExportToPdf(table, saveDialog.FileName);
                MessageBox.Show("PDF exportado correctamente.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar el PDF:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}