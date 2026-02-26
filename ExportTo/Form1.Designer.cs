namespace ExportTo
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            button1 = new Button();
            buttonImport = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 12);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(526, 389);
            dataGridView1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(585, 286);
            button1.Name = "button1";
            button1.Size = new Size(182, 60);
            button1.TabIndex = 1;
            button1.Text = "ExportarPDF";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Button1_Click;
            // 
            // buttonImport
            // 
            buttonImport.Location = new Point(585, 12);
            buttonImport.Name = "buttonImport";
            buttonImport.Size = new Size(182, 60);
            buttonImport.TabIndex = 2;
            buttonImport.Text = "Importar CSV";
            buttonImport.UseVisualStyleBackColor = true;
            buttonImport.Click += ButtonImport_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonImport);
            Controls.Add(button1);
            Controls.Add(dataGridView1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private Button button1;
        private Button buttonImport;
    }
}