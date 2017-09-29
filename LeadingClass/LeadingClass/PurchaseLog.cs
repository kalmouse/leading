using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class PurchaseLog
    {
        private int m_Id;
        private int m_PurchaseId;
        private int m_StoreId;
        private string m_PurchaseType;
        private double m_SumMoney;
        private int m_UserId;
        private string m_PurchaseStatus;
        private string m_Memo;
        private DateTime m_UpdateTime;
        private DBOperate m_dbo;

        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int PurchaseId { get { return m_PurchaseId; } set { m_PurchaseId = value; } }
        public int StoreId { get { return m_StoreId; } set { m_StoreId = value; } }
        public string PurchaseType { get { return m_PurchaseType; } set { m_PurchaseType = value; } }
        public double SumMoney { get { return m_SumMoney; } set { m_SumMoney = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public string PurchaseStatus { get { return m_PurchaseStatus; } set { m_PurchaseStatus = value; } }
        public string Memo { get { return m_Memo; } set { m_Memo = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public PurchaseLog()
        {
            m_Id = 0;
            m_PurchaseId = 0;
            m_StoreId = 0;
            m_PurchaseType = "";
            m_SumMoney = 0;
            m_UserId = 0;
            m_PurchaseStatus = "";
            m_Memo = "";
            m_UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public PurchaseLog(int Id)
            : this()
        {
            this.Id = Id;
            this.Load();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@PurchaseId", m_PurchaseId));
            arrayList.Add(new SqlParameter("@StoreId", m_StoreId));
            arrayList.Add(new SqlParameter("@PurchaseType", m_PurchaseType));
            arrayList.Add(new SqlParameter("@SumMoney", m_SumMoney));
            arrayList.Add(new SqlParameter("@UserId", m_UserId));
            arrayList.Add(new SqlParameter("@PurchaseStatus", m_PurchaseStatus));
            arrayList.Add(new SqlParameter("@Memo", m_Memo));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("PurchaseLog", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("PurchaseLog", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from PurchaseLog where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_PurchaseId = DBTool.GetIntFromRow(row, "PurchaseId", 0);
                m_StoreId = DBTool.GetIntFromRow(row, "StoreId", 0);
                m_PurchaseType = DBTool.GetStringFromRow(row, "PurchaseType", "");
                m_SumMoney = DBTool.GetDoubleFromRow(row, "SumMoney", 0);
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_PurchaseStatus = DBTool.GetStringFromRow(row, "PurchaseStatus", "");
                m_Memo = DBTool.GetStringFromRow(row, "Memo", "");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
    }
}
