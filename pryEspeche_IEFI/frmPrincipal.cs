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
            striplabelUser.Text = "Usuario: " + nombreUsuario;
            striplabelFecha.Text = "Fecha: " + horaInicio.ToString("dd/MM/yyyy HH:mm");
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

            string insertQuery = "INSERT INTO Auditoria (Usuario, FechaHoraInicio, FechaHoraFin, TiempoUso) VALUES (?, ?, ?, ?)";

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(insertQuery, conn))
                    {
                        cmd.Parameters.Add("Usuario", OleDbType.VarChar).Value = nombreUsuario;
                        cmd.Parameters.Add("FechaHoraInicio", OleDbType.VarChar).Value = horaInicioStr;
                        cmd.Parameters.Add("FechaHoraFin", OleDbType.VarChar).Value = horaFinStr;
                        cmd.Parameters.Add("TiempoUso", OleDbType.VarChar).Value = tiempoStr;

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