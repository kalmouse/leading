using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{

    public class Storedept
    {
        public int Id { get; set; }
        public string SDeptName { get; set; }
        public int SComId { get; set; }
        public string SCode { get; set; }
        public string SPCode { get; set; }
        public int SLevel { get; set; }
        public DateTime SUpdateTime { get; set; }
        public int SUserId { get; set; }
        private DBOperate m_dbo;

        public Storedept()
        {
            Id = 0;
            SDeptName = "";
            SComId = 0;
            SCode = "";
            SPCode = "";
            SLevel = 0;
            SUpdateTime = DateTime.Now;
            SUserId = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@SDeptName", SDeptName));
            arrayList.Add(new SqlParameter("@SComId", SComId));
            arrayList.Add(new SqlParameter("@SCode", SCode));
            arrayList.Add(new SqlParameter("@SPCode", SPCode));
            arrayList.Add(new SqlParameter("@SLevel", SLevel));
            arrayList.Add(new SqlParameter("@SUpdateTime", SUpdateTime));
            arrayList.Add(new SqlParameter("@SUserId", SUserId));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("StoreDept", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("StoreDept", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from StoreDept where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                SDeptName = DBTool.GetStringFromRow(row, "SDeptName", "");
                SComId = DBTool.GetIntFromRow(row, "SComId", 0);
                SCode = DBTool.GetStringFromRow(row, "SCode", "");
                SPCode = DBTool.GetStringFromRow(row, "SPCode", "");
                SLevel = DBTool.GetIntFromRow(row, "SLevel", 0);
                SUpdateTime = DBTool.GetDateTimeFromRow(row, "SUpdateTime");
                SUserId = DBTool.GetIntFromRow(row, "SUserId", 0);
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from StoreDept where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 读取公司对应的部门信息  add by hjy 2016-6-17
        /// </summary>
        /// <returns></returns>
        public DataSet ReadDeptByComId()
        {
            string sql = string.Format(@"select * from StoreDept  where SComId={0} order by SCode", SComId);
            return m_dbo.GetDataSet(sql);
        }
        public Storedept(int Id)
        {
            m_dbo = new DBOperate();
            this.Id = Id;
            this.Load();
        }
        //判断部门名称是否存在
        public int CheckDeptName(string dName)
        {
            string sql = "select count(*) from StoreDept where SDeptName='" + dName + "'";
            DataSet ds = m_dbo.GetDataSet(sql);
            return Convert.ToInt16(ds.Tables[0].Rows[0][0]);
        }
        ////显示部门列表
        //public DataSet showDeptInfo(int SUserId)
        //{
        //    string sql = "select * from StoreDept where SUserId='" + SUserId + "'";
        //    DataSet ds = m_dbo.GetDataSet(sql);
        //    return ds;
        //}
        //显示未入库订单
        public DataSet showOrderOutofStore(int SUserId)
        {
            string sql = "select * from [Order] where IsStorage=0 and ComId='" + SUserId + "'";
            DataSet ds = m_dbo.GetDataSet(sql);
            return ds;
        }
        //找到本公司本级部门的最大的code
        public string GetMaxCodeByPCode(string PCode, int ComId)
        {
            string sql = string.Format( "select MAX(SCode) from StoreDept where 1=1 and SPCode='{0}' and SComId={1} ", PCode, ComId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return (ds.Tables[0].Rows[0][0] != null) ? ds.Tables[0].Rows[0][0].ToString() : "";
                }
            }
            return "";
        }

        /// <summary>
        /// 根据目前最大的 子Code，获取下一个 子Code 的代码， MaxChildCode 由 Dept 类 提供
        /// </summary>
        /// <param name="MaxChildCode"></param>
        /// <returns></returns>
        public string GetAChildCode(string MaxChildCode)
        {
            string result = "";
            List<string> childpath = GetPath(MaxChildCode);
            if (childpath.Count > 0)
            {
                try
                {
                    string newId = (int.Parse(childpath[childpath.Count - 1]) + 1).ToString();
                    switch (newId.Length)
                    {
                        case 1:
                            newId = "00" + newId;
                            break;
                        case 2:
                            newId = "0" + newId;
                            break;
                        case 3:
                            break;
                        default:
                            break;
                    }

                    for (int i = 0; i < childpath.Count - 1; i++)
                    {
                        result += childpath[i] + "|";
                    }
                    result += newId;

                }
                catch
                {
                }
            }
            else
            {
                result = "001";
            }
            return result;
        }

        /// <summary>
        /// 根据“|”拆分Code路径
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private List<string> GetPath(string code)
        {
            string[] path = code.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> result = new List<string>();
            for (int i = 0; i < path.Length; i++)
            {
                result.Add(path[i]);
            }
            return result;
        }
    }
}
