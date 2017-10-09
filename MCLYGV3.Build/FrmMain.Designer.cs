namespace DM.WinForm
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.ColFieldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColFieldType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColIsMultiple = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.ColIsPK = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.ColIsNull = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.ColExplain = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtTname = new System.Windows.Forms.TextBox();
			this.txtTexplain = new System.Windows.Forms.TextBox();
			this.btnS1 = new System.Windows.Forms.Button();
			this.btnS2 = new System.Windows.Forms.Button();
			this.btnFadd = new System.Windows.Forms.Button();
			this.btnFDel = new System.Windows.Forms.Button();
			this.btnFEdit = new System.Windows.Forms.Button();
			this.btnFUp = new System.Windows.Forms.Button();
			this.btnFDown = new System.Windows.Forms.Button();
			this.btnNewTable = new System.Windows.Forms.Button();
			this.btnDelTable = new System.Windows.Forms.Button();
			this.btnRenameTable = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel6 = new System.Windows.Forms.Panel();
			this.panel5 = new System.Windows.Forms.Panel();
			this.txtTQueryField = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtTMenuId = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.panel4 = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel6.SuspendLayout();
			this.panel5.SuspendLayout();
			this.panel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBox1.Enabled = false;
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 12;
			this.listBox1.Location = new System.Drawing.Point(10, 10);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(247, 803);
			this.listBox1.TabIndex = 2;
			this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColFieldName,
            this.ColFieldType,
            this.ColIsMultiple,
            this.ColIsPK,
            this.ColIsNull,
            this.ColExplain});
			this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView1.Enabled = false;
			this.dataGridView1.Location = new System.Drawing.Point(0, 0);
			this.dataGridView1.MultiSelect = false;
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowHeadersVisible = false;
			this.dataGridView1.RowTemplate.Height = 23;
			this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView1.Size = new System.Drawing.Size(822, 625);
			this.dataGridView1.TabIndex = 3;
			this.dataGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentDoubleClick);
			this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
			// 
			// ColFieldName
			// 
			this.ColFieldName.HeaderText = "列名";
			this.ColFieldName.Name = "ColFieldName";
			this.ColFieldName.Width = 150;
			// 
			// ColFieldType
			// 
			this.ColFieldType.HeaderText = "类型";
			this.ColFieldType.Name = "ColFieldType";
			this.ColFieldType.Width = 80;
			// 
			// ColIsMultiple
			// 
			this.ColIsMultiple.HeaderText = "是否集合";
			this.ColIsMultiple.Name = "ColIsMultiple";
			// 
			// ColIsPK
			// 
			this.ColIsPK.HeaderText = "主键";
			this.ColIsPK.Name = "ColIsPK";
			this.ColIsPK.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.ColIsPK.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.ColIsPK.Width = 70;
			// 
			// ColIsNull
			// 
			this.ColIsNull.HeaderText = "为空";
			this.ColIsNull.Name = "ColIsNull";
			this.ColIsNull.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.ColIsNull.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.ColIsNull.Width = 70;
			// 
			// ColExplain
			// 
			this.ColExplain.HeaderText = "描述";
			this.ColExplain.Name = "ColExplain";
			this.ColExplain.Width = 200;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(29, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 12);
			this.label1.TabIndex = 4;
			this.label1.Text = "表名：";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(241, 6);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(41, 12);
			this.label2.TabIndex = 4;
			this.label2.Text = "描述：";
			// 
			// txtTname
			// 
			this.txtTname.Enabled = false;
			this.txtTname.Location = new System.Drawing.Point(76, 3);
			this.txtTname.Name = "txtTname";
			this.txtTname.Size = new System.Drawing.Size(150, 21);
			this.txtTname.TabIndex = 5;
			// 
			// txtTexplain
			// 
			this.txtTexplain.Location = new System.Drawing.Point(288, 3);
			this.txtTexplain.Name = "txtTexplain";
			this.txtTexplain.Size = new System.Drawing.Size(150, 21);
			this.txtTexplain.TabIndex = 5;
			this.txtTexplain.TextChanged += new System.EventHandler(this.txtTexplain_TextChanged);
			// 
			// btnS1
			// 
			this.btnS1.Enabled = false;
			this.btnS1.Location = new System.Drawing.Point(11, 69);
			this.btnS1.Name = "btnS1";
			this.btnS1.Size = new System.Drawing.Size(75, 40);
			this.btnS1.TabIndex = 6;
			this.btnS1.Text = "应用";
			this.btnS1.UseVisualStyleBackColor = true;
			this.btnS1.Click += new System.EventHandler(this.btnS1_Click);
			// 
			// btnS2
			// 
			this.btnS2.Enabled = false;
			this.btnS2.Location = new System.Drawing.Point(92, 69);
			this.btnS2.Name = "btnS2";
			this.btnS2.Size = new System.Drawing.Size(75, 40);
			this.btnS2.TabIndex = 6;
			this.btnS2.Text = "取消";
			this.btnS2.UseVisualStyleBackColor = true;
			this.btnS2.Click += new System.EventHandler(this.btnS2_Click);
			// 
			// btnFadd
			// 
			this.btnFadd.Enabled = false;
			this.btnFadd.Location = new System.Drawing.Point(11, 22);
			this.btnFadd.Name = "btnFadd";
			this.btnFadd.Size = new System.Drawing.Size(59, 26);
			this.btnFadd.TabIndex = 7;
			this.btnFadd.Text = "添加";
			this.btnFadd.UseVisualStyleBackColor = true;
			this.btnFadd.Click += new System.EventHandler(this.btnFadd_Click);
			// 
			// btnFDel
			// 
			this.btnFDel.Enabled = false;
			this.btnFDel.Location = new System.Drawing.Point(76, 22);
			this.btnFDel.Name = "btnFDel";
			this.btnFDel.Size = new System.Drawing.Size(59, 26);
			this.btnFDel.TabIndex = 7;
			this.btnFDel.Text = "删除";
			this.btnFDel.UseVisualStyleBackColor = true;
			this.btnFDel.Click += new System.EventHandler(this.btnFDel_Click);
			// 
			// btnFEdit
			// 
			this.btnFEdit.Enabled = false;
			this.btnFEdit.Location = new System.Drawing.Point(141, 22);
			this.btnFEdit.Name = "btnFEdit";
			this.btnFEdit.Size = new System.Drawing.Size(59, 26);
			this.btnFEdit.TabIndex = 7;
			this.btnFEdit.Text = "编辑";
			this.btnFEdit.UseVisualStyleBackColor = true;
			this.btnFEdit.Click += new System.EventHandler(this.btnFEdit_Click);
			// 
			// btnFUp
			// 
			this.btnFUp.Enabled = false;
			this.btnFUp.Location = new System.Drawing.Point(206, 22);
			this.btnFUp.Name = "btnFUp";
			this.btnFUp.Size = new System.Drawing.Size(59, 26);
			this.btnFUp.TabIndex = 7;
			this.btnFUp.Text = "上移";
			this.btnFUp.UseVisualStyleBackColor = true;
			this.btnFUp.Click += new System.EventHandler(this.btnFUp_Click);
			// 
			// btnFDown
			// 
			this.btnFDown.Enabled = false;
			this.btnFDown.Location = new System.Drawing.Point(271, 22);
			this.btnFDown.Name = "btnFDown";
			this.btnFDown.Size = new System.Drawing.Size(59, 26);
			this.btnFDown.TabIndex = 7;
			this.btnFDown.Text = "下移";
			this.btnFDown.UseVisualStyleBackColor = true;
			this.btnFDown.Click += new System.EventHandler(this.btnFDown_Click);
			// 
			// btnNewTable
			// 
			this.btnNewTable.Enabled = false;
			this.btnNewTable.Location = new System.Drawing.Point(0, 19);
			this.btnNewTable.Name = "btnNewTable";
			this.btnNewTable.Size = new System.Drawing.Size(75, 40);
			this.btnNewTable.TabIndex = 6;
			this.btnNewTable.Text = "新建表";
			this.btnNewTable.UseVisualStyleBackColor = true;
			this.btnNewTable.Click += new System.EventHandler(this.btnNewTable_Click);
			// 
			// btnDelTable
			// 
			this.btnDelTable.Enabled = false;
			this.btnDelTable.Location = new System.Drawing.Point(81, 19);
			this.btnDelTable.Name = "btnDelTable";
			this.btnDelTable.Size = new System.Drawing.Size(75, 40);
			this.btnDelTable.TabIndex = 6;
			this.btnDelTable.Text = "删除表";
			this.btnDelTable.UseVisualStyleBackColor = true;
			this.btnDelTable.Click += new System.EventHandler(this.btnDelTable_Click);
			// 
			// btnRenameTable
			// 
			this.btnRenameTable.Enabled = false;
			this.btnRenameTable.Location = new System.Drawing.Point(162, 19);
			this.btnRenameTable.Name = "btnRenameTable";
			this.btnRenameTable.Size = new System.Drawing.Size(75, 40);
			this.btnRenameTable.TabIndex = 6;
			this.btnRenameTable.Text = "重命名";
			this.btnRenameTable.UseVisualStyleBackColor = true;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(173, 69);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 40);
			this.button3.TabIndex = 6;
			this.button3.Text = "导出";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Controls.Add(this.listBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(10);
			this.panel1.Size = new System.Drawing.Size(267, 823);
			this.panel1.TabIndex = 8;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.btnNewTable);
			this.panel2.Controls.Add(this.btnDelTable);
			this.panel2.Controls.Add(this.btnRenameTable);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(10, 752);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(247, 61);
			this.panel2.TabIndex = 3;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.panel6);
			this.panel3.Controls.Add(this.panel5);
			this.panel3.Controls.Add(this.panel4);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(267, 0);
			this.panel3.Name = "panel3";
			this.panel3.Padding = new System.Windows.Forms.Padding(10);
			this.panel3.Size = new System.Drawing.Size(842, 823);
			this.panel3.TabIndex = 9;
			// 
			// panel6
			// 
			this.panel6.Controls.Add(this.dataGridView1);
			this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel6.Location = new System.Drawing.Point(10, 76);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(822, 625);
			this.panel6.TabIndex = 4;
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.txtTQueryField);
			this.panel5.Controls.Add(this.label4);
			this.panel5.Controls.Add(this.txtTname);
			this.panel5.Controls.Add(this.label1);
			this.panel5.Controls.Add(this.txtTMenuId);
			this.panel5.Controls.Add(this.label3);
			this.panel5.Controls.Add(this.txtTexplain);
			this.panel5.Controls.Add(this.label2);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel5.Location = new System.Drawing.Point(10, 10);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(822, 66);
			this.panel5.TabIndex = 1;
			// 
			// txtTQueryField
			// 
			this.txtTQueryField.Location = new System.Drawing.Point(88, 30);
			this.txtTQueryField.Name = "txtTQueryField";
			this.txtTQueryField.Size = new System.Drawing.Size(138, 21);
			this.txtTQueryField.TabIndex = 5;
			this.txtTQueryField.TextChanged += new System.EventHandler(this.txtTQueryField_TextChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(29, 33);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(53, 12);
			this.label4.TabIndex = 4;
			this.label4.Text = "检索列：";
			// 
			// txtTMenuId
			// 
			this.txtTMenuId.Location = new System.Drawing.Point(496, 3);
			this.txtTMenuId.Name = "txtTMenuId";
			this.txtTMenuId.Size = new System.Drawing.Size(150, 21);
			this.txtTMenuId.TabIndex = 5;
			this.txtTMenuId.TextChanged += new System.EventHandler(this.txtTMenuId_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(449, 6);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(41, 12);
			this.label3.TabIndex = 4;
			this.label3.Text = "菜单：";
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.btnS1);
			this.panel4.Controls.Add(this.btnS2);
			this.panel4.Controls.Add(this.btnFDown);
			this.panel4.Controls.Add(this.button3);
			this.panel4.Controls.Add(this.btnFUp);
			this.panel4.Controls.Add(this.btnFadd);
			this.panel4.Controls.Add(this.btnFEdit);
			this.panel4.Controls.Add(this.btnFDel);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel4.Location = new System.Drawing.Point(10, 701);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(822, 112);
			this.panel4.TabIndex = 0;
			// 
			// FrmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1109, 823);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.panel1);
			this.MinimizeBox = false;
			this.Name = "FrmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " 代码生成器";
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel6.ResumeLayout(false);
			this.panel5.ResumeLayout(false);
			this.panel5.PerformLayout();
			this.panel4.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTname;
        private System.Windows.Forms.TextBox txtTexplain;
        private System.Windows.Forms.Button btnS1;
        private System.Windows.Forms.Button btnS2;
        private System.Windows.Forms.Button btnFadd;
        private System.Windows.Forms.Button btnFDel;
        private System.Windows.Forms.Button btnFEdit;
        private System.Windows.Forms.Button btnFUp;
        private System.Windows.Forms.Button btnFDown;
        private System.Windows.Forms.Button btnNewTable;
        private System.Windows.Forms.Button btnDelTable;
        private System.Windows.Forms.Button btnRenameTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColFieldName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColFieldType;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColIsMultiple;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColIsPK;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColIsNull;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColExplain;
        private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.TextBox txtTMenuId;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtTQueryField;
		private System.Windows.Forms.Label label4;
	}
}

