using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;


namespace LeadingClass
{
    public class PurchaseCashConfirm
    {
        private int m_Id;
        private int m_PurchaseUserId;
        private string m_Title;
        private double m_SumMoney;
        private string m_Memo;
        private DateTime m_UpdateTime;
        private int m_ConfirmUserId;
        private int m_BranchId;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int PurchaseUserId { get { return m_PurchaseUserId; } set { m_PurchaseUserId = value; } }
        public string Title { get { return m_Title; } set { m_Title = value; } }
        public double SumMoney { get { return m_SumMoney; } set { m_SumMoney = value; } }
        public string Memo { get { return m_Memo; } set { m_Memo = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public int ConfirmUserId { get { return m_ConfirmUserId; } set { m_ConfirmUserId = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public PurchaseCashConfirm()
        {
            m_Id = 0;
            m_PurchaseUserId = 0;
            m_Title = "";
            m_SumMoney = 0;
            m_Memo = "";
            m_UpdateTime = DateTime.Now;
            m_ConfirmUserId = 0;
            m_BranchId = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@PurchaseUserId", m_PurchaseUserId));
            arrayList.Add(new SqlParameter("@Title", m_Title));
            arrayList.Add(new SqlParameter("@SumMoney", Math.Round(m_SumMoney, 2)));
            arrayList.Add(new SqlParameter("@Memo", m_Memo));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            arrayList.Add(new SqlParameter("@ConfirmUserId", m_ConfirmUserId));
            arrayList.Add(new SqlParameter("@BranchId", m_BranchId));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("PurchaseCashConfirm", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("PurchaseCashConfirm", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from PurchaseCashConfirm where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_PurchaseUserId = DBTool.GetIntFromRow(row, "PurchaseUserId", 0);
                m_Title = DBTool.GetStringFromRow(row, "Title", "");
                m_SumMoney = DBTool.GetDoubleFromRow(row, "SumMoney", 0);
                m_Memo = DBTool.GetStringFromRow(row, "Memo", "");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_ConfirmUserId = DBTool.GetIntFromRow(row, "ConfirmUserId", 0);
                m_BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                return true;
            }
            return false;
        } 
 
    }

    public class PurchaseCashConfirmDetail
    {
        private int m_Id;
        private int m_PurchaseCashConfirmId;
        private int m_PurchaseId;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int PurchaseCashConfirmId { get { return m_PurchaseCashConfirmId; } set { m_PurchaseCashConfirmId = value; } }
        public int PurchaseId { get { return m_PurchaseId; } set { m_PurchaseId = value; } }
        public PurchaseCashConfirmDetail()
        {
            m_Id = 0;
            m_PurchaseCashConfirmId = 0;
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
            arrayList.Add(new SqlParameter("@PurchaseCashConfirmId", m_PurchaseCashConfirmId));
            arrayList.Add(new SqlParameter("@PurchaseId", m_PurchaseId));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("PurchaseCashConfirmDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("PurchaseCashConfirmDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from PurchaseCashConfirmDetail where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_PurchaseCashConfirmId = DBTool.GetIntFromRow(row, "PurchaseCashConfirmId", 0);
                m_PurchaseId = DBTool.GetIntFromRow(row, "PurchaseId", 0);
                return true;
            }
            return false;
        } 
 
    }

    public class PurchaseCashConfirmOption
    {
        private int m_PurchaseUserId;
        private DateTime m_StartDate;
        private DateTime m_EndDate;
        private int m_BranchId;
        public int PurchaseUserId { get { return m_PurchaseUserId; } set { m_PurchaseUserId = value; } }
        public DateTime StartDate { get { return m_StartDate; } set { m_StartDate = value; } }
        public DateTime EndDate { get { return m_EndDate; } set { m_EndDate = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public PurchaseCashConfirmOption()
        {
            m_PurchaseUserId = 0;
            m_StartDate = new DateTime(1900, 1, 1);
            m_EndDate = new DateTime(1900, 1, 1);
            m_BranchId = 0;
        }

    }

    public class PurchaseCashConfirmManager
    {
        private DBOperate m_dbo;
        public PurchaseCashConfirmManager()
        {
            m_dbo = new DBOperate();
        }
        public int SavePurchaseCashConfirm(PurchaseCashConfirm pcc, PurchaseCashConfirmDetail[] pccds)
        {
            PurchaseCashConfirm purchasecashConfirm = new PurchaseCashConfirm();
            purchasecashConfirm.ConfirmUserId = pcc.ConfirmUserId;
            purchasecashConfirm.Id = pcc.Id;
            purchasecashConfirm.Memo = pcc.Memo;
            purchasecashConfirm.PurchaseUserId = pcc.PurchaseUserId;
            purchasecashConfirm.SumMoney = pcc.SumMoney;
            purchasecashConfirm.Title = pcc.Title;
            purchasecashConfirm.UpdateTime = DateTime.Now;
            purchasecashConfirm.BranchId = pcc.BranchId;
            int pccId = purchasecashConfirm.Save();
            if (pccId > 0)
            {
                for (int i = 0; i < pccds.Length; i++)
                {
                    PurchaseCashConfirmDetail pccd = new PurchaseCashConfirmDetail();
                    pccd.Id = pccds[i].Id;
                    pccd.PurchaseCashConfirmId = pccId;
                    pccd.PurchaseId = pccds[i].PurchaseId;
                    if (pccd.Save()>0)
                    {
                        Purchase p = new Purchase();
                        p.Id = pccds[i].PurchaseId;
                        p.Load();
                        p.IsCashConfirm = 1;
                        p.Save();
                    }
                }
            }
            return pccId;
        }

        public DataSet ReadPurchaseCashConfirm(PurchaseCashConfirmOption option)
        {
            string sql = " select * from PurchaseCashConfirm where 1=1 ";
            if (option.BranchId != 0)
            {
                sql += string.Format(" and BranchId={0}", option.BranchId);
            }
            if (option.EndDate != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and UpdateTime < '{0}' ", option.EndDate.AddDays(1).ToShortDateString());
            }
            if (option.StartDate != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and UpdateTime >= '{0}' ", option.StartDate.ToShortDateString());
            }
            if (option.PurchaseUserId > 0)
            {
                sql += string.Format(" and PurchaseUserId ={0} ", option.PurchaseUserId);
            }
            sql += " order by Id";
            return m_dbo.GetDataSet(sql);
        }

        public DataSet ReadPurchaseCashConfirmDetail(int purchaseCashConfirmId)
        {
            string sql =string.Format( "select * from View_PurchaseCashConfirmDetail where purchaseCashConfirmId={0} order by purchaseId ",purchaseCashConfirmId);
            return m_dbo.GetDataSet(sql);

        }
        public bool DeletePurchaseCahsConfirm(int PurchaseCashConfirmId)
        {
            string sql = string.Format(@" update Purchase set IsCashConfirm=0 where Id in ( select PurchaseId from PurchaseCashConfirmDetail where PurchaseCashConfirmId = {0});
delete from PurchaseCashConfirmDetail where PurchaseCashConfirmId={0} ;
delete from PurchaseCashConfirm where Id={0} ",PurchaseCashConfirmId);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}
