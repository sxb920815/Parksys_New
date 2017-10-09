using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using MCLYGV3.DB;
using MCLYGV3.Web.Models;
using Newtonsoft.Json;
using Aspose.Cells;

namespace MCLYGV3.Web.Controllers
{
    public class UpLoadController : ApiController
    {
        [HttpPost]
        public DataJsonResult UploadFiles()
        {
            string requestString = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
            var obj = JsonConvert.DeserializeObject<FileModelRequest>(requestString);
            string fileName = obj.FileName;
            string data = obj.Data;
            ResultFile resultFile;
            var res = new DataJsonResult();
            if (DB.B_SysFile.SaveFiles(fileName, data, out resultFile))
            {
                res.Data = resultFile;
                return res;
            }
            res.ErrorMessage = "上传失败";
            res.ReturnCode = "0001";
            return res;
        }

        [HttpPost]
        public DataJsonResult UploadExcel()
        {
            var res = new DataJsonResult();
            string requestString = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
            var obj = JsonConvert.DeserializeObject<FileModelRequest>(requestString);
            ResultFile resultFile;
            if (DB.B_SysFile.SaveFiles(obj.FileName, obj.Data, out resultFile))
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + resultFile.Url;
                try
                {
                    Workbook wb = new Workbook(path);
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
                        string ProfessionName=null;
                        int ProfessionLv = 0;
                        if (obj.InsuranceCompany == "PA")
                        {
                             ProfessionName = PAGroupInsInfo.GetProfessionNameByCode(ProfessionCode);
                             ProfessionLv = PAGroupInsInfo.GetProfessionLvByCode(ProfessionCode);
                        }
                        if (obj.InsuranceCompany == "CT")
                        {
                             ProfessionName = CTGroupInsInfo.GetProfessionName(ProfessionCode);
                            ProfessionLv = CTGroupInsInfo.GetProfessionLv(ProfessionCode);
                        }

                        extendJson.zylb = ProfessionCode;
                        extendJson.zymc = ProfessionName;
                        if(string.IsNullOrWhiteSpace(cell[i,7].StringValue))
                        {
                            extendJson.btime = 0;
                        }
                        else
                        {
                            extendJson.btime = Convert.ToInt16(cell[i, 7].StringValue);
                        }

                        int dutyAount;
                        dutyAount = cell[i, 4].IntValue;
                        extendJson.acciBe = dutyAount;
                        dutyAount = cell[i, 5].IntValue;
                        extendJson.medicalBe = dutyAount;
                        dutyAount = cell[i, 6].IntValue;
                        extendJson.allowanceBe = dutyAount;
                        extendJson.totalBe = extendJson.acciBe + extendJson.allowanceBe + extendJson.medicalBe;
                        extendJson.id = cell[i, 0].IntValue;
                        extendJson.name = cell[i, 1].StringValue;
                        extendJson.idno = cell[i, 2].StringValue;

                        rootObject.extendjson.Add(extendJson);
                    }
                    rootObject.savejson = "";

                    rootObject.totalBf = totalBf.ToString();
                    rootObject.path = path;
                    rootObject.thumb = "";
                    rootObject.ext = "xls";
                    rootObject.size = 0;
                    res.Data = rootObject;
                    return res;

                }
                catch (Exception ex)
                {
                    res.ReturnCode = "601";
                    res.ErrorMessage = ex.Message;
                    return res;
                }
                // string a = "{\"status\":1,\"msg\":\"上传文件成功！\",\"name\":\"tb.xls\",\"extendjson\":[{\"id\":1,\"name\":\"沈默\",\"idno\":\"230805197910280217\",\"zylb\":\"0001001\",\"zymc\":\"机关内勤、公务员(不从事凶险工作)\",\"btime\":12,\"acciBe\":50000,\"acciRoat\":0.0004,\"acciBf\":20,\"medicalBe\":0,\"medicalRoat\":0.00128571,\"medicalBf\":0,\"allowanceBe\":0,\"allowanceRoat\":0.15933857,\"allowanceBf\":0,\"totalBf\":20000,\"totalBe\":50000,\"totalJf\":7},{\"id\":2,\"name\":\"沈默2\",\"idno\":\"230805197910280217\",\"zylb\":\"0001001\",\"zymc\":\"机关内勤、公务员(不从事凶险工作)\",\"btime\":12,\"acciBe\":50000,\"acciRoat\":0.0004,\"acciBf\":20,\"medicalBe\":0,\"medicalRoat\":0.00128571,\"medicalBf\":0,\"allowanceBe\":0,\"allowanceRoat\":0.15933857,\"allowanceBf\":0,\"totalBf\":20,\"totalBe\":50000,\"totalJf\":7},{\"id\":3,\"name\":\"沈默3\",\"idno\":\"230805197910280217\",\"zylb\":\"0001001\",\"zymc\":\"机关内勤、公务员(不从事凶险工作)\",\"btime\":12,\"acciBe\":50000,\"acciRoat\":0.0004,\"acciBf\":20,\"medicalBe\":0,\"medicalRoat\":0.00128571,\"medicalBf\":0,\"allowanceBe\":0,\"allowanceRoat\":0.15933857,\"allowanceBf\":0,\"totalBf\":20,\"totalBe\":50000,\"totalJf\":7}],\"savejson\":\"123\",\"btime\":12,\"totalBf\":\"60\",\"path\":\"/upload/201707/11/201707111240430764.xls\",\"thumb\":\"/upload/201707/11/201707111240430764.xls\",\"size\":22016,\"ext\":\"xls\"}";
                //return a;
                //处理完毕，返回JOSN格式的文件信息  

            }
           
                    res.ErrorMessage = "Error 2";
                    return res;
        }

    }
    public class FileModelRequest
    {
        public string FileName { get; set; }
        public string InsuranceCompany { get; set; }
        public string Data { get; set; }
    }
    public class Rootobject
    {
        public int status { get; set; }
        public string msg { get; set; }
        public string name { get; set; }
        public List<Extendjson> extendjson { get; set; }
        public string savejson { get; set; }
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
        public int medicalBe { get; set; }
        public int allowanceBe { get; set; }
        public int totalBe { get; set; }
    }
}
