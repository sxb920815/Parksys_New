using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCLYGV3.DB
{
    public partial class B_UserInfo
    {
        /// <summary>
        /// 获取最大Id
        /// </summary>
        /// <returns></returns>
        public static int GetMaxId()
        {
            using (DBContext db = new DBContext())
            {
                int maxId = 0;
                if (db.UserInfoList.Any())
                {
                    maxId = db.UserInfoList.Max(x => x.ID);

                }
                return maxId;
            }
        }
    }
}
