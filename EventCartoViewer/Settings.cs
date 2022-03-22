using System;
using System.Collections.Generic;
using System.IO;

namespace EventCartoViewer
{
    public static class Settings
    {
        static string ConFile = "EventCartoViewer.config.txt";
        public static string version = "0.0.1";
        public static string dateVersion = "21/03/2022";
        public static List<string> cartes;
        public static bool afficherLabel;
        public static int tailleBuffer;
        public static int niveauZoomCentrer;

        public static void LoadDefaultValues()
        {
            cartes = new List<string>();
            afficherLabel = false;
            niveauZoomCentrer = 13;
            tailleBuffer = 1;
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
                if (tab2[0] == "afficherLabel") Settings.afficherLabel = (tab2[1].ToLower() == "true");
                else if (tab2[0] == "cartes")
                {
                    string[] x = tab2[1].Split('|');
                    Settings.cartes.AddRange(x);
                }
                else if (tab2[0] == "niveauZoomCentrer") Settings.niveauZoomCentrer = int.Parse(tab2[1]);
                else if (tab2[0] == "tailleBuffer") Settings.tailleBuffer = int.Parse(tab2[1]);
            }

            return true;
        }

        public static void WriteConfigFile()
        {
            string s = "" +
            "afficherLabel=" + Settings.afficherLabel + Environment.NewLine +
            "cartes=" + String.Join("|", Settings.cartes) + Environment.NewLine +
            "niveauZoomCentrer=" + Settings.niveauZoomCentrer + Environment.NewLine +
            "tailleBuffer=" + Settings.tailleBuffer + Environment.NewLine;

            Util.WriteFile(s, ConFile);
        }
    }
}