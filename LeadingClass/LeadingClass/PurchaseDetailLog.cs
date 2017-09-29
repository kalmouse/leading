using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{

    public class PurchaseDetailLog
    {
        private int m_Id;
        private int m_PurchaseId;
        private int m_StoreId;
        private int m_UserId;
        private int m_GoodsId;
        private int m_OldNum;
        private int m_Num;
        private int m_NewNum;
        private double m_OldPrice;
        private double m_NewPrice;
        private string m_Memo;
        private DateTime m_UpdateTime;
        private DBOperate m_dbo;

        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int PurchaseId { get { return m_PurchaseId; } set { m_PurchaseId = value; } }
        public int StoreId { get { return m_StoreId; } set { m_StoreId = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public int GoodsId { get { return m_GoodsId; } set { m_GoodsId = value; } }
        public int OldNum { get { return m_OldNum; } set { m_OldNum = value; } }
        public int Num { get { return m_Num; } set { m_Num = value; } }
        public int NewNum { get { return m_NewNum; } set { m_NewNum = value; } }
        public double OldPrice { get { return m_OldPrice; } set { m_OldPrice = value; } }
        public double NewPrice { get { return m_NewPrice; } set { m_NewPrice = value; } }
        public string Memo { get { return m_Memo; } set { m_Memo = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public PurchaseDetailLog()
        {
            m_Id = 0;
            m_PurchaseId = 0;
            m_StoreId = 0;
            m_UserId = 0;
            m_GoodsId = 0;
            m_OldNum = 0;
            m_Num = 0;
            m_NewNum = 0;
            m_OldPrice = 0;
            m_NewPrice = 0;
            m_Memo = "";
            m_UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public PurchaseDetailLog(int Id)
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
            arrayList.Add(new SqlParameter("@PurchaseId", m_PurchaseId));
            arrayList.Add(new SqlParameter("@StoreId", m_StoreId));
            arrayList.Add(new SqlParameter("@UserId", m_UserId));
            arrayList.Add(new SqlParameter("@GoodsId", m_GoodsId));
            arrayList.Add(new SqlParameter("@OldNum", m_OldNum));
            arrayList.Add(new SqlParameter("@Num", m_Num));
            arrayList.Add(new SqlParameter("@NewNum", m_NewNum));
            arrayList.Add(new SqlParameter("@OldPrice", m_OldPrice));
            arrayList.Add(new SqlParameter("@NewPrice", m_NewPrice));
            arrayList.Add(new SqlParameter("@Memo", m_Memo));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("PurchaseDetailLog", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("PurchaseDetailLog", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from PurchaseDetailLog where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_PurchaseId = DBTool.GetIntFromRow(row, "PurchaseId", 0);
                m_StoreId = DBTool.GetIntFromRow(row, "StoreId", 0);
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                m_OldNum = DBTool.GetIntFromRow(row, "OldNum", 0);
                m_Num = DBTool.GetIntFromRow(row, "Num", 0);
                m_NewNum = DBTool.GetIntFromRow(row, "NewNum", 0);
                m_OldPrice = DBTool.GetIntFromRow(row, "OldPrice", 0);
                m_NewPrice = DBTool.GetIntFromRow(row, "NewPrice", 0);
                m_Memo = DBTool.GetStringFromRow(row, "Memo", "");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
    }
}
