using System;
using System.Collections.Generic;
using System.Drawing;

namespace EventCartoViewer
{
    public class EventShape
    {
        public int Id { get; set; }
        public DateTime GdhDebut { get; set; }
        public DateTime GdhFin { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public Color Style { get; set; }
        public List<EventCoord> Coordinates { get; set; }
        public int TypeForme { get; set; }

        public static int TriGdh(EventShape x, EventShape y)
        {
            if (x.GdhDebut < y.GdhDebut) return -1;
            else if (x.GdhDebut > y.GdhDebut) return 1;
            else return 0;
        }
    }
}
