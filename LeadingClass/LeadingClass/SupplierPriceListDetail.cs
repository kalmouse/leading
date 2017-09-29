using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace LeadingClass
{
    public  class SupplierPriceListDetail
    {
        private int m_Id;
        private int m_SupplierPriceListId;
        private int m_GoodsId;
        private double m_Price;
        private int m_IsNew;
        private DateTime m_UpdateTime;
        private int m_MinSellUnit;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int SupplierPriceListId { get { return m_SupplierPriceListId; } set { m_SupplierPriceListId = value; } }
        public int GoodsId { get { return m_GoodsId; } set { m_GoodsId = value; } }
        public double Price { get { return m_Price; } set { m_Price = value; } }
        public int IsNew { get { return m_IsNew; } set { m_IsNew = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public int MinSellUnit { get { return m_MinSellUnit; } set { m_MinSellUnit = value; } }
        public SupplierPriceListDetail()
        {
            m_Id = 0;
            m_SupplierPriceListId = 0;
            m_GoodsId = 0;
            m_Price = 0;
            m_IsNew = 0;
            m_UpdateTime = DateTime.Now;
            m_MinSellUnit = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@SupplierPriceListId", m_SupplierPriceListId));
            arrayList.Add(new SqlParameter("@GoodsId", m_GoodsId));
            arrayList.Add(new SqlParameter("@Price", m_Price));
            arrayList.Add(new SqlParameter("@IsNew", m_IsNew));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            arrayList.Add(new SqlParameter("@MinSellUnit", m_MinSellUnit));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("SupplierPriceListDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("SupplierPriceListDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from SupplierPriceListDetail where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_SupplierPriceListId = DBTool.GetIntFromRow(row, "SupplierPriceListId", 0);
                m_GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                m_Price = DBTool.GetDoubleFromRow(row, "Price", 0);
                m_IsNew = DBTool.GetIntFromRow(row, "IsNew", 0);
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_MinSellUnit = DBTool.GetIntFromRow(row, "MinSellUnit",0);
                return true;
            }
            return false;
        }
        public SupplierPriceListDetail LoadBySupplierPriceId()
        {
            SupplierPriceListDetail spld = new SupplierPriceListDetail();

            string sql = string.Format("select * from SupplierPriceListDetail where SupplierPriceListId={0}", m_SupplierPriceListId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                spld.Id = DBTool.GetIntFromRow(row, "Id", 0);
                spld.SupplierPriceListId = DBTool.GetIntFromRow(row, "SupplierPriceListId", 0);
                spld.GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                spld.Price = DBTool.GetDoubleFromRow(row, "Price", 0);
                spld.IsNew = DBTool.GetIntFromRow(row, "IsNew", 0);
                spld.UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                spld.MinSellUnit = DBTool.GetIntFromRow(row, "MinSellUnit",0);
                return spld;
            }
            return null;
        }
 
    }
}
