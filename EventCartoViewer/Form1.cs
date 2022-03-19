using System;
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
        int niveauZoomCentrer = 13;
        List<EventPoint> points = new List<EventPoint>();

        public Form1()
        {
            InitializeComponent();
            s = new Slider(this, new System.Drawing.Point(40, 250), new System.Drawing.Size(400, 22), 0, 100);
            s.SpanMoving += S_SpanMoving;
            s.SpanResizing += S_SpanResizing;
            LoadCarto();
            InitShapeFiles();
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

            DateTime dtStart = points[0].Gdh.AddMinutes(val);
            DateTime dtFin = dtStart.AddMinutes(span);
            List<EventPoint> pointsDessines = new List<EventPoint>();

            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Gdh == null ||
                    (points[i].Gdh >= dtStart && points[i].Gdh <= dtFin))
                {
                    pointsDessines.Add(points[i]);
                }
            }

            ClearCarto();
            DrawPoints(pointsDessines);
            axMap1.Redraw();
        }

        private void LoadCarto()
        {
            axMap1.Clear();
            axMap1.TileProvider = tkTileProvider.ProviderNone;
            axMap1.CursorMode = tkCursorMode.cmPan;
            axMap1.MapUnits = tkUnitsOfMeasure.umMeters;
            axMap1.SendSelectBoxFinal = true;
            axMap1.SendMouseUp = true;
            axMap1.ShowCoordinatesFormat = tkAngleFormat.afDegrees;

            axMap1.MouseUpEvent += AxMap1_MouseUpEvent;

            foreach (string file in Directory.EnumerateFiles("carte/", "*.tif"))
            {
                string nom = Path.GetFileName(file);
                cblb_couches.Items.Add(nom, true);

                if (File.Exists(file))
                    axMap1.AddLayerFromFilename(file, tkFileOpenStrategy.fosAutoDetect, true);

                break;
            }
        }

        int layerLine = -1;
        int layerPoint = -1;
        int layerSurface = -1;

        private void InitShapeFiles()
        {
            Shapefile sf = new Shapefile();
            sf.CreateNewWithShapeID("", ShpfileType.SHP_POLYLINE);
            layerLine = axMap1.AddLayer(sf, true);

            Utils utils = new Utils();
            LinePattern pattern = new LinePattern();
            pattern.AddLine(utils.ColorByName(tkMapColor.Black), 2.0f, tkDashStyle.dsSolid);
            ShapefileCategory ct = sf.Categories.Add("Line");
            ct.DrawingOptions.LinePattern = pattern;
            ct.DrawingOptions.UseLinePattern = true;

            sf = new Shapefile();
            sf.CreateNewWithShapeID("", ShpfileType.SHP_POINT);
            layerPoint = axMap1.AddLayer(sf, true);

            ShapeDrawingOptions options = sf.DefaultDrawingOptions;
            options.PointType = tkPointSymbolType.ptSymbolStandard;
            options.FillColor = utils.ColorByName(tkMapColor.Blue);
            options.PointSize = 10.0f;
            sf.CollisionMode = tkCollisionMode.AllowCollisions;


            sf = new Shapefile();
            sf.CreateNewWithShapeID("", ShpfileType.SHP_POLYGON);
            layerSurface = axMap1.AddLayer(sf, true);

            //axMap1.Redraw();
        }


        public void DrawPoint(Coord cPoint)
        {
            double x = 0, y = 0;
            axMap1.DegreesToProj(cPoint.X, cPoint.Y, ref x, ref y);
            Point p = new Point { x = x, y = y };

            Shapefile sf = axMap1.get_Shapefile(axMap1.get_LayerHandle(layerPoint));

            //dessin
            Shape shp = new Shape();
            shp.Create(ShpfileType.SHP_POINT);

            int index = shp.numPoints;
            shp.InsertPoint(p, ref index);
            index = sf.NumShapes;
            sf.EditInsertShape(shp, ref index);
        }

        public void DrawLine(Coord c1, Coord c2)
        {
            double x = 0, y = 0;
            axMap1.DegreesToProj(c1.X, c1.Y, ref x, ref y);
            Point p1 = new Point { x = x, y = y };
            axMap1.DegreesToProj(c2.X, c2.Y, ref x, ref y);
            Point p2 = new Point { x = x, y = y };

            Shapefile sf = axMap1.get_Shapefile(axMap1.get_LayerHandle(layerLine));

            //dessin
            Shape shp = new Shape();
            shp.Create(ShpfileType.SHP_POLYLINE);
            int index = shp.numPoints;
            shp.InsertPoint(p1, ref index);
            index = shp.numPoints;
            shp.InsertPoint(p2, ref index);
            index = sf.NumShapes;
            sf.EditInsertShape(shp, ref index);
            sf.set_ShapeCategory(index, 0);

        }

        public void ClearCarto()
        {
            //vider la carto courante
            Shapefile sf = axMap1.get_Shapefile(axMap1.get_LayerHandle(layerLine));
            sf.EditClear();
            sf = axMap1.get_Shapefile(axMap1.get_LayerHandle(layerPoint));
            sf.EditClear();
            sf = axMap1.get_Shapefile(axMap1.get_LayerHandle(layerSurface));
            sf.EditClear();
        }


        public void CentrerCoordonnee(string mgrs)
        {
            Coord c = Util.ConvertMGRStoLatLong(mgrs);
            CentrerCoordonnee(c);
        }

        public void CentrerCoordonnee(Coord c)
        {
            axMap1.SetLatitudeLongitude(c.Y, c.X);
            axMap1.ZoomToTileLevel(niveauZoomCentrer);
        }

        private void AxMap1_MouseUpEvent(object sender, AxMapWinGIS._DMapEvents_MouseUpEvent e)
        {
            double currentX = 0.0, currentY = 0.0;
            axMap1.PixelToDegrees(e.x, e.y, ref currentX, ref currentY);
            
            //MessageBox.Show(Util.ConvertLatLongToMGRS(currentX, currentY));
        }


        private List<EventPoint> ReadFileToPoints(string filename)
        {
            //X, Y
            List<EventPoint> points = new List<EventPoint>();

            StreamReader sr = new StreamReader(filename);

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] tab = line.Split(new char[] { ';' });

                //X, Y, gdh, label, style

                points.Add(new EventPoint
                {
                    Id = points.Count,
                    Coordinates = new Coord { X = double.Parse(tab[0]), Y = double.Parse(tab[1]) },
                    Gdh = DateTime.Parse(tab[2]),
                    //Label = tab[3],
                    //Style = int.Parse(tab[4])
                });
            }

            sr.Close();

            points.Sort(EventPoint.TriGdh);
            return points;
        }

        private void DrawPoints(List<EventPoint> points)
        {
            for (int i = 0; i < points.Count; i++)
            {
                DrawPoint(points[i].Coordinates);
            }
        }

        private void LoadPointsToDgv(List<EventPoint> points)
        {
            for (int i = 0; i < points.Count; i++)
            {

            }
        }

        private void SetSlider()
        {
            DateTime dtMin = points[0].Gdh;
            DateTime dtMax = points[points.Count - 1].Gdh;
            TimeSpan ts = dtMax - dtMin;
            s.Minimum = 0;
            s.Maximum = (int)Math.Ceiling(ts.TotalMinutes);
        }

        private void b_loadData_Click(object sender, EventArgs e)
        {
            points = ReadFileToPoints("data.csv");
            SetSlider();
        }
    }
}
