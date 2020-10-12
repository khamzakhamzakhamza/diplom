using Diplom.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Diplom
{
    public partial class VideoFeedControl : UserControl, IVideoFeedControl
    {
        #region Fields&Properties
        private IForm1 iForm1;
        public VideoStream vs;
        private bool isMouseDown = false;

        public Rectangle road { get; set; }
        public bool bgn { get; set; }
        public List<List<Vehicle>> vehicles { get; set; }
        public Bitmap bm { get; set; }
        public DateTime start { get; set; }
        #endregion 

        public VideoFeedControl(VideoStream vs, Form1 form)
        {
            InitializeComponent();
            this.iForm1 = form;

            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.Paint += pictureBox1_Paint;
            this.vs = vs;

                this.Dock = DockStyle.Right;

                if (!AddBrowser(vs))
                    this.Dispose();

                this.Width = this.getSize();

        }

        #region PictureBox
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                var tmp = road;
                tmp.Width = e.X - tmp.X; tmp.Height = e.Y - tmp.Y;
                road = tmp;
                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            var tmp = new Rectangle();
            tmp.Location = e.Location;
            road = tmp;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (road.Width > 0 && road.Height > 0)
            {
                e.Graphics.DrawRectangle(new Pen(Color.Blue), road);
                e.Graphics.DrawString("Дорога", new Font(toolStripLabel1.Font, FontStyle.Regular), new SolidBrush(Color.Blue), road.X, road.Y - 20);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                isMouseDown = false;

                var tmp = road;
                tmp.Width = e.X - tmp.X; tmp.Height = e.Y - tmp.Y;
                road = tmp;
                pictureBox1.Refresh();
            }
        }
        #endregion

        private int getSize()
        {
            var width = iForm1.panel2Width;

            if (iForm1.vsList != null && iForm1.vsList.Count != 1)
                return width / iForm1.vsList.Count - 1;
            else return width - 1;
        }

        #region BtnClick
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Dispose();
            DeleteItem(vs);
            Log.AddToLog("Видалено трансляцію під назвою " + vs.name + ";");
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            new NewVideoStream(vs);
            this.browser.Load(vs.url);
            this.toolStripLabel1.Text = vs.name;
            Log.AddToLog("Змінено параметри трансляциї під назвою " + vs.name + ";");
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.browser.Load(vs.url);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (!bgn)
            {
                toolStripButton4.Text = "Зупинити стеження";
                Log.AddToLog("Розпочато стеження за трансляцією під назвою " + vs.name + ".\r\nДля продовження виделіть на трансльованому знизу зображенні дорогу для візначення її розмиру у пікселях;");
                var stop = Resources.Stop_red_37107;
                toolStripButton4.Image = stop;
                var sc = new ScreenCapture();
                sc.Begin(this);
            }
            else
            {
                toolStripButton4.Text = "Розпочати стеження";
                Log.AddToLog("Закінченно стеження за трансляцією під назвою " + vs.name + ";");
                toolStripButton4.Image = Resources.ic_play_arrow_128_28560;
                bgn = false;
            }
        }
        #endregion

        #region misc 
        private void DeleteItem(VideoStream vs)
        {
            for(int i =0; i<iForm1.vsList.Count; i++)
            {
                if (iForm1.vsList[i].id == vs.id)
                {
                    iForm1.vsList.RemoveAt(i);
                    break;
                }

            }
        }
        #endregion

    }
}
