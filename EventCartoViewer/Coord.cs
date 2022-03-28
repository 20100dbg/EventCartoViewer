using System;

namespace EventCartoViewer
{
    public class Coord
    {
        public double Y { get; set; }
        public double X { get; set; }
        public string Zone { get; set; }
        public int Fuseau { get; set; }
    }

    public class EventCoord
    {
        public double Y { get; set; }
        public double X { get; set; }

        public static int TriCoord(EventCoord x, EventCoord y)
        {
            if (x.Y > y.Y) return -1;
            else
            {
                if (x.X > y.X) return -1;
                else if (x.X < y.X) return 1;
                else return 0;
            }
        }
    }
}
