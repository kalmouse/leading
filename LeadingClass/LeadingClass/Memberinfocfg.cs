using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class Memberinfocfg
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public Memberinfocfg()
        {
            Id = 0;
            MemberId = 0;
            Type = "";
            Value = 0;
            UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@MemberId", MemberId));
            arrayList.Add(new SqlParameter("@Type", Type));
            arrayList.Add(new SqlParameter("@Value", Value));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("MemberInfoCfg", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("MemberInfoCfg", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from MemberInfoCfg where MemberId={0}", MemberId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);
                Type = DBTool.GetStringFromRow(row, "Type", "");
                Value = DBTool.GetIntFromRow(row, "Value", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from MemberInfoCfg where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberIds"></param>
        /// <returns></returns>
        public DataSet GetIsPassAudit(List<int> memberIds)
        {
            string sql = "select mc.*,m.Email from MemberInfoCfg mc join Member m on mc.MemberId = m.Id where 1=1 ";
            sql += " and MemberId in ( ";
            for (int i = 0; i < memberIds.Count; i++)
            {
                if (i == 0)
                {
                    sql += string.Format(" '{0}' ", memberIds[0]);
                }
                else sql += string.Format(" ,'{0}' ", memberIds[i]);
            }
            sql += " ) ";
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 邮件审核是否开启
        /// </summary>
        /// <param name="MemberId"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public bool UpdateValue(int MemberId,int Value)
        {
            string sql = string.Format("update MemberInfoCfg set Value = {1},UpdateTime = GETDATE() where MemberId={0};", MemberId, Value);
            return m_dbo.ExecuteNonQuery(sql);
        }

    }
}
