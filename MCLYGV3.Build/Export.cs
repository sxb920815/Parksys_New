using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DM.WinForm
{
	public static class Export
	{
		public static void Start(Table ExpTable, List<Table> TableList)
		{
			string exepath = Environment.CurrentDirectory;
			//E:\项目\金融模型评估\FinancialModelAssessment\MCLYGV3.Build\bin\Debug


			string EnglishName = ConfigurationManager.AppSettings["EnglishName"];
			string basepath = exepath.Replace($"{EnglishName}.Build\\bin\\Debug", "");
			//E:\项目\金融模型评估\FinancialModelAssessment\

			string DBPath = basepath + $"{EnglishName}.DB\\DB";

			string ViewControllBasePath = @"E:\铭辰利云\MCLYGroupV3\ViewAndControllers";
			string ViewPath = $"{ViewControllBasePath}\\Views";
			string ControllerPath = $"{ViewControllBasePath}\\Controllers";
			Directory.CreateDirectory(ViewPath);
			Directory.CreateDirectory(ControllerPath);


			MakeModel(EnglishName, DBPath, ExpTable, TableList);
			MakeBllAdd(EnglishName, DBPath, ExpTable, TableList);
			MakeBllEdit(EnglishName, DBPath, ExpTable, TableList);
			MakeBllDel(EnglishName, DBPath, ExpTable, TableList);

			MakeViewList(EnglishName, ViewPath, ExpTable, TableList);
			MakeViewDetail(EnglishName, ViewPath, ExpTable, TableList);
			MakeViewEdit(EnglishName, ViewPath, ExpTable, TableList);
			MakeViewAdd(EnglishName, ViewPath, ExpTable, TableList);

			MakeControllerFunction(EnglishName, ControllerPath, ExpTable, TableList);

		}


		#region MakeViewList
		private static void MakeViewList(string NameSpaceName, string Path, Table ExpTable, List<Table> TableList)
		{
			string E = "\r\n";

			StringBuilder Columns = new StringBuilder();
			string PKColumn = "";

			foreach (var item in ExpTable.Columns)
			{
				string type = item.ColumnType;
				if (item.IsPK)
					PKColumn = item.ColumnName;
				bool IsExt = false;
				if (type != "Identity" && type != "int" && type != "string" && type != "double" && type != "decimal" && type != "DateTime" && type != "bool")
				{
					IsExt = true;
				}
				//, formatter: function (value) { return TableFormatIsShow(value) } 

				if (type =="bool")
					Columns.Append($"{C.ST(4)}{{ field: '{item.ColumnName}', title: '{item.Explain}', sortable: true, formatter: function (value) {{ return IsTrueOrFalse(value) }}  }},{E}");
				else
					Columns.Append($"{C.ST(4)}{{ field: '{item.ColumnName}', title: '{item.Explain}', sortable: true }},{E}");

			}

			string template = File.ReadAllText("Template\\ViewList.txt");
			template = template.Replace("<%NameSpaceName%>", NameSpaceName);
			template = template.Replace("<%ExpTable.Explain%>", ExpTable.Explain);
			template = template.Replace("<%ExpTable.MenuId%>", ExpTable.MenuId);
			template = template.Replace("<%ExpTable.TableName%>", ExpTable.TableName);

			template = template.Replace("<%PKColumn%>", PKColumn);
			template = template.Replace("<%Columns%>", Columns.ToString().TrimStart('\t'));

			string NewFileName = Path + "\\" + ExpTable.TableName + "_List.cshtml";
			File.WriteAllText(NewFileName, template, Encoding.UTF8);

		}
		#endregion

		#region MakeControllerFunction
		private static void MakeControllerFunction(string NameSpaceName, string Path, Table ExpTable, List<Table> TableList)
		{
			string E = "\r\n";

			StringBuilder ParamPK = new StringBuilder();
			StringBuilder ParamPKValue = new StringBuilder();

			foreach (var item in ExpTable.Columns)
			{
				string type = item.ColumnType;
				string ColumnName = item.ColumnName;

				if (type == "Identity")
					type = "int";

				if (item.IsPK)
				{
					ParamPK.Append($"{type} {ColumnName},");
					ParamPKValue.Append($"{ColumnName},");
				}
			}

			string template = File.ReadAllText("Template\\ControllerFunction.txt");
			template = template.Replace("<%NameSpaceName%>", NameSpaceName);
			template = template.Replace("<%ExpTable.Explain%>", ExpTable.Explain);
			template = template.Replace("<%ExpTable.QueryFieldName%>", ExpTable.QueryFieldName);
			template = template.Replace("<%ExpTable.TableName%>", ExpTable.TableName);

			template = template.Replace("<%ParamPK%>", ParamPK.ToString().TrimEnd(','));
			template = template.Replace("<%ParamPKValue%>", ParamPKValue.ToString().TrimEnd(','));

			string NewFileName = Path + $"\\AdministratorController_{ExpTable.TableName}.cs";
			if (File.Exists(NewFileName))
			{
				//File.Move(NewFileName, NewFileName + "." + DateTime.Now.ToString("yyyyMMddHHmmss"));
			}
			File.WriteAllText(NewFileName, template);

		}
		#endregion

		#region MakeViewAdd
		private static void MakeViewAdd(string NameSpaceName, string Path, Table ExpTable, List<Table> TableList)
		{
			string E = "\r\n";

			StringBuilder tr = new StringBuilder();

			foreach (var item in ExpTable.Columns)
			{
				if (item.ColumnType != "Identity" && item.IsMultiple != true)
				{
					tr.Append($"{C.ST(3)}<tr>{E}");
					tr.Append($"{C.ST(4)}<th>{item.Explain}:</th>{E}");
					tr.Append($"{C.ST(4)}<td>{E}");
					tr.Append($"{C.ST(5)}@Html.EditorFor(model => model.{item.ColumnName}){E}");
					tr.Append($"{C.ST(5)}@Html.ValidationMessageFor(model => model.{item.ColumnName}){E}");
					tr.Append($"{C.ST(4)}</td>{E}");
					tr.Append($"{C.ST(3)}</tr>{E}");
				}
			}

			string template = File.ReadAllText("Template\\ViewAdd.txt");
			template = template.Replace("<%NameSpaceName%>", NameSpaceName);
			template = template.Replace("<%ExpTable.Explain%>", ExpTable.Explain);
			template = template.Replace("<%ExpTable.TableName%>", ExpTable.TableName);

			template = template.Replace("<%tr%>", tr.ToString().TrimStart('\t'));

			string NewFileName = Path + "\\" + ExpTable.TableName + "_Add.cshtml";
			File.WriteAllText(NewFileName, template, Encoding.UTF8);

		}
		#endregion

		#region MakeViewEdit
		private static void MakeViewEdit(string NameSpaceName, string Path, Table ExpTable, List<Table> TableList)
		{
			string E = "\r\n";

			StringBuilder tr = new StringBuilder();
			StringBuilder HtmlHiddenFor = new StringBuilder();

			foreach (var item in ExpTable.Columns)
			{
				if (item.IsPK)
				{
					HtmlHiddenFor.Append($"{C.ST(1)}@Html.HiddenFor(model => model.{item.ColumnName}){E}");
				}
				else
				{
					if(item.IsMultiple==false)
					{
						tr.Append($"{C.ST(3)}<tr>{E}");
						tr.Append($"{C.ST(4)}<th>{item.Explain}:</th>{E}");
						tr.Append($"{C.ST(4)}<td>{E}");
						tr.Append($"{C.ST(5)}@Html.EditorFor(model => model.{item.ColumnName}){E}");
						tr.Append($"{C.ST(5)}@Html.ValidationMessageFor(model => model.{item.ColumnName}){E}");
						tr.Append($"{C.ST(4)}</td>{E}");
						tr.Append($"{C.ST(3)}</tr>{E}");
					}
				}
			}

			string template = File.ReadAllText("Template\\ViewEdit.txt");
			template = template.Replace("<%NameSpaceName%>", NameSpaceName);
			template = template.Replace("<%ExpTable.Explain%>", ExpTable.Explain);
			template = template.Replace("<%ExpTable.TableName%>", ExpTable.TableName);

			template = template.Replace("<%tr%>", tr.ToString().TrimStart('\t'));
			template = template.Replace("<%HtmlHiddenFor%>", HtmlHiddenFor.ToString().TrimStart('\t'));

			string NewFileName = Path + "\\" + ExpTable.TableName + "_Edit.cshtml";
			File.WriteAllText(NewFileName, template, Encoding.UTF8);

		}
		#endregion

		#region MakeViewDetail


		private static void MakeViewDetail(string NameSpaceName, string Path, Table ExpTable, List<Table> TableList)
		{
			string E = "\r\n";

			StringBuilder tr = new StringBuilder();

			foreach (var item in ExpTable.Columns)
			{

				tr.Append($"{C.ST(2)}<tr>{E}");
				tr.Append($"{C.ST(3)}<th>{item.Explain}:</th>{E}");
				tr.Append($"{C.ST(3)}<td>@Html.DisplayFor(model => model.{item.ColumnName})</td>{E}");
				tr.Append($"{C.ST(2)}</tr>{E}");
			}

			string template = File.ReadAllText("Template\\ViewDetail.txt");
			template = template.Replace("<%NameSpaceName%>", NameSpaceName);
			template = template.Replace("<%ExpTable.Explain%>", ExpTable.Explain);
			template = template.Replace("<%ExpTable.TableName%>", ExpTable.TableName);

			template = template.Replace("<%tr%>", tr.ToString().TrimStart('\t'));

			string NewFileName = Path + "\\" + ExpTable.TableName + "_Detail.cshtml";
			File.WriteAllText(NewFileName, template, Encoding.UTF8);

		}
		#endregion

		#region MakeBllDel
		private static void MakeBllDel(string NameSpaceName, string Path, Table ExpTable, List<Table> TableList)
		{
			string E = "\r\n";

			StringBuilder ParamPK = new StringBuilder();
			StringBuilder ParamPKExplain = new StringBuilder();
			StringBuilder MakeDelObj = new StringBuilder();
			StringBuilder ParamPKValue = new StringBuilder();



			foreach (var item in ExpTable.Columns)
			{
				string type = item.ColumnType;
				string ColumnName = item.ColumnName;
				bool isTypeExtend = false;



				if (type != "Identity" && type != "int" && type != "string" && type != "double" && type != "decimal" && type != "DateTime" && type != "bool")
				{
					isTypeExtend = true;
				}
				if (type == "Identity")
					type = "int";

				if (item.IsPK)
				{
					ParamPKExplain.Append($"{C.ST(2)}/// <param name=\"{ColumnName}\">{item.Explain}</param>{E}");
					ParamPK.Append($"{type} {ColumnName},");
					MakeDelObj.Append($"{ColumnName} = {ColumnName},");
					ParamPKValue.Append($"{ColumnName},");
				}



			}

			string template = File.ReadAllText("Template\\Bll_DelQuery.txt");
			template = template.Replace("<%NameSpaceName%>", NameSpaceName);
			template = template.Replace("<%ExpTable.Explain%>", ExpTable.Explain);
			template = template.Replace("<%ExpTable.TableName%>", ExpTable.TableName);

			template = template.Replace("<%ParamPKExplain%>", ParamPKExplain.ToString().TrimStart('\t'));
			template = template.Replace("<%ParamPK%>", ParamPK.ToString().TrimEnd(','));
			template = template.Replace("<%MakeDelObj%>", MakeDelObj.ToString().TrimEnd(','));
			template = template.Replace("<%ParamPKValue%>", ParamPKValue.ToString().TrimEnd(','));

			string NewFileName = Path + "\\" + ExpTable.TableName + "_Bll_DelQuery.cs";
			if (File.Exists(NewFileName))
			{
				//File.Move(NewFileName, NewFileName + "." + DateTime.Now.ToString("yyyyMMddHHmmss"));
			}
			File.WriteAllText(NewFileName, template);

		}
		#endregion

		#region MakeBllEdit
		private static void MakeBllEdit(string NameSpaceName, string Path, Table ExpTable, List<Table> TableList)
		{
			string E = "\r\n";

			StringBuilder EditObjToObj = new StringBuilder();
			StringBuilder ParentFind = new StringBuilder();


			foreach (var item in ExpTable.Columns)
			{
				string type = item.ColumnType;
				string ColumnName = item.ColumnName;
				bool isTypeExtend = false;

				if (type != "Identity" && type != "int" && type != "string" && type != "double" && type != "decimal" && type != "DateTime" && type != "bool")
				{
					isTypeExtend = true;
				}
				if (type == "Identity")
					type = "int";
				if (item.IsMultiple == false)
				{
					if (item.IsPK)
					{
						ParentFind.Append($"Edit{ExpTable.TableName}Obj.{ColumnName},");
					}
					else
					{
						if (isTypeExtend == false)
							EditObjToObj.Append($"{C.ST(5)}{ExpTable.TableName}Obj.{ColumnName} = Edit{ExpTable.TableName}Obj.{ColumnName};{E}");
						else
						{
							Table ChildTable = TableList.FirstOrDefault(t => t.TableName == type);
							string ChildFind = "";
							foreach (var ChildItem in ChildTable.Columns)
							{
								if (ChildItem.IsPK)
								{
									ChildFind = $"{ChildFind} Edit{ExpTable.TableName}Obj.{ColumnName}.{ChildItem.ColumnName},";
								}
							}
							EditObjToObj.Append($"{C.ST(5)}if (Edit{ExpTable.TableName}Obj.{ColumnName} != null){E}");
							EditObjToObj.Append($"{C.ST(6)}{ExpTable.TableName}Obj.{ColumnName} = db.{type}List.Find({ChildFind.TrimEnd(',')});{E}");
							EditObjToObj.Append($"{C.ST(5)}else{E}");
							EditObjToObj.Append($"{C.ST(6)}{ExpTable.TableName}Obj.{ColumnName} = null;{E}");
						}
					}
				}
			}

			string template = File.ReadAllText("Template\\Bll_Edit.txt");
			template = template.Replace("<%NameSpaceName%>", NameSpaceName);
			template = template.Replace("<%ExpTable.Explain%>", ExpTable.Explain);
			template = template.Replace("<%ExpTable.TableName%>", ExpTable.TableName);

			template = template.Replace("<%EditObjToObj%>", EditObjToObj.ToString().TrimStart('\t'));
			template = template.Replace("<%ParentFind%>", ParentFind.ToString().TrimEnd(','));

			string NewFileName = Path + "\\" + ExpTable.TableName + "_Bll_Edit.cs";
			if (File.Exists(NewFileName))
			{
				//File.Move(NewFileName, NewFileName + "." + DateTime.Now.ToString("yyyyMMddHHmmss"));
			}
			File.WriteAllText(NewFileName, template);

		}
		#endregion

		#region MakeBllAdd
		private static void MakeBllAdd(string NameSpaceName, string Path, Table ExpTable, List<Table> TableList)
		{
			string E = "\r\n";


			StringBuilder ParamExplain = new StringBuilder();
			StringBuilder Param = new StringBuilder();
			StringBuilder ParamToObj = new StringBuilder();
			StringBuilder AttachChild = new StringBuilder();

			foreach (var item in ExpTable.Columns)
			{
				string type = item.ColumnType;
				string ColumnName = item.ColumnName;
				bool isTypeExtend = false;
				if (type != "Identity" && type != "int" && type != "string" && type != "double" && type != "decimal" && type != "DateTime" && type != "bool")
				{
					isTypeExtend = true;
					type = "M_" + type;
				}

				if (item.IsMultiple == false)
				{
					if (type != "Identity")
					{
						ParamExplain.Append($"{C.ST(2)}/// <param name=\"{ColumnName}\">{item.Explain}</param>{E}");
						Param.Append($"{type} {ColumnName},");
						ParamToObj.Append($"{C.ST(3)}{ExpTable.TableName}Obj.{ColumnName} = {ColumnName};{E}");
					}
					if (isTypeExtend)
					{
						AttachChild.Append($"{C.ST(5)}if ({ExpTable.TableName}Obj.{ColumnName} != null){E}");
						AttachChild.Append($"{C.ST(6)}db.{item.ColumnType}List.Attach({ExpTable.TableName}Obj.{ColumnName});{E}");
					}
				}
			}

			string template = File.ReadAllText("Template\\Bll_Add.txt");
			template = template.Replace("<%NameSpaceName%>", NameSpaceName);
			template = template.Replace("<%ExpTable.Explain%>", ExpTable.Explain);
			template = template.Replace("<%ExpTable.TableName%>", ExpTable.TableName);

			template = template.Replace("<%ParamExplain%>", ParamExplain.ToString().TrimStart('\t'));
			template = template.Replace("<%Param%>", Param.ToString().TrimEnd(','));
			template = template.Replace("<%ParamToObj%>", ParamToObj.ToString().TrimStart('\t'));
			template = template.Replace("<%AttachChild%>", AttachChild.ToString().TrimStart('\t'));

			string NewFileName = Path + "\\" + ExpTable.TableName + "_Bll_Add.cs";
			if (File.Exists(NewFileName))
			{
				//File.Move(NewFileName, NewFileName + "." + DateTime.Now.ToString("yyyyMMddHHmmss"));
			}
			File.WriteAllText(NewFileName, template);

		}
		#endregion

		#region MakeModel
		private static void MakeModel(string NameSpaceName, string Path, Table ExpTable, List<Table> TableList)
		{
			string E = "\r\n";
			StringBuilder sb = new StringBuilder();
			StringBuilder sbConstructor = new StringBuilder();
			sb.Append($"using System;{E}");
			sb.Append($"using Newtonsoft.Json;{E}");
			sb.Append($"using System.Collections.Generic;{E}");
			sb.Append($"using System.ComponentModel;{E}");
			sb.Append($"using System.ComponentModel.DataAnnotations;{E}");
			sb.Append($"using System.ComponentModel.DataAnnotations.Schema;{E}{E}{E}");


			sb.Append($"namespace {NameSpaceName}.DB{E}");
			sb.Append($"{{{E}");

			#region 数据库实体类

			int pknum = 1;
			sb.Append($"{C.ST(1)}/// <summary>{E}");
			sb.Append($"{C.ST(1)}/// {ExpTable.Explain}{E}");
			sb.Append($"{C.ST(1)}/// </summary>{E}");
			sb.Append($"{C.ST(1)}[Table(\"{ExpTable.TableName}\")]{E}");
			//sb.Append($"{C.ST(1)}[JsonObject(MemberSerialization.OptOut)]{E}");
			sb.Append($"{C.ST(1)}public class M_{ExpTable.TableName}{E}");
			sb.Append($"{C.ST(1)}{{{E}");

			foreach (var item in ExpTable.Columns)
			{
				sb.Append($"{C.ST(2)}/// <summary>{E}");
				sb.Append($"{C.ST(2)}/// {item.Explain}{E}");
				sb.Append($"{C.ST(2)}/// </summary>{E}");
				sb.Append($"{C.ST(2)}[DisplayName(\"{item.Explain}\")]{E}");
				string type = item.ColumnType;
				string ColumnName = item.ColumnName;
				bool isTypeExt = false;
				if (type != "Identity" && type != "int" && type != "string" && type != "double" && type != "decimal" && type != "DateTime" && type != "bool")
				{
					type = "M_" + type;
					isTypeExt = true;
				}

				if (item.IsEmpty == true)
				{
					if (type == "int" || type == "double" || type == "decimal" || type == "DateTime")
						type = type + "?";
				}
				if (item.IsMultiple)
				{
					type = "virtual ICollection<" + type + ">";
					ColumnName = ColumnName + "List";
					sbConstructor.Append($"{C.ST(3)}{ColumnName} = new HashSet<M_{item.ColumnType}>();{E}");
					//sb.Append($"{C.ST(2)}[JsonIgnore]{E}");
				}
				else
				{
					if (isTypeExt)
						type = "virtual " + type;
				}

				if (item.IsPK)
				{
					sb.Append($"{C.ST(2)}[Key]{E}");
					sb.Append($"{C.ST(2)}[Column(Order = {pknum})]{E}");
					pknum++;
				}

				if (type == "Identity")
				{
					sb.Append($"{C.ST(2)}[DatabaseGenerated(DatabaseGeneratedOption.Identity)]{E}");
					type = "int";
				}
				else if (type == "int" && item.IsPK)
					sb.Append($"{C.ST(2)}[DatabaseGenerated(DatabaseGeneratedOption.None)]{E}");

				if (item.IsEmpty == false && item.IsMultiple == false)
					sb.Append($"{C.ST(2)}[Required]{E}");

				sb.Append($"{C.ST(2)}public {type} {ColumnName}  {{ get; set; }}{E}");
			}
			sb.Append($"{E}{E}{E}");

			if (!string.IsNullOrEmpty(sbConstructor.ToString()))
			{
				sb.Append($"{C.ST(2)}public M_{ExpTable.TableName}(){E}");
				sb.Append($"{C.ST(2)}{{{E}");
				sb.Append(sbConstructor.ToString());
				sb.Append($"{C.ST(2)}}}{E}");
			}
			sb.Append($"{C.ST(1)}}}{E}");
			#endregion

			sb.Append("}\r\n");
			string NewFileName = $"{Path}\\{ExpTable.TableName}_M.cs";
			if (File.Exists(NewFileName))
			{
				//File.Move(NewFileName, NewFileName + "." + DateTime.Now.ToString("yyyyMMddHHmmss"));
			}

			File.WriteAllText(NewFileName, sb.ToString());

		}
		#endregion


	}
}
