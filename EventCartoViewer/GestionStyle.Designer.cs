namespace EventCartoViewer
{
    partial class GestionStyle
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tb_nomStyle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rb_point = new System.Windows.Forms.RadioButton();
            this.rb_ligne = new System.Windows.Forms.RadioButton();
            this.rb_surface = new System.Windows.Forms.RadioButton();
            this.num_taillePoint = new System.Windows.Forms.NumericUpDown();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.b_couleurPoint = new System.Windows.Forms.Button();
            this.gb_point = new System.Windows.Forms.GroupBox();
            this.b_imagePoint = new System.Windows.Forms.Button();
            this.gb_ligne = new System.Windows.Forms.GroupBox();
            this.num_tailleLigne = new System.Windows.Forms.NumericUpDown();
            this.b_couleurLigne = new System.Windows.Forms.Button();
            this.cb_styleLigne = new System.Windows.Forms.ComboBox();
            this.gb_surface = new System.Windows.Forms.GroupBox();
            this.num_transparenceSurface = new System.Windows.Forms.NumericUpDown();
            this.b_couleurSurface = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_couleurPoint = new System.Windows.Forms.TextBox();
            this.tb_couleurLigne = new System.Windows.Forms.TextBox();
            this.tb_couleurSurface = new System.Windows.Forms.TextBox();
            this.cb_selectionStyle = new System.Windows.Forms.ComboBox();
            this.b_nouveauStyle = new System.Windows.Forms.Button();
            this.b_supprimerStyle = new System.Windows.Forms.Button();
            this.b_enregistrerStyle = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.num_taillePoint)).BeginInit();
            this.gb_point.SuspendLayout();
            this.gb_ligne.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_tailleLigne)).BeginInit();
            this.gb_surface.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_transparenceSurface)).BeginInit();
            this.SuspendLayout();
            // 
            // tb_nomStyle
            // 
            this.tb_nomStyle.Location = new System.Drawing.Point(71, 52);
            this.tb_nomStyle.Name = "tb_nomStyle";
            this.tb_nomStyle.Size = new System.Drawing.Size(100, 20);
            this.tb_nomStyle.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Nom";
            // 
            // rb_point
            // 
            this.rb_point.AutoSize = true;
            this.rb_point.Location = new System.Drawing.Point(56, 93);
            this.rb_point.Name = "rb_point";
            this.rb_point.Size = new System.Drawing.Size(49, 17);
            this.rb_point.TabIndex = 2;
            this.rb_point.TabStop = true;
            this.rb_point.Text = "Point";
            this.rb_point.UseVisualStyleBackColor = true;
            this.rb_point.CheckedChanged += new System.EventHandler(this.rb_point_CheckedChanged);
            // 
            // rb_ligne
            // 
            this.rb_ligne.AutoSize = true;
            this.rb_ligne.Location = new System.Drawing.Point(219, 93);
            this.rb_ligne.Name = "rb_ligne";
            this.rb_ligne.Size = new System.Drawing.Size(51, 17);
            this.rb_ligne.TabIndex = 3;
            this.rb_ligne.TabStop = true;
            this.rb_ligne.Text = "Ligne";
            this.rb_ligne.UseVisualStyleBackColor = true;
            this.rb_ligne.CheckedChanged += new System.EventHandler(this.rb_ligne_CheckedChanged);
            // 
            // rb_surface
            // 
            this.rb_surface.AutoSize = true;
            this.rb_surface.Location = new System.Drawing.Point(445, 93);
            this.rb_surface.Name = "rb_surface";
            this.rb_surface.Size = new System.Drawing.Size(62, 17);
            this.rb_surface.TabIndex = 4;
            this.rb_surface.TabStop = true;
            this.rb_surface.Text = "Surface";
            this.rb_surface.UseVisualStyleBackColor = true;
            this.rb_surface.CheckedChanged += new System.EventHandler(this.rb_surface_CheckedChanged);
            // 
            // num_taillePoint
            // 
            this.num_taillePoint.Location = new System.Drawing.Point(44, 23);
            this.num_taillePoint.Name = "num_taillePoint";
            this.num_taillePoint.Size = new System.Drawing.Size(52, 20);
            this.num_taillePoint.TabIndex = 5;
            // 
            // b_couleurPoint
            // 
            this.b_couleurPoint.Location = new System.Drawing.Point(44, 49);
            this.b_couleurPoint.Name = "b_couleurPoint";
            this.b_couleurPoint.Size = new System.Drawing.Size(75, 23);
            this.b_couleurPoint.TabIndex = 6;
            this.b_couleurPoint.Text = "Couleur";
            this.b_couleurPoint.UseVisualStyleBackColor = true;
            this.b_couleurPoint.Click += new System.EventHandler(this.b_couleurPoint_Click);
            // 
            // gb_point
            // 
            this.gb_point.Controls.Add(this.tb_couleurPoint);
            this.gb_point.Controls.Add(this.label3);
            this.gb_point.Controls.Add(this.b_imagePoint);
            this.gb_point.Controls.Add(this.num_taillePoint);
            this.gb_point.Controls.Add(this.b_couleurPoint);
            this.gb_point.Location = new System.Drawing.Point(12, 116);
            this.gb_point.Name = "gb_point";
            this.gb_point.Size = new System.Drawing.Size(146, 112);
            this.gb_point.TabIndex = 7;
            this.gb_point.TabStop = false;
            this.gb_point.Text = "Point";
            // 
            // b_imagePoint
            // 
            this.b_imagePoint.Location = new System.Drawing.Point(44, 78);
            this.b_imagePoint.Name = "b_imagePoint";
            this.b_imagePoint.Size = new System.Drawing.Size(75, 23);
            this.b_imagePoint.TabIndex = 8;
            this.b_imagePoint.Text = "Image";
            this.b_imagePoint.UseVisualStyleBackColor = true;
            this.b_imagePoint.Click += new System.EventHandler(this.b_imagePoint_Click);
            // 
            // gb_ligne
            // 
            this.gb_ligne.Controls.Add(this.tb_couleurLigne);
            this.gb_ligne.Controls.Add(this.label4);
            this.gb_ligne.Controls.Add(this.cb_styleLigne);
            this.gb_ligne.Controls.Add(this.num_tailleLigne);
            this.gb_ligne.Controls.Add(this.b_couleurLigne);
            this.gb_ligne.Location = new System.Drawing.Point(191, 116);
            this.gb_ligne.Name = "gb_ligne";
            this.gb_ligne.Size = new System.Drawing.Size(141, 112);
            this.gb_ligne.TabIndex = 9;
            this.gb_ligne.TabStop = false;
            this.gb_ligne.Text = "Ligne";
            // 
            // num_tailleLigne
            // 
            this.num_tailleLigne.Location = new System.Drawing.Point(44, 23);
            this.num_tailleLigne.Name = "num_tailleLigne";
            this.num_tailleLigne.Size = new System.Drawing.Size(52, 20);
            this.num_tailleLigne.TabIndex = 5;
            // 
            // b_couleurLigne
            // 
            this.b_couleurLigne.Location = new System.Drawing.Point(44, 51);
            this.b_couleurLigne.Name = "b_couleurLigne";
            this.b_couleurLigne.Size = new System.Drawing.Size(75, 23);
            this.b_couleurLigne.TabIndex = 6;
            this.b_couleurLigne.Text = "Couleur";
            this.b_couleurLigne.UseVisualStyleBackColor = true;
            this.b_couleurLigne.Click += new System.EventHandler(this.b_couleurLigne_Click);
            // 
            // cb_styleLigne
            // 
            this.cb_styleLigne.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_styleLigne.FormattingEnabled = true;
            this.cb_styleLigne.Location = new System.Drawing.Point(44, 80);
            this.cb_styleLigne.Name = "cb_styleLigne";
            this.cb_styleLigne.Size = new System.Drawing.Size(75, 21);
            this.cb_styleLigne.TabIndex = 7;
            // 
            // gb_surface
            // 
            this.gb_surface.Controls.Add(this.tb_couleurSurface);
            this.gb_surface.Controls.Add(this.label2);
            this.gb_surface.Controls.Add(this.num_transparenceSurface);
            this.gb_surface.Controls.Add(this.b_couleurSurface);
            this.gb_surface.Location = new System.Drawing.Point(359, 116);
            this.gb_surface.Name = "gb_surface";
            this.gb_surface.Size = new System.Drawing.Size(185, 82);
            this.gb_surface.TabIndex = 10;
            this.gb_surface.TabStop = false;
            this.gb_surface.Text = "Surface";
            // 
            // num_transparenceSurface
            // 
            this.num_transparenceSurface.Location = new System.Drawing.Point(86, 48);
            this.num_transparenceSurface.Name = "num_transparenceSurface";
            this.num_transparenceSurface.Size = new System.Drawing.Size(52, 20);
            this.num_transparenceSurface.TabIndex = 5;
            // 
            // b_couleurSurface
            // 
            this.b_couleurSurface.Location = new System.Drawing.Point(86, 19);
            this.b_couleurSurface.Name = "b_couleurSurface";
            this.b_couleurSurface.Size = new System.Drawing.Size(75, 23);
            this.b_couleurSurface.TabIndex = 6;
            this.b_couleurSurface.Text = "Couleur";
            this.b_couleurSurface.UseVisualStyleBackColor = true;
            this.b_couleurSurface.Click += new System.EventHandler(this.b_couleurSurface_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Transparence";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Taille";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Taille";
            // 
            // tb_couleurPoint
            // 
            this.tb_couleurPoint.Location = new System.Drawing.Point(5, 52);
            this.tb_couleurPoint.Name = "tb_couleurPoint";
            this.tb_couleurPoint.ReadOnly = true;
            this.tb_couleurPoint.Size = new System.Drawing.Size(29, 20);
            this.tb_couleurPoint.TabIndex = 11;
            // 
            // tb_couleurLigne
            // 
            this.tb_couleurLigne.Location = new System.Drawing.Point(9, 54);
            this.tb_couleurLigne.Name = "tb_couleurLigne";
            this.tb_couleurLigne.ReadOnly = true;
            this.tb_couleurLigne.Size = new System.Drawing.Size(29, 20);
            this.tb_couleurLigne.TabIndex = 13;
            // 
            // tb_couleurSurface
            // 
            this.tb_couleurSurface.Location = new System.Drawing.Point(9, 21);
            this.tb_couleurSurface.Name = "tb_couleurSurface";
            this.tb_couleurSurface.ReadOnly = true;
            this.tb_couleurSurface.Size = new System.Drawing.Size(29, 20);
            this.tb_couleurSurface.TabIndex = 14;
            // 
            // cb_selectionStyle
            // 
            this.cb_selectionStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_selectionStyle.FormattingEnabled = true;
            this.cb_selectionStyle.Location = new System.Drawing.Point(71, 12);
            this.cb_selectionStyle.Name = "cb_selectionStyle";
            this.cb_selectionStyle.Size = new System.Drawing.Size(123, 21);
            this.cb_selectionStyle.TabIndex = 14;
            this.cb_selectionStyle.SelectedIndexChanged += new System.EventHandler(this.cb_selectionStyle_SelectedIndexChanged);
            // 
            // b_nouveauStyle
            // 
            this.b_nouveauStyle.Location = new System.Drawing.Point(219, 12);
            this.b_nouveauStyle.Name = "b_nouveauStyle";
            this.b_nouveauStyle.Size = new System.Drawing.Size(75, 23);
            this.b_nouveauStyle.TabIndex = 14;
            this.b_nouveauStyle.Text = "Nouveau";
            this.b_nouveauStyle.UseVisualStyleBackColor = true;
            this.b_nouveauStyle.Click += new System.EventHandler(this.b_nouveauStyle_Click);
            // 
            // b_supprimerStyle
            // 
            this.b_supprimerStyle.Location = new System.Drawing.Point(312, 12);
            this.b_supprimerStyle.Name = "b_supprimerStyle";
            this.b_supprimerStyle.Size = new System.Drawing.Size(75, 23);
            this.b_supprimerStyle.TabIndex = 15;
            this.b_supprimerStyle.Text = "Supprimer";
            this.b_supprimerStyle.UseVisualStyleBackColor = true;
            this.b_supprimerStyle.Click += new System.EventHandler(this.b_supprimerStyle_Click);
            // 
            // b_enregistrerStyle
            // 
            this.b_enregistrerStyle.Location = new System.Drawing.Point(469, 246);
            this.b_enregistrerStyle.Name = "b_enregistrerStyle";
            this.b_enregistrerStyle.Size = new System.Drawing.Size(75, 23);
            this.b_enregistrerStyle.TabIndex = 16;
            this.b_enregistrerStyle.Text = "Enregistrer";
            this.b_enregistrerStyle.UseVisualStyleBackColor = true;
            this.b_enregistrerStyle.Click += new System.EventHandler(this.b_enregistrerStyle_Click);
            // 
            // GestionStyle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 305);
            this.Controls.Add(this.b_enregistrerStyle);
            this.Controls.Add(this.b_supprimerStyle);
            this.Controls.Add(this.b_nouveauStyle);
            this.Controls.Add(this.cb_selectionStyle);
            this.Controls.Add(this.gb_surface);
            this.Controls.Add(this.gb_ligne);
            this.Controls.Add(this.gb_point);
            this.Controls.Add(this.rb_surface);
            this.Controls.Add(this.rb_ligne);
            this.Controls.Add(this.rb_point);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_nomStyle);
            this.Name = "GestionStyle";
            this.Text = "GestionStyle";
            ((System.ComponentModel.ISupportInitialize)(this.num_taillePoint)).EndInit();
            this.gb_point.ResumeLayout(false);
            this.gb_point.PerformLayout();
            this.gb_ligne.ResumeLayout(false);
            this.gb_ligne.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_tailleLigne)).EndInit();
            this.gb_surface.ResumeLayout(false);
            this.gb_surface.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_transparenceSurface)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_nomStyle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rb_point;
        private System.Windows.Forms.RadioButton rb_ligne;
        private System.Windows.Forms.RadioButton rb_surface;
        private System.Windows.Forms.NumericUpDown num_taillePoint;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button b_couleurPoint;
        private System.Windows.Forms.GroupBox gb_point;
        private System.Windows.Forms.Button b_imagePoint;
        private System.Windows.Forms.GroupBox gb_ligne;
        private System.Windows.Forms.NumericUpDown num_tailleLigne;
        private System.Windows.Forms.Button b_couleurLigne;
        private System.Windows.Forms.ComboBox cb_styleLigne;
        private System.Windows.Forms.GroupBox gb_surface;
        private System.Windows.Forms.NumericUpDown num_transparenceSurface;
        private System.Windows.Forms.Button b_couleurSurface;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_couleurPoint;
        private System.Windows.Forms.TextBox tb_couleurLigne;
        private System.Windows.Forms.TextBox tb_couleurSurface;
        private System.Windows.Forms.ComboBox cb_selectionStyle;
        private System.Windows.Forms.Button b_nouveauStyle;
        private System.Windows.Forms.Button b_supprimerStyle;
        private System.Windows.Forms.Button b_enregistrerStyle;
    }
}