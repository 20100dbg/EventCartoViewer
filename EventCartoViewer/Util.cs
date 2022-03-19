using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace EventCartoViewer
{
    public static class Util
    {
        public const int CoordHS = -99999;
        public static string DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        public static void WriteFile(string txt, string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding(1252)))
            {
                sw.Write(txt);
            }
        }

        public static string FixDecSeparator(string s)
        {
            string oldSeparator = (DecimalSeparator == ",") ? "." : ",";
            s = s.Replace(oldSeparator, DecimalSeparator);
            return s;
        }

        public static double DMStoDec(string dms)
        {
            Regex rgx = new Regex(@"[0-9\.,]{1,}");
            MatchCollection mc = rgx.Matches(dms);
            if (mc.Count != 3) return CoordHS;

            dms = dms.ToUpper();
            if (!dms.Contains("N") && !dms.Contains("S") && !dms.Contains("E") && !dms.Contains("W") && !dms.Contains("O"))
                return CoordHS;

            float degres = float.Parse(mc[0].Value);
            float minutes = float.Parse(mc[1].Value);
            float secondes = float.Parse(Util.FixDecSeparator(mc[2].Value));
            double tmp = degres + (minutes / 60) + (secondes / 3600);
            tmp = Math.Round(tmp, 6);
            if (dms.Contains("S") || dms.Contains("W") || dms.Contains("O")) tmp = tmp * -1;

            return tmp;
        }

        public static double CleanCoord(string coord)
        {
            double x;
            coord = coord.Trim();

            if (coord != "")
            {
                coord = FixDecSeparator(coord);
                if (double.TryParse(coord, out x)) return x;
                else
                {
                    return DMStoDec(coord);
                }
            }

            return CoordHS;
        }

        public static string ConvertLatLongToMGRS(double X, double Y)
        {
            Coord c = Wgs84toUTM(Y, X);
            string mgrs = UTM_MGRS(c.X, c.Y, c.Fuseau, c.Zone);
            return mgrs;
        }

        public static Coord ConvertMGRStoLatLong(string mgrs)
        {
            Coord c = MGRS_UTM(mgrs);
            c = UTMtoWgs84(c.Y, c.X, c.Fuseau);
            return c;
        }

        public static Coord Wgs84toUTM(Double LATITUDE, Double LONGITUDE)
        {
            Coord Wgs84toUTM = new Coord();

            Double lat, lng, lng0, T, c, vphi, Ac;
            double Zn;
            Double[] S = new double[5];
            Double[] lb = new double[4];

            lat = deg2rad(LATITUDE);
            lng = deg2rad(LONGITUDE);

            Double x1 = Math.Floor((LONGITUDE + 180) / 360);
            Double x2 = Math.Floor((LONGITUDE - x1 * 360 + 180) / 6);
            Zn = x2 + 1;

            //Zn = Int((LONGITUDE - Int((LONGITUDE + 180) / 360) * 360 + 180) / 6) + 1;

            lng0 = deg2rad(((Zn - 1) * 6) - 180 + 3);
            vphi = 1 / Math.Sqrt(1 - (Math.Pow(e, 2) * Math.Sin(lat) * Math.Sin(lat)));
            Ac = (lng - lng0) * Math.Cos(lat);


            S[1] = (1 - (Math.Pow(e, 2) / 4) - (3 * Math.Pow(e, 4) / 64) - (5 * Math.Pow(e, 6) / 256)) * lat;
            S[2] = ((3 * Math.Pow(e, 2) / 8) + (3 * Math.Pow(e, 4) / 32) + (45 * Math.Pow(e, 6) / 1024)) * Math.Sin(2 * lat);
            S[3] = ((15 * Math.Pow(e, 4) / 256) + (45 * Math.Pow(e, 6) / 1024)) * Math.Sin(4 * lat);
            S[4] = (35 * Math.Pow(e, 6) / 3072) * Math.Sin(6 * lat);
            S[0] = S[1] - S[2] + S[3] - S[4];
            T = Math.Pow(Math.Tan(lat), 2);
            c = (Math.Pow(e, 2) / (1 - Math.Pow(e, 2))) * Math.Pow(Math.Cos(lat), 2);


            lb[1] = Math.Pow(Ac, 2) / 2;
            lb[2] = (5 - T + (9 * c) + (4 * Math.Pow(c, 2))) * (Math.Pow(Ac, 4)) / 24;
            lb[3] = (61 - (58 * T) + (T * T)) * (Math.Pow(Ac, 6)) / 720;
            lb[0] = k0 * a * (S[0] + (vphi * Math.Tan(lat) * (lb[1] + lb[2] + lb[3])));


            Wgs84toUTM.Y = lb[0] / 1000 + ((lat < 0) ? 10000 : 0);
            Wgs84toUTM.X = 500 + (k0 * a / 1000 * vphi * (Ac + (1 - T + c) * Math.Pow(Ac, 3) / 6 +
                             (5 - (18 * T) + Math.Pow(T, 2)) * Math.Pow(Ac, 5) / 120));


            Wgs84toUTM.Y = Wgs84toUTM.Y * 1000;
            Wgs84toUTM.X = Wgs84toUTM.X * 1000;

            Wgs84toUTM.Zone = Utm_Letter(LATITUDE);
            Wgs84toUTM.Fuseau = (int)Zn;
            return Wgs84toUTM;
        }

        public static String Utm_Letter(Double lat)
        {
            string s = "";
            if (lat >= 72 && lat <= 84) s = "X";
            else if (lat >= 64 && lat <= 72) s = "W";
            else if (lat >= 56 && lat <= 64) s = "V";
            else if (lat >= 48 && lat <= 56) s = "U";
            else if (lat >= 40 && lat <= 48) s = "T";
            else if (lat >= 32 && lat <= 40) s = "S";
            else if (lat >= 24 && lat <= 32) s = "R";
            else if (lat >= 16 && lat <= 24) s = "Q";
            else if (lat >= 8 && lat <= 16) s = "P";
            else if (lat >= 0 && lat <= 8) s = "N";
            else if (lat >= -8 && lat <= 0) s = "M";
            else if (lat >= -16 && lat <= -8) s = "L";
            else if (lat >= -24 && lat <= -16) s = "K";
            else if (lat >= -32 && lat <= -24) s = "J";
            else if (lat >= -40 && lat <= -32) s = "H";
            else if (lat >= -48 && lat <= -40) s = "G";
            else if (lat >= -56 && lat <= -48) s = "F";
            else if (lat >= -64 && lat <= -56) s = "E";
            else if (lat >= -72 && lat <= -64) s = "D";
            else if (lat >= -80 && lat <= -72) s = "C";
            else s = "";
            return s;
        }


        public static double PI = 3.14159265358979;
        public static double a = 6378137;//            ' demi grand axe de l'ellipsoide(m)
        public static double e = 0.08181919106;    //  ' première excentricité de l'ellipsoide
        public static double k0 = 0.9996;

        public static double x0 = 700000;   //         ' coordonnées à l'origine
        public static double y0 = 6600000;    //       ' coordonnées à l'origine

        public static Coord MGRS_UTM(string mgrs)
        {
            Coord c = new Coord();
            c.Fuseau = MGRS_FusUTM(mgrs);
            c.Zone = MGRS_ZonUTM(mgrs);
            c.Y = MGRS_UTMNorthing(mgrs) / 1000;
            c.X = MGRS_UTMEasting(mgrs) / 1000;

            return c;
        }

        public static string UTM_MGRS(double UtmX, double UtmY, int UtmFuseau, string UtmZone, int Prec = -1, string CarSep = "")
        {
            string Sep, res;
            double L1, L2, Offset;
            double X, Y, Fact;

            if (Prec == -1) Prec = 5;
            else
            {
                if (Prec > 5) Prec = 5;
                if (Prec < 1) Prec = 1;
            }
            double tmp = Math.Pow(10, 5 - Prec);
            Fact = tmp * 0.5;
            if (CarSep == "") Sep = " ";
            else Sep = CarSep.Substring(0, 1);

            res = UtmFuseau.ToString().PadLeft(2, '0') + Sep + UtmZone + Sep;
            //res = Format(UtmFuseau, "00") & Sep & UtmZone & Sep

            L1 = ((UtmFuseau - 1) % 3) * 8 + Math.Floor(UtmX / 100000);

            if (UtmFuseau % 2 == 0) Offset = 5;
            else Offset = 0;

            L2 = ((Offset + Math.Floor(UtmY / 100000)) % 20) + 1;

            res = res + "ABCDEFGHJKLMNPQRSTUVWXYZ".Substring((int)L1 - 1, 1) + "ABCDEFGHJKLMNPQRSTUV".Substring((int)L2 - 1, 1) + Sep;
            X = UtmX - Math.Floor(UtmX / 100000) * 100000;
            X = X + Fact;
            Y = UtmY - Math.Floor(UtmY / 100000) * 100000;
            Y = Y + Fact;

            string tmpX = ((int)X).ToString();
            tmpX = tmpX.PadLeft(5, '0').Substring(0, Prec);
            string tmpY = ((int)Y).ToString();
            tmpY = tmpY.PadLeft(5, '0').Substring(0, Prec);

            return res + tmpX + Sep + tmpY;
        }

        static int MGRS_FusUTM(string mgrs)
        {
            string VarIn = "";
            char c;

            for (int i = 0; i < mgrs.Length; i++)
            {
                c = mgrs.Substring(i, 1)[0];
                if (c >= (int)'A' && c <= (int)'Z') VarIn = VarIn + c;
                if (c >= (int)'0' && c <= (int)'9') VarIn = VarIn + c;
            }

            return int.Parse(VarIn.Substring(0, 2));
        }

        static string MGRS_ZonUTM(string mgrs)
        {
            string VarIn = "";
            char c;

            for (int i = 0; i < mgrs.Length; i++)
            {
                c = mgrs.Substring(i, 1)[0];
                if (c >= (int)'A' && c <= (int)'Z') VarIn = VarIn + c;
                if (c >= (int)'0' && c <= (int)'9') VarIn = VarIn + c;
            }

            return VarIn.Substring(2, 1);
        }

        static double MGRS_UTMEasting(string mgrs)
        {
            string L1, XY;
            double X;
            string VarIn = "";
            char c;

            for (int i = 0; i < mgrs.Length; i++)
            {
                c = mgrs.Substring(i, 1)[0];
                if (c >= (int)'A' && c <= (int)'Z') VarIn = VarIn + c;
                if (c >= (int)'0' && c <= (int)'9') VarIn = VarIn + c;
            }


            L1 = VarIn.Substring(3, 1);
            XY = VarIn.Substring(5);
            int l = XY.Length / 2;
            X = int.Parse(XY.Substring(0, l));
            int OffX = "ABCDEFGHJKLMNPQRSTUVWXYZ".IndexOf(L1); //retirer - 1
            OffX = (OffX % 8) + 1;
            OffX = OffX * 100000;
            return OffX + X;
        }

        static double MGRS_UTMNorthing(string mgrs)
        {
            int l, j, RUtm;
            string VarIn, L2, XY;
            double Y, FloorUtm;
            VarIn = "";
            char c;
            char ZonUtm;
            double OffY;

            for (int i = 0; i < mgrs.Length; i++)
            {
                c = mgrs.Substring(i, 1)[0];
                if (c >= (int)'A' && c <= (int)'Z') VarIn = VarIn + c;
                if (c >= (int)'0' && c <= (int)'9') VarIn = VarIn + c;
            }

            L2 = VarIn.Substring(4, 1);
            ZonUtm = VarIn.Substring(2, 1)[0];
            XY = VarIn.Substring(5);
            l = XY.Length / 2;
            Y = int.Parse(XY.Substring(XY.Length - l));

            OffY = "ABCDEFGHJKLMNPQRSTUV".IndexOf(L2); //retirer - 1
            int k = int.Parse(VarIn.Substring(0, 2));
            j = (k / 2);
            if (k == 2 * j)
                OffY = OffY - 5;

            OffY = OffY * 100000;
            OffY = OffY + Y;

            if (ZonUtm >= (int)'N')
            {
                RUtm = "ABCDEFGHJKLMNPQRSTUVWXYZ".IndexOf(ZonUtm) - 13 + 1; //ajouter + 1
                if (RUtm > 10) RUtm = 10;
                FloorUtm = 110500 * 8 * RUtm;

                while (OffY < FloorUtm) OffY = OffY + 2000000;
            }
            else
            {
                RUtm = 13 - "ABCDEFGHJKLMNPQRSTUVWXYZ".IndexOf(ZonUtm) + 1;//ajouter + 1
                FloorUtm = 110500 * (90 - (RUtm * 8));

                while (OffY < FloorUtm) OffY = OffY + 2000000;
            }

            return OffY;
        }

        public static string Prettify(string mgrs)
        {
            return mgrs.Substring(0, 2) + " " + mgrs.Substring(3, 3) + mgrs.Substring(6, 5) + mgrs.Substring(mgrs.Length - 5);
        }

        public static Coord UTMtoWgs84(double LATITUDE, double LONGITUDE, int Fuseau, string Ltr = "")
        {
            Coord UTMtoWgs84 = new Coord();


            double X, Y, ecctrty, eccPS;
            double mu, e1, phi1rad;
            double N1, T1, C1, R1, D;
            double lng, lat;


            X = (LONGITUDE * 1000) - 500000;
            Y = (LATITUDE * 1000);
            if (Ltr != "") Y = Y - ((((int)Ltr[0]) < 78) ? 10000000 : 0);

            ecctrty = Math.Pow(e, 2);
            eccPS = ecctrty / (1 - ecctrty);

            mu = Y / k0 / (a * (1 - ecctrty / 4 - 3 * Math.Pow(ecctrty, 2) / 64 - 5 * Math.Pow(ecctrty, 3) / 256));
            e1 = (1 - Math.Sqrt(1 - ecctrty)) / (1 + Math.Sqrt(1 - ecctrty));
            phi1rad = mu + (3 * e1 / 2 - 27 * Math.Pow(e1, 3) / 32) * Math.Sin(2 * mu) + (21 * Math.Pow(e1, 2)
                      / 16 - 55 * Math.Pow(e1, 4) / 32) * Math.Sin(4 * mu) + (151 * Math.Pow(e1, 3) / 96) * Math.Sin(6 * mu);
            N1 = a / Math.Sqrt(1 - ecctrty * Math.Sin(phi1rad) * Math.Sin(phi1rad));
            T1 = Math.Pow(Math.Tan(phi1rad), 2);
            C1 = ecctrty * Math.Pow(Math.Cos(phi1rad), 2);
            R1 = a * (1 - ecctrty) / (Math.Pow(1 - ecctrty * Math.Pow(Math.Sin(phi1rad), 2), 1.5));
            D = X / (N1 * k0);

            lat = phi1rad - (N1 * Math.Tan(phi1rad) / R1) * (Math.Pow(D, 2) / 2 - (5 + 3 * T1 + 10 * C1 - 4
                 * Math.Pow(C1, 2) - 9 * eccPS) * Math.Pow(D, 4) / 24 + (61 + 90 * T1 + 298 * C1 + 45
                 * Math.Pow(T1, 2) - 252 * eccPS - 3 * Math.Pow(C1, 2)) * Math.Pow(D, 6) / 720);

            lng = (D - (1 + 2 * T1 + C1) * Math.Pow(D, 3) / 6 + (5 - 2 * C1 + 28 * T1 - 3
                  * Math.Pow(C1, 2) + 8 * eccPS + 24 * Math.Pow(T1, 2)) * Math.Pow(D, 5) / 120) / Math.Cos(phi1rad);

            UTMtoWgs84.Y = rad2deg(lat);
            UTMtoWgs84.X = rad2deg(lng) + (Fuseau - 1) * 6 - 180 + 3;


            return UTMtoWgs84;
        }

        public static double deg2rad(double deg)
        {
            return deg * Math.PI / 180;
        }

        public static double rad2deg(double rad)
        {
            return rad / Math.PI * 180;
        }

        public static double ATanH(double value)
        {
            return Math.Log((1 / value + 1) / (1 / value - 1)) / 2;
        }

        public static string DecToDMS(double val, bool isLatitude, bool formatBR = false)
        {
            string cardinal;
            if (isLatitude) cardinal = (val < 0) ? "S" : "N";
            else cardinal = (val < 0) ? "W" : "E";

            double deg = ((val < 0) ? Math.Ceiling(val) : Math.Floor(val));
            double tmpmin = (val - deg) * 60;
            double min = ((tmpmin < 0) ? Math.Ceiling(tmpmin) : Math.Floor(tmpmin));
            double sec = Math.Round((tmpmin - min) * 60, 4);

            string sdeg = Math.Abs(deg).ToString();
            string smin = Math.Abs(min).ToString();
            string ssec = Math.Floor(Math.Abs(sec)).ToString();

            if (!formatBR) return sdeg + "° " + smin + "' " + ssec + "\" " + cardinal;
            else return cardinal + "-" + sdeg.PadLeft(((isLatitude) ? 2 : 3), '0') + "-" + smin.PadLeft(2, '0') + "-" + ssec.PadLeft(2, '0');
        }

        public static string GetCoordBR(double y, double x, int precision, bool cardinalBefore = true, bool longitudeBefore = true)
        {
            string latitude = Util.DecToDMS(y, true, true);
            string longitude = Util.DecToDMS(x, false, true);

            string[] lat = latitude.Split('-');
            string[] lng = longitude.Split('-');

            string coordLat = "", coordLng = "";
            for (int j = 1; j < precision + 1; j++) coordLat += lat[j];
            for (int j = 1; j < precision + 1; j++) coordLng += lng[j];

            if (cardinalBefore)
            {
                coordLat = lat[0] + coordLat;
                coordLng = lng[0] + coordLng;
            }
            else
            {
                coordLat = coordLat + lat[0];
                coordLng = coordLng + lng[0];
            }

            if (longitudeBefore) return coordLng + coordLat;
            else return coordLat + coordLng;
        }

    }
}
