//2017-4-18 yanghaiyang   需求008

using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using System.Data;
using System.Data.SqlClient;


namespace LeadingClass
{
    public class Supplier
    {
        private int m_Id;
        private DateTime m_UpdateTime;
        private DateTime m_AddTime;
        private string m_Company;
        private string m_ShortName;
        private string m_Type;
        private string m_Attitude;
        private int m_PriceSheet;
        private string m_InvoiceType;
        private string m_PayType;
        private int m_YearTarget;
        private double m_MonthAmount;
        private string m_LinkMan1;
        private string m_LinkMan2;
        private string m_TelPhone1;
        private string m_TelPhone2;
        private string m_Mobile1;
        private string m_Mobile2;
        private string m_QQ1;
        private string m_QQ2;
        private string m_Address;
        private string m_MainBrand;
        private string m_Major;
        private int m_BillsDiscount;
        private int m_Branchid;
        private string m_ContractType;
        private string m_IsChangRefund;
        private string m_PurchasRewards;
        private string m_PaymentMethod;
        private string m_AccountNum;
        private string m_TaxpayerIdentificationCode;
        private string m_OpenBank;
        private string m_FinancialLinkMan;
        private string m_FinancialTelPhone;
        private string m_Other;
        private int m_GoodsTypeId;
        private string m_PY;
        private double m_TaxRate;
        private string m_OfficeAddress;
        private string m_WarhouseAddress;
        private string m_Delivery;
        private string m_DeliveryRegion;
  
        
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public DateTime AddTime { get { return m_AddTime; } set { m_AddTime = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public string Company { get { return m_Company; } set { m_Company = value; } }
        public string ShortName { get { return m_ShortName; } set { m_ShortName = value; } }
        public string Type { get { return m_Type; } set { m_Type = value; } }
        public string Attitude { get { return m_Attitude; } set { m_Attitude = value; } }
        public int PriceSheet { get { return m_PriceSheet; } set { m_PriceSheet = value; } }
        public string InvoiceType { get { return m_InvoiceType; } set { m_InvoiceType = value; } }
        public string PayType { get { return m_PayType; } set { m_PayType = value; } }
        public int YearTarget { get { return m_YearTarget; } set { m_YearTarget = value; } }
        public double MonthAmount { get { return m_MonthAmount; } set { m_MonthAmount = value; } }
        public string LinkMan1 { get { return m_LinkMan1; } set { m_LinkMan1 = value; } }
        public string LinkMan2 { get { return m_LinkMan2; } set { m_LinkMan2 = value; } }
        public string TelPhone1 { get { return m_TelPhone1; } set { m_TelPhone1 = value; } }
        public string TelPhone2 { get { return m_TelPhone2; } set { m_TelPhone2 = value; } }
        public string Mobile1 { get { return m_Mobile1; } set { m_Mobile1 = value; } }
        public string Mobile2 { get { return m_Mobile2; } set { m_Mobile2 = value; } }
        public string QQ1 { get { return m_QQ1; } set { m_QQ1 = value; } }
        public string QQ2 { get { return m_QQ2; } set { m_QQ2 = value; } }
        public string Address { get { return m_Address; } set { m_Address = value; } }
        public string MainBrand { get { return m_MainBrand; } set { m_MainBrand = value; } }
        public string Major { get { return m_Major; } set { m_Major = value; } }
        public int BillsDiscount { get { return m_BillsDiscount; } set { m_BillsDiscount = value; } }
        public int BranchId { get { return m_Branchid; } set { m_Branchid = value; } }
        public string ContractType { get { return m_ContractType; } set { m_ContractType = value; } }
        public string IsChangRefund { get { return m_IsChangRefund; } set { m_IsChangRefund = value; } }
        public string PurchasRewards { get { return m_PurchasRewards; } set { m_PurchasRewards = value; } }
        public string PaymentMethod { get { return m_PaymentMethod; } set { m_PaymentMethod = value; } }
        public string AccountNum { get { return m_AccountNum; } set { m_AccountNum = value; } }
        public string TaxpayerIdentificationCode { get { return m_TaxpayerIdentificationCode; } set { m_TaxpayerIdentificationCode = value; } }
        public string OpenBank { get { return m_OpenBank; } set { m_OpenBank = value; } }
        public string FinancialLinkMan { get { return m_FinancialLinkMan; } set { m_FinancialLinkMan = value; } }
        public string FinancialTelPhone { get { return m_FinancialTelPhone; } set { m_FinancialTelPhone = value; } }
        public string Other { get { return m_Other; } set { m_Other = value; } }
        public int GoodsTypeId { get { return m_GoodsTypeId; } set { m_GoodsTypeId = value; } }
        public string PY { get { return m_PY ; } set { m_PY = value; } }
        public double TaxRate { get { return m_TaxRate; } set { m_TaxRate = value; } }
        public string OfficeAddress { get { return m_OfficeAddress; } set { m_OfficeAddress = value; } }
        public string WarhouseAddress { get { return m_WarhouseAddress; } set { m_WarhouseAddress = value; } }
        public string Delivery { get { return m_Delivery; } set { m_Delivery = value; } }
        public string DeliveryRegion { get { return m_DeliveryRegion; } set { m_DeliveryRegion = value; } }
        public Supplier()
        {
            m_Id = 0;
            m_AddTime = DateTime.Now;
            m_UpdateTime = DateTime.Now;
            m_Company = "";
            m_ShortName = "";
            m_Type = "";
            m_Attitude = "";
            m_PriceSheet = 0;
            m_InvoiceType = "";
            m_PayType = "";
            m_YearTarget = 0;
            m_MonthAmount = 0;
            m_LinkMan1 = "";
            m_LinkMan2 = "";
            m_TelPhone1 = "";
            m_TelPhone2 = "";
            m_Mobile1 = "";
            m_Mobile2 = "";
            m_QQ1 = "";
            m_QQ2 = "";
            m_Address = "";
            m_MainBrand = "";
            m_Major = "";
            m_BillsDiscount = 100;
            m_Branchid = 0;
            m_ContractType = "";
            m_IsChangRefund = "";
            m_PurchasRewards = "";
            m_PaymentMethod = "";
            m_AccountNum = "";
            m_TaxpayerIdentificationCode = "";
            m_OpenBank = "";
            m_FinancialLinkMan = "";
            m_FinancialTelPhone = "";
            m_Other = "";
            m_GoodsTypeId = 0;
            m_PY = "";
            m_TaxRate = 0;
            m_WarhouseAddress = "";
            m_OfficeAddress = "";
            m_Delivery = "";
            m_DeliveryRegion = "";
            m_dbo = new DBOperate();
        }
        public Supplier(int Id)
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
            arrayList.Add(new SqlParameter("@AddTime", m_AddTime));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            arrayList.Add(new SqlParameter("@Company", m_Company));
            arrayList.Add(new SqlParameter("@ShortName", m_ShortName));
            arrayList.Add(new SqlParameter("@Type", m_Type));
            arrayList.Add(new SqlParameter("@Attitude", m_Attitude));
            arrayList.Add(new SqlParameter("@PriceSheet", m_PriceSheet));
            arrayList.Add(new SqlParameter("@InvoiceType", m_InvoiceType));
            arrayList.Add(new SqlParameter("@PayType", m_PayType));
            arrayList.Add(new SqlParameter("@YearTarget", m_YearTarget));
            arrayList.Add(new SqlParameter("@MonthAmount", m_MonthAmount));
            arrayList.Add(new SqlParameter("@LinkMan1", m_LinkMan1));
            arrayList.Add(new SqlParameter("@LinkMan2", m_LinkMan2));
            arrayList.Add(new SqlParameter("@TelPhone1", m_TelPhone1));
            arrayList.Add(new SqlParameter("@TelPhone2", m_TelPhone2));
            arrayList.Add(new SqlParameter("@Mobile1", m_Mobile1));
            arrayList.Add(new SqlParameter("@Mobile2", m_Mobile2));
            arrayList.Add(new SqlParameter("@QQ1", m_QQ1));
            arrayList.Add(new SqlParameter("@QQ2", m_QQ2));
            arrayList.Add(new SqlParameter("@Address", m_Address));
            arrayList.Add(new SqlParameter("@MainBrand", m_MainBrand));
            arrayList.Add(new SqlParameter("@Major", m_Major));
            arrayList.Add(new SqlParameter("@BillsDiscount", m_BillsDiscount));
            arrayList.Add(new SqlParameter("@BranchId", m_Branchid));
            arrayList.Add(new SqlParameter("@ContractType", m_ContractType));
            arrayList.Add(new SqlParameter("@IsChangRefund", m_IsChangRefund));
            arrayList.Add(new SqlParameter("@PurchasRewards", m_PurchasRewards));
            arrayList.Add(new SqlParameter("@PaymentMethod", m_PaymentMethod));
            arrayList.Add(new SqlParameter("@AccountNum", m_AccountNum));
            arrayList.Add(new SqlParameter("@TaxpayerIdentificationCode", m_TaxpayerIdentificationCode));
            arrayList.Add(new SqlParameter("@OpenBank", m_OpenBank));
            arrayList.Add(new SqlParameter("@FinancialLinkMan", m_FinancialLinkMan));
            arrayList.Add(new SqlParameter("@FinancialTelPhone", m_FinancialTelPhone));
            arrayList.Add(new SqlParameter("@Other", m_Other));
            arrayList.Add(new SqlParameter("@GoodsTypeId", m_GoodsTypeId));
            arrayList.Add(new SqlParameter("@PY", CommenClass.StringTools.GetFirstPYLetter("("+m_ShortName+")"+m_Company)));
            arrayList.Add(new SqlParameter("@TaxRate", m_TaxRate));
            arrayList.Add(new SqlParameter("@WarhouseAddress", m_WarhouseAddress));
            arrayList.Add(new SqlParameter("@OfficeAddress", m_OfficeAddress));
            arrayList.Add(new SqlParameter("@Delivery", m_Delivery));
            arrayList.Add(new SqlParameter("@DeliveryRegion", m_DeliveryRegion));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Supplier", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Supplier", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Supplier where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_AddTime = DBTool.GetDateTimeFromRow(row, "AddTime");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_Company = DBTool.GetStringFromRow(row, "Company", "");
                m_ShortName = DBTool.GetStringFromRow(row, "ShortName", "");
                m_Type = DBTool.GetStringFromRow(row, "Type", "");
                m_Attitude = DBTool.GetStringFromRow(row, "Attitude", "");
                m_PriceSheet = DBTool.GetIntFromRow(row, "PriceSheet", 0);
                m_InvoiceType = DBTool.GetStringFromRow(row, "InvoiceType", "");
                m_PayType = DBTool.GetStringFromRow(row, "PayType", "");
                m_YearTarget = DBTool.GetIntFromRow(row, "YearTarget", 0);
                m_MonthAmount = DBTool.GetDoubleFromRow(row, "MonthAmount", 0);
                m_LinkMan1 = DBTool.GetStringFromRow(row, "LinkMan1", "");
                m_LinkMan2 = DBTool.GetStringFromRow(row, "LinkMan2", "");
                m_TelPhone1 = DBTool.GetStringFromRow(row, "TelPhone1", "");
                m_TelPhone2 = DBTool.GetStringFromRow(row, "TelPhone2", "");
                m_Mobile1 = DBTool.GetStringFromRow(row, "Mobile1", "");
                m_Mobile2 = DBTool.GetStringFromRow(row, "Mobile2", "");
                m_QQ1 = DBTool.GetStringFromRow(row, "QQ1", "");
                m_QQ2 = DBTool.GetStringFromRow(row, "QQ2", "");
                m_Address = DBTool.GetStringFromRow(row, "Address", "");
                m_MainBrand = DBTool.GetStringFromRow(row, "MainBrand", "");
                m_Major = DBTool.GetStringFromRow(row, "Major", "");
                m_BillsDiscount = DBTool.GetIntFromRow(row, "BillsDiscount", 100);
                m_Branchid = DBTool.GetIntFromRow(row, "BranchId", 1);
                m_ContractType = DBTool.GetStringFromRow(row, "ContractType", "");
                m_IsChangRefund = DBTool.GetStringFromRow(row, "IsChangRefund", "");
                m_PurchasRewards = DBTool.GetStringFromRow(row, "PurchasRewards", "");
                m_PaymentMethod = DBTool.GetStringFromRow(row, "PaymentMethod", "");
                m_AccountNum = DBTool.GetStringFromRow(row, "AccountNum", "");
                m_TaxpayerIdentificationCode = DBTool.GetStringFromRow(row, "TaxpayerIdentificationCode", "");
                m_OpenBank = DBTool.GetStringFromRow(row, "OpenBank", "");
                m_FinancialLinkMan = DBTool.GetStringFromRow(row, "FinancialLinkMan", "");
                m_FinancialTelPhone = DBTool.GetStringFromRow(row, "FinancialTelPhone", "");
                m_Other = DBTool.GetStringFromRow(row, "Other", "");
                m_GoodsTypeId = DBTool.GetIntFromRow(row, "GoodsTypeId", 0);
                m_PY = DBTool.GetStringFromRow(row, "PY", "");
                m_TaxRate = DBTool.GetDoubleFromRow(row, "TaxRate", 0);
                m_OfficeAddress = DBTool.GetStringFromRow(row, "OfficeAddress","");
                m_WarhouseAddress = DBTool.GetStringFromRow(row, "WarhouseAddress", "");
                m_Delivery = DBTool.GetStringFromRow(row, "Delivery", "");
                m_DeliveryRegion = DBTool.GetStringFromRow(row, "DeliveryRegion", "");
                return true;
            }
            return false;
        } 
    }

    public class SupplierManager
    {
        private DBOperate m_dbo;
        private Supplier m_supplier;
        public Supplier supplier { get { return m_supplier; } set { m_supplier = value; } }
        public SupplierManager()
        {
            m_supplier = new Supplier();
            m_dbo = new DBOperate();
        }

        public bool Delectbyid(int Id)
        {
            string sql = string.Format(" delete  from  Supplier where ID={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        public DataSet ReadSupplierById(int Id)
        {
            string sql = string.Format(" select * from supplier where ID={0} ", Id);
            return m_dbo.GetDataSet(sql);
        }

        public DataSet ReadSuppliersForCache(int BranchId)
        {
            //string sql = "select *,'['+CONVERT(varchar(10),ID)+']  [('+ShortName+')'+Company+'] ['+Major+']' as NewsFullName from supplier";
            string sql = @"select *,'['+CONVERT(varchar(10),ID)+']  [('+ShortName+')'+Company+'] ['+Major+']' as NewsFullName,
isnull( (select COUNT(ID) from Purchase WHERE Purchase.SupplierId = Supplier.ID GROUP BY SupplierId),0) PURCHASENUM
from supplier ";
            if (BranchId > 0)
            {
                sql += string.Format(" where BranchId={0}", BranchId);
            }
            sql += " order by ShortName";
            return m_dbo.GetDataSet(sql);
        }
       /// <summary>
        ///  ERP:FAddSupplier.cs
        /// </summary>
        /// <param name="comName"></param>
        /// <returns></returns>
        public bool ReadSuppliersCompany(string comName)
        {
            if (comName != "")
            {
                string sql = string.Format(" select * from supplier where  Company = '{0}'",comName);
                DataSet ds = m_dbo.GetDataSet(sql);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return true;
                }
                else return false;
            }
            else return true;
        }
    }
}