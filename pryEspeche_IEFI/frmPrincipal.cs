using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Data.SqlTypes;

namespace pryEspeche_IEFI
{
    public partial class frmPrincipal : Form
    {
        private string nombreUsuario;
        private DateTime horaInicio;
        private string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=iefiBD.mdb;";

        public frmPrincipal(string usuario)
        {
            InitializeComponent();
            this.FormClosing += frmPrincipal_FormClosing;
            nombreUsuario = usuario;
            horaInicio = DateTime.Now;
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            striplabelUser.Text = "Usuario: " + nombreUsuario;
            striplabelFecha.Text = "Fecha: " + horaInicio.ToString("dd/MM/yyyy HH:mm");

            // Solo mostrar el menú de Auditoría si el usuario es admin
            if (nombreUsuario.ToLower() != "admin")
            {
                auditoriaToolStripMenuItem.Visible = false;
            }
        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("Se está cerrando el formulario. Registrando auditoría...");
            RegistrarAuditoria();
        }

        private void RegistrarAuditoria()
        {
            DateTime horaFin = DateTime.Now;
            TimeSpan tiempoUso = horaFin - horaInicio;

            // Guardar solo hora:minuto:segundo como texto
            string horaInicioStr = horaInicio.ToString("HH:mm:ss");
            string horaFinStr = horaFin.ToString("HH:mm:ss");
            string tiempoStr = tiempoUso.ToString(@"hh\:mm\:ss");

            string insertQuery = "INSERT INTO Auditoria (Usuario, HoraInicio, HoraFin, TiempoUso, Fecha) VALUES (?, ?, ?, ?, ?)";

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(insertQuery, conn))
                    {
                        cmd.Parameters.Add("Usuario", OleDbType.VarChar).Value = nombreUsuario;
                        cmd.Parameters.Add("FechaHoraInicio", OleDbType.Date).Value = horaInicio; // tipo Date
                        cmd.Parameters.Add("FechaHoraFin", OleDbType.Date).Value = horaFin;
                        cmd.Parameters.Add("TiempoUso", OleDbType.VarChar).Value = tiempoStr;
                        cmd.Parameters.Add("Fecha", OleDbType.Date).Value = horaInicio.Date; // Solo la fecha
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar auditoría: " + ex.Message);
                }
            }
        }

        private void auditoriaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAuditoria auditoria = new frmAuditoria();
            auditoria.ShowDialog();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            this.Close(); // dispara el evento FormClosing y se graba auditoría
        }
    }
}