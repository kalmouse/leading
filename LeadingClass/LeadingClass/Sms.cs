using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
namespace LeadingClass
{
    public class Sms
    {
        public int Id { get; set; }
        public string Mobile { get; set; }
        public string SMSContent { get; set; }
        public int Status { get; set; }
        public DateTime Addtime { get; set; }
        private DBOperate m_dbo;

        public Sms()
        {
            Id = 0;
            Mobile = "";
            SMSContent = "";
            Status = 0;
            Addtime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@Mobile", Mobile));
            arrayList.Add(new SqlParameter("@SMSContent", SMSContent));
            arrayList.Add(new SqlParameter("@Status", Status));
            arrayList.Add(new SqlParameter("@Addtime", Addtime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Sms", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Sms", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Sms where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                Mobile = DBTool.GetStringFromRow(row, "Mobile", "");
                SMSContent = DBTool.GetStringFromRow(row, "SMSContent", "");
                Status = DBTool.GetIntFromRow(row, "Status", 0);
                Addtime = DBTool.GetDateTimeFromRow(row, "Addtime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from Sms where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
    public class Smsstore
    {
        public int Id { get; set; }
        public string Mobile { get; set; }
        public string SMSCode { get; set; }
        public int SMSNumber { get; set; }
        public DateTime SendTime { get; set; }
        private DBOperate m_dbo;

        public Smsstore()
        {
            Id = 0;
            Mobile = "";
            SMSCode = "";
            SMSNumber = 0;
            SendTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@Mobile", Mobile));
            arrayList.Add(new SqlParameter("@SMSCode", SMSCode));
            arrayList.Add(new SqlParameter("@SMSNumber", SMSNumber));
            arrayList.Add(new SqlParameter("@SendTime", SendTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("SmsStore", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("SmsStore", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from SmsStore where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                Mobile = DBTool.GetStringFromRow(row, "Mobile", "");
                SMSCode = DBTool.GetStringFromRow(row, "SMSCode", "");
                SMSNumber = DBTool.GetIntFromRow(row, "SMSNumber", 0);
                SendTime = DBTool.GetDateTimeFromRow(row, "SendTime");
                return true;
            }
            return false;
        }

        public DataSet Model(string mobile)
        {
            string sql = string.Format("select * from SmsStore where Mobile={0}", mobile);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds;
            }
            else return null;          
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from SmsStore where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }

}
