using MapWinGIS;
using AxMapWinGIS;
using System.Collections.Generic;

namespace EventCartoViewer
{
    internal static class Carto
    {
        static int layerLine = -1;
        static int layerPoint = -1;
        static int layerArea = -1;
        //static int layerBuffer = -1;
        static AxMap axMap1;

        public static void Init(AxMap mapObj)
        {
            axMap1 = mapObj;
        
            axMap1.Clear();
            axMap1.TileProvider = tkTileProvider.ProviderNone;
            axMap1.CursorMode = tkCursorMode.cmPan;
            axMap1.MapUnits = tkUnitsOfMeasure.umMeters;
            axMap1.SendMouseUp = true;
            axMap1.ShowCoordinatesFormat = tkAngleFormat.afDegrees;
            axMap1.MouseUpEvent += AxMap1_MouseUpEvent;

            for (int i = 0; i < Settings.cartes.Count; i++)
            {
                string carteFichier = "carte/" + Settings.cartes[i];
                AddLayer(carteFichier);
            }

            InitShapeFiles();
        }

        public static void AddLayer(string filename)
        {
            axMap1.AddLayerFromFilename(filename, tkFileOpenStrategy.fosAutoDetect, true);
        }

        public static void InitShapeFiles()
        {
            Shapefile sf = new Shapefile();
            sf.CreateNewWithShapeID("", ShpfileType.SHP_POLYGON);
            layerArea = axMap1.AddLayer(sf, true);

            ColorScheme scheme = new ColorScheme();
            scheme.SetColors2(tkMapColor.Wheat, tkMapColor.Salmon);
            sf.Categories.ApplyColorScheme(tkColorSchemeType.ctSchemeGraduated, scheme);

            sf = new Shapefile();
            sf.CreateNewWithShapeID("", ShpfileType.SHP_POLYLINE);
            layerLine = axMap1.AddLayer(sf, true);

            Utils utils = new Utils();
            LinePattern pattern = new LinePattern();
            pattern.AddLine(utils.ColorByName(tkMapColor.Black), 1.0f, tkDashStyle.dsSolid);
            
            ShapefileCategory ct = sf.Categories.Add("TirGonio");
            ct.DrawingOptions.LinePattern = pattern;
            ct.DrawingOptions.UseLinePattern = true;
            

            sf = new Shapefile();
            sf.CreateNewWithShapeID("", ShpfileType.SHP_POINT);

            sf.StartEditingTable();
            sf.EditAddField("label", FieldType.STRING_FIELD, 0, 50);

            sf.Labels.FrameVisible = false;
            sf.Labels.FontSize = 8;
            sf.Labels.AvoidCollisions = false;

            layerPoint = axMap1.AddLayer(sf, true);

            sf.DefaultDrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
            sf.DefaultDrawingOptions.FillColor = utils.ColorByName(tkMapColor.Black);
            sf.DefaultDrawingOptions.PointSize = 8.0f;
            sf.CollisionMode = tkCollisionMode.AllowCollisions;
        }

        public static void DrawPoint(EventCoord cPoint, string label)
        {
            Shapefile sf = axMap1.get_Shapefile(axMap1.get_LayerHandle(layerPoint));
            Shape shp = new Shape();
            shp.Create(ShpfileType.SHP_POINT);

            double x = 0, y = 0;
            axMap1.DegreesToProj(cPoint.X, cPoint.Y, ref x, ref y);
            shp.AddPoint(x, y);

            sf.EditAddShape(shp);
            sf.EditCellValue(1, sf.NumShapes - 1, label);
        }

        public static void DrawLine(List<EventCoord> coordinates)
        {
            Shapefile sf = axMap1.get_Shapefile(axMap1.get_LayerHandle(layerLine));
            Shape shp = new Shape();
            shp.Create(ShpfileType.SHP_POLYLINE);

            double x = 0, y = 0;
            for (int i = 0; i < coordinates.Count; i++)
            {
                axMap1.DegreesToProj(coordinates[i].X, coordinates[i].Y, ref x, ref y);
                shp.AddPoint(x, y);
            }

            sf.EditAddShape(shp);
        }

        public static void DrawArea(List<EventCoord> coordinates)
        {
            Shapefile sf = axMap1.get_Shapefile(axMap1.get_LayerHandle(layerArea));
            Shape shp = new Shape();
            shp.Create(ShpfileType.SHP_POLYGON);

            double x = 0, y = 0;
            for (int i = 0; i < coordinates.Count; i++)
            {
                axMap1.DegreesToProj(coordinates[i].X, coordinates[i].Y, ref x, ref y);
                shp.AddPoint(x, y);
            }

            //fermer la boucle
            axMap1.DegreesToProj(coordinates[0].X, coordinates[0].Y, ref x, ref y);
            shp.AddPoint(x, y);

            sf.EditAddShape(shp);
        }

        public static void GenerateLabels()
        {
            Shapefile sf = axMap1.get_Shapefile(axMap1.get_LayerHandle(layerPoint));

            for (int i = 0; i < sf.NumShapes; i++)
            {
                string text = sf.Table.CellValue[1, i].ToString();
                double x = sf.Shape[i].Center.x;
                double y = sf.Shape[i].Center.y;

                if (Settings.centrerLabel)
                {
                    x = x - text.Length * 500;
                    y = y + 1500;
                }

                sf.Labels.AddLabel(text, x, y);
            }
            sf.Labels.Synchronized = true;
        }

        public static void SetBuffer()
        {
            Shapefile sfArea = axMap1.get_Shapefile(axMap1.get_LayerHandle(layerArea));

            double distance = Settings.tailleBuffer; //metres
            Shapefile sfBuffer = sfArea.BufferByDistance(distance, 30, false, true);

            var utils = new Utils();
            sfBuffer.DefaultDrawingOptions.LineWidth = 1.0f;
            sfBuffer.DefaultDrawingOptions.LineColor = utils.ColorByName(tkMapColor.Blue);
            sfBuffer.DefaultDrawingOptions.FillBgColor = utils.ColorByName(tkMapColor.LightBlue);

            int layerBuffer = axMap1.AddLayer(sfBuffer, true);
            axMap1.MoveLayer(layerBuffer, layerArea);
        }

        /// <summary>
        /// Supprime tous les vecteurs affichés
        /// </summary>
        public static void ClearCarto()
        {
            Shapefile sf = axMap1.get_Shapefile(axMap1.get_LayerHandle(layerLine));
            sf.EditClear();
            sf = axMap1.get_Shapefile(axMap1.get_LayerHandle(layerPoint));
            sf.EditClear();
            sf = axMap1.get_Shapefile(axMap1.get_LayerHandle(layerArea));
            sf.EditClear();
        }

        /// <summary>
        /// Centre la carto sur les coordonnées MGRS fournies
        /// </summary>
        /// <param name="mgrs"></param>
        public static void CentrerCoordonnee(string mgrs)
        {
            Coord c = Util.ConvertMGRStoLatLong(mgrs);
            CentrerCoordonnee(c);
        }

        /// <summary>
        /// Centre la carto sur les coordonnées (object Coord)
        /// </summary>
        /// <param name="c"></param>
        public static void CentrerCoordonnee(Coord c)
        {
            axMap1.SetLatitudeLongitude(c.Y, c.X);
            axMap1.ZoomToTileLevel(Settings.niveauZoomCentrer);
        }

        public static void AxMap1_MouseUpEvent(object sender, AxMapWinGIS._DMapEvents_MouseUpEvent e)
        {
            double currentX = 0.0, currentY = 0.0;
            axMap1.PixelToDegrees(e.x, e.y, ref currentX, ref currentY);

            //MessageBox.Show(Util.ConvertLatLongToMGRS(currentX, currentY));
        }
    }
}
