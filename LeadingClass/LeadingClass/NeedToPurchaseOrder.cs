using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;


namespace LeadingClass
{
    public class NeedToPurchaseOrder
    {
        private int m_Id;
        private int m_NeedToPurchaseId;
        private int m_OrderId;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int NeedToPurchaseId { get { return m_NeedToPurchaseId; } set { m_NeedToPurchaseId = value; } }
        public int OrderId { get { return m_OrderId; } set { m_OrderId = value; } }
        public NeedToPurchaseOrder()
        {
            m_Id = 0;
            m_NeedToPurchaseId = 0;
            m_OrderId = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@NeedToPurchaseId", m_NeedToPurchaseId));
            arrayList.Add(new SqlParameter("@OrderId", m_OrderId));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("NeedToPurchaseOrder", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("NeedToPurchaseOrder", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from NeedToPurchaseOrder where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_NeedToPurchaseId = DBTool.GetIntFromRow(row, "NeedToPurchaseId", 0);
                m_OrderId = DBTool.GetIntFromRow(row, "OrderId", 0);
                return true;
            }
            return false;
        }
    }
}
