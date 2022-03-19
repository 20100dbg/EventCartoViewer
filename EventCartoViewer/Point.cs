using System;

namespace EventCartoViewer
{
    internal class EventPoint
    {
        public int Id { get; set; }
        public Coord Coordinates { get; set; }
        public DateTime Gdh { get; set; }
        public string Label { get; set; }
        public int Style { get; set; }

        public static int TriGdh(EventPoint x, EventPoint y)
        {
            if (x.Gdh < y.Gdh) return -1;
            else if (x.Gdh > y.Gdh) return 1;
            else return 0;
        }

    }
}
