using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pryEspeche_IEFI
{
    public partial class frmTareas : Form
    {
        public frmTareas()
        {
            InitializeComponent();
        }

        private void dgvDatos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDatos.CurrentRow != null)
            {
                cmbTarea.Text = dgvDatos.CurrentRow.Cells["Tarea"].Value.ToString();
                cmbLugar.Text = dgvDatos.CurrentRow.Cells["Lugar"].Value.ToString();
                dtpFecha.Value = Convert.ToDateTime(dgvDatos.CurrentRow.Cells["Fecha"].Value);
            }
        }

        private void frmTareas_Load(object sender, EventArgs e)
        {
            cmbTarea.Items.Add("Auditoría");
            cmbTarea.Items.Add("Consultas");
            cmbTarea.Items.Add("Inspección");
            cmbTarea.Items.Add("Reclamos");
            cmbTarea.Items.Add("Visita");
            cmbTarea.Items.Add("Capacitación");
            cmbTarea.Items.Add("Supervisión");
            cmbLugar.Items.Add("Empresa");
            cmbLugar.Items.Add("Servicio");
            cmbLugar.Items.Add("Oficina");
            cmbLugar.Items.Add("Depósito");
            cmbLugar.Items.Add("Terreno");

            if (cmbTarea.Items.Count > 0)
                cmbTarea.SelectedIndex = 0;

            if (cmbLugar.Items.Count > 0)
                cmbLugar.SelectedIndex = 0;

            dtpFecha.MaxDate = DateTime.Today;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string tarea = cmbTarea.Text;
            string lugar = cmbLugar.Text;
            DateTime fecha = dtpFecha.Value.Date;

            if (string.IsNullOrWhiteSpace(tarea) || string.IsNullOrWhiteSpace(lugar))
            {
                MessageBox.Show("Debe seleccionar una tarea y un lugar.");
                return;
            }

            string consulta = "INSERT INTO Tareas (Fecha, Tarea, Lugar) VALUES (?, ?, ?)";
            ConexionBD.EjecutarNonQuery(consulta, fecha, tarea, lugar);

            MessageBox.Show("Tarea registrada correctamente.");
            CargarTareas(); // Para actualizar el DataGridView
        }
        private void CargarTareas()
        {
            string consulta = "SELECT * FROM Tareas ORDER BY Fecha DESC";
            DataTable dt = ConexionBD.EjecutarSelect(consulta);
            dgvDatos.DataSource = dt; // Asumiendo que se llama dgvTareas
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgvDatos.CurrentRow.Cells["ID"].Value);

                DialogResult confirmacion = MessageBox.Show("¿Está seguro que desea eliminar esta tarea?", "Confirmar", MessageBoxButtons.YesNo);
                if (confirmacion == DialogResult.Yes)
                {
                    string consulta = "DELETE FROM Tareas WHERE ID = ?";
                    ConexionBD.EjecutarNonQuery(consulta, id);
                    MessageBox.Show("Tarea eliminada.");
                    CargarTareas();
                }
            }
            else
            {
                MessageBox.Show("Seleccione una tarea para eliminar.");
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgvDatos.CurrentRow.Cells["ID"].Value);
                string tarea = cmbTarea.Text;
                string lugar = cmbLugar.Text;
                DateTime fecha = dtpFecha.Value.Date;

                string consulta = "UPDATE Tareas SET Fecha = ?, Tarea = ?, Lugar = ? WHERE ID = ?";
                ConexionBD.EjecutarNonQuery(consulta, fecha, tarea, lugar, id);

                MessageBox.Show("Tarea modificada correctamente.");
                CargarTareas();
            }
            else
            {
                MessageBox.Show("Seleccione una tarea para modificar.");
            }
        }
    }
}
