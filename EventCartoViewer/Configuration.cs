using System;
using System.Windows.Forms;

namespace EventCartoViewer
{
    public partial class Configuration : Form
    {
        public Configuration()
        {
            InitializeComponent();

            LoadSettings();
        }

        private void LoadSettings()
        {
            cb_triCoordSurface.Checked = Settings.triCoordSurface;
            cb_afficherLabel.Checked = Settings.afficherLabel;
            cb_centrerLabel.Checked = Settings.centrerLabel;
            cb_afficherBuffer.Checked = Settings.afficherBuffer;
            num_niveauZoomCentrer.Value = Settings.niveauZoomCentrer;
            num_tailleBuffer.Value = Settings.tailleBuffer;
        }

        private void b_saveConfig_Click(object sender, EventArgs e)
        {
            Settings.triCoordSurface = cb_triCoordSurface.Checked;
            Settings.afficherLabel = cb_afficherLabel.Checked;
            Settings.centrerLabel = cb_centrerLabel.Checked;
            Settings.afficherBuffer = cb_afficherBuffer.Checked;
            Settings.niveauZoomCentrer = (int)num_niveauZoomCentrer.Value;
            Settings.tailleBuffer = (int)num_tailleBuffer.Value;

            Settings.WriteConfigFile();
        }
    }
}
