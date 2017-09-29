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
    public class OrderOption
    {
        private int m_ComId;
        private int[] m_DeptId;
        private int m_MemberId;
        private int m_OrderId;
        private int m_GoodsId;
        private int m_IsShowTestOrder;//0：不显示测试账号的数据 comId>=1000 1:显示测试账号数据；  删除了 IsShowBackOrder 2015-4-6 lihailong
        private DateTime m_OrderTimeS;
        private DateTime m_OrderTimeE;
        private DateTime m_PlanDateS;
        private DateTime m_PlanDateE;
        private DateTime m_StoreTimeS;//出库日期 2014-9-20
        private DateTime m_StoreTimeE;//出库日期 2014-9-20
        private DateTime m_FinishDateS;
        private DateTime m_FinishDateE;
        private DateTime m_PrintTimeS;//打印日期 add by lihailong 2015-4-27
        private DateTime m_PrintTimeE;
        private double m_SumMoneyS;
        private double m_SumMoneyE;
        private string m_Company;
        private int m_UserId;
        private string m_OrderType;
        private string m_PayStatus;
        private int m_IsInner;
        private int m_ServiceId;
        private string m_ServiceStatus;
        private string m_PurchaseStatus;
        private string m_StoreStatus;
        private string m_InvoiceStatus;
        private string m_DeliveryStatus;
        private string m_LastUpdateTime;
        private string m_SalesName;
        private int m_StartOrder;
        private DataOrderBy m_OrderBy;
        private Order_Status m_Status;
        private int m_IsPrint;
        private int m_IsCopied;//-1:全部；0：真实订单；1：读出假单
        private int m_SalesId;//2014-9-24
        private int m_BranchId;//2015-4-23  订单属于哪个合作伙伴 lin
        private int m_IsShow;//是否显示（组合商品的明细商品：0）
        private int m_IsCalc;//是否内部计算(组合商品：0)
        private int m_TPI_OrderId { get; set; }
        private int m_IsCBranchId;//2015-10-15是否合作伙伴的客户 0：是 ；1否 lin
        private int m_CustomerBranchId;//2015-10-15 客户属于哪个合作伙伴 lin
        private int m_IsStorage;
        private int m_IsWDSH;//是否是网单审核

        public int ComId { get { return m_ComId; } set { m_ComId = value; } }
        public int[] DeptId { get { return m_DeptId; } set { m_DeptId = value; } }
        public int MemberId { get { return m_MemberId; } set { m_MemberId = value; } }
        public int OrderId { get { return m_OrderId; } set { m_OrderId = value; } }
        public int GoodsId { get { return m_GoodsId; } set { m_GoodsId = value; } }
        public int IsShowTestOrder { get { return m_IsShowTestOrder; } set { m_IsShowTestOrder = value; } }
        public DateTime OrderTimeS { get { return m_OrderTimeS; } set { m_OrderTimeS = value; } }
        public DateTime OrderTimeE { get { return m_OrderTimeE; } set { m_OrderTimeE = value; } }
        public DateTime PlanDateS { get { return m_PlanDateS; } set { m_PlanDateS = value; } }
        public DateTime PlanDateE { get { return m_PlanDateE; } set { m_PlanDateE = value; } }
        public DateTime StoreTimeS { get { return m_StoreTimeS; } set { m_StoreTimeS = value; } }
        public DateTime StoreTimeE { get { return m_StoreTimeE; } set { m_StoreTimeE = value; } }
        public DateTime FinishDateS { get { return m_FinishDateS; } set { m_FinishDateS = value; } }
        public DateTime FinishDateE { get { return m_FinishDateE; } set { m_FinishDateE = value; } }
        public DateTime PrintTimeS { get { return m_PrintTimeS; } set { m_PrintTimeS = value; } }
        public DateTime PrintTimeE { get { return m_PrintTimeE; } set { m_PrintTimeE = value; } }
        public double SumMoneyS { get { return m_SumMoneyS; } set { m_SumMoneyS = value; } }
        public double SumMoneyE { get { return m_SumMoneyE; } set { m_SumMoneyE = value; } }
        public string Company { get { return m_Company; } set { m_Company = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public string OrderType { get { return m_OrderType; } set { m_OrderType = value; } }
        public string PayStatus { get { return m_PayStatus; } set { m_PayStatus = value; } }
        public int IsInner { get { return m_IsInner; } set { m_IsInner = value; } }
        public int ServiceId { get { return m_ServiceId; } set { m_ServiceId = value; } }
        public string ServiceStatus { get { return m_ServiceStatus; } set { m_ServiceStatus = value; } }
        public string SalesName { get { return m_SalesName; } set { m_SalesName = value; } }
        public string PurchaseStatus { get { return m_PurchaseStatus; } set { m_PurchaseStatus = value; } }
        public string StoreStatus { get { return m_StoreStatus; } set { m_StoreStatus = value; } }
        public string InvoiceStatus { get { return m_InvoiceStatus; } set { m_InvoiceStatus = value; } }
        public string DeliveryStatus { get { return m_DeliveryStatus; } set { m_DeliveryStatus = value; } }
        public Order_Status Status { get { return m_Status; } set { m_Status = value; } }
        public string LastUpdateTime { get { return m_LastUpdateTime; } set { m_LastUpdateTime = value; } }
        public DataOrderBy OrderBy { get { return m_OrderBy; } set { m_OrderBy = value; } }
        public int StartOrder { get { return m_StartOrder; } set { m_StartOrder = value; } }
        public int IsPrint { get { return m_IsPrint; } set { m_IsPrint = value; } }
        public int IsCopied { get { return m_IsCopied; } set { m_IsCopied = value; } }
        public int SalesId { get { return m_SalesId; } set { m_SalesId = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public int IsShow { get { return m_IsShow; } set { m_IsShow = value; } }
        public int IsCalc { get { return m_IsCalc; } set { m_IsCalc = value; } }
        public int TPI_OrderId { get { return m_TPI_OrderId; } set { m_TPI_OrderId = value; } }
        public int IsCBranchId { get { return m_IsCBranchId; } set { m_IsCBranchId = value; } }
        public int CustomerBranchId { get { return m_CustomerBranchId; } set { m_CustomerBranchId = value; } }
        public int IsStorage { get { return m_IsStorage; } set { m_IsStorage = value; } }
        public int IsWDSH { get { return m_IsWDSH; } set { m_IsWDSH = value; } }
        public OrderOption()
        {
            m_ComId = 0;
            m_MemberId = 0;
            m_OrderId = 0;
            m_GoodsId = 0;
            m_IsShowTestOrder = 0;
            m_OrderTimeS = new DateTime(1900, 1, 1);
            m_OrderTimeE = new DateTime(1900, 1, 1);
            m_PlanDateS = new DateTime(1900, 1, 1);
            m_PlanDateE = new DateTime(1900, 1, 1);
            m_StoreTimeS = new DateTime(1900, 1, 1);
            m_StoreTimeE = new DateTime(1900, 1, 1);
            m_FinishDateS = new DateTime(1900, 1, 1);
            m_FinishDateE = new DateTime(1900, 1, 1);
            m_PrintTimeE = new DateTime(1900, 1, 1);
            m_PrintTimeS = new DateTime(1900, 1, 1);
            m_SumMoneyS = 0;
            m_SumMoneyE = 0;
            m_Company = "";
            m_UserId = 0;
            m_OrderType = "";
            m_PayStatus = "";
            m_IsInner = -1;
            m_ServiceId = 0;
            m_ServiceStatus = "";//客服状态
            m_PurchaseStatus = "";//采购状态
            m_StoreStatus = "";//仓库状态
            m_InvoiceStatus = "";//发票状态
            m_DeliveryStatus = "";//配送状态
            m_Status = Order_Status.全部状态;
            m_LastUpdateTime = "";
            m_OrderBy = CommenClass.DataOrderBy.默认;
            m_StartOrder = -1;
            m_IsPrint = -1;
            m_IsCopied = -1;
            m_SalesId = 0;
            m_BranchId = -1;//合作伙伴处理订单
            m_IsCalc = -1;
            m_SalesName = "";//业务员名称
            m_IsShow = -1;
            m_TPI_OrderId = 0;
            m_CustomerBranchId = -1;//客户属于合作伙伴
            m_IsCBranchId = -1;// 是否自己处理订单
            m_IsStorage = -1;//是否入库，默认为0，未入库  add by hjy 2016/6/20
            m_IsWDSH = 0;
        }
    }

    public class OrderManager
    {
        private DBOperate m_dbo;
        private OrderOption m_option;
        public OrderOption option { get { return m_option; } set { m_option = value; } }
        public OrderManager()
        {
            m_dbo = new DBOperate();
            m_option = new OrderOption();

        }
        public OrderManager(string connection)
        {
            m_dbo = new DBOperate(connection);
            m_option = new OrderOption();
        }
        //public DataSet ReadStatement()
        //{
        //    string sql = "select OrderId,DeptName,RealName,SumMoney,PayStatus,OrderTime,Memo,RawOrderId,ComId,PlanDate from View_Order where IsDelete=0 ";//IsInner =-100,表示已经删除的订单
        //    if (option.BranchId > 1)
        //    {
        //        sql += string.Format(" and BranchId={0}", option.BranchId);
        //    }
        //    if (option.OrderId > 0)
        //    {
        //        sql += string.Format(" and OrderId={0} ", option.OrderId);
        //        if (option.MemberId > 0)
        //        {
        //            sql += string.Format(" and MemberId={0} ", option.MemberId);
        //        }

        //    }
        //    else
        //    {
        //        sql += " and RawOrderId=0 ";//不显示假单 2014-12-19 add by 李海龙

        //        if (option.ComId > 0)
        //        {
        //            sql += string.Format(" and ComId={0} ", option.ComId);
        //        }
        //        else if (option.IsShowTestOrder == 0)
        //        {
        //            sql += " and ComId !=1 ";
        //        }

        //        if (option.OrderType != "")
        //        {
        //            string[] ordertype = option.OrderType.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        //            if (ordertype.Length == 1)
        //            {
        //                sql += string.Format(" and OrderType = '{0}' ", option.OrderType);
        //                if (option.OrderType == CommenClass.OrderType.网上订单.ToString())
        //                {
        //                    sql += string.Format(" and OrderTime <='{0}' ", DateTime.Now.AddMinutes(-5));
        //                }
        //            }
        //            else
        //            {
        //                sql += " and OrderType in ( ";
        //                for (int i = 0; i < ordertype.Length; i++)
        //                {
        //                    if (i == 0)
        //                        sql += string.Format(" '{0}' ", ordertype[i]);
        //                    else
        //                        sql += string.Format(" ,'{0}' ", ordertype[i]);
        //                }
        //                sql += " ) ";
        //            }
        //        }
        //        if (option.Company != "")
        //        {
        //            sql += string.Format(" and Company like '%{0}%' ", option.Company);
        //        }
        //        if (option.DeptId.Length > 0)
        //        {
        //            //sql += string.Format(" and DeptId={0}", option.DeptId);
        //            sql += " and DeptId in ( ";
        //            for (int i = 0; i < option.DeptId.Length; i++)
        //            {
        //                if (i == 0)
        //                {
        //                    sql += string.Format(" '{0}' ", option.DeptId[0]);
        //                }
        //                else sql += string.Format(" ,'{0}' ", option.DeptId[i]);
        //            }
        //            sql += " ) ";
        //        }
        //        if (option.StartOrder != -1)
        //        {
        //            sql += string.Format(" and StartOrder = {0} ", option.StartOrder);
        //        }
        //        if (option.DeliveryStatus != "")
        //        {
        //            sql += string.Format(" and DeliveryStatus = '{0}' ", option.DeliveryStatus);
        //        }
        //        if (option.FinishDateE > new DateTime(1900, 1, 1))
        //        {
        //            sql += string.Format(" and DeliveryFinishTime < '{0}' ", option.FinishDateE.AddDays(1).ToShortDateString());
        //        }
        //        if (option.FinishDateS > new DateTime(1900, 1, 1))
        //        {
        //            sql += string.Format(" and DeliveryFinishTime >= '{0}' ", option.FinishDateS.ToShortDateString());
        //        }
        //        if (option.InvoiceStatus != "")
        //        {
        //            sql += string.Format(" and InvoiceStatus = '{0}' ", option.InvoiceStatus);
        //        }
        //        if (option.IsInner == 1)
        //        {
        //            sql += " and IsInner =1 ";
        //        }
        //        if (option.MemberId > 0)
        //        {
        //            sql += string.Format(" and MemberId={0} ", option.MemberId);
        //        }
        //        if (option.OrderTimeE > new DateTime(1900, 1, 1))
        //        {
        //            sql += string.Format(" and OrderTime <'{0}' ", option.OrderTimeE.AddDays(1).ToShortDateString());
        //        }
        //        if (option.OrderTimeS > new DateTime(1900, 1, 1))
        //        {
        //            sql += string.Format(" and OrderTime >= '{0}' ", option.OrderTimeS.ToShortDateString());
        //        }

        //        if (option.PayStatus != "")
        //        {
        //            sql += string.Format(" and PayStatus = '{0}' ", option.PayStatus);
        //        }
        //        if (option.PlanDateS > new DateTime(1900, 1, 1))
        //        {
        //            sql += string.Format(" and PlanDate >= '{0}' ", option.PlanDateS.ToShortDateString());
        //        }
        //        if (option.PlanDateE > new DateTime(1900, 1, 1))
        //        {
        //            sql += string.Format(" and PlanDate < '{0}' ", option.PlanDateE.AddDays(1).ToShortDateString());
        //        }
        //        if (option.StoreTimeS > new DateTime(1900, 1, 1))
        //        {
        //            sql += string.Format(" and StoreFinishTime >='{0}' ", option.StoreTimeS.ToShortDateString());
        //        }
        //        if (option.StoreTimeE > new DateTime(1900, 1, 1))
        //        {
        //            sql += string.Format(" and StoreFinishTime <'{0}' ", option.StoreTimeE.AddDays(1).ToShortDateString());
        //        }
        //        //订单表中的打印时间
        //        if (option.PrintTimeS > new DateTime(2014, 1, 1))
        //        {
        //            sql += string.Format(" and PrintDateTime >'{0}' ", option.PrintTimeS.ToShortDateString());
        //        }
        //        if (option.PrintTimeE > new DateTime(2014, 1, 1))
        //        {
        //            sql += string.Format(" and PrintDateTime <'{0}' ", option.PrintTimeE.AddDays(1).ToShortDateString());
        //        }
        //        if (option.PurchaseStatus != "")
        //        {
        //            sql += string.Format(" and PurchaseStatus = '{0}' ", option.PurchaseStatus);
        //        }
        //        if (option.ServiceId > 0)
        //        {
        //            sql += string.Format(" and ServiceId = {0} ", option.ServiceId);
        //        }
        //        if (option.ServiceStatus != "")
        //        {
        //            sql += string.Format(" and ServiceStatus = '{0}' ", option.ServiceStatus);
        //        }
        //        switch (option.Status)
        //        {
        //            case Order_Status.未完成:
        //                sql += " and ( DeliveryStatus <> '已完成' or StoreStatus <> '已完成' ) ";
        //                break;
        //        }
        //        if (option.StoreStatus != "")
        //        {
        //            sql += string.Format(" and StoreStatus ='{0}' ", option.StoreStatus);
        //        }
        //        if (option.SumMoneyE > 0)
        //        {
        //            sql += string.Format(" and SumMoney <= {0} ", option.SumMoneyE);
        //        }
        //        if (option.SumMoneyS > 0)
        //        {
        //            sql += string.Format(" and SumMoney >= {0} ", option.SumMoneyS);
        //        }
        //        if (option.UserId > 0)
        //        {
        //            sql += string.Format(" and UserId = {0} ", option.UserId);
        //        }
        //        if (option.LastUpdateTime != "")
        //        {
        //            sql += string.Format(" and UpdateTime > '{0}' ", option.LastUpdateTime);
        //        }
        //        if (option.IsPrint != -1)
        //        {
        //            if (option.IsPrint == 0)
        //            {
        //                sql += " and PrintNum =0 ";
        //            }
        //            else sql += " and PrintNum >0 ";
        //        }
        //        if (option.IsCopied != -1)
        //        {
        //            if (option.IsCopied == 0)//显示原始订单
        //            {
        //                sql += " and RawOrderId =0 ";
        //            }
        //            if (option.IsCopied == 1)//显示正常订单+假单
        //            {
        //                sql += " and IsCopied = 0 ";
        //            }
        //        }
        //        if (option.GoodsId > 0)
        //        {
        //            sql += string.Format(" and OrderId in ( select distinct(OrderId) from View_OrderDetail where  GoodsId={0} ) ", option.GoodsId.ToString());
        //        }
        //        if (option.SalesId > 0)
        //        {
        //            sql += string.Format(" and SalesId ={0} ", option.SalesId);
        //        }
        //    }
            
        //    string orderby = option.OrderBy.ToString();
        //    switch (orderby)
        //    {
        //        case "默认":
        //            sql += " order by  plandate,OrderTime";
        //            break;
        //        case "公司":
        //            sql += " order by ComId,OrderId ";
        //            break;
        //        case "下单时间":
        //            sql += "order by OrderTime desc";
        //            break;
        //        default:
        //            sql += " order by  plandate,OrderTime";
        //            break;

        //    }
            
        //    return m_dbo.GetDataSet(sql);
        //}
        /// <summary>
        /// 订单查询，读取修改订单列表
        /// </summary>
        /// <returns></retur。ns>
        public DataSet ReadAlterOrders()
        {
            string sql = "select * from View_Order where OrderId in (select OrderId from view_OrderModify group by OrderId) ";//IsDelete=0,表示已经删除的订单
            sql += GetSQLWhere();
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 订单查询，读取订单列表--核心方法--OK
        /// ERP  FWSearchOrder.cs 中调用
        /// </summary>
        /// <returns></retur。ns>
        public DataSet ReadOrders()
        {
            string sql = "select *,SumMoney/(1+TaxRate) as unTaxSumMoney from View_Order where IsDelete=0 ";//IsDelete=0,表示已经删除的订单
            if (option.IsWDSH == 1)
            {
                sql = "select *,(select ReviewId from TPI_Order where TPI_Order.Id =TPI_OrderId)as ReviewId,(select ProjectId  from TPI_Order where TPI_Order.Id =TPI_OrderId) as ProjectId from View_Order where IsDelete=0 ";//IsDelete=0,表示已经删除的订单
            }
           
            sql += GetSQLWhere();
            return m_dbo.GetDataSet(sql);
        }

        public string GetSQLWhere()
        {
            string sql = "";
            if (option.CustomerBranchId == option.BranchId && option.CustomerBranchId > 0)
            {
                sql += string.Format(" and (CustomerBranchId={0} or BranchId={0}) ", option.CustomerBranchId);
            }
            else
            {
                if (option.CustomerBranchId != -1)//客户属于合作伙伴
                {
                    sql += string.Format(" and CustomerBranchId ={0}", option.CustomerBranchId);
                }
                if (option.BranchId != -1)//订单由合作伙伴处理的
                {
                    if (option.IsCBranchId > 0)
                    {
                        sql += string.Format(" and BranchId<>{0}", option.BranchId);
                    }
                    else
                    {
                        sql += string.Format(" and BranchId={0}", option.BranchId);
                    }
                }
            }
            if (option.DeliveryStatus != "")
            {
                sql += string.Format(" and DeliveryStatus = '{0}' ", option.DeliveryStatus);
            }
            if (option.OrderType != "")
            {
                string[] ordertype = option.OrderType.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (ordertype.Length == 1)
                {
                    sql += string.Format(" and OrderType = '{0}' ", option.OrderType);
                    if (option.OrderType == CommenClass.OrderType.网上订单.ToString())
                    {
                        sql += string.Format(" and OrderTime <='{0}' ", DateTime.Now.AddMinutes(-5));
                    }
                }
                else
                {
                    sql += " and OrderType in ( ";
                    for (int i = 0; i < ordertype.Length; i++)
                    {
                        if (i == 0)
                            sql += string.Format(" '{0}' ", ordertype[i]);
                        else
                            sql += string.Format(" ,'{0}' ", ordertype[i]);
                    }
                    sql += " ) ";
                }
            }
            if (option.StoreStatus != "")
            {
                sql += string.Format(" and StoreStatus ='{0}' ", option.StoreStatus);
            }
            if (option.OrderId > 0)
            {
                sql += string.Format(" and OrderId={0} ", option.OrderId);
                if (option.MemberId > 0)
                {
                    sql += string.Format(" and MemberId={0} ", option.MemberId);
                }

            }
            else
            {
                sql += " and RawOrderId=0 ";//不显示假单 2014-12-19 add by 李海龙

                if (option.ComId > 0)
                {
                    sql += string.Format(" and ComId={0} ", option.ComId);
                }
                else if (option.IsShowTestOrder == 0)
                {
                    sql += " and ComId !=1 ";
                }


                if (option.Company != "")
                {
                    sql += string.Format(" and Company like '%{0}%' ", option.Company);
                }
                if (option.DeptId != null)
                {
                    //sql += string.Format(" and DeptId={0}", option.DeptId);
                    sql += " and DeptId in ( ";
                    for (int i = 0; i < option.DeptId.Length; i++)
                    {
                        if (i == 0)
                        {
                            sql += string.Format(" '{0}' ", option.DeptId[0]);
                        }
                        else sql += string.Format(" ,'{0}' ", option.DeptId[i]);
                    }
                    sql += " ) ";
                }
                if (option.StartOrder != -1)
                {
                    sql += string.Format(" and StartOrder = {0} ", option.StartOrder);
                }
                if (option.FinishDateE > new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and DeliveryFinishTime < '{0}' ", option.FinishDateE.AddDays(1).ToShortDateString());
                }
                if (option.FinishDateS > new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and DeliveryFinishTime >= '{0}' ", option.FinishDateS.ToShortDateString());
                }
                if (option.InvoiceStatus != "")
                {
                    sql += string.Format(" and InvoiceStatus = '{0}' ", option.InvoiceStatus);
                }
                if (option.IsInner == 1)
                {
                    sql += " and IsInner =1 ";
                }
                if (option.MemberId > 0)
                {
                    sql += string.Format(" and MemberId={0} ", option.MemberId);
                }
                if (option.OrderTimeE > new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and OrderTime <'{0}' ", option.OrderTimeE.AddDays(1).ToShortDateString());
                }
                if (option.OrderTimeS > new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and OrderTime >= '{0}' ", option.OrderTimeS.ToShortDateString());
                }

                if (option.PayStatus != "")
                {
                    sql += string.Format(" and PayStatus = '{0}' ", option.PayStatus);
                }
                if (option.PlanDateS > new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and PlanDate >= '{0}' ", option.PlanDateS.ToShortDateString());
                    if (option.PlanDateE > new DateTime(1900, 1, 1))
                    {
                        sql += string.Format(" and PlanDate < '{0}' ", option.PlanDateE.AddDays(1).ToShortDateString());
                    }
                }
                else
                {
                    if (option.PlanDateE > new DateTime(1900, 1, 1))
                    {
                        sql += string.Format("and  PlanDate < GETDATE()");//SalesName like '%刘同营%'
                    }
                }
                if (option.StoreTimeS > new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and StoreFinishTime >='{0}' ", option.StoreTimeS.ToShortDateString());
                }
                if (option.StoreTimeE > new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and StoreFinishTime <'{0}' ", option.StoreTimeE.AddDays(1).ToShortDateString());
                }
                //订单表中的打印时间
                if (option.PrintTimeS > new DateTime(2014, 1, 1))
                {
                    sql += string.Format(" and PrintDateTime >'{0}' ", option.PrintTimeS.ToShortDateString());
                }
                if (option.PrintTimeE > new DateTime(2014, 1, 1))
                {
                    sql += string.Format(" and PrintDateTime <'{0}' ", option.PrintTimeE.AddDays(1).ToShortDateString());
                }
                if (option.PurchaseStatus != "")
                {
                    sql += string.Format(" and PurchaseStatus = '{0}' ", option.PurchaseStatus);
                }
                if (option.ServiceId > 0)
                {
                    sql += string.Format(" and ServiceId = {0} ", option.ServiceId);
                }
                if (option.ServiceStatus != "")
                {
                    sql += string.Format(" and ServiceStatus = '{0}' ", option.ServiceStatus);
                }
                switch (option.Status)
                {
                    case Order_Status.未完成:
                        // sql += " and ( DeliveryStatus <> '已完成' or StoreStatus <> '已完成' ) ";
                        sql += " and (StoreStatus <> '已完成' ) ";
                        break;
                }

                if (option.SumMoneyE > 0)
                {
                    sql += string.Format(" and SumMoney <= {0} ", option.SumMoneyE);
                }
                if (option.SumMoneyS > 0)
                {
                    sql += string.Format(" and SumMoney >= {0} ", option.SumMoneyS);
                }
                if (option.UserId > 0)
                {
                    sql += string.Format(" and UserId = {0} ", option.UserId);
                }
                if (option.LastUpdateTime != "")
                {
                    sql += string.Format(" and UpdateTime > '{0}' ", option.LastUpdateTime);
                }
                if (option.IsPrint != -1)
                {
                    if (option.IsPrint == 0)
                    {
                        sql += " and PrintNum =0 ";
                    }
                    else sql += " and PrintNum >0 ";
                }
                if (option.IsCopied != -1)
                {
                    if (option.IsCopied == 0)//显示原始订单
                    {
                        sql += " and RawOrderId =0 ";
                    }
                    if (option.IsCopied == 1)//显示正常订单+假单
                    {
                        sql += " and IsCopied = 0 ";
                    }
                }
                if (option.GoodsId > 0)
                {
                    sql += string.Format(" and OrderId in ( select distinct(OrderId) from View_OrderDetail where  GoodsId={0} ) ", option.GoodsId.ToString());
                }
                if (option.SalesId > 0)
                {
                    sql += string.Format(" and OrderSalesId ={0} ", option.SalesId);
                }
                if (option.IsStorage > -1)
                {
                    sql += string.Format(" and IsStorage={0}", option.IsStorage);
                }
            }
            string orderby = option.OrderBy.ToString();
            switch (orderby)
            {
                case "默认":
                    sql += " order by  plandate,OrderTime";
                    break;
                case "公司":
                    sql += " order by ComId,OrderId ";
                    break;
                case "下单时间":
                    sql += "order by OrderTime desc";
                    break;
                default:
                    sql += " order by plandate,OrderTime";
                    break;
            }
            return sql;
        }



        /// <summary>
        /// 读取正在配货的订单列表
        /// </summary>
        /// <returns></returns>
        public DataSet ReadPickingOrders()
        {
            string sql = " select * from View_OrderPicking where IsDelete=0 and StoreStatus <> '已完成' ";
            if (option.BranchId > 0)
            {
                sql += string.Format(" and BranchId={0}", option.BranchId);
            }
            if (option.OrderId > 0)
            {
                sql += string.Format(" and OrderId={0} ", option.OrderId);
            }
            else
            {
                if (option.ComId > 0)
                {
                    sql += string.Format(" and ComId={0} ", option.ComId);
                }
                if (option.Company != "")
                {
                    sql += string.Format(" and Company like '%{0}%' ", option.Company);
                }
                if (option.FinishDateE > new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and FinishDate < '{0}' ", option.FinishDateE.AddDays(1).ToShortDateString());
                }
                if (option.FinishDateS > new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and FinishDate >= '{0}' ", option.FinishDateS.ToShortDateString());
                }
                if (option.OrderTimeE > new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and OrderTime <'{0}' ", option.OrderTimeE.AddDays(1).ToShortDateString());
                }
                if (option.OrderTimeS > new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and OrderTime >= '{0}' ", option.OrderTimeS.ToShortDateString());
                }
                if (option.PlanDateS > new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and PlanDate >= '{0}' ", option.PlanDateS.ToShortDateString());
                }
                if (option.PlanDateE > new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and PlanDate < '{0}' ", option.PlanDateE.AddDays(1).ToShortDateString());
                }

                if (option.StoreStatus != "")
                {
                    sql += string.Format(" and StoreStatus ='{0}' ", option.StoreStatus);
                }
                if (option.SumMoneyE > 0)
                {
                    sql += string.Format(" and SumMoney <= {0} ", option.SumMoneyE);
                }
                if (option.SumMoneyS > 0)
                {
                    sql += string.Format(" and SumMoney >= {0} ", option.SumMoneyS);
                }
                if (option.UserId > 0)
                {
                    sql += string.Format(" and PickingUserId = {0} ", option.UserId);
                }
            }
            sql += " order by ComId,address ";
            return m_dbo.GetDataSet(sql);

        }
        /// <summary>
        /// 读取订单明细----2014-05-24
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public DataSet ReadOrderDetail(int[] orderIds)
        {
            string sql = "select * from view_OrderDetail where 1=1 ";
            if (orderIds.Length == 1)
            {
                sql += string.Format(" and  OrderId ={0} ", orderIds[0]);
            }
            else
            {
                string ids = " and OrderId in ( ";
                for (int i = 0; i < orderIds.Length; i++)
                {
                    if (i == 0)
                    {
                        ids += orderIds[i].ToString();
                    }
                    else ids += "," + orderIds[i].ToString();
                }
                ids += " ) ";
                sql += ids;
            }
            sql += " Order by ID ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 计算订单利润
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool OrderGrossProfit(int orderId)
        {
            string sql = string.Format(@"update [Order] set GrossProfit = SumMoney/(1+TaxRate)-(select isnull(SUM(AC*Num),0) from OrderDetail where OrderId ={0}),
                                                            TaxGrossProfit = SumMoney - (select isnull(SUM(TaxAC*Num),0) from OrderDetail where OrderId ={0}) where Id={0}", orderId);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 查询订单状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public DataSet ReadOrderDetail(int orderId)
        {
            string sql = "select * from view_OrderDetail where 1=1 ";
            sql += string.Format(" and  OrderId ={0} ", orderId);
            if (option.IsCalc > -1)
            {
                sql += string.Format(" and IsCalc = {0} ", option.IsCalc);
            }
            if (option.IsShow > -1)
            {
                sql += string.Format(" and IsShow ={0} ", option.IsShow);
            }
            sql += " Order by Id ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 河南电子商城—查询获得订单
        /// </summary>
        /// <returns></returns>
        public DataSet Read_Orders()
        {
            string sql = string.Format("select * from [order] where 1=1 ");
            return m_dbo.GetDataSet(sql);
        }
        public DataSet ReadSingleGoods(int orderId, int goodsStoreId)
        {
            string sql = string.Format(@"select vo.Id as DetailId,vs.Id,vo.OrderId,vs.StoreId,vo.Model, vo.GoodsId,vo.DisplayName,vo.Num ,vs.Num as StoreNum,vs.StoreZone,vo.StoreStatus,vo.PickNum,vs.Unit,vo.PurchaseMemo from View_OrderDetail vo left join view_goodsStore vs on vo.GoodsId=vs.GoodsId where 1=1 and vs.Id={0} and OrderId={1}", goodsStoreId, orderId);
            if (option.IsCalc > -1)
            {
                sql += string.Format(" and IsCalc = {0} ", option.IsCalc);
            }
            if (option.IsShow > -1)
            {
                sql += string.Format(" and IsShow ={0} ", option.IsShow);
            }
            return m_dbo.GetDataSet(sql);
        }

        public DataSet ReadGoodsDetailByOrderId(int storeId, int orderId)
        {
            string sql = string.Format(@"select vo.Id as DetailId,vo.OrderId,{0} as StoreId,vo.Model, vo.GoodsId,vo.DisplayName,vo.Num ,vo.StoreStatus,vo.PickNum,vo.Unit,vo.PurchaseMemo,
Isnull((select Id from GoodsStore where StoreId={0} and GoodsId=vo.GoodsId and StoreZone<>'收货区'),0) as Id,
Isnull((select Num from GoodsStore where StoreId={0} and GoodsId=vo.GoodsId and StoreZone<>'收货区'),0) as StoreNum,
Isnull((select StoreZone from GoodsStore where StoreId={0} and GoodsId=vo.GoodsId and StoreZone<>'收货区'),null) as StoreZone
 from View_OrderDetail vo
  where 1=1 and vo.OrderId={1} and IsCalc=1 order by StoreZone", storeId, orderId);
            return m_dbo.GetDataSet(sql);
        }

        public DataSet ReadOrderDetail(int orderId, int storeId)
        {
            string sql = string.Format("select * from View_OrderDetail where OrderId={0} and StoreId={1} and IsCalc=1 order by Id", orderId, storeId);
            return m_dbo.GetDataSet(sql);
        }

        public DataSet ReadOrderDetailForPicking(int orderId, int BranchId)
        {
            string sql = string.Format(@"select GoodsId,vod.DisplayName,Model,vgm.Name1,vgm.Name2,vgm.Name3,PurchaseMemo, Num,TaxInPrice,vod.Unit,SalePrice,IsShow,IsCalc,
dbo.GetGoodsStore(GoodsId,1,{0}) as StoreNum,
dbo.GetReSaleGoods(GoodsId,{0}) as ReSale
from view_OrderDetail vod  left outer join view_GoodsModel vgm on vod.GoodsId=vgm.goodssubId where 1=1 ", BranchId);
            sql += string.Format(" and  OrderId ={0} ", orderId);
            sql += " Order by Code ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取订单明细汇总
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        public DataSet ReadOrderDetailSum(int[] orderIds, int BranchId)
        {
            string sql = string.Format(@"select Code,GoodsId,DisplayName,SUM(Num) as Num,Count(Num) as OrderNum,TaxInPrice,Unit,
dbo.GetGoodsStore(GoodsId,1,{0}) as StoreNum,
dbo.GetReSaleGoods(GoodsId,{0}) as ReSale
from view_OrderDetail where 1=1 and IsCalc=1  ", BranchId);
            if (orderIds.Length == 1)
            {
                sql += string.Format(" and OrderId ={0} ", orderIds[0]);
            }
            else
            {
                string ids = " and OrderId in ( ";
                for (int i = 0; i < orderIds.Length; i++)
                {
                    if (i == 0)
                    {
                        ids += orderIds[i].ToString();
                    }
                    else ids += "," + orderIds[i].ToString();
                }
                ids += " ) ";
                sql += ids;
            }
            sql += " group by Code,GoodsId,DisplayName,TaxInPrice,Unit order by Code,DisplayName ";
            return m_dbo.GetDataSet(sql);
        }

        #region 保密单位导出订单是用到的方法
        /// <summary>
        /// 合并订单
        /// </summary>
        /// <returns></returns>
        public DataSet OrderSummary(int[] OrderIds)
        {
            string sql = string.Format("select GoodsId,sum(Num) as Num,model,Price,DisplayName,unit from View_OrderDetail where 1=1");
            if (OrderIds != null)
            {
                sql += " and OrderId in ( ";
                for (int i = 0; i < OrderIds.Length; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", OrderIds[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", OrderIds[i]);
                }
                sql += " ) ";
            }
            sql += "group by GoodsId,model,Price,DisplayName,unit order by Price desc";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 查询当前公司是否有新订单
        /// </summary>
        /// <returns></returns>
        public DataSet GetLastOrderTime()
        {
            string sql = string.Format("select * from [Order] where ComId={0} and FinishDate ='{1}';select SUM(SumMoney) as Total from [Order] where ComId={0} and FinishDate ='{1}'", option.ComId, option.FinishDateS);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 重新下载订单
        /// </summary>
        /// <returns></returns>
        public DataSet AgainExportOrder()
        {
            string sql = string.Format("select * from [Order] where ComId={0} and TPI_OrderId={1};select SUM(SumMoney) as Total from [Order] where ComId={0} and TPI_OrderId={1}", option.ComId, option.TPI_OrderId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 获取最大的TPI_OrderId 
        /// </summary>
        /// <returns></returns>
        public DataRow GetMaxTPI_OrderId()
        {
            string sql = "select MAX(TPI_OrderId) as MaxId from [order]";
            DataSet ds = m_dbo.GetDataSet(sql);
            return ds.Tables[0].Rows[0];
        }
        /// <summary>
        /// 通过下载记录表获取到 要汇总订单的orderid
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public DataSet GetOrderIds(int recordId)
        {
            string sql = string.Format("select * from [Order] where TPI_OrderId={0}", recordId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 更改订单时间
        /// </summary>
        /// <returns></returns>
        public DataSet GetExportOrder(DateTime NowTime, DateTime LastTime)
        {
            string sql = string.Format("update [Order] set FinishDate ='{0}' where FinishDate ='{1}';", NowTime, LastTime);
            sql += string.Format("select * from [Order] where FinishDate >='{0}'", NowTime);
            return m_dbo.GetDataSet(sql);
        }
        #endregion

        /// <summary>
        /// 添加新订单--手工开单--lihailong
        /// </summary>
        /// <param name="go"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        public int AddOrder(Order go, OrderDetail[] orders)
        {
            bool result = true;

            Order order = new Order();
            order.Address = go.Address;
            order.ComId = go.ComId;
            order.Company = go.Company;
            Customer customer = new Customer();
            customer.Load(go.ComId);
            order.SalesId = customer.SalesId;

            order.DeptName = go.DeptName;
            order.Email = go.Email;
            order.FinishDate = go.FinishDate;
            order.GrossProfit = go.GrossProfit;
            order.Id = go.Id;
            order.Invoice = go.Invoice;
            order.Invoice_Content = go.Invoice_Content;
            order.Invoice_Name = go.Invoice_Name;
            order.Invoice_Type = go.Invoice_Type;
            order.IsInner = go.IsInner;
            order.MemberId = go.MemberId;
            order.Memo = go.Memo;
            order.Mobile = go.Mobile;
            order.OrderTime = DateTime.Now;
            order.OrderType = go.OrderType;
            order.Pay_method = go.Pay_method;
            order.PayStatus = go.PayStatus;
            order.PlanDate = go.PlanDate;
            order.Point = go.Point;
            order.PrintNum = 0;
            order.RealName = go.RealName;
            order.SaveNum = go.SaveNum + 1;
            order.SumMoney = go.SumMoney;
            order.Telphone = go.Telphone;
            order.UserId = go.UserId;
            order.RowNum = orders.Length;
            order.Emergency = go.Emergency;
            order.DeptId = go.DeptId;
            order.ApplyId = go.ApplyId;
            order.TPI_Name = go.TPI_Name;
            order.GUID = Guid.NewGuid().ToString();
            order.Tax = go.Tax;
            order.TaxRate = go.TaxRate;
            order.TaxGrossProfit = go.TaxGrossProfit;
            if (go.BranchId > 0)
            {
                order.BranchId = go.BranchId;
            }
            //else
            //{
            //    order.BranchId = 1;
            //}
            order.TPI_OrderId = go.TPI_OrderId;// add by luochuhui 07-17 接口来的订单Id
            int goodsOrderId = order.Save();
            if (goodsOrderId > 0)
            {
                GoodsManager gm = new GoodsManager();
                for (int i = 0; i < orders.Length; i++)
                {
                    Goods goods = new Goods();
                    goods.Id = orders[i].GoodsId;
                    goods.Load();

                    //添加订单明细
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.Amount = orders[i].Num * orders[i].SalePrice;
                    orderDetail.Discount = orders[i].Discount;
                    orderDetail.GoodsId = orders[i].GoodsId;
                    //add by wangpl 2015-7-17
                    orderDetail.SkuName = orders[i].SkuName;
                    //orderDetail.Model = orders[i].Model;
                    orderDetail.Num = orders[i].Num;
                    orderDetail.OrderId = goodsOrderId;
                    orderDetail.Point = orders[i].Point;
                    orderDetail.Price = orders[i].Price;
                    orderDetail.SalePrice = orders[i].SalePrice;
                    orderDetail.TaxInPrice = orders[i].TaxInPrice;
                    orderDetail.IsGift = orders[i].IsGift;
                    orderDetail.PurchaseMemo = orders[i].PurchaseMemo.Replace(" ", "").Replace("　　", "");//add by quxiaoshan 2014-11-06
                    orderDetail.IsShow = 1;
                    orderDetail.GroupParentId = 0;                                   
                    if (goods.ParentId == 1)//是否是组合商品
                    {
                        orderDetail.IsCalc = 0;
                    }
                    else orderDetail.IsCalc = 1;

                    if (orderDetail.Save() > 0)
                    {
                        if (goods.ParentId == 1)
                        {
                            //如果是组合商品，添加组合商品的明细
                            AddGroupGoodsDetail(orderDetail);
                        }
                    }
                    else
                    {
                        result = false;
                        break;

                    }

                }
                if (result)
                {
                    //添加订单状态明细
                    OrderStatusDetail osd = new OrderStatusDetail();
                    osd.IsInner = 0;
                    osd.OrderId = goodsOrderId;
                    osd.Status = "您提交了订单，请等待系统确认!";
                    osd.UserId = order.UserId;
                    osd.StatusId = 1;
                    osd.TPIStatus = CommenClass.Order_Status.未处理.ToString();
                    if (osd.UserId == 0)
                    {
                        osd.UserName = "系统";
                    }
                    else
                    {
                        Sys_Users sys_users = new Sys_Users();
                        sys_users.Id = osd.UserId;
                        if (sys_users.Load())
                        {
                            osd.UserName = sys_users.Name;
                        }
                    }
                    osd.Save();

                    //如果是礼品兑换单，减少用户的积分(增加积分是在订单出库的时候)
                    if (order.OrderType == CommenClass.OrderType.礼品兑换.ToString())
                    {
                        Pointdetail pd = new Pointdetail();
                        pd.UpdateTime = DateTime.Now;
                        pd.MemberId = order.MemberId;
                        pd.Type = CommenClass.PointType.兑换礼品.ToString();
                        pd.RelationId = order.Id;
                        pd.Income = 0;
                        pd.Spend = 0 - order.Point;

                        Member member = new Member(order.MemberId);

                        pd.Balance = member.Point + order.Point;
                        pd.Memo = "";
                        if (pd.Save() > 0)
                        {
                            return goodsOrderId;
                        }
                        //return pd.Save();
                    }
                }
                else
                {
                    order.Id = goodsOrderId;
                    order.Delete();
                    goodsOrderId = 0;
                }
                //修改订单毛利
                //order.CalcProfit();
                order.UpdateOrderSumMoney(goodsOrderId);
            }
            return goodsOrderId;
        }
        /// <summary>
        /// 新建订单时：将组合商品的 明细添加到 orderdetail表中
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        private bool AddGroupGoodsDetail(OrderDetail detail)
        {
            GoodsGroupDetail ggd = new GoodsGroupDetail();
            DataSet ds = ggd.GetGoodsGroupDetail(detail.GoodsId);
            int OKNum = 0;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                OrderDetail od = new OrderDetail();
                od.OrderId = detail.OrderId;
                od.IsShow = 0;
                od.IsCalc = 1;
                od.GroupParentId = detail.GoodsId;
                od.GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                od.Num = detail.Num * DBTool.GetIntFromRow(row, "Num", 0);
                if (od.Save() > 0)
                {
                    OKNum += 1;
                }
            }
            if (OKNum == ds.Tables[0].Rows.Count)
            {
                return true;
            }
            else return false;
        }
        /// <summary>
        /// 修改订单时：如果组合商品进行了数量修改（包括新增、删除） 将组合商品的明细变动记录到 OrderDetail表中
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        private bool ModifyGroupGoodsDetail(OrderDetail detail)
        {
            string sql = string.Format(" select * from OrderDetail where orderId={0} and GroupParentId={1} ", detail.OrderId, detail.GoodsId);
            DataSet ds = m_dbo.GetDataSet(sql);
            int OKNum = 0;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int GroupParentId = DBTool.GetIntFromRow(row, "GroupParentId", 0);
                int GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                int Id = DBTool.GetIntFromRow(row, "Id", 0);
                GoodsGroupDetail ggd = new GoodsGroupDetail();
                if (ggd.Load(GroupParentId, GoodsId))
                {
                    OrderDetail od = new OrderDetail();
                    od.Id = Id;
                    od.Load();
                    od.Num = detail.Num * ggd.Num;
                    if (od.Save() > 0)
                    {
                        OKNum += 1;
                    }
                }
            }
            if (OKNum == ds.Tables[0].Rows.Count)
            {
                return true;
            }
            else return false;
        }
        /// <summary>
        /// 修改订单--手工开单--2014-6-24
        /// </summary>
        /// <param name="go"></param>
        /// <param name="Orders"></param>
        /// <returns></returns>
        public bool ModifyOrder(Order go, OrderDetail[] orders, int userId)
        {
            Order order = new Order();
            order.Id = go.Id;
            order.Load();//主要是为了读取 老订单的仓库号,积分等等

            OrderStatus os = new OrderStatus();
            os.LoadFromOrderId(go.Id);
            order.Address = go.Address;
            order.ComId = go.ComId;
            order.Company = go.Company;

            order.DeptName = go.DeptName;
            order.Email = go.Email;
            order.FinishDate = go.FinishDate;
            order.GrossProfit = go.GrossProfit;
            order.Id = go.Id;
            order.Invoice = go.Invoice;
            order.Invoice_Content = go.Invoice_Content;
            order.Invoice_Name = go.Invoice_Name;
            order.Invoice_Type = go.Invoice_Type;
            order.IsInner = go.IsInner;
            order.MemberId = go.MemberId;
            order.Memo = go.Memo;
            order.Mobile = go.Mobile;
            order.OrderTime = go.OrderTime;
            order.OrderType = go.OrderType;
            order.Pay_method = go.Pay_method;
            order.PayStatus = go.PayStatus;
            order.PlanDate = go.PlanDate;
            order.Point = go.Point;
            order.PrintNum = 0;
            order.RealName = go.RealName;
            order.SaveNum = go.SaveNum + 1;
            order.SumMoney = go.SumMoney;
            order.Telphone = go.Telphone;
            order.Emergency = go.Emergency;
            order.RowNum = orders.Length;
            order.TaxGrossProfit= go.TaxGrossProfit;
            order.Tax = go.Tax;
            order.TaxRate = go.TaxRate;

            if (go.BranchId > 0)
            {
                order.BranchId = go.BranchId;
            }
            else
            {
                order.BranchId = 1;
            }
            //order.UserId = go.UserId;//只保留订单添加人，其他的在修改记录中体现
            int goodsOrderId = order.Save();
            if (goodsOrderId > 0)
            {
                DataTable dtOld = this.ReadOrderDetail(go.Id).Tables[0];//读取订单原来的明细
                //循环新表，处理新增商品和 修改过数量和价格的商品
                for (int i = 0; i < orders.Length; i++)
                {
                    DataRow[] rows = dtOld.Select(string.Format(" GoodsId={0} ", orders[i].GoodsId));
                    if (rows.Length == 1)//有这个商品
                    {
                        int oldNum = DBTool.GetIntFromRow(rows[0], "num", 0);
                        double oldPrice = DBTool.GetDoubleFromRow(rows[0], "SalePrice", 0);
                        string oldPurchaseMemo = DBTool.GetStringFromRow(rows[0], "PurchaseMemo", "");                      
                        if (orders[i].Num != oldNum || orders[i].SalePrice != oldPrice || orders[i].PurchaseMemo != oldPurchaseMemo )//有变化需要修改
                        {
                            OrderDetail od = new OrderDetail();
                            od.Id = DBTool.GetIntFromRow(rows[0], "Id", 0);
                            od.Load();
                            od.Num = orders[i].Num;
                            od.SalePrice = orders[i].SalePrice;
                            od.Amount = orders[i].Amount;
                            od.PurchaseMemo = orders[i].PurchaseMemo.Replace(" ", "").Replace("　　", "");//add by quxiaoshan 2014-11-06
                           
                            if (od.Save() > 0)
                            {
                                //如果是 组合商品 而且修改了数量，需要修改组合商品的明细数据数量
                                if (orders[i].Num != oldNum)
                                {
                                    Goods goods = new Goods();
                                    goods.Id = od.GoodsId;
                                    goods.Load();
                                    if (goods.ParentId == 1)
                                    {
                                        //修改组合商品明细的数量
                                        ModifyGroupGoodsDetail(od);
                                    }
                                }
                                string PurchaseStatus = "";
                                if (os.PurchaseStatus == CommenClass.Order_Status.已接受.ToString())
                                {
                                    PurchaseStatus = CommenClass.Order_Status.未处理.ToString();
                                }
                                //记录修改明细记录
                                orders[i].OrderId = goodsOrderId;
                                SaveOrderModify(orders[i], oldNum, oldPrice, userId, PurchaseStatus);
                            }
                        }
                    }
                    else //新增商品
                    {
                        OrderDetail od = new OrderDetail();
                        //od.Model = orders[i].Model;
                        od.Discount = orders[i].Discount;
                        od.GoodsId = orders[i].GoodsId;
                        od.TaxInPrice = orders[i].TaxInPrice;
                        od.Num = orders[i].Num;
                        od.OrderId = goodsOrderId;
                        od.Point = orders[i].Point;
                        od.Price = orders[i].Price;
                        od.SalePrice = orders[i].SalePrice;
                        od.Amount = orders[i].Num * orders[i].SalePrice;
                        od.IsGift = orders[i].IsGift;
                        od.IsShow = 1;
                        od.GroupParentId = 0;
                       
                        if (os.PurchaseStatus == CommenClass.Order_Status.已接受.ToString())
                        {
                            od.PurchaseStatus = CommenClass.Order_Status.未处理.ToString();
                        }
                        else
                        {
                            od.PurchaseStatus = "";
                        }
                        Goods goods = new Goods();
                        goods.Id = od.GoodsId;
                        goods.Load();

                        if (goods.ParentId == 1)//是否是组合商品
                        {
                            od.IsCalc = 0;
                        }
                        else od.IsCalc = 1;

                        if (od.Save() > 0)
                        {
                            //如果是组合商品，需要新增 组合的明细
                            if (goods.ParentId == 1)
                            {
                                AddGroupGoodsDetail(od);
                            }
                            string PurchaseStatus = "";
                            if (os.PurchaseStatus == CommenClass.Order_Status.已接受.ToString())
                            {
                                PurchaseStatus = CommenClass.Order_Status.未处理.ToString();
                            }
                            //记录订单修改明细
                            orders[i].OrderId = goodsOrderId;
                            SaveOrderModify(orders[i], 0, 0, userId, PurchaseStatus);
                        }

                    }
                }
                //循环旧表，查找新表中没有的项。删除，记录明细
                foreach (DataRow row in dtOld.Select(" IsShow = 1 "))
                {
                    int goodsId = DBTool.GetIntFromRow(row, "goodsId", 0);
                    string model = DBTool.GetStringFromRow(row, "Model", "");
                    int oldnum = DBTool.GetIntFromRow(row, "num", 0);
                    double oldPrice = DBTool.GetDoubleFromRow(row, "SalePrice", 0);
                    bool isExsist = false;
                    for (int i = 0; i < orders.Length; i++)
                    {
                        if (orders[i].GoodsId == goodsId)
                        {
                            isExsist = true;
                            break;
                        }
                    }
                    if (isExsist == false)
                    {
                        //新订单中无 此项
                        int odId = DBTool.GetIntFromRow(row, "Id", 0);
                        OrderDetail od = new OrderDetail();
                        od.Id = odId;
                        od.GoodsId = goodsId;
                        //od.Model = model;
                        od.Num = 0;
                        od.OrderId = goodsOrderId;
                        od.SalePrice = oldPrice;
                        if (od.Delete())//删除订单明细 如果是组合商品 删除
                        {
                            //记录订单 修改记录
                            string PurchaseStatus = "";
                            if (os.PurchaseStatus == CommenClass.Order_Status.已接受.ToString())
                            {
                                PurchaseStatus = CommenClass.Order_Status.未处理.ToString();
                            }
                            SaveOrderModify(od, oldnum, oldPrice, userId, PurchaseStatus);
                        }
                    }
                }
                order.UpdateOrderSumMoney(goodsOrderId);
            }
            return true;

        }
        /// <summary>
        /// 批量修改订单加急状态
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        public int ModifyEmergency(int[] orderIds)
        {
            int n = 0;
            for (int i = 0; orderIds.Length > i; i++)
            {
                string sql = string.Format("update [Order] set Emergency=1  where  Id ={0} ", orderIds[i]);
                if (m_dbo.ExecuteNonQuery(sql))
                {
                    n += 1;
                }
            }
            return n;
        }

        private int SaveOrderModify(OrderDetail od, int OldNum, double OldPrice, int UserId, string PurchaseStatus)
        {
            OrderModify omd = new OrderModify();
            omd.GoodsId = od.GoodsId;
            omd.Id = 0;
            //omd.Model = od.Model;
            omd.NewNum = od.Num;
            omd.NewPrice = od.SalePrice;
            omd.OldNum = OldNum;
            omd.OldPrice = OldPrice;
            omd.OrderId = od.OrderId;
            omd.UpdateTime = DateTime.Now;
            omd.UserId = UserId;
            omd.OrderId = od.OrderId;
            omd.PurchaseType = PurchaseStatus;
            return omd.Save();
        }
        /// <summary>
        /// 读取订单修改明细记录
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public DataSet ReadOrderModify(int orderId)
        {
            string sql = string.Format("select * from view_OrderModify where OrderId={0} order by Id ", orderId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取订单修改列表
        /// </summary>
        /// <returns></returns>
        public DataSet ReadModifyOrderReport(ReportOption option)
        {
            string sql = string.Format(@"
select DeliveryName as 配送员,COUNT(OrderId) as 订单数量 ,SUM(PackageNum)as 件数 from View_Order  where SumMoney<0 and BranchId={0} and StoreFinishTime>='{1}' and StoreFinishTime<'{2}' group by DeliveryName order by 订单数量 desc;
", option.BranchId, option.StartDate.ToShortDateString(), option.EndDate.AddDays(1).ToShortDateString());
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 删除订单--手工订单/网络订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool DeleteOrder(int orderId)
        {
            Order go = new Order();
            go.Id = orderId;
            //go.Load();
            return go.Delete();
        }
        /// <summary>
        /// 订单出库--修改库存
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool OutBoundOrder(int orderId, int storeId, int UserId)
        {
            if (!IsOrderCanOutBound(orderId, storeId))
            {
                return false;
            }

            //修改订单状态
            string sql = string.Format(" update OrderStatus set StoreStatus='已完成',StoreFinishTime='{0}' where OrderId={1} ", DateTime.Now.ToString(), orderId);
            if (m_dbo.ExecuteNonQuery(sql))
            {
                Order order = new Order();
                order.Id = orderId;
                order.Load();
                if (order.OrderType == CommenClass.OrderType.销售退单.ToString())
                {
                    string sql2 = string.Format("  update OrderStatus set DeliveryStatus='已完成',DeliveryFinishTime='{0}' where OrderId={1} ", DateTime.Now.ToString(), orderId);
                    m_dbo.ExecuteNonQuery(sql2);
                }

                GoodsStoreManager gsm = new GoodsStoreManager();
                //只读取IsCalc=1的商品明细
                option.IsCalc = 1;
                option.IsShow = -1;
                DataSet ds = ReadOrderDetail(orderId);
                int Error = 0;
                //读取商品明细
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    //循环单个商品
                    int goodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                    string Model = DBTool.GetStringFromRow(row, "Model", "");
                    int num = DBTool.GetIntFromRow(row, "Num", 0);
                    double saleprice = DBTool.GetDoubleFromRow(row, "SalePrice", 0);                   
                    int Id = DBTool.GetIntFromRow(row, "Id", 0);
                    GoodsStore gs = new GoodsStore();
                    gs.GoodsId = goodsId;
                    gs.StoreId = storeId;
                    gs.Load(storeId,goodsId);
                    gs.UpdateTime = DateTime.Now;
                    gs.Num = gs.Num - num;
                    if (gs.Save() > 0)
                    {
                        GoodsStoreDetail gsd = new GoodsStoreDetail();
                        gsd.GoodsId = goodsId;
                        gsd.Id = 0;
                        gsd.NewNum = gs.Num;
                        gsd.Num = num;
                        gsd.OldNum = gs.Num + num;
                        if (num > 0)
                        {
                            if (saleprice == 0)
                            {
                                gsd.Operate = CommenClass.StoreOperateType.赠品出库.ToString();
                            }
                            else gsd.Operate = CommenClass.StoreOperateType.销售出库.ToString();
                        }
                        else
                        {
                            gsd.Operate = CommenClass.StoreOperateType.销售退货.ToString();
                        }
                        gsd.RelationId = orderId;
                        gsd.StoreId = storeId;
                        gsd.UpdateTime = DateTime.Now;
                        gsd.UserId = UserId;
                        gsd.AC = gs.AC;
                        gsd.TaxAC = gs.TaxAC;
                        if (gsd.Save() > 0)
                        {
                            OrderDetail od = new OrderDetail();
                            od.Id = Id;
                            od.Load();
                            od.AC = gs.AC;
                            od.TaxAC = gs.TaxAC;
                            if (od.Save() < 1) {
                                Error += 1;
                            }
                            
                        }
                        else {
                            Error += 1;
                        }
                    }
                    else {
                        Error += 1;
                    }                  
                   
                }
                if (Error == 0)
                {
                    //修改订单毛利
                    string sql3 = string.Format(@"update [Order] set GrossProfit = SumMoney/(1+TaxRate)-(select SUM(AC*Num) from OrderDetail where OrderId ={0}),
                                                                     TaxGrossProfit = SumMoney - (select SUM(TaxAC*Num) from OrderDetail where OrderId ={0})
                                                  where Id ={0}", orderId);
                    m_dbo.ExecuteNonQuery(sql3);

                    //增加网络订单的积分
                    if (order.OrderType == CommenClass.OrderType.网上订单.ToString())
                    {
                        AddOrderPoint(order);
                    }
                    return true;
                }
                else
                {
                    Sys_ErrorLog errorlog = new Sys_ErrorLog();
                    errorlog.ErrorMessage = string.Format("订单出库失败：订单号{0}，已出库但是商品明细出库部分有错误！", orderId);
                    errorlog.RelationId = orderId.ToString();
                    errorlog.TableName = "OrderStatus";
                    errorlog.UpdateTime = DateTime.Now;
                    errorlog.Save();
                    return false;
                }
            }
            else
            {
                Sys_ErrorLog errorlog = new Sys_ErrorLog();
                errorlog.ErrorMessage = string.Format("订单出库失败：订单号{0}，订单状态修改失败！", orderId);
                errorlog.RelationId = orderId.ToString();
                errorlog.TableName = "OrderStatus";
                errorlog.UpdateTime = DateTime.Now;
                errorlog.Save();
                return false;
            }

        }
        /// <summary>
        /// 根据出库是的状态，增加用户的积分
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private int AddOrderPoint(Order order)
        {
            VIPCounterCompany vCounterCompany = new VIPCounterCompany();
            if (vCounterCompany.LoadFromComId(order.ComId))
            {
                return 0;
            }
            Pointdetail pd = new Pointdetail();
            pd.UpdateTime = DateTime.Now;
            pd.MemberId = order.MemberId;
            pd.Type = CommenClass.PointType.订单积分.ToString();
            pd.RelationId = order.Id;
            pd.Spend = 0;


            double income = 0;
            OrderManager om = new OrderManager();
            DataSet dsDetail = om.ReadOrderDetail(order.Id);
            foreach (DataRow row in dsDetail.Tables[0].Rows)
            {
                income += DBTool.GetDoubleFromRow(row, "SalePrice", 0) * DBTool.GetDoubleFromRow(row, "Num", 0) * DBTool.GetDoubleFromRow(row, "rate", 0) / 100;
            }

            Member member = new Member(order.MemberId);

            pd.Income = income;
            pd.Balance = member.Point + income;
            pd.Memo = "";
            return pd.Save();
        }

        /// <summary>
        /// 订单积分
        /// </summary>
        /// <param name="orderId"></param>
        public void OrderPoint(int orderId)
        {
            Order order = new Order(orderId);
            if (order.OrderType == CommenClass.OrderType.网上订单.ToString())
            {
                AddOrderPoint(order);
            }
        }

        /// <summary>
        /// 取消兑换的礼品，将积分返还
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int DeleteOrderPoint(int orderId)
        {
            Pointdetail pd = new Pointdetail();
            pd.RelationId = orderId;
            DataTable dt = pd.ReadByRelationId(orderId).Tables[0];
            double pointSpend = Convert.ToDouble(dt.Rows[0]["Spend"]);
            int memberId = Convert.ToInt32(dt.Rows[0]["MemberId"]);

            pd.UpdateTime = DateTime.Now;
            pd.Type = CommenClass.PointType.订单积分.ToString();
            pd.RelationId = orderId;
            pd.Spend = 0;
            pd.Income = pointSpend;
            pd.MemberId = memberId;

            Member member = new Member(memberId);
            pd.Balance = member.Point + pointSpend;
            pd.Memo = "";
            return pd.Save();
        }

        #region  检查出库bug用的方法
        /// <summary>
        /// 检查 订单是否已经有了出库明细记录
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>true：有明细记录</returns>
        public bool IsOrderHaveGoodsStoreDetail(int orderId)
        {
            string sql = string.Format(" select Count(*) as Num from GoodsStoreDetail where RelationId ={0} and ( Operate = '{1}' or Operate='{2}' or Operate='{3}'  ) ", orderId, CommenClass.StoreOperateType.销售出库.ToString(), CommenClass.StoreOperateType.销售退货.ToString(), CommenClass.StoreOperateType.赠品出库.ToString());
            DataSet ds = m_dbo.GetDataSet(sql);
            if (DBTool.GetIntFromRow(ds.Tables[0].Rows[0], "Num", 0) > 0)
            {
                return true;
            }
            else return false;
        }
        #endregion
        /// <summary>
        /// 检查订单是否库存充足可以出库，没有检查订单状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="StoreId"></param>
        /// <returns></returns>
        public bool IsOrderCanOutBound(int orderId, int StoreId)
        {
            GoodsStoreManager gsm = new GoodsStoreManager();
            //读取IsCalc=1 的商品汇总明细
            string sql = string.Format(@"select GoodsId,SUM(Num) as Num,
(select SUM(Num)from view_GoodsStore where GoodsId=a.GoodsId and StoreId={1} and StoreZone<>'收货区' group by GoodsId ) as StoreNum
from view_OrderDetail a where IsCalc=1 and OrderId={0} group by GoodsId ", orderId, StoreId);

            DataSet ds = m_dbo.GetDataSet(sql);
            //读取商品明细
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                //循环单个商品
                int num = DBTool.GetIntFromRow(row, "Num", 0);
                int storeNum = DBTool.GetIntFromRow(row, "StoreNum", 0);
                if (storeNum < num)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 读取已配齐缺货订单 缺货汇总---2014-10-08 已废弃
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public DataSet ReadYiPeiQiQueHuo(int branchId)
        {
            string sql = string.Format(@"select GoodsId,DisplayName, Model,SUM(Num) as Num,unit, dbo.GetGoodsStore(GoodsId,1,{0}) as StockNum from View_OrderDetail where OrderId in
( select OrderId from View_Order where StoreStatus='已配齐' and BranchId={0})
group by GoodsId,DisplayName,Model,unit", branchId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 订单复制,做假单
        /// </summary>
        /// <param name="RawOrderId"></param>
        /// <returns>返回新订单的单号</returns>
        public int CopyOrderReplace(int RawOrderId, int UserId)
        {
            Order r = new Order();
            r.Id = RawOrderId;
            r.Load();

            Order n = new Order();
            n.Address = r.Address;
            n.ComId = r.ComId;
            n.Company = r.Company;
            n.DeptName = r.DeptName;
            n.Email = r.Email;
            n.FinishDate = DateTime.Now;
            n.GrossProfit = 0;
            n.Invoice = r.Invoice;
            n.Invoice_Content = r.Invoice_Content;
            n.Invoice_Name = r.Invoice_Name;
            n.Invoice_Type = r.Invoice_Type;
            n.IsCopied = 0;
            n.IsInner = 0;
            n.MemberId = r.MemberId;
            n.Memo = r.Memo;
            n.Mobile = r.Mobile;
            n.OrderTime = r.OrderTime;
            n.OrderType = r.OrderType;
            n.Pay_method = r.Pay_method;
            n.PayStatus = CommenClass.PayStatus.未付款.ToString();//复制的假单 ，付款状态都 设置为 未付款
            n.PlanDate = r.PlanDate;
            n.Point = 0;
            n.PrintNum = 0;
            n.RawOrderId = RawOrderId;
            n.RealName = r.RealName;
            n.RowNum = r.RowNum;
            n.SaveNum = 1;
            n.SumMoney = r.SumMoney;
            n.Telphone = r.Telphone;
            n.UpdateTime = DateTime.Now;
            n.UserId = UserId;
            n.BranchId = r.BranchId;
            n.TPI_Name = "";
            n.GUID = Guid.NewGuid().ToString();
            int nId = n.Save();
            if (nId > 0)
            {
                OrderStatus os = new OrderStatus();
                os.LoadFromOrderId(nId);
                os.StoreFinishTime = DateTime.Now;
                os.StoreStatus = CommenClass.Order_Status.已完成.ToString();
                os.DeliveryFinishTime = DateTime.Now;
                os.DeliveryStatus = CommenClass.Order_Status.已完成.ToString();
                os.Save();//保存新订单的 状态：均设置为已完成

                r.IsCopied = 1;
                r.Save();//老订单修改：复制标志

                DataSet ds = this.ReadOrderDetail(RawOrderId);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    OrderDetail od = new OrderDetail();
                    od.Id = DBTool.GetIntFromRow(ds.Tables[0].Rows[i], "Id", 0);
                    od.Load();
                    od.Id = 0;
                    od.OrderId = nId;
                    od.Save();
                }
            }
            return nId;
        }

        /// <summary>
        /// 订单复制
        /// </summary>
        /// <param name="RawOrderId"></param>
        /// <returns>返回新订单的单号</returns>
        public int CopyOrder(int RawOrderId, int UserId)
        {
            Order r = new Order();
            r.Id = RawOrderId;
            r.Load();

            Order n = new Order();
            n.Address = r.Address;
            n.ComId = r.ComId;
            n.Company = r.Company;
            n.DeptName = r.DeptName;
            n.Email = r.Email;
            n.FinishDate = DateTime.Now;
            n.GrossProfit = 0;
            n.Invoice = r.Invoice;
            n.Invoice_Content = r.Invoice_Content;
            n.Invoice_Name = r.Invoice_Name;
            n.Invoice_Type = r.Invoice_Type;
            n.IsCopied = 0;
            n.IsInner = 0;
            n.MemberId = r.MemberId;
            n.Memo = r.Memo;
            n.Mobile = r.Mobile;
            n.OrderTime = r.OrderTime;
            n.OrderType = r.OrderType;
            n.Pay_method = r.Pay_method;
            n.PayStatus = r.PayStatus;
            n.PlanDate = r.PlanDate;
            n.Point = 0;
            n.PrintNum = 0;
            n.RawOrderId = 0;
            n.RealName = r.RealName;
            n.RowNum = r.RowNum;
            n.SaveNum = 1;
            n.SumMoney = r.SumMoney;
            n.Telphone = r.Telphone;
            n.UpdateTime = DateTime.Now;
            n.UserId = UserId;
            n.TPI_Name = "";
            n.GUID = Guid.NewGuid().ToString();
            int nId = n.Save();
            if (nId > 0)
            {
                DataSet ds = this.ReadOrderDetail(RawOrderId);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    OrderDetail od = new OrderDetail();
                    od.Id = DBTool.GetIntFromRow(ds.Tables[0].Rows[i], "Id", 0);
                    od.Load();
                    od.Id = 0;
                    od.OrderId = nId;
                    od.Save();
                }
            }
            return nId;
        }

        /// <summary>
        /// 读取各个类别的销售总额 add by quxiaoshan 2014-11-04
        /// </summary>
        /// <returns></returns>
        public DataSet ReadPCodeSumAmount()
        {
            //string sqlaa = string.Format(@"select   SUBSTRING( d.Code,1,2) as PPCode , (select top 1  TypeName  from dbo.GoodsType t where t.Code = (SUBSTRING(d.Code,1,2))) as PPCodeName, SUM(Amount) as SumAmount  ,company ,SalesName
            //from dbo.View_OrderDetail  d 
            //where d.StoreFinishTime>='2014-10-01' and d.StoreFinishTime<'2014-11-01' and ComId=1027   and   SalesId=19
            //group by SUBSTRING( d.Code,1,2)  ,SalesName,company
            //order by SUBSTRING( d.Code,1,2);");

            string sqlSelect = @"select  SUBSTRING( d.Code,1,2) as PPCode ,ComId ,company , (select top 1  TypeName  from dbo.GoodsType t where t.Code = (SUBSTRING(d.Code,1,2))) as PPCodeName,OrderSalesName, SUM(Amount) as SumAmount ";
            string sqlWhere = "";

            sqlSelect += " from dbo.View_OrderDetail  d where 1=1 ";
            if (option.ComId > 0)
            {
                sqlSelect += string.Format(" and ComId={0} ", option.ComId);
            }
            if (option.BranchId != 0)
            {
                sqlSelect += string.Format(" and BranchId={0}", option.BranchId);
            }
            if (option.SalesId > 0)
            {
                sqlSelect += string.Format(" and OrderSalesId={0} ", option.SalesId);
            }
            if (option.StoreTimeS > new DateTime(1900, 1, 1))
            {
                sqlSelect += string.Format(" and StoreFinishTime >='{0}' ", option.StoreTimeS.ToShortDateString());
            }
            if (option.StoreTimeE > new DateTime(1900, 1, 1))
            {
                sqlSelect += string.Format(" and StoreFinishTime <'{0}' ", option.StoreTimeE.AddDays(1).ToShortDateString());
            }
            sqlSelect += sqlWhere + "group by SUBSTRING( d.Code,1,2) " + ",OrderSalesName,ComId,company " + " order by ComId, SUBSTRING( d.Code,1,2);";

            return m_dbo.GetDataSet(sqlSelect);
        }

        public string DeleteOrderDetail(int OrderId, int GoodsId, int userId)
        {
            Order order = new Order();
            order.Id = OrderId;
            order.Load();//主要是为了读取 老订单的仓库号,积分等等

            OrderStatus os = new OrderStatus();
            os.LoadFromOrderId(OrderId);

            OrderDetail od = new OrderDetail();
            od.Load(OrderId, GoodsId);
            string Exception = "";
            if (od.PickNum == 0)
            {
                if (od.Delete())//删除订单明细 如果是组合商品 删除
                {
                    order.UpdateOrderSumMoney(OrderId);
                    od.GoodsId = GoodsId;
                    od.OrderId = OrderId;
                    int OldNum = od.Num;
                    od.Num = 0;
                    //记录订单 修改记录
                    string PurchaseStatus = "";
                    if (os.PurchaseStatus == CommenClass.Order_Status.已接受.ToString())
                    {
                        PurchaseStatus = CommenClass.Order_Status.未处理.ToString();
                    }
                    SaveOrderModify(od, OldNum, od.SalePrice, userId, PurchaseStatus);
                   
                    Exception = "删除成功！";
                }
            }
            else
            {
                Exception = "该商品已经拣货，请联系拣货人员撤销该商品！";
            }
            return Exception;
        }

        public bool DeleteOrderDetail(int OrderId, int GoodsId, int userId, int num)
        {
            Order order = new Order();
            OrderDetail od = new OrderDetail();
            od.Load(OrderId, GoodsId);
            int OldNum = od.Num;
            od.GoodsId = GoodsId;
            od.OrderId = OrderId;
            bool result = false;
            if (num > 0)
            {
                if (od.UpdateNum(OrderId, num, GoodsId))
                {
                    od.Num = num;
                    result = true;
                }
            }
            else
            {
                if (od.Delete())//删除订单明细 如果是组合商品 删除
                {
                    od.Num = 0;
                    result = true;
                }
            }
            SaveOrderModify(od, OldNum, od.SalePrice, userId, "");
            order.UpdateOrderSumMoney(OrderId);
            if (od.DetailCount(OrderId) == 0)
            {
                order.Id = OrderId;
                order.Load();
                order.IsDelete = 1;
                order.Save();
            }
            return result;
        }
        /// <summary>
        /// 根据订单号修改(新加)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool ModifyOrderIsStorage(int Id)
        {
            string sql = string.Format("update [Order] set IsStorage=1 where Id={0}", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        #region
        /// <summary>
        /// 按照对账单读取订单
        /// </summary>
        /// <returns></returns>
        public DataSet ReadStatementOrder()
        {
            string sql = @"select isnull( (select COUNT(OrderId) from OrderPhoto op where op.OrderId  =vo.OrderId group by OrderId  ),0 )OrderPhotoNum,OrderId,DeptName,DeptId,RealName,SumMoney,OrderTime,StoreFinishTime,DeliveryStatus,OrderSalesName,UserName,PickingName,DeliveryName,Memo,ServiceId,SalesId,PickingUserId,DeliveryUserId,GUID,Tax,TaxRate,Invoice_Type from View_Order vo  where IsDelete=0 ";
            sql += GetSQLWhere();
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrderStatementId"></param>
        /// <returns></returns>
        public string GetStatementguid(int OrderStatementId)
        {
            string sql = string.Format("select top 1 GUID from OrderStatementDetail osd join (select *,isnull( (select COUNT(OrderId) from OrderPhoto op where  op.OrderId  =o.Id group by OrderId  ),0) OrderPhotoNum from [Order] o  where o.GUID !='' ) as orderP on osd.OrderId=orderP.Id and orderP.OrderPhotoNum>0 and osd.OrderStatementId={0} order by GUID desc;", OrderStatementId);
            object o = m_dbo.ExecuteScalar(sql);
            try
            {
                return o.ToString();
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region 订单拆单
        public int UnweaveOrder(int OldOrderId, OrderDetail[] details, int UserId)
        {
            int SynchOrderId = SetOrderDetail(OldOrderId, details);
            if (SynchOrderId > 0)
            {
                UpOrderDetail(OldOrderId, SynchOrderId, UserId);
            }
            return SynchOrderId;
        }


        public int SetOrderDetail(int OrderId, OrderDetail[] details)
        {
            Order order = new Order(OrderId);
            order.Id = 0;
            order.RowNum = details.Length;
            order.SaveNum = 0;
            order.GrossProfit = 0;
            order.TaxGrossProfit = 0;
            order.SumMoney = 0;
            order.Tax = 0;            
            order.Memo += string.Format("[{0}拆单]", OrderId);//2016-08-09 huangwenhua  解决拆单备注
            OrderManager om = new OrderManager();
            return om.AddOrder(order, details);
        }

        /// <summary>
        /// 同步主的订单
        /// </summary>
        /// <param name="OldPurchaseId"></param>
        /// <param name="SynchPurchaseId"></param>
        /// <returns></returns>
        private bool UpOrderDetail(int OldOrderId, int SynchOrderId, int UserId)
        {
            string sql = string.Format(@"update od1 set Num=od1.Num-od2.Num,Amount=(od1.Num-od2.Num)* od1.SalePrice from OrderDetail od1 inner join OrderDetail od2 on od1.GoodsId=od2.GoodsId where od1.OrderId={0} and od2.OrderId={1};
                                         delete OrderDetail where orderid={0} and Num=0;
                                         update [Order] set SumMoney=(select ISNULL(SUM(Amount),0) from OrderDetail  where OrderId= {0} and IsShow=1 ),
                                                            RowNum=isnull((select COUNT(OrderId) from OrderDetail  where OrderId= {0}),0),
                                                            Tax=(select ISNULL(SUM(Amount),0) from OrderDetail  where OrderId= {0} and IsShow=1 )/(1+TaxRate)*TaxRate  where Id={0}", OldOrderId, SynchOrderId);
            return m_dbo.ExecuteNonQuery(sql);
        }

        public DataSet GetPickGoodsId(int Orderid, string GoodsIds)
        {
            string sql = string.Format("select * from OrderDetail where PickNum>0 and OrderId ={0} and GoodsId in({1})", Orderid, GoodsIds);
            return m_dbo.GetDataSet(sql);
        }



        /// <summary>
        /// 批量修改订单下单日期
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        public int ModifyOrderOrderTime(int[] orderIds, string updatetime)
        {
            int n = 0;
            for (int i = 0; orderIds.Length > i; i++)
            {
                string sql = string.Format(" update [Order] set OrderTime= '{0}' , updateTime = '{0}' where Id={1} ", updatetime, orderIds[i]);
                if (m_dbo.ExecuteNonQuery(sql))
                {
                    n += 1;
                }
            }
            return n;
        }
        //public DataSet GetPickGoodsId(int Orderid, string GoodsIds)
        //{
        //    string sql = string.Format("select * from OrderDetail where PickNum>0 and OrderId ={0} and GoodsId in({1})", Orderid, GoodsIds);
        //    return m_dbo.GetDataSet(sql);
        //}
        #endregion

        public DataSet GetSalesVolume(DateTime startTime, DateTime endTime)
        {
            string sql = string.Format(@"select convert(varchar(10),OrderTime,120) ordertime,sum(SumMoney) summoney  from View_Order where BranchId=1 and DeliveryStatus='已完成' and OrderTime between '{0}' and '{1}'
   group by convert(varchar(10),OrderTime,120) order by ordertime", startTime, endTime);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 根据BranchId读取待配送订单
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public DataSet GetNeedDeliveryLoading(int branchId)
        {
            string sql = string.Format(@"select * from View_Order  where  BranchId={0} and IsDelete<>1 and IsInner<>1 and IsCopied<>1 and storeStatus='已完成' and DeliveryStatus<>'已完成' order by StoreFinishTime desc", branchId);
            return m_dbo.GetDataSet(sql);
        }

    }

}
