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
        private void MostrarUsuarios()
        {
            string consulta = "SELECT * FROM Usuarios";
            dgvDatos.DataSource = ConexionBD.EjecutarSelect(consulta);
        }

        private void frmUsuarios_Load(object sender, EventArgs e)
        {
            MostrarUsuarios();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text != "" && txtContraseña.Text != "")
            {
                string consulta = "INSERT INTO Usuarios (Nombre, Contraseña) VALUES (?, ?)";
                ConexionBD.EjecutarNonQuery(consulta, txtNombre.Text, txtContraseña.Text);
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
                string consulta = "UPDATE Usuarios SET Nombre=?, Contraseña=? WHERE ID=?";
                ConexionBD.EjecutarNonQuery(consulta, txtNombre.Text, txtContraseña.Text, Convert.ToInt32(txtID.Text));
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
                string consulta = "DELETE FROM Usuarios WHERE ID=?";
                ConexionBD.EjecutarNonQuery(consulta, Convert.ToInt32(txtID.Text));
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
