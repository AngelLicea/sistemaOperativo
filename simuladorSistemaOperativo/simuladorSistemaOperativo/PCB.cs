using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simuladorSistemaOperativo
{
    public partial class PCB : Form
    {
        int cont = 0;
        public PCB()
        {
            InitializeComponent();
            actualizarFilitas();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
        }
        public void actualizarFilitas()
        {
            for(int i = cont; i < Form1.nombre; i++)
            {
                this.dataGridView1.Rows.Add(Form1.nombres[i], Form1.horaLleg[i], Form1.duracion[i], Form1.acum[i], Form1.usoImpr[i], Form1.horaImpr[i], Form1.tiempoImpr[i], Form1.tamano[i]);
            }
            cont = Form1.nombre;
            foreach (DataGridViewRow myRow in dataGridView1.Rows)
            {
                myRow.Cells[3].Value = null;
            }
            for(int i = 0; i < cont; i++)
            {
                this.dataGridView1.Rows[i].Cells[3].Value = Form1.acum[i];
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            actualizarFilitas();
        }
    }
}
