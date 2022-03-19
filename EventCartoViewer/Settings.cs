using System;
using System.IO;

namespace EventCartoViewer
{
    public static class Settings
    {
        static string ConFile = "EventCartoViewer.config.txt";
        public static string version = "0.0.1";
        public static string dateVersion = "19/03/2022";
        public static string test = "";


        public static bool TexteComsExactValues { get; set; }

        public static void LoadDefaultValues()
        {
            test = "";

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
                if (tab2[0] == "test") Settings.test = tab2[1];
            }

            return true;
        }

        public static void WriteConfigFile()
        {
            string s = "" +
            "test=" + Settings.test + Environment.NewLine;

            Util.WriteFile(s, ConFile);
        }
    }
}