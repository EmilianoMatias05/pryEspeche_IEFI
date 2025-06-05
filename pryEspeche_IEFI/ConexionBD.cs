using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace pryEspeche_IEFI
{
    public class ConexionBD
    {
        private static string cadenaConexion = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=iefiBD.mdb";
        public static OleDbConnection conexion = new OleDbConnection(cadenaConexion); // Agregada

        public static DataTable EjecutarSelect(string query, params object[] parametros)
        {
            DataTable tabla = new DataTable();
            using (OleDbConnection conexion = new OleDbConnection(cadenaConexion))
            {
                using (OleDbCommand comando = new OleDbCommand(query, conexion))
                {
                    for (int i = 0; i < parametros.Length; i++)
                    {
                        comando.Parameters.AddWithValue($"@p{i}", parametros[i]);
                    }
                    using (OleDbDataAdapter adaptador = new OleDbDataAdapter(comando))
                    {
                        adaptador.Fill(tabla);
                    }
                }
            }
            return tabla;
        }
        public static void EjecutarNonQuery(string query, params object[] parametros)
        {
            using (OleDbConnection conexion = new OleDbConnection(cadenaConexion))
            {
                conexion.Open();
                using (OleDbCommand comando = new OleDbCommand(query, conexion))
                {
                    for (int i = 0; i < parametros.Length; i++)
                    {
                        comando.Parameters.AddWithValue($"@p{i}", parametros[i]);
                    }
                    comando.ExecuteNonQuery();
                }
            }
        }
    }
}