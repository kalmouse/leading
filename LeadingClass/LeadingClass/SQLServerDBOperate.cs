using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Configuration;


namespace LeadingClass
{

    /// <summary>
    /// DBOperate 的摘要说明。
    /// </summary>
    public interface IDb
    {
        DataSet GetDataSet(string sql);

        bool DeleteAt(string tableName, SqlParameter[] paramArray);
        bool ExecuteNonQuery(string sql);
        bool OperateData(string sql, SqlParameter[] myParamArray);
        int InsertData(string tableName, SqlParameter[] paramArray);
        /// <summary>
        /// update表
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="paramArray">表中的字段参数，第一个必须是要更新的记录的主键</param>
        bool UpdateData(string tableName, SqlParameter[] paramArray);
    }

    /// <summary>
    /// sql server 数据库通用操作类
    /// </summary>
    [Serializable()]
    public class DBOperate : IDb
    {
        private string m_ConnectionString;

        /// <summary>
        /// Property ConnectionString (string)
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this.m_ConnectionString;
            }
            set
            {
                this.m_ConnectionString = value;
            }
        }

        public DBOperate()
        {
            try
            {
                m_ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            }
            catch
            {
                try
                {
                    m_ConnectionString = System.Configuration.ConfigurationManager.AppSettings["connectionString"].ToString();
                }
                catch { }
            }
        }

        public DBOperate(string connetctionString)
        {
            m_ConnectionString = connetctionString;
        }
        #region 通用方法
        /// <summary>
        /// 测试连接是否可用
        /// </summary>
        /// <returns></returns>
        public bool TestConnection()
        {
            SqlConnection sqlConn = new SqlConnection(m_ConnectionString);
            try
            {
                sqlConn.Open();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                sqlConn.Dispose();
            }
        }
        public DataSet GetDataSet(string sql)
        {
            DataSet ds = new DataSet();
            SqlConnection sqlConn = null;
            SqlDataAdapter adapter = null;
            try
            {
                sqlConn = new SqlConnection(m_ConnectionString);
                sqlConn.Open();
                adapter = new SqlDataAdapter(sql, sqlConn);
                adapter.Fill(ds,"ds");
                return ds;
            }
            finally
            {
                if (adapter != null)
                {
                    adapter.Dispose();
                }
                if (sqlConn != null)
                {
                    sqlConn.Dispose();
                }
                sqlConn.Close();
            }
        }
       
        public DataSet GetDataSet(string sql, int StartRecord, int MaxRecord)
        {
            DataSet ds = new DataSet();
            SqlConnection sqlConn = null;
            SqlDataAdapter adapter = null;
            try
            {
                sqlConn = new SqlConnection(m_ConnectionString);
                sqlConn.Open();
                adapter = new SqlDataAdapter(sql, sqlConn);
                adapter.Fill(ds, StartRecord, MaxRecord, "ResultTable");
                return ds;
            }
            finally
            {
                if (adapter != null)
                {
                    adapter.Dispose();
                }
                if (sqlConn != null)
                {
                    sqlConn.Dispose();
                }
            }
        }
        public bool OperateData(string sql, SqlParameter[] myParamArray)
        {
            SqlConnection sqlConn = new SqlConnection(m_ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, sqlConn);
            try
            {
                for (int j = 0; j < myParamArray.Length; j++)
                {
                    cmd.Parameters.Add(myParamArray[j]);
                }
                sqlConn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            finally
            {
                cmd.Dispose();
                sqlConn.Dispose();
            }
        }
        public bool ExecuteNonQuery(string sql)
        {
            SqlConnection sqlConn = new SqlConnection(m_ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, sqlConn);
            try
            {
                sqlConn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Sys_ErrorLog errorlog = new Sys_ErrorLog();
                string m = sql + " " + e.Message;
                errorlog.ErrorMessage = m.Substring(0, 1999);
                errorlog.UpdateTime = DateTime.Now;
                errorlog.Save();
                return false;
            }
            finally
            {
                cmd.Dispose();
                sqlConn.Dispose();
            }
        }
        //处理事务日志
        public int ExecNoQuerySqlTran(List<string> sql)
        {
            //创建数据库连接对象
            SqlConnection sqlConn = new SqlConnection(m_ConnectionString);
            //打开连接
            sqlConn.Open();
            int l = 0;
            SqlTransaction tran = sqlConn.BeginTransaction();
            try
            {
                for (int k = 0; k < sql.Count; k++)
                {
                    //实例化命令对象
                    SqlCommand cmd = new SqlCommand(sql[k], sqlConn, tran);

                    //执行命令
                    l = cmd.ExecuteNonQuery();
                }
                //提交处理到数据库
                tran.Commit();
                sqlConn.Close();
            }
            catch
            {
                //回滚事物处理 取消之前的操作
                tran.Rollback();
            }
            return l;
        }
        public object ExecuteScalar(string sql)
        {
            SqlConnection sqlConn = new SqlConnection(m_ConnectionString);
            SqlCommand cmd = new SqlCommand(sql, sqlConn);
            try
            {
                sqlConn.Open();
                return cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                Sys_ErrorLog errorlog = new Sys_ErrorLog();
                errorlog.TableName = e.Message;
                errorlog.ErrorMessage = e.Message;
                errorlog.UpdateTime = DateTime.Now;
                errorlog.Save();
                return 0;
            }
            finally
            {
                cmd.Dispose();
                sqlConn.Dispose();
            }

        }

        public bool DeleteAt(string tableName, SqlParameter[] paramArray)
        {
            SqlConnection sqlConn = new SqlConnection(m_ConnectionString);
            SqlCommand cmd = new SqlCommand(GetDeleteSql(tableName, paramArray), sqlConn);
            try
            {
                sqlConn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                cmd.Dispose();
                sqlConn.Dispose();
            }
        }

        public int InsertData(string tableName, SqlParameter[] paramArray)
        {
            SqlConnection sqlConn = new SqlConnection(m_ConnectionString);
            string insertsql = GetInsertSql(tableName, paramArray);
            SqlCommand cmd = new SqlCommand(insertsql, sqlConn);
            try
            {
                for (int j = 0; j < paramArray.Length; j++)
                {
                    cmd.Parameters.Add(paramArray[j]);
                }
                sqlConn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception e)
            {
                Sys_ErrorLog errorlog = new Sys_ErrorLog();
                errorlog.TableName = tableName;
                errorlog.ErrorMessage = e.Message;
                errorlog.UpdateTime = DateTime.Now;
                errorlog.Save();
                return 0;
            }
            finally
            {
                cmd.Dispose();
                sqlConn.Dispose();
            }
        }

        public bool UpdateData(string tableName, SqlParameter[] paramArray)
        {
            SqlConnection sqlConn = new SqlConnection(m_ConnectionString);
            sqlConn.Open();
            string updatesql = GetUpdateSql(tableName, paramArray);
            SqlCommand cmd = new SqlCommand(updatesql, sqlConn);
            try
            {
                for (int j = 0; j < paramArray.Length; j++)
                {
                    cmd.Parameters.Add(paramArray[j]);
                }

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                string msg = e.Message;
                //记录错误日志
                Sys_ErrorLog errorlog = new Sys_ErrorLog();
                errorlog.TableName = tableName;
                string m = updatesql + " " + e.Message;
                if (m.Length > 2000)
                {
                    m = m.Substring(0, 1999);
                }
                errorlog.ErrorMessage = m;
                errorlog.RelationId = paramArray[0].ToString();
                errorlog.UpdateTime = DateTime.Now;
                errorlog.Save();
                return false;
            }
            finally
            {
                cmd.Dispose();
                sqlConn.Dispose();

            }
        }

        private string GetInsertSql(string tableName, SqlParameter[] paramArray)
        {
            string insertPart = "";
            string valuePart = "";
            foreach (SqlParameter param in paramArray)
            {
                insertPart += param.ParameterName.TrimStart("@".ToCharArray()) + ",";
                valuePart += param.ParameterName + ",";
            }
            insertPart = insertPart.TrimEnd(",".ToCharArray());
            valuePart = valuePart.TrimEnd(",".ToCharArray());

            string sql = string.Format(@"INSERT INTO {0}({1}) VALUES({2});
              select ident_current('{0}');", tableName, insertPart, valuePart);
            return sql;
        }
        private string GetUpdateSql(string tableName, SqlParameter[] paramArray)
        {
            string valuePart = "";
            for (int i = 1; i < paramArray.Length; i++)//跳过第一个条件
            {
                valuePart += string.Format(" {0}=@{0},", paramArray[i].ParameterName.TrimStart("@".ToCharArray()));
            }
            valuePart = valuePart.TrimEnd(",".ToCharArray());
            string optionPart = string.Format(" {0}=@{0}", paramArray[0].ParameterName.TrimStart("@".ToCharArray()));
            string sql = string.Format(@"Update {0} set {1} where {2}", tableName, valuePart, optionPart);
            return sql;
        }
        private string GetDeleteSql(string tableName, SqlParameter[] paramArray)
        {
            string valuePart = "";
            for (int i = 0; i < paramArray.Length; i++)
            {
                valuePart += string.Format(" {0}={1}", paramArray[i].ParameterName.TrimStart("@".ToCharArray()), paramArray[i].Value);
            }
            string sql = string.Format(@"delete {0} where {1}", tableName, valuePart);
            return sql;
        }

        /// <summary>
        /// 新增重载，执行一条计算查询结果语句，返回查询结果（object）。(add by zhengyuefeng 2017-09-13)
        /// </summary>
        /// <param name="connection">SqlConnection对象</param>
        /// <param name="trans">SqlTransaction事务</param>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(SqlConnection connection, SqlTransaction trans, string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    PrepareCommand(cmd, connection, trans, SQLString, cmdParms);
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
                    //trans.Rollback();
                    throw e;
                }
            }
        }
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
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
        /// <summary>
        /// 执行查询语句，返回DataSet(add by zhangyuefeng 2017-09-19)
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public DataSet Query(string SQLString)
        {

            using (SqlConnection connection = new SqlConnection(m_ConnectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }

        }
        #endregion
        //////////////////////////////////////////////SQLOption 支持的方法//////////////////////////////////////////////
        /// <summary>
        /// 根据传入的sql执行Delete
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool DeleteAt(string sql)
        {
            SqlConnection sqlConn = null;
            SqlCommand cmd = null;
            try
            {
                sqlConn = new SqlConnection(m_ConnectionString);
                sqlConn.Open();
                cmd = new SqlCommand(sql, sqlConn);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Sys_ErrorLog errorlog = new Sys_ErrorLog();
                errorlog.Memo = sql;
                errorlog.ErrorMessage = e.Message;
                errorlog.UpdateTime = DateTime.Now;
                errorlog.Save();
                return false;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (sqlConn != null)
                {
                    sqlConn.Close();
                }
            }
        }

        public int InsertData(string sql)
        {
            sql += SqlHelper.SCOPE_IDENTITY;
            SqlConnection sqlConn = null;
            SqlCommand cmd = null;
            try
            {
                sqlConn = new SqlConnection(m_ConnectionString);
                sqlConn.Open();
                cmd = new SqlCommand(sql, sqlConn);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception e)
            {
                Sys_ErrorLog errorlog = new Sys_ErrorLog();
                errorlog.Memo = sql;
                errorlog.ErrorMessage = e.Message;
                errorlog.UpdateTime = DateTime.Now;
                errorlog.Save();
                return 0;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (sqlConn != null)
                {
                    sqlConn.Close();
                }
            }
        }
        /// <summary>
        /// 操作修改
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool UpdateData(string sql)
        {

            SqlConnection sqlConn = null;
            SqlCommand cmd = null;

            try
            {
                sqlConn = new SqlConnection(m_ConnectionString);
                sqlConn.Open();
                cmd = new SqlCommand(sql, sqlConn);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                //记录错误日志
                Sys_ErrorLog errorlog = new Sys_ErrorLog();
                errorlog.ErrorMessage = e.Message;
                errorlog.Memo = sql;
                errorlog.UpdateTime = DateTime.Now;
                errorlog.Save();
                return false;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (sqlConn != null)
                {
                    sqlConn.Close();
                }
            }
        }

        /// <summary>
        /// 将传入的sql 在事物中执行。并返回操作条数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecNoQuerySqlTran(string sql)
        {
            //创建数据库连接对象
            SqlConnection sqlConn = null;
            SqlCommand cmd = null;
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                sqlConn = new SqlConnection(m_ConnectionString);
                sqlConn.Open();
                tran = sqlConn.BeginTransaction();
                cmd = new SqlCommand(sql, sqlConn, tran);
                result = cmd.ExecuteNonQuery();
                tran.Commit();
            }
            catch
            {
                //回滚事物处理 取消之前的操作
                tran.Rollback();
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (sqlConn != null)
                {
                    sqlConn.Close();
                }
            }

            return result;
        }
        /// <summary>
        /// 调用存储过程
        /// </summary>
        /// <param name="ProcedureName">存储过程的名称</param>
        /// <param name="parameter">存储过程的参数值和参数名称以成对的字典类型实现</param>
        /// <returns></returns>
        public DataSet UseProcedure(string ProcedureName, Dictionary<string, string> parameter)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(m_ConnectionString))
            {
                conn.Open();
                using (SqlCommand sqlComm = conn.CreateCommand())
                {
                    sqlComm.CommandText = ProcedureName;
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    SqlParameter sp = new SqlParameter();
                    foreach (var item in parameter)
                    {
                        sqlComm.Parameters.Add(new SqlParameter(item.Key,item.Value));
                    }
                    SqlDataAdapter dp = new SqlDataAdapter(sqlComm);
                    dp.Fill(ds);
                }
            }
            return ds;
        }
    }
    /// <summary>
    /// sqlserver 从行读取字段的静态方法
    /// </summary>
    public abstract class DBTool
    {
        public static int GetIntFromRow(DataRow row, string columnName, int defaultValue)
        {
            if (row[columnName] is DBNull)
            {
                return defaultValue;
            }
            else
            {
                try
                {
                    return Convert.ToInt32(row[columnName]);
                }
                catch
                {
                    return defaultValue;
                }
            }
        }

        public static double GetDoubleFromRow(DataRow row, string columnName, double defaultValue)
        {
            if (row[columnName] is DBNull)
            {
                return defaultValue;
            }
            else
            {
                try
                {
                    return Convert.ToDouble(row[columnName]);
                }
                catch
                {
                    return defaultValue;
                }
            }
        }

        public static decimal GetDecimalFromRow(DataRow row, string columnName, decimal defaultValue)
        {
            if (row[columnName] is DBNull)
            {
                return defaultValue;
            }
            else
            {
                try
                {
                    return Convert.ToDecimal(row[columnName]);
                }
                catch
                {
                    return defaultValue;
                }
            }
        }
        public static string GetStringFromRow(DataRow row, string columnName, string defaultValue)
        {
            if (row[columnName] is DBNull)
            {
                return defaultValue;
            }
            else
            {
                return row[columnName].ToString();
            }
        }

        public static DateTime GetDateTimeFromRow(DataRow row, string columnName)
        {
            try
            {
                return Convert.ToDateTime(row[columnName]);
            }
            catch
            {
                return new DateTime(1900, 1, 1);
            }
        }

        public static object GetEnumFromRow(DataRow row, string columnName, System.Type enumType)
        {
            object result = Enum.Parse(enumType, row[columnName].ToString(), true);
            if (!Enum.IsDefined(enumType, result))
            {
                throw new System.ArgumentOutOfRangeException(columnName, result, "枚举值未定义");
            }
            return result;
        }

        public static long GetLongFromRow(DataRow row, string columnName, long defaultValue)
        {
            if (row[columnName] is DBNull)
            {
                return defaultValue;
            }
            else
            {
                try
                {
                    return Convert.ToInt64(row[columnName]);
                }
                catch
                {
                    return defaultValue;
                }
            }
        }
    }

}