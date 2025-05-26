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
            
            string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;;Data Source=iefiBD.mdb;";

            string username = txtUsuario.Text;
            string password = txtContraseña.Text;

            
            string query = "SELECT COUNT(*) FROM Usuarios WHERE Nombre = ? AND Contraseña = ?";

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand(query, conn);

                    
                    cmd.Parameters.AddWithValue("?", username);
                    cmd.Parameters.AddWithValue("?", password);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Inicio de sesión exitoso.");
                        frmPrincipal principal = new frmPrincipal(username); // pasa el nombre de usuario
                        this.Hide();
                        principal.Show();
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
