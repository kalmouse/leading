using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class NeedToPurchaseDetail
    {
        private int m_Id;
        private int m_NeedToPurchaseId;
        private int m_GoodsId;
        private string m_Model;
        private int m_Num;
        private string m_PurchaseType;
        private string m_PurchaseMemo;
        private string m_CreditCard;
        private DateTime m_UpdateTime;
        private int m_LackNum;
        private int m_Emergency;
        private int m_UserId;
        private string m_GoodsMemo;//add by quxiaoshan 2014-11-11
        private int m_SupplierId;
        private double m_InPrice;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int NeedToPurchaseId { get { return m_NeedToPurchaseId; } set { m_NeedToPurchaseId = value; } }
        public int GoodsId { get { return m_GoodsId; } set { m_GoodsId = value; } }
        public string Model { get { return m_Model; } set { m_Model = value; } }
        public int Num { get { return m_Num; } set { m_Num = value; } }
        public string PurchaseType { get { return m_PurchaseType; } set { m_PurchaseType = value; } }
        public string PurchaseMemo { get { return m_PurchaseMemo; } set { m_PurchaseMemo = value; } }
        public string CreditCard { get { return m_CreditCard; } set { m_CreditCard = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public int LackNum { get { return m_LackNum; } set { m_LackNum = value; } }
        public int Emergency { get { return m_Emergency; } set { m_Emergency = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public string GoodsMemo { get { return m_GoodsMemo; } set { m_GoodsMemo = value; } }//add by quxiaoshan 2014-11-11
        public int SupplierId { get { return m_SupplierId; } set { m_SupplierId = value; } }
        public double InPrice { get { return m_InPrice; } set { m_InPrice = value; } }
        public double SprTaxRate{get;set;}
        public string SprInvoiceType{get;set;}        
        public NeedToPurchaseDetail()
        {
            m_Id = 0;
            m_NeedToPurchaseId = 0;
            m_GoodsId = 0;
            m_Model = "";
            m_Num = 0;
            m_UserId = 0;
            m_PurchaseType = "";
            m_PurchaseMemo = "";
            m_CreditCard = "";
            m_UpdateTime = DateTime.Now;
            m_LackNum = 0;
            m_Emergency = 0;
            m_GoodsMemo = "";//add by quxiaoshan 2014-11-11
            m_SupplierId = 0;
            m_InPrice = 0;
            SprInvoiceType = "";
            SprTaxRate = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@NeedToPurchaseId", m_NeedToPurchaseId));
            arrayList.Add(new SqlParameter("@GoodsId", m_GoodsId));
            arrayList.Add(new SqlParameter("@Model", m_Model));
            arrayList.Add(new SqlParameter("@Num", m_Num));
            arrayList.Add(new SqlParameter("@UserId", m_UserId));
            arrayList.Add(new SqlParameter("@PurchaseType", m_PurchaseType));
            arrayList.Add(new SqlParameter("@PurchaseMemo", m_PurchaseMemo));
            arrayList.Add(new SqlParameter("@CreditCard", m_CreditCard));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            arrayList.Add(new SqlParameter("@LackNum", m_LackNum));
            arrayList.Add(new SqlParameter("@Emergency", m_Emergency));
            arrayList.Add(new SqlParameter("@GoodsMemo", m_GoodsMemo));//add by quxiaoshan 2014-11-11
            arrayList.Add(new SqlParameter("@SupplierId", m_SupplierId));
            arrayList.Add(new SqlParameter("@InPrice", m_InPrice));
            arrayList.Add(new SqlParameter("@SprInvoiceType", SprInvoiceType));
            arrayList.Add(new SqlParameter("@SprTaxRate", SprTaxRate));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("NeedToPurchaseDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("NeedToPurchaseDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from NeedToPurchaseDetail where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_NeedToPurchaseId = DBTool.GetIntFromRow(row, "NeedToPurchaseId", 0);
                m_GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                m_Model = DBTool.GetStringFromRow(row, "Model", "");
                m_Num = DBTool.GetIntFromRow(row, "Num", 0);
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_PurchaseType = DBTool.GetStringFromRow(row, "PurchaseType", "");
                m_PurchaseMemo = DBTool.GetStringFromRow(row, "PurchaseMemo", "");
                m_CreditCard = DBTool.GetStringFromRow(row, "CreditCard", "");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_LackNum = DBTool.GetIntFromRow(row, "LackNum", 0);
                m_Emergency = DBTool.GetIntFromRow(row, "Emergency", 0);
                m_GoodsMemo = DBTool.GetStringFromRow(row, "GoodsMemo", "");
                m_SupplierId = DBTool.GetIntFromRow(row, "SupplierId", 0);
                m_InPrice = DBTool.GetDoubleFromRow(row, "InPrice", 0);
                SprInvoiceType = DBTool.GetStringFromRow(row, "SprInvoiceType", "");
                SprTaxRate = DBTool.GetDoubleFromRow(row, "SprTaxRate", 0);
                return true;
            }
            return false;
        }

        public bool Load(int NeedToPurchaseId, int GoodsId)
        {
            string sql = string.Format("select * from NeedToPurchaseDetail where NeedToPurchaseId ={0} and GoodsId={1}", NeedToPurchaseId, GoodsId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_NeedToPurchaseId = DBTool.GetIntFromRow(row, "NeedToPurchaseId", 0);
                m_GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                m_Model = DBTool.GetStringFromRow(row, "Model", "");
                m_Num = DBTool.GetIntFromRow(row, "Num", 0);
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_PurchaseType = DBTool.GetStringFromRow(row, "PurchaseType", "");
                m_PurchaseMemo = DBTool.GetStringFromRow(row, "PurchaseMemo", "");
                m_CreditCard = DBTool.GetStringFromRow(row, "CreditCard", "");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_LackNum = DBTool.GetIntFromRow(row, "LackNum", 0);
                m_Emergency = DBTool.GetIntFromRow(row, "Emergency", 0);
                m_GoodsMemo = DBTool.GetStringFromRow(row, "GoodsMemo", "");
                m_SupplierId = DBTool.GetIntFromRow(row, "SupplierId", 0);
                m_InPrice = DBTool.GetDoubleFromRow(row, "InPrice", 0);
                SprInvoiceType = DBTool.GetStringFromRow(row, "SprInvoiceType", "");
                SprTaxRate = DBTool.GetDoubleFromRow(row, "SprTaxRate", 0);
                return true;
            }
            return false;
        }

        private DataSet ReadNeedToPurchaseDetail(int NeedToPurchaseId)
        {
            string sql = string.Format("select  DISTINCT SupplierId,UserId, NeedToPurchaseId ,PurchaseType,SprInvoiceType,SprTaxRate from NeedToPurchaseDetail where NeedToPurchaseId={0} and LackNum<0 order by  SupplierId ,UserId", NeedToPurchaseId);
            return m_dbo.GetDataSet(sql);
        }

        public DataSet ReadOrderModifyNeedToPurchaseDetail(int branchId)
        {
            string sql = string.Format(@"select * from (
select vod.GoodsId,DisplayName,ISNULL( pd.PurchaseUserId,0) as UserId,pd.PurchaseUserName,SUM(vod.NewNum) as Num, CASE  WHEN (gst.Num-POD.Pnum)>0 THEN gst.Num-POD.Pnum-SUM(vod.NewNum) ELSE -SUM(vod.NewNum) END as LackNum,case WHEN  pd.InPrice >0 THEN pd.InPrice ELSE min(vod.InPrice) end InPrice,isnull(pd.SupplierId,0) as SupplierId ,pd.Company,pd.PurchaseType,vod.Id   from View_OrderModify vod left join (select pd.GoodsId,PurchaseUserName,PurchaseUserId,Company,SupplierId,InPrice,PurchaseType from (select GoodsId,max(Id) as Id from  View_PurchaseDetail where branchId={0}  and  IsInner=1 and PurchaseType<>'签单退货' and goodsId in (select goodsId from View_OrderModify where PurchaseType='未处理' and NewNum>0 and (OldNum-NewNum)<0)  group by GoodsId) as pd inner join View_PurchaseDetail vpd on pd.Id=vpd.Id) pd on vod.GoodsId=pd.GoodsId LEFT  JOIN (select goodsId,(SUM(Num)-SUM(picknum)) as Pnum from view_OrderDetail where   PurchaseStatus='已接受' and (storestatus = '已接受' or storestatus='配货中' )and BranchId={0} and IsDelete=0 and RawOrderId=0 and ComId>=1000 group by GoodsId) as POD on vod.GoodsId=POD.GoodsId LEFT  JOIN (select GoodsId,sum(Num) Num from view_GoodsStore where BranchId={0}  group by GoodsId ) as gst on vod.GoodsId=gst.GoodsId where vod.BranchId={0} and  vod.PurchaseType='未处理' and vod.NewNum>0 and (OldNum-NewNum)<0 group by vod.GoodsId,POD.Pnum,gst.Num, DisplayName,Unit,pd.PurchaseUserId,pd.PurchaseUserName,pd.Company,pd.SupplierId,pd.InPrice,pd.PurchaseType,vod.Id ) as c where c.LackNum<=0;
", branchId);
            return m_dbo.GetDataSet(sql);
        }


        /// <summary>
        /// 商品汇总单拆分采购单
        /// </summary>
        /// <param name="NeedToPurchaseId"></param>
        /// <param name="branchId"></param>
        /// <param name="StoreId"></param>
        /// <returns></returns>
        public bool PurchaseSave(int NeedToPurchaseId, int branchId, int StoreId)
        {
            DataTable dtNeedToPurchaseDetail = ReadNeedToPurchaseDetail(NeedToPurchaseId).Tables[0];
            if (dtNeedToPurchaseDetail.Rows.Count > 0)
            {
                int SId = 0;
                double taxRate = 0;
                int isTax = 0;
                string invoiceType = "";
                int SupplierId = 0;
                foreach (DataRow dr in dtNeedToPurchaseDetail.Rows)
                {
                    SupplierId = DBTool.GetIntFromRow(dr, "SupplierId", 0);
                    taxRate = DBTool.GetDoubleFromRow(dr, "SprTaxRate", 0.17);
                    invoiceType = DBTool.GetStringFromRow(dr, "SprInvoiceType","");
                    switch (invoiceType)
                    {
                        case "无发票":
                            isTax = 0;
                            taxRate = 0;
                            break;
                        case "增票":
                            isTax = 1;
                            break;
                        case "普票":
                            isTax = 2;
                            break;
                        default:
                            isTax = 1;
                            taxRate = 0.17;
                            break;
                    }
                    int UserId = DBTool.GetIntFromRow(dr, "UserId", 0);
                    string PurchaseType = DBTool.GetStringFromRow(dr, "PurchaseType", "");
                    Purchase p = new Purchase();
                    p.BranchId = branchId;
                    p.NeedToPurchaseId = NeedToPurchaseId;
                    p.PurchaseStatus = CommenClass.PurchaseStatus.未处理.ToString();
                    p.PurchaseType = PurchaseType;
                    p.UserId = 0;
                    p.StoreId = StoreId;
                    p.SupplierId = SupplierId;
                    p.PurchaseUserId = UserId;
                    p.IsTax = isTax;
                    p.TaxRate = taxRate;
                    int purchaseId = p.Save();
                    if (purchaseId > 0)
                    {
                        string sql = string.Format(@"insert into PurchaseDetail(PurchaseId,GoodsId,StoreId,Model,BillsPrice,InPrice,Num,Amount,ReceivedNum,OldStore,OldAC,Emergency,TaxInPrice)
                                                                        select {0},
                                                                               GoodsId,
                                                                               {1},
                                                                               '',
                                                                               InPrice,
                                                                               (case when {7}=0 then InPrice*1.25 when {7}=1 then InPrice/(1+{6}) when {7}=2 then InPrice end),
                                                                               -LackNum,
                                                                               (InPrice * (-LackNum)),
                                                                               0,
                                                                               {1},
                                                                               InPrice,
                                                                               Emergency,
                                                                               (case when {7}=0 then InPrice*1.25*1.17 when {7}=1 then InPrice when {7}=2 then InPrice*(1+{6}) end)
                                                                       from NeedToPurchaseDetail where NeedToPurchaseId={2} and SupplierId={3} and UserId={4}  and LackNum<0;
                         update Purchase set SumMoney=(select ISNULL(SUM(Amount),0) from PurchaseDetail where PurchaseId={0}),
                                                  Tax=(select ISNULL(SUM(Amount),0) from PurchaseDetail where PurchaseId={0})/(1+{6})*{6}  where Id={0}", purchaseId, StoreId, NeedToPurchaseId, SupplierId, UserId, PurchaseType, taxRate, isTax);
                        m_dbo.ExecuteNonQuery(sql);
                        SId = SupplierId;
                    }
                }
                NeedToPurchase ntp = new NeedToPurchase();
                ntp.AddStatus(NeedToPurchaseId);
                return true;
            }
            return false;
        }

        public bool SetNeedToPurchaseDetail(int ntpId, int BranchId,int StoreId)
        {
                    string sql = string.Format(@"insert into NeedToPurchaseDetail
                                                             (NeedToPurchaseId,
                                                              GoodsId,
                                                              UserId,
                                                               Model,
                                                               Num,
                                                               LackNum,
                                                               InPrice,
                                                               PurchaseType,
                                                               PurchaseMemo,
                                                               CreditCard,
                                                               UpdateTime,
                                                               Emergency,
                                                               SupplierId,
                                                               SprInvoiceType,
                                                               SprTaxRate
                                                               ) 
                                                      select '{0}',
                                                             vod.GoodsId,
                                                             ISNULL( pd.PurchaseUserId,0) as UserId,
                                                             '',
                                                             vod.Num,
                                                             (case WHEN (ISNULL( (gst.Num),0))-(isnull((POD.Pnum),0))+vod.Num<0 THEN 0-vod.Num ELSE (ISNULL( (gst.Num),0))-(isnull((POD.Pnum),0)) end),
                                                             case WHEN  pd.BillsPrice >0 THEN pd.BillsPrice ELSE min(vod.InPrice) end,
                                                             pd.PurchaseType,
                                                             purchaseMemo,
	                                                         '',
	                                                         getdate(),
	                                                         MAX(Emergency) as Emergency ,
	                                                         isnull(pd.SupplierId,0),
	                                                         ISNULL(pd.SprInvoiceType,''),
	                                                         ISNULL(pd.SprTaxRate,0)   
   
                                                        from 
                                                          (select GoodsId,
                                                                  SUM(Num) as Num,
                                                                  InPrice,
                                                                  purchaseMemo,
                                                                  NeedToPurchaseId,
                                                                  IsCalc,
                                                                  DisplayName,
                                                                  Unit,
                                                                  Emergency
                                                            from View_OrderDetailNeedToPurchase 
                                                            where NeedToPurchaseId={0} 
                                                            group by GoodsId,InPrice,purchaseMemo,NeedToPurchaseId,IsCalc,DisplayName,Unit,Emergency
                                                          ) vod left join 
                                                                (select  pd1.GoodsId,
                                                                         PurchaseUserId,
                                                                         (Company+ShortName) AS Company,
                                                                         SupplierId,BillsPrice,
                                                                         PurchaseType,
                                                                         SprInvoiceType,
                                                                         SprTaxRate 
                                                                   from (
                                                                           select GoodsId,
                                                                                  max(Id) as Id
                                                                           from  View_PurchaseDetail 
                                                                           where branchId={1}  and PurchaseType<>'签单退货' and PurchaseType <>'现退单' 
                                                                                 and goodsId in (select goodsId from View_OrderDetailNeedToPurchase where NeedToPurchaseId={0})  
                                                                           group by GoodsId) as pd1 
                                                                           inner join View_PurchaseDetail vpd on pd1.Id=vpd.Id
                                                                 ) pd 
                                                                 on vod.GoodsId=pd.GoodsId 
                                                                 LEFT  JOIN
                                                                       (select goodsId,
                                                                               SUM(Num-picknum) as Pnum 
                                                                          from view_OrderDetail 
                                                                          where  ServiceStatus='已完成'  and storestatus <> '已完成' and OrderType<>'销售退单' and BranchId={1} and IsDelete=0 
                                                                                 and RawOrderId=0 and StartOrder=0 and ComId>=1000 
                                                                          group by GoodsId
                                                                       ) as POD 
                                                                         on vod.GoodsId=POD.GoodsId 
                                                                LEFT  JOIN 
                                                                      (select GoodsId,
                                                                              sum(Num) Num from view_GoodsStore where BranchId={1} and StoreId={2} group by GoodsId   
                                                                      ) as gst 
                                                                      on vod.GoodsId=gst.GoodsId       
                                                    where IsCalc=1 and NeedToPurchaseId={0}   
                                                    group by vod.GoodsId,POD.Pnum,gst.Num,vod.Num, DisplayName,Unit,purchaseMemo,pd.PurchaseUserId,pd.Company,pd.SupplierId,
                                                             pd.BillsPrice,pd.PurchaseType,pd.SprInvoiceType,pd.SprTaxRate 
                                                    order by vod.GoodsId, DisplayName;", ntpId, BranchId, StoreId);

           return m_dbo.ExecuteNonQuery(sql);
        }

        public bool OrderModifyPurchaseSave(DataTable dtNeedToPurchaseDetail, int branchId, int StoreId, string OrderModifyIdTable)
        {
            DataTable dt = dtNeedToPurchaseDetail.DefaultView.ToTable(true, "SupplierId", "UserId", "PurchaseType");
            if (dt.Rows.Count > 0)
            {
                int SId = 0;
                foreach (DataRow drSupplier in dt.Rows)
                {
                    int SupplierId = DBTool.GetIntFromRow(drSupplier, "SupplierId", 0);
                    if (SId == SupplierId && SupplierId != 0)
                    {
                        continue;
                    }
                    int UserId = DBTool.GetIntFromRow(drSupplier, "UserId", 0);
                    string PurchaseType = DBTool.GetStringFromRow(drSupplier, "PurchaseType", "");
                    Purchase p = new Purchase();
                    p.BranchId = branchId;
                    p.NeedToPurchaseId = NeedToPurchaseId;
                    p.PurchaseStatus = CommenClass.PurchaseStatus.未处理.ToString();
                    p.PurchaseType = PurchaseType;
                    p.UserId = 0;
                    p.StoreId = StoreId;
                    p.SupplierId = SupplierId;
                    p.PurchaseUserId = UserId;
                    int purchaseId = p.Save();
                    if (purchaseId > 0)
                    {
                        string sql = string.Format("SupplierId='{0}'", SupplierId);
                        DataRow[] drw = dtNeedToPurchaseDetail.Select(sql);
                        foreach (DataRow dr in drw)
                        {
                            PurchaseDetail pd = new PurchaseDetail();
                            pd.PurchaseId = purchaseId;
                            pd.GoodsId = DBTool.GetIntFromRow(dr, "GoodsId", 0);
                            pd.StoreId = StoreId;
                            pd.Model = "";
                            pd.BillsPrice = DBTool.GetDoubleFromRow(dr, "InPrice", 0);
                            pd.InPrice = DBTool.GetDoubleFromRow(dr, "InPrice", 0);
                            pd.Num = DBTool.GetIntFromRow(dr, "LackNum", 0);
                            pd.Amount = pd.InPrice * pd.Num;
                            pd.Save();
                        }
                        SId = SupplierId;
                        PurchaseManager pm = new PurchaseManager();
                        pm.UpdatePurchaseSumMoney(purchaseId);
                    }
                }
                if (SId > 0)
                {
                    OrderModify om = new OrderModify();
                    return om.UpdatePurchaseType(OrderModifyIdTable);
                }
            }
            return false;
        }
    }
}
