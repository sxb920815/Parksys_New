using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
// ReSharper disable All

namespace DM.WinForm
{
	public partial class FrmMain : Form
	{
		public static List<Table> TableList;
		public static Table NowTable;
		private bool IsGridChanged;

		public FrmMain()
		{
			InitializeComponent();
			this.dataGridView1.AutoGenerateColumns = false;
			ReadProject();
		}


		#region 读取项目
		private void ReadProject()
		{
			string EnglishName = ConfigurationManager.AppSettings["EnglishName"];
			string FileName = EnglishName + ".txt";
			if (File.Exists(FileName))
			{
				string a = File.ReadAllText(FileName);
				TableList = (List<Table>)JsonConvert.DeserializeObject(a, typeof(List<Table>));
			}
			else
			{
				TableList = new List<Table>();
			}
			listBox1Refresh();
			IsGridChanged = false;
			listBox1.Enabled = true;
			btnNewTable.Enabled = true;
			btnDelTable.Enabled = true;
			btnRenameTable.Enabled = true;

			txtTname.Text = "";
			txtTname.Enabled = false;
			txtTexplain.Text = "";
			txtTMenuId.Text = "";
			txtTQueryField.Text = "";

			dataGridView1.Enabled = false;
			btnFadd.Enabled = false;
			btnFDel.Enabled = false;
			btnFEdit.Enabled = false;
			btnFUp.Enabled = false;
			btnFDown.Enabled = false;
			btnS1.Enabled = false;
			btnS2.Enabled = false;

			dataGridView1.Rows.Clear();
		}

		private void listBox1Refresh()
		{
			listBox1.Items.Clear();
			var TableNameList = TableList.Select(t => t.TableName).ToList();
			foreach (var item in TableNameList)
			{
				listBox1.Items.Add(item);
			}
		}


		#endregion


		#region ListBox部分
		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			//if (!this.IsGridChanged || (DialogResult.OK == MessageBox.Show("您要放弃刚刚的修改吗？", "消息", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk)))
			{
				if (listBox1.SelectedItem == null)
					return;



				string TableName = listBox1.SelectedItem.ToString();
				NowTable = TableList.FirstOrDefault(t => t.TableName == TableName);

				if (NowTable == null)
					return;
				txtTname.Enabled = false;
				txtTname.Text = NowTable.TableName;
				txtTexplain.Text = NowTable.Explain;
				txtTMenuId.Text = NowTable.MenuId;
				txtTQueryField.Text = NowTable.QueryFieldName;

				dataGridView1Refresh();

				dataGridView1.Enabled = true;
				btnFadd.Enabled = true;
				btnFDel.Enabled = true;
				btnFEdit.Enabled = true;
				btnFUp.Enabled = true;
				btnFDown.Enabled = true;
				btnS1.Enabled = false;
				btnS2.Enabled = false;
			}

			//dataGridView1
		}
		private void btnNewTable_Click(object sender, EventArgs e)
		{
			this.dataGridView1.Rows.Clear();
			this.txtTname.Enabled = true;
			string str = "NewTable";
			this.txtTname.Text = str;

			btnNewTable.Enabled = false;
			btnDelTable.Enabled = false;
			btnRenameTable.Enabled = false;
			listBox1.Enabled = false;

			btnFadd.Enabled = true;
			btnFDel.Enabled = true;
			btnFEdit.Enabled = true;
			btnFUp.Enabled = true;
			btnFDown.Enabled = true;
			btnS1.Enabled = true;
			btnS2.Enabled = true;
		}

		private void btnDelTable_Click(object sender, EventArgs e)
		{
			if (!this.IsGridChanged || (DialogResult.OK == MessageBox.Show("您要放弃刚刚的修改吗？", "消息", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk)))
			{
				string TableName = listBox1.SelectedItem.ToString();
				NowTable = TableList.FirstOrDefault(t => t.TableName == TableName);
				TableList.Remove(NowTable);
				if (TableList.Count > 0)
					NowTable = TableList.FirstOrDefault();
				else
					NowTable = null;

				string FileName = ConfigurationManager.AppSettings["EnglishName"] + ".txt";
				string txt = JsonConvert.SerializeObject(TableList);
				File.WriteAllText(FileName, txt);

				dataGridView1Refresh();
				listBox1Refresh();
				for (int i = 0; i < listBox1.Items.Count; i++)
				{
					//comboBox1.SelectedIndex = i;
					if (listBox1.Items[i].ToString() == NowTable.TableName)
					{
						listBox1.SelectedIndex = i;
						break;
					}
				}
			}
		}
		#endregion

		#region GridView部分
		private bool GetBool(object value)
		{
			if (value == DBNull.Value)
				return false;

			return Convert.ToBoolean(value);

		}
		private void dataGridView1Refresh()
		{
			dataGridView1.Rows.Clear();
			if (NowTable != null)
			{
				if (NowTable.Columns != null)
				{
					foreach (var item in NowTable.Columns)
					{
						int num = this.dataGridView1.Rows.Add();
						this.dataGridView1.Rows[num].Cells["ColFieldName"].Value = item.ColumnName;
						this.dataGridView1.Rows[num].Cells["ColFieldType"].Value = item.ColumnType;
						this.dataGridView1.Rows[num].Cells["ColIsMultiple"].Value = item.IsMultiple;
						this.dataGridView1.Rows[num].Cells["ColIsPK"].Value = item.IsPK;
						this.dataGridView1.Rows[num].Cells["ColIsNull"].Value = item.IsEmpty;
						this.dataGridView1.Rows[num].Cells["ColExplain"].Value = item.Explain;
					}
				}
			}
			this.btnS1.Enabled = false;
			this.btnS2.Enabled = false;
			this.IsGridChanged = false;
			this.listBox1.Enabled = true;
			btnNewTable.Enabled = true;
			btnDelTable.Enabled = true;
			btnRenameTable.Enabled = true;
		}

		private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			this.IsGridChanged = true;
			this.btnS1.Enabled = true;
			this.btnS2.Enabled = true;
			listBox1.Enabled = false;
			btnNewTable.Enabled = false;
			btnDelTable.Enabled = false;
			btnRenameTable.Enabled = false;
		}

		private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (this.dataGridView1.SelectedRows.Count > 0)
			{
				int index = this.dataGridView1.CurrentRow.Index;
				this.EditField(index);
			}
		}

		#endregion

		#region 字段的增删改查
		private bool IsColumnReiteration(List<string> ColumnNames, List<ColumnItem> ColumnItemList)
		{
			foreach (var item in ColumnNames)
			{
				if (ColumnItemList.Where(t => t.ColumnName == item).ToList().Count > 1)
					return true;
			}
			return false;
		}
		private bool IsTableNameReiteration()
		{
			foreach (var item in TableList)
			{
				string tname = item.TableName;
				if (TableList.Where(t => t.TableName == tname).ToList().Count > 1)
					return true;
			}
			return false;
		}
		private void EditField(int index)
		{
			ColumnItem editColumnItem = new ColumnItem
			{

				ColumnName = this.dataGridView1[0, index].Value.ToString(),
				ColumnType = this.dataGridView1[1, index].Value.ToString(),
				IsMultiple = Convert.ToBoolean(this.dataGridView1[2, index].Value),
				IsPK = Convert.ToBoolean(this.dataGridView1[3, index].Value),
				IsEmpty = Convert.ToBoolean(this.dataGridView1[4, index].Value),
				Explain = this.dataGridView1[5, index].Value.ToString()
			};
			FrmFieldAddEdit edit = new FrmFieldAddEdit(editColumnItem, "Edit", this);
			if (DialogResult.OK == edit.ShowDialog())
			{
				editColumnItem = edit.ColumnInfo;
				this.dataGridView1[0, index].Value = editColumnItem.ColumnName;
				this.dataGridView1[1, index].Value = editColumnItem.ColumnType;
				this.dataGridView1[2, index].Value = editColumnItem.IsMultiple;
				this.dataGridView1[3, index].Value = editColumnItem.IsPK;
				this.dataGridView1[4, index].Value = editColumnItem.IsEmpty;
				this.dataGridView1[5, index].Value = editColumnItem.Explain;
				this.IsGridChanged = true;
				this.listBox1.Enabled = false;
				btnNewTable.Enabled = false;
				btnDelTable.Enabled = false;
				btnRenameTable.Enabled = false;
				this.btnS1.Enabled = true;
				this.btnS2.Enabled = true;
			}
		}
		public void AddField(ColumnItem addColumnItem)
		{
			int num = this.dataGridView1.Rows.Add();
			this.dataGridView1[0, num].Value = addColumnItem.ColumnName;
			this.dataGridView1[1, num].Value = addColumnItem.ColumnType;
			this.dataGridView1[2, num].Value = addColumnItem.IsMultiple;
			this.dataGridView1[3, num].Value = addColumnItem.IsPK;
			this.dataGridView1[4, num].Value = addColumnItem.IsEmpty;
			this.dataGridView1[5, num].Value = addColumnItem.Explain;
			this.IsGridChanged = true;
			this.listBox1.Enabled = false;
			btnNewTable.Enabled = false;
			btnDelTable.Enabled = false;
			btnRenameTable.Enabled = false;
			this.btnS1.Enabled = true;
			this.btnS2.Enabled = true;
		}
		private void btnFadd_Click(object sender, EventArgs e)
		{
			ColumnItem editColumnItem = new ColumnItem();
			FrmFieldAddEdit edit = new FrmFieldAddEdit(editColumnItem, "Add", this);
			if (DialogResult.OK == edit.ShowDialog())
			{
			}
		}
		private void btnFDown_Click(object sender, EventArgs e)
		{
			if (this.dataGridView1.SelectedRows.Count == 0)
			{
				MessageBox.Show("请选择您要移动哪一行!");
			}
			else
			{
				int index = this.dataGridView1.CurrentRow.Index;
				if (index < (this.dataGridView1.Rows.Count - 1))
				{
					int tid = index + 1;
					this.EditGridView(index, tid);
				}
			}
		}
		private void EditGridView(int sid, int tid)
		{
			this.IsGridChanged = true;
			listBox1.Enabled = false;
			this.btnS1.Enabled = true;
			this.btnS2.Enabled = true;
			ColumnItem info = new ColumnItem
			{
				ColumnName = this.dataGridView1[0, sid].Value.ToString(),
				ColumnType = this.dataGridView1[1, sid].Value.ToString(),
				IsMultiple = Convert.ToBoolean(this.dataGridView1[2, sid].Value.ToString()),
				IsPK = Convert.ToBoolean(this.dataGridView1[3, sid].Value),
				IsEmpty = Convert.ToBoolean(this.dataGridView1[4, sid].Value),
				Explain = this.dataGridView1[5, sid].Value.ToString()
			};
			for (int i = 0; i < 6; i++)
			{
				this.dataGridView1[i, sid].Value = this.dataGridView1[i, tid].Value;
			}
			this.dataGridView1[0, tid].Value = info.ColumnName;
			this.dataGridView1[1, tid].Value = info.ColumnType;
			this.dataGridView1[2, tid].Value = info.IsMultiple;
			this.dataGridView1[3, tid].Value = info.IsPK;
			this.dataGridView1[4, tid].Value = info.IsEmpty;
			this.dataGridView1[5, tid].Value = info.Explain;
			this.dataGridView1.Rows[tid].Cells[0].Selected = true;
		}

		private void btnFUp_Click(object sender, EventArgs e)
		{
			if (this.dataGridView1.SelectedRows.Count == 0)
			{
				MessageBox.Show("请选择您要移动哪一行!");
			}
			else
			{
				int index = this.dataGridView1.CurrentRow.Index;
				if (index > 0)
				{
					int tid = index - 1;
					this.EditGridView(index, tid);
				}
			}
		}

		private void btnFEdit_Click(object sender, EventArgs e)
		{
			if (this.dataGridView1.SelectedRows.Count > 0)
			{
				int index = this.dataGridView1.CurrentRow.Index;
				this.EditField(index);
			}
			else
			{
				MessageBox.Show("请选择您要编辑哪个字段!");
			}
		}

		private void btnFDel_Click(object sender, EventArgs e)
		{
			if (this.dataGridView1.SelectedRows.Count > 0)
			{
				this.dataGridView1.Rows.Remove(this.dataGridView1.CurrentRow);
				this.IsGridChanged = true;
				this.listBox1.Enabled = false;
				btnNewTable.Enabled = false;
				btnDelTable.Enabled = false;
				btnRenameTable.Enabled = false;
				this.btnS1.Enabled = true;
				this.btnS2.Enabled = true;
			}
			else
			{
				MessageBox.Show("请选择您要删除哪个字段!");
			}
		}
		#endregion

		#region 保存表信息S1S2
		private void btnS1_Click(object sender, EventArgs e)
		{
			List<ColumnItem> ColumnItemList = new List<ColumnItem>();
			foreach (DataGridViewRow item in dataGridView1.Rows)
			{
				ColumnItem obj = new ColumnItem();
				obj.ColumnName = item.Cells[0].Value.ToString();
				obj.ColumnType = item.Cells[1].Value.ToString();
				obj.IsMultiple = GetBool(item.Cells[2].Value);
				obj.IsPK = GetBool(item.Cells[3].Value);
				obj.IsEmpty = GetBool(item.Cells[4].Value);
				obj.Explain = item.Cells[5].Value.ToString();

				if (obj.IsPK && obj.IsEmpty)
				{
					MessageBox.Show("主键不能为空！");
					return;
				}
				if (obj.IsMultiple && obj.IsEmpty == false)
				{
					MessageBox.Show("集合属性必须可为空！");
					return;
				}
				string type = obj.ColumnType;
				bool isTypeExtend = false;
				if (type != "Identity" && type != "int" && type != "string" && type != "double" && type != "decimal" && type != "DateTime" && type != "bool")
				{
					isTypeExtend = true;
				}
				if (isTypeExtend && obj.IsEmpty && obj.IsMultiple == false)
				{
					MessageBox.Show("外键的关联字段不可为空！");
					return;
				}


				ColumnItemList.Add(obj);
			}
			var ColumnNames = ColumnItemList.Select(t => t.ColumnName).ToList();
			if (IsColumnReiteration(ColumnNames, ColumnItemList))
			{
				MessageBox.Show("列名不能有重复！");
				return;
			}
			if (IsTableNameReiteration())
			{
				MessageBox.Show("表名不能有重复！");
				return;
			}

			bool IsNew = false;
			if (TableList.Where(t => t.TableName == txtTname.Text).ToList().Count == 0)
			{
				NowTable = new Table() { TableName = txtTname.Text, Explain = txtTexplain.Text, Columns = new List<ColumnItem>() };
				IsNew = true;
			}

			NowTable.Explain = txtTexplain.Text;
			NowTable.MenuId = txtTMenuId.Text;
			NowTable.QueryFieldName = txtTQueryField.Text;
			NowTable.Columns = ColumnItemList;

			TableList.Remove(TableList.FirstOrDefault(t => t.TableName == NowTable.TableName));
			TableList.Add(NowTable);
			TableList.Sort(new myComparer());

			string FileName = ConfigurationManager.AppSettings["EnglishName"] + ".txt";
			string txt = JsonConvert.SerializeObject(TableList);
			File.WriteAllText(FileName, txt);


			if (IsNew)
			{
				listBox1Refresh();
				for (int i = 0; i < listBox1.Items.Count; i++)
				{
					//comboBox1.SelectedIndex = i;
					if (listBox1.Items[i].ToString() == NowTable.TableName)
					{
						listBox1.SelectedIndex = i;
						break;
					}
				}
			}
			else
			{
				dataGridView1Refresh();

			}


		}
		private void btnS2_Click(object sender, EventArgs e)
		{
			dataGridView1Refresh();
			txtTname.Text = NowTable.TableName;
			txtTexplain.Text = NowTable.Explain;
			txtTMenuId.Text = NowTable.MenuId;
			txtTQueryField.Text = NowTable.QueryFieldName;
		}
		#endregion

		private void txtTexplain_TextChanged(object sender, EventArgs e)
		{
			if (NowTable == null)
				return;
			if (txtTexplain.Text != NowTable.Explain)
			{
				this.IsGridChanged = true;
				this.listBox1.Enabled = false;
				btnNewTable.Enabled = false;
				btnDelTable.Enabled = false;
				btnRenameTable.Enabled = false;
				this.btnS1.Enabled = true;
				this.btnS2.Enabled = true;
			}
		}
		private void txtTMenuId_TextChanged(object sender, EventArgs e)
		{
			if (NowTable == null)
				return;
			if (txtTMenuId.Text != NowTable.MenuId)
			{
				this.IsGridChanged = true;
				this.listBox1.Enabled = false;
				btnNewTable.Enabled = false;
				btnDelTable.Enabled = false;
				btnRenameTable.Enabled = false;
				this.btnS1.Enabled = true;
				this.btnS2.Enabled = true;
			}
		}
		private void button3_Click(object sender, EventArgs e)
		{
			foreach (var item in TableList)
			{
				Export.Start(item, TableList);
			}

			MessageBox.Show("导出完毕！");
		}

		private void txtTQueryField_TextChanged(object sender, EventArgs e)
		{
			if (NowTable == null)
				return;
			if (txtTQueryField.Text != NowTable.QueryFieldName)
			{
				this.IsGridChanged = true;
				this.listBox1.Enabled = false;
				btnNewTable.Enabled = false;
				btnDelTable.Enabled = false;
				btnRenameTable.Enabled = false;
				this.btnS1.Enabled = true;
				this.btnS2.Enabled = true;
			}
		}
	}
	public class myComparer : IComparer<Table>
	{
		//实现按年龄升序排列
		public int Compare(Table x, Table y)
		{
			return (x.TableName.CompareTo(y.TableName)); //age代表年龄属性是整型，即其已支持CompareTo方法
		}
	}


	public class ListItem
	{
		private string m_Text;
		private string m_Value;
		public ListItem()
		{
			this.m_Text = String.Empty;
			this.m_Value = String.Empty;
		}
		public string Text
		{
			get { return this.m_Text; }
			set { this.m_Text = value; }
		}
		public string Value
		{
			get { return this.m_Value; }
			set { this.m_Value = value; }
		}
		public override string ToString()
		{
			return this.m_Text;  //最关键的
		}
	}
}
