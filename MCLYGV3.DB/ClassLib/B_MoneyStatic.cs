using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCLYGV3.DB
{
    public partial class B_MoneyStatic
    {
        /// <summary>
        /// 统计数据录入
        /// </summary>
        /// <returns></returns>
        public bool AddMoneyStatic(string poliyNo,int userId,decimal amount,string productName,out string errorMessage)
        {
            errorMessage = "";
            var user = B_UserInfo.Find(userId);
            int company = 0;
            if (user==null)
            {
                errorMessage = "该用户不存在";
                return false;
            }
            var moneyStatic=new M_MoneyStatic()
            {
                Money = amount,
                CompanyId = user.InCompany.ID,
                UserId = userId,
                
            };
            return true;
        }
    }
}
