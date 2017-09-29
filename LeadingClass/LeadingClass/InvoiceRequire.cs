using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class InvoiceRequire
    {
        private int m_Id;
        private int m_ComId;
        private int m_StatementId;
        private string m_InvoiceName;
        private string m_InvoiceType;
        private string m_InvoiceContent;
        private double m_InvoiceAmount;
        private string m_InvoiceMemo;
        private int m_UserId;
        private DateTime m_UpdateTime;
        private string m_InvoiceStatus;
        private int m_MemberInvoiceId;
        private string m_applicationNo;

        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int ComId { get { return m_ComId; } set { m_ComId = value; } }
        public int StatementId { get { return m_StatementId; } set { m_StatementId = value; } }
        public string InvoiceName { get { return m_InvoiceName; } set { m_InvoiceName = value; } }
        public string InvoiceType { get { return m_InvoiceType; } set { m_InvoiceType = value; } }
        public string InvoiceContent { get { return m_InvoiceContent; } set { m_InvoiceContent = value; } }
        public double InvoiceAmount { get { return m_InvoiceAmount; } set { m_InvoiceAmount = value; } }
        public string InvoiceMemo { get { return m_InvoiceMemo; } set { m_InvoiceMemo = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public string InvoiceStatus { get { return m_InvoiceStatus; } set { m_InvoiceStatus = value; } }
        public int MemberInvoiceId { get { return m_MemberInvoiceId; } set { m_MemberInvoiceId = value; } }
        public string applicationNo { get { return m_applicationNo; } set { m_applicationNo = value; } }
        public InvoiceRequire()
        {
            m_Id = 0;
            m_ComId = 0;
            m_StatementId = 0;
            m_InvoiceName = "";
            m_InvoiceType = "";
            m_InvoiceContent = "";
            m_InvoiceAmount = 0;
            m_InvoiceMemo = "";
            m_UserId = 0;
            m_UpdateTime = DateTime.Now;
            m_InvoiceStatus = CommenClass.InvoiceStatus.待开票.ToString();
            m_MemberInvoiceId = 0;
            m_applicationNo = "";
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@ComId", m_ComId));
            arrayList.Add(new SqlParameter("@StatementId", m_StatementId));
            arrayList.Add(new SqlParameter("@InvoiceName", m_InvoiceName));
            arrayList.Add(new SqlParameter("@InvoiceType", m_InvoiceType));
            arrayList.Add(new SqlParameter("@InvoiceContent", m_InvoiceContent));
            arrayList.Add(new SqlParameter("@InvoiceAmount", m_InvoiceAmount));
            arrayList.Add(new SqlParameter("@InvoiceMemo", m_InvoiceMemo));
            arrayList.Add(new SqlParameter("@UserId", m_UserId));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            arrayList.Add(new SqlParameter("@InvoiceStatus", m_InvoiceStatus));
            arrayList.Add(new SqlParameter("@MemberInvoiceId", m_MemberInvoiceId));
            arrayList.Add(new SqlParameter("@applicationNo", m_applicationNo));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("InvoiceRequire", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("InvoiceRequire", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from InvoiceRequire where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_ComId = DBTool.GetIntFromRow(row, "ComId", 0);
                m_StatementId = DBTool.GetIntFromRow(row, "StatementId", 0);
                m_InvoiceName = DBTool.GetStringFromRow(row, "InvoiceName", "");
                m_InvoiceType = DBTool.GetStringFromRow(row, "InvoiceType", "");
                m_InvoiceContent = DBTool.GetStringFromRow(row, "InvoiceContent", "");
                m_InvoiceAmount = DBTool.GetDoubleFromRow(row, "InvoiceAmount", 0);
                m_InvoiceMemo = DBTool.GetStringFromRow(row, "InvoiceMemo", "");
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_InvoiceStatus = DBTool.GetStringFromRow(row, "InvoiceStatus", "");
                m_MemberInvoiceId = DBTool.GetIntFromRow(row, "MemberInvoiceId", 0);
                m_applicationNo = DBTool.GetStringFromRow(row, "applicationNo", "");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format(" Delete from InvoiceRequire where Id={0} ", m_Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}
