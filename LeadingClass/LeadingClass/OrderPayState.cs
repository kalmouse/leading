using CommenClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace LeadingClass
{
    public class OrderPayState
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MemberId { get; set; }
        public decimal OrderSumMoney { get; set; }
        public DateTime OrderTime { get; set; }
        public int PayState { get; set; }
        public string PayRemarks { get; set; }
        public int IsInner { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;
        public OrderPayState()
        {
            Id = 0;
            OrderId = 0;
            MemberId = 0;
            OrderSumMoney = 0;
            OrderTime = DateTime.Now;
            PayState = 0;
            PayRemarks = "未付款";
            IsInner = 0;
            UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@OrderId", OrderId));
            arrayList.Add(new SqlParameter("@MemberId", MemberId));
            arrayList.Add(new SqlParameter("@OrderSumMoney", OrderSumMoney));
            arrayList.Add(new SqlParameter("@OrderTime", OrderTime));
            arrayList.Add(new SqlParameter("@PayState", PayState));
            arrayList.Add(new SqlParameter("@PayRemarks", PayRemarks));
            arrayList.Add(new SqlParameter("@IsInner", IsInner));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (Id > 0)
            {
                m_dbo.UpdateData("OrderPayState", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                Id = m_dbo.InsertData("OrderPayState", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from OrderPayState where OrderId={0}", OrderId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                OrderId = DBTool.GetIntFromRow(row, "OrderId", 0);
                MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);
                OrderSumMoney = DBTool.GetDecimalFromRow(row, "OrderSumMoney", 0);
                OrderTime = DBTool.GetDateTimeFromRow(row, "OrderTime");
                PayState = DBTool.GetIntFromRow(row, "PayState", 0);
                IsInner = DBTool.GetIntFromRow(row, "IsInner", 0);
                PayRemarks = DBTool.GetStringFromRow(row, "PayRemarks", "");
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        /// <summary>
        /// 根据OrderId更新订单支付状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool UpdatePayState(int orderId)
        {
            string sql = string.Format("update OrderPayState set PayState=1,PayRemarks='已付款' where OrderId={0}", orderId);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 根据orderId查询订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool LoadOrder(int orderId)
        {
            string sql = string.Format("select * from OrderPayState where OrderId={0}", orderId);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool UpdateIsInner(int orderId)
        {
            string sql = string.Format("update OrderPayState set IsInner='-100' where OrderId={0}", orderId);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}
