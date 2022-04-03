namespace EventCartoViewer
{
    partial class EventViewer
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventViewer));
            this.cblb_couches = new System.Windows.Forms.CheckedListBox();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.b_loadData = new System.Windows.Forms.Button();
            this.b_applyCarto = new System.Windows.Forms.Button();
            this.b_test = new System.Windows.Forms.Button();
            this.l_currentValue = new System.Windows.Forms.Label();
            this.l_unite = new System.Windows.Forms.Label();
            this.axMap1 = new AxMapWinGIS.AxMap();
            ((System.ComponentModel.ISupportInitialize)(this.axMap1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // cblb_couches
            // 
            this.cblb_couches.FormattingEnabled = true;
            this.cblb_couches.Location = new System.Drawing.Point(860, 10);
            this.cblb_couches.Name = "cblb_couches";
            this.cblb_couches.Size = new System.Drawing.Size(135, 199);
            this.cblb_couches.TabIndex = 1;
            this.cblb_couches.CheckOnClick = true;
            //
            // axMap1
            //
            this.axMap1.Enabled = true;
            this.axMap1.Location = new System.Drawing.Point(10, 10);
            this.axMap1.Name = "axMap1";
            this.axMap1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMap1.OcxState")));
            this.axMap1.Size = new System.Drawing.Size(840, 430);
            this.axMap1.TabIndex = 28;
            // 
            // dgv
            // 
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Location = new System.Drawing.Point(12, 480);
            this.dgv.Name = "dgv";
            this.dgv.Size = new System.Drawing.Size(776, 130);
            this.dgv.TabIndex = 2;
            // 
            // b_loadData
            // 
            this.b_loadData.Location = new System.Drawing.Point(882, 400);
            this.b_loadData.Name = "b_loadData";
            this.b_loadData.Size = new System.Drawing.Size(113, 35);
            this.b_loadData.TabIndex = 63;
            this.b_loadData.Text = "Charger des données";
            this.b_loadData.UseVisualStyleBackColor = true;
            this.b_loadData.Click += new System.EventHandler(this.b_loadData_Click);
            // 
            // b_applyCarto
            // 
            this.b_applyCarto.Location = new System.Drawing.Point(860, 220);
            this.b_applyCarto.Name = "b_applyCarto";
            this.b_applyCarto.Size = new System.Drawing.Size(135, 29);
            this.b_applyCarto.TabIndex = 63;
            this.b_applyCarto.Text = "Appliquer carto";
            this.b_applyCarto.UseVisualStyleBackColor = true;
            this.b_applyCarto.Click += new System.EventHandler(this.b_applyCarto_Click);
            // 
            // b_test
            // 
            this.b_test.Location = new System.Drawing.Point(792, 441);
            this.b_test.Name = "b_test";
            this.b_test.Size = new System.Drawing.Size(84, 29);
            this.b_test.TabIndex = 65;
            this.b_test.Text = "test";
            this.b_test.UseVisualStyleBackColor = true;
            this.b_test.Click += new System.EventHandler(this.b_test_Click);
            this.b_test.Visible = false;
            // 
            // l_currentValue
            // 
            this.l_currentValue.AutoSize = true;
            this.l_currentValue.Location = new System.Drawing.Point(465, 455);
            this.l_currentValue.Name = "l_currentValue";
            this.l_currentValue.Size = new System.Drawing.Size(44, 13);
            this.l_currentValue.TabIndex = 99;
            this.l_currentValue.Text = "Valeur";
            // 
            // l_unite
            // 
            this.l_unite.AutoSize = true;
            this.l_unite.Location = new System.Drawing.Point(580, 455);
            this.l_unite.Name = "l_unite";
            this.l_unite.Size = new System.Drawing.Size(44, 13);
            this.l_unite.TabIndex = 99;
            this.l_unite.Text = "Unite";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 620);
            this.Controls.Add(this.b_test);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.cblb_couches);
            this.Controls.Add(this.b_loadData);
            this.Controls.Add(this.b_applyCarto);
            this.Controls.Add(this.l_currentValue);
            this.Controls.Add(this.l_unite);
            this.Controls.Add(this.axMap1);
            this.Name = "EventViewer";
            this.Text = "EventViewer";
            ((System.ComponentModel.ISupportInitialize)(this.axMap1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.CheckedListBox cblb_couches;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button b_loadData;
        private System.Windows.Forms.Button b_applyCarto;
        private AxMapWinGIS.AxMap axMap1;
        private System.Windows.Forms.Button b_test;
        private System.Windows.Forms.Label l_currentValue;
        private System.Windows.Forms.Label l_unite;
        #endregion

    }
}

