using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace LeadingClass
{
    public  class SupplierPriceList
    {
        private int m_Id;
        private int m_SupplierId;
        private string m_Title;
        private DateTime m_ValidDate;
        private int m_UserId;
        private DateTime m_UpdateTime;
        private int m_GoodsNum;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int SupplierId { get { return m_SupplierId; } set { m_SupplierId = value; } }
        public string Title { get { return m_Title; } set { m_Title = value; } }
        public DateTime ValidDate { get { return m_ValidDate; } set { m_ValidDate = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public int GoodsNum { get { return m_GoodsNum; } set { m_GoodsNum = value; } }
        public SupplierPriceList()
        {
            m_Id = 0;
            m_SupplierId = 0;
            m_Title = "";
            m_ValidDate = DateTime.Now;
            m_UserId = 0;
            m_UpdateTime = DateTime.Now;
            m_GoodsNum = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@SupplierId", m_SupplierId));
            arrayList.Add(new SqlParameter("@Title", m_Title));
            arrayList.Add(new SqlParameter("@ValidDate", m_ValidDate));
            arrayList.Add(new SqlParameter("@UserId", m_UserId));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            arrayList.Add(new SqlParameter("@GoodsNum", m_GoodsNum));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("SupplierPriceList", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("SupplierPriceList", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from SupplierPriceList where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_SupplierId = DBTool.GetIntFromRow(row, "SupplierId", 0);
                m_Title = DBTool.GetStringFromRow(row, "Title", "");
                m_ValidDate = DBTool.GetDateTimeFromRow(row, "ValidDate");
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_GoodsNum = DBTool.GetIntFromRow(row, "GoodsNum", 0);
                return true;
            }
            return false;
        }
        
    }
}
