namespace ExportTo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button1.Click += Button1_Click;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            using var saveDialog = new SaveFileDialog
            {
                Title = "Guardar PDF",
                Filter = "Archivos PDF (*.pdf)|*.pdf",
                FileName = "exportacion.pdf",
                DefaultExt = "pdf"
            };

            if (saveDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                Export.ExportToPdf(dataGridView1, saveDialog.FileName);
                MessageBox.Show(
                    "PDF exportado correctamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al exportar el PDF:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}