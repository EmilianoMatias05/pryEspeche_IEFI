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
            dgvDatos.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvDatos.DefaultCellStyle.WrapMode = DataGridViewTriState.True;        

            cmbTarea.Items.Add("Auditoría");
            cmbTarea.Items.Add("Consultas");
            cmbTarea.Items.Add("Inspección");
            cmbTarea.Items.Add("Reclamos");
            cmbTarea.Items.Add("Capacitación");

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

            CargarTareas();
        }

        private void CargarTareas()
        {
            string consulta = "SELECT * FROM Tareas ORDER BY Fecha DESC";
            DataTable dt = ConexionBD.EjecutarSelect(consulta);
            dgvDatos.DataSource = dt;

            // Asegúrate de que las columnas existan antes de accederlas
            if (dgvDatos.Columns.Contains("ID")) dgvDatos.Columns["ID"].Width = 50;
            if (dgvDatos.Columns.Contains("Fecha")) dgvDatos.Columns["Fecha"].Width = 100;
            if (dgvDatos.Columns.Contains("Tarea")) dgvDatos.Columns["Tarea"].Width = 120;
            if (dgvDatos.Columns.Contains("Lugar")) dgvDatos.Columns["Lugar"].Width = 100;
            if (dgvDatos.Columns.Contains("Detalles"))
            {
                dgvDatos.Columns["Detalles"].Width = 300;
                dgvDatos.Columns["Detalles"].HeaderText = "Detalles";
                dgvDatos.Columns["Detalles"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dgvDatos.Columns.Contains("Comentarios")) dgvDatos.Columns["Comentarios"].Width = 450;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // Verificamos que haya una fila seleccionada
            if (dgvDatos.CurrentRow != null)
            {
                // Verificamos que la celda "ID" tenga un valor (no sea DBNull)
                if (dgvDatos.CurrentRow.Cells["ID"].Value != DBNull.Value)
                {
                    // Convertimos el valor de la celda a entero
                    int id = Convert.ToInt32(dgvDatos.CurrentRow.Cells["ID"].Value);

                    // Mostramos un cuadro de confirmación al usuario
                    DialogResult confirmacion = MessageBox.Show(
                        "¿Está seguro que desea eliminar esta tarea?",
                        "Confirmar",
                        MessageBoxButtons.YesNo
                    );

                    // Si el usuario confirma, ejecutamos la eliminación
                    if (confirmacion == DialogResult.Yes)
                    {
                        string consulta = "DELETE FROM Tareas WHERE ID = ?";
                        ConexionBD.EjecutarNonQuery(consulta, id); // Ejecutamos la consulta con el parámetro
                        MessageBox.Show("Tarea eliminada.");
                        CargarTareas(); // Recargamos la grilla
                    }
                }
                else
                {
                    MessageBox.Show("El ID de la tarea está vacío o no es válido.");
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

                // Reconstruir los detalles desde los checkboxes
                List<string> detalles = new List<string>();
                if (chkMovilidad.Checked) detalles.Add("Movilidad");
                if (chkSupervisor.Checked) detalles.Add("Supervisor");
                if (chkMateriales.Checked) detalles.Add("Materiales");
                if (chkSeguridad.Checked) detalles.Add("Seguridad");
                if (chkInforme.Checked) detalles.Add("Informe");
                if (chkUrgente.Checked) detalles.Add("Urgente"); // Esto también debe estar
                string detallesTexto = string.Join(", ", detalles);

                string comentarios = txtComentario.Text;

                // Consulta actualizada
                string consulta = "UPDATE Tareas SET Fecha = ?, Tarea = ?, Lugar = ?, Detalles = ?, Comentarios = ? WHERE ID = ?";
                ConexionBD.EjecutarNonQuery(consulta, fecha, tarea, lugar, detallesTexto, comentarios, id);

                MessageBox.Show("Tarea modificada correctamente.");
                CargarTareas();
            }
            else
            {
                MessageBox.Show("Seleccione una tarea para modificar.");
            }
        }

        private void dgvDatos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDatos.CurrentRow != null)
            {
                cmbTarea.Text = dgvDatos.CurrentRow.Cells["Tarea"].Value.ToString();
                cmbLugar.Text = dgvDatos.CurrentRow.Cells["Lugar"].Value.ToString();
                string detalles = dgvDatos.CurrentRow.Cells["Detalles"].Value.ToString();
                chkMovilidad.Checked = detalles.Contains("Movilidad");
                chkSupervisor.Checked = detalles.Contains("Supervisor");
                chkMateriales.Checked = detalles.Contains("Materiales");
                chkSeguridad.Checked = detalles.Contains("Seguridad");
                chkInforme.Checked = detalles.Contains("Informe");
                txtComentario.Text = dgvDatos.CurrentRow.Cells["Comentarios"].Value.ToString();

            }
            // Verificamos si la celda "Fecha" de la fila seleccionada NO es DBNull (es decir, tiene un valor válido)
            if (dgvDatos.CurrentRow.Cells["Fecha"].Value != DBNull.Value)
            {
                // Si tiene un valor válido, lo convertimos a DateTime y lo asignamos al DateTimePicker
                dtpFecha.Value = Convert.ToDateTime(dgvDatos.CurrentRow.Cells["Fecha"].Value);
            }
            else
            {
                // Si el valor es DBNull (está vacío en la base de datos), asignamos una fecha por defecto
                // Puedes cambiar 'DateTime.Today' por otra fecha si lo deseas, o incluso dejar el DateTimePicker sin modificar
                dtpFecha.Value = DateTime.Today;

                // Otra opción: podrías deshabilitar el control o mostrar un mensaje si la fecha es obligatoria
                // dtpFecha.Enabled = false; // ejemplo opcional
            }
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validación simple
                if (cmbTarea.Text == "" || cmbLugar.Text == "")
                {
                    MessageBox.Show("Por favor complete los campos de Tarea y Lugar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Construir la lista de detalles
                List<string> detalles = new List<string>();
                if (chkMovilidad.Checked) detalles.Add("Movilidad");
                if (chkSupervisor.Checked) detalles.Add("Supervisor");
                if (chkMateriales.Checked) detalles.Add("Materiales");
                if (chkSeguridad.Checked) detalles.Add("Seguridad");
                if (chkInforme.Checked) detalles.Add("Informe");
                if (chkUrgente.Checked) detalles.Add("Urgente");
                string detallesTexto = string.Join(", ", detalles);
                // Armar la consulta SQL
                string consulta = "INSERT INTO Tareas (Tarea, Fecha, Lugar, Detalles, Comentarios) " + "VALUES (?, ?, ?, ?, ?)";
                OleDbCommand comando = new OleDbCommand(consulta, ConexionBD.conexion);
                comando.Parameters.AddWithValue("?", cmbTarea.Text);
                comando.Parameters.AddWithValue("?", dtpFecha.Value);
                comando.Parameters.AddWithValue("?", cmbLugar.Text);
                comando.Parameters.AddWithValue("?", detallesTexto);
                comando.Parameters.AddWithValue("?", txtComentario.Text);
                ConexionBD.conexion.Open();
                comando.ExecuteNonQuery();
                ConexionBD.conexion.Close();
                MessageBox.Show("Tarea registrada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guar      dar la tarea: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (ConexionBD.conexion.State == ConnectionState.Open)
                    ConexionBD.conexion.Close();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show("¿Está seguro que desea cancelar? Se perderán todos los datos ingresados.", "Confirmar cancelación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (respuesta == DialogResult.Yes)
            {
                LimpiarFormulario();
            }
        }

        private void LimpiarFormulario()
        {
            cmbTarea.SelectedIndex = -1;
            cmbLugar.SelectedIndex = -1;
            dtpFecha.Value = DateTime.Today;
            chkMovilidad.Checked = false;
            chkSupervisor.Checked = false;
            chkMateriales.Checked = false;
            chkSeguridad.Checked = false;
            chkInforme.Checked = false;
            chkUrgente.Checked = false;
            txtComentario.Clear();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarTareas();
        }
    }
}