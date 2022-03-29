namespace EventCartoViewer
{
    partial class Configuration
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
            this.cb_triCoordSurface = new System.Windows.Forms.CheckBox();
            this.num_niveauZoomCentrer = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_afficherLabel = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.num_tailleBuffer = new System.Windows.Forms.NumericUpDown();
            this.cb_centrerLabel = new System.Windows.Forms.CheckBox();
            this.cb_afficherBuffer = new System.Windows.Forms.CheckBox();
            this.b_saveConfig = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.num_niveauZoomCentrer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_tailleBuffer)).BeginInit();
            this.SuspendLayout();
            // 
            // cb_triCoordSurface
            // 
            this.cb_triCoordSurface.AutoSize = true;
            this.cb_triCoordSurface.Location = new System.Drawing.Point(42, 23);
            this.cb_triCoordSurface.Name = "cb_triCoordSurface";
            this.cb_triCoordSurface.Size = new System.Drawing.Size(99, 17);
            this.cb_triCoordSurface.TabIndex = 0;
            this.cb_triCoordSurface.Text = "triCoordSurface";
            this.cb_triCoordSurface.UseVisualStyleBackColor = true;
            // 
            // num_niveauZoomCentrer
            // 
            this.num_niveauZoomCentrer.Location = new System.Drawing.Point(42, 219);
            this.num_niveauZoomCentrer.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.num_niveauZoomCentrer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_niveauZoomCentrer.Name = "num_niveauZoomCentrer";
            this.num_niveauZoomCentrer.Size = new System.Drawing.Size(63, 20);
            this.num_niveauZoomCentrer.TabIndex = 1;
            this.num_niveauZoomCentrer.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(111, 223);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "niveauZoomCentrer";
            // 
            // cb_afficherLabel
            // 
            this.cb_afficherLabel.AutoSize = true;
            this.cb_afficherLabel.Location = new System.Drawing.Point(42, 58);
            this.cb_afficherLabel.Name = "cb_afficherLabel";
            this.cb_afficherLabel.Size = new System.Drawing.Size(87, 17);
            this.cb_afficherLabel.TabIndex = 3;
            this.cb_afficherLabel.Text = "afficherLabel";
            this.cb_afficherLabel.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(111, 263);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "tailleBuffer";
            // 
            // num_tailleBuffer
            // 
            this.num_tailleBuffer.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.num_tailleBuffer.Location = new System.Drawing.Point(42, 259);
            this.num_tailleBuffer.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.num_tailleBuffer.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.num_tailleBuffer.Name = "num_tailleBuffer";
            this.num_tailleBuffer.Size = new System.Drawing.Size(63, 20);
            this.num_tailleBuffer.TabIndex = 4;
            this.num_tailleBuffer.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // cb_centrerLabel
            // 
            this.cb_centrerLabel.AutoSize = true;
            this.cb_centrerLabel.Location = new System.Drawing.Point(42, 96);
            this.cb_centrerLabel.Name = "cb_centrerLabel";
            this.cb_centrerLabel.Size = new System.Drawing.Size(85, 17);
            this.cb_centrerLabel.TabIndex = 6;
            this.cb_centrerLabel.Text = "centrerLabel";
            this.cb_centrerLabel.UseVisualStyleBackColor = true;
            // 
            // cb_afficherBuffer
            // 
            this.cb_afficherBuffer.AutoSize = true;
            this.cb_afficherBuffer.Location = new System.Drawing.Point(42, 134);
            this.cb_afficherBuffer.Name = "cb_afficherBuffer";
            this.cb_afficherBuffer.Size = new System.Drawing.Size(89, 17);
            this.cb_afficherBuffer.TabIndex = 7;
            this.cb_afficherBuffer.Text = "afficherBuffer";
            this.cb_afficherBuffer.UseVisualStyleBackColor = true;
            // 
            // b_saveConfig
            // 
            this.b_saveConfig.Location = new System.Drawing.Point(393, 263);
            this.b_saveConfig.Name = "b_saveConfig";
            this.b_saveConfig.Size = new System.Drawing.Size(97, 23);
            this.b_saveConfig.TabIndex = 8;
            this.b_saveConfig.Text = "Sauvegarder";
            this.b_saveConfig.UseVisualStyleBackColor = true;
            this.b_saveConfig.Click += new System.EventHandler(this.b_saveConfig_Click);
            // 
            // Configuration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 311);
            this.Controls.Add(this.b_saveConfig);
            this.Controls.Add(this.cb_afficherBuffer);
            this.Controls.Add(this.cb_centrerLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.num_tailleBuffer);
            this.Controls.Add(this.cb_afficherLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.num_niveauZoomCentrer);
            this.Controls.Add(this.cb_triCoordSurface);
            this.Name = "Configuration";
            this.Text = "Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.num_niveauZoomCentrer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_tailleBuffer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cb_triCoordSurface;
        private System.Windows.Forms.NumericUpDown num_niveauZoomCentrer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cb_afficherLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown num_tailleBuffer;
        private System.Windows.Forms.CheckBox cb_centrerLabel;
        private System.Windows.Forms.CheckBox cb_afficherBuffer;
        private System.Windows.Forms.Button b_saveConfig;
    }
}