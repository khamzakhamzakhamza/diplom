using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    public interface IVideoFeedControl
    {
        Rectangle road { get; set; }
        bool bgn { get; set; }
        List<List<Vehicle>> vehicles { get; set; }
        Bitmap bm { get; set; }
    }
}
