using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
namespace LeadingClass
{
    public class PurchaseStatement
    {
        private int m_Id;
        private int m_BranchId;
        private int m_SupplierId;
        private string m_Memo;
        private double m_SumMoney;
        private double m_PaidMoney;
        private string m_PayStatus;
        private int m_UserId;
        private DateTime m_UpdateTime;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public int SupplierId { get { return m_SupplierId; } set { m_SupplierId = value; } }
        public string Memo { get { return m_Memo; } set { m_Memo = value; } }
        public double SumMoney { get { return m_SumMoney; } set { m_SumMoney = value; } }
        public double PaidMoney { get { return m_PaidMoney; } set { m_PaidMoney = value; } }
        public string PayStatus { get { return m_PayStatus; } set { m_PayStatus = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public PurchaseStatement()
        {
            m_Id = 0;
            m_BranchId = 0;
            m_SupplierId = 0;
            m_Memo = "";
            m_SumMoney = 0;
            m_PaidMoney = 0;
            m_PayStatus = "";
            m_UserId = 0;
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
            arrayList.Add(new SqlParameter("@SupplierId", m_SupplierId));
            arrayList.Add(new SqlParameter("@Memo", m_Memo));
            arrayList.Add(new SqlParameter("@SumMoney", Math.Round(m_SumMoney, 2)));
            arrayList.Add(new SqlParameter("@PaidMoney", Math.Round(m_PaidMoney, 2)));
            arrayList.Add(new SqlParameter("@PayStatus", m_PayStatus));
            arrayList.Add(new SqlParameter("@UserId", m_UserId));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("PurchaseStatement", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("PurchaseStatement", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from PurchaseStatement where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                m_SupplierId = DBTool.GetIntFromRow(row, "SupplierId", 0);
                m_Memo = DBTool.GetStringFromRow(row, "Memo", "");
                m_SumMoney = DBTool.GetDoubleFromRow(row, "SumMoney", 0);
                m_PaidMoney = DBTool.GetDoubleFromRow(row, "PaidMoney", 0);
                m_PayStatus = DBTool.GetStringFromRow(row, "PayStatus", "");
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format(@"update Purchase set IsPaid=0 where IsPaid=2 and Id in (select  purchaseId from purchasestatementDetail where purchaseStatementId={0});
                                         delete from PurchaseStatementDetail where PurchaseStatementId={0};
                                         delete from PurchaseStatement where Id={0};", this.Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 添加新的付款记录后，更改对账单的付款金额和状态
        /// </summary>
        /// <returns></returns>
        public int UpdateStatus()
        {
            this.Load();
            string sql = string.Format("select SUM(ChargeOff) as ChargeOff,SUM(PayMoney) as PayMoney from PurchasePay where PurchaseStatementId={0} ", this.Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            double chargeOff = DBTool.GetDoubleFromRow(ds.Tables[0].Rows[0], "ChargeOff", 0);
            double payMoney = DBTool.GetDoubleFromRow(ds.Tables[0].Rows[0], "PayMoney", 0);
            this.PaidMoney = payMoney;
            if (chargeOff >= this.SumMoney)
            {
                if (chargeOff == payMoney)
                {
                    this.PayStatus = CommenClass.PayStatus.全额付清.ToString();
                }
                else if (chargeOff > payMoney)
                {
                    this.PayStatus = CommenClass.PayStatus.优惠付清.ToString();
                }
                else if (chargeOff < payMoney)
                {
                    this.PayStatus = CommenClass.PayStatus.超额付清.ToString();
                }
            }
            else if (chargeOff > 0)
            {
                this.PayStatus = CommenClass.PayStatus.部分付款.ToString();
            }
            else if (chargeOff == 0)
            {
                this.PayStatus = CommenClass.PayStatus.未付款.ToString();
            }
            return this.Save();
        }
    }

    public class PurchaseStatementDetail
    {
        private int m_Id;
        private int m_PurchaseStatementId;
        private int m_PurchaseId;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int PurchaseStatementId { get { return m_PurchaseStatementId; } set { m_PurchaseStatementId = value; } }
        public int PurchaseId { get { return m_PurchaseId; } set { m_PurchaseId = value; } }
        public PurchaseStatementDetail()
        {
            m_Id = 0;
            m_PurchaseStatementId = 0;
            m_PurchaseId = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@PurchaseStatementId", m_PurchaseStatementId));
            arrayList.Add(new SqlParameter("@PurchaseId", m_PurchaseId));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("PurchaseStatementDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("PurchaseStatementDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from PurchaseStatementDetail where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_PurchaseStatementId = DBTool.GetIntFromRow(row, "PurchaseStatementId", 0);
                m_PurchaseId = DBTool.GetIntFromRow(row, "PurchaseId", 0);
                return true;
            }
            return false;
        } 
 
    }

    public class PurchaseStatementManger
    {
        private DBOperate m_dbo;

        public PurchaseStatementManger()
        {
            m_dbo = new DBOperate();
        }
        /// <summary>
        /// 保存采购对账单
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="psds"></param>
        /// <returns></returns>
        public int SavePurchaseStatement(PurchaseStatement ps, PurchaseStatementDetail[] psds)
        {
            PurchaseStatement purchaseStatement = new PurchaseStatement();
            purchaseStatement.BranchId = ps.BranchId;
            purchaseStatement.Id = ps.Id;
            purchaseStatement.Memo = ps.Memo;
            purchaseStatement.PaidMoney = ps.PaidMoney;
            purchaseStatement.PayStatus = ps.PayStatus;
            purchaseStatement.SumMoney = ps.SumMoney;
            purchaseStatement.SupplierId = ps.SupplierId;
            purchaseStatement.UpdateTime = DateTime.Now;
            purchaseStatement.UserId = ps.UserId;
            int id = purchaseStatement.Save();
            if (id > 0)
            {
                for (int i = 0; i < psds.Length; i++)
                {
                    PurchaseStatementDetail psd = new PurchaseStatementDetail();
                    psd.Id = 0;
                    psd.PurchaseId = psds[i].PurchaseId;
                    psd.PurchaseStatementId = id;
                    if (psd.Save() > 0)
                    {
                        Purchase p = new Purchase();
                        p.Id = psds[i].PurchaseId;
                        p.Load();
                        p.IsPaid = 2;
                        p.Save();
                    }
                }
            }
            return id;
        }
        /// <summary>
        /// 读取采购对账单
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadPurchaseStatement(PurchaseStatementOption option)
        {
            string sql = " select * from view_PurchaseStatement where 1=1 ";
            if (option.BranchId > 0)
            {
                sql += string.Format(" and BranchId ={0} ", option.BranchId);
            }
            if (option.EndDate != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and UpdateTime < '{0}' ", option.EndDate.AddDays(1).Date.ToShortDateString());
            }
            if (option.PayStatus != "")
            {
                switch (option.PayStatus)
                {
                    case "未付款":
                        sql += string.Format(" and payStatus = '{0}' ", option.PayStatus);
                        break;
                    case "部分付款":
                        sql += string.Format(" and ( payStatus = '部分付款' or payStatus='未付款' ) ", option.PayStatus);
                        break;
                    case "全部状态":
                        break;
                    default:
                        break;
                }
            }
            if (option.StartDate != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and UpdateTime >= '{0}' ", option.StartDate.ToShortDateString());
            }
            if (option.SupplierId > 0)
            {
                sql += string.Format(" and SupplierId = {0} ", option.SupplierId);
            }
            return m_dbo.GetDataSet(sql);

        }
        /// <summary>
        /// 读取采购对账单明细
        /// </summary>
        /// <param name="purchaseStatementId"></param>
        /// <returns></returns>
        public DataSet ReadPurchaseStatementDetail(int purchaseStatementId)
        {
            string sql = string.Format("select * from View_PurchaseStatementDetail where purchasestatementid={0} order by purchasedate ", purchaseStatementId);
            return m_dbo.GetDataSet(sql);
        }

    }

    public class PurchaseStatementOption
    {
        private int m_BranchId;
        private int m_SupplierId;
        private DateTime m_StartDate;
        private DateTime m_EndDate;
        private string m_PayStatus;
        private string m_InvoiceStatus;
        private string m_InvoiceType;
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public int SupplierId { get { return m_SupplierId; } set { m_SupplierId = value; } }
        public DateTime StartDate { get { return m_StartDate; } set { m_StartDate = value; } }
        public DateTime EndDate { get { return m_EndDate; } set { m_EndDate = value; } }
        public string PayStatus { get { return m_PayStatus; } set { m_PayStatus = value; } }
        public string InvoiceStatus { get { return m_InvoiceStatus; } set { m_InvoiceStatus = value; } }
        public string InvoiceType { get { return m_InvoiceType; } set { m_InvoiceType = value; } }
        public PurchaseStatementOption()
        {
            m_BranchId = 0;
            m_SupplierId = 0;
            m_StartDate = new DateTime(1900, 1, 1);
            m_EndDate = new DateTime(1900, 1, 1);
            m_PayStatus = "";
            m_InvoiceType = "";
            m_InvoiceStatus = "";
        }

    }
}
