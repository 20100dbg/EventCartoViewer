using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SliderControl;

namespace EventCartoViewer
{
    public partial class Form1 : Form
    {
        Slider s;
        List<EventShape> events = new List<EventShape>();

        public Form1()
        {
            InitializeComponent();

            Settings.LoadDefaultValues();
            Settings.ReadConfigFile();

            s = new Slider(this.Controls, new Point(40, 450), new Size(400, 22), 0, 100);
            s.SpanMoving += S_SpanMoving;
            s.SpanMoved += S_SpanMoving;
            s.SpanResizing += S_SpanResizing;
            s.SpanResized += S_SpanResizing;

            Init();
            InitDgv();
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
                    Carto.AddLayer("carte/" + cblb_couches.Items[i].ToString());
                }
            }

            Settings.WriteConfigFile();
        }

        public void ForceRefreshCarto()
        {
            Carto.Init(axMap1);
            SaveCarto();
            GetPoints();    
        }

        private void b_test_Click(object sender, EventArgs e)
        {
            Carto.SetBuffer();
        }

        private void b_applyCarto_Click(object sender, EventArgs e)
        {
            ForceRefreshCarto();
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
        }

        long refTicks = 0;
        long delay = 50000;

        private void GetPoints()
        {
            if (events.Count == 0) return;

            long nowTicks = DateTime.Now.Ticks;
            if (nowTicks < refTicks + delay) return;
            refTicks = nowTicks;

            int val = s.CurrentValue;
            int span = s.CurrentSpan;

            double valSecondes = GetUnitsToSeconds(val);
            double spanSecondes = GetUnitsToSeconds(span);

            DateTime dtStart = gdhMin.AddSeconds(valSecondes);
            DateTime dtFin = dtStart.AddSeconds(spanSecondes);
            l_currentValue.Text = dtStart.ToString();

            List<EventShape> aDessiner = new List<EventShape>();

            Carto.ClearCarto();

            for (int i = 0; i < events.Count; i++)
            {
                if ((events[i].GdhDebut >= dtStart && events[i].GdhDebut <= dtFin) ||
                    (events[i].GdhFin >= dtStart && events[i].GdhFin <= dtFin) ||
                    (events[i].GdhDebut <= dtStart && events[i].GdhFin >= dtFin))
                {
                    //on dessine toujours les points
                    for (int j = 0; j < events[i].Coordinates.Count; j++)
                    {
                        Carto.DrawPoint(events[i].Coordinates[j]);
                    }

                    if (events[i].TypeForme == 2) Carto.DrawLine(events[i].Coordinates);
                    else if (events[i].TypeForme == 3) Carto.DrawArea(events[i].Coordinates);

                    aDessiner.Add(events[i]);
                }
            }

            FillDgv(aDessiner);
            axMap1.Redraw();
        }

        #region import
        private List<EventShape> ReadFileToEvents(string filename)
        {
            List<EventShape> eventsFile = new List<EventShape>();
            StreamReader sr = new StreamReader(filename);

            string line;
            sr.ReadLine(); //entetes

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
                int typeForme = 0;

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
                    typeForme = 3;
                    coord = coord.Substring(10, coord.Length - 13);
                    string[] tabCoord = coord.Split(new string[] { "," }, StringSplitOptions.None);

                    for (int i = 0; i < tabCoord.Length; i++)
                    {
                        string[] tabtmp = tabCoord[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        es.Coordinates.Add(new EventCoord
                        {
                            X = double.Parse(Util.FixDecSeparator(tabtmp[0])),
                            Y = double.Parse(Util.FixDecSeparator(tabtmp[1]))
                        });
                    }
                }
                else if (coord.StartsWith("\"LINESTRING"))
                {
                    typeForme = 2;
                    coord = coord.Substring(12, coord.Length - 14);
                    string[] tabCoord = coord.Split(new string[] { "," }, StringSplitOptions.None);

                    for (int i = 0; i < tabCoord.Length; i++)
                    {
                        string[] tabtmp = tabCoord[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        es.Coordinates.Add(new EventCoord
                        {
                            X = double.Parse(Util.FixDecSeparator(tabtmp[0])),
                            Y = double.Parse(Util.FixDecSeparator(tabtmp[1]))
                        });
                    }
                }
                else if (coord.StartsWith("\"POINT"))
                {
                    typeForme = 1;
                    coord = coord.Substring(7, coord.Length - 9);
                    string[] tabtmp = coord.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    es.Coordinates.Add(new EventCoord
                    {
                        X = double.Parse(Util.FixDecSeparator(tabtmp[0])),
                        Y = double.Parse(Util.FixDecSeparator(tabtmp[1]))
                    });
                }

                es.TypeForme = typeForme;
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
                events = ReadFileToEvents(datafile);
            }
            else if (datafile.EndsWith(".wkt"))
            {
                events = ReadWKT(datafile);
            }

            GetTimeUnit();
            SetSlider();
            GetPoints();
        }
        #endregion

        
        int[] tabUnit = new int[] { 1, 60, 24, 30, 12, 1 };
        int idxTabUnit = -1;
        DateTime gdhMin = DateTime.MaxValue, gdhMax = DateTime.MinValue;

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
            double nb = ts.TotalSeconds;
            idxTabUnit = -1;

            while (nb > seuil && idxTabUnit < tabUnit.Length)
            {
                nb = nb / tabUnit[++idxTabUnit];
            }
        }
        
        private double GetSecondsToUnits(double seconds)
        {
            for (int i = 0; i < idxTabUnit; i++)
                seconds = seconds / tabUnit[i];

            return seconds;
        }

        private double GetUnitsToSeconds(double units)
        {
            for (int i = 0; i < idxTabUnit; i++)
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
        }
        
    }
}
