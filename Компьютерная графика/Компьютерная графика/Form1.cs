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
            float scale=100;
            PointF center=new PointF(panel1.Width/2,panel1.Height/2);
            polyHedron.Draw(g,scale,center);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AfinPreobr.Mx(polyHedron.getPoints());
            panel1.Invalidate();
        }
    }
}
