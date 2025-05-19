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
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            // Crear StatusStrip
            StatusStrip statusStrip = new StatusStrip();

            // Label izquierda
            ToolStripStatusLabel lblIzquierda = new ToolStripStatusLabel("Conectado");
            lblIzquierda.Spring = true; // Ocupará el espacio restante

            // Label derecha
            ToolStripStatusLabel lblDerecha = new ToolStripStatusLabel("Usuario: admin");
            lblDerecha.TextAlign = ContentAlignment.MiddleRight;

            // Agregar al StatusStrip
            statusStrip.Items.Add(lblIzquierda);
            statusStrip.Items.Add(lblDerecha);

            // Agregar al formulario
            this.Controls.Add(statusStrip);
        }
    }
}
