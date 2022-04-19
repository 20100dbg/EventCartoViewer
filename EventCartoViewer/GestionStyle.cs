using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace EventCartoViewer
{
    public partial class GestionStyle : Form
    {
        
        StyleCouche sc = new StyleCouche();


        public GestionStyle()
        {
            InitializeComponent();

            for (int i = 0; i < Carto.styles.Count ; i++)
            {
                cb_selectionStyle.Items.Add(Carto.styles[i].Nom);
            }

            cb_styleLigne.Items.Add("Continu");
            cb_styleLigne.Items.Add("Tirets");
            cb_styleLigne.Items.Add("Pointillés");
            cb_styleLigne.SelectedIndex = 0;
        }

        private void rb_point_CheckedChanged(object sender, EventArgs e)
        {
            gb_ligne.Visible = gb_surface.Visible = false;
            gb_point.Visible = true;
        }

        private void rb_ligne_CheckedChanged(object sender, EventArgs e)
        {
            gb_point.Visible = gb_surface.Visible = false;
            gb_ligne.Visible = true;
        }

        private void rb_surface_CheckedChanged(object sender, EventArgs e)
        {
            gb_ligne.Visible = gb_point.Visible = false;
            gb_surface.Visible = true;
        }

        private void b_couleurPoint_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            int color = colorDialog1.Color.ToArgb();

            tb_couleurPoint.Text = color.ToString();
            tb_couleurPoint.ForeColor = colorDialog1.Color;
            tb_couleurPoint.BackColor = colorDialog1.Color;

            sc.PointCouleur = (MapWinGIS.tkMapColor)color;
        }

        private void b_couleurLigne_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            int color = colorDialog1.Color.ToArgb();

            tb_couleurLigne.Text = color.ToString();
            tb_couleurLigne.ForeColor = colorDialog1.Color;
            tb_couleurLigne.BackColor = colorDialog1.Color;

            sc.LigneCouleur = (MapWinGIS.tkMapColor)color;
        }

        private void b_couleurSurface_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            int color = colorDialog1.Color.ToArgb();

            tb_couleurSurface.Text = color.ToString();
            tb_couleurSurface.ForeColor = colorDialog1.Color;
            tb_couleurSurface.BackColor = colorDialog1.Color;

            sc.SurfaceCouleur = (MapWinGIS.tkMapColor)color;
        }

        private void b_imagePoint_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Application.StartupPath;
            ofd.ShowDialog();
            string filepath = ofd.FileName;

            if (string.IsNullOrEmpty(filepath) || !File.Exists(filepath)) return;
        }

        private void b_enregistrerStyle_Click(object sender, EventArgs e)
        {
            EnregistrerStyle();
        }

        private void EnregistrerStyle()
        {
            StyleCouche sc = new StyleCouche();

            if (rb_point.Checked) sc.TypeShapefile = TypeShapefile.Point;
            if (rb_ligne.Checked) sc.TypeShapefile = TypeShapefile.Ligne;
            if (rb_surface.Checked) sc.TypeShapefile = TypeShapefile.Surface;

            sc.Nom = tb_nomStyle.Text;

            sc.PointTaille = (float)num_taillePoint.Value;
            sc.PointCouleur = (MapWinGIS.tkMapColor)int.Parse(tb_couleurPoint.Text);
            sc.ImagePath = "";
            //if (File.Exists(sc.ImagePath)) sc.Image = Image.FromFile(sc.ImagePath);

            sc.LigneTaille = (float)num_tailleLigne.Value;
            sc.LigneCouleur = (MapWinGIS.tkMapColor)int.Parse(tb_couleurLigne.Text);
            sc.LigneStyle = (MapWinGIS.tkDashStyle)cb_styleLigne.SelectedIndex;

            sc.SurfaceCouleur = (MapWinGIS.tkMapColor)int.Parse(tb_couleurSurface.Text);
            sc.SurfaceTransparence = (float)num_transparenceSurface.Value;

            Carto.styles.Add(sc);
            ViderChamps();
        }

        private void cb_selectionStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            StyleCouche sc = (StyleCouche)cb_selectionStyle.SelectedItem;

            tb_nomStyle.Text = sc.Nom;

            num_taillePoint.Value = (decimal)sc.PointTaille;
            int c = (int)sc.PointCouleur;
            tb_couleurPoint.Text = c.ToString();
            tb_couleurPoint.ForeColor = Color.FromArgb(c);
            tb_couleurPoint.BackColor = Color.FromArgb(c);

            sc.ImagePath = "";
            //if (File.Exists(sc.ImagePath)) sc.Image = Image.FromFile(sc.ImagePath);

            num_tailleLigne.Value = (decimal)sc.LigneTaille;
            int c2 = (int)sc.LigneCouleur;
            tb_couleurLigne.Text = c2.ToString();
            cb_styleLigne.SelectedIndex = (int)sc.LigneStyle;

            int c3 = (int)sc.SurfaceCouleur;
            tb_couleurSurface.Text = c3.ToString();
            tb_couleurSurface.ForeColor = Color.FromArgb(c3);
            tb_couleurSurface.BackColor = Color.FromArgb(c3);
            num_transparenceSurface.Value = (decimal)sc.SurfaceTransparence;

        }

        private void b_nouveauStyle_Click(object sender, EventArgs e)
        {
            ViderChamps();
            tb_nomStyle.Text = "Nouveau";
        }

        private void b_supprimerStyle_Click(object sender, EventArgs e)
        {

        }

        private void ViderChamps()
        {
            tb_nomStyle.Clear();

            num_taillePoint.Value = 0;
            tb_couleurPoint.Clear();
            tb_couleurPoint.ForeColor = tb_couleurPoint.BackColor = SystemColors.Control;

            num_tailleLigne.Value = 0;
            tb_couleurLigne.Clear();
            tb_couleurLigne.ForeColor = tb_couleurLigne.BackColor = SystemColors.Control;

            num_transparenceSurface.Value = 0;
            tb_couleurSurface.Clear();
            tb_couleurSurface.ForeColor = tb_couleurSurface.BackColor = SystemColors.Control;
            
        }
    }
}
