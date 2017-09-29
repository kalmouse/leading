using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class Customer
    {
        public int Id { get; set; }
        public int ComId { get; set; }
        public int ModelId { get; set; }
        public int Level { get; set; }
        public int SalesId { get; set; }
        public int ServiceId { get; set; }
        public string StatementMan { get; set; }
        public double MonthSales { get; set; }
        public string OrderMemo { get; set; }
        public string PackageType { get; set; }
        public string PackageMemo { get; set; }
        public string Sign { get; set; }
        public string DeliveryMemo { get; set; }
        public string PayType { get; set; }
        public string PayMethod { get; set; }
        public double Credit { get; set; }
        public int CreditDays { get; set; }
        public string StatementMemo { get; set; }
        public string InvoiceType { get; set; }
        public string Invoice_Name { get; set; }
        public string Invoice_Content { get; set; }
        public string Invoice_TaxNo { get; set; }
        public string Invoice_Address { get; set; }
        public string Invoice_Phone { get; set; }
        public string Invoice_Bank { get; set; }
        public string Invoice_BankAccount { get; set; }
        public string InvoiceMemo { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int StartOrder { get; set; }
        public int StatementManId { get; set; }
        public int IsSPAssess { get; set; }
        public int BranchId { get; set; }
        public int IsNational { get; set; }
        public int ConfirmLevel { get; set; }//公司的审核级别 add by quxiaoshan 201-5-6
        public int IsPass { get; set; }//1越级 0默认为不越级add by quxiaoshan 201-5-6
        public int IsAgainConfirm { get; set; }// 是否需要上一级领导在审核
        public string IsBanningOrders { get; set ; }//客户下单管理
        public int IsManyNum { get; set; }//是否启用多账号多部门  默认是1启用
        public int IsComfirmOrder { get; set; }//是否启用审核下单流程  默认是0不启用
        public int IsCounter { get; set; }//是否启用专柜  默认是0不启用
        public int IsBudget { get; set; }//是否启用预算管理  默认是0不启用
        public int IsArchive { get; set; }//是否单独归档
        public int OrderPrintNum { get; set; }//订单打印份数，默认2份
        public int IsBuyOutCounter { get; set; }//是否可以够买专柜外商品  默认是0不可以 ***** 只要IsCounter=1 这个开关才可以起效******
        public int IsDownloadOrder { get; set; }//是否可以下载订单列表  人保接口用到
        public int IsStoreManager { get; set; }//是否有库存管理功能
        public int IsOrderConfig { get; set; }//是否开启订单设置功能
        public double TaxRate {get;set; }
        private DBOperate m_dbo;

        public Customer()
        {
            Id = 0;
            ComId = 0;
            ModelId = 0;
            Level = 3;
            SalesId = 0;
            ServiceId = 0;
            StatementMan = "";
            MonthSales = 0;
            OrderMemo = "";
            PackageType = "";
            PackageMemo = "";
            Sign = "";
            DeliveryMemo = "";
            PayType = "现结";
            PayMethod = "";
            Credit = 0;
            CreditDays = 0;
            StatementMemo = "";
            InvoiceType = "";
            Invoice_Name = "";
            Invoice_Content = "";
            Invoice_TaxNo = "";
            Invoice_Address = "";
            Invoice_Phone = "";
            Invoice_Bank = "";
            Invoice_BankAccount = "";
            InvoiceMemo = "";
            AddTime = DateTime.Now;
            UpdateTime = DateTime.Now;
            StartOrder = 0;
            StatementManId = 0;
            IsSPAssess = 0;
            BranchId = 0;
            IsNational = 0;
            ConfirmLevel = 5;
            IsPass = 1;//1：越级审核    0不越级，一级一级的往上提交审核 quxiaoshan 2015-5-22
            IsAgainConfirm = 0;  //0是直接转订单 1上一级再审核
            IsManyNum = 1;//1 启用多账号多部门  //0 没有开启则是 一个账号可以给多个部门下单 采购报表还是到各个部门的
            IsComfirmOrder = 0;//0 不启用审核下单功能
            IsCounter = 0;//0 没有专柜
            IsBudget = 0;// 0 默认不启用预算功能
            IsArchive = 0;
            OrderPrintNum = 2;
            IsBuyOutCounter = 0;//默认不可以购买
            IsDownloadOrder = 0;//默认不能下载
            IsStoreManager = 0;//默认为0 ,没有库存管理
            IsBanningOrders = "";//客户下单管理
            IsOrderConfig = 0;//默认不开启   --开启的话，会设置 每月订单数量、下订单是日期、单笔订单的金额
            TaxRate = 0;
            m_dbo = new DBOperate();
        }
        public Customer(int ComId)
        {
            m_dbo = new DBOperate();
            Load(ComId);
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@ComId", ComId));
            arrayList.Add(new SqlParameter("@ModelId", ModelId));
            arrayList.Add(new SqlParameter("@Level", Level));
            arrayList.Add(new SqlParameter("@SalesId", SalesId));
            arrayList.Add(new SqlParameter("@ServiceId", ServiceId));
            arrayList.Add(new SqlParameter("@StatementManId", StatementManId));
            arrayList.Add(new SqlParameter("@StatementMan", StatementMan));
            arrayList.Add(new SqlParameter("@MonthSales", MonthSales));
            arrayList.Add(new SqlParameter("@OrderMemo", OrderMemo));
            arrayList.Add(new SqlParameter("@PackageType", PackageType));
            arrayList.Add(new SqlParameter("@PackageMemo", PackageMemo));
            arrayList.Add(new SqlParameter("@Sign", Sign));
            arrayList.Add(new SqlParameter("@DeliveryMemo", DeliveryMemo));
            arrayList.Add(new SqlParameter("@PayType", PayType));
            arrayList.Add(new SqlParameter("@PayMethod", PayMethod));
            arrayList.Add(new SqlParameter("@Credit", Credit));
            arrayList.Add(new SqlParameter("@CreditDays", CreditDays));
            arrayList.Add(new SqlParameter("@StatementMemo", StatementMemo));
            arrayList.Add(new SqlParameter("@InvoiceType", InvoiceType));
            arrayList.Add(new SqlParameter("@Invoice_Name", Invoice_Name));
            arrayList.Add(new SqlParameter("@Invoice_Content", Invoice_Content));
            arrayList.Add(new SqlParameter("@Invoice_TaxNo", Invoice_TaxNo));
            arrayList.Add(new SqlParameter("@Invoice_Address", Invoice_Address));
            arrayList.Add(new SqlParameter("@Invoice_Phone", Invoice_Phone));
            arrayList.Add(new SqlParameter("@Invoice_Bank", Invoice_Bank));
            arrayList.Add(new SqlParameter("@Invoice_BankAccount", Invoice_BankAccount));
            arrayList.Add(new SqlParameter("@InvoiceMemo", InvoiceMemo));
            arrayList.Add(new SqlParameter("@AddTime", AddTime));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@StartOrder", StartOrder));
            arrayList.Add(new SqlParameter("@IsSPAssess", IsSPAssess));
            arrayList.Add(new SqlParameter("@BranchId", BranchId));
            arrayList.Add(new SqlParameter("@IsNational", IsNational));
            arrayList.Add(new SqlParameter("@ConfirmLevel", ConfirmLevel));
            arrayList.Add(new SqlParameter("@IsPass", IsPass));
            arrayList.Add(new SqlParameter("@IsAgainConfirm", IsAgainConfirm));
            arrayList.Add(new SqlParameter("@IsManyNum", IsManyNum));
            arrayList.Add(new SqlParameter("@IsComfirmOrder", IsComfirmOrder));
            arrayList.Add(new SqlParameter("@IsCounter", IsCounter));
            arrayList.Add(new SqlParameter("@IsBudget", IsBudget));
            arrayList.Add(new SqlParameter("@IsArchive", IsArchive));
            arrayList.Add(new SqlParameter("@OrderPrintNum", OrderPrintNum));
            arrayList.Add(new SqlParameter("@IsBuyOutCounter", IsBuyOutCounter));
            arrayList.Add(new SqlParameter("@IsDownloadOrder", IsDownloadOrder));
            arrayList.Add(new SqlParameter("@IsBanningOrders", IsBanningOrders));
            arrayList.Add(new SqlParameter("@IsOrderConfig", IsOrderConfig));            
            arrayList.Add(new SqlParameter("@IsStoreManager", IsStoreManager));
            arrayList.Add(new SqlParameter("@TaxRate", TaxRate));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Customer", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Customer", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Customer where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                return LoadFromRow(row);
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from Customer where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// ERP FOrderStatement.cs 中调用
        /// </summary>
        /// <param name="comId"></param>
        /// <returns></returns>
        public bool Load(int comId)
        {
            string sql = string.Format("select * from Customer where comId={0}", comId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                return LoadFromRow(row);
            }
            return false;
        }

        private bool LoadFromRow(DataRow row)
        {
            Id = DBTool.GetIntFromRow(row, "Id", 0);
            ComId = DBTool.GetIntFromRow(row, "ComId", 0);
            ModelId = DBTool.GetIntFromRow(row, "ModelId", 0);
            Level = DBTool.GetIntFromRow(row, "Level", 0);
            SalesId = DBTool.GetIntFromRow(row, "SalesId", 0);
            ServiceId = DBTool.GetIntFromRow(row, "ServiceId", 0);
            StatementManId = DBTool.GetIntFromRow(row, "StatementManId", 0);
            StatementMan = DBTool.GetStringFromRow(row, "StatementMan", "");
            MonthSales = DBTool.GetDoubleFromRow(row, "MonthSales", 0);
            OrderMemo = DBTool.GetStringFromRow(row, "OrderMemo", "");
            PackageType = DBTool.GetStringFromRow(row, "PackageType", "");
            PackageMemo = DBTool.GetStringFromRow(row, "PackageMemo", "");
            Sign = DBTool.GetStringFromRow(row, "Sign", "");
            DeliveryMemo = DBTool.GetStringFromRow(row, "DeliveryMemo", "");
            PayType = DBTool.GetStringFromRow(row, "PayType", "");
            PayMethod = DBTool.GetStringFromRow(row, "PayMethod", "");
            Credit = DBTool.GetDoubleFromRow(row, "Credit", 0);
            CreditDays = DBTool.GetIntFromRow(row, "CreditDays", 0);
            StatementMemo = DBTool.GetStringFromRow(row, "StatementMemo", "");
            InvoiceType = DBTool.GetStringFromRow(row, "InvoiceType", "");
            Invoice_Name = DBTool.GetStringFromRow(row, "Invoice_Name", "");
            Invoice_Content = DBTool.GetStringFromRow(row, "Invoice_Content", "");
            Invoice_TaxNo = DBTool.GetStringFromRow(row, "Invoice_TaxNo", "");
            Invoice_Address = DBTool.GetStringFromRow(row, "Invoice_Address", "");
            Invoice_Phone = DBTool.GetStringFromRow(row, "Invoice_Phone", "");
            Invoice_Bank = DBTool.GetStringFromRow(row, "Invoice_Bank", "");
            Invoice_BankAccount = DBTool.GetStringFromRow(row, "Invoice_BankAccount", "");
            InvoiceMemo = DBTool.GetStringFromRow(row, "InvoiceMemo", "");
            AddTime = DBTool.GetDateTimeFromRow(row, "AddTime");
            UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
            StartOrder = DBTool.GetIntFromRow(row, "StartOrder", 0);
            IsSPAssess = DBTool.GetIntFromRow(row, "IsSPAssess", 0);
            BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
            IsNational = DBTool.GetIntFromRow(row, "IsNational", 0);
            ConfirmLevel = DBTool.GetIntFromRow(row, "ConfirmLevel", 0);
            IsPass = DBTool.GetIntFromRow(row, "IsPass", 0);
            IsAgainConfirm = DBTool.GetIntFromRow(row, "IsAgainConfirm", 0);
            IsManyNum = DBTool.GetIntFromRow(row, "IsManyNum", 0);
            IsComfirmOrder = DBTool.GetIntFromRow(row, "IsComfirmOrder", 0);
            IsCounter = DBTool.GetIntFromRow(row, "IsCounter", 0);
            IsBudget = DBTool.GetIntFromRow(row, "IsBudget", 0);
            IsArchive = DBTool.GetIntFromRow(row, "IsArchive", 0);
            OrderPrintNum = DBTool.GetIntFromRow(row, "OrderPrintNum", 0);
            IsBuyOutCounter = DBTool.GetIntFromRow(row, "IsBuyOutCounter", 0);
            IsDownloadOrder = DBTool.GetIntFromRow(row, "IsDownloadOrder", 0);
            IsStoreManager = DBTool.GetIntFromRow(row, "IsStoreManager", 0);//库存管理
            IsBanningOrders = DBTool.GetStringFromRow(row, "IsBanningOrders", "");//客户下单管理  
            IsOrderConfig = DBTool.GetIntFromRow(row, "IsOrderConfig", 0);
            TaxRate = DBTool.GetDoubleFromRow(row, "TaxRate", 0);
            return true;
        }
        /// <summary>
        /// 修改 连锁下单状态
        /// </summary>
        /// <param name="startOrder"></param>
        /// <returns></returns>
        public bool SetStartOrder(int startOrder, int comId)
        {
            string sql = string.Format(" update Customer set StartOrder={0} where ComId={1} ", startOrder, comId);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 根据业务员获取客户
        /// </summary>
        /// <returns></returns>
        public DataSet GetCustomerBySale(int saleId)
        {
            string sql = string.Format("select *from Customer where SalesId={0}", saleId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 获取账期时间
        /// </summary>
        /// <returns></returns>
        public object GetAccountExpire(int OrderId,int ComId)
        {
            string sql = string.Format(@"declare  @FeedBackFinishTime datetime= getdate();
                        declare  @CreditDays int= 0;
                        select @FeedBackFinishTime=FeedBackFinishTime from OrderStatus where OrderId={0};
                        select @CreditDays=CreditDays from Customer where ComId={1}
                        select dateadd(day,@CreditDays,@FeedBackFinishTime) as [Time]", OrderId, ComId);
            return m_dbo.ExecuteScalar(sql);
        }
    }

    public class CustomerManager
    {
        private DBOperate m_dbo;
        public CustomerManager()
        {
            m_dbo = new DBOperate();
        }

        /// <summary>
        /// 客户分配给合作伙伴
        /// </summary>
        /// <param name="comId"></param>
        /// <param name="BranchId"></param>
        /// <returns></returns>
        public bool UpdateBranchId(int[] comId, int BranchId)
        {
            StringBuilder stb = new StringBuilder();
            for (int i = 0; i < comId.Length; i++)
            {
                if (i > 0)
                {
                    stb.Append(" , ");
                }
                stb.Append(comId[i]);
            }
            string sql = string.Format(" update Customer set BranchId ='{0}' where ComId in({1});update [Order] set BranchId ='{0}' where ComId in({1})", BranchId, stb.ToString());
            return m_dbo.ExecuteNonQuery(sql);
        }     
    }

}