using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class InvoiceMemberInfo
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string InvoiceTaxNo { get; set; }
        public string InvoiceAddress { get; set; }
        public string InvoicePhone { get; set; }
        public string InvoiceBank { get; set; }
        public string InvoiceAccount { get; set; }
        public string InvoiceMemo { get; set; }
        public string MemberName { get; set; }
        private DBOperate m_dbo;

        public InvoiceMemberInfo()
        {
            Id = 0;
            MemberId = 0;
            InvoiceTaxNo = "";
            InvoiceAddress = "";
            InvoicePhone = "";
            InvoiceBank = "";
            InvoiceAccount = "";
            InvoiceMemo = "";
            MemberName = "";
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@MemberId", MemberId));
            arrayList.Add(new SqlParameter("@InvoiceTaxNo", InvoiceTaxNo));
            arrayList.Add(new SqlParameter("@InvoiceAddress", InvoiceAddress));
            arrayList.Add(new SqlParameter("@InvoicePhone", InvoicePhone));
            arrayList.Add(new SqlParameter("@InvoiceBank", InvoiceBank));
            arrayList.Add(new SqlParameter("@InvoiceAccount", InvoiceAccount));
            arrayList.Add(new SqlParameter("@InvoiceMemo", InvoiceMemo));
            arrayList.Add(new SqlParameter("@MemberName", MemberName));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("InvoiceMemberInfo", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("InvoiceMemberInfo", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from InvoiceMemberInfo where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);
                InvoiceTaxNo = DBTool.GetStringFromRow(row, "InvoiceTaxNo", "");
                InvoiceAddress = DBTool.GetStringFromRow(row, "InvoiceAddress", "");
                InvoicePhone = DBTool.GetStringFromRow(row, "InvoicePhone", "");
                InvoiceBank = DBTool.GetStringFromRow(row, "InvoiceBank", "");
                InvoiceAccount = DBTool.GetStringFromRow(row, "InvoiceAccount", "");
                InvoiceMemo = DBTool.GetStringFromRow(row, "InvoiceMemo", "");
                MemberName = DBTool.GetStringFromRow(row, "MemberName", "");
                return true;
            }
            return false;
        }

        public bool Delete()
        {
            string sql = string.Format("Delete from InvoiceMemberInfo where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        public DataSet GetInvoiceInfoByMemberId()
        {
            string sql = string.Format("select * from InvoiceMemberInfo where MemberId={0} ", MemberId);
            return m_dbo.GetDataSet(sql);
        }
    }
}
