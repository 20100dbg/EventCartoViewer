using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using MapWinGIS;
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
            
            s = new Slider(this, new System.Drawing.Point(40, 250), new System.Drawing.Size(400, 22), 0, 100);
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
                if (events[i].GdhDebut == null ||
                    (events[i].GdhDebut >= dtStart && events[i].GdhDebut <= dtFin) ||
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

        private void LoadCarto()
        {
            foreach (string file in Directory.EnumerateFiles("carte/", "*.tif"))
            {
                string nom = Path.GetFileName(file);

                bool flag = Settings.couchesSelectionnees.Contains(nom);
                cblb_couches.Items.Add(nom, flag);
                
                if (flag)
                    axMap1.AddLayerFromFilename(file, tkFileOpenStrategy.fosAutoDetect, true);
            }
        }
        

        private List<EventShape> ReadFileToEvents(string filename)
        {
            StreamReader sr = new StreamReader(filename);

            string line;
            sr.ReadLine(); //entetes

            while ((line = sr.ReadLine()) != null)
            {
                string[] tab = line.Split(new char[] { ';' });

                int idEvent = int.Parse(tab[6]);
                //EventShape es = events.Find((x) => { return x.Id == idEvent; });

                double x = double.Parse(tab[0]);
                double y = double.Parse(tab[1]);

                EventShape es = new EventShape
                {
                    Id = idEvent,
                    GdhDebut = DateTime.Parse(tab[2]),
                    GdhFin = DateTime.Parse(tab[3]),
                    Label = tab[4],
                    Description = tab[5],
                    Coordinates = new List<Coord>(),
                    TypeForme = int.Parse(tab[7]),
                    Style = StyleToColor(idEvent)
                };
                es.Coordinates.Add(new Coord { X = x, Y = y });
                events.Add(es);
            }

            sr.Close();

            events.Sort(EventShape.TriGdh);
            return events;
        }
        
        private void DrawShapes(List<EventShape> shapes)
        {
            shapes.ForEach((x) => { 
                if (x.TypeForme == 1) Carto.DrawPoint(x.Coordinates[0]);
                else if (x.TypeForme == 3) Carto.DrawLine(x.Coordinates);
                else if (x.TypeForme == 4) Carto.DrawArea(x.Coordinates);
            });
        }

        private void InitDgv()
        {
            dgv.Columns.Add("Id", "Id");
            dgv.Columns.Add("Label", "Label");
            dgv.Columns.Add("Description", "Description");
        }

        private void SelectDGV(List<EventShape> events)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
                dgv.Rows[i].Selected = (events.Exists((x) => x.Id == (int)dgv[0, i].Value));
        }

        private void LoadShapesToDgv(List<EventShape> shapes)
        {
            for (int i = 0; i < shapes.Count; i++)
            {

            }
        }

        private void SetSlider()
        {
            DateTime dtMin = events[0].GdhDebut;
            DateTime dtMax = events[events.Count - 1].GdhFin;
            TimeSpan ts = dtMax - dtMin;
            s.Minimum = 0;
            s.Maximum = (int)Math.Ceiling(ts.TotalMinutes);
            s.LargeChange = s.CurrentSpan;
        }

        private void SaveCarto()
        {
            Settings.couchesSelectionnees.Clear();

            for (int i = 0; i < cblb_couches.Items.Count; i++)
            {
                if (cblb_couches.GetItemChecked(i))
                    Settings.couchesSelectionnees.Add(cblb_couches.Items[i].ToString());
            }

            Settings.WriteConfigFile();
        }


        private void b_loadData_Click(object sender, EventArgs e)
        {
            SaveCarto();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Application.StartupPath;
            ofd.ShowDialog();
            string datafile = ofd.FileName;

            if (string.IsNullOrEmpty(datafile) || !File.Exists(datafile)) return;


            events = ReadFileToEvents(datafile);
            SetSlider();
        }
    }
}
