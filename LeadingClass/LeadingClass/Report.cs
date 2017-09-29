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
    public class ReportOption
    {
        public int BranchId { get; set; }
        public int MemberId { get; set; }
        public int ComId { get; set; }//-1：汇总；0：按照公司汇总，>0 只统计一个公司
        public OrderTimeType DateType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SalesId { get; set; }
        public string StoreStatus { get; set; }
        public string DeliveryStatus { get; set; }
        public int StoreId { get; set; }
        public int TypeId { get; set; }
        public string KeyWords { get; set; }
        public int[] DeptId { get; set; }//update by luochunhui 根据子部门获取部门的数组
        public int Level { get; set; }
        public PageModel pageModel { get; set; }
        public int IsMonthReport { get; set; }//一个方法两个地方调用，只做标识
        public int GoodsStoreID { get; set; }
        public int GoodsID { get; set; }
        public ReportOption()
        {
            BranchId = 0;
            ComId = 0;
            MemberId = 0;
            DateType = OrderTimeType.出库日期;
            StartDate = new DateTime(1900, 1, 1);
            EndDate = new DateTime(1900, 1, 1);
            SalesId = 0;
            StoreStatus = "";
            DeliveryStatus = "";
            StoreId = 0;
            TypeId = 0;
            KeyWords = "";
            Level = 0;
            DeptId = null;
            GoodsStoreID = 0;
            GoodsID = 0;
            pageModel = new PageModel();
        }
    }
    public class Report
    {
        private DBOperate m_dbo;
        public ReportOption reportOption { get; set; }//add by quxiaoshan 2015-3-7 公司各部门的月采购数量
        public Report()
        {
            reportOption = new ReportOption();
            m_dbo = new DBOperate();
        }
        #region 应收应付
        
        //应付账款汇总表
        //ERP FAP.CS 中调用
        public DataSet ReadAP(ReportOption option)
        {
            string sql = string.Format(@"with cte_Purchase as(
                                          SELECT SupplierId,s.Company,s.ShortName,
                                          Sum(SumMoney) as NeedToPayWDZ 
                                          FROM [Purchase](nolock) p
                                          left join  Supplier(nolock) s on p.SupplierId=s.Id
                                          WHERE  IsPaid='0' and PurchaseStatus='已入库'");
            if (option.EndDate != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and UpdateTime < '{0}' ", option.EndDate.AddDays(1).Date);
            }
            if (option.SalesId > 0)
            {
                sql += string.Format(" and SalesId={0} ", option.SalesId);
            }
            if (option.BranchId != 0)
            {
                sql += string.Format(@" and p.BranchId={0}", option.BranchId);
            }
            sql += string.Format(@"group by SupplierId,s.Company,s.ShortName),
                                   cte_ps as(
                                   SELECT SupplierId,s.Company,s.ShortName,
                                   sum(SumMoney -PaidMoney) as NeedToPayYDZ 
                                   FROM [PurchaseStatement] ps left join Supplier(nolock) s on ps.SupplierId=s.Id
                                   WHERE  PayStatus<>'已付款'");
            if (option.BranchId != 0)
            {
                sql += string.Format(@"and ps.BranchId=1", option.BranchId);
            }
            if (option.SalesId > 0)
            {
                sql += string.Format(" and SalesId={0} ", option.SalesId);
            }

            sql += string.Format(@"group by SupplierId,s.Company,s.ShortName)
select a.SupplierId,a.Company,a.ShortName,isnull(a.NeedToPayWDZ,0)as NeedToPayWDZ ,isnull(b.NeedToPayYDZ,0)as NeedToPayYDZ ,isnull((a.NeedToPayWDZ + b.NeedToPayYDZ),0) as NeedToPay  from cte_Purchase a left join cte_ps b on A.SupplierId=b.SupplierId 
union
select a.SupplierId,a.Company,a.ShortName,isnull(b.NeedToPayWDZ,0) as NeedToPayWDZ,isnull(a.NeedToPayYDZ,0) as NeedToPayYDZ,isnull((b.NeedToPayWDZ + a.NeedToPayYDZ),0) as NeedToPay from cte_ps a left join cte_Purchase b on A.SupplierId=b.SupplierId  order by NeedToPay desc");
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取应付账款--区间数据
        /// ERP  FAP.CS 中调用
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadAPReport(ReportOption option)
        {
            string sql = string.Format(@"with cte1 as(
select pd.SupplierId,SUM(SumMoney) as QCCG  from Purchase(nolock) pd  where  UpdateTime >='1900-1-1' and UpdateTime<'{1}' and BranchId={0} and PurchaseStatus='已入库'  and (PurchaseType = '签单收货' or PurchaseType = '签单退货')group by pd.SupplierId),
    cte2 as (select ps.SupplierId,SUM(PayMoney) as QCFK from PurchasePay(nolock) pp INNER JOIN PurchaseStatement(nolock) ps ON pp.PurchaseStatementId = ps.Id  where  PayDate >='1900-1-1' and PayDate<'{1}' and pp.BranchId={0} group by ps.SupplierId),
    cte3 as (SELECT p.SupplierId, sum(p.SumMoney) as QJXJCG	FROM dbo.Purchase(nolock) p LEFT JOIN dbo.Supplier(nolock) s ON p.SupplierId = s.ID where  p.UpdateTime >='{1}' and p.UpdateTime<'{2}' and p.BranchId={0} and PurchaseStatus='已入库'  and (PurchaseType = '现金采购' or PurchaseType = '现金退货') group by p.SupplierId),
    cte4 as (SELECT p.SupplierId, sum(p.SumMoney) as QJQDCG	FROM dbo.Purchase(nolock) p LEFT JOIN dbo.Supplier(nolock) s ON p.SupplierId = s.ID  where p.UpdateTime >='{1}' and p.UpdateTime<'{2}' and p.BranchId={0} and PurchaseStatus='已入库' and (PurchaseType = '签单收货' or PurchaseType = '签单退货') group by p.SupplierId),
    cte5 as (select ps.SupplierId,SUM(PayMoney) as QJFK,SUM(Chargeoff) as QJXZ from PurchasePay(nolock) pp  INNER JOIN PurchaseStatement(nolock) ps ON pp.PurchaseStatementId = ps.Id where PayDate >='{1}' and PayDate<'{2}' and pp.BranchId={0} group by ps.SupplierId),
    cte6 as (SELECT s.ID,s.Company FROM dbo.Purchase(nolock) p INNER JOIN  dbo.PurchaseDetail(nolock) ON p.Id = dbo.PurchaseDetail.PurchaseId INNER JOIN dbo.Goods(nolock) ON dbo.PurchaseDetail.GoodsId = dbo.Goods.ID INNER JOIN  dbo.Supplier(nolock) s ON p.SupplierId = s.ID  where p.BranchId={0} and PurchaseStatus='已入库' and p.UpdateTime <'{2}' group by s.ID,s.Company)
	select s.ID,s.Company,c1.QCCG,c2.QCFK,isnull(c3.QJXJCG,0) as QJXJCG,c4.QJQDCG,c5.QJFK,c5.QJXZ from cte6 s left join cte1 c1 on c1.SupplierId=s.ID 	left join cte2 c2 on c2.SupplierId=s.ID	left join cte3 c3 on c3.SupplierId=s.ID	left join cte4 c4 on c4.SupplierId=s.ID left join cte5 c5 on c5.SupplierId=s.ID
", option.BranchId, option.StartDate.ToShortDateString(), option.EndDate.AddDays(1).ToShortDateString());
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 应收账款汇总表--余额
        /// ERP FAR.cs 中调用
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadAR(ReportOption option)
        {
            List<DateTime> dates = new List<DateTime>();
            dates.Add(option.EndDate.AddDays(1).Date);
            DateTime firstStart = new DateTime(option.EndDate.Year, option.EndDate.Month, 1);
            dates.Add(firstStart);
            dates.Add(firstStart.AddMonths(-1));
            dates.Add(firstStart.AddMonths(-2));
            dates.Add(firstStart.AddMonths(-3));
            string sql = string.Format(@" select ComId as '客户ID',Name as '客户名称', SalesName as '销售员',ServiceName as '客服',StatementManName as '对账负责人', 
(select SUM(summoney-chargeoff) from View_Order where IsDelete=0 and RawOrderId=0 and PayStatus <> '已付款'
and StoreFinishTime>'2014-1-1' and StoreFinishTime <'{0}' and ComId=View_Company.ComId) as 应收总额,
(select SUM(summoney-chargeoff) from View_Order where IsDelete=0 and RawOrderId=0 and  PayStatus <> '已付款'
and StoreFinishTime>='{1}'and StoreFinishTime <'{0}' and ComId=View_Company.ComId) as '{5}',
(select SUM(summoney-chargeoff) from View_Order where IsDelete=0 and RawOrderId=0 and  PayStatus <> '已付款'
and StoreFinishTime>='{2}'and StoreFinishTime <'{1}' and ComId=View_Company.ComId) as '{6}',
(select SUM(summoney-chargeoff) from View_Order where IsDelete=0 and RawOrderId=0 and  PayStatus <> '已付款'
and StoreFinishTime>='{3}'and StoreFinishTime <'{2}' and ComId=View_Company.ComId) as '{7}',
(select SUM(summoney-chargeoff) from View_Order where IsDelete=0 and RawOrderId=0 and  PayStatus <> '已付款'
and StoreFinishTime>='{4}'and StoreFinishTime <'{3}' and ComId=View_Company.ComId) as '{8}',
(select SUM(summoney-chargeoff) from View_Order where IsDelete=0 and RawOrderId=0 and  PayStatus <> '已付款'
and StoreFinishTime>'2014-1-1'and StoreFinishTime <'{4}' and ComId=View_Company.ComId) as '更久',
CreditDays as '账期天数'
from View_Company where ComId in (
select distinct ComId from View_Order where BranchId={9} and IsDelete=0 and RawOrderId=0 and summoney <>0 and  PayStatus <> '已付款'
and StoreFinishTime>'2014-1-1' and StoreFinishTime <'{0}') 
    order by 应收总额 desc",
                       dates[0].ToShortDateString(), dates[1].ToShortDateString(), dates[2].ToShortDateString(), dates[3].ToShortDateString(), dates[4].ToShortDateString(),
                       dates[1].Month, dates[2].Month, dates[3].Month, dates[4].Month,option.BranchId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 应收账款报表--区间数据 2014-10-10 lihailong  效率较低，待改进
        /// ERP FAR.cs 中调用
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadARReport(ReportOption option)
        {
            //--QC	起初总数=qichuxiaoshou-qichushouhui	    XS销售qijianxiaoshou		SH收款       QJXZ期间销账 	 QM期末金额=QC-qijianxiaoshou+SH
            //QC期初数	XS销售	SH收回	QM期末余额
            string sql = string.Format(@" select ComId,Name,SalesId, SalesName,ServiceId, ServiceName, 0 as QC,
(select SUM(SumMoney) from dbo.View_Order o
    where  o.ComId=c.ComId and  IsDelete=0 and RawOrderId=0 and o.StoreFinishTime  >='2014-01-01' and StoreFinishTime<'{1}' ) as 'qichuxiaoshou'
,(select SUM(chargeoff) from View_OrderReceiveMoney  rm 
    where ComId=c.ComId and ReceiveDate >='2014-01-01' and ReceiveDate<'{1}' and BranchId={0} and PayStatus <> '' ) as 'qichuxiaozhang'
,(select SUM(SumMoney) from View_Order o 
    where o.ComId=c.ComId and  IsDelete=0 and RawOrderId=0   and StoreFinishTime >='{1}' and StoreFinishTime<'{2}' and BranchId={0} ) as 'qijianxiaoshou'
,(select SUM(chargeoff) from View_OrderReceiveMoney rm 
    where rm.ComId=c.ComId and ReceiveDate >='{1}' and ReceiveDate<'{2}' and BranchId={0} ) as 'qijianxiaozhang'
,(select SUM(ReceiveMoney) from View_OrderReceiveMoney rm 
    where rm.ComId=c.ComId and ReceiveDate >='{1}' and ReceiveDate<'{2}' and BranchId={0} ) as 'qijianshoukuan'
,(select SUM(Chargeoff) from View_OrderReceiveMoney  rm 
    where rm.ComId=c.ComId and ReceiveDate >='{1}' and ReceiveDate<'{2}' and BranchId={0} ) as 'QJXZ'
,
(select SUM(InvoiceMoney) from OrderStatement where ComId = c.ComId and  PayStatus <> '已付款') InvoiceMoney ,
(select SUM(NeedToInvoice) from OrderStatement where ComId = c.ComId and  PayStatus <> '已付款') NeedToInvoice 
from view_company c
    where c.ComId in
    (
select distinct ComId from View_Order where 1=1  and  IsDelete=0 and RawOrderId=0 and  StoreFinishTime<'{2}'   and BranchId={0}
union all
select distinct comId from View_OrderReceiveMoney where BranchId={0} and ReceiveDate <'{2}'
    ) order by  qichuxiaoshou desc;", option.BranchId, option.StartDate.ToShortDateString(), option.EndDate.AddDays(1).ToShortDateString());
            return m_dbo.GetDataSet(sql);
        }
        #endregion 应收应付

        #region 销售报表
        /// <summary>
        /// 销售报表--按照公司统计
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadSalesReportByCompany(ReportOption option)
        {
            string sql = @" select ComId,Company,Count(OrderId) as OrderCount, OrderSalesName,OrderSalesId,ServiceName,ServiceId, 
                        SUM(SumMoney) as SumMoney, SUM(SumMoney/(1+TaxRate)) as unTaxSumMoney,SUM(GrossProfit) as GrossProfit,SUM(TaxGrossProfit) as TaxGrossProfit from View_Order where RawOrderId=0 and  IsDelete=0 ";//top 10 
            switch (option.DateType.ToString())
            {
                //下单日期 = 1, 订单日期 = 2, 采购完成 = 3, 出库日期 = 4, 送货日期
                case "出库日期":
                    sql += string.Format(" and StoreFinishTime>='{0}' and StoreFinishTime<'{1}' ", option.StartDate, option.EndDate.AddDays(1).Date);
                    break;
                case "订单日期":
                    sql += string.Format(" and PlanDate>='{0}' and PlanDate<'{1}' ", option.StartDate, option.EndDate.AddDays(1).Date);
                    break;
                case "送货日期":
                    sql += string.Format(" and DeliveryFinishTime>='{0}' and DeliveryFinishTime<'{1}' ", option.StartDate, option.EndDate.AddDays(1).Date);
                    break;
                case "下单日期":
                    sql += string.Format(" and OrderTime>='{0}' and OrderTime<'{1}' ", option.StartDate, option.EndDate.AddDays(1).Date);
                    break;
                default:
                    break;
            }
            if (option.BranchId != 0)
            {
                sql += string.Format(" and BranchId={0}", option.BranchId);
            }
            if (option.SalesId > 0)
            {
                sql += string.Format(" and OrderSalesId={0} ", option.SalesId);
            }
            sql += "group by ComId,Company,OrderSalesName,OrderSalesId ,ServiceName,ServiceId  order by SumMoney desc ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 客户毛利销售额统计 全年
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadSalesReportByCom(ReportOption option)
        {
            string sql = @" select  ComId,Company,Count(OrderId) as OrderCount, OrderSalesName,OrderSalesId,ServiceName,ServiceId, 
                        SUM(SumMoney) as SumMoney,SUM(GrossProfit) as GrossProfit, convert(varchar(7),StoreFinishTime,120)as Month from View_Order where RawOrderId=0 and  IsDelete=0  ";//top 10 
            switch (option.DateType.ToString())
            {
                //下单日期 = 1, 订单日期 = 2, 采购完成 = 3, 出库日期 = 4, 送货日期
                case "出库日期":
                    sql += string.Format(" and StoreFinishTime>='{0}' and StoreFinishTime<'{1}' ", option.StartDate, option.EndDate.AddDays(1).Date);
                    break;
                case "订单日期":
                    sql += string.Format(" and PlanDate>='{0}' and PlanDate<'{1}' ", option.StartDate, option.EndDate.AddDays(1).Date);
                    break;
                case "送货日期":
                    sql += string.Format(" and DeliveryFinishTime>='{0}' and DeliveryFinishTime<'{1}' ", option.StartDate, option.EndDate.AddDays(1).Date);
                    break;
                case "下单日期":
                    sql += string.Format(" and OrderTime>='{0}' and OrderTime<'{1}' ", option.StartDate, option.EndDate.AddDays(1).Date);
                    break;
                default:
                    break;
            }
            if (option.BranchId != 0)
            {
                sql += string.Format(" and BranchId={0}", option.BranchId);
            }
            if (option.SalesId > 0)
            {
                sql += string.Format(" and OrderSalesId={0} ", option.SalesId);
            }
            if (option.ComId > 0)
            {
                sql += string.Format(" and ComId={0} ", option.ComId);
            }
            sql += "group by ComId,Company,OrderSalesName,OrderSalesId ,ServiceName,ServiceId ,convert(varchar(7),StoreFinishTime,120) order by SumMoney desc ";
            return m_dbo.GetDataSet(sql);
        }


        /// <summary>
        /// 读取销售 汇总表
        ///  调用：ERP：FSalesReport.cs
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadSalesReport(ReportOption option)
        {
            string sql = @" select count(distinct ComId) as ComCount,Count(OrderId) as OrderCount, 
                            OrderSalesName , SUM(SumMoney) as SumMoney, SUM(SumMoney/(1+TaxRate)) as unTaxSumMoney,SUM(GrossProfit) as GrossProfit,SUM(TaxGrossProfit) as TaxGrossProfit
                        from View_Order where  RawOrderId=0 and  IsDelete=0  ";
            switch (option.DateType.ToString())
            {
                //下单日期 = 1, 订单日期 = 2, 采购完成 = 3, 出库日期 = 4, 送货日期
                case "出库日期":
                    sql += string.Format(" and StoreFinishTime>='{0}' and StoreFinishTime<'{1}' ", option.StartDate, option.EndDate.AddDays(1).Date);
                    break;
                case "订单日期":
                    sql += string.Format(" and PlanDate>='{0}' and PlanDate<'{1}' ", option.StartDate, option.EndDate.AddDays(1).Date);
                    break;
                case "送货日期":
                    sql += string.Format(" and DeliveryFinishTime>='{0}' and DeliveryFinishTime<'{1}' ", option.StartDate, option.EndDate.AddDays(1).Date);
                    break;
                case "下单日期":
                    sql += string.Format(" and OrderTime>='{0}' and OrderTime<'{1}' ", option.StartDate, option.EndDate.AddDays(1).Date);
                    break;
                default:
                    break;
            }
            if (option.BranchId != 0)
            {
                sql += string.Format(" and BranchId={0}", option.BranchId);
            }
            if (option.SalesId > 0)
            {
                sql += string.Format(" and OrderSalesId={0} ", option.SalesId);
            }
            sql += "group by OrderSalesName order by SumMoney desc ";
            return m_dbo.GetDataSet(sql);
        }
        #endregion 销售报表

        #region 采购报表
        public DataSet ReadPurchaseReport(ReportOption option)
        {
            //tables[0]：月采购数据汇总
            //tables[1]：现金采购数据汇总
            //供应商采购汇总数据
            //信用卡刷卡统计
            //每月采购商品的比例
            //新增供应商
            //采购新品数量
            string sql = string.Format(@"
                select purchaseType as 采购类型, Sum(SumMoney) as '金额',COUNT(summoney) as 次数,COUNT(distinct Company) as 供应商家数 
            from view_purchase where BranchId={0} and  PurchaseDate >='{1}' and PurchaseDate <'{2}' and SupplierId <> 94 and SupplierId <> 322
            group by PurchaseType order by 金额 desc;
                select PurchaseUserName as 采购员, SUM(summoney) as 金额,COUNT(summoney) as 次数 
            from View_Purchase where BranchId={0} and  PurchaseDate >='{1}' and PurchaseDate <'{2}' and SupplierId <> 94 and SupplierId <> 322  and PurchaseType like '现金%'
            group by PurchaseUserName order by 金额 desc;
                select SupplierId as 供应商编号,Company as 供应商名称,ShortName as 供应商简称,Sum(SumMoney) as 采购金额
            from view_Purchase 
            where BranchId={0} and  PurchaseDate >='{1}' and PurchaseDate <'{2}'  and SupplierId <>94 and SupplierId<>322
            group by SupplierId,Company,ShortName order by 采购金额 desc;
                select Company,SUM(summoney) as 金额,COUNT(summoney) as 次数 
            from View_Purchase where BranchId={0} and  PurchaseDate >='{1}' and PurchaseDate <'{2}' and Company like '%信用卡%'
            group by Company order by 金额 desc;
                select gt.TypeName as 商品类型,gt.Code,0 as 金额 from GoodsType gt where gt.Level=1 order by gt.Code;
                select ID as 供应商编号,Company as 供应商名称,ShortName as 供应商简称,AddTime as 添加时间,Major as 主营业务
            from Supplier where  BranchId={0} and AddTime >='{1}' and AddTime <'{2}' and Company not like '%信用卡%' order by ID;
                select count(distinct GoodsId) as 新品数量 from View_PurchaseDetail
            where GoodsId in ( select Id from Goods where goods.AddTime >='{1}' and goods.AddTime <'{2}');
                select left(code,2) as code,SUM(Amount) as 金额 from View_PurchaseDetail vpd  
            where BranchId={0} and  PurchaseDate >='{1}' and PurchaseDate <'{2}' group by LEFT(code,2);", option.BranchId, option.StartDate.ToShortDateString(), option.EndDate.AddDays(1).ToShortDateString());
            return m_dbo.GetDataSet(sql);
        }
        #endregion

        #region 下单统计--客服
        /// <summary>
        /// 调用：ERP FOrderReport.cs
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadAddOrderReport(ReportOption option)
        {
            string sql = string.Format(@"select UserName as '客服',ordertype as '类型',COUNT(OrderId) as '下单数量',sum(RowNum) as '下单行数', Sum(SumMoney) as '下单金额'
from View_Order
where BranchId={0} and RawOrderId=0 and IsDelete=0
and OrderTime between '{1}' and '{2}'
group by UserName,ordertype order by '客服','类型' desc", 
                    option.BranchId, option.StartDate.ToShortDateString(), option.EndDate.AddDays(1).ToShortDateString());
            return m_dbo.GetDataSet(sql);
        }
        #endregion

        #region 配送
        public DataSet ReadDeliveryWorkReport(ReportOption option)
        {

            string sql = string.Format(@"
 select DeliveryUserId as 送货员编号,DeliveryName as 送货员姓名,COUNT(OrderId ) as 配送订单量,SUM(PackageNum)as 配送包裹量,SUM(SumMoney )as 配送总金额 from View_OrderDelivery where DeliveryFinishTime >='{1}' and  DeliveryFinishTime <'{2}'  and BranchId ={0} ", option.BranchId, option.StartDate.ToShortDateString(), option.EndDate.AddDays(1).ToShortDateString());
            if (option.SalesId != 0)
            {
                sql += string.Format("and DeliveryUserId = {0}", option.SalesId);
            }
            sql += string.Format("group by DeliveryUserId,DeliveryName ; ");

           sql += string.Format(@"
 select CarUserId  as 送货员编号,CarUserName as 送货员姓名,COUNT(DISTINCT (OrderId)) as 配送订单量,COUNT(OrderBoxId) as 配送包裹量,(select SUM(od.SumMoney) from [Order] od where Id in (select OrderId  from View_OrderBoxDelivery vob where vob.CarUserId=vo.CarUserId and DeliveryFinishTime>='{1}' and DeliveryFinishTime<'{2}' )) as 配送总金额
  from View_OrderBoxDelivery vo where BranchId ={0} and DeliveryFinishTime>='{1}' and DeliveryFinishTime<'{2}'  ", option.BranchId, option.StartDate.ToShortDateString(), option.EndDate.AddDays(1).ToShortDateString());
            if (option.SalesId != 0)
            {
                sql += string.Format("and CarUserId = {0}", option.SalesId);
            }
            sql += string.Format("group by CarUserId ,CarUserName");
            return m_dbo.GetDataSet(sql);
        }

        #endregion

        #region 仓库
        /// <summary>
        /// 仓库工作量统计，退单数量统计
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadStoreWorkReport(ReportOption option)
        {
            string sql = string.Format(@"select PickingName as 姓名,COUNT(orderid) as 拣货订单数量, 
SUM(Summoney) as 拣货总金额,SUM(RowNum) as 拣货行数,SUM(PackageNum) as 拣货件数 
from View_OrderPicking
where BranchId={0} and StoreFinishTime>='{1}' and StoreFinishTime<'{2}'", option.BranchId, option.StartDate.ToShortDateString(), option.EndDate.AddDays(1).ToShortDateString());
            if (option.SalesId != 0)
            {
                sql += string.Format("and PickingUserId = {0}", option.SalesId);
            }
            sql += string.Format("group by PickingName order by 姓名 desc;");
            sql += string.Format(@"select PackageName as 打包员,COUNT(orderid) as 打包订单数量, 
SUM(Summoney) as 打包总金额,SUM(RowNum) as 打包行数,SUM(PackageNum) as 打包件数 
from View_OrderPicking
where BranchId={0} and StoreFinishTime>='{1}' and StoreFinishTime<'{2}'
", option.BranchId, option.StartDate.ToShortDateString(), option.EndDate.AddDays(1).ToShortDateString());
            if (option.SalesId != 0)
            {
                sql += string.Format("and PackageUserId = {0}", option.SalesId);
            }
            sql += ("group by PackageName order by 打包员 desc;");

            sql += string.Format(@"select COUNT(OrderId) as 退单数量, SUM(SumMoney) as 退货金额 from View_Order 
        where SumMoney<0 and BranchId={0} and StoreFinishTime>='{1}' and StoreFinishTime<'{2}' ;
", option.BranchId, option.StartDate.ToShortDateString(), option.EndDate.AddDays(1).ToShortDateString());
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取库存总数变化记录
        /// </summary>
        /// <returns></returns>
        public DataSet ReadStoreReport(ReportOption option)
        {
            string sql = string.Format(@"select StoreDayData.*,store.BranchId,store.Name 
    from StoreDayData inner join Store on StoreDayData.StoreId=store.ID 
    where store.BranchId ={0} and StoreDayData.UpdateTime >'{1}' and StoreDayData.Updatetime <'{2}' ",
    option.BranchId, option.StartDate.ToShortDateString(), option.EndDate.AddDays(1).ToShortDateString());
            if (option.StoreId > 0)
            {
                sql += string.Format(" and StoreId = {0} ", option.StoreId);
            }
            sql += " order by StoreId,UpdateTime desc ";
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取库存明细
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadStoreDetail(ReportOption option)
        {
            string sql = string.Format(@"select BranchId,StoreId,TypeId,TypeName,Code,GoodsId,DisplayName,Num,Unit,AC,StoreZone  
    from View_GoodsStore where BranchId={0} and StoreZone<>'收货区'", option.BranchId);
            if(option.StoreId >0)
            {
                sql += string.Format(" and StoreId={0} ",option.StoreId);
            }
            if (option.TypeId > 0)
            {
                GoodsType gt = new GoodsType();
                gt.Id = option.TypeId;
                gt.Load();
                sql += string.Format(" and Code like '{0}%'  ", gt.Code);
            }
            if (option.KeyWords != "")
            {
                sql += string.Format(" and DisplayName like '%{0}%' ", option.KeyWords);
            }
            if (option.SalesId != 0)//重用了该字段：只显示库存不为零的商品
            {
                sql += " and Num <> 0 ";
            }
            if (option.GoodsStoreID != 0) {
                sql += string.Format(" and Id={0} ", option.GoodsStoreID);
            } 
            if (option.GoodsID != 0)
            {
                sql += string.Format(" and GoodsId={0} ", option.GoodsID);
            }
            sql += "order by Code ";

            return m_dbo.GetDataSet(sql);
        }


        /// <summary>
        /// 读取待盘点报表
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadStoreDetailInventory(ReportOption option)
        {
            string sql = string.Format(@"select Id as '库存ID',StoreId as '仓库',StoreZone as '货架编号',GoodsId as '商品编号',TypeName as '商品类别',DisplayName as '商品名称',Num as '当前库存量',Unit as '单位',0 as'盘点数量'  from View_GoodsStore where BranchId={0} and StoreZone<>'收货区'", option.BranchId);
            if (option.StoreId > 0)
            {
                sql += string.Format(" and StoreId={0} ", option.StoreId);
            }
            if (option.TypeId > 0)
            {
                GoodsType gt = new GoodsType();
                gt.Id = option.TypeId;
                gt.Load();
                sql += string.Format(" and Code like '{0}%'  ", gt.Code);
            }
            if (option.KeyWords != "")
            {
                sql += string.Format(" and DisplayName like '%{0}%' ", option.KeyWords);
            }
            if (option.SalesId != 0)//重用了该字段：只显示库存不为零的商品
            {
                sql += " and Num <> 0 ";
            }
            sql += " order by StoreZone desc ";

            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 分类统计库存量
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadStoreByType(ReportOption option)
        {
            string sql = string.Format(@"select Name, Code,TypeName,sum(Num*AC  ) as Amount, sum(Num * TaxAc) as TaxAmount
        from View_GoodsStore where BranchId={0} ",option.BranchId);
            if(option.StoreId>0)
            {
                sql += string.Format(" and StoreId={0} ",option.StoreId);
            }
            sql += " group by Name, Code,TypeName  order by Code ";
            return m_dbo.GetDataSet(sql);
        }

        #endregion

        #region 采购报表 add by luochunhui 2015-3-5

        /// <summary>
        /// 获取各部门的总费用（第一,四张图）
        /// </summary>
        /// <returns></returns>
        public DataSet GetAmount(string[] fn)
        {
            string sql = "";            
            for (int i = 0; i < fn.Length;i++ )
            {
                string[] a = fn[i].Split(',');
                sql += string.Format("select '{0}' as DeptName,",a[0]);
                if (reportOption.IsMonthReport == 1)//按部门按月份分组
                {
                    sql += "year(OrderTime) as OrderYear,MONTH(OrderTime) as OrderMonth,";
                }
                sql +=string.Format( "ISNULL(SUM(SumMoney),0) as Amount from [Order] a inner join Dept d on a.DeptId=d.Id where 1=1 and Code like '{0}%'",a[1]);
                if (reportOption.ComId > 0)
                {
                    sql += string.Format(" and a.ComId={0} ", reportOption.ComId);
                }
                if (reportOption.StartDate > new DateTime(1900, 1, 1) && reportOption.EndDate > new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and  OrderTime  between '{0}' and '{1}' ", reportOption.StartDate.ToShortDateString(), reportOption.EndDate.AddDays(1).ToShortDateString());
                }
                sql += " and RawOrderId=0 and IsDelete=0";
                if (reportOption.IsMonthReport == 1)
                {
                    sql += " group by YEAR(OrderTime) ,MONTH(OrderTime)";
                }
                if (i < fn.Length-1) {
                    sql += " union  ";
                }
            }
            if (reportOption.IsMonthReport == 1)
            {
                sql += string.Format(" order by DeptName, YEAR( OrderTime) ,MONTH(OrderTime);");
            }
            else
            {
                sql += string.Format(" order by Amount desc");
            }
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取各个类别的采购数量（第二张图）
        /// </summary>
        /// <returns></returns>
        public DataSet ReadOrdersGroupbyType()
        {
            string sql = string.Format(@"select SUM(Amount) as Amount,
(select TypeName  from dbo.GoodsType t where t.Code = SUBSTRING(od.Code,1,2)) as DeptName
  from dbo.View_OrderDetail od
where 1=1  ");
            if (reportOption.MemberId > 0)
            {
                sql += string.Format(" and MemberId={0} ", reportOption.MemberId);
            }
            if (reportOption.DeptId != null)
            {
                sql += " and DeptId in ( ";
                for (int i = 0; i < reportOption.DeptId.Length; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", reportOption.DeptId[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", reportOption.DeptId[i]);
                }
                sql += " ) ";
            }
            if (reportOption.ComId > 0)
            {
                sql += string.Format(" and ComId={0} ", reportOption.ComId);
            }
            if (reportOption.StartDate > new DateTime(1900, 1, 1) && reportOption.EndDate > new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and  OrderTime  between '{0}' and '{1}' ", reportOption.StartDate.ToShortDateString(), reportOption.EndDate.AddDays(1).ToShortDateString());
            }
            sql += string.Format(" and RawOrderId=0 and IsDelete=0 group by SUBSTRING( Code,1,2) order by Amount desc");
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 公司各月份的采购汇总（第三张图）
        /// </summary>
        /// <returns></returns>
        public DataSet ReadOrderGroupbyMonths()
        {
            string sql = string.Format(@"select  year(OrderTime) as OrderYear,MONTH(OrderTime)  as OrderMonth,SUM(SumMoney) as OrderSumMoney 
from dbo.[Order] where 1=1  ");
            if (reportOption.ComId > 0)
            {
                sql += string.Format(" and comId ={0} ", reportOption.ComId);
            }
            if (reportOption.DeptId != null)
            {
                sql += " and DeptId in ( ";
                for (int i = 0; i < reportOption.DeptId.Length; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", reportOption.DeptId[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", reportOption.DeptId[i]);
                }
                sql += " ) ";
            }
            if (reportOption.MemberId > 0)
            {
                sql += string.Format(" and MemberId={0} ", reportOption.MemberId);
            }
            if (reportOption.StartDate > new DateTime(1900, 1, 1) && reportOption.EndDate > new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and  OrderTime  between '{0}' and '{1}' ", reportOption.StartDate.ToShortDateString(), reportOption.EndDate.AddDays(1).ToShortDateString());
            }
            sql += string.Format(@" and RawOrderId=0 and IsDelete=0 group by year(OrderTime), MONTH(OrderTime)
order by YEAR( OrderTime) ,MONTH(OrderTime) ");

            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取商品明细汇总
        /// </summary>
        /// <returns></returns>
        public DataSet ReadOrderGoodsDetailGather()
        {
            string sql = "select ";
            if (reportOption.pageModel.PageSize > 0)
            {
                sql += string.Format(" top {0} ", reportOption.pageModel.PageSize);
            }

            sql += string.Format(@" GoodsId,DisplayName,dbo.OrderDetail.Model,SalePrice,SUM( Num) as Num,Unit,SUM(Amount) as Amount
 from dbo.[Order] left join dbo.OrderDetail on dbo.[Order].Id=dbo.OrderDetail.OrderId join dbo.Goods on dbo.Goods.ID=dbo.OrderDetail.GoodsId
 where 1=1 ");

            if (reportOption.ComId > 0)
            {
                sql += string.Format(" and comId ={0} ", reportOption.ComId);
            }
            if (reportOption.MemberId > 0)
            {
                sql += string.Format(" and MemberId={0} ", reportOption.MemberId);
            }
            if (reportOption.StartDate > new DateTime(1900, 1, 1) && reportOption.EndDate > new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and  OrderTime  between '{0}' and '{1}' ", reportOption.StartDate.ToShortDateString(), reportOption.EndDate.AddDays(1).ToShortDateString());
            }
            sql += string.Format(@" and RawOrderId=0 and IsDelete=0 group by GoodsId,DisplayName,dbo.OrderDetail.Model,SalePrice,Unit
order by Amount desc  ");
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取订单明细 
        /// </summary>
        /// <returns></returns>
        public DataSet ReadOrderGoodsDetail(PageModel pageModel, int CounterId)
        {
            string sql = string.Format(@"select DeptName,OrderId,OrderDetail.GoodsId,DisplayName,VIPCounterDetail.Remark , dbo.OrderDetail.Model,Unit,SalePrice,Num,Amount
 from dbo.[Order] left join dbo.OrderDetail on dbo.[Order].Id=dbo.OrderDetail.OrderId join dbo.Goods on dbo.Goods.ID=dbo.OrderDetail.GoodsId left join VIPCounterDetail on Goods.ID=VIPCounterDetail.GoodsId  and CounterId={0} join dbo.Member on dbo.[Order].MemberId=dbo.Member.Id where 1=1",CounterId);

            if (reportOption.ComId > 0)
            {
                sql += string.Format(" and dbo.[Order].ComId ={0} ", reportOption.ComId);
            }
            if (reportOption.MemberId > 0)
            {
                sql += string.Format(" and MemberId={0} ", reportOption.MemberId);
            }
            if (reportOption.DeptId != null)
            {
                sql += string.Format("  and dbo.Member.DeptId={0} ", reportOption.DeptId);
            }
            if (reportOption.StartDate > new DateTime(1900, 1, 1) && reportOption.EndDate > new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and  OrderTime  between '{0}' and '{1}' ", reportOption.StartDate.ToShortDateString(), reportOption.EndDate.AddDays(1).ToShortDateString());
            }
            sql += string.Format(@" order by OrderId  desc ");
            DataSet ds = m_dbo.GetDataSet(sql, (pageModel.CurrentPage - 1) * pageModel.PageSize, pageModel.PageSize);

            return ds;
        }

        public DataSet ReadAllTableDataToDownload()
        {
            string sql = string.Format(@"select DeptId,DeptName as 部门名称 ,SUM(SumMoney) as 金额 
    from dbo.[Order] 
    where ComId={0} and  OrderTime  between '{1}' and '{2}' and IsDelete=0  and RawOrderId=0
    group by DeptId,DeptName
    order by 金额 desc;
select(select TypeName  from dbo.GoodsType t where t.Code = SUBSTRING(od.Code,1,2)) as 商品类别,SUM(Amount) as 金额 
    from dbo.View_OrderDetail od 
    where ComId={0}  and  OrderTime  between '{1}' and '{2}' and IsDelete=0 and RawOrderId=0
    group by SUBSTRING( Code,1,2) 
    order by 金额 desc;
select  year(OrderTime) as 年,MONTH(OrderTime)  as 月份,SUM(SumMoney) as 金额 
    from dbo.[Order] 
    where comId ={0}  and  OrderTime  between '{1}' and '{2}'and IsDelete=0  and RawOrderId=0
    group by year(OrderTime),MONTH(OrderTime)
    order by YEAR( OrderTime) ,MONTH(OrderTime);
select DeptName as 部门名称 ,year(OrderTime) as 年,MONTH(OrderTime)  as 月,SUM(SumMoney) as 金额 
    from dbo.[Order] 
    where comId ={0}  and  OrderTime  between '{1}' and '{2}' and IsDelete=0  and RawOrderId=0
    group by DeptName ,year(OrderTime), MONTH(OrderTime) 
    order by DeptName ,YEAR( OrderTime) ,MONTH(OrderTime);
select GoodsId as 商品编号,DisplayName as 商品名称,dbo.OrderDetail.Model as 规格,Unit as 单位,SalePrice as 单价,SUM( Num) as 数量,SUM(Amount) as 金额 
    from dbo.[Order] left join dbo.OrderDetail on dbo.[Order].Id=dbo.OrderDetail.OrderId join dbo.Goods on dbo.Goods.ID=dbo.OrderDetail.GoodsId 
    where comId ={0}  and  OrderTime  between '{1}' and '{2}' and IsDelete=0 and RawOrderId=0
    group by GoodsId,DisplayName,dbo.OrderDetail.Model,SalePrice,Unit 
    order by 金额 desc;"
                , reportOption.ComId, reportOption.StartDate, reportOption.EndDate);
            return m_dbo.GetDataSet(sql);
        }
        #endregion
    }
}
    