using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Room_planner.Elements
{
    class Furniture : IElement
    {
        Dictionary<string, Image> Resources = new Dictionary<string, Image>
        {
            {"coffee", Properties.Resources.coffee_table },
            {"table", Properties.Resources.table },
            {"sofa", Properties.Resources.sofa },
            {"bed", Properties.Resources.double_bed }
        };

        Dictionary<string, Image> TransparentResources = new Dictionary<string, Image>()
        {
             {"coffee", Properties.Resources.coffee_table.SetOpacity(0.5f) },
            {"table", Properties.Resources.table.SetOpacity(0.5f) },
            {"sofa", Properties.Resources.sofa.SetOpacity(0.5f) },
            {"bed", Properties.Resources.double_bed.SetOpacity(0.5f) }
        };
        public int Id { get; set; }

        public string Name { get; set; }
        public Image Resource { get; set; }

        public Image Semitransparent { get; set; }

        public Image Rotated { get; set; }
        public Image RotatedTransparent { get; set; }
        public Point Position { get; set; }
        private ResourceManager resourceManager = null;
        private float r = 0;
        public float Rotation
        {
            get => r;
            set
            {
                r = value;
                needToRotate = true;
            }
        }
        private bool needToRotate { get; set; }
        public Furniture(string name, Point p)
        {
            Resource = Resources[name];
            Semitransparent = TransparentResources[name];
            Position = new Point(p.X - Resource.Width / 2, p.Y - Resource.Height / 2);
            Name = name;
            needToRotate = true;
            resourceManager = new ResourceManager("Room_planner.Form1",
               Assembly.GetExecutingAssembly());
        }
        public void Draw(Graphics gfx, bool selected)
        {
            var tmp = new Bitmap(Resource.Width + 74, Resource.Height + 74);
            using (var g = Graphics.FromImage(tmp))
            {
                if (needToRotate)
                {
                    Rotated = Resource.Rotate(Rotation);
                    RotatedTransparent = Semitransparent.Rotate(Rotation);
                    needToRotate = false;
                }
                if (selected)
                {
                    g.DrawImage(RotatedTransparent, new Point(0, 0));
                }
                else
                {
                    g.DrawImage(Rotated, new Point(0, 0));
                }

            }
            Point p = new Point(Position.X - 37, Position.Y - 37);
            gfx.DrawImage(tmp, p);
        }

        public override string ToString()
        {
            return $"{resourceManager.GetString(Name)}:{{X={Position.X + Resource.Width / 2},Y={Position.Y + Resource.Height / 2}}}";
        }

        public bool WasHit(Point p)
        {
            if (Position.X + Resource.Width >= p.X
                && Position.X <= p.X
                && Position.Y + Resource.Height >= p.Y
                && Position.Y <= p.Y)
            {
                return true;
            }
            return false;
        }

        public void UpdatePosition(int x, int y)
        {
            Position = new Point(Position.X + x, Position.Y + y);
        }
        public void Save(StreamWriter file)
        {
            file.WriteLine($"{Id} {Name} {Position.X} {Position.Y} {Rotation}");
        }

       
    }
}
