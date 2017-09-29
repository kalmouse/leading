using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class PurchasePay
    {
        private int m_Id;
        private int m_BranchId;
        private int m_PurchaseStatementId;
        private double m_PayMoney;
        private string m_PayStatus;//支付后的状态
        private int m_BankAccountId;//银行账户
        private string m_PayType;//支付方式：现金、支票等
        private DateTime m_PayDate;//支付日期
        private string m_Memo;
        private int m_UserId;
        private DateTime m_UpdateTime;
        private double m_ChargeOff;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public int PurchaseStatementId { get { return m_PurchaseStatementId; } set { m_PurchaseStatementId = value; } }
        public double PayMoney { get { return m_PayMoney; } set { m_PayMoney = value; } }
        public string PayStatus { get { return m_PayStatus; } set { m_PayStatus = value; } }
        public int BankAccountId { get { return m_BankAccountId; } set { m_BankAccountId = value; } }
        public string PayType { get { return m_PayType; } set { m_PayType = value; } }
        public string Memo { get { return m_Memo; } set { m_Memo = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public DateTime PayDate { get { return m_PayDate; } set { m_PayDate = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public double ChargeOff { get { return m_ChargeOff; } set { m_ChargeOff = value; } }
        public PurchasePay()
        {
            m_Id = 0;
            m_BranchId = 0;
            m_PurchaseStatementId = 0;
            m_PayMoney = 0;
            m_PayStatus = "";
            m_BankAccountId = 0;
            m_PayType = "";
            m_Memo = "";
            m_UserId = 0;
            m_PayDate = DateTime.Now;
            m_UpdateTime = DateTime.Now;
            m_ChargeOff = 0;
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
            arrayList.Add(new SqlParameter("@PurchaseStatementId", m_PurchaseStatementId));
            arrayList.Add(new SqlParameter("@PayMoney", Math.Round(m_PayMoney, 2)));
            arrayList.Add(new SqlParameter("@PayStatus", m_PayStatus));
            arrayList.Add(new SqlParameter("@BankAccountId", m_BankAccountId));
            arrayList.Add(new SqlParameter("@PayType", m_PayType));
            arrayList.Add(new SqlParameter("@Memo", m_Memo));
            arrayList.Add(new SqlParameter("@UserId", m_UserId));
            arrayList.Add(new SqlParameter("@PayDate", m_PayDate));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            arrayList.Add(new SqlParameter("@ChargeOff", Math.Round(m_ChargeOff, 2)));


            if (this.Id > 0)
            {
                m_dbo.UpdateData("PurchasePay", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
                //新增
            }
            else
            {
                this.Id = m_dbo.InsertData("PurchasePay", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
                //修改
            }
            if (this.Id > 0)
            {
                PurchaseStatement ps = new PurchaseStatement();
                ps.Id = this.PurchaseStatementId;
                ps.UpdateStatus();
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from PurchasePay where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                m_PurchaseStatementId = DBTool.GetIntFromRow(row, "PurchaseStatementId", 0);
                m_PayMoney = DBTool.GetDoubleFromRow(row, "PayMoney", 0);
                m_PayStatus = DBTool.GetStringFromRow(row, "PayStatus", "");
                m_BankAccountId = DBTool.GetIntFromRow(row, "BankAccountId", 0);
                m_PayType = DBTool.GetStringFromRow(row, "PayType", "");
                m_Memo = DBTool.GetStringFromRow(row, "Memo", "");
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_PayDate = DBTool.GetDateTimeFromRow(row, "PayDate");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_ChargeOff = DBTool.GetDoubleFromRow(row, "ChargeOff", 0);
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            this.Load();
            PurchaseStatement ps = new PurchaseStatement();
            ps.Id = this.PurchaseStatementId;
            string sql = string.Format(" delete from PurchasePay where Id={0} ", this.Id);
            if (m_dbo.ExecuteNonQuery(sql))
            {
                ps.UpdateStatus();
                return true;
            }
            else return false;

        }
     }

    public class PurchasePayManager
    {
        private DBOperate m_dbo;

        public PurchasePayManager()
        {
            m_dbo = new DBOperate();
        }

        public DataSet ReadPurchasePay(PurchasePayOption option)
        {
            string sql = "select * from view_purchasePay where 1=1";
            if (option.BankAccountId > 0)
            {
                sql += string.Format(" and BankAccountId ={0} ", option.BankAccountId);
            }
            if (option.BranchId > 0)
            {
                sql += string.Format(" and BranchId={0} ", option.BranchId);
            }
            if (option.PayMoneyE > 0)
            {
                sql += string.Format(" and PayMoney <= {0} ", option.PayMoneyE);
            }
            if (option.PayMoneyS > 0)
            {
                sql += string.Format(" and PayMoney >= {0} ", option.PayMoneyS);
            }
            if (option.PayStatus != "")
            {
                sql += string.Format(" and PayStatus = '{0}' ", option.PayStatus);
            }
            if (option.PayType != "")
            {
                sql += string.Format(" and PayType='{0}' ", option.PayType);
            }
            if (option.PurchaseStatementId > 0)
            {
                sql += string.Format(" and PurchaseStatementId={0} ", option.PurchaseStatementId);
            }
            if (option.SupplierId > 0)
            {
                sql += string.Format(" and SupplierId = {0} ", option.SupplierId);
            }
            if (option.PayDateE != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and PayDate < '{0}' ", option.PayDateE.AddDays(1).ToShortDateString());
            }
            if (option.PayDateS != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and PayDate >= '{0}' ", option.PayDateS.ToShortDateString());
            }
            if (option.UserId > 0)
            {
                sql += string.Format(" and UserId = {0} ", option.UserId);
            }
            sql += " order by PurchaseStatementId, Id ";

            return m_dbo.GetDataSet(sql);
        }
    }

    public class PurchasePayOption
    {
        private int m_BranchId;
        private int m_PurchaseStatementId;
        private double m_PayMoneyS;
        private double m_PayMoneyE;
        private string m_PayStatus;
        private int m_BankAccountId;
        private string m_PayType;
        private int m_SupplierId;
        private int m_UserId;
        private DateTime m_PayDateS;
        private DateTime m_PayDateE;

        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public int PurchaseStatementId { get { return m_PurchaseStatementId; } set { m_PurchaseStatementId = value; } }
        public double PayMoneyS { get { return m_PayMoneyS; } set { m_PayMoneyS = value; } }
        public double PayMoneyE { get { return m_PayMoneyE; } set { m_PayMoneyE = value; } }
        public string PayStatus { get { return m_PayStatus; } set { m_PayStatus = value; } }
        public int BankAccountId { get { return m_BankAccountId; } set { m_BankAccountId = value; } }
        public string PayType { get { return m_PayType; } set { m_PayType = value; } }
        public int SupplierId { get { return m_SupplierId; } set { m_SupplierId = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public DateTime PayDateS { get { return m_PayDateS; } set { m_PayDateS = value; } }
        public DateTime PayDateE { get { return m_PayDateE; } set { m_PayDateE = value; } }
        public PurchasePayOption()
        {
            m_BranchId = 0;
            m_PurchaseStatementId = 0;
            m_PayMoneyS = 0;
            m_PayMoneyE = 0;
            m_PayStatus = "";
            m_BankAccountId = 0;
            m_PayType = "";
            m_SupplierId = 0;
            m_UserId = 0;
            m_PayDateS = new DateTime(1900, 1, 1);
            m_PayDateE = new DateTime(1900, 1, 1);
        }

     }
}
