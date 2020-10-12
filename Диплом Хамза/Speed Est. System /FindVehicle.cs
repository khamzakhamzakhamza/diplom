using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Diplom
{
    class FindVehicle
    {
        static readonly CascadeClassifier clsfr = new CascadeClassifier("cars.xml");

        public void DisplayImage(VideoFeedControl vfc, Bitmap bm)
        {
            DetectVehicle(bm, vfc);
            vfc.pictureBox1.Image = bm;
        }

        public Bitmap DetectVehicle(Bitmap img, VideoFeedControl vfc)
        {
            if (!vfc.IsDisposed)
            {
                Graphics g = Graphics.FromImage(img);

                if (vfc.road.Width > 0 && vfc.road.Height > 0)
                {
                    VhlList(vfc);

                    Image<Bgr, byte> grayImage = new Image<Bgr, byte>(img);
                    Rectangle[] rcts = clsfr.DetectMultiScale(grayImage, 1.05, 3);
                    int i = 0;

                    foreach (var rct in rcts)
                    {
                        var car = new Vehicle(rct, i, vfc);

                        var speed = GetSpeed(vfc, car);

                        Pen pen;

                        if (CompareSpeed(speed, vfc))
                            pen = new Pen(Color.Blue, 2);
                        else
                        {
                            pen = new Pen(Color.Red, 2);
                        }

                        g.DrawRectangle(pen, rct);

                        SpeedDisplay(g, pen.Color, speed, car);

                        if (!CompareSpeed(speed, vfc))
                        {
                            var path = SaveBm(img, vfc.vs.name + "_перевищення_швидкості");

                            Log.AddToLog("На трансляції під назвою " + vfc.vs.name +
                                "було виявлено правопорушення;\r\nПравопорушення збереженно за адресою: " + path + ";");
                        }

                        i++;

                    }
                    TrackVehicle(g, vfc, Color.Blue);
                }
                else
                {
                    g.DrawString("Виділіть на записі дорогу ", new Font(FontFamily.GenericSansSerif, 17, FontStyle.Regular), new SolidBrush(Color.Blue), 3, 3);

                }
                return img;
            }return null;
        }

        private void VhlList(VideoFeedControl vfc)
        {
            if (vfc.vehicles == null)
            {
                vfc.vehicles = new List<List<Vehicle>>();
                vfc.vehicles.Add(new List<Vehicle>());
            }
            else
            {
                vfc.vehicles.Add(new List<Vehicle>());
            }
        }

        #region Speed
        private float? GetSpeed(VideoFeedControl vfc, Vehicle vehicle)
        {
            if (vfc.vs.road_size != null) {
                var time = DateTime.Now - vfc.start;
                return SpeedEst.Estimate(vfc.road, (float)vfc.vs.road_size, vehicle, (float)time.TotalMilliseconds);
            }
            else return null;
        }

        private void SpeedDisplay(Graphics g, Color color, float? speed, Vehicle vehicle)
        {
            if (speed != null)
            {
               if (vehicle.rct.Y - 30 >= 0)
               g.DrawString(string.Format("{0:0.0}",speed), new Font(FontFamily.GenericSansSerif, 17, FontStyle.Regular), new SolidBrush(color), vehicle.rct.X, vehicle.rct.Y - 30);
            }
        }

        private bool CompareSpeed (float? speed, VideoFeedControl vfc)
        {
            if (vfc.vs.max_speed != null && speed != null)
            {
                if (speed > vfc.vs.max_speed)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region Track 
        private void TrackVehicle(Graphics g, VideoFeedControl vfc, Color color)
        {
                if (vfc.vehicles != null)
                {
                    var vhls = vfc.vehicles[vfc.vehicles.Count - 1];

                    foreach (var vhl in vhls)
                    {
                        var pen = new Pen(color, 2);

                        if (vhl.path.Count >= 1)
                            for (int i = 1; i < vhl.path.Count; i++)
                            {
                                g.DrawLine(pen, vhl.path[i - 1], vhl.path[i]);
                            }

                    }
                }
        }
        #endregion

        public static string SaveBm(Bitmap bitmap, string dir_name)
        {
            try
            {
                string startupPath = Directory.GetCurrentDirectory();
                var dirPath = startupPath + "\\" + dir_name;
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
                bool saved = false;
                int i = 0;
                string path;
                do
                {
                    path = dirPath + "\\" + i + ".png";
                    if (File.Exists(path))
                        i++;
                    else
                    {
                        bitmap.Save(path,ImageFormat.Png);
                        return path;
                    }
                } while (!saved);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + Properties.Resources.ErrorMsgBody, Properties.Resources.ErrorMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return null;
            }
        }
    }
}
