using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class TPI_Refundwaybill
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreateTime { get; set; }
        public int Tpi_refundId { get; set; }
        public string DeliveryMerchantId { get; set; }
        public int DeliveryMerchantType { get; set; }
        public string DeliveryId { get; set; }
        private DBOperate m_dbo;

        public TPI_Refundwaybill()
        {
            Id = 0;
            ProjectId = 0;
            CreateTime = DateTime.Now;
            Tpi_refundId = 0;
            DeliveryMerchantId = "";
            DeliveryMerchantType = 0;
            DeliveryId = "";
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@ProjectId", ProjectId));
            arrayList.Add(new SqlParameter("@CreateTime", CreateTime));
            arrayList.Add(new SqlParameter("@Tpi_refundId", Tpi_refundId));
            arrayList.Add(new SqlParameter("@DeliveryMerchantId", DeliveryMerchantId));
            arrayList.Add(new SqlParameter("@DeliveryMerchantType", DeliveryMerchantType));
            arrayList.Add(new SqlParameter("@DeliveryId", DeliveryId));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("TPI_RefundWaybill", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("TPI_RefundWaybill", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from TPI_RefundWaybill where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                ProjectId = DBTool.GetIntFromRow(row, "ProjectId", 0);
                CreateTime = DBTool.GetDateTimeFromRow(row, "CreateTime");
                Tpi_refundId = DBTool.GetIntFromRow(row, "Tpi_refundId", 0);
                DeliveryMerchantId = DBTool.GetStringFromRow(row, "DeliveryMerchantId", "");
                DeliveryMerchantType = DBTool.GetIntFromRow(row, "DeliveryMerchantType", 0);
                DeliveryId = DBTool.GetStringFromRow(row, "DeliveryId", "");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from TPI_RefundWaybill where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}
