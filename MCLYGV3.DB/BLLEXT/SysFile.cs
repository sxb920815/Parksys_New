using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;

namespace MCLYGV3.DB
{

	/// <summary>
	/// 文件上传数据库操作类
	/// </summary>
	public partial class B_SysFile
	{
        public static bool SaveFiles(string fileName, string data,out ResultFile resultFile)
        {
            string[] imageArrty = data.Split(',');
            string imgBase64Data = imageArrty[1];
            string base64 = imgBase64Data;
            string dummyData = base64.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+");
            if (dummyData.Length % 4 > 0)
            {
                dummyData = dummyData.PadRight(dummyData.Length + 4 - dummyData.Length % 4, '=');
            }
            byte[] bytes = Convert.FromBase64String(dummyData);
            var fileStream = new MemoryStream(bytes);
            try
            {
                var fileSize = fileStream.Length;
                if (fileSize > 0)
                {
                    var fileId = Guid.NewGuid();
                    string relativePath;
                    var savePath = GetFolderPath(out relativePath);
                    var fileExtension = Path.GetExtension(fileName)?.ToLower();
                    int imageWidth = 0, imageHeight = 0;
                    //保存到文件
                    var path = $"{savePath}\\{fileId}{fileExtension}";
                    if (".jpg,.png,.jpeg".Contains(fileExtension))
                    {
                        //取长宽
                        System.Drawing.Image tempimage = System.Drawing.Image.FromStream(fileStream, true);
                        imageWidth = tempimage.Width;//宽
                        imageHeight = tempimage.Height;//高
                        tempimage.Save(path);
                    }
                    else
                    {
                        fileStream.Position = 0;
                        StreamWriter sw = new StreamWriter(path);
                        fileStream.CopyTo(sw.BaseStream);
                        sw.Flush();
                        sw.Close();
                        fileStream.Dispose();
                    }
                    var files=DB.B_SysFile.Add(fileId.ToString(), fileName, fileExtension, $"\\{relativePath}\\{fileId}{fileExtension}", "", DateTime.Now, "", "");
                    resultFile = new ResultFile()
                    {
                        Id = files.ID,
                        Url = files.Url
                    };
                    return true;
                }
                resultFile = null;
                return false;
            }
            catch (Exception ex)
            {
                resultFile = null;
                return false;
            }
        }

        private static string GetFolderPath(out string relativePath)
        {
            relativePath = $"StorageFiles\\{DateTime.Now.Year}\\{DateTime.Now.Month}\\{DateTime.Now.Day}";
            string path =
                $"{AppDomain.CurrentDomain.BaseDirectory}/{relativePath}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }

    public class ResultFile
    {
        public string Id { get; set; }
        public string Url { get; set; }
    }
}