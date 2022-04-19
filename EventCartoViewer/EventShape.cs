using MapWinGIS;
using System;
using System.Collections.Generic;
using Image = MapWinGIS.Image;

namespace EventCartoViewer
{
    public class EventShape : ICloneable
    {
        //public int Id { get; set; }
        public DateTime GdhDebut { get; set; }
        public DateTime GdhFin { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string NomStyle { get; set; }
        public TypeShapefile TypeShapefile { get; set; }
        public List<EventCoord> Coordinates { get; set; }

        public static int TriGdh(EventShape x, EventShape y)
        {
            if (x.GdhDebut < y.GdhDebut) return -1;
            else if (x.GdhDebut > y.GdhDebut) return 1;
            else return 0;
        }

        public object Clone()
        {
            return new EventShape
            {
                GdhDebut = this.GdhDebut,
                GdhFin = this.GdhFin,
                Coordinates = this.Coordinates,
                Label = this.Label,
                Description = this.Description,
                NomStyle = this.NomStyle,
                TypeShapefile = this.TypeShapefile
            };
        }
    }

    public static class Constantes
    {
        public static int DEFAUT_POINT = 0;
        public static int DEFAUT_LIGNE = 1;
        public static int DEFAUT_SURFACE = 2;

    }


    public enum TypeShapefile { Point, Ligne, Surface, Buffer };

    public class Couche
    {
        public int IdLayer { get; set; }
        public Shapefile Shapefile { get; set; }
        public TypeShapefile TypeShapefile { get; set; }

        public StyleCouche Style { get; set; }
    }

    public class StyleCouche
    {
        public string Nom { get; set; }
        public TypeShapefile TypeShapefile { get; set; }

        public float PointTaille { get; set; }
        public tkMapColor PointCouleur { get; set; }
        public Image Image { get; set; }
        public string ImagePath { get; set; }

        public float LigneTaille { get; set; }
        public tkMapColor LigneCouleur { get; set; }
        public tkDashStyle LigneStyle { get; set; }

        public tkMapColor SurfaceCouleur { get; set; }
        public float SurfaceTransparence { get; set; }

        public override string ToString()
        {
            return Nom + " (" + TypeShapefile + ")";
        }
    }
}
