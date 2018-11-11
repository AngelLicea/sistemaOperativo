namespace simuladorSistemaOperativo
{
    partial class PCB
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.idColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hrLlegadaColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DuracionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.acumColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usaImprColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.horaUsoIOColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tiempoImprColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.ColumnSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idColumn,
            this.hrLlegadaColumn,
            this.DuracionColumn,
            this.acumColumn,
            this.usaImprColumn,
            this.horaUsoIOColumn,
            this.tiempoImprColumn,
            this.ColumnSize});
            this.dataGridView1.Location = new System.Drawing.Point(13, 13);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(847, 289);
            this.dataGridView1.TabIndex = 0;
            // 
            // idColumn
            // 
            this.idColumn.HeaderText = "ID";
            this.idColumn.Name = "idColumn";
            this.idColumn.ReadOnly = true;
            // 
            // hrLlegadaColumn
            // 
            this.hrLlegadaColumn.HeaderText = "Hora de llegada";
            this.hrLlegadaColumn.Name = "hrLlegadaColumn";
            this.hrLlegadaColumn.ReadOnly = true;
            // 
            // DuracionColumn
            // 
            this.DuracionColumn.HeaderText = "Duracion";
            this.DuracionColumn.Name = "DuracionColumn";
            this.DuracionColumn.ReadOnly = true;
            // 
            // acumColumn
            // 
            this.acumColumn.HeaderText = "Acumulado";
            this.acumColumn.Name = "acumColumn";
            this.acumColumn.ReadOnly = true;
            // 
            // usaImprColumn
            // 
            this.usaImprColumn.HeaderText = "Uso de impresora";
            this.usaImprColumn.Name = "usaImprColumn";
            this.usaImprColumn.ReadOnly = true;
            // 
            // horaUsoIOColumn
            // 
            this.horaUsoIOColumn.HeaderText = "Hora de uso I/O";
            this.horaUsoIOColumn.Name = "horaUsoIOColumn";
            this.horaUsoIOColumn.ReadOnly = true;
            // 
            // tiempoImprColumn
            // 
            this.tiempoImprColumn.HeaderText = "Tiempo de impresion";
            this.tiempoImprColumn.Name = "tiempoImprColumn";
            this.tiempoImprColumn.ReadOnly = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(439, 322);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Ocultar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(358, 322);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Actualizar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ColumnSize
            // 
            this.ColumnSize.HeaderText = "Tamano";
            this.ColumnSize.Name = "ColumnSize";
            this.ColumnSize.ReadOnly = true;
            // 
            // PCB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 357);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "PCB";
            this.Text = "PCB";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewTextBoxColumn idColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hrLlegadaColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DuracionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn acumColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn usaImprColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn horaUsoIOColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tiempoImprColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSize;
    }
}