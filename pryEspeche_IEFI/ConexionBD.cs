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

        public static DataTable EjecutarSelect(string consulta, params object[] parametros)
        {
            DataTable tabla = new DataTable();

            using (OleDbConnection conn = new OleDbConnection(cadenaConexion))
            using (OleDbCommand cmd = new OleDbCommand(consulta, conn))
            {
                for (int i = 0; i < parametros.Length; i++)
                {
                    cmd.Parameters.AddWithValue($"@p{i}", parametros[i]);
                }

                try
                {
                    conn.Open();
                    OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                    adapter.Fill(tabla);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en la base de datos: " + ex.Message);
                }
            }

            return tabla;
        }

        public static void EjecutarNonQuery(string consulta, params object[] parametros)
        {
            using (OleDbConnection conn = new OleDbConnection(cadenaConexion))
            using (OleDbCommand cmd = new OleDbCommand(consulta, conn))
            {
                for (int i = 0; i < parametros.Length; i++)
                {
                    cmd.Parameters.AddWithValue($"@p{i}", parametros[i]);
                }

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al ejecutar: " + ex.Message);
                }
            }
        }
    }
}

