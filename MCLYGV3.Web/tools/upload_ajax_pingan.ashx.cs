using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Web.SessionState;
using System.Web;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Aspose.Cells;
using System.Text;
using MCLYGV3.Web;

namespace DTcms.Web.tools
{
	/// <summary>
	/// 文件上传处理页
	/// </summary>
	public class upload_ajax_pingan : IHttpHandler, IRequiresSessionState
	{
		#region 声明
		public string NewFileName;
		public string insuredName = null;

		public string TableData;
		#endregion
		public string path;
		public upload_ajax_pingan()
		{
			
		}
		public void ProcessRequest(HttpContext context)
		{
            //取得处事类型
            //string action = DTRequest.GetQueryString("action");
            UpLoadFile(context);
            //context.Response.Write("hello");
		}

		#region 上传文件处理===================================
		private void UpLoadFile(HttpContext context)
		{
            //Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig();
            //string _delfile = DTRequest.GetString("DelFilePath");
            //HttpPostedFile _upfile = context.Request.Files["file"];
            HttpPostedFile _upfile = context.Request.Files[0];
			bool _iswater = false; //默认不打水印
			bool _isthumbnail = false; //默认不生成缩略图
			bool _isexcel = false;//默认不是excel模板上传

			//if (DTRequest.GetQueryString("IsExcel") == "1")
				_isexcel = true;
			//if (DTRequest.GetQueryString("IsWater") == "1")
				_iswater = true;
			//if (DTRequest.GetQueryString("IsThumbnail") == "1")
				_isthumbnail = true;
			if (_upfile == null)
			{
				context.Response.Write("{\"status\": 0, \"msg\": \"请选择要上传文件！\"}");
				return;
			}
			//UpLoad upFiles = new UpLoad();
			string msg = fileSaveAspingan(_upfile, _isthumbnail, _iswater, _isexcel);
			//删除已存在的旧文件，旧文件不为空且应是上传文件，防止跨目录删除
			//if (!string.IsNullOrEmpty(_delfile) && _delfile.IndexOf("..") == -1 && _delfile.ToLower().StartsWith(siteConfig.webpath.ToLower() + siteConfig.filepath.ToLower()))
			//{
				//Utils.DeleteUpFile(_delfile);
			//}
			//返回成功信息
			context.Response.Write(msg);
			context.Response.End();
		}
		#endregion

		/// <summary>
		/// 返回上传目录相对路径
		/// </summary>
		/// <param name="fileName">上传文件名</param>
		private string GetUpLoadPath()
		{
            //string path = siteConfig.webpath + siteConfig.filepath + "/"; //站点目录+上传目录
            string path = "";
			//switch (this.siteConfig.filesave)
            switch (1)
            {
				case 1: //按年月日每天一个文件夹
					path += DateTime.Now.ToString("yyyyMMdd");
					break;
				default: //按年月/日存入不同的文件夹
					path += DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.ToString("dd");
					break;
			}
			return path + "/";
		}

		/// <summary>
		/// 文件上传方法
		/// </summary>
		/// <param name="postedFile">文件流</param>
		/// <param name="isThumbnail">是否生成缩略图</param>
		/// <param name="isWater">是否打水印</param>
		/// <param name="isExcel">是否为模板上传</param>
		/// <returns>上传后文件信息</returns>
		public string fileSaveAspingan(HttpPostedFile postedFile, bool isThumbnail, bool isWater, bool isExcel = false)
		{
			try
			{
                //string fileExt = Utils.GetFileExt(postedFile.FileName); //文件扩展名，不含“.”
                string fileExt = "xls";
				int fileSize = postedFile.ContentLength; //获得文件大小，以字节为单位
				string fileName = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf(@"\") + 1); //取得原文件名
				//string newFileName = Utils.GetRamCode() + "." + fileExt; //随机生成新的文件名
                string newFileName=DateTime.Now.ToString("yyyyMMddhhmmss")+"."+fileExt;
				string newThumbnailFileName = "thumb_" + newFileName; //随机生成缩略图文件名
				string upLoadPath = GetUpLoadPath(); //上传目录相对路径
				//string fullUpLoadPath = Utils.GetMapPath(upLoadPath); //上传目录的物理路径
                string fullUpLoadPath="D:\\";
                string newfullUpLoadPath = fullUpLoadPath + newFileName; //上传的物理路径文件名
				string newFilePath = upLoadPath + newFileName; //上传后的路径
				string newThumbnailPath = upLoadPath + newThumbnailFileName; //上传后的缩略图路径

				//PAGroupInsInfo.Test();

				//检查上传的物理路径是否存在，不存在则创建
				if (!Directory.Exists(fullUpLoadPath))
				{
					Directory.CreateDirectory(fullUpLoadPath);
				}

				//保存文件
				postedFile.SaveAs(fullUpLoadPath + newFileName);

				if (isExcel)
				{
					//返回
					Workbook wb = new Workbook(fullUpLoadPath + newFileName);
					Cells cell = wb.Worksheets[0].Cells;
					Rootobject rootObject = new Rootobject();
					rootObject.status = 1;
					rootObject.msg = "上传文件成功";
					rootObject.name = "";
					decimal totalBf = 0;
					rootObject.extendjson = new List<Extendjson>();
					for (int i = 1; cell[i, 0].Value != null; i++)
					{
						Extendjson extendJson = new Extendjson();
						string ProfessionCode = cell[i, 3].StringValue;
						string ProfessionName = PAGroupInsInfo.GetProfessionNameByCode(ProfessionCode);
						int ProfessionLv = PAGroupInsInfo.GetProfessionLvByCode(ProfessionCode);

						extendJson.zylb = ProfessionCode;
						extendJson.zymc = ProfessionName;
						extendJson.btime = cell[i, 7].IntValue;

						int dutyAount;

						dutyAount = cell[i, 4].IntValue;
						extendJson.acciBe = dutyAount;
						extendJson.acciRoat = 0;
						extendJson.acciBf = PAGroupInsInfo.GetPrice(PAGroupInsInfo.PlanCodeList[0], ProfessionLv, dutyAount);

						dutyAount = cell[i, 5].IntValue;
						extendJson.medicalBe = dutyAount;
						extendJson.medicalRoat = 0;
						extendJson.medicalBf = PAGroupInsInfo.GetPrice(PAGroupInsInfo.PlanCodeList[1], ProfessionLv, dutyAount);

						dutyAount = cell[i, 6].IntValue;
						extendJson.allowanceBe = dutyAount;
						extendJson.allowanceRoat = 0;
						extendJson.allowanceBf = PAGroupInsInfo.GetPrice(PAGroupInsInfo.PlanCodeList[2], ProfessionLv, dutyAount);

						extendJson.totalBe = extendJson.acciBe + extendJson.allowanceBe + extendJson.medicalBe;
						extendJson.totalBf = extendJson.acciBf + extendJson.allowanceBf + extendJson.medicalBf;
						totalBf += extendJson.totalBf;
						extendJson.totalJf = 0;

						extendJson.id = cell[i, 0].IntValue;
						extendJson.name = cell[i, 1].StringValue;
						extendJson.idno = cell[i, 2].StringValue;
												
						rootObject.extendjson.Add(extendJson);
					}

					HttpContext.Current.Session["ExcelFilePath"] = fullUpLoadPath + newFileName;

					rootObject.savejson = "";
					rootObject.btime = 0;
					rootObject.totalBf = totalBf.ToString();
					rootObject.path = newFileName;
					rootObject.thumb = newThumbnailPath;
					rootObject.ext = "xls";
					rootObject.size = 0;
					return JsonConvert.SerializeObject(rootObject);
					// string a = "{\"status\":1,\"msg\":\"上传文件成功！\",\"name\":\"tb.xls\",\"extendjson\":[{\"id\":1,\"name\":\"沈默\",\"idno\":\"230805197910280217\",\"zylb\":\"0001001\",\"zymc\":\"机关内勤、公务员(不从事凶险工作)\",\"btime\":12,\"acciBe\":50000,\"acciRoat\":0.0004,\"acciBf\":20,\"medicalBe\":0,\"medicalRoat\":0.00128571,\"medicalBf\":0,\"allowanceBe\":0,\"allowanceRoat\":0.15933857,\"allowanceBf\":0,\"totalBf\":20000,\"totalBe\":50000,\"totalJf\":7},{\"id\":2,\"name\":\"沈默2\",\"idno\":\"230805197910280217\",\"zylb\":\"0001001\",\"zymc\":\"机关内勤、公务员(不从事凶险工作)\",\"btime\":12,\"acciBe\":50000,\"acciRoat\":0.0004,\"acciBf\":20,\"medicalBe\":0,\"medicalRoat\":0.00128571,\"medicalBf\":0,\"allowanceBe\":0,\"allowanceRoat\":0.15933857,\"allowanceBf\":0,\"totalBf\":20,\"totalBe\":50000,\"totalJf\":7},{\"id\":3,\"name\":\"沈默3\",\"idno\":\"230805197910280217\",\"zylb\":\"0001001\",\"zymc\":\"机关内勤、公务员(不从事凶险工作)\",\"btime\":12,\"acciBe\":50000,\"acciRoat\":0.0004,\"acciBf\":20,\"medicalBe\":0,\"medicalRoat\":0.00128571,\"medicalBf\":0,\"allowanceBe\":0,\"allowanceRoat\":0.15933857,\"allowanceBf\":0,\"totalBf\":20,\"totalBe\":50000,\"totalJf\":7}],\"savejson\":\"123\",\"btime\":12,\"totalBf\":\"60\",\"path\":\"/upload/201707/11/201707111240430764.xls\",\"thumb\":\"/upload/201707/11/201707111240430764.xls\",\"size\":22016,\"ext\":\"xls\"}";
					//return a;
					//处理完毕，返回JOSN格式的文件信息  

					
				}

				//处理完毕，返回JOSN格式的文件信息
				return "{\"status\": 1, \"msg\": \"上传文件成功！\", \"name\": \""
					+ fileName + "\", \"path\": \"" + newFilePath + "\", \"thumb\": \""
					+ newThumbnailPath + "\", \"size\": " + fileSize + ", \"ext\": \"" + fileExt + "\"}";
			}
			catch (Exception ex)
			{
				HttpContext.Current.Session["ExcelFilePath"] = null;
				return "{\"status\": 0, \"msg\": \"上传过程中发生意外错误！" + ex.Message + "\"}";
			}
		}


		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public class Rootobject
		{
			public int status { get; set; }
			public string msg { get; set; }
			public string name { get; set; }
			public List<Extendjson> extendjson { get; set; }
			public string savejson { get; set; }
			public int btime { get; set; }
			public string totalBf { get; set; }
			public string path { get; set; }
			public string thumb { get; set; }
			public int size { get; set; }
			public string ext { get; set; }
		}

		public class Extendjson
		{
			public int id { get; set; }
			public string name { get; set; }
			public string idno { get; set; }
			public string zylb { get; set; }
			public string zymc { get; set; }
			public int btime { get; set; }
			public int acciBe { get; set; }
			public float acciRoat { get; set; }
			public decimal acciBf { get; set; }
			public int medicalBe { get; set; }
			public float medicalRoat { get; set; }
			public decimal medicalBf { get; set; }
			public int allowanceBe { get; set; }
			public float allowanceRoat { get; set; }
			public decimal allowanceBf { get; set; }
			public decimal totalBf { get; set; }
			public int totalBe { get; set; }
			public decimal totalJf { get; set; }
		}

	}
}