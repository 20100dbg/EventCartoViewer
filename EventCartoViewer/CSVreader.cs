using System;
using System.Collections.Generic;
using System.IO;

namespace EventCartoViewer
{
    public static class CSVreader
    {
        public static List<Colonne> fields { get; set; }

        public static void InitCSVreader()
        {
            fields = new List<Colonne>();
            fields.Add(new Colonne { Id = -1, Nom = "X" });
            fields.Add(new Colonne { Id = -1, Nom = "Y" });
            fields.Add(new Colonne { Id = -1, Nom = "GDHDEBUT" });
            fields.Add(new Colonne { Id = -1, Nom = "GDHFIN" });
            fields.Add(new Colonne { Id = -1, Nom = "LABEL" });
            fields.Add(new Colonne { Id = -1, Nom = "DESCRIPTION" });
            fields.Add(new Colonne { Id = -1, Nom = "STYLE" });
            fields.Add(new Colonne { Id = -1, Nom = "AZM" });
        }

        public static bool ReadHeaders(string[] cols)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i] == "X" || cols[i] == "X CAPTEUR" || cols[i] == "LONG" || cols[i] == "LONGITUDE") fields[0].Id = i;
                else if (cols[i] == "Y" || cols[i] == "Y CAPTEUR" || cols[i] == "LAT" || cols[i] == "LATITUDE") fields[1].Id = i;
                else if (cols[i] == "GDHDEBUT" || cols[i] == "GDH") fields[2].Id = i;
                else if (cols[i] == "GDHFIN") fields[3].Id = i;
                else if (cols[i] == "LABEL") fields[4].Id = i;
                else if (cols[i] == "DESCRIPTION") fields[5].Id = i;
                else if (cols[i] == "STYLE") fields[6].Id = i;
                else if (cols[i] == "AZM" || cols[i] == "AZT" || cols[i] == "AZIMUT") fields[7].Id = i;
                else fields.Add(new Colonne { Id = i, Nom = cols[i] });
            }

            if (fields[0].Id == -1 || fields[1].Id == -1 || fields[2].Id == -1)
            {
                return false;
            }

            if (fields[3].Id == -1) fields[3].Id = fields[2].Id;
            if (fields[5].Id == -1) fields[5].Id = fields[4].Id;

            return true;
        }

        public static List<EventShape> ReadCSV(StreamReader sr)
        {
            List<EventShape> events = new List<EventShape>();

            string line;
            sr.ReadLine(); //entetes

            while ((line = sr.ReadLine()) != null)
            {
                string[] tab = line.Split(new char[] { ';' });

                EventShape es = new EventShape
                {
                    Coordinates = new List<EventCoord>() {
                        new EventCoord
                        {
                            X = double.Parse(Util.FixDecSeparator(tab[fields[0].Id])),
                            Y = double.Parse(Util.FixDecSeparator(tab[fields[1].Id]))
                        } },
                    GdhDebut = DateTime.Parse(tab[fields[2].Id]),
                    GdhFin = DateTime.Parse(tab[fields[3].Id]),
                    KeyValues = new Dictionary<int, string>()
                };

                if (fields[4].Id > -1) es.Label = tab[fields[4].Id];
                if (fields[5].Id > -1) es.Description = tab[fields[5].Id];
                if (fields[6].Id > -1) es.NomStyle = tab[fields[6].Id];
                if (fields[7].Id > -1 && tab[fields[7].Id] != "")
                {
                    Coord capteur = Coord.FromEventCoord(es.Coordinates[0]);

                    float azm = float.Parse(Util.FixDecSeparator(tab[fields[7].Id]));
                    Coord cible = GetCible(capteur, azm, Settings.distanceReleve);
                    es.Coordinates.Add(EventCoord.FromCoord(cible));
                    es.TypeShapefile = TypeShapefile.Ligne;
                }

                for (int i = 8; i < fields.Count; i++)
                {
                    if (fields[i].Id > -1)
                        es.KeyValues.Add(fields[i].Id, tab[fields[i].Id]);
                }

                events.Add(es);
            }

            return events;
        }

        public static Coord GetCible(Coord capteur, float azm, float distance)
        {
            double cibleY = Math.Asin(Math.Sin(capteur.Y * Math.PI / 180) * Math.Cos(distance / 6371) + Math.Cos(capteur.Y * Math.PI / 180) * Math.Sin(distance / 6371) * Math.Cos(azm * Math.PI / 180)) * 180 / Math.PI;
            double cibleX = (capteur.X * Math.PI / 180 + Math.Atan2(Math.Sin(azm * Math.PI / 180) * Math.Sin(distance / 6371) * Math.Cos(cibleY * Math.PI / 180), Math.Cos(distance / 6371) - Math.Sin(cibleY * Math.PI / 180) * Math.Sin(cibleY * Math.PI / 180))) * 180 / Math.PI;

            return new Coord { X = cibleX, Y = cibleY };
        }
    }

    public class Colonne
    {
        public int Id { get; set; }
        public string Nom { get; set; }
    }
}
