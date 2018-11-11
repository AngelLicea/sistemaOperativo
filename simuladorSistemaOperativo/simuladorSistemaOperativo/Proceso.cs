using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simuladorSistemaOperativo
{
    class Proceso
    {
        public string id { get; set; }
        public int horaLlegada { get; set; }
        public int duracion { get; set; }
        public int acumEjecucion { get; set; }
        public bool usoImpresora { get; set; }
        public int horaUsoImpresora { get; set; }
        public int tiempoEnImpresora { get; set; }
        public int numProceso { get; set; }
        public int tiempoImprimiendo { get; set; }
        public int tamano { get; set; }
        public int tiempoEnDisco { get; set; }
        public int tiempoUsandoDisco { get; set; }
        public int direccionRAM { get; set; }
        public int paginaActualNecesaria { get; set; }

        public Proceso(int hrLlega, int dura, int acumEjec, bool usaImpr, int hrUsaImpr, int tiempoEnImpre, int numP, int size, int tiemEnDisk)
        {
            horaLlegada = hrLlega;
            duracion = dura;
            acumEjecucion = acumEjec;
            usoImpresora = usaImpr;
            numProceso = numP;
            tiempoImprimiendo = 0;
            tamano = size;
            tiempoEnDisco = tiemEnDisk;
            if (usoImpresora == false)
            {
                horaUsoImpresora = 0;
                tiempoEnImpresora = 0;
            }
            else
            {
                horaUsoImpresora = hrUsaImpr;
                tiempoEnImpresora = tiempoEnImpre;
            }
        }
        public Proceso() { }
    }
}
