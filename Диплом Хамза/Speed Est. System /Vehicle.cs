using System.Collections.Generic;
using System.Drawing;

namespace Diplom
{
    public class Vehicle
    {
        public Rectangle rct { get; set; }
        public int id { get; set; }
        public IVideoFeedControl isc { get; set; }
        public List<Point> path {get;set;}

        public Vehicle(Rectangle rct, int id, VideoFeedControl vfc)
        {
            this.rct = rct;
            this.id = id;
            this.isc = vfc;
            AddToList(this);
            CreatePath(this);
        }

        private void AddToList(Vehicle vhl)
        { 
                isc.vehicles[isc.vehicles.Count-1].Add(vhl);      
        }

        private void CreatePath(Vehicle vhl)
        {

            this.path = new List<Point>();

            if (isc.vehicles.Count >= 2)
            foreach(var clm in isc.vehicles[isc.vehicles.Count-2])
            {
                var dstX = 20; var dstY = 50;

                if (clm.rct.X-this.rct.X < dstX && clm.rct.Y - this.rct.Y < dstY && clm.rct.X - this.rct.X > -1 * dstX && clm.rct.Y - this.rct.Y > -1 * dstY)
                {
                        if (this.Compare(this.rct, clm.rct, isc.bm)) {
                            foreach (var pnt in clm.path)
                                this.path.Add(pnt);

                            break;
                        }
                }
            }

            this.path.Add(new Point(this.rct.X, this.rct.Y));

        }

        private bool Compare(Rectangle rct1, Rectangle rct2, Bitmap bm)
        {
            int match = 0;

            int yi = 0;
            for (int y = rct1.Y; y < rct1.Y + rct1.Height; y++)
            {
                int xi = 0;

                for (int x = rct1.X; x < rct1.X + rct1.Width; x++)
                {
                    if (xi <= rct2.Width && yi <= rct2.Height && bm.Width > rct2.X + xi && bm.Height > rct2.Y + yi && rct2.Y + yi > 0 && rct2.X + xi >0)
                    {
                        var px1 = bm.GetPixel(x, y); var px2 = bm.GetPixel(rct2.X + xi, rct2.Y + yi);

                        if (px1.ToKnownColor() == px2.ToKnownColor())
                            match++;
                    }

                    xi++;
                }
                yi++;
            }

            if (match > (rct1.Width * rct1.Height) / 1.2)
                return true;

            return false;
        }

    }
}
