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
            // También puedes poner aquí el evento, pero es igual en Load
            // this.Load += FrmLogin_Load;
        }

        private int IntentosFallidos;

        private void FrmLogin_Load_1(object sender, EventArgs e)
        {
            BtnConectar.Enabled = false;

            // Asociar evento TextChanged a ambos textbox
            txtUsuario.TextChanged += ValidarCampos;
            txtContraseña.TextChanged += ValidarCampos;

            // Mostrar asteriscos en contraseña
            txtContraseña.PasswordChar = '*';
        }

        private void ValidarCampos(object sender, EventArgs e)
        {
            BtnConectar.Enabled = !string.IsNullOrWhiteSpace(txtUsuario.Text) &&
                                  !string.IsNullOrWhiteSpace(txtContraseña.Text);
        }

        private void BtnConectar_Click(object sender, EventArgs e)
        {
            string username = txtUsuario.Text;
            string password = txtContraseña.Text;

            string query = "SELECT COUNT(*) FROM Usuarios WHERE Nombre = ? AND Contraseña = ?";

            DataTable dt = ConexionBD.EjecutarSelect(query, username, password);

            if (dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) > 0)
            {
                MessageBox.Show("Inicio de sesión exitoso.");

                frmPrincipal principal = new frmPrincipal(username);
                principal.ShowDialog();
                this.Close();
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
    }
}