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
    public partial class FormDisk : Form
    {
        public FormDisk()
        {
            InitializeComponent();
            generarEspacios();
            actualizar();
        }
        public void generarEspacios()
        {
            dataGridView1.Rows.Clear();

            dataGridView1.RowTemplate.Height = 40;
            for (int i = 0; i < 6; i++)
            {
                this.dataGridView1.Rows.Add();
            }
        }
        public void actualizar()
        {
            double numerito;
            int aux;
            int cont;
            int j = 0;
            int k = 0;
            for (int i = 0; i < Form1.nombre; i++)
            {
                numerito = Convert.ToDouble(Form1.tamano[i]) / Convert.ToDouble(Form1.pageSize);
                aux = Convert.ToInt32(Math.Ceiling(numerito));
                if (i == (Form1.nombre - 1)) { aux = Convert.ToInt32(numerito) - 1; }

                cont = 0;
                bool terminoProc = false;
                for (int c = 0; c < Form1.contProcTerminados; c++)
                {
                    if (Form1.terminateProcessNumbers[c] == i)
                        terminoProc = true;
                }
                if (terminoProc == false)
                    while (j < 6)
                    {
                    for (; k < 6; k++)
                    {
                            this.dataGridView1.Rows[j].Cells[k].Value = i + "." + cont;
                        cont++;
                        if (cont > aux) { break; }
                    }
                        if (k == 6)
                        {
                            k = 0;
                            j++;
                        }
                        if (cont > aux) { break; }
                    }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            actualizar();
        }
    }
}
