using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class OrderModify
    {
        private int m_Id;
        private int m_OrderId;
        private int m_GoodsId;
        private string m_Model;
        private int m_OldNum;
        private int m_NewNum;
        private double m_OldPrice;
        private double m_NewPrice;
        private int m_UserId;
        private string m_PurchaseType;
        private DateTime m_UpdateTime;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int OrderId { get { return m_OrderId; } set { m_OrderId = value; } }
        public int GoodsId { get { return m_GoodsId; } set { m_GoodsId = value; } }
        public string Model { get { return m_Model; } set { m_Model = value; } }
        public int OldNum { get { return m_OldNum; } set { m_OldNum = value; } }
        public int NewNum { get { return m_NewNum; } set { m_NewNum = value; } }
        public double OldPrice { get { return m_OldPrice; } set { m_OldPrice = value; } }
        public double NewPrice { get { return m_NewPrice; } set { m_NewPrice = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public string PurchaseType { get { return m_PurchaseType; } set { m_PurchaseType = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public OrderModify()
        {
            m_Id = 0;
            m_OrderId = 0;
            m_GoodsId = 0;
            m_Model = "";
            m_OldNum = 0;
            m_NewNum = 0;
            m_OldPrice = 0;
            m_NewPrice = 0;
            m_UserId = 0;
            m_PurchaseType = "";
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
            arrayList.Add(new SqlParameter("@OrderId", m_OrderId));
            arrayList.Add(new SqlParameter("@GoodsId", m_GoodsId));
            arrayList.Add(new SqlParameter("@Model", m_Model));
            arrayList.Add(new SqlParameter("@OldNum", m_OldNum));
            arrayList.Add(new SqlParameter("@NewNum", m_NewNum));
            arrayList.Add(new SqlParameter("@OldPrice", m_OldPrice));
            arrayList.Add(new SqlParameter("@NewPrice", m_NewPrice));
            arrayList.Add(new SqlParameter("@UserId", m_UserId));
            arrayList.Add(new SqlParameter("@PurchaseType", m_PurchaseType));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("OrderModify", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("OrderModify", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from OrderModify where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_OrderId = DBTool.GetIntFromRow(row, "OrderId", 0);
                m_GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                m_Model = DBTool.GetStringFromRow(row, "Model", "");
                m_OldNum = DBTool.GetIntFromRow(row, "OldNum", 0);
                m_NewNum = DBTool.GetIntFromRow(row, "NewNum", 0);
                m_OldPrice = DBTool.GetDoubleFromRow(row, "OldPrice", 0);
                m_NewPrice = DBTool.GetDoubleFromRow(row, "NewPrice", 0);
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_PurchaseType = DBTool.GetStringFromRow(row, "PurchaseType", "");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        } 
        /// <summary>
        /// 找到对应订单修改的商品
        /// </summary>
        /// <returns></returns>
        public DataSet GetModifyGoodsByOrderId()
        { 
            if (OrderId > 0)
            {
                string sql = string.Format(" select * from OrderModify where OrderId={0} and UserId={1} ", OrderId,UserId);
                return m_dbo.GetDataSet(sql);
            }
            return null;
        }
 
        /// <summary>
        /// 修改订单明细状态
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public bool UpdatePurchaseType(string Id)
        {
            string sql = string.Format("Update OrderModify set PurchaseType='已完成' where Id in({0})", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

    }
}
