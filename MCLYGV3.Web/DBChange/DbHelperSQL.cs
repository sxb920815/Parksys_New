/*
 * 类名：数据库操作类
 *	
 *		剔除了一些效率低下的而且不常用的方法
 *		最好配合DotNetSQLHelper软件自动生成的那些DAL、BLL、Model使用的。
 *		也可以作为数据库操作类，单独使用。
 *
 *		DotNetSQLHelper软件由　默默　开发
 *
 *		Email： wwwmomo@126.com
 *		QQ:32561 
 *
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Web;
using System.Text;
using System.IO;
namespace MCLYGV3.Web
{
    /// <summary>
    /// 数据访问抽象基础类
    /// </summary>
    public abstract class DbHelperSQL
    {
        /* 
         * 数据库连接字符串。
         * 可以用任意的方法，比如web.config或者自定义xml来配置，或者写死在里面
        */
        public static string connStr;
        static DbHelperSQL()
        {
            //一定要给connStr赋值


            string Server = "90.63.1.7";//SQL Server服务器的IP地址
            string DBName = "DTcmsdb";
            string UserName = "sa";
            string PassWord = "123456";
			//server=.;uid=sa;pwd=cptbtptp47!@#$;database=DTcmsdb;

			/*

            string Server = ".";//SQL Server服务器的IP地址
            string DBName = "BDJL";
            string UserName = "sa";
            string PassWord = "123456";
            * */
			/*
              * 
              * 
             string file = HttpContext.Current.Server.MapPath(@"~\App_Data\system.xml");
             XmlNode node = FileToXmlNode(file, "root").SelectSingleNode("ConnectionString");
             string Server = node.SelectSingleNode("Server").InnerText;
             string DBName = node.SelectSingleNode("DBName").InnerText;
             string UserName = node.SelectSingleNode("UserName").InnerText;
             string PassWord = node.SelectSingleNode("PassWord").InnerText;
             */
			connStr = string.Format("Data Source={0};Initial Catalog={1};User ID={2};pwd={3};", Server, DBName, UserName, PassWord);
            //File.AppendAllText(@"E:\123.txt", connStr);
        }
        /// <summary>
        /// 解析Xml文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="root">根标签</param>
        /// <returns></returns>
        private static XmlNode FileToXmlNode(string fileName, string root)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                XmlNode node = xmlDoc.SelectSingleNode(root);
                return node;
            }
            catch
            {
                return null;
            }
        }

        #region SqlBulkCopy——批量导数据
        /// <summary>
        /// 批量将DataTable中的内容导入到数据库。dt的结构必须与数据库的表一致。
        /// </summary>
        /// <param name="dt">导入的DataTable</param>
        /// <param name="TableName">数据库中的表名</param>
        public static void SqlBulkCopy(DataTable dt, string TableName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    connection.Open();
                    using (SqlBulkCopy bcp = new SqlBulkCopy(connection))
                    {
                        bcp.DestinationTableName = TableName;
                        bcp.WriteToServer(dt);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
        #endregion

        #region 返回查询结果是否有数据
        /// <summary>
        /// 返回一个查询结果，是否有数据
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public static bool Exists(string sql)
        {
            bool rebool;
            try
            {
                using (SqlConnection myconn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, myconn))
                    {
                        myconn.Open();
                        using (SqlDataReader myReader = cmd.ExecuteReader())
                        {
                            if (myReader.Read())
                            {
                                rebool = true;
                            }
                            else
                            {
                                rebool = false;
                            }
                            return rebool;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "\r\n" + sql);
            }
        }
        /// <summary>
        /// 返回一个查询结果，是否有数据
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdParms">参数</param>
        /// <returns></returns>
        public static bool Exists(string sql, params SqlParameter[] cmdParms)
        {
            bool rebool;
            try
            {
                using (SqlConnection myconn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, myconn, null, sql, cmdParms);
                        SqlDataReader myReader = cmd.ExecuteReader();
                        if (myReader.Read())
                        {
                            rebool = true;
                        }
                        else
                        {
                            rebool = false;
                        }
                        return rebool;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// 执行无返回结果的SQL语句，返回执行是否成功
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static bool ExecuteNonQuery(string sql)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 执行无返回结果的SQL语句，返回执行是否成功
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="cmdParms">参数</param>
        /// <returns></returns>
        public static bool ExecuteNonQuery(string sql, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, sql, cmdParms);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }
        #endregion

        #region ExecuteScalar
        /// <summary>
        /// 执行一条计算查询结果语句，返回第一行第一列（object）。
        /// </summary>
        /// <param name="sql">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object ExecuteScalar(string sql)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message + sql);
                    }
                }
            }
        }
        /// <summary>
        /// 执行一条计算查询结果语句，返回第一行第一列（object）。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="cmdParms">参数</param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, sql, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }
        #endregion

        #region ExecuteDataSet



        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataSet(string SQLString, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            strSql.Append("order by T." + orderby);
            strSql.Append(")AS Row, T.*  from (" + SQLString + ") T ");
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.ExecuteDataSet(strSql.ToString());
        }



        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="cmdParms">参数</param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string sql, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, sql, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                    return ds;
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataSet(string sql)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(sql, connection);
                    command.Fill(ds, "ds");
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message + sql);
                }
                return ds;
            }
        }

        #endregion

        #region ExecuteReader
        /// <summary>
        /// 执行查询语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string sql)
        {
            SqlConnection connection = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand(sql, connection);
            try
            {
                connection.Open();
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// 执行查询语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="cmdParms">参数</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string sql, params SqlParameter[] cmdParms)
        {
            SqlConnection connection = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, sql, cmdParms);
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="Par">参数</param>
        /// <param name="OutFaildName">返回值字段</param>
        /// <returns>存储过程返回值</returns>
        public static string ExecuteProcReturn(string ProcName, SqlParameter[] Par, string OutFaildName)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandText = ProcName;
                comm.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < Par.Length; i++)
                {
                    comm.Parameters.Add(Par[i]);
                }
                Par[Par.Length - 1].Direction = ParameterDirection.ReturnValue;
                comm.ExecuteNonQuery();
                return Convert.ToString(comm.Parameters[OutFaildName].Value);
            }

        }

        #region 公共函数
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }
        #endregion

        #region 执行多条SQL语句，实现数据库事务。
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public static bool ExecuteSqlTran(List<String> SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    foreach (string sql in SQLStringList)
                    {
                        if (string.IsNullOrEmpty(sql) == false && sql != "")
                        {
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return true;
                }
                catch
                {
                    tx.Rollback();
                    return false;
                }
            }
        }
        #endregion
    }
}