namespace Tri_Gei

{
    partial class Fen1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnTesterConnexion = new Button();
            dgvGei = new DataGridView();
            btnImportFichier = new Button();
            btnTrier = new Button();
            btnExporter = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvGei).BeginInit();
            SuspendLayout();
            // 
            // btnTesterConnexion
            // 
            btnTesterConnexion.Location = new Point(19, 20);
            btnTesterConnexion.Margin = new Padding(3, 2, 3, 2);
            btnTesterConnexion.Name = "btnTesterConnexion";
            btnTesterConnexion.Size = new Size(145, 22);
            btnTesterConnexion.TabIndex = 0;
            btnTesterConnexion.Text = "Tester la connexion";
            btnTesterConnexion.UseVisualStyleBackColor = true;
            btnTesterConnexion.Click += btnTesterConnexion_Click;
            // 
            // dgvGei
            // 
            dgvGei.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvGei.GridColor = SystemColors.MenuHighlight;
            dgvGei.Location = new Point(24, 59);
            dgvGei.Margin = new Padding(3, 2, 3, 2);
            dgvGei.MultiSelect = false;
            dgvGei.Name = "dgvGei";
            dgvGei.RowHeadersWidth = 51;
            dgvGei.Size = new Size(1025, 519);
            dgvGei.TabIndex = 1;
            // 
            // btnImportFichier
            // 
            btnImportFichier.Location = new Point(180, 20);
            btnImportFichier.Name = "btnImportFichier";
            btnImportFichier.Size = new Size(114, 23);
            btnImportFichier.TabIndex = 2;
            btnImportFichier.Text = "Import Fichier";
            btnImportFichier.UseVisualStyleBackColor = true;
            btnImportFichier.Click += btnImportFichier_Click;
            // 
            // btnTrier
            // 
            btnTrier.Location = new Point(315, 20);
            btnTrier.Name = "btnTrier";
            btnTrier.Size = new Size(100, 23);
            btnTrier.TabIndex = 3;
            btnTrier.Text = "Trier ";
            btnTrier.UseVisualStyleBackColor = true;
            btnTrier.Click += btnTrier_Click;
            // 
            // btnExporter
            // 
            btnExporter.Location = new Point(432, 20);
            btnExporter.Name = "btnExporter";
            btnExporter.Size = new Size(128, 23);
            btnExporter.TabIndex = 4;
            btnExporter.Text = "Exporter Resultat";
            btnExporter.UseVisualStyleBackColor = true;
            btnExporter.Click += btnExporter_Click;
            // 
            // Fen1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1061, 604);
            Controls.Add(btnExporter);
            Controls.Add(btnTrier);
            Controls.Add(btnImportFichier);
            Controls.Add(dgvGei);
            Controls.Add(btnTesterConnexion);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Fen1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dgvGei).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnTesterConnexion;
        private DataGridView dgvGei;
        private Button btnImportFichier;
        private Button btnTrier;
        private Button btnExporter;
    }
}
