using MCLYGV3.DB;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Aspose.Cells;

namespace MCLYGV3.Web.Controllers
{
	public partial class AdministratorController : AdministratorControll
	{

		#region 订单

		public ActionResult Order_List()
		{
			return View(MyUser);
		}
		public ActionResult Order_DownLoad()
		{
			List<M_UserInfo> userList = B_UserInfo.GetList(t => true);

			var selectList = new SelectList(userList, "ID", "UserName");
			var selectItemList = new List<SelectListItem>() {
				new SelectListItem(){Value="All",Text="全部用户",Selected=true}
			};
			selectItemList.AddRange(selectList);
			ViewBag.UserList = selectItemList;
			return View(MyUser);
		}
		public ActionResult Order_Add()
		{
			return View();
		}
		public ActionResult Order_Detail(string OrderCode)
		{
			M_Order obj = B_Order.Find(OrderCode);
			return View(obj);
		}
		public ActionResult Order_Edit(string OrderCode)
		{
			M_Order obj = B_Order.Find(OrderCode);
			return View(obj);
		}

		[HttpPost]
		public string DelOrder(string OrderCode)
		{
			JsonMessage result;

			bool bol = B_Order.Del(OrderCode);

			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = "" };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = "" };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string EditOrder()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_Order obj = JsonConvert.DeserializeObject<M_Order>(req);
			bool bol = B_Order.Update(obj);
			if (bol)
				result = new JsonMessage() { type = 0, message = "成功", value = req };
			else
				result = new JsonMessage() { type = -1, message = "失败", value = req };

			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		[HttpPost]
		public string AddOrder()
		{
			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string req = Encoding.UTF8.GetString(byts);

			M_Order obj = JsonConvert.DeserializeObject<M_Order>(req);
			obj = B_Order.Add(obj);
			result = new JsonMessage() { type = 0, message = "成功", value = req };
			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";
			return JsonConvert.SerializeObject(result);
		}

		public string GetOrderList(GridPager pager, string queryStr, string policy)
		{
			using (var db = new DBContext())
			{
				List<M_Order> list;
				int count;
				if (string.IsNullOrEmpty(queryStr) && string.IsNullOrEmpty(policy))
				{
					list = B_Order.GetListByPage(t => 1 == 1, pager);
					count = B_Order.GetCount(t => 1 == 1);
				}
				else if (string.IsNullOrEmpty(policy))
				{
					list = B_Order.GetListByPage(t => t.OrderCode.Contains(queryStr), pager);
					count = B_Order.GetCount(t => t.OrderCode.Contains(queryStr));
				}
				else if (string.IsNullOrEmpty(queryStr))
				{
					list = B_Order.GetListByPage(t => t.PolicyNo.Contains(policy), pager);
					count = B_Order.GetCount(t => t.PolicyNo.Contains(policy));
				}
				else
				{
					list = B_Order.GetListByPage(t => t.PolicyNo.Contains(policy) && t.OrderCode.Contains(queryStr), pager);
					count = B_Order.GetCount(t => t.PolicyNo.Contains(policy) && t.OrderCode.Contains(queryStr));
				}
				GridRows<M_Order> grs = new GridRows<M_Order>();
				grs.rows = list;
				grs.total = count;
				Response.ContentType = "application/json";
				Response.Charset = "UTF-8";
				IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
				timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
				return JsonConvert.SerializeObject(grs, Formatting.Indented, timeFormat);
			}

		}
		public string ExcelDownLoadByCompany()
		{
			Response.ContentType = "application/json";
			Response.Charset = "UTF-8";

			JsonMessage result;
			byte[] byts = new byte[Request.InputStream.Length];
			Request.InputStream.Read(byts, 0, byts.Length);
			string reqStr = Encoding.UTF8.GetString(byts);
			ExcelDownLoadByCompanyReq req;
			try
			{
				req = JsonConvert.DeserializeObject<ExcelDownLoadByCompanyReq>(reqStr);
			}
			catch (Exception)
			{
				result = new JsonMessage() { type = -1, message = "数据转换失败", value = "" };
				return JsonConvert.SerializeObject(result);
			}
			//bool bol = false;
			//if (bol)
			//	result = new JsonMessage() { type = 0, message = "成功", value = "" };
			//else
			//	result = new JsonMessage() { type = -1, message = "失败", value = "" };
            try
            {
                DateTime start = DateTime.Parse(req.startTime);
                DateTime end = DateTime.Parse(req.endTime);
                List<M_Order> orderList = null;
                if (req.companySelect == "All")
                {
                    orderList = B_Order.GetList(t => t.BuyTime > start && t.BuyTime < end);
                }
                else
                {
                    orderList = B_Order.GetList(t => t.BuyTime > start && t.BuyTime < end && t.InsuranceCompany == req.companySelect);
                }
                string path = AppDomain.CurrentDomain.BaseDirectory;
                Workbook wb = new Workbook($"{path}\\PageBrace\\OrderTemplate.xls");
                Worksheet ws = wb.Worksheets[0];
                Cells cells = ws.Cells;
                int 当前写到第几行 = 1;
                foreach (var list in orderList)
                {
                    if (当前写到第几行 > 1)
                    {
                        cells.CopyRow(cells, 0, 当前写到第几行);
                        当前写到第几行++;
                        cells.CopyRow(cells, 1, 当前写到第几行);
                    }
                    cells[当前写到第几行, 0].PutValue(list.OrderCode);
                    cells[当前写到第几行, 1].PutValue(B_UserInfo.Find(list.UserId).UserName);
                    cells[当前写到第几行, 2].PutValue(list.InsuredName);
                    cells[当前写到第几行, 3].PutValue(list.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    cells[当前写到第几行, 4].PutValue(list.EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    cells[当前写到第几行, 5].PutValue(list.FirstModalPremium);
                    cells[当前写到第几行, 6].PutValue(list.NowModalPremium);
                    decimal 净保费 = list.NowModalPremium / (decimal)1.06;
                    cells[当前写到第几行, 7].PutValue(decimal.Round(净保费, 2));
                    cells[当前写到第几行, 8].PutValue("65 %");
                    decimal 手续费 = 净保费 / (decimal)0.65;
                    cells[当前写到第几行, 9].PutValue(decimal.Round(手续费, 2));
                    decimal 发票金额 = 手续费 * (decimal)(1 + 0.03);
                    cells[当前写到第几行, 10].PutValue(decimal.Round(发票金额, 2));
                    cells[当前写到第几行, 11].PutValue(list.PolicyNo);
                    当前写到第几行++;
                    if (当前写到第几行 > 3)
                    {
                        cells.CopyRow(cells, 2, 当前写到第几行);
                    }
                    当前写到第几行++;
                    List<M_OrderPersion> personList = B_OrderPersion.GetList(t => t.OrderCode == list.OrderCode);
                    foreach (var person in personList)
                    {
                        if (当前写到第几行 > 3)
                        {
                            cells.CopyRow(cells, 3, 当前写到第几行);
                        }
                        cells[当前写到第几行, 1].PutValue(person.RealName);
                        cells[当前写到第几行, 2].PutValue(person.IdNum);
                        cells[当前写到第几行, 3].PutValue(person.ProfessionCode);
                        cells[当前写到第几行, 4].PutValue(person.ProfessionName);
                        cells[当前写到第几行, 5].PutValue(person.AcciDutyAount);
                        cells[当前写到第几行, 6].PutValue(person.AcciPremium);
                        cells[当前写到第几行, 7].PutValue(person.MedicalDutyAount);
                        cells[当前写到第几行, 8].PutValue(person.MedicalPremium);
                        cells[当前写到第几行, 9].PutValue(person.AllowanceDutyAount);
                        cells[当前写到第几行, 10].PutValue(person.AllowancePremium);
                        cells[当前写到第几行, 11].PutValue(person.AcciPremium + person.MedicalPremium + person.AllowancePremium);
                        当前写到第几行++;
                    }
                    当前写到第几行++;
                }
                string fileName = DateTime.Now.ToString("yyyy-MM-ddHHmmss") + ".xls";
                string fullPath = $"{path}\\TmpDownload\\{fileName}";
                wb.Save(fullPath);
                result = new JsonMessage() { type = 0, message = "成功", value = $"/TmpDownload/{fileName}" };
            }
            catch(Exception e)
            {
                result = new JsonMessage() { type = -1, message = e.Message, value = "" };
            }
            return JsonConvert.SerializeObject(result);

        }
		public string ExcelDownLoadByName()
		{
            Response.ContentType = "application/json";
            Response.Charset = "UTF-8";

            JsonMessage result;
            byte[] byts = new byte[Request.InputStream.Length];
            Request.InputStream.Read(byts, 0, byts.Length);
            string reqStr = Encoding.UTF8.GetString(byts);
            ExcelDownLoadByNameReq req;
            try
            {
                req = JsonConvert.DeserializeObject<ExcelDownLoadByNameReq>(reqStr);
            }
            catch (Exception)
            {
                result = new JsonMessage() { type = -1, message = "数据转换失败", value = "" };
                return JsonConvert.SerializeObject(result);
            }
            try
            {
                DateTime start = DateTime.Parse(req.startTime);
                DateTime end = DateTime.Parse(req.endTime);
                List<M_Order> orderList = null;
                if (req.user== "All")
                {
                    orderList = B_Order.GetList(t => t.BuyTime > start && t.BuyTime < end);
                }
                else
                {
                    int userId = Convert.ToInt16(req.user);
                    orderList = B_Order.GetList(t => t.BuyTime > start && t.BuyTime < end && t.UserId == userId);
                }
                string path = AppDomain.CurrentDomain.BaseDirectory;
                Workbook wb = new Workbook($"{path}/PageBrace/OrderTemplate.xls");
                Worksheet ws = wb.Worksheets[0];
                Cells cells = ws.Cells;
                int 当前写到第几行 = 1;
                foreach (var list in orderList)
                {
                    if (当前写到第几行 > 1)
                    {
                        cells.CopyRow(cells, 0, 当前写到第几行);
                        当前写到第几行++;
                        cells.CopyRow(cells, 1, 当前写到第几行);
                    }
                    cells[当前写到第几行, 0].PutValue(list.OrderCode);
                    cells[当前写到第几行, 1].PutValue(B_UserInfo.Find(list.UserId).UserName);
                    cells[当前写到第几行, 2].PutValue(list.InsuredName);
                    cells[当前写到第几行, 3].PutValue(list.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    cells[当前写到第几行, 4].PutValue(list.EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    cells[当前写到第几行, 5].PutValue(list.FirstModalPremium);
                    cells[当前写到第几行, 6].PutValue(list.NowModalPremium);
                    decimal 净保费 = list.NowModalPremium / (decimal)1.06;
                    cells[当前写到第几行, 7].PutValue(decimal.Round(净保费, 2));
                    cells[当前写到第几行, 8].PutValue("65 %");
                    decimal 手续费 = 净保费 / (decimal)0.65;
                    cells[当前写到第几行, 9].PutValue(decimal.Round(手续费, 2));
                    decimal 发票金额 = 手续费 * (decimal)(1 + 0.03);
                    cells[当前写到第几行, 10].PutValue(decimal.Round(发票金额, 2));
                    cells[当前写到第几行, 11].PutValue(list.PolicyNo);
                    当前写到第几行++;
                    if (当前写到第几行 > 3)
                    {
                        cells.CopyRow(cells, 2, 当前写到第几行);
                    }
                    当前写到第几行++;
                    List<M_OrderPersion> personList = B_OrderPersion.GetList(t => t.OrderCode == list.OrderCode);
                    foreach (var person in personList)
                    {
                        if (当前写到第几行 > 3)
                        {
                            cells.CopyRow(cells, 3, 当前写到第几行);
                        }
                        cells[当前写到第几行, 1].PutValue(person.RealName);
                        cells[当前写到第几行, 2].PutValue(person.IdNum);
                        cells[当前写到第几行, 3].PutValue(person.ProfessionCode);
                        cells[当前写到第几行, 4].PutValue(person.ProfessionName);
                        cells[当前写到第几行, 5].PutValue(person.AcciDutyAount);
                        cells[当前写到第几行, 6].PutValue(person.AcciPremium);
                        cells[当前写到第几行, 7].PutValue(person.MedicalDutyAount);
                        cells[当前写到第几行, 8].PutValue(person.MedicalPremium);
                        cells[当前写到第几行, 9].PutValue(person.AllowanceDutyAount);
                        cells[当前写到第几行, 10].PutValue(person.AllowancePremium);
                        cells[当前写到第几行, 11].PutValue(person.AcciPremium + person.MedicalPremium + person.AllowancePremium);
                        当前写到第几行++;
                    }
                    当前写到第几行++;
                }
                string fileName = DateTime.Now.ToString("yyyy-MM-ddHHmmss") + ".xls";
                string fullPath = $"{path}/TmpDownload/{fileName}";
                wb.Save(fullPath);
                result = new JsonMessage() { type = 0, message = "成功", value = $"/TmpDownload/{fileName}" };
            }catch(Exception e)
            {
                result = new JsonMessage() { type = -1, message = e.Message, value = "" };
            }
            return JsonConvert.SerializeObject(result);
		}
		#endregion
	}
	public class ExcelDownLoadByCompanyReq
	{
		public string startTime { get; set; }
		public string endTime { get; set; }
		public string companySelect { get; set; }

	}
    public class ExcelDownLoadByNameReq
    {
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string user { get; set; }
    }
}
