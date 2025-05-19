using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace pryEspeche_IEFI
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private int IntentosFallidos;

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            txtContraseña.UseSystemPasswordChar = true;
        }

        private void BtnConectar_Click(object sender, EventArgs e)
        {
            // 📁 Ruta al archivo Access (podés poner la ruta completa si querés)
            string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;;Data Source=Login.mdb;";

            string username = txtUsuario.Text;
            string password = txtContraseña.Text;

            // 🟡 Adaptado a nombres de campo reales en Access: Nombre y Contraseña
            string query = "SELECT COUNT(*) FROM Usuarios WHERE Nombre = ? AND Contraseña = ?";

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand(query, conn);

                    // ⚠️ En OleDb los parámetros son por orden, no por nombre
                    cmd.Parameters.AddWithValue("?", username);
                    cmd.Parameters.AddWithValue("?", password);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Inicio de sesión exitoso.");
                        // Podés abrir otro formulario acá si querés
                        this.Hide();
                    }
                    else
                    {
                        IntentosFallidos++;
                        MessageBox.Show("Usuario o contraseña incorrectos. Intento " + IntentosFallidos + " de 3.");

                        if (IntentosFallidos >= 3)
                        {
                            MessageBox.Show("Demasiados intentos fallidos. El formulario se cerrará.");
                            this.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectar con la base de datos: " + ex.Message);
                }
            }
        }
    }
}
