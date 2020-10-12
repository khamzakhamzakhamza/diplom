using System;
using System.Drawing;

namespace Diplom
{
    class SpeedEst
    {
        public static float? Estimate(Rectangle road, float road_sz, Vehicle vhl, float time)
        {
            if (road_sz <= 0) return null;

            var px_sz = road_sz / road.Height;

            if (vhl.path.Count > 1)
            {
                var distPx = vhl.path[vhl.path.Count - 1].Y - vhl.path[vhl.path.Count - 2].Y;

                var distKm = distPx * px_sz;

                var KmMsc = distKm / time;

                var result = Math.Abs(KmMsc * 1000 * 60 * 60);

                return result;
            }
            else 
                return null;
        } 
    }
}
