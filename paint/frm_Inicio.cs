using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace paint
{
    public partial class frm_Inicio : Form
    {
        public frm_Inicio()
        {
            InitializeComponent();
        }

        private void frm_Inicio_Load(object sender, EventArgs e)
        {
            tmr_inicio.Interval = 2000;
            tmr_inicio.Enabled = true;
            tmr_inicio.Start();
        }

        private void tmr_inicio_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
