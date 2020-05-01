using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;


namespace Room_planner.Elements
{
    class Wall : IElement
    {
        public int Id { get; set; }
        public string Name { get; set; } = "wall";
        public Image Resource { get; set; }

        public Image Semitransparent { get; set; }
        public Point Position { get; set ; }
        public List<Point> Endpoints { get; set; }

        private float _rotation = 0;
        private float _prev_rotation = 0;

        private ResourceManager resourcemanager = null;
        public float Rotation
        {
            get => _rotation;
            set
            {
                _prev_rotation = _rotation;
                _rotation = value - _prev_rotation;
                //double angleX = Math.PI * _rotation / 180.0;
                //double sin = Math.Sin(angleX);
                //double cos = Math.Cos(angleX);
                for (int i = 1; i < Endpoints.Count;i++)
                {
                    Endpoints[i] = Rotate(Endpoints[0], Endpoints[i], Rotation);
                }
            } 
        }

        public static Point Rotate(Point center, Point point, double angle)
        {
            
            return
                new Point
                {
                    X = (int)((point.X - center.X) * Math.Cos(angle * Math.PI / 180) - (point.Y - center.Y) * Math.Sin(angle * Math.PI / 180) + center.X),
                    Y = (int)((point.X - center.X) * Math.Sin(angle * Math.PI / 180) + (point.Y - center.Y) * Math.Cos(angle * Math.PI / 180) + center.Y)
                };
        }

        public Wall(Point p)
        {
            Position = p;
            Endpoints = new List<Point>();
            AddEndpoint(p);
            resourcemanager = new ResourceManager("Room_planner.Form1",
                Assembly.GetExecutingAssembly());
        }

        public void AddEndpoint(Point p)
        {            
            Endpoints.Add(p);
        }

        public void Draw(Graphics gfx, bool selected)
        {
            Pen pen = selected ? new Pen(Color.FromArgb(255 / 2, 0, 0, 0), 10) : new Pen(Color.Black,10);
            pen.Alignment = PenAlignment.Center;
            var path = new GraphicsPath();
            path.StartFigure();
            for (int i = 1; i < Endpoints.Count; i++)
            {
                path.AddLine(Endpoints[i - 1], Endpoints[i]);
            }
            pen.LineJoin = LineJoin.Bevel;
            gfx.DrawPath(pen, path);
            pen.Dispose();
            path.Dispose();
        }

        public void DrawTemp(Graphics gfx, Point p)
        {
            Pen pen =  new Pen(Color.Black, 10);
            pen.Alignment = PenAlignment.Center;
            var path = new GraphicsPath();
            path.StartFigure();
            for (int i = 1; i < Endpoints.Count; i++)
            {
                path.AddLine(Endpoints[i - 1], Endpoints[i]);
            }
            path.AddLine(Endpoints.Last(), p);
            pen.LineJoin = LineJoin.Bevel;
            gfx.DrawPath(pen, path);
            pen.Dispose();
            path.Dispose();
        }

        public bool WasHit(Point p)
        {
            bool result = false;
            for (int i = 1; i < Endpoints.Count; i++)
            {
                result = PointOnLine(Endpoints[i - 1], Endpoints[i], p);
                if (result)
                {
                    break;
                }
            }
            return result;
        }
        bool PointOnLine(Point p1, Point p2, Point p)
        {
            using (var path = new GraphicsPath())
            {
                using (var pen = new Pen(Brushes.Black, 10))
                {
                    pen.Alignment = PenAlignment.Center;
                    pen.LineJoin = LineJoin.Bevel;
                    path.AddLine(p1, p2);
                    return path.IsOutlineVisible(p, pen);
                }
            }
        }

        public void UpdatePosition(int x, int y)
        {
            Position = new Point(Position.X + x, Position.Y + y);
            for(int i = 0; i < Endpoints.Count; i++)
            {
                Endpoints[i] = new Point(Endpoints[i].X + x, Endpoints[i].Y + y);
            }
        }

        public override string ToString()
        {
            return $"{resourcemanager.GetString(Name)}:{{X={Endpoints[0].X},Y={Endpoints[0].X}}}";
        }

        public void Save(StreamWriter file)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var endPoint in Endpoints)
            {
                sb.Append(endPoint.X);
                sb.Append(" ");
                sb.Append(endPoint.Y);
                sb.Append(" ");
            }
            file.WriteLine($"{Id} {Name} {Rotation} {Endpoints.Count} {sb}");
        }
    }
}
