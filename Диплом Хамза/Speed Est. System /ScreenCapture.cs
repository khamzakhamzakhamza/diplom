using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Diplom
{
    class ScreenCapture
    {
        public async void Begin(VideoFeedControl vfc)
        {

            IVideoFeedControl isc = vfc;
            isc.bgn = true;

            int i = 0;
            while (isc.bgn)
            {
                vfc.start = DateTime.Now;

                var bm = CaptureScreen(vfc);

                isc.bm = bm;

                var fnd = new FindVehicle();
                fnd.DisplayImage(vfc, bm);

                await Task.Delay(20);

                i++;

            }

        }

        public static Bitmap CaptureScreen(VideoFeedControl vfc)
        {
            if (!vfc.IsDisposed)
            {
                try
                {
                    var cnt = vfc.browser;

                    var screen = Screen.FromPoint(cnt.Location);
                    Point location = cnt.PointToScreen(cnt.Location);

                    var bm = new Bitmap(cnt.ClientSize.Width + 500, cnt.ClientSize.Height + 150);
                    var g = Graphics.FromImage(bm);

                    g.CopyFromScreen(location.X + 130, location.Y + 40, 0, 0, bm.Size);

                    return bm;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

    }
}
