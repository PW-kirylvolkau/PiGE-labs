using Room_planner.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Room_planner
{
    public enum Modes
    {
        Casual, Wall, Dragging
    }
    public partial class Form1 : System.Windows.Forms.Form
    {
        Blueprint bp;
        Button selectedItem;
        Modes mode;
        Point LastMouseLocation;

        private ResourceManager resourcemanager = null;
        public Form1()
        {

            InitializeComponent();
            resourcemanager = new ResourceManager("Room_planner.Form1",
                Assembly.GetExecutingAssembly());
            UpdateUIControls();
            bp = new Blueprint(canvas.Size);
            canvas.Image = bp.Canvas;
            canvas.MouseWheel += new MouseEventHandler(mwPanel1_mouseWheel);
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(mw_keyDown);
            this.DoubleBuffered = true;
            mode = Modes.Casual;

        }

        private void mw_keyDown(object sender, KeyEventArgs e)
        {
            this.Focus();
            if (e.KeyCode == Keys.Delete)
            {

                if (bp.Selected != null)
                {
                    furnitureList.Items.Remove(bp.Selected);
                    bp.Elements.Remove(bp.Selected);
                    bp.Selected = null;
                    bp.Draw();
                    canvas.Image = bp.Canvas;
                }
            }
        }

        private void mwPanel1_mouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
            if (bp.Selected != null)
            {
                if (bp.Canvas != canvas.Image)
                {
                    canvas.Image.Dispose();
                }
                bp.PreDraw();
                canvas.Image = (Image)bp.Canvas.Clone();
                float angle = e.Delta * SystemInformation.MouseWheelScrollLines / 40;
                bp.Selected.Rotation += angle;
                using (var gfx = Graphics.FromImage(canvas.Image))
                {
                    bp.Selected.Draw(gfx, true);
                }

            }
        }

        private void renewCanvas_Click(object sender, EventArgs e)
        {
            canvas.Image.Dispose();
            bp = null;
            canvas.Size = mainWindow.Panel1.Size;
            bp = new Blueprint(mainWindow.Panel1.Size);
            canvas.Image = bp.Canvas;
            furnitureList.Items.Clear();
            deselectItem();
            mode = Modes.Casual;
        }
        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (mode != Modes.Wall)
                {
                    if (selectedItem != null)
                    {
                        if (selectedItem == bedButton)
                        {
                            bp.AddElement("bed", e.Location);
                        }
                        if (selectedItem == sofaButton)
                        {
                            bp.AddElement("sofa", e.Location);
                        }
                        if (selectedItem == coffeeButton)
                        {
                            bp.AddElement("coffee", e.Location);
                        }
                        if (selectedItem == tableButton)
                        {
                            bp.AddElement("table", e.Location);
                        }
                        if (selectedItem == wallButton)
                        {
                            bp.AddElement("wall", e.Location);
                            furnitureList.Items.Add(bp.Elements.Last());
                            mode = Modes.Wall;
                            return;
                        }
                        furnitureList.Items.Add(bp.Elements.Last());
                        canvas.Invalidate();
                        deselectItem();
                    }
                    else
                    {
                        canvas.Image = bp.SelectItem(e.Location);
                        furnitureList.SelectedItem = bp.Selected;
                        LastMouseLocation = e.Location;
                        mode = Modes.Dragging;
                    }
                }
                else
                {
                    bp.Canvas = new Bitmap(canvas.Image); //because line is actually drawn in MouseMove.
                    try // there is a situation, when Bitmap is renewed, then this part of code throws exception.
                    {
                        var line = (Wall)bp.Elements.Last();
                        line.AddEndpoint(e.Location);
                    }
                    catch { }
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                if (mode == Modes.Wall)
                {
                    mode = Modes.Casual;
                    canvas.Image = bp.Canvas;
                    deselectItem();
                }
            }

        }

        //selecting button
        private void selectItem_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            Color defColor = Color.White;
            Color selColor = Color.FromArgb(255 / 2, 128, 128, 128);
            bp.DeselectItem();
            canvas.Image = bp.Canvas;
            furnitureList.SelectedItem = null;
            if (selectedItem == null)
            {
                button.BackColor = selColor;
                selectedItem = button;
            }
            else if (selectedItem == button)
            {
                button.BackColor = defColor;
                if (selectedItem == wallButton)
                {
                    canvas.Image = bp.Canvas;
                    mode = Modes.Casual;
                }
                selectedItem = null;
            }
            else
            {
                selectedItem.BackColor = defColor;
                button.BackColor = selColor;
                selectedItem = button;
            }
        }
        //deselecting buttton
        private void deselectItem()
        {
            if (selectedItem != null)
            {
                selectedItem.BackColor = Color.White;
                selectedItem = null;
            }

        }
        private void mainWindow_Panel1_SizeChanged(object sender, EventArgs e)
        {
            if (bp != null)
            {
                Size tmp = new Size();
                tmp.Width = bp.Canvas.Width > mainWindow.Panel1.Width ? bp.Canvas.Width : mainWindow.Panel1.Width;
                tmp.Height = bp.Canvas.Height > mainWindow.Panel1.Height ? bp.Canvas.Height : mainWindow.Panel1.Height;
                canvas.Size = tmp;
                bp.UpdateSize(tmp);
                canvas.Image = bp.Canvas;

            }
        }
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (mode == Modes.Wall && selectedItem == wallButton)
            {
                if (canvas.Image != bp.Canvas)
                {
                    canvas.Image.Dispose();
                }
                canvas.Image = (Image)bp.Canvas.Clone();
                var wall = (Wall)bp.Elements.LastOrDefault();
                using (var gfx = Graphics.FromImage(canvas.Image))
                {
                    if (wall != null)
                    {
                        wall.DrawTemp(gfx, e.Location);
                    }
                }
            }
            if (mode == Modes.Dragging)
            {
                if (bp.Selected != null)
                {
                    if (bp.Canvas != canvas.Image)
                    {
                        canvas.Image.Dispose();
                    }
                    canvas.Image = (Image)bp.Canvas.Clone();
                    bp.Selected.UpdatePosition(-LastMouseLocation.X + e.X, -LastMouseLocation.Y + e.Y);
                    using (var gfx = Graphics.FromImage(canvas.Image))
                    {
                        bp.Selected.Draw(gfx, true);
                    }
                    LastMouseLocation = e.Location;

                    furnitureList.Items[furnitureList.SelectedIndex] = furnitureList.Items[furnitureList.SelectedIndex];

                }
            }
        }

        private void furnitureList_SelectedIndexChanged(object sender, EventArgs e)
        { 
            if (furnitureList.SelectedItem != bp.Selected && furnitureList.SelectedItem != null)
            {

                Point p;
                if (bp.Selected != null)
                    bp.DeselectItem();
                var el = (IElement)furnitureList.SelectedItem;
                if (el.Name == "wall")
                {
                    var ele = el as Wall;
                    p = ele.Endpoints[1];
                }
                else
                {
                    p = el.Position;
                }
                canvas.Image = bp.SelectItem(p);
            }
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (mode == Modes.Dragging)
            {
                mode = Modes.Casual;
            }
        }

        private void saveBlueprintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var saveFile = new SaveFileDialog();
                saveFile.Filter = "Blueprints files |*.bp";
                saveFile.ShowDialog();
                if (saveFile.FileName != "")
                {
                    Blueprint.Save(saveFile.FileName, bp);
                }
                string message =resourcemanager.GetString("blueprintSaved");
                string caption = resourcemanager.GetString("operationSuccess");
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;
                result = MessageBox.Show(message, caption, buttons);
            }
            catch
            {
                string message = resourcemanager.GetString("notSaved");
                string caption = resourcemanager.GetString("operationFailure");
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;
                result = MessageBox.Show(message, caption, buttons);
            }
        }

        private void openBlueprintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Blueprints files |*.bp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var bp2 = Blueprint.Restore(openFileDialog.FileName);
                    if (bp2 != null)
                    {
                        mode = Modes.Casual;
                        bp.Canvas.Dispose();
                        bp = bp2;
                        canvas.Image = bp.Canvas;
                        canvas.Invalidate();
                        furnitureList.Items.Clear();
                        foreach (var el in bp.Elements)
                        {
                            furnitureList.Items.Add(el);
                        }
                        furnitureList.SelectedItem = bp.Selected;
                    }
                }
                string message = resourcemanager.GetString("blueprintOpened");
                string caption = resourcemanager.GetString("operationSuccess");
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;
                result = MessageBox.Show(message, caption, buttons);
            }
            catch
            {
                string message = resourcemanager.GetString("yourBPnot");
                string caption = resourcemanager.GetString("operationFailure");
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;
                result = MessageBox.Show(message, caption, buttons);
            }
        }

        private void ChangeLanguage(string lang)
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
            UpdateUIControls();
        }

        private void UpdateUIControls()
        {
            try
            {
                if (resourcemanager != null)
                {
                    this.Text = resourcemanager.GetString("$this.Text");
                    buttonsBox.Text = resourcemanager.GetString("buttonsBox.Text");
                    displayBox.Text = resourcemanager.GetString("displayBox.Text");
                    fileToolStripMenuItem.Text = resourcemanager.GetString("fileToolStripMenuItem.Text");
                    openBlueprintToolStripMenuItem.Text = resourcemanager.GetString("openBlueprintToolStripMenuItem.Text");
                    renewCanvas.Text = resourcemanager.GetString("renewCanvas.Text");
                    saveBlueprintToolStripMenuItem.Text = resourcemanager.GetString("saveBlueprintToolStripMenuItem.Text");
                    languageToolStripMenuItem.Text = resourcemanager.GetString("languageToolStripMenuItem");
                    russianToolStripMenuItem.Text = resourcemanager.GetString("russianToolStripMenuItem");
                    englishToolStripMenuItem.Text = resourcemanager.GetString("englishToolStripMenuItem");
                    for(int i = 0; i < furnitureList.Items.Count; i++)
                    {
                        furnitureList.Items[i] = furnitureList.Items[i];
                    }
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void russianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeLanguage("ru-RU");
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeLanguage("en-US");
        }
    }
}
