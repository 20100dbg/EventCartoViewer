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

            Settings.LoadDefaultValues();
            Settings.ReadConfigFile();

            s = new Slider(this.Controls, new Point(40, 450), new Size(400, 22), 0, 100);
            s.TickRate = 50;
            s.SpanMoving += S_SpanMoving;
            s.SpanMoved += S_SpanMoving;
            s.SpanResizing += S_SpanResizing;
            s.SpanResized += S_SpanResizing;

            Init();
            InitDgv();
        }


        public void test()
        {
            /*
            int val = 35;
            int minVal = 1;
            int maxVal = 300;

            double nuance = Util.GetRapport(val, minVal, maxVal, 1025);
            Color c = Util.GetColorFromNuance(nuance, 255);
            */
        }

        public void Init()
        {
            //Affiche les couches dispo
            ListeCartoDispo();

            //charge la carto
            Carto.Init(axMap1);
        }

        #region dgv
        private void InitDgv()
        {
            dgv.Columns.Add("Id", "Id");
            dgv.Columns.Add("GdhDebut", "GdhDebut");
            dgv.Columns.Add("GdhFin", "GdhFin");
            dgv.Columns.Add("Label", "Label");
            dgv.Columns.Add("Description", "Description");

            dgv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //DataGridViewAutoSizeColumnMode.Fill;
        }

        private void FillDgv(List<EventShape> events)
        {
            dgv.Rows.Clear();

            for (int i = 0; i < events.Count; i++)
            {
                dgv.Rows.Add(new string[] {
                    events[i].Id.ToString(),
                    events[i].GdhDebut.ToString(),
                    events[i].GdhFin.ToString(),
                    events[i].Label,
                    events[i].Description,
                });
            }
        }

        private void SelectDGV(List<EventShape> events)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
                dgv.Rows[i].Selected = (events.Exists((x) => x.Id == (int)dgv[0, i].Value));
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

        private void b_test_Click(object sender, EventArgs e)
        {

        }

        private void b_applyCarto_Click(object sender, EventArgs e)
        {
            SaveCarto();

            Carto.Init(axMap1);
            
            GetPoints();
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


        private void GetPoints()
        {
            Carto.ClearCarto();
            if (events.Count == 0) return;

            int val = s.CurrentValue;
            int span = s.CurrentSpan;

            double valSecondes = GetUnitsToSeconds(val);
            double spanSecondes = GetUnitsToSeconds(span);

            DateTime dtStart = gdhMin.AddSeconds(valSecondes);
            DateTime dtFin = dtStart.AddSeconds(spanSecondes);
            l_currentValue.Text = dtStart.ToString();

            List<EventShape> aDessiner = new List<EventShape>();

            for (int i = 0; i < events.Count; i++)
            {
                if ((events[i].GdhDebut >= dtStart && events[i].GdhDebut <= dtFin) ||
                    (events[i].GdhFin >= dtStart && events[i].GdhFin <= dtFin) ||
                    (events[i].GdhDebut <= dtStart && events[i].GdhFin >= dtFin))
                {
                    //on dessine toujours les points
                    for (int j = 0; j < events[i].Coordinates.Count; j++)
                    {
                        Carto.DrawPoint(events[i].Coordinates[j], events[i].Label);
                    }

                    if (events[i].TypeForme == 2)
                    {
                        Carto.DrawLine(events[i].Coordinates);
                    }
                    else if (events[i].TypeForme == 3)
                    {
                        Carto.DrawArea(events[i].Coordinates);
                    }

                    aDessiner.Add(events[i]);
                }
            }

            //Carto.SetHeatmap();

            if (Settings.afficherBuffer)
                Carto.SetBuffer();

            if (Settings.afficherLabel)
                Carto.GenerateLabels();

            FillDgv(aDessiner);
            axMap1.Redraw();
        }

        #region import
        private List<EventShape> ReadCsv(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            string[] entetes = sr.ReadLine().Split(new char[] { ';' });

            List<EventShape> events;
            if (entetes.Length == 4) events = ReadCSVmin(sr);
            else if (entetes.Length == 8) events = ReadFileToEvents(sr);
            else events = new List<EventShape>();

            sr.Close();
            return events;
        }

        private List<EventShape> ReadFileToEvents(StreamReader sr)
        {
            List<EventShape> eventsFile = new List<EventShape>();

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] tab = line.Split(new char[] { ';' });
                int idEvent = int.Parse(tab[6]);

                EventShape es = new EventShape
                {
                    Id = idEvent,
                    Label = tab[4],
                    GdhDebut = DateTime.Parse(tab[2]),
                    GdhFin = DateTime.Parse(tab[3]),
                    Description = tab[5],
                    Coordinates = new List<EventCoord>(),
                    TypeForme = int.Parse(tab[7]),
                    Style = StyleToColor(idEvent)
                };

                es.Coordinates.Add(new EventCoord { 
                    X = double.Parse(tab[0]),
                    Y = double.Parse(tab[1])
                });

                eventsFile.Add(es);
            }
            sr.Close();


            List<EventShape> final = new List<EventShape>();

            for (int i = 0; i < eventsFile.Count; i++)
            {
                EventShape es = (EventShape)eventsFile[i].Clone();

                if (i < eventsFile.Count - 1)
                {
                    List<EventShape> tmp = eventsFile.GetRange(i + 1, eventsFile.Count - i - 2).FindAll((x) =>
                        {
                            return x.Id == eventsFile[i].Id && x.GdhDebut == eventsFile[i].GdhDebut
                                    && x.GdhFin == eventsFile[i].GdhFin;
                        });

                    for (int j = tmp.Count - 1; j >= 0; j--)
                    {
                        es.Coordinates.AddRange(tmp[j].Coordinates);
                        tmp.RemoveAt(i);
                    }
                }

                final.Add(es);
            }

            return final;
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
                    Id = eventsFile.Count,
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
                    TypeForme = 1,
                    Style = StyleToColor(eventsFile.Count)
                });
            }

            sr.Close();
            return eventsFile;
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
                int idEvent = int.Parse(tab[5]);

                EventShape es = new EventShape
                {
                    Id = idEvent,
                    Label = tab[3],
                    GdhDebut = DateTime.Parse(tab[1]),
                    GdhFin = DateTime.Parse(tab[2]),
                    Description = tab[4],
                    Coordinates = new List<EventCoord>(),
                    Style = StyleToColor(idEvent)
                };

                if (coord.StartsWith("\"POLYGON"))
                {
                    es.TypeForme = 3;
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
                    es.TypeForme = 2;
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
                    es.TypeForme = 1;
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

        private void b_loadData_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Application.StartupPath;
            ofd.ShowDialog();
            string datafile = ofd.FileName;

            if (string.IsNullOrEmpty(datafile) || !File.Exists(datafile)) return;

            if (datafile.EndsWith(".csv"))
            {
                events = ReadCsv(datafile);
            }
            else if (datafile.EndsWith(".wkt"))
            {
                events = ReadWKT(datafile);
            }

            
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

            s.SetSpan((int)(s.Maximum * 0.05));
        }
        
    }
}
