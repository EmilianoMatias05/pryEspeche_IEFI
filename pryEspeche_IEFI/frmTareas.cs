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

namespace pryEspeche_IEFI
{
    public partial class frmTareas : Form
    {
        public frmTareas()
        {
            InitializeComponent();
        }

        private void frmTareas_Load(object sender, EventArgs e)
        {
            cmbTarea.Items.AddRange(new string[] { "Auditoría", "Consultas", "Inspección", "Reclamos", "Capacitación" });
            cmbLugar.Items.AddRange(new string[] { "Empresa", "Servicio", "Oficina", "Depósito", "Terreno" });
            cmbTarea.SelectedIndex = 0;
            cmbLugar.SelectedIndex = 0;
            dtpFecha.MaxDate = DateTime.Today;
            CargarTareas();
        }

        private void CargarTareas()
        {
            dgvDatos.DataSource = ConexionBD.EjecutarSelect("SELECT * FROM Tareas ORDER BY Fecha DESC");

            if (dgvDatos.Columns.Contains("ID")) dgvDatos.Columns["ID"].Width = 50;
            if (dgvDatos.Columns.Contains("Fecha")) dgvDatos.Columns["Fecha"].Width = 100;
            if (dgvDatos.Columns.Contains("Tarea")) dgvDatos.Columns["Tarea"].Width = 120;
            if (dgvDatos.Columns.Contains("Lugar")) dgvDatos.Columns["Lugar"].Width = 100;

            if (dgvDatos.Columns.Contains("Detalles"))
            {
                dgvDatos.Columns["Detalles"].Width = 300;
                dgvDatos.Columns["Detalles"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (dgvDatos.Columns.Contains("Comentarios"))
                dgvDatos.Columns["Comentarios"].Width = 450;
        }

        private string ObtenerDetalles()
        {
            string detalles = "";

            if (chkMovilidad.Checked) detalles += "Movilidad";
            if (chkSupervisor.Checked)
            {
                if (detalles != "") detalles += ", ";
                detalles += "Supervisor";
            }
            if (chkMateriales.Checked)
            {
                if (detalles != "") detalles += ", ";
                detalles += "Materiales";
            }
            if (chkSeguridad.Checked)
            {
                if (detalles != "") detalles += ", ";
                detalles += "Seguridad";
            }
            if (chkInforme.Checked)
            {
                if (detalles != "") detalles += ", ";
                detalles += "Informe";
            }
            if (chkUrgente.Checked)
            {
                if (detalles != "") detalles += ", ";
                detalles += "Urgente";
            }

            return detalles;
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (cmbTarea.Text == "" || cmbLugar.Text == "")
            {
                MessageBox.Show("Complete los campos obligatorios.");
                return;
            }

            var nueva = new clsTarea.Tarea(cmbTarea.Text, dtpFecha.Value, cmbLugar.Text, ObtenerDetalles(), txtComentario.Text);

            ConexionBD.EjecutarNonQuery("INSERT INTO Tareas (Tarea, Fecha, Lugar, Detalles, Comentarios) VALUES (?, ?, ?, ?, ?)",
                nueva.TareaNombre, nueva.Fecha, nueva.Lugar, nueva.Detalles, nueva.Comentarios);

            MessageBox.Show("Tarea registrada.");
            LimpiarFormulario();
            CargarTareas();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una tarea para modificar.");
                return;
            }

            int id = Convert.ToInt32(dgvDatos.CurrentRow.Cells["ID"].Value);
            var t = new clsTarea.Tarea(id, cmbTarea.Text, dtpFecha.Value, cmbLugar.Text, ObtenerDetalles(), txtComentario.Text);

            ConexionBD.EjecutarNonQuery("UPDATE Tareas SET Fecha = ?, Tarea = ?, Lugar = ?, Detalles = ?, Comentarios = ? WHERE ID = ?",
                t.Fecha, t.TareaNombre, t.Lugar, t.Detalles, t.Comentarios, t.ID);

            MessageBox.Show("Tarea modificada.");
            CargarTareas();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.CurrentRow == null || dgvDatos.CurrentRow.Cells["ID"].Value == DBNull.Value)
            {
                MessageBox.Show("Seleccione una tarea válida.");
                return;
            }

            int id = Convert.ToInt32(dgvDatos.CurrentRow.Cells["ID"].Value);

            if (MessageBox.Show("¿Eliminar esta tarea?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ConexionBD.EjecutarNonQuery("DELETE FROM Tareas WHERE ID = ?", id);
                MessageBox.Show("Tarea eliminada.");
                CargarTareas();
            }
        }

        private void dgvDatos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDatos.CurrentRow == null) return;

            cmbTarea.Text = dgvDatos.CurrentRow.Cells["Tarea"].Value.ToString();
            cmbLugar.Text = dgvDatos.CurrentRow.Cells["Lugar"].Value.ToString();
            txtComentario.Text = dgvDatos.CurrentRow.Cells["Comentarios"].Value.ToString();

            string detalles = dgvDatos.CurrentRow.Cells["Detalles"].Value.ToString();
            chkMovilidad.Checked = detalles.Contains("Movilidad");
            chkSupervisor.Checked = detalles.Contains("Supervisor");
            chkMateriales.Checked = detalles.Contains("Materiales");
            chkSeguridad.Checked = detalles.Contains("Seguridad");
            chkInforme.Checked = detalles.Contains("Informe");
            chkUrgente.Checked = detalles.Contains("Urgente");

            dtpFecha.Value = dgvDatos.CurrentRow.Cells["Fecha"].Value != DBNull.Value
                ? Convert.ToDateTime(dgvDatos.CurrentRow.Cells["Fecha"].Value)
                : DateTime.Today;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Cancelar ingreso?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
                LimpiarFormulario();
        }

        private void btnActualizar_Click(object sender, EventArgs e) => CargarTareas();

        private void LimpiarFormulario()
        {
            cmbTarea.SelectedIndex = -1;
            cmbLugar.SelectedIndex = -1;
            dtpFecha.Value = DateTime.Today;
            chkMovilidad.Checked = chkSupervisor.Checked = chkMateriales.Checked =
            chkSeguridad.Checked = chkInforme.Checked = chkUrgente.Checked = false;
            txtComentario.Clear();
        }
    }
}