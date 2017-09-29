using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class TPI_Refund
    {
        public int Id { get; set; }
        public int projectId { get; set; }

        public DateTime createTime { get; set; }
        public string OrderRefundId { get; set; }
        public string OrigOrderId { get; set; }
        public string CommodityId { get; set; }
        public string SkuId { get; set; }
        public string CustomerId { get; set; }
        public int RefundNum { get; set; }
        public double RefundAmt { get; set; }
        public DateTime RefundTime { get; set; }
        public string RefundReason { get; set; }
        public string RefundName { get; set; }
        public string RefundMobile { get; set; }
        public string RefundTelephone { get; set; }
        public string RefundProvince { get; set; }
        public string RefundCity { get; set; }
        public string RefundCounty { get; set; }
        public string RefundAddressDetail { get; set; }
        public string RefundDescription { get; set; }
        public int RefundType { get; set; }
        public string StoreId { get; set; }
        public double RefundIntelAmt { get; set; }
        public double RefundCouponAmt { get; set; }
        public int RefundResult { get; set; }
        public DateTime OrderRefundTime { get; set; }
        public string OrderRefundDesc { get; set; }
        public int orderId { get; set; }
        private DBOperate m_dbo;

        public TPI_Refund()
        {
            Id = 0;
            projectId = 0;
            createTime = DateTime.Now;
            OrderRefundId = "";
            OrigOrderId = "";
            CommodityId = "";
            SkuId = "";
            CustomerId = "";
            RefundNum = 0;
            RefundAmt = 0;
            RefundTime = DateTime.Now;
            RefundReason = "";
            RefundName = "";
            RefundMobile = "";
            RefundTelephone = "";
            RefundProvince = "";
            RefundCity = "";
            RefundCounty = "";
            RefundAddressDetail = "";
            RefundDescription = "";
            RefundType = 0;
            StoreId = "";
            RefundIntelAmt = 0;
            RefundCouponAmt = 0;
            RefundResult = 0;
            OrderRefundTime = DateTime.Now;
            OrderRefundDesc = "";
            orderId = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@projectId", projectId));
            arrayList.Add(new SqlParameter("@createTime", createTime));
            arrayList.Add(new SqlParameter("@OrderRefundId", OrderRefundId));
            arrayList.Add(new SqlParameter("@OrigOrderId", OrigOrderId));
            arrayList.Add(new SqlParameter("@CommodityId", CommodityId));
            arrayList.Add(new SqlParameter("@SkuId", SkuId));
            arrayList.Add(new SqlParameter("@CustomerId", CustomerId));
            arrayList.Add(new SqlParameter("@RefundNum", RefundNum));
            arrayList.Add(new SqlParameter("@RefundAmt", RefundAmt));
            arrayList.Add(new SqlParameter("@RefundTime", RefundTime));
            arrayList.Add(new SqlParameter("@RefundReason", RefundReason));
            arrayList.Add(new SqlParameter("@RefundName", RefundName));
            arrayList.Add(new SqlParameter("@RefundMobile", RefundMobile));
            arrayList.Add(new SqlParameter("@RefundTelephone", RefundTelephone));
            arrayList.Add(new SqlParameter("@RefundProvince", RefundProvince));
            arrayList.Add(new SqlParameter("@RefundCity", RefundCity));
            arrayList.Add(new SqlParameter("@RefundCounty", RefundCounty));
            arrayList.Add(new SqlParameter("@RefundAddressDetail", RefundAddressDetail));
            arrayList.Add(new SqlParameter("@RefundDescription", RefundDescription));
            arrayList.Add(new SqlParameter("@RefundType", RefundType));
            arrayList.Add(new SqlParameter("@StoreId", StoreId));
            arrayList.Add(new SqlParameter("@RefundIntelAmt", RefundIntelAmt));
            arrayList.Add(new SqlParameter("@RefundCouponAmt", RefundCouponAmt));
            arrayList.Add(new SqlParameter("@RefundResult", RefundResult));
            arrayList.Add(new SqlParameter("@OrderRefundTime", OrderRefundTime));
            arrayList.Add(new SqlParameter("@OrderRefundDesc", OrderRefundDesc));
            arrayList.Add(new SqlParameter("@orderId", orderId));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("TPI_Refund", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("TPI_Refund", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from TPI_Refund where 1=1 ");
            if (Id > 0)
            {
                sql += string.Format("and id={0} ", Id);
            }
            if (projectId > 0)
            {
                sql += string.Format("and projectId={0} ", projectId);
            }
            if (OrderRefundId !="")
            {
                sql += string.Format("and OrderRefundId={0} ", OrderRefundId);
            }
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                projectId = DBTool.GetIntFromRow(row, "projectId", 0);
                createTime = DBTool.GetDateTimeFromRow(row, "createTime");
                OrderRefundId = DBTool.GetStringFromRow(row, "OrderRefundId", "");
                OrigOrderId = DBTool.GetStringFromRow(row, "OrigOrderId", "");
                CommodityId = DBTool.GetStringFromRow(row, "CommodityId", "");
                SkuId = DBTool.GetStringFromRow(row, "SkuId", "");
                CustomerId = DBTool.GetStringFromRow(row, "CustomerId", "");
                RefundNum = DBTool.GetIntFromRow(row, "RefundNum", 0);
                RefundAmt = DBTool.GetDoubleFromRow(row, "RefundAmt", 0);
                RefundTime = DBTool.GetDateTimeFromRow(row, "RefundTime");
                RefundReason = DBTool.GetStringFromRow(row, "RefundReason", "");
                RefundName = DBTool.GetStringFromRow(row, "RefundName", "");
                RefundMobile = DBTool.GetStringFromRow(row, "RefundMobile", "");
                RefundTelephone = DBTool.GetStringFromRow(row, "RefundTelephone", "");
                RefundProvince = DBTool.GetStringFromRow(row, "RefundProvince", "");
                RefundCity = DBTool.GetStringFromRow(row, "RefundCity", "");
                RefundCounty = DBTool.GetStringFromRow(row, "RefundCounty", "");
                RefundAddressDetail = DBTool.GetStringFromRow(row, "RefundAddressDetail", "");
                RefundDescription = DBTool.GetStringFromRow(row, "RefundDescription", "");
                RefundType = DBTool.GetIntFromRow(row, "RefundType", 0);
                StoreId = DBTool.GetStringFromRow(row, "StoreId", "");
                RefundIntelAmt = DBTool.GetDoubleFromRow(row, "RefundIntelAmt", 0);
                RefundCouponAmt = DBTool.GetDoubleFromRow(row, "RefundCouponAmt", 0);
                RefundResult = DBTool.GetIntFromRow(row, "RefundResult", 0);
                OrderRefundTime = DBTool.GetDateTimeFromRow(row, "OrderRefundTime");
                OrderRefundDesc = DBTool.GetStringFromRow(row, "OrderRefundDesc", "");
                orderId = DBTool.GetIntFromRow(row, "orderId", 0);
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from TPI_Refund where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        public DataTable getTableByIds(string Ids)
        {
            string sql = string.Format("select * from TPI_refund where id in({0})", Ids);
            DataTable dt = m_dbo.GetDataSet(sql).Tables[0];
            return dt;
        }
    }
}
