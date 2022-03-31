using System;
using System.Collections.Generic;
using System.IO;

namespace EventCartoViewer
{
    public static class Settings
    {
        static string ConFile = "EventCartoViewer.config.txt";
        public static string version = "0.0.2";
        public static string dateVersion = "31/03/2022";
        
        public static List<string> cartes;
        public static int niveauZoomCentrer;
        public static bool triCoordSurface;
        public static bool afficherLabel;
        public static bool centrerLabel;
        public static bool afficherBuffer;
        public static int tailleBuffer;

        public static void LoadDefaultValues()
        {
            cartes = new List<string>();
            afficherLabel = false;
            niveauZoomCentrer = 13;
            tailleBuffer = 800;
            triCoordSurface = true;
            afficherBuffer = true;
            centrerLabel = true;
        }

        public static void ResetSettings()
        {
            File.Move(ConFile, ConFile + "_backup_" + DateTime.Now.ToString("yyyyMMdd_HHmmss.f"));
            LoadDefaultValues();
            WriteConfigFile();
        }

        public static bool ReadConfigFile()
        {
            if (!File.Exists(ConFile)) return false;
            string conStr = File.ReadAllText(ConFile);

            string[] tab = conStr.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i = 0; i < tab.Length; i++)
            {
                string[] tab2 = tab[i].Split(new char[] { '=' });
                
                if (tab2[0] == "cartes")
                {
                    string[] x = tab2[1].Split('|');
                    Settings.cartes.AddRange(x);
                }
                else if (tab2[0] == "niveauZoomCentrer") Settings.niveauZoomCentrer = int.Parse(tab2[1]);
                else if (tab2[0] == "triCoordSurface") Settings.triCoordSurface = (tab2[1].ToLower() == "true");
                else if (tab2[0] == "afficherBuffer") Settings.afficherBuffer = (tab2[1].ToLower() == "true");
                else if (tab2[0] == "tailleBuffer") Settings.tailleBuffer = int.Parse(tab2[1]);
                else if (tab2[0] == "afficherLabel") Settings.afficherLabel = (tab2[1].ToLower() == "true");
                else if (tab2[0] == "centrerLabel") Settings.centrerLabel = (tab2[1].ToLower() == "true");
            }

            return true;
        }

        public static void WriteConfigFile()
        {
            string s = "" +
            "cartes=" + String.Join("|", Settings.cartes) + Environment.NewLine +
            "niveauZoomCentrer=" + Settings.niveauZoomCentrer + Environment.NewLine +
            "triCoordSurface=" + Settings.triCoordSurface + Environment.NewLine +
            "afficherBuffer=" + Settings.afficherBuffer + Environment.NewLine +
            "tailleBuffer=" + Settings.tailleBuffer + Environment.NewLine +
            "afficherLabel=" + Settings.afficherLabel + Environment.NewLine +
            "centrerLabel=" + Settings.centrerLabel + Environment.NewLine;

            Util.WriteFile(s, ConFile);
        }
    }
}