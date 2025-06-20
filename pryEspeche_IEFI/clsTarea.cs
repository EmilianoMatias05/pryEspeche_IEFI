using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pryEspeche_IEFI
{
    public class clsTarea
    {
        public class Tarea
        {
            public int ID { get; set; }
            public string TareaNombre { get; set; }
            public DateTime Fecha { get; set; }
            public string Lugar { get; set; }
            public string Detalles { get; set; }
            public string Comentarios { get; set; }

            public Tarea(string tareaNombre, DateTime fecha, string lugar, string detalles, string comentarios)
            {
                TareaNombre = tareaNombre;
                Fecha = fecha;
                Lugar = lugar;
                Detalles = detalles;
                Comentarios = comentarios;
            }
            public Tarea(int id, string tareaNombre, DateTime fecha, string lugar, string detalles, string comentarios)
                : this(tareaNombre, fecha, lugar, detalles, comentarios)
            {
                ID = id;
            }
        }
    }
}
