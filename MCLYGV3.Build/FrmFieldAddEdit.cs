using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DM.WinForm
{
    public partial class FrmFieldAddEdit : Form
    {
        private FrmMain _form;
        private string _addoredit;
        public ColumnItem ColumnInfo;
        public FrmFieldAddEdit(ColumnItem ColumnInfo, string AddorEdit, FrmMain form)
        {
            this.InitializeComponent();
            this._addoredit = AddorEdit;
            this._form = form;
            this.ColumnInfo = ColumnInfo;
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.txtName.Text == "")
            {
                MessageBox.Show("字段名不能为空!");
            }
            else if (this.comboBoxEx1.Text == "")
            {
                MessageBox.Show("字段类型不能为空!");
            }
            else
            {
                ColumnInfo.ColumnName = this.txtName.Text;
                ColumnInfo.ColumnType = this.comboBoxEx1.Text;
                ColumnInfo.IsPK = this.checkBox1.Checked;
                ColumnInfo.IsEmpty = this.checkBox2.Checked;
                ColumnInfo.IsMultiple = this.checkBox3.Checked;
                ColumnInfo.Explain=this.txtExplain.Text;

                if (this._addoredit == "Add")
                {
                    this._form.AddField(ColumnInfo);
                }
                else
                {
                    base.DialogResult = DialogResult.OK;
                }
            }
        }

        private void FrmFieldAddEdit_Load(object sender, EventArgs e)
        {
            this.txtName.Text = this.ColumnInfo.ColumnName;
            if (this.ColumnInfo.ColumnType == null)
            {
                this.comboBoxEx1.Text = "string";
            }
            else
            {
                this.comboBoxEx1.Text = this.ColumnInfo.ColumnType;
            }
            this.checkBox1.Checked = this.ColumnInfo.IsPK;
            this.checkBox2.Checked = this.ColumnInfo.IsEmpty;
            this.checkBox3.Checked = this.ColumnInfo.IsMultiple;
            this.txtExplain.Text = this.ColumnInfo.Explain;
        }
    }
}
