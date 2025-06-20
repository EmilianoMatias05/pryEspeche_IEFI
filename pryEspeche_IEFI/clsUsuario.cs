using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pryEspeche_IEFI
{
    public class clsUsuario
    {
        public class Usuario
        {
            public int ID { get; set; }
            public string Nombre { get; set; }
            public string Contraseña { get; set; }
            
            public Usuario(string nombre, string contraseña)
            {
                Nombre = nombre;
                Contraseña = contraseña;
            }
        
            public Usuario(int id, string nombre, string contraseña) : this(nombre, contraseña)
            {
                ID = id;
            }
        }
    }
}
