using EventCartoViewer.Properties;

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
            this.tb_coord = new System.Windows.Forms.TextBox();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.b_loadData = new System.Windows.Forms.Button();
            this.b_applyCarto = new System.Windows.Forms.Button();
            this.l_currentValue = new System.Windows.Forms.Label();
            this.l_currentValue2 = new System.Windows.Forms.Label();
            this.l_unite = new System.Windows.Forms.Label();
            this.l_minValue = new System.Windows.Forms.Label();
            this.l_maxValue = new System.Windows.Forms.Label();
            this.l_nbEvent = new System.Windows.Forms.Label();
            this.axMap1 = new AxMapWinGIS.AxMap();
            ((System.ComponentModel.ISupportInitialize)(this.axMap1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
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
            // cblb_couches
            // 
            this.cblb_couches.CheckOnClick = true;
            this.cblb_couches.FormattingEnabled = true;
            this.cblb_couches.Location = new System.Drawing.Point(860, 10);
            this.cblb_couches.Name = "cblb_couches";
            this.cblb_couches.Size = new System.Drawing.Size(135, 199);
            this.cblb_couches.TabIndex = 1;
            // 
            // tb_coord
            // 
            this.tb_coord.Location = new System.Drawing.Point(860, 340);
            this.tb_coord.Name = "tb_coord";
            this.tb_coord.ReadOnly = true;
            this.tb_coord.Size = new System.Drawing.Size(133, 20);
            this.tb_coord.TabIndex = 104;
            // 
            // dgv
            // 
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Location = new System.Drawing.Point(12, 490);
            this.dgv.Name = "dgv";
            this.dgv.Size = new System.Drawing.Size(776, 110);
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
            // l_nbEvent
            // 
            this.l_nbEvent.AutoSize = true;
            this.l_nbEvent.Location = new System.Drawing.Point(800, 490);
            this.l_nbEvent.Name = "l_nbEvent";
            this.l_nbEvent.Size = new System.Drawing.Size(37, 13);
            this.l_nbEvent.TabIndex = 99;
            this.l_nbEvent.Text = "Nb event";
            // 
            // l_currentValue
            // 
            this.l_currentValue.AutoSize = true;
            this.l_currentValue.Location = new System.Drawing.Point(220, 445);
            this.l_currentValue.Name = "l_currentValue";
            this.l_currentValue.Size = new System.Drawing.Size(37, 13);
            this.l_currentValue.TabIndex = 99;
            this.l_currentValue.Text = "Valeur";
            // 
            // l_currentValue2
            // 
            this.l_currentValue2.AutoSize = true;
            this.l_currentValue2.Location = new System.Drawing.Point(340, 445);
            this.l_currentValue2.Name = "l_currentValue2";
            this.l_currentValue2.Size = new System.Drawing.Size(37, 13);
            this.l_currentValue2.TabIndex = 99;
            this.l_currentValue2.Text = "Valeur 2";
            // 
            // l_unite
            // 
            this.l_unite.AutoSize = true;
            this.l_unite.Location = new System.Drawing.Point(610, 465);
            this.l_unite.Name = "l_unite";
            this.l_unite.Size = new System.Drawing.Size(32, 13);
            this.l_unite.TabIndex = 99;
            this.l_unite.Text = "Unite";
            // 
            // l_minValue
            // 
            this.l_minValue.AutoSize = true;
            this.l_minValue.Location = new System.Drawing.Point(12, 460);
            this.l_minValue.Name = "l_minValue";
            this.l_minValue.Size = new System.Drawing.Size(24, 13);
            this.l_minValue.TabIndex = 100;
            this.l_minValue.Text = "Min";
            // 
            // l_maxValue
            // 
            this.l_maxValue.AutoSize = true;
            this.l_maxValue.Location = new System.Drawing.Point(530, 460);
            this.l_maxValue.Name = "l_maxValue";
            this.l_maxValue.Size = new System.Drawing.Size(27, 13);
            this.l_maxValue.TabIndex = 101;
            this.l_maxValue.Text = "Max";
            // 
            // EventViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 620);
            this.Controls.Add(this.l_maxValue);
            this.Controls.Add(this.l_minValue);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.cblb_couches);
            this.Controls.Add(this.b_loadData);
            this.Controls.Add(this.b_applyCarto);
            this.Controls.Add(this.l_currentValue);
            this.Controls.Add(this.l_currentValue2);
            this.Controls.Add(this.l_unite);
            this.Controls.Add(this.tb_coord);
            this.Controls.Add(this.l_nbEvent);
            this.Name = "EventViewer";
            this.Text = "EventViewer";
            this.Controls.Add(this.axMap1);
            ((System.ComponentModel.ISupportInitialize)(this.axMap1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private AxMapWinGIS.AxMap axMap1;
        private System.Windows.Forms.CheckedListBox cblb_couches;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button b_loadData;
        private System.Windows.Forms.Button b_applyCarto;
        private System.Windows.Forms.Label l_currentValue;
        private System.Windows.Forms.Label l_currentValue2;
        private System.Windows.Forms.Label l_unite;
        private System.Windows.Forms.Label l_nbEvent;
        private System.Windows.Forms.TextBox tb_coord;
        #endregion

        private System.Windows.Forms.Label l_minValue;
        private System.Windows.Forms.Label l_maxValue;
    }
}

