using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Threading;

namespace simuladorSistemaOperativo
{
    public partial class Form1 : Form
    {
        ayudita form2 = new ayudita();
        int estado = 3;
        int reloj = 0;
        int quantum = 0;
        int memSize = 48;
        int procSize = 24;
        double memoriaSO = 0;
        bool sistemaOperativo = false;
        Random crearproceso = new Random();
        public static string[] nombres = new string[500];
        public static int[] horaLleg = new int[500];
        public static int[] duracion = new int[500];
        public static int[] acum = new int[500];
        public static bool[] usoImpr = new bool[500];
        public static int[] horaImpr = new int[500];
        public static int[] tiempoImpr = new int[500];
        public static int[] tamano = new int[500];
        public static int[,] direccionesDeMemoria = new int[500, 12];
        public static string[,] direccionesDeMemoriaString = new string[500, 6];
        bool[,] espaciosDisponibles = new bool[6, 6];
        string[] memoriaRam;
        int[] horaDeLLegadaRAM = new int[500];
        int[] vecesDeUsoPagina = new int[500];
        int espacioRam = 0;
        int numFrames = 12;
        int difFrames = 0;
        public static int contProcTerminados = 0;
        public static int[] terminateProcessNumbers = new int[500];
        Queue<Proceso> terminated = new Queue<Proceso>();
        Queue<Proceso> hold = new Queue<Proceso>();
        Queue<Proceso> ready = new Queue<Proceso>();
        Queue<Proceso> running = new Queue<Proceso>();
        Queue<Proceso> waiting = new Queue<Proceso>();
        Queue<Proceso> printing = new Queue<Proceso>();
        Queue<Proceso> waitingDisk = new Queue<Proceso>();
        Queue<Proceso> usingDisk = new Queue<Proceso>();
        public static int nombre = 0;
        public static int espacioEnDisco = 0;
        public static int pageSize = 4;
        public Form1()
        {
            InitializeComponent();
            valoresDefault();
            formSize();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            definirEspacioRAM();
            formSize();
            estado = 1;
            while (estado == 1)
            {
                relojes();
                if (crearproceso.Next(0, 100) < trackBar1.Value)
                {
                    nuevoProceso();
                    tap();
                }
                if (hold.Count > 0 && sistemaOperativo == true)
                {
                    while (hold.Count > 0 && (ready.Count + waiting.Count + running.Count + printing.Count) < trackBar6.Value)
                    {
                        Thread.Sleep(300);
                        moverAReady();
                        Thread.Sleep(300);
                    }
                }
                if (running.Count > 0 && sistemaOperativo == true && running.First<Proceso>().acumEjecucion == running.First<Proceso>().duracion)
                {
                    moverATerminated();
                    tap();
                }
                if (printing.Count == 0 && waiting.Count > 0)
                {
                    Thread.Sleep(300);
                    moverAPrinting();
                    Thread.Sleep(300);
                }
                if (printing.Count > 0 && printing.First().tiempoImprimiendo == printing.First().tiempoEnImpresora)
                {
                    Thread.Sleep(300);
                    moverPrintingAReady();
                    Thread.Sleep(300);
                }
                if (running.Count > 0 && sistemaOperativo == true && running.First().usoImpresora == true && running.First().horaUsoImpresora == running.First().acumEjecucion)
                {
                    Thread.Sleep(300);
                    moverAWaiting();
                    Thread.Sleep(300);
                }
                if (running.Count > 0 && sistemaOperativo == true && ready.Count > 0)
                {
                    Thread.Sleep(300);
                    moverRunningAReady();
                    Thread.Sleep(300);
                }
                if (running.Count == 0 && ready.Count > 0 && sistemaOperativo == true)
                {
                    Thread.Sleep(300);
                    moverARunning();
                    Thread.Sleep(300);
                }
                if (running.Count > 0 && sistemaOperativo == true && radioButtonRoundRobin.Checked)
                {
                    if (!runningEstaEnRam())
                    {
                        Thread.Sleep(300);
                        moverAWaitingDisk();
                        Thread.Sleep(300);
                    }
                }
                if (usingDisk.Count == 0 && waitingDisk.Count > 0 && radioButtonRoundRobin.Checked)
                {
                    Thread.Sleep(300);
                    moverAUsingDisk();
                    if (cierreForzoso())
                    {
                        moverUsingDiskATerminated();
                        tap();
                    }
                    else
                        ponerPaginaEnRAM();
                    Thread.Sleep(300);
                }
                if (usingDisk.Count > 0 && usingDisk.First().tiempoUsandoDisco == usingDisk.First().tiempoEnDisco && radioButtonRoundRobin.Checked)
                {
                    Thread.Sleep(300);
                    moverUsingDiskAReady();
                    Thread.Sleep(300);
                }
                estadoSistemaOperativo();
                delay();
                reloj++;
                Application.DoEvents();
            }
        }
        public void moverUsingDiskATerminated()
        {
            terminated.Enqueue(usingDisk.Dequeue());
            dataGridView1.Rows.Add(terminated.Last<Proceso>().id, reloj - terminated.Last<Proceso>().horaLlegada, terminated.Last<Proceso>().duracion + terminated.Last<Proceso>().tiempoEnImpresora, reloj - terminated.Last<Proceso>().horaLlegada - terminated.Last<Proceso>().duracion - terminated.Last<Proceso>().tiempoEnImpresora, ((float)(terminated.Last<Proceso>().duracion + terminated.Last<Proceso>().tiempoEnImpresora) / (reloj - terminated.Last<Proceso>().horaLlegada) * 100).ToString("0.00"));
            labelUsingDisk.Text = "";
            labelUsingDisk.Refresh();
            dataGridView1.Refresh();
            terminateProcessNumbers[contProcTerminados] = terminated.Last<Proceso>().numProceso;
            contProcTerminados++;
            espacioEnDisco -= Convert.ToInt32(Math.Ceiling(Convert.ToDouble(terminated.Last<Proceso>().tamano) / Convert.ToDouble(pageSize)));
            liberarMemoria(terminated.Last<Proceso>().numProceso);
        }
        public bool cierreForzoso()
        {
            double numerito = Convert.ToDouble(tamano[usingDisk.Last().numProceso]) / Convert.ToDouble(pageSize);
            int aux = Convert.ToInt32(Math.Ceiling(numerito));
            if (aux > 6)
            {
                MessageBox.Show("Programa excede numero de paginas" + System.Environment.NewLine + "Cierre forzoso");
                return true;
            }
            return false;
        }
        public void ponerPaginaEnRAM()
        {
            if (espacioRam < Int32.Parse(textBoxNumOfFrames.Text))
            {
                memoriaRam[espacioRam] = usingDisk.Last<Proceso>().id + "." + usingDisk.Last().paginaActualNecesaria;
                usingDisk.Last().direccionRAM = espacioRam;
                horaDeLLegadaRAM[espacioRam] = reloj;
                dataGridView3.Rows[espacioRam].Cells[0].Value = memoriaRam[espacioRam];
                dataGridView3.Refresh();
                espacioRam++;
            }
            else if(radioButtonMasAntigua.Checked)
            {
                cambiarMasAntiguo();
            }
            else if (radioButtonMenosUsada.Checked)
            {
                cambiarMenosUsado();
            }
        }
        public void cambiarMenosUsado()
        {
            int inicio = Convert.ToInt32(memoriaSO) / pageSize;
            int lessUsed = 0;
            for (int i = inicio; i < espacioRam; i++)
            {
                if (vecesDeUsoPagina[i] > 0) { lessUsed = i; }
            }
            for(int i = inicio; i < espacioRam; i++)
            {
                if (vecesDeUsoPagina[i] < vecesDeUsoPagina[lessUsed])
                    lessUsed = i;
            }
            if (lessUsed > 0)
            {
                memoriaRam[lessUsed] = usingDisk.Last<Proceso>().id + "." + usingDisk.Last().paginaActualNecesaria;
                usingDisk.Last().direccionRAM = lessUsed;
                vecesDeUsoPagina[lessUsed] = 0;
                dataGridView3.Rows[lessUsed].Cells[0].Value = memoriaRam[lessUsed];
                dataGridView3.Refresh();
            }
        }
        public void cambiarMasAntiguo()
        {
            int viejo = Convert.ToInt32(memoriaSO) / pageSize;
            for(int i=viejo; i < espacioRam; i++)
            {
                if (horaDeLLegadaRAM[i] < horaDeLLegadaRAM[viejo])
                    viejo = i;
            }
            memoriaRam[viejo] = usingDisk.Last<Proceso>().id + "." + usingDisk.Last().paginaActualNecesaria;
            usingDisk.Last().direccionRAM = espacioRam;
            horaDeLLegadaRAM[viejo] = reloj;
            dataGridView3.Rows[viejo].Cells[0].Value = memoriaRam[viejo];
            dataGridView3.Refresh();
        }
        public bool runningEstaEnRam()
        {
            Random nuevaPagina=new Random();
            double numerito = Convert.ToDouble(running.Last().tamano) / double.Parse(comboBoxPageSize.SelectedItem.ToString());
            int aux = Convert.ToInt32(Math.Ceiling(numerito));
            int sigPagina = crearproceso.Next(1, aux);
            if (nuevaPagina.Next(0, 100) < trackBarProbSaltoPagina.Value && running.Last().tamano / pageSize > 1)
            {
                while (sigPagina == running.Last().paginaActualNecesaria)
                    sigPagina = crearproceso.Next(1, aux);
                running.Last().paginaActualNecesaria = sigPagina;
            }
            for (int i=0; i < espacioRam; i++)
            {
                if (memoriaRam[i] != null) 
                    if (running.Last<Proceso>().id + "." + running.Last().paginaActualNecesaria == memoriaRam[i])
                        return true;
            }
            return false;
        }
        public void moverUsingDiskAReady()
        {
            ready.Enqueue(usingDisk.Dequeue());
            labelReady.Text += ready.Last<Proceso>().id + System.Environment.NewLine;
            labelUsingDisk.Text = "";
            labelUsingDisk.Refresh();
            labelReady.Refresh();
        }
        public void moverAUsingDisk()
        {
            usingDisk.Enqueue(waitingDisk.Dequeue());
            labelUsingDisk.Text = usingDisk.First<Proceso>().id;
            labelUsingDisk.Refresh();
            labelWaitingDiskRefresh();
            labelWaitingDisk.Refresh();
        }
        public void labelWaitingDiskRefresh()
        {
            string temp = "";
            foreach (Proceso T in waitingDisk)
            {
                temp = temp + T.id + System.Environment.NewLine;
            }
            labelWaitingDisk.Text = temp;
        }
        public void moverAWaitingDisk()
        {
            waitingDisk.Enqueue(running.Dequeue());
            labelWaitingDisk.Text += waitingDisk.Last().id + System.Environment.NewLine;
            labelRunning.Text = "";
            labelRunning.Refresh();
            labelWaitingDisk.Refresh();
            waitingDisk.Last().tiempoUsandoDisco = 0;
        }
        public void tap()
        {
            double numerito;
            int aux;
            int cont;
            int j = 0;
            int k = 0;
            for (int i = 0; i < nombre; i++)
            {
                numerito = Convert.ToDouble(tamano[i]) / Convert.ToDouble(pageSize);
                aux = Convert.ToInt32(Math.Ceiling(numerito));
                //if (i == (Form1.nombre - 1)) { aux = Convert.ToInt32(numerito) - 1; }

                cont = 0;
                bool terminoProc = false;
                for (int c = 0; c < contProcTerminados; c++)
                {
                    if (terminateProcessNumbers[c] == i)
                        terminoProc = true;
                }
                if (aux > 6) { terminoProc = true; }
                if (terminoProc == false)
                    while (j < 6)
                    {
                        for (; k < 6; k++)
                        {
                            if (cont < aux)
                            {
                                direccionesDeMemoriaString[i, cont] = j.ToString() + "." + k.ToString();
                                direccionesDeMemoria[i, cont] = j;
                                direccionesDeMemoria[i, (cont + 6)] = k;
                                espaciosDisponibles[j, k] = false;
                            }
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
            dataGridView2.Rows.Clear();
            for (int i = 0; i < nombre; i++)
            {
                bool terminoProc = false;
                for (int c = 0; c < contProcTerminados; c++)
                {
                    if (terminateProcessNumbers[c] == i)
                        terminoProc = true;
                }
                if (terminoProc == false)
                    dataGridView2.Rows.Add("Proceso " + i, direccionesDeMemoriaString[i, 0], direccionesDeMemoriaString[i, 1], direccionesDeMemoriaString[i, 2], direccionesDeMemoriaString[i, 3], direccionesDeMemoriaString[i, 4], direccionesDeMemoriaString[i, 5]);
            }
        }
        public void estadoSistemaOperativo()
        {
            sistemaOperativo = false;
            if (radioButtonRoundRobin.Checked == true)
            {
                if (running.Count == 0 || quantum == trackBar2.Value || running.First<Proceso>().acumEjecucion == running.First<Proceso>().duracion || (running.First().acumEjecucion == running.First().horaUsoImpresora && running.First().usoImpresora == true))
                {
                    sistemaOperativo = true;
                    quantum = 0;
                }
            }
            if (radioButtonFCFS.Checked == true)
            {
                if (running.Count == 0 || running.First<Proceso>().acumEjecucion == running.First<Proceso>().duracion || (running.First().acumEjecucion == running.First().horaUsoImpresora && running.First().usoImpresora == true))
                {
                    sistemaOperativo = true;
                }
            }
        }
        public void relojes()
        {
            if (radioButtonFCFS.Checked)
            {
                labelQuantum.Visible = false;
                label14.Visible = false;
            }
            if (radioButtonRoundRobin.Checked)
            {
                labelQuantum.Visible = true;
                label14.Visible = true;
            }
            if (running.Count > 0 && sistemaOperativo == false)
            {
                quantum++;
                running.First<Proceso>().acumEjecucion++;
                acum[running.First().numProceso] = running.First().acumEjecucion;
            }
            if (printing.Count > 0)
            {
                printing.First().tiempoImprimiendo++;
            }
            if (usingDisk.Count > 0)
            {
                usingDisk.First().tiempoUsandoDisco++;
            }
            labelQuantum.Text = quantum.ToString();
            labelQuantum.Refresh();
            label26.Text = reloj.ToString();
            label26.Refresh();
        }
        public void delay()
        {
            switch (trackBar7.Value)
            {
                case 0:
                    break;
                case 1:
                    Thread.Sleep(500);
                    break;
                case 2:
                    Thread.Sleep(1000);
                    break;
            }
        }
        public void moverAPrinting()
        {
            printing.Enqueue(waiting.Dequeue());
            labelPrinting.Text = printing.First<Proceso>().id;
            labelPrinting.Refresh();
            labelWaitingRefresh();
            labelWaiting.Refresh();
        }
        public void labelWaitingRefresh()
        {
            string temp = "";
            foreach (Proceso T in waiting)
            {
                temp = temp + T.id + System.Environment.NewLine;
            }
            labelWaiting.Text = temp;
        }
        public void moverAWaiting()
        {
            waiting.Enqueue(running.Dequeue());
            labelWaiting.Text += waiting.Last().id + System.Environment.NewLine;
            labelRunning.Text = "";
            labelRunning.Refresh();
            labelWaiting.Refresh();
        }
        public void moverATerminated()
        {
            terminated.Enqueue(running.Dequeue());
            dataGridView1.Rows.Add(terminated.Last<Proceso>().id, reloj - terminated.Last<Proceso>().horaLlegada, terminated.Last<Proceso>().duracion + terminated.Last<Proceso>().tiempoEnImpresora, reloj - terminated.Last<Proceso>().horaLlegada - terminated.Last<Proceso>().duracion - terminated.Last<Proceso>().tiempoEnImpresora, ((float)(terminated.Last<Proceso>().duracion + terminated.Last<Proceso>().tiempoEnImpresora) / (reloj - terminated.Last<Proceso>().horaLlegada) * 100).ToString("0.00"));
            labelRunning.Text = "";
            labelRunning.Refresh();
            dataGridView1.Refresh();
            terminateProcessNumbers[contProcTerminados] = terminated.Last<Proceso>().numProceso;
            contProcTerminados++;
            espacioEnDisco -= Convert.ToInt32(Math.Ceiling(Convert.ToDouble(terminated.Last<Proceso>().tamano) / Convert.ToDouble(pageSize)));
            liberarMemoria(terminated.Last<Proceso>().numProceso);
        }
        private void liberarMemoria(int p)
        {
            for (int i = 0; i < 6; i++)
            {
                espaciosDisponibles[direccionesDeMemoria[p, i], direccionesDeMemoria[p, i + 6]] = true;
            }
        }
        public void moverPrintingAReady()
        {
            ready.Enqueue(printing.Dequeue());
            labelReady.Text += ready.Last<Proceso>().id + System.Environment.NewLine;
            ready.Last().usoImpresora = false;
            labelPrinting.Text = "";
            labelPrinting.Refresh();
            labelReady.Refresh();
        }
        public void moverRunningAReady()
        {
            ready.Enqueue(running.Dequeue());
            labelReady.Text += ready.Last<Proceso>().id;
            labelRunning.Text = "";
            labelRunning.Refresh();
            labelReady.Refresh();
        }
        public void moverARunning()
        {
            running.Enqueue(ready.Dequeue());
            labelRunning.Text = running.First<Proceso>().id;
            vecesDeUsoPagina[running.First().direccionRAM]++;
            labelRunning.Refresh();
            labelReadyRefresh();
            labelReady.Refresh();
        }
        public void labelReadyRefresh()
        {
            string temp = "";
            foreach (Proceso T in ready)
            {
                temp = temp + T.id + System.Environment.NewLine;
            }
            labelReady.Text = temp;
        }
        public void moverAReady()
        {
            ready.Enqueue(hold.Dequeue());
            labelReady.Text += ready.Last<Proceso>().id + System.Environment.NewLine;
            labelReady.Refresh();
            labelNewRefresh();
            labelNew.Refresh();
        }
        public void labelNewRefresh()
        {
            string temp = "";
            foreach (Proceso T in hold)
            {
                temp = temp + T.id + System.Environment.NewLine;
            }
            labelNew.Text = temp;
        }
        public void nuevoProceso()
        {
            if (espacioEnDisco < 36 || radioButtonFCFS.Checked)
            {
                Proceso tmp = new Proceso(reloj, crearproceso.Next(-3, 3) + trackBar3.Value, 0, (crearproceso.Next(100) < trackBar4.Value), crearproceso.Next(2, crearproceso.Next(-2, -1) + trackBar3.Value), crearproceso.Next(-5, 5) + trackBar5.Value, nombre, crearproceso.Next(1, procSize), crearproceso.Next(-1, 3) + trackBarTiempoUsoDisco.Value);
                if ((tmp.tamano / pageSize) + espacioEnDisco < 36)
                {
                    tmp.id = "Proceso" + nombre.ToString();
                    nombres[nombre] = tmp.id;
                    horaLleg[nombre] = tmp.horaLlegada;
                    duracion[nombre] = tmp.duracion;
                    usoImpr[nombre] = tmp.usoImpresora;
                    horaImpr[nombre] = tmp.horaUsoImpresora;
                    tiempoImpr[nombre] = tmp.tiempoEnImpresora;
                    tamano[nombre] = tmp.tamano;
                    tmp.tiempoUsandoDisco = 0;
                    tmp.paginaActualNecesaria = 1;
                    labelNew.Text += tmp.id + System.Environment.NewLine;
                    labelNew.Refresh();
                    hold.Enqueue(tmp);
                    if (tamano[nombre] % pageSize == 0) { espacioEnDisco += (tamano[nombre] / pageSize); }
                    else { espacioEnDisco += (tamano[nombre] / pageSize) + 1; }
                    nombre++;
                }
            }
            else
                MessageBox.Show("No hay suficiente espacio en disco para crear un nuevo proceso");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            estado = 2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            valoresDefault();
            formSize();
            estado = 3;
        }
        public void inicializarEspacioDisponible()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    espaciosDisponibles[i, j] = true;
                }
            }
        }
        public void valoresDefault()
        {
            inicializarEspacioDisponible();
            comboBoxPageSize.SelectedIndex = 0;
            textBoxNumOfFrames.Text = "12";
            hold.Clear();
            ready.Clear();
            running.Clear();
            terminated.Clear();
            waiting.Clear();
            printing.Clear();
            waitingDisk.Clear();
            usingDisk.Clear();
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            radioButtonRoundRobin.Checked = true;
            radioButtonMasAntigua.Checked = true;
            Array.Clear(nombres, 0, nombre);
            Array.Clear(horaLleg, 0, nombre);
            Array.Clear(duracion, 0, nombre);
            Array.Clear(acum, 0, nombre);
            Array.Clear(usoImpr, 0, nombre);
            Array.Clear(horaImpr, 0, nombre);
            Array.Clear(tiempoImpr, 0, nombre);
            Array.Clear(tamano, 0, nombre);
            Array.Clear(terminateProcessNumbers, 0, nombre);
            Array.Clear(direccionesDeMemoria, 0, 500 * 12);
            Array.Clear(direccionesDeMemoriaString, 0, 500 * 6);
            Array.Clear(espaciosDisponibles, 0, 36);
            labelQuantum.Text = "0";
            label26.Text = "0";
            labelNew.Text = "";
            labelReady.Text = "";
            labelRunning.Text = "";
            labelWaiting.Text = "";
            labelPrinting.Text = "";
            labelWaitingDisk.Text = "";
            labelUsingDisk.Text = "";
            reloj = 0;
            nombre = 0;
            quantum = 0;
            memSize = 48;
            procSize = 24;
            pageSize = 4;
            memoriaSO = 0;
            difFrames = 0;
            espacioEnDisco = 0;
            contProcTerminados = 0;
            espacioRam = 0;
            numFrames = 12;
            trackBar1.Value = 50;
            trackBar2.Value = 5;
            trackBar3.Value = 20;
            trackBar4.Value = 50;
            trackBar5.Value = 20;
            trackBar6.Value = 20;
            trackBar7.Value = 1;
            trackBarProbSaltoPagina.Value = 25;
            trackBarOSMemory.Value = 25;
            trackBarTiempoUsoDisco.Value = 8;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PCB form1 = new PCB();
            form1.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            form2.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(trackBar1, trackBar1.Value.ToString());
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(trackBar2, trackBar2.Value.ToString());
        }
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(trackBar3, trackBar3.Value.ToString());
        }
        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(trackBar4, trackBar4.Value.ToString());
        }
        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(trackBar5, trackBar5.Value.ToString());
        }
        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(trackBar6, trackBar6.Value.ToString());
        }
        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            if (trackBar7.Value == 0)
            {
                toolTip1.SetToolTip(trackBar7, "Rapido");
            }
            if (trackBar7.Value == 1)
            {
                toolTip1.SetToolTip(trackBar7, "Normal");
            }
            if (trackBar7.Value == 2)
            {
                toolTip1.SetToolTip(trackBar7, "Lento");
            }
        }
        private void trackBarProbSaltoPagina_Scroll(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(trackBarProbSaltoPagina, trackBarProbSaltoPagina.Value.ToString());
        }
        private void trackBarOSMemory_Scroll(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(trackBarOSMemory, trackBarOSMemory.Value.ToString());
        }
        private void trackBarTiempoUsoDisco_Scroll(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(trackBarTiempoUsoDisco, trackBarTiempoUsoDisco.Value.ToString());
        }
        private void buttonUpdateMemorySize_Click(object sender, EventArgs e)
        {
            definirEspacioRAM();
        }
        public void definirEspacioRAM()
        {
            if (estado != 3)
                difFrames = numFrames - Int32.Parse(textBoxNumOfFrames.Text);
            numFrames = Int32.Parse(textBoxNumOfFrames.Text);
            memSize = (Int32.Parse(comboBoxPageSize.SelectedItem.ToString()) * numFrames);
            if (memSize > 256)
                MessageBox.Show("La memoria no puede superar los 256kb" + System.Environment.NewLine + "Cambiar algun parametro");
            else
                labelMemorySize.Text = memSize.ToString();

            procSize = (Int32.Parse(comboBoxPageSize.SelectedItem.ToString()) * 6);
            labelProcessSize.Text = procSize.ToString();

            if (estado == 3)
            {
                memoriaRam = new string[500];
            }
            pageSize = Int32.Parse(comboBoxPageSize.SelectedItem.ToString());

            dataGridView3.Rows.Clear();

            dataGridView3.RowTemplate.Height = 400 / Int32.Parse(textBoxNumOfFrames.Text);
            for (int i = 0; i < Int32.Parse(textBoxNumOfFrames.Text); i++)
                this.dataGridView3.Rows.Add();

            memoriaSO = Convert.ToDouble(memSize) * Convert.ToDouble(trackBarOSMemory.Value) / 100;

            int aux = Convert.ToInt32(memoriaSO)/pageSize;
            if (estado == 3) { espacioRam += aux; }
            espacioRam -= difFrames;
            if(difFrames!=0)
                for(int i = espacioRam; i < (espacioRam + difFrames); i++)
                {
                    memoriaRam[i] = null;
                }
            for (int i = 0; i < aux; i++)
            {
                this.dataGridView3.Rows[i].Cells[0].Value=("S.O.");
            }
            for(int i = aux; i < espacioRam; i++)
                dataGridView3.Rows[i].Cells[0].Value = memoriaRam[i];
            dataGridView3.Refresh();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            FormDisk formita = new FormDisk();
            formita.Show();
        }
        public void formSize()
        {
            if (radioButtonRoundRobin.Checked == true)
            {
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(1200, 591);
                label30.Visible = true;
            }
            if (radioButtonFCFS.Checked == true)
            {
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(623, 591);
                label30.Visible = false;
            }
        }
    }
}
