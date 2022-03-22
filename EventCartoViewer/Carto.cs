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
        static AxMap axMap1;

        public static void Init(AxMap mapObj)
        {
            axMap1 = mapObj;
        
            //axMap1.Clear();
            axMap1.TileProvider = tkTileProvider.ProviderNone;
            axMap1.CursorMode = tkCursorMode.cmPan;
            axMap1.MapUnits = tkUnitsOfMeasure.umMeters;
            axMap1.SendMouseUp = true;
            axMap1.ShowCoordinatesFormat = tkAngleFormat.afDegrees;
            axMap1.MouseUpEvent += AxMap1_MouseUpEvent;

            for (int i = 0; i < Settings.cartes.Count; i++)
            {
                string carteFichier = "carte/" + Settings.cartes[i];
                axMap1.AddLayerFromFilename(carteFichier, tkFileOpenStrategy.fosAutoDetect, true);
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
            sf.CreateNewWithShapeID("", ShpfileType.SHP_POLYLINE);
            layerLine = axMap1.AddLayer(sf, true);

            Utils utils = new Utils();
            LinePattern pattern = new LinePattern();
            pattern.AddLine(utils.ColorByName(tkMapColor.Black), 2.0f, tkDashStyle.dsSolid);
            
            ShapefileCategory ct = sf.Categories.Add("TirGonio");
            ct.DrawingOptions.LinePattern = pattern;
            ct.DrawingOptions.UseLinePattern = true;
            

            sf = new Shapefile();
            sf.CreateNewWithShapeID("", ShpfileType.SHP_POINT);
            layerPoint = axMap1.AddLayer(sf, true);

            sf.DefaultDrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
            sf.DefaultDrawingOptions.FillColor = utils.ColorByName(tkMapColor.Blue);
            sf.DefaultDrawingOptions.PointSize = 10.0f;
            sf.CollisionMode = tkCollisionMode.AllowCollisions;


            sf = new Shapefile();
            sf.CreateNewWithShapeID("", ShpfileType.SHP_POLYGON);
            layerArea = axMap1.AddLayer(sf, true);
        }

        public static void DrawPoint(EventCoord cPoint)
        {
            double x = 0, y = 0;
            axMap1.DegreesToProj(cPoint.X, cPoint.Y, ref x, ref y);
            Point p = new Point { x = x, y = y };

            Shapefile sf = axMap1.get_Shapefile(axMap1.get_LayerHandle(layerPoint));
            Shape shp = new Shape();
            shp.Create(ShpfileType.SHP_POINT);

            int index = shp.numPoints;
            shp.InsertPoint(p, ref index);
            index = sf.NumShapes;
            
            sf.EditInsertShape(shp, ref index);
        }

        public static void DrawLine(List<EventCoord> coordinates)
        {
            Shapefile sf = axMap1.get_Shapefile(axMap1.get_LayerHandle(layerLine));
            Shape shp = new Shape();
            shp.Create(ShpfileType.SHP_POLYLINE);
            int index = shp.numPoints;

            double x = 0, y = 0;
            for (int i = 0; i < coordinates.Count; i++)
            {
                axMap1.DegreesToProj(coordinates[i].X, coordinates[i].Y, ref x, ref y);
                shp.AddPoint(x, y);
                /*
                Point p = new Point { x = x, y = y };
                shp.InsertPoint(p, ref index);
                index = shp.numPoints;
                */
            }

            sf.EditAddShape(shp);
            
            //sf.EditInsertShape(shp, ref index);
            //sf.set_ShapeCategory(index, 0);
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

        public static void SetBuffer()
        {
            Shapefile sfArea = axMap1.get_Shapefile(axMap1.get_LayerHandle(layerArea));

            Shapefile sfBuffer = sfArea.BufferByDistance(1, 20, false, true);
            int layerBuffer = axMap1.AddLayer(sfBuffer, true);
            
            //eventuellement
            axMap1.MoveLayerTop(layerBuffer);
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
