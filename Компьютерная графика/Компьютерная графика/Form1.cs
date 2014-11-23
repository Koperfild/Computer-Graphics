using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot;

namespace Компьютерная_графика
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //DrawAxis(panel1);
            //panel1.CreateGraphics().DrawLine(new Pen(Color.Black, 2), new Point(33, 33),new Point(66,66));
            //panel1.Invalidate();
            /*Plot = new OxyPlot.WindowsForms.PlotView();
            Plot.Model = new PlotModel();
            PlotController myController = new PlotController();
            Plot.Controller = myController;
            //Plot.Location=new Point(15,15);
            //Plot.Dock = DockStyle.None;
            Plot.Size = new Size(550, 350);
            //Plot.Dock = DockStyle.Fill;
            this.Controls.Add(Plot);

            Plot.Model.PlotType = PlotType.XY;
            Plot.Model.Background = OxyColor.FromRgb(255, 255, 255);
            Plot.Model.TextColor = OxyColor.FromRgb(0, 0, 0);
            //Plot.Model.Series.Add(lineSeries);
            Plot.Model.Axes.Add(new OxyPlot.Axes.LinearAxis(OxyPlot.Axes.AxisPosition.Bottom, -5.0, 5.0));
            Plot.Model.Axes.Add(new OxyPlot.Axes.LinearAxis(OxyPlot.Axes.AxisPosition.Left, -5.0, 5.0));*/
        }
        private void DrawAxis(System.Windows.Forms.Panel panel)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //System.Drawing.Graphics g = panel.CreateGraphics();
            System.Drawing.Pen p = new Pen(Color.Black, 1);
            //Ось X
            Graphics g = panel1.CreateGraphics();
            g.DrawLine(p, new PointF(0, (int)panel1.Height / 2), new PointF((int)panel1.Width, (int)panel1.Height / 2));
            //Ось Y
            g.DrawLine(p, new PointF(panel1.Width / 2, 0), new PointF(panel1.Width / 2, panel1.Height));
            //panel.Invalidate();
            //float scale = 100;
            PointF center = new PointF(panel1.Width / 2, panel1.Height / 2);
            polyHedron.Draw(g, center);
            p.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AfinPreobr.Rx(Convert.ToDouble(numericUpDown7.Value), polyHedron.getPoints());
            if (Z_buffer_Was_Used)
            {
                polyHedron.Z_Buffer();
            }
            panel1.Invalidate();
            //panel1_Paint(new object sender,new PaintEventArgs() e);
        }
        /*private void Z_Buffer()
        {
            float[,] M = new float[3, 4];//Coefficient matrix for LinearEquation by Gauss method
            ThreeDPoint[] tmpPoint = new ThreeDPoint[3];
            for (int i = 0; i < polyHedron.getBounds().Count; ++i)
            {
                for (int j = 0; j < tmpPoint.Length; ++j)
                {
                    //Filling M matrix with coefficients (x,y,z)
                    tmpPoint[j] = polyHedron.getBounds()[i].getEdge(j).Point1;//As bound has at least 3 edges I don't care about different points, just take 1st of each edge
                }

                //LinearEquationSolver.Solve(M);
                float A, B, C, D;
                A = (float)(tmpPoint[0].y * (tmpPoint[1].z - tmpPoint[2].z) + tmpPoint[1].y * (tmpPoint[2].z - tmpPoint[0].z) + tmpPoint[2].y * (tmpPoint[0].z - tmpPoint[1].z));
                B = (float)(tmpPoint[0].z * (tmpPoint[1].x - tmpPoint[2].x) + tmpPoint[1].z * (tmpPoint[2].x - tmpPoint[0].x) + tmpPoint[2].z * (tmpPoint[0].x - tmpPoint[1].x));
                C = (float)(tmpPoint[0].x * (tmpPoint[1].y - tmpPoint[2].y) + tmpPoint[1].x * (tmpPoint[2].y - tmpPoint[0].y) + tmpPoint[1].x * (tmpPoint[0].y - tmpPoint[1].y));
                D = (float)(-(tmpPoint[0].x * (tmpPoint[1].y * tmpPoint[2].z - tmpPoint[2].y * tmpPoint[1].z) + tmpPoint[1].x * (tmpPoint[2].y * tmpPoint[0].z - tmpPoint[0].y * tmpPoint[2].z) + tmpPoint[2].x * (tmpPoint[0].y * tmpPoint[1].z - tmpPoint[1].y * tmpPoint[0].z)));
                /*for (int k = 0; k < panel1.Width; ++k)
                {
                    Z = Z(x, y);//Вставлять итерационную формулу и перед ней расчёт z[0]. Перед всем этим создать Z Buffer и заполнить максимальными/минимальными значениями. Далее сравнивать z(x,y) с этими значениями и обновлять Z Buffer             }
                }*/
            //}
        //}

        private void button8_Click(object sender, EventArgs e)
        {
            AfinPreobr.T(Convert.ToDouble(numericUpDown4.Value), Convert.ToDouble(numericUpDown5.Value), Convert.ToDouble(numericUpDown6.Value), polyHedron.getPoints());
            panel1.Invalidate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AfinPreobr.D(Convert.ToDouble(numericUpDown3.Value), Convert.ToDouble(numericUpDown2.Value), Convert.ToDouble(numericUpDown1.Value), polyHedron.getPoints());
            panel1.Invalidate();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            polyHedron.Z_Buffer();
            Z_buffer_Was_Used = true;
            panel1.Invalidate();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AfinPreobr.Mx(polyHedron.getPoints());
            panel1.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AfinPreobr.Ry(Convert.ToDouble(numericUpDown8.Value), polyHedron.getPoints());
            if (Z_buffer_Was_Used)
            {
                polyHedron.Z_Buffer();
            }
            panel1.Invalidate();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            AfinPreobr.Rz(Convert.ToDouble(numericUpDown9.Value), polyHedron.getPoints());
            if (Z_buffer_Was_Used)
            {
                polyHedron.Z_Buffer();
            }
            panel1.Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AfinPreobr.D(1/Convert.ToDouble(numericUpDown3.Value), 1/Convert.ToDouble(numericUpDown2.Value), 1/Convert.ToDouble(numericUpDown1.Value), polyHedron.getPoints());
            panel1.Invalidate();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            AfinPreobr.T(-Convert.ToDouble(numericUpDown4.Value), -Convert.ToDouble(numericUpDown5.Value), -Convert.ToDouble(numericUpDown6.Value), polyHedron.getPoints());
            panel1.Invalidate();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AfinPreobr.My(polyHedron.getPoints());
            panel1.Invalidate();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AfinPreobr.Mz(polyHedron.getPoints());
            panel1.Invalidate();
        }
    }
}
