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
            
            s = new Slider(this, new Point(40, 470), new Size(400, 22), 0, 100);
            s.SpanMoving += S_SpanMoving;
            s.SpanResizing += S_SpanResizing;

            Init();
            InitDgv();
        }

        public void Init()
        {
            //Affiche les couches dispo
            LoadCarto();

            //charge la carto
            Carto.Init(axMap1);

            this.cblb_couches.ItemCheck += Cblb_couches_ItemCheck;
        }

        private void InitDgv()
        {
            dgv.Columns.Add("Id", "Id");
            dgv.Columns.Add("Label", "Label");
            dgv.Columns.Add("Description", "Description");
        }

        private void LoadCarto()
        {
            foreach (string file in Directory.EnumerateFiles("carte/", "*.tif"))
            {
                string nom = Path.GetFileName(file);

                bool flag = Settings.cartes.Contains(nom);
                cblb_couches.Items.Add(nom, flag);
            }
        }

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

        private void GetPoints()
        {
            int val = s.CurrentValue;
            int span = s.CurrentSpan;

            DateTime dtStart = events[0].GdhDebut.AddMinutes(val);
            DateTime dtFin = dtStart.AddMinutes(span);
            
            List<EventShape> aDessiner = new List<EventShape>();

            for (int i = 0; i < events.Count; i++)
            {
                if ((events[i].GdhDebut >= dtStart && events[i].GdhDebut <= dtFin) ||
                    (events[i].GdhFin >= dtStart && events[i].GdhFin <= dtFin) ||
                    (events[i].GdhDebut <= dtStart && events[i].GdhFin >= dtFin))
                {
                    aDessiner.Add(events[i]);
                }
            }

            Carto.ClearCarto();
            DrawShapes(aDessiner);
            axMap1.Redraw();
        }

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
                DateTime GdhDebut = DateTime.Parse(tab[2]);
                DateTime GdhFin = DateTime.Parse(tab[3]);

                EventShape es = eventsFile.Find((x) => { return x.Id == idEvent; });

                if (es == null)
                {
                    es = new EventShape
                    {
                        Id = idEvent,
                        Label = tab[4],
                        GdhDebut = GdhDebut,
                        GdhFin = GdhFin,
                        Description = tab[5],
                        Coordinates = new List<EventCoord>(),
                        TypeForme = int.Parse(tab[7]),
                        Style = StyleToColor(idEvent)
                    };

                    eventsFile.Add(es);
                }
                /*
                else
                {
                    if (es.GdhDebut > GdhDebut) es.GdhDebut = GdhDebut;
                    if (es.GdhFin < GdhFin) es.GdhFin = GdhFin;
                }
                */

                es.Coordinates.Add(new EventCoord { 
                    X = double.Parse(tab[0]),
                    Y = double.Parse(tab[1])
                });

                
            }

            sr.Close();

            //yo.Sort(EventShape.TriGdh);
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

                if (coord.StartsWith("POLYGON"))
                {
                    coord = coord.Substring(10, coord.Length - 13);
                    string[] tabCoord = coord.Split(new string[] { "," }, StringSplitOptions.None);

                    for (int i = 0; i < tabCoord.Length; i++)
                    {
                        string[] tabtmp = tabCoord[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        es.Coordinates.Add(new EventCoord
                        {
                            X = double.Parse(tabtmp[0]),
                            Y = double.Parse(tabtmp[1])
                        });
                    }
                }
                else if (coord.StartsWith("LINESTRING"))
                {
                    coord = coord.Substring(12, coord.Length - 14);
                    string[] tabCoord = coord.Split(new string[] { "," }, StringSplitOptions.None);

                    for (int i = 0; i < tabCoord.Length; i++)
                    {
                        string[] tabtmp = tabCoord[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        es.Coordinates.Add(new EventCoord
                        {
                            X = double.Parse(tabtmp[0]),
                            Y = double.Parse(tabtmp[1])
                        });
                    }
                }
                else if (coord.StartsWith("POINT"))
                {
                    coord = coord.Substring(7, coord.Length - 9);
                    string[] tabtmp = coord.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    es.Coordinates.Add(new EventCoord
                    {
                        X = double.Parse(tabtmp[0]),
                        Y = double.Parse(tabtmp[1])
                    });
                }

                eventsFile.Add(es);
            }
            sr.Close();

            return eventsFile;
        }

        private void DrawShapes(List<EventShape> shapes)
        {
            shapes.ForEach((x) => { 
                if (x.TypeForme == 1) Carto.DrawPoint(x.Coordinates[0]);
                else if (x.TypeForme == 3) Carto.DrawLine(x.Coordinates);
                else if (x.TypeForme == 4) Carto.DrawArea(x.Coordinates);
            });
        }

        private void SelectDGV(List<EventShape> events)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
                dgv.Rows[i].Selected = (events.Exists((x) => x.Id == (int)dgv[0, i].Value));
        }

        private void GetTimeUnit()
        {
            int seuil = 10;

            DateTime gdhMin = DateTime.MaxValue, gdhMax = DateTime.MinValue;

            for (int i = 0; i < events.Count; i++)
            {
                if (gdhMin > events[i].GdhDebut) gdhMin = events[i].GdhDebut;
                if (gdhMax > events[i].GdhFin) gdhMax = events[i].GdhFin;
            }

            TimeSpan ts = gdhMax - gdhMin;
            int[] tab = new int[] { 60, 24, 30, 12 };
            double nb = ts.TotalSeconds;
            int idxTab = 0;

            while (nb > seuil && idxTab < tab.Length)
            {
                nb = nb / tab[idxTab++];
            }
        }


        private void SetSlider()
        {
            DateTime dtMin = events[0].GdhDebut;
            DateTime dtMax = events[events.Count - 1].GdhFin;
            TimeSpan ts = dtMax - dtMin;

            s.Minimum = 0;
            s.Maximum = (int)Math.Ceiling(ts.TotalMinutes);
            s.LargeChange = (int)(s.Maximum * 0.1);
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

            SetSlider();
        }

        private void Cblb_couches_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            SaveCarto();
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

        private void b_test_Click(object sender, EventArgs e)
        {

        }
    }
}
