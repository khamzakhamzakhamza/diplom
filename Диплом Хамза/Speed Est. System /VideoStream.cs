using System.Collections.Generic;

namespace Diplom
{
    public class VideoStream
    {
        public int id { get; set; }

        public string name { get; set; }

        public float? road_size { get; set; }

        public int? max_speed { get; set; }

        public string url { get; set; }

        private IForm1 iForm1;

        public VideoStream(Form1 context, string name = null, string url = null, float? road_size = null, int? max_speed = null)
        {
            
            this.iForm1 = context;
            this.name = name;
            this.url = url;
            this.road_size = road_size;
            this.max_speed = max_speed;

        }

        public void AddToList()
        {

            if (iForm1.vsList == null)
                iForm1.vsList = new List<VideoStream>();

            id = iForm1.vsList.Count;
            iForm1.vsList.Add(this);

        }
    }


}
