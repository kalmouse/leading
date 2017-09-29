using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using CommenClass;

namespace LeadingClass
{
    public class Purchase
    {
        private int m_Id;
        private int m_BranchId;
        private int m_StoreId;
        private int m_SupplierId;
        private string m_PurchaseType;
        private double m_SumMoney;
        private DateTime m_PurchaseDate;
        private int m_PurchaseUserId;
        private int m_IsPaid;//0:未付款，1：已付款，2：已对账
        private int m_IsTax;
        private string m_Memo;
        private int m_IsConfirm;
        private int m_IsCashConfirm;
        private DateTime m_UpdateTime;
        private int m_UserId;
        private string m_PurchaseStatus;
        private int m_NeedToPurchaseId;
        private int m_ReceiptUserId;
        private DateTime m_ConfirmDate;
        public double Tax { get; set; }
        public double TaxRate { get; set; }
       
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public int StoreId { get { return m_StoreId; } set { m_StoreId = value; } }
        public int SupplierId { get { return m_SupplierId; } set { m_SupplierId = value; } }
        public string PurchaseType { get { return m_PurchaseType; } set { m_PurchaseType = value; } }
        public double SumMoney { get { return m_SumMoney; } set { m_SumMoney = value; } }
        public DateTime PurchaseDate { get { return m_PurchaseDate; } set { m_PurchaseDate = value; } }
        public int PurchaseUserId { get { return m_PurchaseUserId; } set { m_PurchaseUserId = value; } }
        public int IsPaid { get { return m_IsPaid; } set { m_IsPaid = value; } }
        public int IsTax { get { return m_IsTax; } set { m_IsTax = value; } }
        public string Memo { get { return m_Memo; } set { m_Memo = value; } }
        public int IsConfirm { get { return m_IsConfirm; } set { m_IsConfirm = value; } }
        public int IsCashConfirm { get { return m_IsCashConfirm; } set { m_IsCashConfirm = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public string PurchaseStatus { get { return m_PurchaseStatus; } set { m_PurchaseStatus = value; } }
        public int NeedToPurchaseId { get { return m_NeedToPurchaseId; } set { m_NeedToPurchaseId = value; } }
        public int ReceiptUserId { get { return m_ReceiptUserId; } set { m_ReceiptUserId = value; } }
        public DateTime ConfirmDate { get { return m_ConfirmDate; } set { m_ConfirmDate = value; } }
        public Purchase()
        {
            m_Id = 0;
            m_BranchId = 0;
            m_StoreId = 0;
            m_SupplierId = 0;
            m_PurchaseType = "";
            m_SumMoney = 0;
            m_PurchaseDate = DateTime.Now;
            m_PurchaseUserId = 0;
            m_IsPaid = 0;
            m_IsTax = 0;
            m_Memo = "";
            m_IsConfirm = 0;
            m_IsCashConfirm = 0;
            m_UpdateTime = DateTime.Now;
            m_UserId = 0;
            m_PurchaseStatus = "";
            m_NeedToPurchaseId = 0;
            m_ReceiptUserId=0;
            m_ConfirmDate = Convert.ToDateTime("2000-01-01");
            Tax = 0;
            TaxRate = 0;
         
            m_dbo = new DBOperate();
        }
        public Purchase(int Id)
            : this()
        {
            this.Id = Id;
            this.Load();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@BranchId", m_BranchId));
            arrayList.Add(new SqlParameter("@StoreId", m_StoreId));
            arrayList.Add(new SqlParameter("@SupplierId", m_SupplierId));
            arrayList.Add(new SqlParameter("@PurchaseType", m_PurchaseType));
            arrayList.Add(new SqlParameter("@SumMoney", Math.Round(m_SumMoney, 2)));
            arrayList.Add(new SqlParameter("@PurchaseDate", m_PurchaseDate));
            arrayList.Add(new SqlParameter("@PurchaseUserId", m_PurchaseUserId));
            arrayList.Add(new SqlParameter("@IsTax", m_IsTax));
            arrayList.Add(new SqlParameter("@Memo", m_Memo));
            arrayList.Add(new SqlParameter("@IsConfirm", m_IsConfirm));
            arrayList.Add(new SqlParameter("@IsCashConfirm", m_IsCashConfirm));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            arrayList.Add(new SqlParameter("@UserId", m_UserId));
            arrayList.Add(new SqlParameter("@IsPaid", m_IsPaid));
            arrayList.Add(new SqlParameter("@PurchaseStatus", m_PurchaseStatus));
            arrayList.Add(new SqlParameter("@NeedToPurchaseId", m_NeedToPurchaseId));
            arrayList.Add(new SqlParameter("@ReceiptUserId",m_ReceiptUserId));
            arrayList.Add(new SqlParameter("@ConfirmDate", m_ConfirmDate));
            arrayList.Add(new SqlParameter("@Tax", Tax));
            arrayList.Add(new SqlParameter("@TaxRate", TaxRate));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Purchase", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {

                this.Id = m_dbo.InsertData("Purchase", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Purchase where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                m_StoreId = DBTool.GetIntFromRow(row, "StoreId", 0);
                m_SupplierId = DBTool.GetIntFromRow(row, "SupplierId", 0);
                m_PurchaseType = DBTool.GetStringFromRow(row, "PurchaseType", "");
                m_SumMoney = DBTool.GetDoubleFromRow(row, "SumMoney", 0);
                m_PurchaseDate = DBTool.GetDateTimeFromRow(row, "PurchaseDate");
                m_PurchaseUserId = DBTool.GetIntFromRow(row, "PurchaseUserId", 0);
                m_IsPaid = DBTool.GetIntFromRow(row, "IsPaid", 0);
                m_IsTax = DBTool.GetIntFromRow(row, "IsTax", 0);
                m_IsConfirm = DBTool.GetIntFromRow(row, "IsConfirm", 0);
                m_IsCashConfirm = DBTool.GetIntFromRow(row, "IsCashConfirm", 0);
                m_Memo = DBTool.GetStringFromRow(row, "Memo", "");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_PurchaseStatus = DBTool.GetStringFromRow(row, "PurchaseStatus", "");
                m_NeedToPurchaseId = DBTool.GetIntFromRow(row, "NeedToPurchaseId", 0);
                m_ReceiptUserId=DBTool.GetIntFromRow(row,"ReceiptUserId",0);
                m_ConfirmDate = DBTool.GetDateTimeFromRow(row, "ConfirmDate");
                Tax= DBTool.GetDoubleFromRow(row, "Tax", 0);
                TaxRate = DBTool.GetDoubleFromRow(row, "TaxRate", 0); 
               
                return true;
            }
            return false;
        }
        public bool UpdatePurchaseStatus(int Pid)
        {
            string sql = string.Format("update Purchase set PurchaseStatus='已确认',ConfirmDate='"+DateTime.Now.ToString()+"' where Id={0}", Pid);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 采购单保存完成后计算采购单金额
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdatePurchaseSumMoney(int id)
        {
            string sql = string.Format("update Purchase set SumMoney=(select ISNULL(SUM(Amount),0) from PurchaseDetail where PurchaseId={0}) , Tax=(select ISNULL(SUM(Amount),0) from PurchaseDetail where PurchaseId={0})/(1+TaxRate)*TaxRate  where Id={0}", id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }

    public class PurchaseDetail
    {
        private int m_Id;
        private int m_PurchaseId;
        private int m_GoodsId;
        private int m_StoreId;
        private string m_Model;
        private double m_BillsPrice;
        private double m_InPrice;
        private int m_Num;
        private double m_Amount;
        private int m_OldStore;
        private double m_OldAC;
        private int m_ReceivedNum;
        private int m_Emergency;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int PurchaseId { get { return m_PurchaseId; } set { m_PurchaseId = value; } }
        public int GoodsId { get { return m_GoodsId; } set { m_GoodsId = value; } }
        public int StoreId { get { return m_StoreId; } set { m_StoreId = value; } }
        public string Model { get { return m_Model; } set { m_Model = value; } }
        public double BillsPrice { get { return m_BillsPrice; } set { m_BillsPrice = value; } }
        public double InPrice { get { return m_InPrice; } set { m_InPrice = value; } }
        public int Num { get { return m_Num; } set { m_Num = value; } }
        public double Amount { get { return m_Amount; } set { m_Amount = value; } }
        public int OldStore { get { return m_OldStore; } set { m_OldStore = value; } }
        public double OldAC { get { return m_OldAC; } set { m_OldAC = value; } }
        public int ReceivedNum { get { return m_ReceivedNum; } set { m_ReceivedNum = value; } }
        public int Emergency { get { return m_Emergency; } set { m_Emergency = value; } }
        public double TaxInPrice { get; set; }        
        public PurchaseDetail()
        {
            m_Id = 0;
            m_PurchaseId = 0;
            m_GoodsId = 0;
            m_StoreId = 0;
            m_Model = "";
            m_BillsPrice = 0;
            m_InPrice = 0;
            m_Num = 0;
            m_Amount = 0;
            m_OldStore = 0;
            m_OldAC = 0;
            m_ReceivedNum = 0;
            m_Emergency = 0;
            TaxInPrice = 0;            
            m_dbo = new DBOperate();
        }
        public PurchaseDetail(int Id)
            : this()
        {
            this.Id = Id;
            this.Load();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@PurchaseId", m_PurchaseId));
            arrayList.Add(new SqlParameter("@GoodsId", m_GoodsId));
            arrayList.Add(new SqlParameter("@StoreId", m_StoreId));
            arrayList.Add(new SqlParameter("@Model", m_Model));
            arrayList.Add(new SqlParameter("@BillsPrice", m_BillsPrice));
            arrayList.Add(new SqlParameter("@InPrice", m_InPrice));
            arrayList.Add(new SqlParameter("@Num", m_Num));
            arrayList.Add(new SqlParameter("@Amount", m_Amount));
            arrayList.Add(new SqlParameter("@OldStore", m_OldStore));
            arrayList.Add(new SqlParameter("@OldAC", m_OldAC));
            arrayList.Add(new SqlParameter("@ReceivedNum", m_ReceivedNum));
            arrayList.Add(new SqlParameter("@Emergency", m_Emergency));
            arrayList.Add(new SqlParameter("@TaxInPrice", TaxInPrice));           
            if (this.Id > 0)
            {
                m_dbo.UpdateData("PurchaseDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("PurchaseDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from PurchaseDetail where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_PurchaseId = DBTool.GetIntFromRow(row, "PurchaseId", 0);
                m_GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                m_StoreId = DBTool.GetIntFromRow(row, "StoreId", 0);
                m_Model = DBTool.GetStringFromRow(row, "Model", "");
                m_BillsPrice = DBTool.GetDoubleFromRow(row, "BillsPrice", 0);
                m_InPrice = DBTool.GetDoubleFromRow(row, "InPrice", 0);
                m_Num = DBTool.GetIntFromRow(row, "Num", 0);
                m_Amount = DBTool.GetDoubleFromRow(row, "Amount", 0);
                m_OldStore = DBTool.GetIntFromRow(row, "OldStore", 0);
                m_OldAC = DBTool.GetDoubleFromRow(row, "OldAC", 0);
                m_ReceivedNum = DBTool.GetIntFromRow(row, "ReceivedNum", 0);
                m_Emergency = DBTool.GetIntFromRow(row, "Emergency", 0);
                TaxInPrice = DBTool.GetDoubleFromRow(row, "TaxInPrice", 0);               
                return true;
            }
            return false;
        }

        public bool Load(int pId, int goodsId)
        {
            string sql = string.Format("select * from PurchaseDetail where PurchaseId={0} and GoodsId={1}",pId,goodsId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_PurchaseId = DBTool.GetIntFromRow(row, "PurchaseId", 0);
                m_GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                m_StoreId = DBTool.GetIntFromRow(row, "StoreId", 0);
                m_Model = DBTool.GetStringFromRow(row, "Model", "");
                m_BillsPrice = DBTool.GetDoubleFromRow(row, "BillsPrice", 0);
                m_InPrice = DBTool.GetDoubleFromRow(row, "InPrice", 0);
                m_Num = DBTool.GetIntFromRow(row, "Num", 0);
                m_Amount = DBTool.GetDoubleFromRow(row, "Amount", 0);
                m_OldStore = DBTool.GetIntFromRow(row, "OldStore", 0);
                m_OldAC = DBTool.GetDoubleFromRow(row, "OldAC", 0);
                m_ReceivedNum = DBTool.GetIntFromRow(row, "ReceivedNum", 0);
                m_Emergency = DBTool.GetIntFromRow(row, "Emergency", 0);
                TaxInPrice = DBTool.GetDoubleFromRow(row, "TaxInPrice", 0);               
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format(" delete from purchaseDetail where id={0} ", this.Id);
            return m_dbo.ExecuteNonQuery(sql);
        }


        public bool Revoke()
        {
            string sql = string.Format("update PurchaseDetail set ReceivedNum=0 where PurchaseId={0} ", this.PurchaseId);
            return m_dbo.ExecuteNonQuery(sql);
        }
     
    }

    public class PurchaseModify
    {
        private int m_Id;
        private int m_PurchaseId;
        private int m_GoodsId;
        private string m_Model;
        private int m_OldNum;
        private int m_NewNum;
        private double m_OldPrice;
        private double m_NewPrice;
        private int m_UserId;
        private DateTime m_UpdateTime;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int PurchaseId { get { return m_PurchaseId; } set { m_PurchaseId = value; } }
        public int GoodsId { get { return m_GoodsId; } set { m_GoodsId = value; } }
        public string Model { get { return m_Model; } set { m_Model = value; } }
        public int OldNum { get { return m_OldNum; } set { m_OldNum = value; } }
        public int NewNum { get { return m_NewNum; } set { m_NewNum = value; } }
        public double OldPrice { get { return m_OldPrice; } set { m_OldPrice = value; } }
        public double NewPrice { get { return m_NewPrice; } set { m_NewPrice = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public PurchaseModify()
        {
            m_Id = 0;
            m_PurchaseId = 0;
            m_GoodsId = 0;
            m_Model = "";
            m_OldNum = 0;
            m_NewNum = 0;
            m_OldPrice = 0;
            m_NewPrice = 0;
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
            arrayList.Add(new SqlParameter("@PurchaseId", m_PurchaseId));
            arrayList.Add(new SqlParameter("@GoodsId", m_GoodsId));
            arrayList.Add(new SqlParameter("@Model", m_Model));
            arrayList.Add(new SqlParameter("@OldNum", m_OldNum));
            arrayList.Add(new SqlParameter("@NewNum", m_NewNum));
            arrayList.Add(new SqlParameter("@OldPrice", m_OldPrice));
            arrayList.Add(new SqlParameter("@NewPrice", m_NewPrice));
            arrayList.Add(new SqlParameter("@UserId", m_UserId));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("PurchaseModify", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("PurchaseModify", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from PurchaseModify where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_PurchaseId = DBTool.GetIntFromRow(row, "PurchaseId", 0);
                m_GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                m_Model = DBTool.GetStringFromRow(row, "Model", "");
                m_OldNum = DBTool.GetIntFromRow(row, "OldNum", 0);
                m_NewNum = DBTool.GetIntFromRow(row, "NewNum", 0);
                m_OldPrice = DBTool.GetDoubleFromRow(row, "OldPrice", 0);
                m_NewPrice = DBTool.GetDoubleFromRow(row, "NewPrice", 0);
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
      

    }
}
