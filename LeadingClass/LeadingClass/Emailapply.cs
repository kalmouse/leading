using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class Emailapply
    {
        public int Id { get; set; }
        public int ApplyId { get; set; }
        public int MemberId { get; set; }
        public string Token { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        private DBOperate m_dbo;

        public Emailapply()
        {
            Id = 0;
            ApplyId = 0;
            MemberId = 0;
            Token = "";
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@ApplyId", ApplyId));
            arrayList.Add(new SqlParameter("@MemberId", MemberId));
            arrayList.Add(new SqlParameter("@Token", Token));
            arrayList.Add(new SqlParameter("@StartTime", StartTime));
            arrayList.Add(new SqlParameter("@EndTime", EndTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("EmailApply", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("EmailApply", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from EmailApply where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                ApplyId = DBTool.GetIntFromRow(row, "ApplyId", 0);
                MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);
                Token = DBTool.GetStringFromRow(row, "Token", "");
                StartTime = DBTool.GetDateTimeFromRow(row, "StartTime");
                EndTime = DBTool.GetDateTimeFromRow(row, "EndTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from EmailApply where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        public DataSet GetMemberByToken()
        {
            string sql = string.Format("select * from EmailApply where Token ='{0}'", Token);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 检查是否已经发送给管理员
        /// </summary>
        /// <returns></returns>
        public bool CheckIsSendToAdmin()
        {
            string sql = string.Format("select * from EmailApply where ApplyId = {0} and MemberId <>{1}", ApplyId, MemberId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
