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
    public partial class frmAuditoria : Form
    {
        public frmAuditoria()
        {
            InitializeComponent();
        }

        private void frmAuditoria_Load(object sender, EventArgs e)
        {
            dtpFecha.MaxDate = DateTime.Today;
            string query = "SELECT Usuario, HoraInicio, HoraFin, TiempoUso, Fecha FROM Auditoria ORDER BY HoraInicio DESC";

            DataTable tabla = ConexionBD.EjecutarSelect(query);

            dgvDatos.DataSource = tabla;

            // Formato de hora solamente (hh:mm:ss) para las columnas de fecha
            dgvDatos.Columns["HoraInicio"].DefaultCellStyle.Format = "HH:mm:ss";
            dgvDatos.Columns["HoraFin"].DefaultCellStyle.Format = "HH:mm:ss";
            dgvDatos.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {
            string sql = "SELECT Usuario, HoraInicio, HoraFin, TiempoUso, Fecha " +
                 "FROM Auditoria WHERE FORMAT(Fecha, 'MM/dd/yyyy') = ? " +
                 "ORDER BY HoraInicio";

            DateTime fechaSeleccionada = dtpFecha.Value.Date;
            string fechaStr = fechaSeleccionada.ToString("MM/dd/yyyy");

            dgvDatos.DataSource = ConexionBD.EjecutarSelect(sql, fechaStr);
        }

        private void btnVerTodos_Click(object sender, EventArgs e)
        {
            dgvDatos.DataSource = ConexionBD.EjecutarSelect("SELECT Usuario, HoraInicio, HoraFin, TiempoUso, Fecha FROM Auditoria ORDER BY HoraInicio DESC");

            dgvDatos.Columns["HoraInicio"].DefaultCellStyle.Format = "HH:mm:ss";
            dgvDatos.Columns["HoraFin"].DefaultCellStyle.Format = "HH:mm:ss";
            dgvDatos.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }
    }
}
