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
    public partial class frm_AB : Form
    {
        // creamos las propiedasdes "A" y "B" a nuestro formulario de la clase "cls_punto" 
        // Los cuales indicaran los Extermos de la Linea a Graficar

        // Estas propiedades nos serviran para pasar los valores ingresados al formulario principal
        public cls_punto A { get; set; }
        public cls_punto B { get; set; }

        public frm_AB()
        {
            InitializeComponent();

            // Inicializamos las propiedades
            A = new cls_punto();
            B = new cls_punto();
        }

        // Evento "Click" del boton "btn_Aceptar", el cual nos servira 
        // mediante el cual el usuario indicara que ha ingresado los valores en el formulario
        private void btn_Aceptar_Click(object sender, EventArgs e)
        {
            // Evaluamos y Asignamos los valores evaluados
            //**************************************************
            // Aquí utilizamos la Función "TryParse" para convertir a un tipo de dato "int", el valor que se 
            // Ingresa por consola desde las cajas de texto "txt_aX, txt_aY, txt_bX, txt_bY".
            // El resultado de la conversión se almacena en la variables "aX, aY, bX, bY".
            // La función "TryParse" devuelve "true" si la conversión tuvo éxito y "false" si NO tuvo éxito
            // SI la Función devuelve "true" el operador condicional "?" seleccionara la variable nota 
            // SI la función devuelve "false" el operador condicional "?" seleccionara 0 como valor por defecto.
            // Todo esto es para evitar que se produzcan errores por conversión de datos
            int aX = int.TryParse(txt_aX.Text, out aX) ? aX : 0;
            int aY = int.TryParse(txt_aY.Text, out aY) ? aY : 0;

            int bX = int.TryParse(txt_bX.Text, out bX) ? bX : 0;
            int bY = int.TryParse(txt_bY.Text, out bY) ? bY : 0;

            // asignamos los valores ingresados a las propiedades "A" y B del formulario
            A.X = aX;
            A.Y = aY;

            B.X = bX;
            B.Y = bY;

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
