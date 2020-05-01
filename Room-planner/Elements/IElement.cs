using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room_planner.Elements
{
    interface IElement
    {
        int Id { get; set; }

        float Rotation { get; set; }

        string Name { get; set; }

        Image Resource {get;set;}
        Image Semitransparent { get; set; }

        Point Position { get; set; }

        void Draw(Graphics gfx, bool selected);

        string ToString();

        bool WasHit(Point p);

        void UpdatePosition(int x, int y);

        void Save(System.IO.StreamWriter file);
    }
}
