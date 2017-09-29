using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class Crm_DeliveryLog
    {
        public int Id { get; set; }
        public int OrderBoxDeliveryId { get; set; }
        public int OrderId { get; set; }
        public string Content { get; set; }
        public string Memo { get; set; }
        private DBOperate m_dbo;

        public Crm_DeliveryLog()
        {
            Id = 0;
            OrderBoxDeliveryId = 0;
            OrderId = 0;
            Content = "";
            Memo = "";
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@OrderBoxDeliveryId", OrderBoxDeliveryId));
            arrayList.Add(new SqlParameter("@OrderId", OrderId));
            arrayList.Add(new SqlParameter("@Content", Content));
            arrayList.Add(new SqlParameter("@Memo", Memo));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Crm_DeliveryLog", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Crm_DeliveryLog", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Crm_DeliveryLog where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                OrderBoxDeliveryId = DBTool.GetIntFromRow(row, "OrderBoxDeliveryId", 0);
                OrderId = DBTool.GetIntFromRow(row, "OrderId", 0);
                Content = DBTool.GetStringFromRow(row, "Content", "");
                Memo = DBTool.GetStringFromRow(row, "Memo", "");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from Crm_DeliveryLog where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}
