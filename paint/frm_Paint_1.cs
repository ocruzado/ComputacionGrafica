using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace paint
{
    public partial class frm_Paint_1 : Form
    {
        //************************************************************
        //Variables de Formulario
        //************************************************************
        //estas variables serán usadas a nivel de todo el formulario, por eso las declaramos aquí
        //************************************************************

        Pen lapiz;
        //Pen pincel;
        Pen goma;

        Bitmap bmp;
        bool cursorDown;
        Point[] puntos;

        Graphics ov_grafico;
        Graphics ov_grafico_2;

        // Esta variable la usaremos para guardar el color seleccionado por el usuario
        Color ov_color;

        // Esta variable se usara para definir el pincel que se usara para rellenar las elipses 
        SolidBrush ov_brocha;

        // Esta variable se usara para guardar el grosor utilizado en las lineas de las formas y que sera definido por el usuario 
        int nv_grosor;

        // Esta variable se usara para guardar el (grosor / 2) 
        // y se usara para centrar las elipses usadas como puntos en la generacion de las formas (Lineas, Circunferencias y Elipses)
        //int nv_grosor_mitad;

        // esta variable se usara para guardar el numeros de los "clicks" hechos por el usuario
        int nv_click = 0;

        // esta variable guradara la forma seleecionada por el usuario
        string sv_forma;

        // esta variable almacenara la coordenada X del evento Click sobre el lienzo 
        int nv_px;

        // esta variable almacenara la coordenada Y del evento Click sobre el lienzo 
        int nv_py;

        // en las variables "p0, p1, p2" guardaremos las coordenadas ingresadas por el raton sobre el lienzo
        Point p0 = new Point();
        Point p1 = new Point();
        Point p2 = new Point();

        int distanciaX;
        int distanciaY;

        public frm_Paint_1()
        {
            InitializeComponent();
        }

        // en el evento load inicializamos las variables del formulario
        private void Form1_Load(object sender, EventArgs e)
        {
            bmp = new Bitmap(pct_Dibujo.Width, pct_Dibujo.Height);

            IconoLapiz();

            iGrafics();

            puntos = new Point[0];

            ov_color = Color.Black;

            trb_grosor.Value = 3;
            nv_grosor = 3;

            sv_forma = "Pincel";

            ov_brocha = new SolidBrush(ov_color);

            lapiz = new Pen(ov_color, nv_grosor);
            lapiz.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            lapiz.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            lapiz.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;

            goma = new Pen(Color.White, nv_grosor);
            goma.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            goma.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            goma.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;


            Point R11 = new Point(23, 40);
            Point R12 = new Point(50, 50);

            ov_grafico.DrawEllipse(lapiz, 23, 40, nv_grosor, nv_grosor);
            ov_grafico_2.DrawEllipse(lapiz, 23, 40, nv_grosor, nv_grosor);


            ov_grafico.DrawLine(lapiz, R11, R12);
            ov_grafico_2.DrawLine(lapiz, R11, R12);
        }

        private void pctDibujo_MouseMove(object sender, MouseEventArgs e)
        {
            int puntoX = e.X;
            int puntoY = pct_Dibujo.Width - e.Y;

            lbl_PuntoX.Text = puntoX.ToString();
            lbl_PuntoY.Text = puntoY.ToString();

            //int puntoX = e.X;
            //int puntoY = e.Y;

            //lbl_PuntoX.Text = puntoX.ToString();
            //lbl_PuntoY.Text = (pct_Dibujo.Width - e.Y).ToString();

            if (sv_forma.Equals("Pincel"))
            {
                if (cursorDown)
                {
                    agregarPunto(new Point(puntoX, puntoY));
                }

                if (puntos.Length > 1)
                {
                    iGrafics();
                    ov_grafico.DrawLines(lapiz, puntos);
                    ov_grafico_2.DrawLines(lapiz, puntos);
                    fGrafics();
                }
            }

            if (sv_forma.Equals("Borrador"))
            {
                if (cursorDown)
                {
                    agregarPunto(new Point(puntoX, puntoY));
                }

                if (puntos.Length > 1)
                {
                    iGrafics();
                    ov_grafico.DrawLines(goma, puntos);
                    ov_grafico_2.DrawLines(goma, puntos);
                    fGrafics();
                }
            }
        }

        private void pctDibujo_MouseClick(object sender, MouseEventArgs e)
        {
            nv_px = e.X;
            nv_py = pct_Dibujo.Width - e.Y;
            //nv_px = e.X;
            //nv_py = e.Y;
            //nv_px = e.X;
            //nv_py = e.Y - pct_Dibujo.Width;

            //if (sv_forma.Equals("Pincel"))
            //{
            iGrafics();
            ov_grafico.FillEllipse(ov_brocha, nv_px, nv_py, nv_grosor, nv_grosor);
            ov_grafico_2.FillEllipse(ov_brocha, nv_px, nv_py, nv_grosor, nv_grosor);
            fGrafics();
            //}
            //else 
            if (sv_forma.Equals("Borrador"))
            {
                iGrafics();
                ov_grafico.FillEllipse(new SolidBrush(Color.White), nv_px, nv_py, nv_grosor, nv_grosor);
                ov_grafico_2.FillEllipse(new SolidBrush(Color.White), nv_px, nv_py, nv_grosor, nv_grosor);
                fGrafics();
            }
            else if (sv_forma.Equals("Triangulo"))
            {
                if (nv_click == 0)
                {
                    p0.X = nv_px;
                    p0.Y = nv_py;

                    nv_click = 1;
                }
                else if (nv_click == 1)
                {
                    p1.X = nv_px;
                    p1.Y = nv_py;

                    nv_click = 2;

                }
                else if (nv_click == 2)
                {

                    //TrazarLineaDDA(new Point(p1.X + (p2.X - p1.X) / 2, p1.Y), new Point(p1.X, p2.Y));
                    //TrazarLineaDDA(new Point(p1.X + (p2.X - p1.X) / 2, p1.Y), new Point(p2.X, p2.Y));
                    //TrazarLineaDDA(new Point(p1.X, p2.Y), new Point(p2.X, p2.Y));

                    p2.X = nv_px;
                    p2.Y = nv_py;


                    TrazarLineaDDA(p0, p1);
                    TrazarLineaDDA(p1, p2);
                    TrazarLineaDDA(p2, p0);

                    nv_click = 0;
                }
            }
            else if (sv_forma.Equals("Rectangulo"))
            {
                if (nv_click == 0)
                {
                    p1.X = nv_px;
                    p1.Y = nv_py;

                    nv_click = 1;
                }
                else
                {
                    p2.X = nv_px;
                    p2.Y = nv_py;

                    TrazarLineaDDA(new Point(p1.X, p1.Y), new Point(p2.X, p1.Y));
                    TrazarLineaDDA(new Point(p1.X, p1.Y), new Point(p1.X, p2.Y));
                    TrazarLineaDDA(new Point(p2.X, p2.Y), new Point(p1.X, p2.Y));
                    TrazarLineaDDA(new Point(p2.X, p2.Y), new Point(p2.X, p1.Y));

                    nv_click = 0;
                }

            }
            //else if (sv_forma.Equals("Linea"))
            //{
            //    if (nv_click == 0)
            //    {
            //        p1.X = nv_px;
            //        p1.Y = nv_py;

            //        nv_click = 1;
            //    }
            //    else
            //    {
            //        p2.X = nv_px;
            //        p2.Y = nv_py;

            //        iGrafics();
            //        ov_grafico.DrawLine(lapiz, p1, p2);
            //        ov_grafico_2.DrawLine(lapiz, p1, p2);
            //        fGrafics();

            //        nv_click = 0;
            //    }
            //}

            else if (sv_forma.Equals("Linea - DDA"))
            {
                if (nv_click == 0)
                {
                    p1.X = nv_px;
                    p1.Y = nv_py;

                    nv_click = 1;
                }
                else
                {
                    p2.X = nv_px;
                    p2.Y = nv_py;

                    TrazarLineaDDA(p1, p2);

                    nv_click = 0;
                }
            }

            else if (sv_forma.Equals("Poligono"))
            {
                nv_click++;

                if (nv_click == 1)
                {
                    p0.X = nv_px;
                    p0.Y = nv_py;

                    p1.X = nv_px;
                    p1.Y = nv_py;
                }
                else
                {
                    p2.X = nv_px;
                    p2.Y = nv_py;


                    //iGrafics();
                    //ov_grafico.DrawLine(lapiz, p1, p2);
                    //ov_grafico_2.DrawLine(lapiz, p1, p2);
                    TrazarLineaDDA(p1, p2);

                    //fGrafics();

                    p1.X = nv_px;
                    p1.Y = nv_py;
                }
            }

            else if (sv_forma.Equals("Circunferencia - Punto Medio"))
            {

                if (nv_click == 0)
                {
                    p1.X = nv_px;
                    p1.Y = nv_py;

                    nv_click = 1;
                }
                else
                {
                    p2.X = nv_px;
                    p2.Y = nv_py;

                    int DX, DY;

                    DX = p2.X - p1.X;
                    DY = p2.Y - p1.Y;

                    DX = Math.Abs(DX);
                    DY = Math.Abs(DY);

                    int r = Convert.ToInt32(Math.Sqrt(Math.Pow(DX, 2) + Math.Pow(DY, 2)));

                    CircunferenciaPuntoMedio(p1, r);

                    nv_click = 0;
                }
            }
            else if (sv_forma.Equals("Circunferencia - DDA"))
            {

                if (nv_click == 0)
                {
                    p1.X = nv_px;
                    p1.Y = nv_py;

                    nv_click = 1;
                }
                else
                {
                    p2.X = nv_px;
                    p2.Y = nv_py;

                    int DX, DY;

                    DX = p2.X - p1.X;
                    DY = p2.Y - p1.Y;

                    DX = Math.Abs(DX);
                    DY = Math.Abs(DY);

                    int r = Convert.ToInt32(Math.Sqrt(Math.Pow(DX, 2) + Math.Pow(DY, 2)));

                    CircunferenciaDDA(p1, r);

                    nv_click = 0;
                }
            }

            else if (sv_forma.Equals("Elipse - Punto Medio"))
            {
                if (nv_click == 0)
                {
                    p0.X = nv_px;
                    p0.Y = nv_py;

                    nv_click = 1;
                }
                else if (nv_click == 1)
                {
                    if (nv_px > p0.X) { distanciaX = nv_px - p0.X; }
                    else { distanciaX = p0.X - nv_px; }

                    if (nv_py > p0.Y) { distanciaY = nv_py - p0.Y; }
                    else { distanciaY = p0.Y - nv_py; }

                    ElipsePuntoMedio(p0, distanciaX, distanciaY);

                    nv_click = 0;
                }


            }
        }

        private void pct_Dibujo_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (sv_forma.Equals("Poligono"))
            {
                p1.X = nv_px;
                p1.Y = nv_py;

                //iGrafics();
                //ov_grafico.DrawLine(lapiz, p1, p0);
                //ov_grafico_2.DrawLine(lapiz, p1, p0);
                TrazarLineaDDA(p1, p0);
                //fGrafics();

                nv_click = 0;
            }
        }

        private void pct_Dibujo_MouseUp(object sender, MouseEventArgs e)
        {
            if (sv_forma.Equals("Pincel") || sv_forma.Equals("Borrador"))
            {
                cursorDown = false;

                puntos = new Point[0];
            }
        }

        private void pct_Dibujo_MouseDown(object sender, MouseEventArgs e)
        {
            if (sv_forma.Equals("Pincel") || sv_forma.Equals("Borrador"))
            {
                cursorDown = true;
            }
        }


        private void trb_grosor_ValueChanged(object sender, EventArgs e)
        {
            nv_grosor = Convert.ToInt32(trb_grosor.Value);

            lapiz.Width = nv_grosor;
            goma.Width = nv_grosor;

            if (sv_forma.Equals("Borrador"))
                CrearGoma();
        }

        #region Paleta de Colores
        private void pct_Red_Click(object sender, EventArgs e)
        {
            pct_Select.BackColor = pct_Red.BackColor;
            ov_color = pct_Red.BackColor;

            ov_brocha.Color = ov_color;
            lapiz.Color = ov_color;
        }

        private void pct_Green_Click(object sender, EventArgs e)
        {
            pct_Select.BackColor = pct_Green.BackColor;
            ov_color = pct_Green.BackColor;

            ov_brocha.Color = ov_color;
            lapiz.Color = ov_color;
        }

        private void pct_Blue_Click(object sender, EventArgs e)
        {
            pct_Select.BackColor = pct_Blue.BackColor;
            ov_color = pct_Blue.BackColor;

            ov_brocha.Color = ov_color;
            lapiz.Color = ov_color;
        }

        private void pct_Yellow_Click(object sender, EventArgs e)
        {
            pct_Select.BackColor = pct_Yellow.BackColor;
            ov_color = pct_Yellow.BackColor;

            ov_brocha.Color = ov_color;
            lapiz.Color = ov_color;
        }

        private void pct_Purpel_Click(object sender, EventArgs e)
        {
            pct_Select.BackColor = pct_Purpel.BackColor;
            ov_color = pct_Purpel.BackColor;

            ov_brocha.Color = ov_color;
            lapiz.Color = ov_color;
        }

        private void pct_Pink_Click(object sender, EventArgs e)
        {
            pct_Select.BackColor = pct_Pink.BackColor;
            ov_color = pct_Pink.BackColor;

            ov_brocha.Color = ov_color;
            lapiz.Color = ov_color;
        }

        private void pct_Gray_Click(object sender, EventArgs e)
        {
            pct_Select.BackColor = pct_Gray.BackColor;
            ov_color = pct_Gray.BackColor;

            ov_brocha.Color = ov_color;
            lapiz.Color = ov_color;
        }

        private void pct_Orange_Click(object sender, EventArgs e)
        {
            pct_Select.BackColor = pct_Orange.BackColor;
            ov_color = pct_Orange.BackColor;

            ov_brocha.Color = ov_color;
            lapiz.Color = ov_color;
        }

        private void pct_White_Click(object sender, EventArgs e)
        {
            pct_Select.BackColor = pct_White.BackColor;
            ov_color = pct_White.BackColor;

            ov_brocha.Color = ov_color;
            lapiz.Color = ov_color;
        }
        #endregion

        private void btn_Borrador_Click(object sender, EventArgs e)
        {
            // Guardamos la nueva forma selecionada en la variables de formulario "sv_forma"
            sv_forma = "Borrador";

            // Reiniciamos el contrador para los clicks
            nv_click = 0;

            CrearGoma();
        }

        #region Formas con Mouse
        private void btn_Pincel_Click(object sender, EventArgs e)
        {
            // Guardamos la nueva forma selecionada en la variables de formulario "sv_forma"
            sv_forma = "Pincel";

            // Reiniciamos el contrador para los clicks
            nv_click = 0;

            IconoLapiz();
        }



        private void btn_LineaDDA_Click(object sender, EventArgs e)
        {
            // Guardamos la nueva forma selecionada en la variables de formulario "sv_forma"
            sv_forma = "Linea - DDA";

            // Reiniciamos el contrador para los clicks
            nv_click = 0;

            IconoLapiz();
        }

        private void btn_Poligono_Click(object sender, EventArgs e)
        {
            // Guardamos la nueva forma selecionada en la variables de formulario "sv_forma"
            sv_forma = "Poligono";

            // Reiniciamos el contrador para los clicks
            nv_click = 0;

            IconoLapiz();
        }

        private void btn_CircunferenciaDDA_Click(object sender, EventArgs e)
        {
            // Guardamos la nueva forma selecionada en la variables de formulario "sv_forma"
            sv_forma = "Circunferencia - DDA";

            // Reiniciamos el contrador para los clicks
            nv_click = 0;

            IconoLapiz();
        }

        private void btn_CircunferenciaPM_Click(object sender, EventArgs e)
        {
            // Guardamos la nueva forma selecionada en la variables de formulario "sv_forma"
            sv_forma = "Circunferencia - Punto Medio";

            // Reiniciamos el contrador para los clicks
            nv_click = 0;

            IconoLapiz();
        }

        private void btn_ElipsePM_Click(object sender, EventArgs e)
        {
            // Guardamos la nueva forma selecionada en la variables de formulario "sv_forma"
            sv_forma = "Elipse - Punto Medio";

            // Reiniciamos el contrador para los clicks
            nv_click = 0;

            IconoLapiz();
        }

        private void btn_Triangulo_Click(object sender, EventArgs e)
        {
            // Guardamos la nueva forma selecionada en la variables de formulario "sv_forma"
            sv_forma = "Triangulo";

            // Reiniciamos el contrador para los clicks
            nv_click = 0;

            IconoLapiz();
        }

        private void btn_Rectangulo_Click(object sender, EventArgs e)
        {
            // Guardamos la nueva forma selecionada en la variables de formulario "sv_forma"
            sv_forma = "Rectangulo";

            // Reiniciamos el contrador para los clicks
            nv_click = 0;

            IconoLapiz();
        }
        #endregion


        #region Figuras con Parametros
        private void btn_LineaParam_Click(object sender, EventArgs e)
        {
            // Creamos la instancia "ofrm_AB" de la clase "frm_AB", el cual es un formulario que usaremos como emergente
            // para el ingreso de los parametros de la Linea DDA
            frm_AB ofrm_AB = new frm_AB();

            // Iniciamos el cuadro de Dialogo con el metodo "ShowDialog" el cual tambien devuelve un objeto de tipo "DialogResult"
            // que podemos usar para capturar la respuesta del usuario

            DialogResult dr_AB = ofrm_AB.ShowDialog();

            // Evaluamos la resspuesta del Cuadro de Dialogo (Formulario Emergente)
            if (dr_AB == DialogResult.OK)
            {
                // Creamos 2 instancias de la clase "Point" a y b
                Point a = new Point();
                Point b = new Point();

                // Aqui Recuperamos los valores ingresados en el formulario emergente y los asignamos a las variables 
                // las cuales usaremos como parametros para graficar la Linea DDA
                // asignamos los valores X y Y a las propiedades de las variables "a" y "b" de clase "Point" ingresadas en el formulario "frm_AB"

                a.X = ofrm_AB.A.X;
                a.Y = ofrm_AB.A.Y;

                b.X = ofrm_AB.B.X;
                b.Y = ofrm_AB.B.Y;

                // Pasamos pasamos las varialbes "a" y "b" como parametros para graficar la Linea mediante el Algoritmo DDA
                TrazarLineaDDA(a, b);
            }
        }

        private void btn_RectanguloParam_Click(object sender, EventArgs e)
        {
            // Creamos la instancia "ofrm_Elipse" de la clase "frm_Elipse", el cual es un formulario que usaremos como emergente
            // para el ingreso de los parametros de la Elipse
            frm_Rectangulo ofrm_Rectangulo = new frm_Rectangulo();

            // Iniciamos el cuadro de Dialogo con el metodo "ShowDialog" el cual tambien devuelve un objeto de tipo "DialogResult"
            // que podemos usar para capturar la respuesta del usuario
            DialogResult dr_Rectangulo = ofrm_Rectangulo.ShowDialog();

            // Evaluamos la resspuesta del Cuadro de Dialogo (Formulario Emergente)
            if (dr_Rectangulo == DialogResult.OK)
            {
                // Creamos una instancia de la clase "Point" c
                Point op1 = new Point();
                Point op2 = new Point();
                Point op3 = new Point();
                Point op4 = new Point();

                // creamos las varialbes y asignamos los valores ingresados en el formulario emergente
                int largo = ofrm_Rectangulo.Largo;
                int alto = ofrm_Rectangulo.Alto;

                // Aqui Recuperamos los valores ingresados en el formulario emergente y los asignamos a las 
                // variables que usaremos como parametros para graficar la circunferencia
                // asignamos los valores X y Y a las propiedades de las variables "c" de clase "Point" ingresadas en el formulario "frm_Elipse"
                op1.X = ofrm_Rectangulo.C.X;
                op1.Y = ofrm_Rectangulo.C.Y;

                op2.X = op1.X + largo;
                op2.Y = op1.Y;

                op3.X = op2.X;
                op3.Y = op2.Y + alto;

                op4.X = op1.X;
                op4.Y = op1.Y + alto;

                // Pasamos pasamos los parametros para graficar la Rectangulo
                TrazarLineaDDA(op1, op2);
                TrazarLineaDDA(op2, op3);
                TrazarLineaDDA(op3, op4);
                TrazarLineaDDA(op4, op1);

            }
        }

        private void btn_CircunferenciaParam_Click(object sender, EventArgs e)
        {
            // Creamos la instancia "ofrm_Circunferencia" de la clase "frm_Circunferencia", el cual es un formulario que usaremos como emergente
            // para el ingreso de los parametros de la Circunferencia
            frm_Circunferencia ofrm_Circunferencia = new frm_Circunferencia();

            // Iniciamos el cuadro de Dialogo con el metodo "ShowDialog" el cual tambien devuelve un objeto de tipo "DialogResult"
            // que podemos usar para capturar la respuesta del usuario
            DialogResult dr_Circunferencia = ofrm_Circunferencia.ShowDialog();

            // Evaluamos la resspuesta del Cuadro de Dialogo (Formulario Emergente)
            if (dr_Circunferencia == DialogResult.OK)
            {
                // Creamos una instancia de la clase "Point" c
                Point c = new Point();

                // Aqui Recuperamos los valores ingresados en el formulario emergente y los asignamos a las 
                // variables que usaremos como parametros para graficar la circunferencia
                // asignamos los valores X y Y a las propiedades de las variables "c" de clase "Point" ingresadas en el formulario "frm_Circunferencia"

                c.X = ofrm_Circunferencia.C.X;
                c.Y = ofrm_Circunferencia.C.Y;

                // creamos las varialbes y asignamos los valores ingresados en el formulario emergente
                int radio = ofrm_Circunferencia.radio;
                int aInicio = ofrm_Circunferencia.aInicio;
                int aFin = ofrm_Circunferencia.aFin;

                // Pasamos pasamos los parametros para graficar la Circunferencia
                Circunferencia(c, radio, aInicio, aFin);
            }
        }

        private void btn_ElipseParam_Click(object sender, EventArgs e)
        {
            // Creamos la instancia "ofrm_Elipse" de la clase "frm_Elipse", el cual es un formulario que usaremos como emergente
            // para el ingreso de los parametros de la Elipse
            frm_Elipse ofrm_Elipse = new frm_Elipse();

            // Iniciamos el cuadro de Dialogo con el metodo "ShowDialog" el cual tambien devuelve un objeto de tipo "DialogResult"
            // que podemos usar para capturar la respuesta del usuario
            DialogResult dr_Circunferencia = ofrm_Elipse.ShowDialog();

            // Evaluamos la resspuesta del Cuadro de Dialogo (Formulario Emergente)
            if (dr_Circunferencia == DialogResult.OK)
            {
                // Creamos una instancia de la clase "Point" c
                Point c = new Point();

                // Aqui Recuperamos los valores ingresados en el formulario emergente y los asignamos a las 
                // variables que usaremos como parametros para graficar la circunferencia
                // asignamos los valores X y Y a las propiedades de las variables "c" de clase "Point" ingresadas en el formulario "frm_Elipse"
                c.X = ofrm_Elipse.C.X;
                c.Y = ofrm_Elipse.C.Y;

                // creamos las varialbes y asignamos los valores ingresados en el formulario emergente
                int radioX = ofrm_Elipse.radioX;
                int radioY = ofrm_Elipse.radioY;

                int aInicio = ofrm_Elipse.aInicio;
                int aFin = ofrm_Elipse.aFin;

                // Pasamos pasamos los parametros para graficar la Elipse
                Elipse(c, radioX, radioY, aInicio, aFin);
            }
        }

        private void btn_CilindroParam_Click(object sender, EventArgs e)
        {
            // Creamos la instancia "ofrm_Elipse" de la clase "frm_Elipse", el cual es un formulario que usaremos como emergente
            // para el ingreso de los parametros de la Elipse
            frm_Cilindro ofrm_Cilindro = new frm_Cilindro();

            // Iniciamos el cuadro de Dialogo con el metodo "ShowDialog" el cual tambien devuelve un objeto de tipo "DialogResult"
            // que podemos usar para capturar la respuesta del usuario
            DialogResult dr_Cilindro = ofrm_Cilindro.ShowDialog();

            // Evaluamos la resspuesta del Cuadro de Dialogo (Formulario Emergente)
            if (dr_Cilindro == DialogResult.OK)
            {
                // Creamos una instancia de la clase "Point" c
                Point c1 = new Point();
                Point c2 = new Point();

                Point op1 = new Point();
                Point op2 = new Point();

                Point op3 = new Point();
                Point op4 = new Point();

                // creamos las varialbes y asignamos los valores ingresados en el formulario emergente
                int radio = ofrm_Cilindro.radio;
                int alto = ofrm_Cilindro.alto;

                // Aqui Recuperamos los valores ingresados en el formulario emergente y los asignamos a las 
                // variables que usaremos como parametros para graficar la circunferencia
                // asignamos los valores X y Y a las propiedades de las variables "c" de clase "Point" ingresadas en el formulario "frm_Elipse"
                c1.X = ofrm_Cilindro.C.X;
                c1.Y = ofrm_Cilindro.C.Y;

                c2.X = c1.X;
                c2.Y = c1.Y + alto;

                op1.X = c1.X - radio;
                op1.Y = c1.Y;

                op3.X = c1.X + radio;
                op3.Y = c1.Y;

                op2.X = c1.X - radio;
                op2.Y = c1.Y + alto;

                op4.X = c1.X + radio;
                op4.Y = c1.Y + alto;

                Circunferencia(c1, radio, 180, 360);
                Circunferencia(c2, radio, 0, 360);

                TrazarLineaDDA(op1, op2);
                TrazarLineaDDA(op3, op4);

            }
        }

        private void btn_Figura01_Click(object sender, EventArgs e)
        {

            // Creamos la instancia "ofrm_Elipse" de la clase "frm_Elipse", el cual es un formulario que usaremos como emergente
            // para el ingreso de los parametros de la Elipse
            frm_Figura01 ofrm_Figura01 = new frm_Figura01();

            // Iniciamos el cuadro de Dialogo con el metodo "ShowDialog" el cual tambien devuelve un objeto de tipo "DialogResult"
            // que podemos usar para capturar la respuesta del usuario
            DialogResult dr_Figura01 = ofrm_Figura01.ShowDialog();

            // Evaluamos la resspuesta del Cuadro de Dialogo (Formulario Emergente)
            if (dr_Figura01 == DialogResult.OK)
            {
                //int Xc = 100;
                //int Yc = 100;
                //int lado = 100;

                int Xc = ofrm_Figura01.C.X;
                int Yc = ofrm_Figura01.C.Y;
                int lado = ofrm_Figura01.lado;

                int Xi = Xc - lado / 2;
                int Yi = Yc - lado / 2;

                int r = Convert.ToInt32((lado * Math.Sqrt(2)) / 2);
                int r2 = lado / 2;

                TrazarLineaDDA(new Point { X = Xi, Y = Yi }, new Point { X = Xi + lado, Y = Yi });
                TrazarLineaDDA(new Point { X = Xi + lado, Y = Yi }, new Point { X = Xi + lado, Y = Yi + lado });
                TrazarLineaDDA(new Point { X = Xi + lado, Y = Yi + lado }, new Point { X = Xi, Y = Yi + lado });
                TrazarLineaDDA(new Point { X = Xi, Y = Yi + lado }, new Point { X = Xi, Y = Yi });

                CircunferenciaDDA(new Point { X = Xc, Y = Yc }, r);
                CircunferenciaDDA(new Point { X = Xc, Y = Yc }, r2);
            }
        }

        private void btn_Figura02_Click(object sender, EventArgs e)
        {
            // Creamos la instancia "ofrm_Elipse" de la clase "frm_Elipse", el cual es un formulario que usaremos como emergente
            // para el ingreso de los parametros de la Elipse
            frm_Figura02 ofrm_Figura02 = new frm_Figura02();

            // Iniciamos el cuadro de Dialogo con el metodo "ShowDialog" el cual tambien devuelve un objeto de tipo "DialogResult"
            // que podemos usar para capturar la respuesta del usuario
            DialogResult dr_Figura02 = ofrm_Figura02.ShowDialog();

            // Evaluamos la resspuesta del Cuadro de Dialogo (Formulario Emergente)
            if (dr_Figura02 == DialogResult.OK)
            {
                //int X = 150;
                //int Y = 150;

                //int lado = 100;

                int X = ofrm_Figura02.C.X;
                int Y = ofrm_Figura02.C.Y;
                int lado = ofrm_Figura02.lado;

                int radio = lado / 2;

                Point c = new Point { X = X, Y = Y };

                Point p1 = new Point { X = X - radio, Y = Y - radio };
                Point p2 = new Point { X = p1.X + lado, Y = p1.Y };
                Point p3 = new Point { X = p2.X, Y = p2.Y + lado };
                Point p4 = new Point { X = p1.X, Y = p1.Y + lado };

                Point c1 = new Point { X = X, Y = Y + radio };
                Point c2 = new Point { X = X + radio, Y = Y };
                Point c3 = new Point { X = X - radio, Y = Y };
                Point c4 = new Point { X = X, Y = Y - radio };

                Circunferencia(c, radio, 0, 360);

                TrazarLineaDDA(p1, p2);
                TrazarLineaDDA(p2, p3);
                TrazarLineaDDA(p3, p4);
                TrazarLineaDDA(p1, p4);

                Circunferencia(c1, radio, 0, 180);

                Circunferencia(c2, radio, 0, 90);
                Circunferencia(c2, radio, 270, 360);

                Circunferencia(c3, radio, 90, 270);
                Circunferencia(c4, radio, 180, 360);
            }
        }

        private void btn_Figura03_Click(object sender, EventArgs e)
        {
            // Creamos la instancia "ofrm_Elipse" de la clase "frm_Elipse", el cual es un formulario que usaremos como emergente
            // para el ingreso de los parametros de la Elipse
            frm_Figura03 ofrm_Figura03 = new frm_Figura03();

            // Iniciamos el cuadro de Dialogo con el metodo "ShowDialog" el cual tambien devuelve un objeto de tipo "DialogResult"
            // que podemos usar para capturar la respuesta del usuario
            DialogResult dr_Figura03 = ofrm_Figura03.ShowDialog();

            // Evaluamos la resspuesta del Cuadro de Dialogo (Formulario Emergente)
            if (dr_Figura03 == DialogResult.OK)
            {
                //int X = 150;
                //int Y = 150;

                //int r = 10;

                int X = ofrm_Figura03.C.X;
                int Y = ofrm_Figura03.C.Y;
                int r = ofrm_Figura03.radio;

                //int X = 150;
                //int Y = 150;

                //int r = 10;

                Point c0 = new Point { X = X, Y = Y };

                Point c1 = new Point { X = X, Y = Y + r };
                Point c2 = new Point { X = X, Y = Y + 3 * r };
                Point c3 = new Point { X = X, Y = Y + 5 * r };

                Point c4 = new Point { X = X, Y = Y + 6 * r };

                Circunferencia(c0, r, 180, 360);

                Circunferencia(c1, r, 0, 360);
                Circunferencia(c2, r, 0, 360);
                Circunferencia(c3, r, 0, 360);

                Circunferencia(c4, r, 0, 180);

                Point p1 = new Point { X = X - r, Y = Y };
                Point p2 = new Point { X = X + r, Y = Y };

                Point p3 = new Point { X = p2.X, Y = p2.Y + 6 * r };
                Point p4 = new Point { X = p1.X, Y = p1.Y + 6 * r };

                TrazarLineaDDA(p1, p2);
                TrazarLineaDDA(p2, p3);
                TrazarLineaDDA(p3, p4);
                TrazarLineaDDA(p1, p4);
            }
        }

        private void btn_Figura04_Click(object sender, EventArgs e)
        {
            // Creamos la instancia "ofrm_Elipse" de la clase "frm_Elipse", el cual es un formulario que usaremos como emergente
            // para el ingreso de los parametros de la Elipse
            frm_Figura04 ofrm_Figura04 = new frm_Figura04();

            // Iniciamos el cuadro de Dialogo con el metodo "ShowDialog" el cual tambien devuelve un objeto de tipo "DialogResult"
            // que podemos usar para capturar la respuesta del usuario
            DialogResult dr_Figura04 = ofrm_Figura04.ShowDialog();

            // Evaluamos la resspuesta del Cuadro de Dialogo (Formulario Emergente)
            if (dr_Figura04 == DialogResult.OK)
            {
                //int X = 150;
                //int Y = 150;

                //int r = 10;

                int X = ofrm_Figura04.C.X;
                int Y = ofrm_Figura04.C.Y;
                int r = ofrm_Figura04.radio;

                Point c0 = new Point { X = X, Y = Y };

                Point c1 = new Point { X = X + r, Y = Y };
                Point c2 = new Point { X = X + 3 * r, Y = Y };
                Point c3 = new Point { X = X + 5 * r, Y = Y };

                Point c4 = new Point { X = X + 6 * r, Y = Y };

                Circunferencia(c0, r, 90, 270);


                Circunferencia(c1, r, 0, 360);
                Circunferencia(c2, r, 0, 360);
                Circunferencia(c3, r, 0, 360);

                Circunferencia(c4, r, 0, 90);
                Circunferencia(c4, r, 270, 360);

                Point p1 = new Point { X = X, Y = Y + r };
                Point p2 = new Point { X = X, Y = Y - r };

                Point p3 = new Point { X = p2.X + 6 * r, Y = p2.Y };
                Point p4 = new Point { X = p1.X + 6 * r, Y = p1.Y };

                TrazarLineaDDA(p1, p2);
                TrazarLineaDDA(p2, p3);
                TrazarLineaDDA(p3, p4);
                TrazarLineaDDA(p1, p4);
            }
        }
        #endregion


        private void btn_info_Click(object sender, EventArgs e)
        {
            frm_info ofrm_info = new frm_info();

            DialogResult dr_Circunferencia = ofrm_info.ShowDialog();
        }

        private void btn_Salir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_Limpiar_Click(object sender, EventArgs e)
        {

            pct_Dibujo.Image = null;
            bmp = new Bitmap(pct_Dibujo.Width, pct_Dibujo.Height);


            pct_Dibujo.Refresh();

            // Reiniciamos el contrador para los clicks
            nv_click = 0;
        }

        private void btn_Guardar_Click(object sender, EventArgs e)
        {
            sfd_Guardar.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp";
            sfd_Guardar.Title = "Guardar Dibujo";

            DialogResult dr_Guardar = sfd_Guardar.ShowDialog();

            if (dr_Guardar == DialogResult.OK)
            {
                Bitmap fondo = new Bitmap(bmp.Width, bmp.Height);
                Graphics g = Graphics.FromImage(fondo);





                g.FillRectangle(new SolidBrush(pct_Dibujo.BackColor), 0, 0, bmp.Width, bmp.Height);
                g.Dispose();

                Graphics g1 = Graphics.FromImage(fondo);

                //g1.TranslateTransform(0, pct_Dibujo.Height);
                //g1.ScaleTransform(1, -1);

                g1.DrawImage(bmp, 0, 0);


                bmp = new Bitmap(fondo);


                fondo.Dispose();
                g1.Dispose();

                switch (sfd_Guardar.FilterIndex)
                {
                    case 1:
                        bmp.Save(sfd_Guardar.FileName, ImageFormat.Jpeg);
                        break;

                    case 2:

                        bmp.Save(sfd_Guardar.FileName, ImageFormat.Bmp);
                        break;
                }
            }
        }

        #region Procedimientos

        private void TrazarLineaDDA(Point a, Point b)
        {
            iGrafics();

            Bitmap oImagen = new Bitmap(pct_Dibujo.Width, pct_Dibujo.Height, ov_grafico);

            int DX, DY;

            float nPasos;

            float nIncrementoX, nIncrementoY;
            float X, Y;

            DX = b.X - a.X;
            DY = b.Y - a.Y;


            if (Math.Abs(DX) > Math.Abs(DY))
                nPasos = Math.Abs(DX);
            else
                nPasos = Math.Abs(DY);

            nIncrementoX = DX / nPasos;
            nIncrementoY = DY / nPasos;

            X = a.X;
            Y = a.Y;

            ov_grafico.FillEllipse(ov_brocha, a.X, a.Y, nv_grosor, nv_grosor);
            ov_grafico_2.FillEllipse(ov_brocha, a.X, a.Y, nv_grosor, nv_grosor);

            for (int i = 1; i < nPasos; i++)
            {
                X = X + nIncrementoX;
                Y = Y + nIncrementoY;

                ov_grafico.FillEllipse(ov_brocha, Convert.ToInt32(X), Convert.ToInt32(Y), nv_grosor, nv_grosor);
                ov_grafico_2.FillEllipse(ov_brocha, Convert.ToInt32(X), Convert.ToInt32(Y), nv_grosor, nv_grosor);
            }

            ov_grafico.FillEllipse(ov_brocha, b.X, b.Y, nv_grosor, nv_grosor);
            ov_grafico_2.FillEllipse(ov_brocha, b.X, b.Y, nv_grosor, nv_grosor);

            ov_grafico.DrawImage(oImagen, 0, 0);
            ov_grafico_2.DrawImage(oImagen, 0, 0);

            fGrafics();
        }

        private void CircunferenciaDDA(Point c, int r)
        {
            int X, Y;
            double P;

            X = 0;
            Y = r;

            iGrafics();

            Bitmap oImagen = new Bitmap(pct_Dibujo.Width, pct_Dibujo.Height, ov_grafico);

            P = r;
            X = Convert.ToInt32(P);
            Y = 0;

            do
            {
                Trazar(oImagen, c.X, c.Y, X, Y);

                P = P - (Y / P);
                X = Convert.ToInt32(P);
                Y = Y + 1;
            }
            while (Y < X);

            ov_grafico.DrawImage(oImagen, 0, 0);
            ov_grafico_2.DrawImage(oImagen, 0, 0);

            fGrafics();
        }

        private void CircunferenciaPuntoMedio(Point c, int r)
        {
            int X, Y;
            int P;

            X = 0;
            Y = r;

            iGrafics();

            Bitmap oImagen = new Bitmap(pct_Dibujo.Width, pct_Dibujo.Height, ov_grafico);

            Trazar(oImagen, c.X, c.Y, X, Y);

            P = 1 - r;

            do
            {
                if (P < 0)
                {
                    X = X + 1;
                }
                else
                {
                    X = X + 1;
                    Y = Y - 1;
                }

                if (P < 0)
                    P = P + (X * 2) + 1;
                else
                    P = P + (X * 2) + 1 - (Y * 2);

                Trazar(oImagen, c.X, c.Y, X, Y);
            }
            while (X < Y);

            ov_grafico.DrawImage(oImagen, 0, 0);
            ov_grafico_2.DrawImage(oImagen, 0, 0);

            fGrafics();
        }

        private void ElipsePuntoMedio(Point c, int rx, int ry)
        {
            int X, Y;
            long CuadDistancia2X, CuadDistancia2Y;
            long P, Px, Py;

            CuadDistancia2X = 2 * (rx * rx);
            CuadDistancia2Y = 2 * (ry * ry);

            X = 0;
            Y = ry;

            iGrafics();

            Bitmap oImagen = new Bitmap(pct_Dibujo.Width, pct_Dibujo.Height, ov_grafico);

            TrazarElipse(oImagen, c.X, c.Y, X, Y);

            P = Convert.ToInt32((ry * ry) - (rx * rx) * ry + (0.25 * (rx * rx)));
            Px = 0;
            Py = CuadDistancia2X * Y;

            do
            {
                X = X + 1;
                Px = Px + CuadDistancia2Y;

                if (P >= 0)
                {
                    Y = Y - 1;
                    Py = Py - CuadDistancia2X;
                }

                if (P < 0)
                    P = P + (ry * ry) + Px;
                else
                    P = P + (ry * ry) + Px - Py;

                TrazarElipse(oImagen, c.X, c.Y, X, Y);
            }
            while (Px < Py);

            P = Convert.ToInt32((ry * ry) * (X + 0.5) * (X + 0.5) + (rx * rx) * (Y - 1) * (Y - 1) - (rx * rx) * (ry * ry));

            do
            {
                Y = Y - 1;
                Py = Py - CuadDistancia2X;

                if (P <= 0)
                {
                    X = X + 1;
                    Px = Px + CuadDistancia2Y;
                }

                if (P > 0)
                    P = P + (rx * rx) - Py;
                else
                    P = P + (rx * rx) - Py + Px;

                TrazarElipse(oImagen, c.X, c.Y, X, Y);
            }
            while (Y > 0);


            ov_grafico.DrawImage(oImagen, 0, 0);
            ov_grafico_2.DrawImage(oImagen, 0, 0);

            fGrafics();
        }

        private void Circunferencia(Point c, int r, int ai, int af)
        {
            int X, Y;

            iGrafics();

            Bitmap oImagen = new Bitmap(pct_Dibujo.Width, pct_Dibujo.Height, ov_grafico);
            double AnguloRadian;

            for (double i = ai; i <= af; i += 0.5)
            {
                AnguloRadian = (3.141592653 / 180) * i;

                X = Convert.ToInt32(c.X + r * Math.Cos(AnguloRadian));
                Y = Convert.ToInt32(c.Y + r * Math.Sin(AnguloRadian));

                if ((pct_Dibujo.Width - 3) >= (X)
                && pct_Dibujo.Height > (Y)
                && X >= 0
                && Y >= 0)
                {
                    ov_grafico.FillEllipse(ov_brocha, X, Y, nv_grosor, nv_grosor);
                    ov_grafico_2.FillEllipse(ov_brocha, X, Y, nv_grosor, nv_grosor);
                }
            }


            ov_grafico.DrawImage(oImagen, 0, 0);
            ov_grafico_2.DrawImage(oImagen, 0, 0);
            fGrafics();
        }

        private void Elipse(Point c, int rx, int ry, int ai, int af)
        {
            int X, Y;

            iGrafics();

            Bitmap oImagen = new Bitmap(pct_Dibujo.Width, pct_Dibujo.Height, ov_grafico);
            double AnguloRadian;

            for (double i = ai; i <= af; i += 0.5)
            {
                AnguloRadian = (3.141592653 / 180) * i;

                X = Convert.ToInt32(c.X + rx * Math.Cos(AnguloRadian));
                Y = Convert.ToInt32(c.Y + ry * Math.Sin(AnguloRadian));

                if ((pct_Dibujo.Width - 3) >= (X)
                && pct_Dibujo.Height > (Y)
                && X >= 0
                && Y >= 0)
                {
                    ov_grafico.FillEllipse(ov_brocha, X, Y, nv_grosor, nv_grosor);
                    ov_grafico_2.FillEllipse(ov_brocha, X, Y, nv_grosor, nv_grosor);
                }
            }


            ov_grafico.DrawImage(oImagen, 0, 0);
            ov_grafico_2.DrawImage(oImagen, 0, 0);
            fGrafics();
        }

        private void TrazarElipse(Bitmap img, int Xc, int Yc, int X, int Y)
        {
            if ((pct_Dibujo.Width - 3) >= (Xc + X)
                && pct_Dibujo.Height > (Yc + Y)
                && (Xc + X) >= 0
                && ((Yc + Y)) >= 0)
            {
                ov_grafico.FillEllipse(ov_brocha, Xc + X, Yc + Y, nv_grosor, nv_grosor);
                ov_grafico_2.FillEllipse(ov_brocha, Xc + X, Yc + Y, nv_grosor, nv_grosor);
            }

            if ((pct_Dibujo.Width - 3) >= (Xc - X)
                && pct_Dibujo.Height > (Yc + Y)
                && (Xc - X) >= 0
                && (Yc + Y) >= 0)
            {
                ov_grafico.FillEllipse(ov_brocha, Xc - X, Yc + Y, nv_grosor, nv_grosor);
                ov_grafico_2.FillEllipse(ov_brocha, Xc - X, Yc + Y, nv_grosor, nv_grosor);
            }

            if ((pct_Dibujo.Width - 3) >= (Xc - X)
                && pct_Dibujo.Height > (Yc - Y)
                && (Xc - X) >= 0
                && (Yc - Y) >= 0)
            {
                ov_grafico.FillEllipse(ov_brocha, Xc - X, Yc - Y, nv_grosor, nv_grosor);
                ov_grafico_2.FillEllipse(ov_brocha, Xc - X, Yc - Y, nv_grosor, nv_grosor);
            }

            if ((pct_Dibujo.Width - 3) >= (Xc + X)
                && pct_Dibujo.Height > (Yc - Y)
                && (Xc + X) >= 0
                && (Yc - Y) >= 0)
            {
                ov_grafico.FillEllipse(ov_brocha, Xc + X, Yc - Y, nv_grosor, nv_grosor);
                ov_grafico_2.FillEllipse(ov_brocha, Xc + X, Yc - Y, nv_grosor, nv_grosor);
            }

        }

        private void Trazar(Bitmap img, int Xc, int Yc, int X, int Y)
        {
            if ((pct_Dibujo.Width - 3) >= (Xc + X)
                && pct_Dibujo.Height > (Yc + Y)
                && (Xc + X) >= 0
                && ((Yc + Y)) >= 0)
            {
                ov_grafico.FillEllipse(ov_brocha, Xc + X, Yc + Y, nv_grosor, nv_grosor);
                ov_grafico_2.FillEllipse(ov_brocha, Xc + X, Yc + Y, nv_grosor, nv_grosor);
            }

            if ((pct_Dibujo.Width - 3) >= (Xc + Y)
                && pct_Dibujo.Height > (Yc + X)
                && (Xc + Y) >= 0
                && (Yc + X) >= 0)
            {
                ov_grafico.FillEllipse(ov_brocha, Xc + Y, Yc + X, nv_grosor, nv_grosor);
                ov_grafico_2.FillEllipse(ov_brocha, Xc + Y, Yc + X, nv_grosor, nv_grosor);
            }

            if ((pct_Dibujo.Width - 3) >= (Xc - X)
                && pct_Dibujo.Height > (Yc + Y)
                && (Xc - X) >= 0
                && (Yc + Y) >= 0)
            {
                ov_grafico.FillEllipse(ov_brocha, Xc - X, Yc + Y, nv_grosor, nv_grosor);
                ov_grafico_2.FillEllipse(ov_brocha, Xc - X, Yc + Y, nv_grosor, nv_grosor);
            }

            if ((pct_Dibujo.Width - 3) >= (Xc - Y)
                && pct_Dibujo.Height > (Yc + X)
                && (Xc - Y) >= 0
                && (Yc + X) >= 0)
            {
                ov_grafico.FillEllipse(ov_brocha, Xc - Y, Yc + X, nv_grosor, nv_grosor);
                ov_grafico_2.FillEllipse(ov_brocha, Xc - Y, Yc + X, nv_grosor, nv_grosor);
            }

            if ((pct_Dibujo.Width - 3) >= (Xc - X)
                && pct_Dibujo.Height > (Yc - Y)
                && (Xc - X) >= 0
                && (Yc - Y) >= 0)
            {
                ov_grafico.FillEllipse(ov_brocha, Xc - X, Yc - Y, nv_grosor, nv_grosor);
                ov_grafico_2.FillEllipse(ov_brocha, Xc - X, Yc - Y, nv_grosor, nv_grosor);
            }

            if ((pct_Dibujo.Width - 3) >= (Xc - Y)
                && pct_Dibujo.Height > (Yc - X)
                && (Xc - Y) >= 0
                && (Yc - X) >= 0)
            {
                ov_grafico.FillEllipse(ov_brocha, Xc - Y, Yc - X, nv_grosor, nv_grosor);
                ov_grafico_2.FillEllipse(ov_brocha, Xc - Y, Yc - X, nv_grosor, nv_grosor);
            }

            if ((pct_Dibujo.Width - 3) >= (Xc + X)
                && pct_Dibujo.Height > (Yc - Y)
                && (Xc + X) >= 0
                && (Yc - Y) >= 0)
            {
                ov_grafico.FillEllipse(ov_brocha, Xc + X, Yc - Y, nv_grosor, nv_grosor);
                ov_grafico_2.FillEllipse(ov_brocha, Xc + X, Yc - Y, nv_grosor, nv_grosor);
            }

            if ((pct_Dibujo.Width - 3) >= (Xc + Y)
                && pct_Dibujo.Height > (Yc - X)
                && (Xc + Y) >= 0
                && (Yc - X) >= 0)
            {
                ov_grafico.FillEllipse(ov_brocha, Xc + Y, Yc - X, nv_grosor, nv_grosor);
                ov_grafico_2.FillEllipse(ov_brocha, Xc + Y, Yc - X, nv_grosor, nv_grosor);
            }

        }

        private void agregarPunto(Point punto)
        {
            //agrega los puntos obtenidos al arreglo
            Point[] temp = new Point[puntos.Length + 1];
            puntos.CopyTo(temp, 0);
            puntos = temp;
            puntos[puntos.Length - 1] = punto;
        }

        private void iGrafics()
        {
            ov_grafico = pct_Dibujo.CreateGraphics();
            ov_grafico.TranslateTransform(0, pct_Dibujo.Height);
            ov_grafico.ScaleTransform(1, -1);
            //ov_grafico.ScaleTransform(1, 1);

            ov_grafico_2 = Graphics.FromImage(bmp);
        }

        private void fGrafics()
        {
            ov_grafico.Dispose();
            ov_grafico_2.Dispose();
        }

        private void CrearGoma()
        {
            int diametroG = Convert.ToInt32(goma.Width);
            Bitmap Goma = new Bitmap(diametroG, diametroG);

            Graphics gGoma = Graphics.FromImage(Goma);
            gGoma.FillRectangle(Brushes.Magenta, 0, 0, diametroG, diametroG);
            gGoma.FillEllipse(Brushes.White, 0, 0, diametroG - 1, diametroG - 1);
            gGoma.DrawEllipse(new Pen(Color.Black, 1), 0, 0, diametroG - 1, diametroG - 1);

            Goma.MakeTransparent(Color.Magenta);

            gGoma.Dispose();

            IntPtr intprCursorGoma = Goma.GetHicon();
            pct_Dibujo.Cursor = new Cursor(intprCursorGoma);
        }

        private void IconoLapiz()
        {
            IntPtr intprCursor = Properties.Resources.Lapiz.GetHicon();
            pct_Dibujo.Cursor = new Cursor(intprCursor);
        }

        #endregion

    }
}
