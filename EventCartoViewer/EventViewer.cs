using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SliderControl;

namespace EventCartoViewer
{
    public partial class EventViewer : Form
    {
        Slider s;
        List<EventShape> events = new List<EventShape>();


        public EventViewer()
        {
            InitializeComponent();
            this.Text = "EventCartoViewer " + Settings.version;
            Settings.LoadDefaultValues();
            Settings.ReadConfigFile();

            s = new Slider(this.Controls, new Point(100, 460), new Size(400, 22), 0, 100);
            s.TickRate = 40;
            s.SpanMoving += S_SpanMoving;
            s.SpanMoved += S_SpanMoving;
            s.SpanResizing += S_SpanResizing;
            s.SpanResized += S_SpanResizing;

            InitStyles();
            Init();
            CSVreader.InitCSVreader();
        }

        public void InitStyles()
        {
            Carto.styles.Add(new StyleCouche { Nom = "PointDefaut", PointTaille = 8.5f, PointCouleur = MapWinGIS.tkMapColor.Black, Image = null, TypeShapefile = TypeShapefile.Point });
            Carto.styles.Add(new StyleCouche { Nom = "LigneDefaut", LigneTaille = 1.5f, LigneCouleur = MapWinGIS.tkMapColor.Black, LigneStyle = MapWinGIS.tkDashStyle.dsSolid, TypeShapefile = TypeShapefile.Ligne });
            Carto.styles.Add(new StyleCouche { Nom = "SurfaceDefaut", SurfaceCouleur = MapWinGIS.tkMapColor.Blue, SurfaceTransparence = 100f, TypeShapefile = TypeShapefile.Surface });
            //Carto.styles.Add(new StyleCouche { Nom = "Buffer", TypeShapefile = TypeShapefile.Buffer });

            Carto.styles.Add(new StyleCouche { Nom = "AMI", PointTaille = 8.5f, PointCouleur = MapWinGIS.tkMapColor.Blue, Image = null, TypeShapefile = TypeShapefile.Point });
            Carto.styles.Add(new StyleCouche { Nom = "ENI", PointTaille = 8.5f, PointCouleur = MapWinGIS.tkMapColor.Red, Image = null, TypeShapefile = TypeShapefile.Point });

            Carto.styles.Add(new StyleCouche { Nom = "FLOT", LigneTaille = 1.5f, LigneCouleur = MapWinGIS.tkMapColor.Red, LigneStyle = MapWinGIS.tkDashStyle.dsDash, TypeShapefile = TypeShapefile.Ligne });
            Carto.styles.Add(new StyleCouche { Nom = "Surface 1", SurfaceCouleur = MapWinGIS.tkMapColor.Blue, SurfaceTransparence = 100f, TypeShapefile = TypeShapefile.Surface });
            Carto.styles.Add(new StyleCouche { Nom = "Surface 2", SurfaceCouleur = MapWinGIS.tkMapColor.RosyBrown, SurfaceTransparence = 100f, TypeShapefile = TypeShapefile.Surface });
        }


        public void Init()
        {
            //Affiche les couches dispo
            ListeCartoDispo();

            //charge la carto
            Carto.Init(this, axMap1);

            Carto.InitStyleLayers(Carto.styles);
        }

        #region dgv
        private void InitDgv(List<Colonne> fields)
        {
            for (int i = 0; i < fields.Count; i++)
            {
                dgv.Columns.Add(fields[i].Nom, fields[i].Nom);
                dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void FillDgv(List<EventShape> events)
        {
            dgv.Rows.Clear();
            List<string> tab = new List<string>();
            
            for (int i = 0; i < events.Count; i++)
            {
                tab.Clear();

                tab.Add(events[i].Coordinates[0].X.ToString());
                tab.Add(events[i].Coordinates[0].Y.ToString());
                tab.Add(events[i].GdhDebut.ToString());
                tab.Add(events[i].GdhFin.ToString());
                tab.Add(events[i].Label);
                tab.Add(events[i].Description);
                tab.Add(events[i].NomStyle);

                foreach (KeyValuePair<int,string> kv in events[i].KeyValues)
                {
                    tab.Add(kv.Value);
                }

                dgv.Rows.Add(tab.ToArray());
            }
        }

        #endregion


        #region carto
        private void ListeCartoDispo()
        {
            cblb_couches.Items.Clear();

            foreach (string file in Directory.EnumerateFiles("carte/", "*.tif"))
            {
                string nom = Path.GetFileName(file);
                bool flag = Settings.cartes.Contains(nom);
                cblb_couches.Items.Add(nom, flag);
            }
        }

        private void SaveCarto()
        {
            Settings.cartes.Clear();

            for (int i = 0; i < cblb_couches.Items.Count; i++)
            {
                if (cblb_couches.GetItemChecked(i))
                {
                    Settings.cartes.Add(cblb_couches.Items[i].ToString());
                }
            }

            Settings.WriteConfigFile();
        }

        private void b_applyCarto_Click(object sender, EventArgs e)
        {
            SaveCarto();

            Carto.Init(this, axMap1);

            Carto.InitStyleLayers(Carto.styles);

            GetPoints();
        }

        public void SetTbMGRS(string mgrs)
        {
            tb_coord.Text = mgrs;
        }

        #endregion

        private Color StyleToColor(int idStyle)
        {
            return Color.FromKnownColor((KnownColor)idStyle);
        }

        private void S_SpanMoving(object sender, SpanMovedEventArgs e)
        {
            GetPoints();
        }

        private void S_SpanResizing(object sender, SpanResizedEventArgs e)
        {
            GetPoints();

            l_unite.Text = s.CurrentSpan + " " + GetTimeUnitStr();
        }


        private int GetLayerWithStyle(string nomStyle)
        {
            StyleCouche sc = Carto.styles.Find((x) => { return x.Nom == nomStyle; });

            return Carto.couches.FindIndex((x) =>
            {
                return x.Style == sc;
            });
        }

        private void GetPoints()
        {
            Carto.ClearCarto(Carto.couches);
            if (events.Count == 0) return;

            DateTime dtStart = gdhMin.AddSeconds(GetUnitsToSeconds(s.CurrentValue));
            DateTime dtFin = dtStart.AddSeconds(GetUnitsToSeconds(s.CurrentSpan));
            l_currentValue.Text = dtStart.ToString();
            l_currentValue2.Text = dtFin.ToString();

            List<EventShape> aDessiner = new List<EventShape>();

            for (int i = 0; i < events.Count; i++)
            {
                if ((events[i].GdhDebut >= dtStart && events[i].GdhDebut <= dtFin) ||
                    (events[i].GdhFin >= dtStart && events[i].GdhFin <= dtFin) ||
                    (events[i].GdhDebut <= dtStart && events[i].GdhFin >= dtFin))
                {
                    int x = GetLayerWithStyle(events[i].NomStyle);
                    if (x == -1)
                    {
                        if (events[i].TypeShapefile == TypeShapefile.Point) x = Constantes.DEFAUT_POINT;
                        else if (events[i].TypeShapefile == TypeShapefile.Ligne) x = Constantes.DEFAUT_LIGNE;
                        else if (events[i].TypeShapefile == TypeShapefile.Surface) x = Constantes.DEFAUT_SURFACE;
                    }
                    int layer = Carto.couches[x].IdLayer;

                    //on dessine toujours les points
                    for (int j = 0; j < events[i].Coordinates.Count; j++)
                    {
                        // 0 = style par défaut
                        int tmpLayer = ((events[i].TypeShapefile == TypeShapefile.Point) ? layer : Carto.couches[Constantes.DEFAUT_POINT].IdLayer);
                        Carto.DrawPoint(events[i].Coordinates[j], events[i].Label, tmpLayer);
                    }

                    if (events[i].TypeShapefile == TypeShapefile.Ligne)
                    {
                        Carto.DrawLine(events[i].Coordinates, layer);
                    }
                    else if (events[i].TypeShapefile == TypeShapefile.Surface)
                    {
                        Carto.DrawArea(events[i].Coordinates, layer);
                    }

                    aDessiner.Add(events[i]);
                }
            }

            //if (Settings.afficherBuffer) Carto.SetBuffer();

            l_nbEvent.Text = aDessiner.Count + " / " + events.Count;
            if (Settings.afficherLabel) Carto.GenerateLabels();

            FillDgv(aDessiner);
            axMap1.Redraw();
        }

        #region import
        private List<EventShape> ReadCsv(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            string[] entetes = sr.ReadLine().ToUpper().Split(new char[] { ';' });

            CSVreader.InitCSVreader();

            List<EventShape> events;
            bool isValid = CSVreader.ReadHeaders(entetes);

            if (isValid)
            {
                InitDgv(CSVreader.fields);
                events = CSVreader.ReadCSV(sr);
            }
            else
            {
                events = new List<EventShape>();
            }

            /*
            if (entetes.Length == 5) events = ReadCSVmin(sr);
            else if (entetes.Length == 7) events = ReadCSVcomplet(sr);
            else events = new List<EventShape>();
            */
            
            sr.Close();
            return events;
        }

        private List<EventShape> ReadWKT(string filename)
        {
            List<EventShape> eventsFile = new List<EventShape>();
            StreamReader sr = new StreamReader(filename);

            string line;
            sr.ReadLine(); //entetes

            while ((line = sr.ReadLine()) != null)
            {
                string[] tab = line.Split(new char[] { ';' });
                string coord = tab[0];

                EventShape es = new EventShape
                {
                    Label = tab[3],
                    GdhDebut = DateTime.Parse(tab[1]),
                    GdhFin = DateTime.Parse(tab[2]),
                    Description = tab[4],
                    Coordinates = new List<EventCoord>(),
                    NomStyle = tab[5],
                    KeyValues = new Dictionary<int, string>()
                };

                if (coord.StartsWith("\"POLYGON"))
                {
                    es.TypeShapefile = TypeShapefile.Surface;
                    coord = coord.Substring(10, coord.Length - 13);
                    string[] tabCoord = coord.Split(new string[] { "," }, StringSplitOptions.None);
                    List<EventCoord> ecTmp = new List<EventCoord>();

                    for (int i = 0; i < tabCoord.Length; i++)
                    {
                        string[] tabtmp = tabCoord[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        ecTmp.Add(new EventCoord
                        {
                            X = double.Parse(Util.FixDecSeparator(tabtmp[0])),
                            Y = double.Parse(Util.FixDecSeparator(tabtmp[1]))
                        });
                    }

                    if (Settings.triCoordSurface) es.Coordinates.AddRange(EventCoord.TriCoord(ecTmp));
                    else es.Coordinates.AddRange(ecTmp);
                }
                else if (coord.StartsWith("\"LINESTRING"))
                {
                    es.TypeShapefile = TypeShapefile.Ligne;
                    coord = coord.Substring(12, coord.Length - 14);
                    string[] tabCoord = coord.Split(new string[] { "," }, StringSplitOptions.None);
                    List<EventCoord> ecTmp = new List<EventCoord>();

                    for (int i = 0; i < tabCoord.Length; i++)
                    {
                        string[] tabtmp = tabCoord[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        
                        ecTmp.Add(new EventCoord
                        {
                            X = double.Parse(Util.FixDecSeparator(tabtmp[0])),
                            Y = double.Parse(Util.FixDecSeparator(tabtmp[1]))
                        });
                    }

                    es.Coordinates.AddRange(ecTmp);
                }
                else if (coord.StartsWith("\"POINT"))
                {
                    es.TypeShapefile = TypeShapefile.Point;
                    coord = coord.Substring(7, coord.Length - 9);
                    string[] tabtmp = coord.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    es.Coordinates.Add(new EventCoord
                    {
                        X = double.Parse(Util.FixDecSeparator(tabtmp[0])),
                        Y = double.Parse(Util.FixDecSeparator(tabtmp[1]))
                    });
                }

                eventsFile.Add(es);
            }
            sr.Close();

            return eventsFile;
        }

        private List<EventShape> ReadCSVmin(StreamReader sr)
        {
            List<EventShape> eventsFile = new List<EventShape>();

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] tab = line.Split(new char[] { ';' });

                string sX = Util.FixDecSeparator(tab[0]);
                string sY = Util.FixDecSeparator(tab[1]);

                eventsFile.Add(new EventShape
                {
                    Coordinates = new List<EventCoord>() {
                        new EventCoord
                        {
                            X = double.Parse(sX),
                            Y = double.Parse(sY)
                        } },
                    Label = tab[3],
                    Description = tab[3],
                    GdhDebut = DateTime.Parse(tab[2]),
                    GdhFin = DateTime.Parse(tab[2]),
                    TypeShapefile = TypeShapefile.Point,
                    NomStyle = tab[4]
                });
            }

            sr.Close();
            return eventsFile;
        }

        private List<EventShape> ReadCSVcomplet(StreamReader sr)
        {
            List<EventShape> eventsFile = new List<EventShape>();

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] tab = line.Split(new char[] { ';' });

                string sX = Util.FixDecSeparator(tab[0]);
                string sY = Util.FixDecSeparator(tab[1]);

                eventsFile.Add(new EventShape
                {
                    Coordinates = new List<EventCoord>() {
                        new EventCoord
                        {
                            X = double.Parse(sX),
                            Y = double.Parse(sY)
                        } },
                    GdhDebut = DateTime.Parse(tab[2]),
                    GdhFin = DateTime.Parse(tab[3]),
                    Label = tab[4],
                    Description = tab[5],
                    NomStyle = tab[6],
                    TypeShapefile = TypeShapefile.Point
                });
            }

            sr.Close();
            return eventsFile;
        }

        private void b_loadData_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Application.StartupPath;
            ofd.ShowDialog();
            string datafile = ofd.FileName;

            if (string.IsNullOrEmpty(datafile) || !File.Exists(datafile)) return;

            dgv.Columns.Clear();

            if (datafile.EndsWith(".csv"))
            {
                events = ReadCsv(datafile);
            }
            else if (datafile.EndsWith(".wkt"))
            {
                InitDgv(CSVreader.fields);
                events = ReadWKT(datafile);
            }

            //MessageBox.Show(events.Count + " evenements chargés");

            if (events.Count == 0) return;

            GetTimeUnit();
            l_unite.Text = s.CurrentSpan + " " + GetTimeUnitStr();
            SetSlider();
            GetPoints();
        }
        #endregion


        string[] tabUnitStr = new string[] { "secondes", "minutes", "heures", "jours", "mois" };
        int[] tabUnit = new int[] { 1, 60, 60, 24, 30 };
        int idxTabUnit = 0;
        DateTime gdhMin = DateTime.MaxValue, gdhMax = DateTime.MinValue;

        private string GetTimeUnitStr()
        {
            return tabUnitStr[idxTabUnit];
        }

        private void GetTimeUnit()
        {
            gdhMin = DateTime.MaxValue;
            gdhMax = DateTime.MinValue;
            int seuil = 10;

            for (int i = 0; i < events.Count; i++)
            {
                if (gdhMin > events[i].GdhDebut) gdhMin = events[i].GdhDebut;
                if (gdhMax < events[i].GdhFin) gdhMax = events[i].GdhFin;
            }
            TimeSpan ts = gdhMax - gdhMin;
            l_minValue.Text = gdhMin.ToShortDateString() + "\n" + gdhMin.ToShortTimeString();
            l_maxValue.Text = gdhMax.ToShortDateString() + "\n" + gdhMax.ToShortTimeString();

            int delta = (int)(ts.TotalSeconds * 0.05);
            gdhMin = gdhMin.Subtract(new TimeSpan(0, 0, delta));
            gdhMax = gdhMax.AddSeconds(delta);

            ts = gdhMax - gdhMin;
            double nb = ts.TotalSeconds;

            idxTabUnit = 0;
            for (int i = 0; i < tabUnit.Length - 1; i++)
            {
                nb = nb / tabUnit[idxTabUnit + 1];
                if (nb < seuil) break;
                idxTabUnit++;
            }
        }
        
        private double GetSecondsToUnits(double seconds)
        {
            for (int i = 0; i < idxTabUnit + 1; i++)
                seconds = seconds / tabUnit[i];

            return seconds;
        }

        private double GetUnitsToSeconds(double units)
        {
            for (int i = 0; i < idxTabUnit + 1; i++)
                units = units * tabUnit[i];

            return units;
        }

        private void SetSlider()
        {
            TimeSpan ts = gdhMax - gdhMin;
            double test = GetSecondsToUnits(ts.TotalSeconds);

            s.Minimum = 0;
            s.Maximum = (int)Math.Ceiling(test);
            s.LargeChange = (int)(s.Maximum * 0.1);

            double xx = s.Maximum * 0.05;
            if (xx < 1) xx = 1;

            s.SetSpan((int)xx);
        }
        
    }
}
