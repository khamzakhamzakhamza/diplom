using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Diplom
{
    public partial class Form1 : Form, IForm1
    {
        public List<VideoStream> vsList { get; set; }
        public int panel2Width { get; set; }
        public Form1()
        {
            InitializeComponent();

            Log.AddToLog("Вітаємо у демо-версії програмного забезпечення, створеного для моніторингу автомобільного руху на перехрестях, як частини \"Iнформаційно-комунікаційної системи слидкування за рухом транспортних засобив під час перетину перехресть\".\r\n\r\nДля початку роботи створіть трансляцію.", this);

            this.SizeChanged += form1SizeChamged;
        }

        private void form1SizeChamged(object sender, EventArgs e)
        {
            BuildPanel2();
        }

        private void BuildPanel2()
        {
            this.splitContainer1.Panel2.Controls.Clear();

            if (vsList != null)
            {
                foreach (var stream in vsList)
                {
                    var feed = new VideoFeedControl(stream, this);

                    if (!feed.IsDisposed)  this.splitContainer1.Panel2.Controls.Add(feed); 
                }
            }

        }

        #region BtnClick

        private void hjbjbToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var vs = new VideoStream(this);

            var nvs = new NewVideoStream(vs);

            this.panel2Width = splitContainer1.Panel2.Width;

            if (nvs.DialogResult == DialogResult.Yes)
            {
                var feed = new VideoFeedControl(vs, this);

                if (!feed.IsDisposed) this.splitContainer1.Panel2.Controls.Add(feed);

                BuildPanel2();

                Log.AddToLog("Створено трансляцію під назвою " + vs.name + ";");
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void прмпрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var vs = new VideoStream(this);

            this.panel2Width = splitContainer1.Panel2.Width;

            vs.url = "https://youtu.be/wqctLW0Hb_0";
            vs.name = "Завантажена дорога";
            vs.road_size = (float?)0.2;
            vs.max_speed = 60;
            vs.AddToList();

            var feed = new VideoFeedControl(vs, this);

            if (!feed.IsDisposed) this.splitContainer1.Panel2.Controls.Add(feed);

            BuildPanel2();

            Log.AddToLog("Створено трансляцію під назвою " + vs.name + ";");
        }

        #endregion
    }
}
