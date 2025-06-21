using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace pryEspeche_IEFI
{
    public partial class frmUsuarios : Form
    {
        public frmUsuarios()
        {
            InitializeComponent();
        }

        private void frmUsuarios_Load(object sender, EventArgs e)
        {
            MostrarUsuarios();
        }

        private void MostrarUsuarios()
        {
            dgvDatos.DataSource = ConexionBD.EjecutarSelect("SELECT * FROM Usuarios");
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text != "" && txtContraseña.Text != "")
            {
                var nuevoUsuario = new clsUsuario.Usuario(txtNombre.Text, txtContraseña.Text);
                string consulta = "INSERT INTO Usuarios (Nombre, Contraseña) VALUES (?, ?)";
                ConexionBD.EjecutarQuery(consulta, nuevoUsuario.Nombre, nuevoUsuario.Contraseña);

                MessageBox.Show("Usuario agregado correctamente.");
                MostrarUsuarios();
            }
            else
            {
                MessageBox.Show("Completa todos los campos.");
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (txtID.Text != "")
            {
                var usuarioModificado = new clsUsuario.Usuario(
                    Convert.ToInt32(txtID.Text),
                    txtNombre.Text,
                    txtContraseña.Text
                );

                string consulta = "UPDATE Usuarios SET Nombre = ?, Contraseña = ? WHERE ID = ?";
                ConexionBD.EjecutarQuery(consulta,
                    usuarioModificado.Nombre,
                    usuarioModificado.Contraseña,
                    usuarioModificado.ID
                );

                MessageBox.Show("Usuario modificado correctamente.");
                MostrarUsuarios();
            }
            else
            {
                MessageBox.Show("Seleccioná un usuario para modificar.");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (txtID.Text != "")
            {
                int id = Convert.ToInt32(txtID.Text);
                string consulta = "DELETE FROM Usuarios WHERE ID = ?";
                ConexionBD.EjecutarQuery(consulta, id);

                MessageBox.Show("Usuario eliminado correctamente.");
                MostrarUsuarios();
            }
            else
            {
                MessageBox.Show("Seleccioná un usuario para eliminar.");
            }
        }

        private void dgvDatos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtID.Text = dgvDatos.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                txtNombre.Text = dgvDatos.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
                txtContraseña.Text = dgvDatos.Rows[e.RowIndex].Cells["Contraseña"].Value.ToString();
            }
        }
    }
}