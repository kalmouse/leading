using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class VIPConfiguration
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int IsReceiveSms { get; set; }//是否接收短信通知 add by wangpengliang 2015-6-10
        public int IsPassAudit { get; set; }//手机号是否通过了审核 add by wangpengliang 2015-6-10
        public string ReceiveSmsTime { get; set; }//接收短信时间 add by wangpengliang 2015-6-10
        public string SmsType { get; set; }//接收短信类型 add by wangpengliang  2015-6-10
        public string Mobile { get; set; }//绑定的手机号
        private DBOperate m_dbo;
        public VIPConfiguration()
        {
            Id = 0;
            MemberId = 0;
            IsReceiveSms = 0;
            IsPassAudit = 0;
            ReceiveSmsTime = "";
            SmsType = "";
            Mobile = "";
            m_dbo = new DBOperate();
        }
        public VIPConfiguration(int Id)
        {
            m_dbo = new DBOperate();
            this.Id = Id;
            this.Load();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@MemberId", MemberId));
            arrayList.Add(new SqlParameter("@IsReceiveSms", IsReceiveSms));
            arrayList.Add(new SqlParameter("@IsPassAudit", IsPassAudit));
            arrayList.Add(new SqlParameter("@ReceiveSmsTime", ReceiveSmsTime));
            arrayList.Add(new SqlParameter("@SmsType", SmsType));
            arrayList.Add(new SqlParameter("@Mobile", Mobile));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("VIPConfiguration", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("VIPConfiguration", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public DataSet Read()
        {
            string sql = string.Format("select * from View_MemberConfiguration");
            return m_dbo.GetDataSet(sql);
        }                                               
        public bool Load()
        {
            string sql = string.Format("select * from VIPConfiguration where MemberId={0}", MemberId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);
                IsReceiveSms = DBTool.GetIntFromRow(row, "IsReceiveSms", 0);
                IsPassAudit = DBTool.GetIntFromRow(row, "IsPassAudit", 0);
                ReceiveSmsTime = DBTool.GetStringFromRow(row, "ReceiveSmsTime", "");
                SmsType = DBTool.GetStringFromRow(row, "SmsType", "");
                Mobile = DBTool.GetStringFromRow(row, "Mobile", "");
                return true;
            }
            return false;
        }
    }
}
