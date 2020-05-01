using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.XPath;
using Room_planner.Elements;

namespace Room_planner
{
    class Blueprint
    {
        public int count { get; set; } = 0;
 
        public Bitmap Canvas { get; set; } 
        public Bitmap Active { get; set; }

        public IElement Selected { get; set; }
        public Size Size { get; set; }
        public List<IElement> Elements { get; set; }
        public Blueprint(Size size)
        {
            Size = size;
            Canvas = new Bitmap(size.Width, size.Height);
            Elements = new List<IElement>();
        }
        public void AddElement(string name, Point p)
       {
            if(name!="wall")
            {
                Elements.Add(new Furniture(name, p));
                using (var gfx = Graphics.FromImage(Canvas))
                {
                    Elements.Last().Draw(gfx, false);
                }
            }
            else
            {
                Elements.Add(new Wall(p));
            }
            Elements.Last().Id = count;
            count++;
       }

        public void UpdateSize(Size size)
        {
            Size = size;
            Bitmap tmp = new Bitmap(Size.Width, Size.Height);
            using (var gfx = Graphics.FromImage(tmp))
            {
                foreach(var el in Elements)
                {
                    if(el!=Selected)
                    {
                        el.Draw(gfx, false);
                    }
                }
                if (Selected != null)
                {
                    Selected.Draw(gfx, true);
                }
            }
            Canvas.Dispose();
            Canvas = tmp;
        }
        public void Draw()
        {
            Bitmap tmp = new Bitmap(Size.Width, Size.Height);
            using (var gfx = Graphics.FromImage(tmp))
            {
                foreach (var element in Elements)
                {
                    element.Draw(gfx, false);
                }
            }
            Canvas = tmp;
            Active = null;
        }
        public Bitmap SelectItem(Point p)
        {
            var element = Elements.FirstOrDefault(e => e.WasHit(p));
            if (element != null)
            {
                DeselectItem();
                Selected = element;
                PreDraw();
                using (var gfx = Graphics.FromImage(Active))
                {
                    Selected.Draw(gfx, true);
                }
            }
            else
            {
                DeselectItem();
                return Canvas;
            }
            return Active;
        }
        public void PreDraw()
        {
            if(Active!=null)
                Active.Dispose();
            Canvas.Dispose();
            Active = new Bitmap(Size.Width, Size.Height);
            using (var gfx = Graphics.FromImage(Active))
            {
                foreach (var element in Elements)
                {
                    if(Selected!=element)
                        element.Draw(gfx, false);
                }
            }
            Canvas = (Bitmap)Active.Clone();
        }

        public void DeselectItem()
        {
            if (Selected != null)
            {
                using (var gfx = Graphics.FromImage(Canvas))
                {
                    Selected.Draw(gfx, false);
                }
                Selected = null;
            }
        }

        public static void Save(string filename, Blueprint bp)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
            {
                file.WriteLine($"{bp.Size.Width} {bp.Size.Height}");
                file.WriteLine($"{bp.Elements.Count}");
                foreach(var element in bp.Elements)
                {
                    element.Save(file);
                }
            }
        }

        public static Blueprint Restore(string filename)
        {
            Blueprint result;
            using(System.IO.StreamReader reader = new System.IO.StreamReader(filename))
            {
                var size = reader.ReadLine().Split();
                int x, y, count;
                Int32.TryParse(size[0], out x);
                Int32.TryParse(size[1], out y);
                result = new Blueprint(new Size(x,y));
                Int32.TryParse(reader.ReadLine(), out count);
                for(int i = 0; i < count; i++)
                {
                    var line = reader.ReadLine().Split();
                    var tmp = RestoreElement(line);
                    result.Elements.Add(tmp);
                }
                result.Draw();
            }
            return result;
        }

        public static IElement RestoreElement(string[] s)
        {
            IElement result = null;
            int id, x, y, count;
            float r;
            Int32.TryParse(s[0], out id);
            if (s[1]!="wall")
            {
                
                Int32.TryParse(s[2], out x);
                Int32.TryParse(s[3], out y);
                float.TryParse(s[4], out r);
                result = new Furniture(s[1], new Point(x, y));
                result.Position = new Point(x, y);
                result.Id = id;
                result.Rotation = r;
            }
            else
            {
                float.TryParse(s[2], out r);
                int.TryParse(s[3], out count);
                Int32.TryParse(s[4], out x);
                Int32.TryParse(s[5], out y);
                result = new Wall(new Point(x, y));
                result.Id = id;
                result.Rotation = r;
                var res = (Wall)result;
                for (int i = 0; i < 2*(count-1); i+=2)
                {
                    Int32.TryParse(s[i+6], out x);
                    Int32.TryParse(s[i+7], out y);
                    res.AddEndpoint(new Point(x, y));
                }
                result = res;
            }
            return result;

        }
    }
    public static class ImageTransform
    {
        public static Image SetOpacity(this Image image, float opacity)
        {
            //https://stackoverflow.com/questions/2201016/how-to-make-a-system-drawing-image-semitransparent?noredirect=1&lq=1
            var colorMatrix = new ColorMatrix();
            colorMatrix.Matrix33 = opacity;
            var imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(
                colorMatrix,
                ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);
            var output = new Bitmap(image.Width, image.Height);
            using (var gfx = Graphics.FromImage(output))
            {
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.DrawImage(
                    image,
                    new Rectangle(0, 0, image.Width, image.Height),
                    0,
                    0,
                    image.Width,
                    image.Height,
                    GraphicsUnit.Pixel,
                    imageAttributes);
            }
            return output;
        }

        //https://stackoverflow.com/questions/2163829/how-do-i-rotate-a-picture-in-winforms
        public static Image Rotate(this Image img, float rotationAngle)
        {
           
            Bitmap bmp = new Bitmap(img.Width + 74, img.Height+74);
            Graphics gfx = Graphics.FromImage(bmp);
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);
            gfx.RotateTransform(rotationAngle);
            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
            gfx.DrawImage(img, new Point(37,37));
            gfx.Dispose();
            return bmp;
        }
    }
}
