using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Компьютерная_графика
{
    public class ThreeDPoint:ICloneable
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public const int HelpVar = 1;
        public ThreeDPoint():base()
        {
        }
        public ThreeDPoint(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    public class Edge
    {
        public ThreeDPoint Point1 { get; set; }
        public ThreeDPoint Point2 { get; set; }
        public Edge(ThreeDPoint Point1, ThreeDPoint Point2)
        {
            this.Point1 = Point1;
            this.Point2 = Point2;
        }

    }
    public class BoundBindingException:Exception//Для использования в конструкторе Bound
    {
        public BoundBindingException(string msg):base(msg)//Наследование базового конструктора от Exception
        {
        }
    }
    /// <summary>
    /// Грань многогранника
    /// </summary>
    public class Bound//Грань
    {
        List<Edge> Edges = new List<Edge>();
        public Edge getEdge(int i)
        {
            return Edges[i];
        }
        public void AddEdge(Edge e)
        {
            Edges.Add(e);
        }
        public Bound():base()
        {
        }
        /// <summary>
        /// Задание грани с помощью рёбер
        /// </summary>
        /// <param name="v">массив рёбер, или рёбра через запятую</param>
        /// <exception cref="BoundBindingException">Возникает если число рёбер меньше 3</exception>"
        public Bound(List<Edge> v)//Задание грани рёбрами
        {
            /*if (v.Count <= 2)
            {
                throw new BoundBindingException("Incorrect input of bound");
            }
            for (int i = 0; i < v.Count; ++i)
            {
                Edges.Add(v[i]);
            }*/
            Edges = v;
        }
    }
    public class PolyHedron
    {
        List<ThreeDPoint> Points = new List<ThreeDPoint>();
        List<Edge> Edges = new List<Edge>();
        List<Bound> Bounds = new List<Bound>();
        public List<ThreeDPoint> getPoints()
        {
            return Points;
        }
        public List<Edge> getEdges()
        {
            return Edges;
        }
        public List<Bound> getBounds()
        {
            return Bounds;
        }
        //public static PolyHedron polyHedron = new PolyHedron("input.vtk");
        /// <summary>
        /// Reads PolyHedron points from file with path location
        /// </summary>
        /// <param name="path">location of file</param>
        public PolyHedron(string path)//Доделывать
        {
            readPoints(path);
        }
        /// <summary>
        /// Reads points coords(x,y,z) from path. Add this points to List
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <exception cref="BoundBindingException">thrown if didn't find anchors in file like Points or Polygons</exception>
        void readPoints(string path)
        {
            ThreeDPoint pt = new ThreeDPoint();
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            string Point="";
            string Polygon;
            const string Anchor1 = "POINTS";
            const string Anchor2 = "POLYGONS";
            int N;//stores quantity of Anchor i entity to read (count of lines)
            string[] PointCoordinates;//stores separated line
            double shift = 0.4;
            if (ShiftLines(Anchor1, file, out N) == 0)
            {
                while (N > 0 && (Point = file.ReadLine()) != null)
                {
                    double x, y, z;//Переменная для использования в tryparse. В ней нельзя юзать ссылочную переменную с out
                    PointCoordinates = Point.Split(new char[] { ' ', '\t', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    ThreeDPoint tmp = new ThreeDPoint();
                    double.TryParse(PointCoordinates[0].Replace(".",","), out x);
                    tmp.x = x+shift;
                    double.TryParse(PointCoordinates[1].Replace(".", ","), out y);
                    tmp.y = y+shift;
                    double.TryParse(PointCoordinates[2].Replace(".", ","), out z);
                    tmp.z = z+shift;
                    Points.Add(tmp);
                    --N;
                }
                if (Point == null) { throw new BoundBindingException("Error of "+Anchor1+" format in file" + path); }
                if (ShiftLines(Anchor2, file, out N) == 0)
                {
                    
                    while (N > 0 && (Point = file.ReadLine()) != null)
                    {
                        Bound tmpBound = new Bound();
                        int x, y;//Переменная для использования в tryparse. В ней нельзя юзать ссылочную переменную с out. Хранят индексы точки для задания ребра
                        PointCoordinates = Point.Split(new char[] { ' ', '\t', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                        int PointsPerBound;
                        int.TryParse(PointCoordinates[0], out PointsPerBound);
                        if (PointCoordinates.Length-1 != PointsPerBound)
                        {
                            throw new BoundBindingException("Error of " + Anchor2 + " format in file " + path);
                        }
                        List<Edge> tmpEdges = new List<Edge>();//List of edges forming 1 bound
                        //Forming list of edges of 1 bound
                        for (int i = 1; i < PointsPerBound; ++i)//
                        {
                            if (!int.TryParse(PointCoordinates[i], out x) || !int.TryParse(PointCoordinates[i + 1], out y))
                            {
                                throw new BoundBindingException("Error of " + Anchor2 + " format in file" + path);
                            }
                            Edge e = new Edge(Points[x], Points[y]);
                            tmpBound.AddEdge(NewEdge(e));
                        }
                        //Замыкаем ребром последнюю и первую точки грани
                        tmpBound.AddEdge(NewEdge(new Edge(Points[int.Parse(PointCoordinates[PointsPerBound])],Points[int.Parse(PointCoordinates[1])])));
                        Bounds.Add(tmpBound);//Adding current bound to list of bounds
                        --N;
                    }
                }
                else throw new BoundBindingException("Error reading "+Anchor2+" of PolyHedron from file " + path);
            }
            else throw new BoundBindingException("Error reading "+Anchor1+" of PolyHedron from file "+path);
        }
        /// <summary>
        /// Returns pointer to the Edge. If the edge wasn't found in list of already added in list edges it creates and returns new Edge. Otherwise it returns pointer to input edge
        /// </summary>
        /// <param name="e">created outdoors edge</param>
        Edge NewEdge(Edge e)
        {
            //bool flag = false;
            for (int i = 0; i < Edges.Count /*&& !flag*/; ++i)
            {
                if (e.Point1 == Edges[i].Point1 && e.Point2 == Edges[i].Point2 || e.Point1 == Edges[i].Point2 && e.Point2 == Edges[i].Point1)
                {
                    //flag = true;
                    return e = Edges[i];
                }
            }
            Edges.Add(e);
            return e;
        }
        /// <summary>
        /// Used in readPoints
        /// Shifts text till the Anchor 1st word in line. If found writes 2nd value (quantity of Anchor) to PointsCount
        /// </summary>
        /// <param name="Anchor">Element name to search as 1st word in line</param>
        /// <param name="file">StreamReader which to continue to shift</param>
        /// <param name="Count">Quantity of Anchor elements</param>
        /// <returns>0 if shifted successfully\n-1 if shifted till the end of file without success</returns>
        int ShiftLines(string Anchor, System.IO.StreamReader file, out int Count)
        {
            string[] PointCoordinates;//stores separated line
            string Point;//stores entre line 
            while ((Point = file.ReadLine()) != null)
            {
                PointCoordinates = Point.Split(new char[] { ' ', '\t', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (string.Compare(PointCoordinates[0], Anchor) == 0)
                {
                    int.TryParse(PointCoordinates[1], out Count);
                    return 0;//all is ok. Unnesseccary text is shifted before end of file
                }
            }
            Count = 0;
            return -1;//Not ok. Shifted till the end of file
        }
        //Можно в будущем этот метод переделать с условием if внутри for. Где будет проверятся ребро на видимость
        public void Draw(System.Drawing.Graphics g,float scale,PointF center)
        {
            System.Drawing.Pen p=new System.Drawing.Pen(System.Drawing.Color.Blue,2);
            for (int i=0;i<Edges.Count;++i){
                PointF P1=new PointF((float)Edges[i].Point1.x,(float)Edges[i].Point1.y);//Можно подправить мой класс Point с double на float
                PointF P2=new PointF((float)Edges[i].Point2.x,(float)Edges[i].Point2.y);
                P1.X = P1.X + center.X;//shift to center and stretch for scale coefficient 
                P1.Y = P1.Y + center.Y;
                P2.X = P2.X + center.X;
                P2.Y = P2.Y + center.Y;
                g.DrawLine(p,P1,P2);
            }
            p.Dispose();
        }
        public void EdgeToRastr()
        {
            List<ThreeDPoint[]> EdgesPoints = new List<ThreeDPoint[]>(Edges.Count);
            for (int i = 0; i < Edges.Count; ++i)
            {
                int xmin=(int)Math.Round(Edges[i].Point1.x);
                int xmax = (int)Math.Round(Edges[i].Point2.x);
                int ymin=(int)Math.Round(Edges[i].Point1.y);
                int ymax=(int)Math.Round(Edges[i].Point2.y);
                int tmp;
                int sign=1;
                if (xmin>xmax){
                    tmp=xmin;
                    xmin=xmax;
                    xmax=tmp;
                    sign=-1;
                }
                if (ymin>ymax){
                    tmp=ymin;
                    ymin=ymax;
                    ymax=tmp;
                }
                if (xmin==xmax){
                    EdgesPoints[i]=new ThreeDPoint[ymax-ymin+1];
                    for (int j=ymin;j<ymax;++j){
                        EdgesPoints[i][j].x=xmin;
                        EdgesPoints[i][j].y=j;
                        EdgesPoints[i][j].z=Edges[i].Point2.z-Edges[i].Point1 Edges[i].Point2.y-Edges[i].Point1.y sign*Math.Abs((Edges[i].Point1.y-Edges[i].Point2.y));
                    }

                }
                for (int j=xmin;j)

            }
        }
        /*void toRastr()
        {
            f
        }*/
        /*
        //Перенести этот метод в клас какой нить где будут храниться и создаваться объекты находящиеся на форме (для прорисовки)
        public void DrawAxises()
        {
            
        }*/

    }
}
