using MapWinGIS;
using AxMapWinGIS;
using System.Collections.Generic;

namespace EventCartoViewer
{
    internal static class Carto
    {

        static EventViewer parentForm;
        static AxMap axMap1;
        public static List<Couche> couches = new List<Couche>();
        public static List<StyleCouche> styles = new List<StyleCouche>();

        public static void Init(EventViewer pf, AxMap mapObj)
        {
            axMap1 = mapObj;
            parentForm = pf;

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

            //InitListShapefile();
            //InitShapeFiles();
        }

        public static void AddLayer(string filename)
        {
            axMap1.AddLayerFromFilename(filename, tkFileOpenStrategy.fosAutoDetect, true);
        }

        public static void InitStyleLayers(List<StyleCouche> styles)
        {
            couches.Clear();

            for (int i = 0; i < styles.Count; i++)
            {
                Couche c = new Couche
                {
                    Style = styles[i],
                    TypeShapefile = styles[i].TypeShapefile
                };

                InitSF(c);
                couches.Add(c);
            }
        }

        public static void InitSF(Couche c)
        {
            Utils utils = new Utils();
            Shapefile sf = new Shapefile();

            if (c.TypeShapefile == TypeShapefile.Point)
            {
                sf.CreateNewWithShapeID("", ShpfileType.SHP_POINT);
                sf.StartEditingTable();
                sf.EditAddField("label", FieldType.STRING_FIELD, 0, 50);

                sf.Labels.FrameVisible = false;
                sf.Labels.FontSize = 8;
                sf.Labels.AvoidCollisions = false;

                if (c.Style.Image != null)
                {
                    sf.DefaultDrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                    sf.DefaultDrawingOptions.Picture = c.Style.Image;
                }
                else
                {
                    sf.DefaultDrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                    sf.DefaultDrawingOptions.FillColor = utils.ColorByName(c.Style.PointCouleur);
                    sf.DefaultDrawingOptions.PointSize = c.Style.PointTaille;
                }

                sf.CollisionMode = tkCollisionMode.AllowCollisions;
            }
            else if (c.TypeShapefile == TypeShapefile.Ligne)
            {
                sf.CreateNewWithShapeID("", ShpfileType.SHP_POLYLINE);
                LinePattern pattern = new LinePattern();
                pattern.AddLine(utils.ColorByName(c.Style.LigneCouleur), c.Style.LigneTaille, c.Style.LigneStyle);
                sf.DefaultDrawingOptions.LinePattern = pattern;
                sf.DefaultDrawingOptions.UseLinePattern = true;
            }
            else if (c.TypeShapefile == TypeShapefile.Surface)
            {
                sf.CreateNewWithShapeID("", ShpfileType.SHP_POLYGON);
                sf.DefaultDrawingOptions.FillBgColor = utils.ColorByName(c.Style.SurfaceCouleur);
                sf.DefaultDrawingOptions.FillTransparency = c.Style.SurfaceTransparence;
            }
            
            c.IdLayer = axMap1.AddLayer(sf, true);
            c.Shapefile = sf;
        }

        public static void InitShapeFiles()
        {
            Utils utils = new Utils();

            //area
            Shapefile sf = new Shapefile();
            sf.CreateNewWithShapeID("", ShpfileType.SHP_POLYGON);
            //layerArea = axMap1.AddLayer(sf, true);
            AddCouche(sf, TypeShapefile.Surface);

            sf.DefaultDrawingOptions.FillBgColor = utils.ColorByName(tkMapColor.Blue);
            sf.DefaultDrawingOptions.FillTransparency = 100f;

            //line
            sf = new Shapefile();
            sf.CreateNewWithShapeID("", ShpfileType.SHP_POLYLINE);
            //layerLine = axMap1.AddLayer(sf, true);
            AddCouche(sf, TypeShapefile.Ligne);

            LinePattern pattern = new LinePattern();
            pattern.AddLine(utils.ColorByName(tkMapColor.Blue), 1.5f, tkDashStyle.dsSolid);

            sf.DefaultDrawingOptions.LinePattern = pattern;
            sf.DefaultDrawingOptions.UseLinePattern = true;

            //point
            sf = new Shapefile();
            sf.CreateNewWithShapeID("", ShpfileType.SHP_POINT);

            sf.StartEditingTable();
            sf.EditAddField("label", FieldType.STRING_FIELD, 0, 50);

            sf.Labels.FrameVisible = false;
            sf.Labels.FontSize = 8;
            sf.Labels.AvoidCollisions = false;

            //layerPoint = axMap1.AddLayer(sf, true);
            AddCouche(sf, TypeShapefile.Point);

            sf.DefaultDrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
            sf.DefaultDrawingOptions.FillColor = utils.ColorByName(tkMapColor.Blue);
            sf.DefaultDrawingOptions.PointSize = 8.0f;
            sf.CollisionMode = tkCollisionMode.AllowCollisions;
        }

        private static void AddCouche(Shapefile sf, TypeShapefile typeShapefile)
        {
            int idLayer = axMap1.AddLayer(sf, true);

            couches.Add(new Couche
            {
                IdLayer = idLayer,
                Shapefile = sf,
                Style = new StyleCouche { TypeShapefile = typeShapefile },
                TypeShapefile = typeShapefile
            });
        }

        public static void DrawPoint(EventCoord cPoint, string label, int layer)
        {
            Shapefile sf = axMap1.get_Shapefile(axMap1.get_LayerHandle(layer));
            Shape shp = new Shape();
            shp.Create(ShpfileType.SHP_POINT);

            double x = 0, y = 0;
            axMap1.DegreesToProj(cPoint.X, cPoint.Y, ref x, ref y);
            shp.AddPoint(x, y);

            sf.EditAddShape(shp);
            sf.EditCellValue(1, sf.NumShapes - 1, label);
        }

        public static void DrawLine(List<EventCoord> coordinates, int layer)
        {
            Shapefile sf = axMap1.get_Shapefile(axMap1.get_LayerHandle(layer));
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

        public static void DrawArea(List<EventCoord> coordinates, int layer)
        {
            Shapefile sf = axMap1.get_Shapefile(axMap1.get_LayerHandle(layer));
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
            for (int i = 0; i < couches.Count; i++)
            {
                if (couches[i].TypeShapefile == TypeShapefile.Point)
                {
                    Shapefile sf = axMap1.get_Shapefile(axMap1.get_LayerHandle(couches[i].IdLayer));

                    for (int j = 0; j < sf.NumShapes; j++)
                    {
                        string text = sf.Table.CellValue[1, j].ToString();
                        double x = sf.Shape[j].Center.x;
                        double y = sf.Shape[j].Center.y;

                        if (Settings.centrerLabel)
                        {
                            x = x - text.Length * 500;
                            y = y + 1500;
                        }

                        sf.Labels.AddLabel(text, x, y);
                    }
                    sf.Labels.Synchronized = true;
                }
            }
        }


        public static void SetBuffer()
        {
            Shapefile sfToBuff = new Shapefile();

            for (int i = 0; i < couches.Count; i++)
            {
                if (couches[i].TypeShapefile == TypeShapefile.Surface)
                {
                    int h = axMap1.get_LayerHandle(couches[i].IdLayer);
                    Shapefile sfArea = axMap1.get_Shapefile(h);

                    sfToBuff = MergeShapefiles(sfToBuff, sfArea);
                }
            }

            double distance = Settings.tailleBuffer; //metres
            Shapefile tmpBuffer = sfToBuff.BufferByDistance(distance, 30, false, true);

            tmpBuffer.DefaultDrawingOptions.LineWidth = 1.0f;
            tmpBuffer.DefaultDrawingOptions.LineColor = 16711680; //blue
            tmpBuffer.DefaultDrawingOptions.FillBgColor = 15128749; //lightblue
            tmpBuffer.DefaultDrawingOptions.FillTransparency = 100f;

            int idLayer = axMap1.AddLayer(tmpBuffer, true);

            //AddCouche(sfBuffer, TypeShapefile.Buffer);
            
        }

        private static Shapefile MergeShapefiles(Shapefile s1, Shapefile s2)
        {
            Shapefile result = new Shapefile();

            for (int i = 0; i < s1.NumShapes; i++)
                result.EditAddShape(s1.Shape[i]);

            for (int i = 0; i < s2.NumShapes; i++)
                result.EditAddShape(s2.Shape[i]);

            return result;
        }

        private static Shapefile MergeShapefiles(params Shapefile[] list)
        {
            Shapefile result = new Shapefile();

            for (int i = 0; i < list.Length; i++)
            {
                for (int j = 0; j < list[i].NumShapes; j++)
                    result.EditAddShape(list[i].Shape[j]);
            }

            return result;
        }

        /// <summary>
        /// Supprime tous les vecteurs affichés
        /// </summary>
        public static void ClearCarto(List<Couche> couches)
        {
            for (int i = 0; i < couches.Count; i++)
            {
                if (couches[i].IdLayer > -1)
                {
                    Shapefile sf = axMap1.get_Shapefile(axMap1.get_LayerHandle(couches[i].IdLayer));
                    sf.EditClear();
                }
            }

            //suppression de la couche buffer
            Couche c = couches[couches.Count - 1];

            if (c.TypeShapefile == TypeShapefile.Buffer)
            {
                axMap1.RemoveLayer(c.IdLayer);
                couches.Remove(c);
            }
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
            parentForm.SetTbMGRS(Util.ConvertLatLongToMGRS(currentX, currentY));
        }
    }
}