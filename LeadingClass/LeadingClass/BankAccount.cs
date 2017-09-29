using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class BankAccount
    {
        private int m_Id;
        private int m_BranchId;
        private string m_Name;
        private string m_Bank;
        private string m_Account;
        private string m_Company;
        private string m_Memo;
        private DateTime m_UpdateTime;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public string Name { get { return m_Name; } set { m_Name = value; } }
        public string Bank { get { return m_Bank; } set { m_Bank = value; } }
        public string Account { get { return m_Account; } set { m_Account = value; } }
        public string Company { get { return m_Company; } set { m_Company = value; } }
        public string Memo { get { return m_Memo; } set { m_Memo = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public BankAccount()
        {
            m_Id = 0;
            m_BranchId = 0;
            m_Name = "";
            m_Bank = "";
            m_Account = "";
            m_Company = "";
            m_Memo = "";
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
            arrayList.Add(new SqlParameter("@BranchId", m_BranchId));
            arrayList.Add(new SqlParameter("@Name", m_Name));
            arrayList.Add(new SqlParameter("@Bank", m_Bank));
            arrayList.Add(new SqlParameter("@Account", m_Account));
            arrayList.Add(new SqlParameter("@Company", m_Company));
            arrayList.Add(new SqlParameter("@Memo", m_Memo));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("BankAccount", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("BankAccount", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from BankAccount where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                m_Name = DBTool.GetStringFromRow(row, "Name", "");
                m_Bank = DBTool.GetStringFromRow(row, "Bank", "");
                m_Account = DBTool.GetStringFromRow(row, "Account", "");
                m_Company = DBTool.GetStringFromRow(row, "Company", "");
                m_Memo = DBTool.GetStringFromRow(row, "Memo", "");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        /// <summary>
        /// ERP FOrderReceiveMoney.cs 中调用
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public DataSet ReadBankAccount(int branchId)
        {
            string sql = string.Format("select * from BankAccount where BranchId={0} ", branchId);
            return m_dbo.GetDataSet(sql);
        }
    }

    
     
}
