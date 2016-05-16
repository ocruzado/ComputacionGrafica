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
    public partial class frm_Elipse : Form
    {
        // creamos las propiedasdes "C" a nuestro formulario de la clase "cls_punto" 
        // el Cual indicara el Centro del Elipse a graficar

        //estas propiedades nos serviran para pasar los valores ingresados al formulario principal
        public cls_punto C { get; set; }

        // Creamos asi tambien las propiedades en el formulario las cuales indicaran: 
        // radioX : el Radio del Elipse a graficar
        // radioY : el Radio del Elipse a graficar
        // aInicio : el Angulo Inicial del Elipse  a graficar
        // aFin : el Angulo Final del Elipse  a graficar
        public int radioX { get; set; }
        public int radioY { get; set; }

        public int aInicio { get; set; }
        public int aFin { get; set; }

        public frm_Elipse()
        {
            InitializeComponent();

            // Inicializamos las propiedades "C"
            C = new cls_punto();
        }
        // Evento "Click" del boton "btn_Aceptar", el cual nos servira 
        // mediante el cual el usuario indicara que ha ingresado los valores en el formulario
        private void btn_Aceptar_Click(object sender, EventArgs e)
        {
            // Evaluamos y Asignamos los valores evaluados
            //**************************************************
            // Aquí utilizamos la Función "TryParse" para convertir a un tipo de dato "int", el valor que se 
            // Ingresa por consola desde las cajas de texto "txt_RadioX, txt_RadioY, txt_cX, txt_cY, txt_aInicio, txt_aFin".
            // El resultado de la conversión se almacena en la variables "rx, ry, cX, cY, inicio, fin".
            // La función "TryParse" devuelve "true" si la conversión tuvo éxito y "false" si NO tuvo éxito
            // SI la Función devuelve "true" el operador condicional "?" seleccionara la variable nota 
            // SI la función devuelve "false" el operador condicional "?" seleccionara 0 como valor por defecto.
            // Todo esto es para evitar que se produzcan errores por conversión de datos
            int rx = int.TryParse(txt_RadioX.Text, out rx) ? rx : 0;
            int ry = int.TryParse(txt_RadioY.Text, out ry) ? ry : 0;

            int cX = int.TryParse(txt_cX.Text, out cX) ? cX : 0;
            int cY = int.TryParse(txt_cY.Text, out cY) ? cY : 0;

            int inicio = int.TryParse(txt_aInicio.Text, out inicio) ? inicio : 0;
            int fin = int.TryParse(txt_aFin.Text, out fin) ? fin : 0;

            // asignamos los valores ingresados a las propiedades del formulario
            radioX = rx;
            radioY = ry;

            C.X = cX;
            C.Y = cY;

            aInicio = inicio;
            aFin = fin;

            // indicamos el resultado del cuadro de dialogo para el formulario con : DialogResult.OK (Operacion realizada con Exito)
            // esto para que se cierre el cuadro de dialogo y poder continuar con la ejecucion del programa en la ventana principal
            this.DialogResult = DialogResult.OK;
        }

        // Evento "Click" del boton "btn_Cancelar", el cual nos servira 
        // mediante el cual el usuario indicara que desea cancelar la operacion
        private void btn_Cancelar_Click(object sender, EventArgs e)
        {
            // indicamos el resultado del cuadro de dialogo para el formulario con : DialogResult.Cancel (Operacion Cancelada por el usuario) 
            // esto para que se cierre el cuadro de dialogo y poder continuar con la ejecucion del programa en la ventana principal
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
