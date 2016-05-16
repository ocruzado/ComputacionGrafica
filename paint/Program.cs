using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace paint
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            frm_Inicio frmSpl = new frm_Inicio();
            frmSpl.StartPosition = FormStartPosition.CenterScreen;
            frmSpl.ShowDialog();
            Application.Run(new frm_Paint_1());
        }
    }
}
