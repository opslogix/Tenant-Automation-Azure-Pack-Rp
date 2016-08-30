namespace Database.CustomAction.UI
{
    partial class frmPaths
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPaths));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.lblTitilo = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkSamePath = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.dgvPaths = new System.Windows.Forms.DataGridView();
            this.dgvColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColPathDialog = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPaths)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.lblDescripcion);
            this.panel1.Controls.Add(this.lblTitilo);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(696, 62);
            this.panel1.TabIndex = 2;
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescripcion.Location = new System.Drawing.Point(27, 29);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(566, 15);
            this.lblDescripcion.TabIndex = 2;
            this.lblDescripcion.Text = "If you don’t set up a folder path, use the first row (uncheck box on the bottom t" +
    "o set up new routes/paths)";
            // 
            // lblTitilo
            // 
            this.lblTitilo.AutoSize = true;
            this.lblTitilo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitilo.Location = new System.Drawing.Point(15, 6);
            this.lblTitilo.Name = "lblTitilo";
            this.lblTitilo.Size = new System.Drawing.Size(118, 15);
            this.lblTitilo.TabIndex = 1;
            this.lblTitilo.Text = "Installation Paths";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Database.CustomAction.Properties.Resources.search4files;
            this.pictureBox1.Location = new System.Drawing.Point(636, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chkSamePath);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOk);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 375);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(696, 47);
            this.panel2.TabIndex = 4;
            // 
            // chkSamePath
            // 
            this.chkSamePath.AutoSize = true;
            this.chkSamePath.Checked = true;
            this.chkSamePath.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSamePath.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.chkSamePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSamePath.Location = new System.Drawing.Point(18, 12);
            this.chkSamePath.Name = "chkSamePath";
            this.chkSamePath.Size = new System.Drawing.Size(410, 17);
            this.chkSamePath.TabIndex = 2;
            this.chkSamePath.Text = "Use the same folder path (as the First row,  ignore rest of the rows) ";
            this.chkSamePath.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(607, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(77, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(482, 12);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(93, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "&Accept";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // dgvPaths
            // 
            this.dgvPaths.AllowUserToAddRows = false;
            this.dgvPaths.AllowUserToDeleteRows = false;
            this.dgvPaths.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPaths.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvColName,
            this.dgvColDescription,
            this.dgvColPath,
            this.dgvColPathDialog});
            this.dgvPaths.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPaths.Location = new System.Drawing.Point(0, 62);
            this.dgvPaths.Name = "dgvPaths";
            this.dgvPaths.Size = new System.Drawing.Size(696, 313);
            this.dgvPaths.TabIndex = 5;
            this.dgvPaths.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPaths_CellClick);
            // 
            // dgvColName
            // 
            this.dgvColName.HeaderText = "Name";
            this.dgvColName.Name = "dgvColName";
            this.dgvColName.ReadOnly = true;
            this.dgvColName.Width = 200;
            // 
            // dgvColDescription
            // 
            this.dgvColDescription.HeaderText = "Description";
            this.dgvColDescription.Name = "dgvColDescription";
            this.dgvColDescription.ReadOnly = true;
            this.dgvColDescription.Width = 200;
            // 
            // dgvColPath
            // 
            this.dgvColPath.HeaderText = "Path";
            this.dgvColPath.Name = "dgvColPath";
            this.dgvColPath.Width = 200;
            // 
            // dgvColPathDialog
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(1);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DarkGray;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dgvColPathDialog.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvColPathDialog.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.dgvColPathDialog.HeaderText = "...";
            this.dgvColPathDialog.Name = "dgvColPathDialog";
            this.dgvColPathDialog.Text = "...";
            this.dgvColPathDialog.ToolTipText = "Seleccionar carpeta";
            this.dgvColPathDialog.UseColumnTextForButtonValue = true;
            this.dgvColPathDialog.Width = 50;
            // 
            // frmPaths
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(696, 422);
            this.Controls.Add(this.dgvPaths);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPaths";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Installation paths";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPaths)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.Label lblTitilo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.DataGridView dgvPaths;
        private System.Windows.Forms.CheckBox chkSamePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColPath;
        private System.Windows.Forms.DataGridViewButtonColumn dgvColPathDialog;

    }
}