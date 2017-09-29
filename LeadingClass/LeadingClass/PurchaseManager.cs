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
    public class NeedToPurchaseOption
    {
        private int m_NotPrint;
        private DateTime m_StartDate;
        private DateTime m_EndDate;
        private int m_Status;
        private int m_BranchId;

        public int NotPrint { get { return m_NotPrint; } set { m_NotPrint = value; } }
        public DateTime StartDate { get { return m_StartDate; } set { m_StartDate = value; } }
        public DateTime EndDate { get { return m_EndDate; } set { m_EndDate = value; } }
        public int Status { get { return m_Status; } set { m_Status = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public NeedToPurchaseOption()
        {
            m_NotPrint = 1;
            m_StartDate = DateTime.Now;
            m_EndDate = DateTime.Now;
            m_Status = 0;
            m_BranchId = 1;
        }
    }

    public class PurchaseOption
    {
        private DateTime m_StartDate;
        private DateTime m_EndDate;
        private int m_PurchaseId;
        private int m_SupplierId;
        private int m_BranchId;
        private int m_StoreId;
        private int m_UserId;
        private int m_PurchaseUserId;
        private int m_GoodsId;
        private int m_IsPaid;
        private int m_IsConfirm;
        private int m_IsCashConfirm;
        private int m_PurchaseStatementId;
        private int m_StartId;
        private int m_EndId;
        private string m_PurchaseType;
        private string m_Company;
        private string m_PurchaseStatus;
        private string m_Memo;

        public DateTime StartDate { get { return m_StartDate; } set { m_StartDate = value; } }
        public DateTime EndDate { get { return m_EndDate; } set { m_EndDate = value; } }
        public int PurchaseId { get { return m_PurchaseId; } set { m_PurchaseId = value; } }
        public int SupplierId { get { return m_SupplierId; } set { m_SupplierId = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public int StoreId { get { return m_StoreId; } set { m_StoreId = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public int PurchaseUserId { get { return m_PurchaseUserId; } set { m_PurchaseUserId = value; } }
        public int GoodsId { get { return m_GoodsId; } set { m_GoodsId = value; } }
        public int IsPaid { get { return m_IsPaid; } set { m_IsPaid = value; } }
        public int IsConfirm { get { return m_IsConfirm; } set { m_IsConfirm = value; } }
        public int IsCashConfirm { get { return m_IsCashConfirm; } set { m_IsCashConfirm = value; } }
        public int PurchaseStatementId { get { return m_PurchaseStatementId; } set { m_PurchaseStatementId = value; } }
        public int StartId { get { return m_StartId; } set { m_StartId = value; } }
        public int EndId { get { return m_EndId; } set { m_EndId = value; } }
        public string PurchaseType { get { return m_PurchaseType; } set { m_PurchaseType = value; } }
        public string Company { get { return m_Company; } set { m_Company = value; } }
        public string PurchaseStatus { get { return m_PurchaseStatus; } set { m_PurchaseStatus = value; } }
        public string Memo { get { return m_Memo; } set { m_Memo = value; } }
        public PurchaseOption()
        {
            m_StartDate = new DateTime(1900, 1, 1);
            m_EndDate = new DateTime(1900, 1, 1);
            m_PurchaseId = 0;
            m_SupplierId = 0;
            m_BranchId = 0;
            m_StoreId = 0;
            m_UserId = 0;
            m_PurchaseUserId = 0;
            m_GoodsId = 0;
            m_IsPaid = -1;
            m_IsConfirm = -1;
            m_IsCashConfirm = -1;
            m_PurchaseStatementId = 0;
            m_PurchaseType = "";
            m_EndId = 0;
            m_StartId = 0;
            m_Company = "";
            m_PurchaseStatus = "";
            m_Memo = "";
        }

    }

    public class PurchaseManager
    {
        private DBOperate m_dbo;
        public PurchaseManager()
        {
            m_dbo = new DBOperate();

        }
        /// <summary>
        /// 保存待采订单，将状态设置为已接受
        /// </summary>
        /// <param name="orderIds"></param>
        /// <param name="needtopurchase"></param>
        /// <returns></returns>
        public int SaveNeedToPurchase(int[] orderIds, NeedToPurchase needtopurchase)
        {
            NeedToPurchase ntp = new NeedToPurchase();
            ntp.Id = needtopurchase.Id;
            ntp.Memo = needtopurchase.Memo;
            ntp.UserId = needtopurchase.UserId;
            ntp.UpdateTime = DateTime.Now;
            ntp.Status = 0;
            ntp.BranchId = needtopurchase.BranchId;
            int ntpId = ntp.Save();
            try
            {
                if (ntpId > 0)
                {
                    OrderStatus os = new OrderStatus();
                    for (int i = 0; i < orderIds.Length; i++)
                    {
                        NeedToPurchaseOrder ntpo = new NeedToPurchaseOrder();
                        ntpo.NeedToPurchaseId = ntpId;
                        ntpo.OrderId = orderIds[i];
                        if (ntpo.Save() > 0)
                        {
                            os.LoadFromOrderId(orderIds[i]);
                            os.UpdateStatus(CommenClass.OrderStatusType.采购, CommenClass.Order_Status.已接受, needtopurchase.UserId);
                        }
                    }

                    //保存采购单汇总明细表
                    DataSet ds = ReadOrderDetailNeedToPurchase(ntpId, needtopurchase.BranchId);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        NeedToPurchaseDetail ntpd = new NeedToPurchaseDetail();
                        ntpd.GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                        //配货库存+备货库存-预售数量
                        ntpd.LackNum = DBTool.GetIntFromRow(row, "Num1", 0) + DBTool.GetIntFromRow(row, "Num2", 0) - DBTool.GetIntFromRow(row, "ReSale", 0);
                        //ntpd.Model = DBTool.GetStringFromRow(row, "Model", "");
                        ntpd.NeedToPurchaseId = ntpId;
                        ntpd.Num = DBTool.GetIntFromRow(row, "Num", 0);
                        ntpd.UpdateTime = DateTime.Now;
                        ntpd.Emergency = DBTool.GetIntFromRow(row, "Emergency", 0);
                        ntpd.GoodsMemo = DBTool.GetStringFromRow(row, "PurchaseMemo", "");
                        ntpd.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                Sys_ErrorLog slog = new Sys_ErrorLog();
                slog.TableName = "NeedToPurchaseDetail";
                slog.ErrorMessage = ex.Message;
                slog.Save();
            }

            return ntpId;

        }

        public int SaveNeedToPurchaseC(int[] orderIds, NeedToPurchase needtopurchase, int storeId)
        {
            NeedToPurchase ntp = new NeedToPurchase();
            ntp.Id = needtopurchase.Id;
            ntp.Memo = needtopurchase.Memo;
            ntp.UserId = needtopurchase.UserId;
            ntp.UpdateTime = DateTime.Now;
            ntp.Status = 0;
            ntp.BranchId = needtopurchase.BranchId;
            int ntpId = ntp.Save();
            try
            {
                if (ntpId > 0)
                {
                    //  OrderStatus os = new OrderStatus();
                    //for (int i = 0; i < orderIds.Length; i++)
                    //{
                    //    NeedToPurchaseOrder ntpo = new NeedToPurchaseOrder();
                    //    ntpo.NeedToPurchaseId = ntpId;
                    //    ntpo.OrderId = orderIds[i];
                    //    if (ntpo.Save() > 0)
                    //    {
                    //        os.LoadFromOrderId(orderIds[i]);
                    //        os.UpdateStatus(CommenClass.OrderStatusType.采购, CommenClass.Order_Status.已接受, needtopurchase.UserId);
                    //    }
                    //}
                    if (SetNeedToPurchaseOrder(ntpId, orderIds))
                    {
                        //保存采购单汇总明细表
                        NeedToPurchaseDetail npd = new NeedToPurchaseDetail();
                        npd.SetNeedToPurchaseDetail(ntpId, needtopurchase.BranchId, storeId);
                    }
                }
            }
            catch (Exception ex)
            {
                Sys_ErrorLog slog = new Sys_ErrorLog();
                slog.TableName = "NeedToPurchaseDetail";
                slog.ErrorMessage = ex.Message;
                slog.Save();
            }

            return ntpId;

        }

        /// <summary>
        /// 删除采购单
        /// </summary>
        /// <param name="Pid">采购单</param>
        /// <param name="UserId">操作人</param>
        /// <returns></returns>
        public string Delete(int Pid, int UserId)
        {
            Purchase p = new Purchase(Pid);
            if (p.PurchaseStatus == CommenClass.PurchaseStatus.未处理.ToString() || p.PurchaseStatus == CommenClass.PurchaseStatus.已确认.ToString())
            {
                PurchaseLog pl = new PurchaseLog();
                pl.Id = 0;
                pl.PurchaseId = Pid;
                pl.StoreId = p.StoreId;
                pl.SumMoney = p.SumMoney;
                pl.PurchaseType = p.PurchaseType;
                pl.UserId = UserId;
                pl.PurchaseStatus = p.PurchaseStatus;
                pl.Memo = JsonHelper.DataTableJson(ReadPurchaseDetail(Pid).Tables[0]);
                pl.UpdateTime = DateTime.Now;
                string sql = string.Format(@"delete from PurchaseDetail where PurchaseId={0};
delete from Purchase where Id={0};", Pid);
                if (m_dbo.ExecuteNonQuery(sql))
                {
                    if (pl.Save() > 0)
                    {
                        return "";
                    }
                }
                return "系统错误！";
            }
            else
            {
                return p.PurchaseStatus;
            }

        }

        /// <summary>
        /// 读取当前未完成的 采购单
        /// </summary>
        /// <returns></returns>
        public DataSet ReadNeedToPurchaseList(NeedToPurchaseOption option)
        {
            string sql = " select * from view_NeedToPurchase where 1=1 ";
            if (option.NotPrint == 1)
            {
                sql += " and PrintNum =0 ";
            }
            else if (option.Status == 1)
            {
                sql += " and Status =0 ";
            }
            else
            {
                sql += " and PrintNum >=0 and  Status >=0 ";
            }
            if (option.BranchId > 0)
            {
                sql += string.Format(" and BranchId = {0} ", option.BranchId);
            }
            sql += string.Format(" and UpdateTime >= '{0}' ", option.StartDate.Date);
            sql += string.Format(" and UpdateTime < '{0}' ", option.EndDate.AddDays(1).Date);
            sql += " order by UpdateTime ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取采购单--订单列表
        /// </summary>
        /// <param name="ntpId"></param>
        /// <returns></returns>
        public DataSet ReadPurchaseOrder(int ntpId)
        {
            string sql = string.Format(@"select a.* ,b.DeptName,b.RealName,c.ShortName,b.Telphone,b.Mobile,b.SumMoney,b.Memo,b.PlanDate,b.OrderTime
                    from NeedToPurchaseOrder a inner join [Order] b on a.OrderId = b.Id inner join Company c on b.ComId =c.Id
                    where a.NeedToPurchaseId = {0} ", ntpId);
            return m_dbo.GetDataSet(sql);

        }
        /// <summary>
        /// 读取采购单的商品明细汇总
        /// </summary>
        /// <param name="ntpId"></param>
        /// <param name="BranchId"></param>
        /// <returns></returns>ReadOrderDetailNeedToPurchase
        public DataSet ReadOrderDetailNeedToPurchase(int ntpId, int BranchId)
        {
            string sql = string.Format(@"select GoodsId,DisplayName,SUM(Num) as Num,purchaseMemo,min(InPrice) as InPrice,Unit,
                                         dbo.GetGoodsStore(GoodsId,1,{0}) as Num1,
                                         dbo.GetGoodsStore(GoodsId,0,{0}) as Num2,
                                         dbo.GetReSaleGoods(GoodsId,{0}) as ReSale,MAX(Emergency) as Emergency
                                         from View_OrderDetailNeedToPurchase where 1=1 and IsCalc=1 and NeedToPurchaseId={1} ", BranchId, ntpId);
            sql += " group by GoodsId,DisplayName,Unit,purchaseMemo order by DisplayName ";
            return m_dbo.GetDataSet(sql);
        }


        /// <summary>
        /// 读取已经保存的 采购单商品明细
        /// </summary>
        /// <param name="ntpId"></param>
        /// <returns></returns>
        //        public DataSet ReadNeedToPurchaseDetail(int ntpId)
        //        {
        //            string sql = string.Format(@"select a.*,b.DisplayName,b.Unit,b.InPrice
        //                                         from needtopurchasedetail a inner join Goods b on a.goodsId = b.ID
        //                                         where NeedToPurchaseId = {0} order by DisplayName ", ntpId);
        //            return m_dbo.GetDataSet(sql);
        //        }
        public DataSet ReadNeedToPurchaseDetail(int ntpId)
        {
            string sql = string.Format(@"select Id as NeedToPurchaseDetailId,NeedToPurchaseId,GoodsId,UserId,(select Name from Sys_Users where Id =UserId) as Name,
                                         CASE SupplierId WHEN 0 THEN (select SupplierId from Goods where ID =ntpd.GoodsId)ELSE SupplierId END as SupplierId ,
                                         CASE SupplierId WHEN 0 THEN  (select ('('+shortname+')' +Company) from Supplier where Id=(select SupplierId from Goods where ID =ntpd.GoodsId))
                                              ELSE (select ('('+shortname+')' +Company) from Supplier where Id= ntpd.SupplierId)  END  as Company,
                                         PurchaseType,InPrice,Num,LackNum,Emergency,UpdateTime,(select Unit from Goods where Id =ntpd.GoodsId) as Unit,
                                        (select DisplayName  from Goods where Id =ntpd.GoodsId) as DisplayName,PurchaseMemo from NeedToPurchaseDetail ntpd where NeedToPurchaseId={0}", ntpId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取已经刷单，但是未入库的商品
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadNotReceiveGoods(PurchaseOption option)
        {
            string sql = string.Format(@"select  GoodsId ,DisplayName ,sum(Num) num,Unit ,
                    (select top 1 Num from View_GoodsStore vgs  where vgs.IsDefault=1 and vgs.BranchId ={0} and vgs.GoodsId =vo.GoodsId and vgs.StoreZone<>'收货区') as StoreNum  
                    from View_OrderDetail vo where vo.StoreStatus <>'已完成' and vo.OrderTime>='{1}' and vo.OrderTime<='{2}' and BranchId ={0} and IsDelete =0   and IsInner =0 and PurchaseMemo not like '%勿采%'  group by GoodsId,DisplayName,Unit  order by GoodsId", 
                    option.BranchId, option.StartDate.ToShortDateString(), option.EndDate.AddDays(1).ToShortDateString());
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 保存采购单
        /// </summary>
        /// <param name="p"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public int SavePurchase(Purchase p, PurchaseDetail[] details)
        {
            Purchase purchase = new Purchase();
            purchase.BranchId = p.BranchId;
            purchase.Id = p.Id;
            purchase.IsPaid = p.IsPaid;
            purchase.Memo = p.Memo;
            purchase.PurchaseDate = p.PurchaseDate;
            purchase.PurchaseType = p.PurchaseType;
            purchase.PurchaseUserId = p.PurchaseUserId;
            purchase.SumMoney = p.SumMoney;
            purchase.SupplierId = p.SupplierId;
            purchase.UpdateTime = DateTime.Now;
            purchase.UserId = p.UserId;
            purchase.IsTax = p.IsTax;
            purchase.StoreId = p.StoreId;
            purchase.IsConfirm = p.IsConfirm;
            purchase.PurchaseStatus = p.PurchaseStatus;
            purchase.ConfirmDate = p.ConfirmDate;
            purchase.Tax = p.Tax;
            purchase.TaxRate = p.TaxRate;
            int pId = purchase.Save();
            if (pId > 0)
            {
                purchase.Id = pId;
             
                for (int i = 0; i < details.Length; i++)
                {
                    try//2014-10-08 添加 try 和 日志记录： by lihailong
                    {
                        SavePurchaseDetail(purchase, details[i]);//保存采购记录明细                       
                    }
                    catch (Exception e)
                    {
                        Sys_ErrorLog errorlog = new Sys_ErrorLog();
                        errorlog.ErrorMessage = e.Message.Substring(0, 1999);
                        errorlog.TableName = "";
                        errorlog.Memo = string.Format("Purchase Id = {0}", purchase.Id);
                        errorlog.Save();
                    }
                }
               
                return pId;
            }
            else return -1;
        }
        private void SavePurchaseDetail(Purchase p, PurchaseDetail pd)
        {
            GoodsStore gs = new GoodsStore();
            gs.StoreId = p.StoreId;
            gs.GoodsId = pd.GoodsId;
            gs.Load(p.StoreId, pd.GoodsId);


            pd.OldAC = gs.AC;
            pd.OldStore = gs.Num;
            pd.PurchaseId = p.Id;
            pd.StoreId = p.StoreId;           
            if (pd.Save() > 0)
            {
                
                if (pd.InPrice != 0 && pd.Num != 0 )
                {
                    if ((gs.Num + pd.Num) != 0)
                    {
                        gs.AC = (gs.AC * gs.Num + pd.Num * pd.InPrice) / (gs.Num + pd.Num);
                        gs.TaxAC = (gs.TaxAC * gs.Num + pd.Num * pd.TaxInPrice) / (gs.Num + pd.Num);
                        
                    }
                    else
                    {
                        gs.AC = pd.InPrice;
                        gs.TaxAC = pd.TaxInPrice;
                    }
                }

                
                gs.Num = gs.Num + pd.Num;
                gs.UpdateTime = DateTime.Now;
                gs.Save();
                //记录商品库存变化明细
                GoodsStoreDetail gsd = new GoodsStoreDetail();
                gsd.GoodsId = pd.GoodsId;
                gsd.Id = 0;
                gsd.NewNum = gs.Num;
                gsd.Num = pd.Num;
                gsd.OldNum = gs.Num - pd.Num;
                gsd.Operate = p.PurchaseType;
                gsd.RelationId = p.Id;
                gsd.StoreId = p.StoreId;
                gsd.UpdateTime = DateTime.Now;
                gsd.UserId = p.UserId;
                gsd.AC = gs.AC;
                gsd.TaxAC = gs.TaxAC;  
                gsd.Save();
            }
        }
        /// <summary>
        /// 修改采购单
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <returns></returns>
        public int ModifyPurchase(Purchase p, PurchaseDetail[] details)
        {
            Purchase purchase = new Purchase();
            purchase.Id = p.Id;
            purchase.Load();
            purchase.BranchId = p.BranchId;
            purchase.IsConfirm = p.IsConfirm;
            purchase.IsPaid = p.IsPaid;
            purchase.IsTax = p.IsTax;
            purchase.Memo = p.Memo;
            purchase.PurchaseDate = p.PurchaseDate;
            purchase.PurchaseType = p.PurchaseType;
            purchase.PurchaseUserId = p.PurchaseUserId;
            purchase.SumMoney = p.SumMoney;
            purchase.SupplierId = p.SupplierId;
            purchase.UpdateTime = DateTime.Now;
            purchase.UserId = p.UserId;
            purchase.StoreId = p.StoreId;
            purchase.Tax = p.Tax;
            purchase.TaxRate = p.TaxRate;
            int pId = purchase.Save();
            if (pId > 0)
            {
                //读取订单明细               
                DataTable dtOld = ReadPurchaseDetail(p.Id).Tables[0];

                //循环新表，处理新增商品和 修改过数量的商品
                for (int i = 0; i < details.Length; i++)
                {
                    DataRow[] rows = dtOld.Select(string.Format(" GoodsId={0} ", details[i].GoodsId));
                    if (rows.Length == 1)//有这个商品
                    {
                        int oldNum = DBTool.GetIntFromRow(rows[0], "Num", 0);
                        double oldPrice = DBTool.GetDoubleFromRow(rows[0], "InPrice", 0);
                        double oldAmount = DBTool.GetDoubleFromRow(rows[0], "Amount", 0);
                        double oldTaxInPrice = DBTool.GetDoubleFromRow(rows[0], "TaxInPrice", 0);
                        if (details[i].Num != oldNum || details[i].InPrice != oldPrice || details[i].Amount != oldAmount || details[i].TaxInPrice != oldTaxInPrice)//有变化需要修改
                        {
                            GoodsStore gs = new GoodsStore();
                            gs.Load(purchase.StoreId, details[i].GoodsId);

                            PurchaseDetail pd = new PurchaseDetail();
                            pd.Id = DBTool.GetIntFromRow(rows[0], "Id", 0);
                            pd.Load();
                            pd.BillsPrice = details[i].BillsPrice;
                            pd.InPrice = details[i].InPrice;
                            pd.Num = details[i].Num;
                            pd.PurchaseId = pId;
                            pd.Amount = details[i].Amount;
                            pd.OldAC = gs.AC;
                            pd.OldStore = gs.Num;
                            pd.StoreId=purchase.StoreId;
                            pd.TaxInPrice = details[i].TaxInPrice;           
                            if (pd.Save() > 0)
                            {

                                //记录修改明细记录
                                details[i].PurchaseId = pId;
                                SavePurchaseModify(details[i], oldNum, oldPrice, purchase.UserId);

                                //修改库存 成本和数量,如果是固定成本的 直接赋值
                                int newNum = gs.Num - oldNum + pd.Num;
                                double oldAC = 0;
                                double oldTaxAC = 0;

                                if (oldPrice <= 0) {
                                    oldAC = gs.AC;
                                    oldTaxAC = gs.TaxAC;
                                }
                                else 
                                {
                                    if (gs.Num - oldNum <= 0)
                                    {
                                        oldAC = oldPrice;
                                        oldTaxAC = oldTaxInPrice;
                                    }
                                    else {
                                        oldAC = (gs.AC * gs.Num - oldNum * oldPrice) / (gs.Num - oldNum);
                                        oldTaxAC = (gs.TaxAC * gs.Num - oldNum * oldTaxInPrice) / (gs.Num - oldNum);
                                    }
                                }  
    
                                if (newNum > 0 && pd.InPrice > 0)
                                {
                                    gs.AC = (oldAC * (gs.Num - oldNum) + pd.Num * pd.InPrice) / newNum;
                                    gs.TaxAC = (oldTaxAC * (gs.Num - oldNum) + pd.Num * pd.TaxInPrice) / newNum;
                                }
                                else
                                {
                                    gs.AC = oldAC;
                                    gs.TaxAC = oldTaxAC;
                                }
                                gs.GoodsId = details[i].GoodsId;
                                gs.StoreId = purchase.StoreId;
                                gs.Num = gs.Num - oldNum + pd.Num;
                                gs.UpdateTime = DateTime.Now;
                                gs.Save();
                                //记录商品库存变化明细
                                GoodsStoreDetail gsd = new GoodsStoreDetail();
                                gsd.GoodsId = pd.GoodsId;
                                gsd.Id = 0;
                                //gsd.Model = pd.Model;
                                gsd.NewNum = gs.Num;
                                gsd.Num = pd.Num;
                                gsd.OldNum = gs.Num - pd.Num + oldNum;
                                gsd.Operate = "入库修改";
                                gsd.RelationId =pId;
                                gsd.StoreId =purchase.StoreId;
                                gsd.UpdateTime = DateTime.Now;
                                gsd.UserId = p.UserId;
                                gsd.AC = gs.AC;
                                gsd.TaxAC = gs.TaxAC;
                                gsd.Save();

                            }

                        }
                    }
                    else //新增商品
                    {
                        SavePurchaseDetail(purchase, details[i]);
                    }
                    //循环旧表 ，查找新表中没有的项。删除，记录明细
                    foreach (DataRow row in dtOld.Rows)
                    {
                        int goodsId = DBTool.GetIntFromRow(row, "goodsId", 0);
                        //string model = DBTool.GetStringFromRow(row, "Model", "");
                        int oldnum = DBTool.GetIntFromRow(row, "num", 0);
                        double oldBillPrice = DBTool.GetDoubleFromRow(row, "BillsPrice", 0);
                        double oldInPrice = DBTool.GetDoubleFromRow(row, "InPrice", 0);
                        bool isExsist = false;
                        for (int j = 0; j < details.Length; j++)
                        {
                            if (details[j].GoodsId == goodsId)
                            {
                                isExsist = true;
                                break;
                            }
                        }
                        if (isExsist == false)
                        {
                            //新订单中无 此项
                            int pdId = DBTool.GetIntFromRow(row, "Id", 0);
                            PurchaseDetail pd = new PurchaseDetail();
                            pd.Id = pdId;
                            pd.Load();
                            if (pd.Delete())
                            {
                                //记录修改明细记录
                                pd.Num = 0;
                                pd.InPrice = 0;
                                SavePurchaseModify(pd, oldnum, oldInPrice, purchase.UserId);
                                //修改库存 数量
                                GoodsStore gs = new GoodsStore();
                                gs.GoodsId = pd.GoodsId;
                                //gs.Model = pd.Model;
                                gs.StoreId = pd.StoreId;
                                gs.Load(pd.StoreId, pd.GoodsId);
                                gs.Num = gs.Num - oldnum;
                                gs.UpdateTime = DateTime.Now;
                                gs.Save();
                                //记录商品库存变化明细
                                GoodsStoreDetail gsd = new GoodsStoreDetail();
                                gsd.GoodsId = pd.GoodsId;
                                gsd.Id = 0;
                                //gsd.Model = pd.Model;
                                gsd.NewNum = gs.Num;
                                gsd.Num = 0 - oldnum;
                                gsd.OldNum = gs.Num + oldnum;
                                gsd.Operate = "修改删除";
                                gsd.RelationId = pd.PurchaseId;
                                gsd.StoreId = pd.StoreId;
                                gsd.UpdateTime = DateTime.Now;
                                gsd.UserId = p.UserId;
                                gsd.Save();
                            }
                        }
                    }

                }
            }
            return pId;
        }

        public int SetModifyPurchase(Purchase p, PurchaseDetail[] details)
        {
            Purchase purchase = new Purchase();
            purchase.Id = p.Id;
            purchase.Load();
            purchase.BranchId = p.BranchId;
            purchase.IsConfirm = p.IsConfirm;
            purchase.IsPaid = p.IsPaid;
            purchase.IsTax = p.IsTax;
            purchase.Memo = p.Memo;
            purchase.PurchaseDate = p.PurchaseDate;
            purchase.PurchaseType = p.PurchaseType;
            purchase.PurchaseUserId = p.PurchaseUserId;
            purchase.SumMoney = p.SumMoney;
            purchase.SupplierId = p.SupplierId;
            purchase.UpdateTime = DateTime.Now;
            purchase.UserId = p.UserId;
            purchase.StoreId = p.StoreId;
            purchase.Tax = p.Tax;
            purchase.TaxRate = p.TaxRate;
            int pId = purchase.Save();
            if (pId > 0)
            {
                //读取订单明细               
                DataTable dtOld = ReadPurchaseDetail(p.Id).Tables[0];

                //循环新表，处理新增商品和 修改过数量的商品
                for (int i = 0; i < details.Length; i++)
                {
                    DataRow[] rows = dtOld.Select(string.Format(" GoodsId={0} ", details[i].GoodsId));
                    if (rows.Length == 1)//有这个商品
                    {
                        int oldNum = DBTool.GetIntFromRow(rows[0], "Num", 0);
                        double oldPrice = DBTool.GetDoubleFromRow(rows[0], "InPrice", 0);
                        double oldAmount = DBTool.GetDoubleFromRow(rows[0], "Amount", 0);
                        double oldTaxInPrice = DBTool.GetDoubleFromRow(rows[0], "TaxInPrice", 0);
                        if (details[i].Num != oldNum || details[i].InPrice != oldPrice || details[i].Amount != oldAmount || details[i].TaxInPrice != oldTaxInPrice||details[i].StoreId != purchase.StoreId)//有变化需要修改
                        {
                            PurchaseDetail pd = new PurchaseDetail();
                            pd.Id = DBTool.GetIntFromRow(rows[0], "Id", 0);
                            pd.Load();
                            pd.BillsPrice = details[i].BillsPrice;
                            pd.InPrice = details[i].InPrice;
                            pd.Num = details[i].Num;
                            pd.PurchaseId = pId;
                            pd.StoreId = purchase.StoreId;
                            pd.Amount = details[i].Amount;
                            pd.TaxInPrice = details[i].TaxInPrice;                           
                            pd.Save();
                        }
                    }
                    else //新增商品
                    {
                        PurchaseDetail d = new PurchaseDetail();
                        d.GoodsId = details[i].GoodsId;
                        d.Id = 0;
                        d.BillsPrice = details[i].BillsPrice;
                        d.InPrice = details[i].InPrice;
                        //d.Model = details[i].Model;
                        d.Num = details[i].Num;
                        d.PurchaseId = pId;
                        d.Amount = details[i].Amount;
                        d.StoreId = purchase.StoreId;
                        d.TaxInPrice = details[i].TaxInPrice;                       
                        d.Save();

                    }
                    //循环旧表 ，查找新表中没有的项。删除，记录明细
                    foreach (DataRow row in dtOld.Rows)
                    {
                        int goodsId = DBTool.GetIntFromRow(row, "goodsId", 0);                        
                        bool isExsist = false;
                        for (int j = 0; j < details.Length; j++)
                        {
                            if (details[j].GoodsId == goodsId)
                            {
                                isExsist = true;
                                break;
                            }
                        }
                        if (isExsist == false)
                        {
                            //新订单中无 此项
                            int pdId = DBTool.GetIntFromRow(row, "Id", 0);
                            PurchaseDetail pd = new PurchaseDetail();
                            pd.Id = pdId;
                            pd.Load();
                            pd.Delete();
                        }
                    }


                }
            }
            return pId;
        }

        private void SavePurchaseModify(PurchaseDetail pd, int oldNum, double oldPrice, int UserId)
        {
            PurchaseModify pm = new PurchaseModify();
            pm.GoodsId = pd.GoodsId;
            //pm.Model = pd.Model;
            pm.NewNum = pd.Num;
            pm.NewPrice = pd.InPrice;
            pm.OldNum = oldNum;
            pm.OldPrice = oldPrice;
            pm.PurchaseId = pd.PurchaseId;
            pm.UpdateTime = DateTime.Now;
            pm.UserId = UserId;
            pm.Save();
        }
        /// <summary>
        /// 读取采购单  update by hjy 2017-3-1
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadPurchase(PurchaseOption option)
        {
            string sql = @"select *,
            CASE Ispaid
                WHEN 0 THEN '未付款'
                WHEN 1 THEN '已付款'
                WHEN 2 THEN '已对账'
            END as PaidStatus,
            CASE IsConfirm
                WHEN 0 THEN '未审核'
                WHEN 1 THEN '已审核'
            END as ConfirmStatus,
            CASE IsTax
                WHEN 0 THEN '无票'
                WHEN 1 THEN '增票'
                WHEN 2 THEN '普票'
            END as InvoiceType 
            from view_purchase where 1=1";

            if (option.BranchId > 0)
            {
                sql += string.Format(" and BranchId ={0} ", option.BranchId);
            }
            
            if (option.PurchaseId > 0)
            {
                sql += string.Format(" and Id = {0} ", option.PurchaseId);
            }
            else if (option.PurchaseStatementId > 0)
            {
                sql += string.Format(" and Id in (select purchaseId from PurchaseStatementDetail where PurchaseStatementId={0} )", option.PurchaseStatementId);
            }
            else
            {
                if (option.PurchaseStatus != "")
                {
                    sql += string.Format(" and PurchaseStatus='{0}'", option.PurchaseStatus);
                }
                if (option.Memo.Trim() != "")
                {
                    sql += string.Format(" and Memo like '%{0}%'", option.Memo.Trim());
                }
                if (option.EndDate != new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and UpdateTime < '{0}' ", option.EndDate.AddDays(1).ToShortDateString());
                }
                if (option.StartDate != new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and UpdateTime >='{0}' ", option.StartDate.ToShortDateString());
                }
                if (option.GoodsId > 0)
                {
                    sql += string.Format(" and ID in (select purchaseId from PurchaseDetail where GoodsId ={0})  ", option.GoodsId);
                }
                if (option.IsPaid > -1)
                {
                    sql += string.Format(" and IsPaid = {0} ", option.IsPaid);
                }
                if (option.IsConfirm > -1)
                {
                    sql += string.Format(" and IsConfirm ={0} ", option.IsConfirm);
                }
                if (option.PurchaseUserId > 0)
                {
                    sql += string.Format(" and PurchaseUserId ={0} ", option.PurchaseUserId);
                }
                if (option.StoreId > 0)
                {
                    sql += string.Format(" and storeId={0} ", option.StoreId);
                }
                if (option.SupplierId > 0)
                {
                    sql += string.Format(" and SupplierId = {0} ", option.SupplierId);
                }
                if (option.UserId > 0)
                {
                    sql += string.Format(" and UserId = {0} ", option.UserId);
                }
                if (option.StartId > 0)
                {
                    sql += string.Format(" and Id >= {0} ", option.StartId);
                }
                if (option.EndId > 0)
                {
                    sql += string.Format(" and Id <={0} ", option.EndId);
                }
                if (option.IsCashConfirm > -1)
                {
                    sql += string.Format(" and IsCashConfirm = {0} ", option.IsCashConfirm);
                }
                if (option.PurchaseType != "")
                {
                    if (option.PurchaseType.IndexOf("|") < 0)
                    {
                        sql += string.Format(" and PurchaseType = '{0}' ", option.PurchaseType);
                    }
                    else
                    {
                        string[] ps = option.PurchaseType.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        sql += " and ( ";
                        for (int i = 0; i < ps.Length; i++)
                        {
                            if (i == 0)
                            {
                                sql += string.Format(" PurchaseType ='{0}' ", ps[i]);
                            }
                            else
                            {
                                sql += string.Format(" or PurchaseType='{0}' ", ps[i]);
                            }

                        }
                        sql += " ) ";
                    }
                }
                else
                {
                    sql += " and PurchaseType <> '现退单'";
                }
                if (option.Company != "")
                {
                    sql += string.Format(" and ( Company like '%{0}%' or ShortName like '%{0}%' )", option.Company);
                }
            }
            sql += " order by UpdateTime,Id ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取采购商品统计--绩效
        /// add by hjy 2017-3-2
        /// </summary>
        /// <returns></returns>
        public DataSet ReadPurchasePerformance(PurchaseOption option)
        {
            string sql = "select GoodsId 商品编号,DisplayName 商品名称,BrandName,sum(Num) 总数量,SUM(Amount) 总金额,COUNT(GoodsId) 采购次数,SUM(Amount)/SUM(Num) 平均价 from view_PurchaseDetail where 1=1";
            if (option.BranchId > 0)
            {
                sql += string.Format(" and BranchId ={0} ", option.BranchId);
            }
            if (option.GoodsId > 0)
            {
                sql += string.Format(" and GoodsId={0}", option.GoodsId);
            }
            if (option.StartDate != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and PurchaseDate >='{0}' ", option.StartDate.ToShortDateString());
            }
            if (option.EndDate != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and PurchaseDate < '{0}' ", option.EndDate.AddDays(1).ToShortDateString());
            }
            sql += @" and (PurchaseType = '现金采购' or PurchaseType = '签单收货')  and PurchaseStatus='已入库' and IsDefault=1 and Num>0  group by GoodsId,DisplayName,BrandName  order by GoodsId ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取采购人员绩效--绩效
        /// add by hjy 2017-3-3
        /// edit by yanghaiyang  整体更换Sql语句
        /// </summary>
        /// <returns></returns>
        public DataSet ReadPersonPerformance(PurchaseOption option)
        {
            string sql = string.Format(@"select Name as 姓名 ,(TotalNum-ConfirmNum) as 未处理,(ConfirmNum-InStoreNum) as 已确认,InStoreNum as 已入库   from Sys_Users left join (

   select PurchaseUserId ,
          sum(case PurchaseStatus when '全部' then Num else 0  end) as TotalNum ,
          sum(case PurchaseStatus when '已确认' then Num else 0  end) as ConfirmNum ,
          sum(case PurchaseStatus when '已入库' then Num else 0  end) as InStoreNum
   from 
  
  ( select PurchaseUserId,PurchaseStatus  , sum(Num)as Num
             from PurchaseDetail ,Purchase  where Purchase.Id =PurchaseDetail.PurchaseId and  PurchaseDate >='{0}' and PurchaseDate <'{1}'  
             and UpdateTime  >='{0}' and UpdateTime <'{1}'  and BranchId ={2} and (PurchaseType = '现金采购' or PurchaseType = '签单收货')
             and purchase.StoreId =(select top 1 Id from Store where BranchId ={2} and IsDefault=1) and PurchaseStatus ='已入库' group by PurchaseUserId,PurchaseStatus  
   union all 
         select PurchaseUserId,'已确认' PurchaseStatus  , sum(Num)as Num 
             from PurchaseDetail ,Purchase  where Purchase.Id =PurchaseDetail.PurchaseId and  PurchaseDate >='{0}' and PurchaseDate <'{1}'  
             and ConfirmDate  >='{0}' and ConfirmDate <'{1}'  and BranchId ={2} and (PurchaseType = '现金采购' or PurchaseType = '签单收货')
             and purchase.StoreId =(select top 1 Id from Store where BranchId ={2} and IsDefault=1)  group by PurchaseUserId
   union all 
          select PurchaseUserId,'全部' PurchaseStatus  ,sum(Num)as Num 
             from PurchaseDetail ,Purchase  where Purchase.Id =PurchaseDetail.PurchaseId and  PurchaseDate >='{0}' and PurchaseDate <'{1}'  
             and BranchId ={2} and (PurchaseType = '现金采购' or PurchaseType = '签单收货')
             and purchase.StoreId =(select top 1 Id from Store where BranchId ={2} and IsDefault=1)  group by PurchaseUserId) temp1 group by PurchaseUserId 
     ) temp2 on Sys_Users.Id=temp2.PurchaseUserId
 where deptId =(select Id from Sys_Dept where Name='采购部' and BranchId ={2}) ", option.StartDate.ToShortDateString(),option.EndDate.AddDays(1).ToShortDateString(),option.BranchId);           

            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取待收货采购单列表 20160307 lihailong
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public DataSet ReadNeedToReceivePurchase(int branchId)
        {
            string sql = string.Format("select SupplierId,Company,Id,PurchaseDate,SumMoney,purchasestatus,PurchaseUserName,IsTax,UpdateTime from view_purchase where 1=1 and (purchasestatus='" + CommenClass.PurchaseStatus.已确认 + "' or purchasestatus='" + CommenClass.PurchaseStatus.收货中 + "') and BranchId={0} order by Id ", branchId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取采购单明细 
        /// ERP中 FPurchase.cs 中曾经用到
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <returns></returns>
        public DataSet ReadPurchaseDetail(int purchaseId)
        {
            string sql = string.Format("select * from view_PurchaseDetail where PurchaseId={0}", purchaseId);
            return m_dbo.GetDataSet(sql);
        }


        /// <summary>
        /// 读取采购单明细 -2017-05-16 新加
        /// ERP中 FPurchase.cs 中用到 
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <returns></returns>
        public DataSet PurchaseDetailBYpurchaseId(int purchaseId)
        {
            string sql = string.Format(@"select * from View_PurchaseDetail where PurchaseId ={0}", purchaseId);
            return m_dbo.GetDataSet(sql);
        }
       
        /// <summary>
        /// update by hjy 2017-3-1
        /// 采购单读取订单单品备注
        /// </summary>
        /// <returns></returns>
        public DataSet ReadPurchaseMenoByPurchaseId(int purchaseId)
        {
            string sql = string.Format(@"select OrderId ,GoodsId ,DisplayName ,Num ,SalePrice ,PurchaseMemo ,Company ,DeptName ,RealName  from View_OrderDetail vod where vod.OrderId in (select npo.OrderId  from NeedToPurchaseOrder npo where npo.NeedToPurchaseId=(select NeedToPurchaseId from Purchase where Id ={0}))  and vod.GoodsId in (
select pd.GoodsId  from PurchaseDetail pd where pd.PurchaseId ={0})",purchaseId);
            return m_dbo.GetDataSet(sql);
        }
        public DataSet ReaddiffPurchaseDetail(int purchaseId)
        {
            string sql = string.Format(@"select * from view_PurchaseDetail where PurchaseId={0};
select GoodsId ,DisplayName,BranchId,SupplierId,(Num-ReceivedNum) num ,unit,InPrice,BillsPrice,StoreId from  View_PurchaseDetail where  PurchaseId={0}", purchaseId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取采购单差异明细
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <returns></returns>
        public DataSet ReadDetail(int purchaseId)
        {
            string sql = string.Format("select * from View_PurchaseDetail where Num!=ReceivedNum and PurchaseId={0} ", purchaseId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 根据userId读取现金采购列表
        /// </summary>
        /// <param name="userId">操作员Id</param>
        /// <returns></returns>
        public DataSet GetCashPurchaseOrder(int userId)
        {
            string sql = string.Format("select * from view_purchase where PurchaseType='{0}' and PurchaseStatus='{1}' and PurchaseUserId='{2}' order by Id", CommenClass.PurchaseType.现金采购.ToString(), CommenClass.PurchaseStatus.未处理.ToString(), userId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取已收货采购单列表
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public DataSet ReadNeedToPutInPurchase(int branchId)
        {
            string sql = string.Format("select BranchId,SupplierId,ReceiptUserId,ReceiveName,Company,Id,PurchaseDate,SumMoney,purchasestatus,UpdateTime,PurchaseType from view_purchase where 1=1 and purchasestatus='" + CommenClass.PurchaseStatus.已收货.ToString() + "' and BranchId={0} order by Id ", branchId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取采购单明细--批量
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadPurchaseDetails(PurchaseOption option)
        {
            string sql = " select * from view_PurchaseDetail where 1=1 ";
            if (option.BranchId > 0)
            {
                sql += string.Format(" and BranchId = {0} ", option.BranchId);
            }
            if (option.PurchaseId > 0)
            {
                sql += string.Format(" and PurchaseId = {0}", option.PurchaseId);
            }
            if (option.SupplierId > 0)
            {
                sql += string.Format(" and SupplierId = {0} ", option.SupplierId);
            }
            if (option.EndDate != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and PurchaseDate < '{0}' ", option.EndDate.AddDays(1).ToShortDateString());
            }
            if (option.StartDate != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and PurchaseDate >='{0}' ", option.StartDate.ToShortDateString());
            }
            if (option.GoodsId > 0)
            {
                sql += string.Format(" and GoodsId = {0} ", option.GoodsId);
            }
            if (option.PurchaseStatus != "")
            {
                sql += string.Format(" and PurchaseStatus='{0}' ", option.PurchaseStatus);
            }
            sql += " order by PurchaseId desc ";

            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// add by quxiaoshan 2014-12-5 
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadNeedToPurchaseDetails(PurchaseOption option)
        {
            string sql = string.Format(@"select NeedToPurchaseDetail.*,Goods.DisplayName as DisplayName,Goods.GoodsRank as GoodsRank,dbo.NeedToPurchase.BranchId from dbo.NeedToPurchaseDetail 
            inner join dbo.Goods on dbo.NeedToPurchaseDetail.GoodsId=dbo.Goods.ID 
            join dbo.NeedToPurchase on dbo.NeedToPurchaseDetail.NeedToPurchaseId=dbo.NeedToPurchase.Id
            where  1=1 "); //join 关联NeedToPurchase，关联加盟商的branchId add by quxiaoshan 2015-4-30


            if (option.GoodsId > 0)
            {
                sql += string.Format("and GoodsId={0}", option.GoodsId);
            }
            if (option.StartDate > new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and NeedToPurchaseDetail.UpdateTime >='{0}' ", option.StartDate.ToShortDateString());
            }
            if (option.EndDate > new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and NeedToPurchaseDetail.UpdateTime < '{0}' ", option.EndDate.AddDays(1).ToShortDateString());
            }
            sql += string.Format(" and dbo.NeedToPurchase.BranchId={0} ", option.BranchId);//add by quxiaoshan 2015-4-30
            sql += " order by dbo.NeedToPurchaseDetail.Id desc";
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 删除采购对账单
        /// </summary>
        /// <param name="PurchaseStatementId"></param>
        /// <returns></returns>
        public bool Delete(int PurchaseStatementId)
        {
            string sql = string.Format(@" update Purchase set IsPaid=0 where Id in( select PurchaseId from PurchaseStatementDetail where PurchaseStatementId={0});
                                     delete PurchaseStatementDetail where PurchaseStatementId ={0};      
                                     delete PurchaseStatement where Id={0}", PurchaseStatementId);
            return m_dbo.ExecuteNonQuery(sql);

        }

        //读取默认仓库的采购商品
        public DataSet ReadPurchaseInPrice(int goodsId, int BranchId)
        {
            string sql = string.Format(@"select top 1 GoodsId,BranchId,InPrice,TaxInPrice from View_PurchaseDetail where BranchId ={0} and GoodsId={1}  and (PurchaseType = '现金采购' or PurchaseType = '签单收货')  and PurchaseStatus='已入库' and IsDefault=1 and Num>0 and BillsPrice >0 order by UpdateTime desc;
select top 1 GoodsId,BranchId,InPrice,TaxInPrice from View_PurchaseDetail where BranchId=1 and GoodsId={1} and (PurchaseType = '现金采购' or PurchaseType = '签单收货')  and PurchaseStatus='已入库' and IsDefault=1 and Num>0 and BillsPrice >0 order by UpdateTime desc", BranchId, goodsId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取多张采购单明细
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <returns></returns>
        public DataSet ReadPurchaseDetails(int[] purchaseId)
        {
            if (purchaseId.Length > 0)
            {
                string PId = "";
                for (int i = 0; i < purchaseId.Length; i++)
                {
                    if (i == 0)
                    {
                        PId = purchaseId[i].ToString();
                    }
                    else
                    {
                        PId += "," + purchaseId[i].ToString();
                    }
                }
                string sql = string.Format("select * from View_PurchaseDetail where PurchaseId in ({0})", PId);
                return m_dbo.GetDataSet(sql);
            }
            return null;
        }
        #region 采购单拆单
        /// <summary>
        /// 生成采购单
        /// </summary>
        /// <param name="p"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public int SetPurchaseDetail(Purchase p, PurchaseDetail[] details)
        {
            Purchase purchase = new Purchase();
            purchase.BranchId = p.BranchId;
            purchase.Id = p.Id;
            purchase.IsPaid = p.IsPaid;
            purchase.Memo = p.Memo;
            purchase.PurchaseDate = p.PurchaseDate;
            purchase.PurchaseType = p.PurchaseType;
            purchase.PurchaseUserId = p.PurchaseUserId;
            purchase.SumMoney = p.SumMoney;
            purchase.SupplierId = p.SupplierId;
            purchase.UpdateTime = DateTime.Now;
            purchase.UserId = p.UserId;
            purchase.IsTax = p.IsTax;
            purchase.StoreId = p.StoreId;
            purchase.IsConfirm = p.IsConfirm;
            purchase.PurchaseStatus = p.PurchaseStatus;
            purchase.ReceiptUserId = p.ReceiptUserId;
            purchase.ConfirmDate = p.ConfirmDate;
            purchase.Tax = p.Tax;
            purchase.TaxRate = p.TaxRate;
            int pId = purchase.Save();
            if (pId > 0)
            {
                StringBuilder stb = new StringBuilder();
                int i = 0;
                stb.Append("insert into PurchaseDetail(PurchaseId,GoodsId,StoreId,BillsPrice,InPrice,Num,Amount,OldStore,OldAC,OldGoodsId,ReceivedNum,Model,TaxInPrice) values ");
                foreach (PurchaseDetail pd in details)
                {
                    if (i == 0)
                    {
                        stb.Append(string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},'',{11})", pId, pd.GoodsId, p.StoreId, pd.BillsPrice, pd.InPrice, pd.Num, pd.Amount, pd.StoreId, pd.InPrice, pd.GoodsId, pd.ReceivedNum, pd.TaxInPrice));
                    }
                    else
                    {
                        stb.Append(string.Format(", ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},'',{11})", pId, pd.GoodsId, p.StoreId, pd.BillsPrice, pd.InPrice, pd.Num, pd.Amount, pd.StoreId, pd.InPrice, pd.GoodsId, pd.ReceivedNum, pd.TaxInPrice));
                    }
                    i++;
                }
                stb.Append(string.Format(@"update Purchase set SumMoney=(select  ISNULL(SUM(Amount),0) from PurchaseDetail where PurchaseId={0}),
                                                  Tax=(select  ISNULL(SUM(Amount),0) from PurchaseDetail where PurchaseId={0})/(1+{1})*{1}  where Id={0}", pId, purchase.TaxRate));//检查PDA是否 前端已经算好 sumMoney
                m_dbo.ExecuteNonQuery(stb.ToString());
                return pId;
            }
            return -1;
        }

        /// <summary>
        /// 同步主的采购单
        /// </summary>
        /// <param name="OldPurchaseId"></param>
        /// <param name="SynchPurchaseId"></param>
        /// <returns></returns>
        private bool UpPurchaseDetail(int OldPurchaseId, int SynchPurchaseId, int UserId, int Operation)
        {
            string sql = string.Format(@"update pd1 set Num=pd1.Num-pd2.Num,Amount=(pd1.Num-pd2.Num)* pd1.BillsPrice from PurchaseDetail pd1 inner join PurchaseDetail pd2 on pd1.GoodsId=pd2.GoodsId
where pd1.PurchaseId={0} and pd2.PurchaseId={1};
delete PurchaseDetail where PurchaseId={0} and Num=0; ", OldPurchaseId, SynchPurchaseId);
            if (m_dbo.ExecuteNonQuery(sql))
            {
                if (Operation == 1)
                {
                    return UPReceiptUser(OldPurchaseId, UserId);
                }
                else
                {
                    return UPPurchaseUser(OldPurchaseId, UserId);
                }
            }
            return false;
        }

        /// <summary>
        /// 收货人同步
        /// </summary>
        /// <param name="OldPurchaseId"></param>
        /// <returns></returns>
        private bool UPReceiptUser(int OldPurchaseId, int Userid)
        {
            string sql = string.Format(@"DECLARE @count money
DECLARE @PurchaseStatus varchar(100)
select  @PurchaseStatus=PurchaseStatus from Purchase where Id={0}
select @count=SUM(Amount) ,@PurchaseStatus=( CASE WHEN sum(Num-ReceivedNum)=0 THEN '已收货' 
WHEN sum(Num-ReceivedNum) >0 and sum(Num-ReceivedNum)<sum(Num)  THEN '收货中'
  ELSE @PurchaseStatus END ) from PurchaseDetail where PurchaseId={0}
 update Purchase set PurchaseStatus=@PurchaseStatus, SumMoney =@count,Tax=@count/(1+TaxRate)*TaxRate, UpdateTime=GETDATE(),ReceiptUserId={1} where Id={0}", OldPurchaseId, Userid);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 采购人同步
        /// </summary>
        /// <param name="OldPurchaseId"></param>
        /// <returns></returns>
        private bool UPPurchaseUser(int OldPurchaseId, int Userid)
        {
            string sql = string.Format(@"DECLARE @count money
DECLARE @PurchaseStatus varchar(100)
DECLARE @PurchaseType varchar(100)
select  @PurchaseStatus=PurchaseStatus,@PurchaseType=PurchaseType from Purchase where Id={0}
select @count=SUM(Amount) ,@PurchaseStatus=( CASE WHEN sum(Num-ReceivedNum)=0  THEN '已确认'
WHEN sum(Num-ReceivedNum) >0 and sum(Num-ReceivedNum)<sum(Num) and @PurchaseType='现金采购'  THEN '未处理'
WHEN sum(Num-ReceivedNum) >0 and sum(Num-ReceivedNum)<sum(Num) and @PurchaseType ='签单收货' THEN '已确认'
  ELSE @PurchaseStatus END ) from PurchaseDetail where PurchaseId={0}
 update Purchase set PurchaseStatus=@PurchaseStatus, SumMoney =@count,Tax=@count/(1+TaxRate)*TaxRate ,UpdateTime=GETDATE(),UserId={1} where Id={0}", OldPurchaseId, Userid);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 采购单拆单(移动设备)
        /// </summary>
        /// <param name="PurchaseId"></param>
        /// <param name="Operation">0:采购拆单 1：收货拆单</param>
        /// <param name="UserId">操作人</param>
        /// <param name="Operation"></param>
        /// <returns></returns>
        public int UnweavePurchase(int PurchaseId, int UserId, int Operation)//operation 0:采购拆单 1：收货拆单
        {
            string sqlPurchase = string.Format(@"insert into Purchase (BranchId,StoreId,SupplierId,PurchaseType,SumMoney,PurchaseDate,PurchaseUserId,IsPaid,IsTax,Memo,IsConfirm,IsCashConfirm,UserId,PurchaseStatus,NeedToPurchaseId)
select BranchId,StoreId,SupplierId,PurchaseType,'',getdate(),PurchaseUserId,IsPaid,IsTax,Memo,IsConfirm,IsCashConfirm,UserId,'{1}',NeedToPurchaseId from Purchase where Id={0};select @@identity",
PurchaseId, CommenClass.PurchaseStatus.未处理.ToString());
            int NewPurchaseId = 0;
            object o = m_dbo.ExecuteScalar(sqlPurchase);
            try
            {
                NewPurchaseId = Convert.ToInt32(o);
            }
            catch
            {
                return 0;
            }
            if (NewPurchaseId > 0)
            {
                string sql = string.Format(@"insert into PurchaseDetail (PurchaseId,GoodsId,StoreId,Model,BillsPrice,InPrice,Num,Amount,OldStore,OldAC,OldGoodsId,ReceivedNum) 
select {0},GoodsId,StoreId,'',BillsPrice,InPrice,Num-ReceivedNum,BillsPrice*(Num-ReceivedNum),
oldstore,OldAC,OldGoodsId,0 from PurchaseDetail where PurchaseId={1} and Num!=ReceivedNum;
update Purchase set SumMoney=(select ISNULL(SUM(Amount),0) from PurchaseDetail where PurchaseId={0}),
                    Tax=(select ISNULL(SUM(Amount),0) from PurchaseDetail where PurchaseId={0})/(1+TaxRate)*TaxRate  where Id={0}", NewPurchaseId, PurchaseId);
                if (m_dbo.ExecuteNonQuery(sql))
                {
                    UpPurchaseDetail(PurchaseId, NewPurchaseId, UserId, Operation);
                }
            }
            return NewPurchaseId;
        }

        /// <summary>
        /// 采购单拆单
        /// </summary>
        /// <param name="OldPurchaseId"></param>
        /// <param name="p"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public int UnweavePurchase(int OldPurchaseId, Purchase p, PurchaseDetail[] details, int UserId)
        {
            int SynchPurchaseId = SetPurchaseDetail(p, details);
            if (SynchPurchaseId > 0)
            {
                UpPurchaseDetail(OldPurchaseId, SynchPurchaseId, UserId, 0);
            }
            return SynchPurchaseId;
        }

        /// <summary>
        /// 同步采购单总价
        /// </summary>
        /// <param name="PId"></param>
        /// <returns></returns>
        public bool UpdatePurchaseSumMoney(int PId)
        {
            string sql = string.Format(@"update Purchase set SumMoney=(select SUM(Amount) from PurchaseDetail where PurchaseId={0}),
                                                  Tax=(select SUM(Amount) from PurchaseDetail where PurchaseId={0})/(1+TaxRate)*TaxRate  where Id={0}", PId);
            return m_dbo.ExecuteNonQuery(sql);
        }
        #endregion

        public bool SetNeedToPurchaseOrder(int NeedToPurchaseId, int[] orderid)
        {
            string sqlNTPO = string.Format(" insert into NeedToPurchaseOrder (NeedToPurchaseId,OrderId) values ");
            string sqlOrderStatus = string.Format(" update OrderStatus set PurchaseStatus='已接受' where OrderId in( ");
            foreach (int Id in orderid)
            {
                sqlNTPO += string.Format("({0},{1}),", NeedToPurchaseId, Id);
                sqlOrderStatus += string.Format("{0},", Id);
            }
            if (m_dbo.ExecuteNonQuery(sqlNTPO.Trim(',')))
            {
                sqlOrderStatus = sqlOrderStatus.Trim(',') + " ) ";
                return m_dbo.ExecuteNonQuery(sqlOrderStatus);
            }
            return false;
        }

        /// <summary>
        /// 读取删除订单商品对应的采购单明细
        /// </summary>
        /// <param name="BranchId"></param>
        /// <returns></returns>
        public DataSet ReadOrderModifyPurchase(int BranchId, int SupplierId, int PurchaseUserId)
        {
            string sql = string.Format(@"select  vpd.Id,PurchaseId,vpd.SupplierId,('('+vpd.ShortName+')'+vpd.Company) as Company,StoreId,vpd.PurchaseUserId,vpd.PurchaseUserName, vpd.Num as PNum,(OldNum-NewNum) as DNum,vpd.GoodsId,vpd.DisplayName,vpd.PurchaseType,vpd.Amount,vpd.PurchaseDate,vpd.PurchaseStatus,vpd.Unit,omc.Id as OMId  from View_PurchaseDetail vpd inner join  (select *,(select NeedToPurchaseId from NeedToPurchaseOrder where OrderId =om.OrderId) as NeedToPurchaseId from OrderModify om where PurchaseType ='未处理' ) omc  on vpd.NeedToPurchaseId= omc.NeedToPurchaseId and vpd.GoodsId=omc.GoodsId where vpd.BranchId={0} and OldNum-NewNum>0 ", BranchId);
            if (SupplierId > 0)
            {
                sql += string.Format(" and SupplierId={0}", SupplierId);
            }
            if (PurchaseUserId > 0)
            {
                sql += string.Format("  and PurchaseUserId={0} ", PurchaseUserId);
            }
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 同步订单删除对应采购商品
        /// </summary>
        /// <param name="PurchaseDetailId"></param>
        /// <param name="PurchaseId"></param>
        /// <param name="Dnum"></param>
        /// <param name="OrderModifyId"></param>
        /// <returns></returns>
        public bool SynchPurchaseDetail(int PurchaseDetailId, int PurchaseId, int Dnum, int OrderModifyId)
        {
            Purchase p = new Purchase();
            p.Id = PurchaseId;
            p.Load();
            if (p.PurchaseStatus == CommenClass.PurchaseStatus.未处理.ToString() || p.PurchaseStatus == CommenClass.PurchaseStatus.已确认.ToString())
            {
                PurchaseDetail pd = new PurchaseDetail();
                pd.Id = PurchaseDetailId;
                pd.Load();
                pd.Num = pd.Num - Dnum;
                pd.Amount = pd.Num * pd.InPrice;
                if (pd.Save() > 0)
                {
                    p.UpdatePurchaseSumMoney(PurchaseId);
                    OrderModify om = new OrderModify();
                    om.Id = OrderModifyId;
                    om.PurchaseType = CommenClass.PurchaseStatus.已确认.ToString();
                    om.Save();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 更新采购详情单中的ReceivedNum为0(当采购完毕后)            
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public bool UpdatePDReceivedNum(int pid)
        {
            if (pid > 0)
            {
                DBOperate dbo = new DBOperate();
                string sqlStr = "update PurchaseDetail set ReceivedNum=0 where PurchaseId=" + pid;
                return dbo.ExecuteNonQuery(sqlStr);
            }
            else
            {
                return false;
            }
        }



        public string DeletePurchaseDetail(int pId, int GoodsId, int userId)
        {
            Purchase p = new Purchase();
            p.Id = pId;
            p.Load();

            PurchaseDetail pd = new PurchaseDetail();
            pd.Load(pId, GoodsId);
            string Exception = "";
            if (p.PurchaseStatus == CommenClass.PurchaseStatus.未处理.ToString() || p.PurchaseStatus == CommenClass.PurchaseStatus.已确认.ToString())
            {
                if (pd.Delete())
                {
                    
                    Purchase purchase = new Purchase();
                    purchase.UpdatePurchaseSumMoney(p.Id);
                    Exception = "删除成功！";
                    //添加删除记录
                    Sys_Log sl = new Sys_Log();
                    sl.Id = 0;
                    sl.UserId = userId;
                    sl.OperateType = LogOperateType.采购单删除.ToString();
                    sl.Operate = "采购单删除商品：采购单号" + pId + "商品Id" + GoodsId + "";
                    sl.ObjectId = pId;
                    sl.UpdateTime = DateTime.Now;
                    sl.Save();

                }


            }
            else
            {
                Exception = string.Format("该商品{0}！", p.PurchaseStatus);
            }
            return Exception;
        }
    }
}
