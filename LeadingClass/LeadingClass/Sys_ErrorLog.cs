using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
namespace LeadingClass
{
    public class Sys_ErrorLog
    {
        private int m_Id;
        private string m_TableName;
        private string m_RelationId;
        private string m_ErrorMessage;
        private string m_Memo;
        private DateTime m_UpdateTime;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public string TableName { get { return m_TableName; } set { m_TableName = value; } }
        public string RelationId { get { return m_RelationId; } set { m_RelationId = value; } }
        public string ErrorMessage { get { return m_ErrorMessage; } set { m_ErrorMessage = value; } }
        public string Memo { get { return m_Memo; } set { m_Memo = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public Sys_ErrorLog()
        {
            m_Id = 0;
            m_TableName = "";
            m_RelationId = "";
            m_ErrorMessage = "";
            m_Memo = "";
            m_UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@TableName", m_TableName));
            arrayList.Add(new SqlParameter("@RelationId", m_RelationId));
            arrayList.Add(new SqlParameter("@ErrorMessage", m_ErrorMessage));
            arrayList.Add(new SqlParameter("@Memo", m_Memo));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("Sys_ErrorLog", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Sys_ErrorLog", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Sys_ErrorLog where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_TableName = DBTool.GetStringFromRow(row, "TableName", "");
                m_RelationId = DBTool.GetStringFromRow(row, "RelationId", "");
                m_ErrorMessage = DBTool.GetStringFromRow(row, "ErrorMessage", "");
                m_Memo = DBTool.GetStringFromRow(row, "Memo", "");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        } 
 
    }
}
