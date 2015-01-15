namespace Codebreak.Tool.Database
{
    partial class NpcTemplateEdition
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSaveSellList = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.btnAddToSell = new System.Windows.Forms.Button();
            this.listViewSellList = new System.Windows.Forms.ListView();
            this.columnId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnInsertInCurrent = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelCurrentItemName = new System.Windows.Forms.ToolStripStatusLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.btnSaveRewards = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnSaveSellList);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.btnAddToSell);
            this.groupBox1.Controls.Add(this.listViewSellList);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(395, 491);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Vente";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(99, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "ItemId :";
            // 
            // btnSaveSellList
            // 
            this.btnSaveSellList.Location = new System.Drawing.Point(311, 18);
            this.btnSaveSellList.Name = "btnSaveSellList";
            this.btnSaveSellList.Size = new System.Drawing.Size(78, 26);
            this.btnSaveSellList.TabIndex = 3;
            this.btnSaveSellList.Text = "Sauvegarder";
            this.btnSaveSellList.UseVisualStyleBackColor = true;
            this.btnSaveSellList.Click += new System.EventHandler(this.btnSaveSellList_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(147, 21);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(63, 20);
            this.numericUpDown1.TabIndex = 2;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // btnAddToSell
            // 
            this.btnAddToSell.Location = new System.Drawing.Point(6, 18);
            this.btnAddToSell.Name = "btnAddToSell";
            this.btnAddToSell.Size = new System.Drawing.Size(87, 26);
            this.btnAddToSell.TabIndex = 1;
            this.btnAddToSell.Text = "Ajouter";
            this.btnAddToSell.UseVisualStyleBackColor = true;
            this.btnAddToSell.Click += new System.EventHandler(this.btnAddToSell_Click);
            // 
            // listViewSellList
            // 
            this.listViewSellList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnId,
            this.columnName});
            this.listViewSellList.FullRowSelect = true;
            this.listViewSellList.GridLines = true;
            this.listViewSellList.Location = new System.Drawing.Point(6, 50);
            this.listViewSellList.Name = "listViewSellList";
            this.listViewSellList.Size = new System.Drawing.Size(383, 435);
            this.listViewSellList.TabIndex = 0;
            this.listViewSellList.UseCompatibleStateImageBehavior = false;
            this.listViewSellList.View = System.Windows.Forms.View.Details;
            this.listViewSellList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listViewSellList_KeyUp);
            // 
            // columnId
            // 
            this.columnId.Text = "ItemId";
            this.columnId.Width = 84;
            // 
            // columnName
            // 
            this.columnName.Text = "Name";
            this.columnName.Width = 281;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSaveRewards);
            this.groupBox2.Controls.Add(this.numericUpDown3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.numericUpDown2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.btnInsertInCurrent);
            this.groupBox2.Controls.Add(this.treeView1);
            this.groupBox2.Location = new System.Drawing.Point(413, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(612, 485);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Echange";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(87, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Type :";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "item",
            "kamas",
            "entry"});
            this.comboBox1.Location = new System.Drawing.Point(129, 20);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(87, 21);
            this.comboBox1.TabIndex = 2;
            // 
            // btnInsertInCurrent
            // 
            this.btnInsertInCurrent.Location = new System.Drawing.Point(6, 18);
            this.btnInsertInCurrent.Name = "btnInsertInCurrent";
            this.btnInsertInCurrent.Size = new System.Drawing.Size(75, 26);
            this.btnInsertInCurrent.TabIndex = 1;
            this.btnInsertInCurrent.Text = "Inserer";
            this.btnInsertInCurrent.UseVisualStyleBackColor = true;
            this.btnInsertInCurrent.Click += new System.EventHandler(this.btnInsertInCurrent_Click);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(6, 50);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(600, 429);
            this.treeView1.TabIndex = 0;
            this.treeView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyUp);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelCurrentItemName});
            this.statusStrip1.Location = new System.Drawing.Point(0, 506);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1037, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelCurrentItemName
            // 
            this.toolStripStatusLabelCurrentItemName.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabelCurrentItemName.Name = "toolStripStatusLabelCurrentItemName";
            this.toolStripStatusLabelCurrentItemName.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabelCurrentItemName.Text = "Nom de l\'item actuel";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(222, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Valeur :";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(271, 21);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            1316134912,
            2328,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(88, 20);
            this.numericUpDown2.TabIndex = 5;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(365, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Quantité :";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(424, 21);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            1316134912,
            2328,
            0,
            0});
            this.numericUpDown3.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(88, 20);
            this.numericUpDown3.TabIndex = 7;
            this.numericUpDown3.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnSaveRewards
            // 
            this.btnSaveRewards.Location = new System.Drawing.Point(518, 18);
            this.btnSaveRewards.Name = "btnSaveRewards";
            this.btnSaveRewards.Size = new System.Drawing.Size(88, 26);
            this.btnSaveRewards.TabIndex = 8;
            this.btnSaveRewards.Text = "Sauvegarder";
            this.btnSaveRewards.UseVisualStyleBackColor = true;
            this.btnSaveRewards.Click += new System.EventHandler(this.btnSaveRewards_Click);
            // 
            // NpcTemplateEdition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 528);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "NpcTemplateEdition";
            this.Text = "Oklm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NpcTemplateEdition_FormClosing);
            this.Load += new System.EventHandler(this.NpcTemplateEdition_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listViewSellList;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button btnAddToSell;
        private System.Windows.Forms.ColumnHeader columnId;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.Button btnSaveSellList;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCurrentItemName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnInsertInCurrent;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSaveRewards;
    }
}