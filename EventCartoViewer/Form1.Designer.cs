﻿namespace EventCartoViewer
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.cblb_couches = new System.Windows.Forms.CheckedListBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.b_loadData = new System.Windows.Forms.Button();
            this.axMap1 = new AxMapWinGIS.AxMap();
            ((System.ComponentModel.ISupportInitialize)(this.axMap1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // cblb_couches
            // 
            this.cblb_couches.FormattingEnabled = true;
            this.cblb_couches.Location = new System.Drawing.Point(660, 10);
            this.cblb_couches.Name = "cblb_couches";
            this.cblb_couches.Size = new System.Drawing.Size(135, 214);
            this.cblb_couches.TabIndex = 1;
            // 
            // b_loadData
            // 
            this.b_loadData.Location = new System.Drawing.Point(680, 240);
            this.b_loadData.Name = "b_loadData";
            this.b_loadData.Size = new System.Drawing.Size(113, 29);
            this.b_loadData.TabIndex = 63;
            this.b_loadData.Text = "Load data";
            this.b_loadData.UseVisualStyleBackColor = true;
            this.b_loadData.Click += new System.EventHandler(this.b_loadData_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 276);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(776, 162);
            this.dataGridView1.TabIndex = 2;
            //
            // axMap1
            //
            this.axMap1.Enabled = true;
            this.axMap1.Location = new System.Drawing.Point(5, 5);
            this.axMap1.Name = "axMap1";
            this.axMap1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMap1.OcxState")));
            this.axMap1.Size = new System.Drawing.Size(650, 240);
            this.axMap1.TabIndex = 28;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.cblb_couches);
            this.Controls.Add(this.b_loadData);
            this.Controls.Add(this.axMap1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMap1)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.CheckedListBox cblb_couches;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button b_loadData;
        private AxMapWinGIS.AxMap axMap1;
        #endregion


    }
}

