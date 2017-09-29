using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class Invoice
    {
        private int m_Id;
        private int m_BranchId;
        private int m_ComId;
        private int m_OrderStatementId;
        private int m_InvoiceRequireId;
        private DateTime m_InvoiceDate;
        private string m_InvoiceNo;
        private string m_InvoiceType;
        private double m_InvoiceMoney;
        private string m_Memo;
        private DateTime m_UpdateTime;
        private int m_UserId;
        private int m_PrintNum;
        private DateTime m_PrintTime;
           
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public int ComId { get { return m_ComId; } set { m_ComId = value; } }
        public int OrderStatementId { get { return m_OrderStatementId; } set { m_OrderStatementId = value; } }
        public int InvoiceRequireId { get { return m_InvoiceRequireId; } set { m_InvoiceRequireId = value; } }
        public DateTime InvoiceDate { get { return m_InvoiceDate; } set { m_InvoiceDate = value; } }
        public string InvoiceNo { get { return m_InvoiceNo; } set { m_InvoiceNo = value; } }
        public string InvoiceType { get { return m_InvoiceType; } set { m_InvoiceType = value; } }
        public double InvoiceMoney { get { return m_InvoiceMoney; } set { m_InvoiceMoney = value; } }
        public string Memo { get { return m_Memo; } set { m_Memo = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public int PrintNum { get { return m_PrintNum; } set { m_PrintNum = value; } }
        public DateTime PrintTime { get { return m_PrintTime; } set { m_PrintTime = value; } }
        public Invoice()
        {
            m_Id = 0;
            m_BranchId = 0;
            m_ComId = 0;
            m_OrderStatementId = 0;
            m_InvoiceRequireId = 0;
            m_InvoiceDate = DateTime.Now;
            m_InvoiceNo = "";
            m_InvoiceType = "";
            m_InvoiceMoney = 0;
            m_Memo = "";
            m_UpdateTime = DateTime.Now;
            m_UserId = 0;
            m_PrintNum = 0;
            m_PrintTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@BranchId", m_BranchId));
            arrayList.Add(new SqlParameter("@ComId", m_ComId));
            arrayList.Add(new SqlParameter("@OrderStatementId", m_OrderStatementId));
            arrayList.Add(new SqlParameter("@InvoiceRequireId", m_InvoiceRequireId));
            arrayList.Add(new SqlParameter("@InvoiceDate", m_InvoiceDate));
            arrayList.Add(new SqlParameter("@InvoiceNo", m_InvoiceNo));
            arrayList.Add(new SqlParameter("@InvoiceType", m_InvoiceType));
            arrayList.Add(new SqlParameter("@InvoiceMoney", Math.Round(m_InvoiceMoney, 2)));
            arrayList.Add(new SqlParameter("@Memo", m_Memo));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            arrayList.Add(new SqlParameter("@UserId", m_UserId));
            arrayList.Add(new SqlParameter("@PrintNum", m_PrintNum));
            arrayList.Add(new SqlParameter("@PrintTime", m_PrintTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("Invoice", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Invoice", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            if (this.Id > 0)
            {
                //修改相关开票申请的开票状态
                InvoiceRequire ir = new InvoiceRequire();
                ir.Id = this.InvoiceRequireId;
                ir.Load();
                ir.InvoiceStatus = CommenClass.InvoiceStatus.已开票.ToString();
                ir.Save();
                OrderStatementManager osm = new OrderStatementManager();
                osm.UpdateOrderStatementInvoiceData(ir.StatementId);
             }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Invoice where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                m_ComId = DBTool.GetIntFromRow(row, "ComId", 0);
                m_OrderStatementId = DBTool.GetIntFromRow(row, "OrderStatementId", 0);
                m_InvoiceRequireId = DBTool.GetIntFromRow(row, "InvoiceRequireId", 0);
                m_InvoiceDate = DBTool.GetDateTimeFromRow(row, "InvoiceDate");
                m_InvoiceNo = DBTool.GetStringFromRow(row, "InvoiceNo", "");
                m_InvoiceType = DBTool.GetStringFromRow(row, "InvoiceType", "");
                m_InvoiceMoney = DBTool.GetDoubleFromRow(row, "InvoiceMoney", 0);
                m_Memo = DBTool.GetStringFromRow(row, "Memo", "");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_PrintNum = DBTool.GetIntFromRow(row, "PrintNum", 0);
                m_PrintTime = DBTool.GetDateTimeFromRow(row, "PrintTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            this.Load();
            string sql = string.Format(" delete from Invoice where id={0} ", m_Id);
            bool r= m_dbo.ExecuteNonQuery(sql);
            if (r)
            {
                //修改相关开票申请的开票状态
                InvoiceRequire ir = new InvoiceRequire();
                ir.Id = this.InvoiceRequireId;
                ir.Load();
                ir.InvoiceStatus = CommenClass.InvoiceStatus.待开票.ToString();
                ir.Save();
                //更新对账单 开金额数据
                OrderStatementManager osm = new OrderStatementManager();
                osm.UpdateOrderStatementInvoiceData(ir.StatementId);
            }
            return r;
        }
        public bool AddPrintNum()
        {
            string sql = string.Format(" update Invoice set PrintNum=printNum+1,PrintTime='{1}' where Id={0} ", m_Id, DateTime.Now);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}
