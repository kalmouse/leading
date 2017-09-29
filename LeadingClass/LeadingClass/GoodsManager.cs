using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using CommenClass;
using System.Text;
namespace LeadingClass
{
    public class GoodsOption
    {
        public int Cuxiao { get; set; }
        public int Hot { get; set; }
        public int New { get;set; }
        public int IsVIP { get; set; }
        public int Order { get; set; }
        public string PCode { get; set; }
        public string KeyWords { get; set; }
        public int IsVisible { get; set; }//默认设置为1，只显示可见的商品。如果是系统管理，可以赋值0，显示不可见商品，赋值－1显示所有商品。
        public int TypeId { get; set; }
        public int ComId { get; set; }
        public int BrandId { get; set; }
        public double StartPrice { get; set; }
        public double EndPrice { get; set; }
        public double StartMaolilv { get; set; }
        public double EndMaolilv { get; set; }
        public int NoBarCode { get; set; }
        public int ShortageBarCode { get; set; }
        public int NoPhoto { get; set; }
        public int HomePhoto { get; set; }
        public int DetailPhoto { get; set; }
        public int IsUseRecorControl { get; set; }
        public int Description { get; set; }
        public RecordControl recordContorl { get; set; }
        public int GoodsId { get; set; }
        public int IsAllow { get; set; }//是否通过审核 1：审核通过；0：未审核；-1：全部
        public int IsPublic { get; set; }//是否公有商品
        public int IsShelves { get; set; }//是否上架 默认1：上架商品，0：下架商品；-1：全部
        public int BranchId { get ; set; }//站点Id, 表示是子站点调用
        public int ParentId { get; set; }//父商品Id,单独读取组合商品时使用
        public DateTime AddTime { get; set; }
        public int IsUseSiteGoods { get; set; }//是否启用切换站点更改商品
        public int IsHideParentGoods { get; set; }//是否隐藏母商品 parentid=2
        public string Memo { get; set; }
        public string GoodsIds { get; set; }
        public GoodsOption()
        {
            TypeId = 0;
            IsVisible = 1;
            PCode = "";
            KeyWords = "";
            Order = 0;
            ComId = 0;
            Cuxiao = 0;
            New = 0;
            Hot = 0;
            IsVIP = 0;
            BrandId = 0;
            Description = 0;
            EndPrice = 0;
            StartPrice = 0;
            StartMaolilv = -1;
            EndMaolilv = -1;
            NoPhoto = 0;
            HomePhoto = 0;
            DetailPhoto = 0;
            NoBarCode = 0;
            ShortageBarCode = 0;
            IsUseRecorControl = 0;
            recordContorl = new RecordControl();
            GoodsId = 0;
            IsAllow = -1;
            IsPublic = -1;
            IsShelves = -1;
            BranchId = 0;
            ParentId = 0;//一般情况下读取的是基本商品 q 
            AddTime = new DateTime(1900, 1, 1);
            IsUseSiteGoods = 0;
            IsHideParentGoods = 0;
            Memo = "";
            GoodsIds="0";
        }
    }

    public class GoodsManager
    {
        private DBOperate m_dbo;
        private GoodsOption m_SnapOption;
        private PageModel m_pageModel;
        public GoodsOption SnapOption { get { return m_SnapOption; } set { m_SnapOption = value; } }
        public PageModel pageModel { get { return m_pageModel; } set { m_pageModel = value; } }
        public GoodsManager()
        {
            m_dbo = new DBOperate();
            m_SnapOption = new GoodsOption();
            m_pageModel = new PageModel();
        }
        public GoodsManager(string connectionstring)
        {
            m_dbo = new DBOperate(connectionstring);
            m_SnapOption = new GoodsOption();
            m_pageModel = new PageModel();

        }
        private string GetSQLWhere()
        {
            string sql = "";
            //判断是否只显示可见商品
            if (SnapOption.IsVisible == 1)
            {
                sql += "  and IsVisible=1  ";
            }
            else if (SnapOption.IsVisible == 0)
            {
                sql += "  and IsVisible=0  ";
            }
            else if (SnapOption.IsVisible == 2)//读出非禁用商品
            {
                sql += " and IsVisible >=0 ";
            }
            if (SnapOption.IsHideParentGoods == 1)
            {
                //读取基本商品 和子商品
                sql += " and( parentId =0 or parentId >2) ";
            }
            else
            {
                //只读取基本商品和组合商品
                sql += " and ParentId <=2 ";
            }            
            if (SnapOption.IsAllow > -1)
            {
                sql += string.Format(" and IsAllow={0} ", SnapOption.IsAllow);
            }
            if (SnapOption.IsPublic > -1)
            {
                sql += string.Format(" and IsPublic ={0} ", SnapOption.IsPublic);
            }
            if (SnapOption.IsShelves !=1)
            {
                //只展示上架的商品，未修改以前展示上架下架的商品  update by hjy 2016-10-12
                sql += string.Format(" and IsShelves ={0} ", 1);
            }
            if (SnapOption.IsUseSiteGoods > 0)//开启切换站点更改商品
            {
                if (SnapOption.BranchId >0)
                {
                    sql += string.Format("  and BranchId={0}", SnapOption.BranchId);
                }
            }
            else
            {
                if (SnapOption.BranchId == 1)
                {
                    sql += "  and BranchId=1 ";
                }
                else if (SnapOption.BranchId > 1)
                {
                    sql += string.Format("  and ( BranchId={0} or BranchId=1)", SnapOption.BranchId);
                }
            }
            if (SnapOption.TypeId != 0)
            {
                GoodsType gt = new GoodsType();
                gt.Id = SnapOption.TypeId;
                gt.Load();
                sql += string.Format(" and Code like '{0}%' ", gt.Code);

            }
            if (SnapOption.PCode != "" && SnapOption.PCode != null)
            {
                sql += string.Format(" and Code like '{0}%' ", SnapOption.PCode);
            }
            if (SnapOption.BrandId > 0)
            {
                sql += string.Format(" and BrandId = {0} ", SnapOption.BrandId);
            }

            if (SnapOption.StartPrice >= 0)
            {
                sql += string.Format(" and Price >={0} ", SnapOption.StartPrice);
            }
            if (SnapOption.EndPrice > 0)
            {
                sql += string.Format(" and Price < {0} ", SnapOption.EndPrice);
            }
            if (SnapOption.StartMaolilv > -1)
            {
                sql += string.Format(" and Maolilv >= {0} ", SnapOption.StartMaolilv);
            }
            if (SnapOption.EndMaolilv > -1)
            {
                sql += string.Format(" and Maolilv <= {0} ", SnapOption.EndMaolilv);
            }
            if (SnapOption.NoPhoto == 1)
            {
                sql += " and (HomeImage = '' or PhotoNum = 0 ) ";
            }
            if (SnapOption.ShortageBarCode == 1)
            {
                sql += " and (BarCodeNum = 0 or BarCodeNum < AllBarCodeNum) ";
            }
            else
            {
                if (SnapOption.NoBarCode == 1)
                {
                    sql += " and BarCodeNum = 0 ";
                }
            }
            if (SnapOption.Cuxiao == 1)
            {
                sql += " and Cuxiao>0 ";
            }
            if (SnapOption.Hot == 1)
            {
                sql += " and IsHot =1 ";
            }
            if (SnapOption.New == 1)
            {
                sql += " and IsNew = 1 ";
            }
            if (SnapOption.Memo != "")
            {
                sql += string.Format(" and Memo='{0}'",SnapOption.Memo);
            }
            if (SnapOption.KeyWords != "" && SnapOption.KeyWords != null && SnapOption.KeyWords != "NULL")
            {
                if (SnapOption.KeyWords.Length > 2)
                {
                    SnapOption.KeyWords = CommenClass.StringTools.SplitKeyWords(SnapOption.KeyWords);
                }
                char[] sep = { ' ' };
                string[] kws = SnapOption.KeyWords.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                if (kws.Length == 1)
                {
                    if (kws[0].Length <= 2)//关键字太短的时候，只从 SN和GoodsName中筛选
                    {
                        sql += string.Format(" and ((PY like '%{0}%')  or (DisplayName like '%{0}%')) ", kws[0]);
                    }
                    else
                    {
                        sql += string.Format(" and ((GoodsId  like '%{0}%') or (DisplayName like '%{0}%') or  (PY like '%{0}%')  or (Feature like '%{0}%')) ", kws[0]);
                        //sql += string.Format(" and ((DisplayName like '%{0}%') or  (PY like '%{0}%')  or (Feature like '%{0}%')) ", kws[0]);
                    }
                }
                else
                {
                    for (int i = 0; i < kws.Length; i++)
                    {
                        sql += string.Format(" and ((DisplayName like '%{0}%') or  (PY like '%{0}%')  or (Feature like '%{0}%')) ", kws[i]);
                    }
                }
            }
            return sql;
        }
        private string GetSQLOrderBy()
        {
            string orderStr = " Order by ";
            switch (SnapOption.Order)
            {
                case 0:
                    orderStr += "  Recommend desc,RecommendDate desc , Displayname ";
                    break;
                case 1:
                    orderStr += " Price Asc  ,Recommend desc,Displayname";
                    break;
                case 2:
                    orderStr += " Price Desc ,Recommend desc,Displayname";
                    break;
                case 3:
                    orderStr += " Code ,Recommend desc,Displayname";
                    break;
                case 4:
                    orderStr += " Displayname ";
                    break;
            }
            return orderStr;
        }
        /// <summary>
        /// 核心的一个方法，读取商品
        /// 调用：ERP FGoodsManager.cs
        /// </summary>
        /// <returns></returns>
        public DataSet ReadGoods()
        {
            string sql = "";
            if (SnapOption.IsUseSiteGoods > 0)
                sql = "select * from View_GoodsBranch where 1=1 ";
            else
                sql = "select * from View_Goods where 1=1 ";
            sql += GetSQLWhere();
            sql += GetSQLOrderBy();
            if (SnapOption.IsUseRecorControl == 1)
            {
                return m_dbo.GetDataSet(sql, SnapOption.recordContorl.StartRecord, SnapOption.recordContorl.PageSize);
            }
            else
            {
                return m_dbo.GetDataSet(sql);
            }
        }
        public DataSet Read_GoodsId(int  goodsId, int projectId)
        {
            string sql = string.Format("select * from  View_TPI_GoodsNEW where  GoodsId ={0} and projectId ={1}", goodsId, projectId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取符合条件的商品数量（返回dataset）---------------去掉where 1=1
        /// </summary>
        /// <returns></returns>
        public DataSet ReadGoodsCountDataset()
        {
            string sql="";
            if (SnapOption.IsUseSiteGoods > 0)
                sql = "select count(*) as goodscount from View_GoodsBranch where 1=1 ";
            else
                sql = "select count(*) as goodscount from view_goods where 1=1 ";
            sql += GetSQLWhere();
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 网站查询商品时，返回各类别共有多少相关的商品
        /// </summary>
        /// <returns></returns>
        public DataSet ReadGoodsInAllTypes()
        {
            string sql = "";
            if (SnapOption.IsUseSiteGoods > 0)
                sql = "select TypeName,TypeId,Code,count(Typename) as TypeCount from View_GoodsBranch where 1=1 ";
            else
                sql = "select TypeName,TypeId,Code,count(Typename) as TypeCount from view_goods where 1=1 ";
            sql += GetSQLWhere();
            sql += "group by Code,TypeName,TypeId order by TypeCount desc,Code ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 返回特定查询条件下 各品牌商品的数量
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public DataSet ReadGoodsBrands()
        {
            string sql = "";
            if (SnapOption.IsUseSiteGoods > 0)
                sql = "select BrandName,BrandId,count(BrandName) as BrandCount from View_GoodsBranch where BrandName is not null  and IsShelves=1 and IsVisible =1"; 
            else
                sql = "select BrandName,BrandId,count(BrandName) as BrandCount from view_goods where BrandName is not null  and IsShelves=1 and IsVisible =1"; 
            sql += GetSQLWhere();
            sql += " group by BrandId,BrandName order by BrandCount desc";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取符合条件的商品的数量
        /// </summary>
        /// <returns></returns>
        public int ReadGoodsCountInt()
        {
            DataSet ds = ReadGoodsCountDataset();
            return DBTool.GetIntFromRow(ds.Tables[0].Rows[0], "goodscount", 0);
        }
        ///// <summary>
        ///// 读取购买过此商品的客户还买了;必须是网站可见商品 IsVisible >0;
        ///// </summary>
        ///// <param name="goodsId"></param>
        ///// <returns></returns>
        //public DataSet ReadRelationOfGoods(int goodsId,int topnum)
        //{
        //    string sql = string.Format("select distinct top {1} GoodsId,GoodsName,GoodsPrice,SN,HomeImage,Feature,Unit from View_OrderDetail where OrderId in (select OrderId from OrderDetail where GoodsId = {0}) and GoodsId <> {0} and IsVisible >0 order by GoodsPrice Desc", goodsId, topnum);
        //    DataSet ds = m_dbo.GetDataSet(sql);
        //    if(ds.Tables[0].Rows.Count ==0)
        //    {
        //        ds = m_dbo.GetDataSet(string.Format("select top {0} *,Id as GoodsId,Price as GoodsPrice from Goods order by recommend desc,recommendDate desc ", topnum));
        //    }
        //    return ds;

        //}
        /// <summary>
        /// 读取商品cache数据---                 ---暂定
        /// </summary>
        /// <returns></returns>
        public DataSet ReadCacheGoods(int allVisible, int branchId )
        {
            string sql = string.Format(@"select ID,CONVERT(varchar(10),Id)+' '+ DisplayName as DisplayName,PY,UpdateTime,Recommend ,IsAllow, ParentId ,IsShelves,isnull(t2.Num,0) Num,Memo,GoodsRank,Package,Feature,Price,HomeImage,BrandId,SN,
                                               TypeId,'['+CONVERT(varchar(10),isnull(Recommend,0))+']'+'[存'+CONVERT(varchar(10),isnull(t2.Num,0))+']'+'['+isnull(GoodsRank,'')+']'+ CONVERT(varchar(10),Id)+' '+ DisplayName as FullName
                                               from goods t1
                                                            left join (
	                                                            select GoodsId,Num from GoodsStore,Store
	                                                            where GoodsStore.StoreId=Store.ID and Store.BranchId={0} and Store.IsDefault=1 and GoodsStore.StoreZone<>'收货区'
                                                            ) t2 on(t1.id=t2.GoodsId)
                                               where IsPublic=1 or (IsPublic=0 and BranchId={0})
                                                             ", branchId);
            //allvisible=1 时显示 全部商品（包括禁用商品）
            if (allVisible == 0)
            {
                sql += " and IsVisible>-1 ";
            }
            sql += " order by Num desc ";           
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取最近销售过的商品列表
        /// </summary>
        /// <param name="orderTime"></param>
        /// <returns></returns>
        public DataSet ReadRecentSaleGoods(DateTime startDate,DateTime endDate)
        {
            string sql = string.Format(@" select distinct GoodsId as Id,convert(varchar(10),GoodsId)+' '+DisplayName as DisplayName,PY, ParentId
from View_OrderDetail where OrderTime >='{0}' and OrderTime<'{1}' order by DisplayName ", startDate.Date.ToShortDateString(), endDate.AddDays(1).Date.ToShortDateString());
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 检查SN是否可用
        /// 调用：ERP：FGoods.cs
        /// </summary>
        /// <param name="BrandId">品牌Id</param>
        /// <param name="SN">商品型号</param>
        /// <returns></returns>
        public bool IsSNAvalible(int BrandId,string SN)
        {
            if (SN != "")
            {
                string sql = string.Format(" select * from Goods where SN = '{0}' and BrandId={1} ", SN, BrandId);
                DataSet ds = m_dbo.GetDataSet(sql);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return true;
                }
                else return false;
            }
            else return true;
        }
        /// <summary>
        /// 读取客户--商品的最新销售价格
        /// </summary>
        /// <param name="comId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public double ReadRecComGoodsPrice(int comId, int goodsId)
        {
            string sql = string.Format("select top 1 SalePrice from view_OrderDetail where ComId = {0} and GoodsId = {1} order by PlanDate Desc ", comId, goodsId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return DBTool.GetDoubleFromRow(ds.Tables[0].Rows[0], "SalePrice", 0);
            }
            else return 0;
        }
        /// <summary>
        /// 读取常用的Models
        /// </summary>
        /// <returns></returns>
        public DataSet ReadGoodsModels()
        {
            string sql = "select model,COUNT(Model) as count from Goods  where Model <> '' and Model is not null group by Model order by count desc ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 返回商品详细信息 王鹏亮  2015-7.3
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        //public DataSet ReadGoodsDetails(int goodsId)
        //{
        //    string sql = string.Format("select * from View_Goods where GoodsId={0}", goodsId);
        //    return m_dbo.GetDataSet(sql);
        //}
        /// <summary>
        /// 读取待审核商品
        /// 调用：ERP FGoodsAllow.cs
        /// </summary>
        /// <param name="KeyWords"></param>
        /// <param name="GoodsId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataSet ReadGoodsIsAllow(string KeyWords, int GoodsId, DateTime startDate, DateTime endDate)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(string.Format("select ID,JD_Link,JD_Price,(select City from Sys_Branch where ID =g.BranchId) as Branch,BranchId,DisplayName,PY,Feature,IsVisible,AddTime,price,IsAllow,ParentId,UserId,IsPublic,Description,(select COUNT(GoodsPhoto.GoodsId)  from GoodsPhoto where GoodsPhoto.GoodsId=g.ID) as PhotoNum,(select Name from Sys_Users u where u.Id=g.UserId) Name from goods g where IsVisible>-1 and g.IsAllow<1 and g.IsAllow >-1 and ParentId<=2 and g.AddTime>='{0}' and g.AddTime<'{1}' ", startDate.Date.ToShortDateString(), endDate.AddDays(1).Date.ToShortDateString()));
            if (KeyWords != "")
            {
                if (KeyWords.Length > 2)
                {
                    SnapOption.KeyWords = CommenClass.StringTools.SplitKeyWords(KeyWords);
                }
                char[] sep = { ' ' };
                string[] kws = KeyWords.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                if (kws.Length == 1)
                {
                    if (kws[0].Length <= 2)//关键字太短的时候，只从 SN和GoodsName中筛选
                    {
                        strb.Append(string.Format(" and ((PY like '%{0}%')  or (DisplayName like '%{0}%')) ", kws[0]));
                    }
                    else
                    {
                        strb.Append(string.Format(" and ((DisplayName like '%{0}%') or  (PY like '%{0}%')  or (Feature like '%{0}%')) ", kws[0]));
                    }
                }
                else
                {
                    for (int i = 0; i < kws.Length; i++)
                    {
                        strb.Append(string.Format(" and ((DisplayName like '%{0}%') or  (PY like '%{0}%')  or (Feature like '%{0}%')) ", kws[i]));
                    }
                }
            }
            if (GoodsId > 0)
            {
                strb.Append(string.Format(" and ID={0}", GoodsId));
            }
            return m_dbo.GetDataSet(strb.ToString());
        }
        /// <summary>
        /// 读取待审核商品和被驳回商品
        /// 调用：ERP：FGoodsManager.cs
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet ReadAuditGoods(int BranchId)
        {
            string sql = string.Format(@"select ID,DisplayName,AddTime,UpdateTime,UserId,IsPublic,ParentId,case when IsAllow = -1 then '被驳回' when IsAllow = 0 then '审批中'  else '审核通过' 
                                         end as Allow ,BranchId,ReJectMsg from Goods where BranchId ={0} and ParentId <=2 and IsAllow <>1 ", BranchId);
            return m_dbo.GetDataSet(sql);
        }

        #region 商品变动情况
        /// <summary>
        /// 读取商品销售记录 2014-9-19
        /// </summary>
        public DataSet ReadGoodsSales(int BranchId,int GoodsId,DateTime StartDate,DateTime EndDate)
        {
            string sql = " select * from view_orderDetail where IsInner=0 and IsDelete=0 and OrderType<>'销售退单' ";
            if(BranchId>0)
            {
                sql += string.Format(" and BranchId ={0} ",BranchId);
            }
            if (GoodsId > 0)
            {
                sql += string.Format(" and GoodsId={0} ", GoodsId);
            }
            if (StartDate != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and PlanDate >= '{0}' ", StartDate.ToShortDateString());
            }
            if (EndDate != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and PlanDate < '{0}' ", EndDate.AddDays(1).ToShortDateString());
            }
            sql += " order by OrderId desc ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取商品库存变化记录
        /// </summary>
        public DataSet ReadGoodsStoreChange(int BranchId, int GoodsId, DateTime StartDate, DateTime EndDate)
        {
            string sql = " select * from view_goodsStoreDetail where 1=1 ";
            if (BranchId > 0)
            {
                sql += string.Format(" and BranchId ={0} ", BranchId);
            }
            if (GoodsId > 0)
            {
                sql += string.Format(" and GoodsId={0} ", GoodsId);
            }
            if (StartDate != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and updatetime >= '{0}' ", StartDate.ToShortDateString());
            }
            if (EndDate != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and updatetime < '{0}' ", EndDate.AddDays(1).ToShortDateString());
            }
            sql += " order by StoreId,Model,UpdateTime desc ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取商品调价记录
        /// </summary>
        public DataSet ReadGoodsPriceChange(int BranchId, int GoodsId, DateTime StartDate, DateTime EndDate)
        {
            string sql = " select * from GoodsPrice where 1=1 ";
            if (BranchId > 0)
            {
                sql += string.Format(" and BranchId ={0} ", BranchId);
            }
            if (GoodsId > 0)
            {
                sql += string.Format(" and GoodsId={0} ", GoodsId);
            }
            if (StartDate != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and UpdateTime >= '{0}' ", StartDate.ToShortDateString());
            }
            if (EndDate != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and UpdateTime < '{0}' ", EndDate.AddDays(1).ToShortDateString());
            }
            sql += " order by UpdateTime desc ";
            return m_dbo.GetDataSet(sql);
        }

        #endregion 商品变动情况

        #region GoodsList

        /// <summary>
        /// 读取相应筛选条件的最高最低价(此方法暂时不用)
        /// </summary>
        /// <returns></returns>
        public DataTable ReadGoodsHighLowPrice() 
        {
            string sql = string.Format(" select MAX(Price) as HighPrice ,MIN(Price) as LowPrice from dbo.View_Goods where 1=1 ");
            sql+= GetSQLWhere();
            return m_dbo.GetDataSet(sql).Tables[0];
        }
        /// <summary>
        /// 读取 销售排行
        /// </summary>
        /// <returns></returns>
        public DataSet ReadSalesRank(int topNum)
        {
            string sql = "";
            if (SnapOption.IsUseSiteGoods > 0)
            {
                sql = string.Format(@"select top {0} * from View_GoodsBranch where 1=1 and IsVisible=1 and IsShelves=1 and (ParentId =0 or ParentId=2)", topNum);
                if (SnapOption.BranchId > 1)
                {
                    sql += string.Format(" and BranchId={0}", SnapOption.BranchId);
                }
            }
            else
            {
                sql = string.Format(@"select top {0} * from View_Goods where 1=1 and IsVisible=1 and IsShelves=1 and (ParentId =0 or ParentId=2)", topNum);
            }
            if (SnapOption.TypeId != 0)
            {
                GoodsType gt = new GoodsType();
                gt.Id = SnapOption.TypeId;
                gt.Load();
                sql += string.Format(" and Code like '{0}%' ", gt.Code);
            }
            if (SnapOption.Order == 0)
            {
                sql += " order by SaleNumber desc,SaleCount desc ";
            }
            else
            {
                sql += " order by SaleCount  desc,SaleNumber desc ";
            }
            return m_dbo.GetDataSet(sql);
        }
        #endregion

        #region 商品积分

        /// <summary>
        /// 商品积分的计算 
        /// </summary>
        /// <returns></returns>
        public double GetGoodsPoint(Goods goods)
        {
            double point = 0;
            if (goods.Rate > 0)          //商品的积分计算方法
            {
                point = goods.Price * goods.Rate / 100;
            }
            else
            {
                GoodsType goodstype = new GoodsType();
                if (goodstype.Load(goods.TypeId))
                {
                    double rate = goodstype.Rate;
                    point = goods.Price * rate / 100;
                }
            }
            return point;
        }

        #endregion

        /// <summary>
        /// 首页商品的读取 暂时不用 q 2015-7-16
        /// </summary>
        /// <returns></returns>
        public DataSet ReadGoodsWebSiteShow() 
        {
            string sql = string.Format(@" select * from (select *,(select COUNT(*) from Goods gs where gs. ParentId=g.ID) as c from dbo.Goods g where ParentId=0)  g where  g.c=0 UNION
                                          select * from (select *,ROW_NUMBER() over(partition by parentId order by recommend) as new_index  from dbo.Goods where ParentId !=0  ) GoodsNew where GoodsNew.new_index=1 ");
            if (SnapOption.TypeId != 0)
            {
                GoodsType gt = new GoodsType();
                gt.Id = SnapOption.TypeId;
                gt.Load();
                sql += string.Format(" and Code like '{0}%' ", gt.Code);

            }
            return  m_dbo.GetDataSet(sql);
        }

        #region 组合商品 q 2015-7-18

        /// <summary>
        /// 读取该站点下的组合和商品
        /// </summary>
        /// <param name="SnapOption">ParentId=1，组合商品，IsPublic=1公有商品，IsPublic=0 and BranchId={1}该站点下的独有商品</param>
        /// <returns></returns>
        public DataSet ReadGoodsGroupDetailByBranchId()
        {
            string sql = string.Format(@" select  dbo.Goods.ID as GoodsId ,* from dbo.Goods 
                                        where ParentId ={0}  and ( IsPublic=1 or (IsPublic=0 and BranchId={1}) )                                 
                                         ", SnapOption.ParentId, SnapOption.BranchId);
            if (SnapOption.KeyWords != "" && SnapOption.KeyWords != null && SnapOption.KeyWords != "NULL")
            {
                if (SnapOption.KeyWords.Length > 2)
                {
                    SnapOption.KeyWords = CommenClass.StringTools.SplitKeyWords(SnapOption.KeyWords);
                }
                char[] sep = { ' ' };
                string[] kws = SnapOption.KeyWords.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                if (kws.Length == 1)
                {
                    if (kws[0].Length <= 2)//关键字太短的时候，只从 SN和GoodsName中筛选
                    {
                        sql += string.Format(" and ((PY like '%{0}%')  or (DisplayName like '%{0}%')) ", kws[0]);
                    }
                    else
                    {
                        sql += string.Format(" and ((DisplayName like '%{0}%') or  (PY like '%{0}%')  or (Feature like '%{0}%')) ", kws[0]);
                    }
                }
                else
                {
                    for (int i = 0; i < kws.Length; i++)
                    {
                        sql += string.Format(" and ((DisplayName like '%{0}%') or  (PY like '%{0}%')  or (Feature like '%{0}%')) ", kws[i]);
                    }
                }
            }
            if (SnapOption.GoodsId != 0)
            {
                sql += string.Format(" and ID={0} ", SnapOption.GoodsId);
            }
            sql += "   order by id desc ;";

            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 添加组合商品明细，销售价格
        /// 调用:FGoods.cs
        /// </summary>
        /// <param name="InGoodsId"></param>
        /// <param name="Price"></param>
        /// <param name="Ggdetail"></param>
        /// <returns></returns>
        public int SaveGoodsGroupDetail(int InGoodsId,double Price, GoodsGroupDetail[] Ggdetail)
        {
            int i = 0;
            Goods goods = new Goods();
            goods.ModifyPrice(InGoodsId, Price);
            foreach (GoodsGroupDetail ggd in Ggdetail)
            {
                GoodsGroupDetail ggdetail = new GoodsGroupDetail();
                ggdetail.GoodsGroupId = InGoodsId;//组合商品id
                ggdetail.GoodsId = ggd.GoodsId;// 
                ggdetail.PackageId = ggd.PackageId;
                ggdetail.Num = ggd.Num;
                ggdetail.UserId = ggd.UserId;
                if (ggdetail.Save()>0) 
                {
                    i += 1;
                }
            }
            return i;
        }

        /// <summary>
        /// 修改组合商品明细
        /// </summary>
        /// <param name="InGoodsId"></param>
        /// <param name="Price"></param>
        /// <param name="Ggdetail"></param>
        /// <returns></returns>
        public bool ModifyGoodsGropDetail(int InGoodsId, double Price, GoodsGroupDetail[] Ggdetail)
        {
            Goods goods = new Goods();
            goods.ModifyPrice(InGoodsId, Price);
            if (InGoodsId > 0)
            {
                GoodsGroupDetail ggd = new GoodsGroupDetail();
                DataTable dtOld = ggd.GetGoodsGroupDetail(InGoodsId).Tables[0];//读取订单原来的明细
                //循环新表，处理新增商品和 修改过数量和价格的商品
                for (int i = 0; i < Ggdetail.Length; i++)
                {
                    DataRow[] rows = dtOld.Select(string.Format(" GoodsId={0} ", Ggdetail[i].GoodsId));
                    if (rows.Length == 1)//有这个商品
                    {
                        int oldNum = DBTool.GetIntFromRow(rows[0], "num", 0);
                        if (Ggdetail[i].Num != oldNum)//有变化需要修改
                        {
                            GoodsGroupDetail ggdl = new GoodsGroupDetail();
                            ggdl.Id = DBTool.GetIntFromRow(rows[0], "Id", 0);
                            ggdl.Load();
                            ggdl.Num = Ggdetail[i].Num;
                            ggdl.UserId = Ggdetail[i].UserId;
                            if (ggdl.Save() > 0)
                            {
                                //记录修改明细记录

                            }
                        }
                    }
                    else //新增商品
                    {
                        GoodsGroupDetail ggdl = new GoodsGroupDetail();
                        ggdl.Id = 0;
                        ggdl.GoodsGroupId = InGoodsId;
                        ggdl.GoodsId = Ggdetail[i].GoodsId;
                        ggdl.PackageId = 0;
                        ggdl.Num = Ggdetail[i].Num;
                        ggdl.UserId = Ggdetail[i].UserId;
                        if (ggdl.Save() > 0)
                        {
                            //如果是组合商品，需要新增 组合的明细

                        }
                    }

                    //循环旧表，查找新表中没有的项。删除，记录明细
                    foreach (DataRow row in dtOld.Select())
                    {
                        int goodsId = DBTool.GetIntFromRow(row, "goodsId", 0);
                        bool isExsist = false;
                        for (int z = 0; z < Ggdetail.Length; i++)
                        {
                            if (Ggdetail[z].GoodsId == goodsId)
                            {
                                isExsist = true;
                                break;
                            }
                        }
                        if (isExsist == false)
                        {
                            //新订单中无 此项
                            int odId = DBTool.GetIntFromRow(row, "Id", 0);
                            GoodsGroupDetail ggdl = new GoodsGroupDetail();
                            ggdl.Id = odId;
                            ggdl.GoodsId = goodsId;
                            if (ggdl.Delete())//删除订单明细 如果是组合商品 删除
                            {
                                //记录订单 修改记录

                            }
                        }
                    }
                }
            }
            return true;
        }
        #endregion

        /// <summary>
        /// 获取VIP付费客户的商品专区（推荐度为6的商品）
        /// </summary>
        /// <returns></returns>
        public DataSet GetVIPPayGoodsList()
        {
            string sql = "select top 100 * from Goods where Recommend = 6";
            return m_dbo.GetDataSet(sql);
        }
        public int ReadVIPGoodsCount()
        {
            string sql = "select count(*) as GoodsCount from Goods where Recommend =6";
            DataSet ds = m_dbo.GetDataSet(sql);
            return DBTool.GetIntFromRow(ds.Tables[0].Rows[0], "GoodsCount", 0);
        }
        /// <summary>
        /// 读取商品设置
        ///  调用：ERP FGoodsAllow.cs
        /// </summary>
        /// <returns></returns>
        public DataSet ReadGoodsSet()
        {
            StringBuilder stb = new StringBuilder();
            stb.Append(@"select * from (select ID,DisplayName,PY,Feature,Unit,Price,InPrice,IsShelves,TypeId,SN,case when IsVisible=0 then '平台' when IsVisible=1 then '网站' else '作废' END as IsVisible
                          ,IsPublic,(select Name from Sys_Branch where Id=g.BranchId) as branchName,branchId,(select Name from Sys_Users where Sys_Users.Id=g.UserId) as userName,
                          UserId,AddTime,PhotoNum,case when Description <>'' then 1 else 0 end as  Description,(select COUNT(GoodsId) from GoodsPhoto gp 
                          where gp.GoodsId=g.ID and (gp.IsDetailPhoto <1 or gp.IsHomeImage >0))as 主图数 ,(select COUNT(GoodsId) from GoodsPhoto gp where gp.GoodsId=g.ID and gp.IsDetailPhoto =1)as 详图数
                          from Goods g )tt where 1=1");
            if (SnapOption.IsVisible == 1)
            {
                stb.Append(" and tt.IsVisible='网站' ");
            }
            else if (SnapOption.IsVisible == 0)
            {
                stb.Append(" and tt.IsVisible='后台' ");
            }
            else if (SnapOption.IsVisible == 2)//读出非禁用商品
            {
                stb.Append(" and tt.IsVisible <> '作废' ");
            }
            //只读取基本商品和组合商品
            //sql += " and ParentId <=2 ";

            if (SnapOption.IsAllow > -1)
            {
                stb.Append(string.Format(" and tt.IsAllow={0} ", SnapOption.IsAllow));
            }
            if (SnapOption.IsPublic > -1)
            {
                stb.Append(string.Format(" and tt.IsPublic ={0} ", SnapOption.IsPublic));
            }
            if (SnapOption.IsShelves > -1)
            {
                stb.Append(string.Format(" and tt.IsShelves ={0} ", SnapOption.IsShelves));
            }
            if (SnapOption.BranchId > 0)
            {
                stb.Append(string.Format("  and tt.BranchId={0} ", SnapOption.BranchId));
            }
            if (0 < SnapOption.HomePhoto && SnapOption.HomePhoto <= 5)
            {
                stb.Append(string.Format(" and 主图数 = '{0}' ", SnapOption.HomePhoto));
            }
            else if (SnapOption.HomePhoto > 5)
            {
                stb.Append(string.Format(" and 主图数 >= '{0}' ", SnapOption.HomePhoto));
            }
            if (0 < SnapOption.DetailPhoto && SnapOption.DetailPhoto <= 5)
            {
                stb.Append(string.Format(" and 详图数 = '{0}' ", SnapOption.DetailPhoto));
            }
            else if (SnapOption.DetailPhoto > 5)
            {
                stb.Append(string.Format(" and 详图数 >= '{0}' ", SnapOption.DetailPhoto));
            }
            if (SnapOption.Description == 0)
            {
                stb.Append(" and tt.Description =1 ");
            }
            else if (SnapOption.Description == 1)
            {
                stb.Append(" and tt.Description =0 ");
            }
            if (SnapOption.GoodsId > 0)
            {
                stb.Append(string.Format(" and tt.Id={0} ", SnapOption.GoodsId));
            }
            if (SnapOption.GoodsIds!=""&&SnapOption.GoodsIds!="0")
            {
                stb.Append(string.Format(" and tt.Id in ({0}) ", SnapOption.GoodsIds));
            }
            if (SnapOption.KeyWords != "" && SnapOption.KeyWords != null && SnapOption.KeyWords != "NULL")
            {
                if (SnapOption.KeyWords.Length > 2)
                {
                    SnapOption.KeyWords = CommenClass.StringTools.SplitKeyWords(SnapOption.KeyWords);
                }
                char[] sep = { ' ' };
                string[] kws = SnapOption.KeyWords.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                if (kws.Length == 1)
                {
                    if (kws[0].Length <= 2)//关键字太短的时候，只从 SN和GoodsName中筛选
                    {
                        stb.Append(string.Format(" and ((tt.PY like '%{0}%')  or (tt.DisplayName like '%{0}%')) ", kws[0]));
                    }
                    else
                    {
                        stb.Append(string.Format(" and ((tt.DisplayName like '%{0}%') or  (tt.PY like '%{0}%')  or (tt.Feature like '%{0}%')) ", kws[0]));
                    }
                }
                else
                {
                    for (int i = 0; i < kws.Length; i++)
                    {
                        stb.Append(string.Format(" and ((tt.DisplayName like '%{0}%') or  (tt.PY like '%{0}%')  or (tt.Feature like '%{0}%')) ", kws[i]));
                    }
                }
            }
            return m_dbo.GetDataSet(stb.ToString());
        }
        //add by fulinlin
        //读取商品总数
        public DataSet GetCountNum(string where)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" select COUNT(ID) as countnum from dbo.Goods ");
            sql.AppendFormat(" where {0} ",where);
            return m_dbo.GetDataSet(sql.ToString());
        }
        //分页读取商品
        public DataSet GetGoodsByPage(int PageSize, int PageNum, int PageCount, string where)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from (select *,ROW_NUMBER() over(order by ID) as rownumber, (select COUNT(GoodsId) from GoodsPhoto where dbo.Goods.Id=GoodsPhoto.GoodsId) as PhotoNums from  dbo.Goods");
            sql.AppendFormat(" where {0}", where);
            sql.AppendFormat(" )t where 1=1 ");
            if (PageNum != PageCount)
            {
                if (PageNum == 1)
                {
                    sql.AppendFormat(" and t.rownumber between 1 and {0}*{1} ", PageSize, PageNum);
                }
                else
                {
                    sql.AppendFormat(" and t.rownumber between {0}*({1}-1)+1 and {0}*{1} ", PageSize, PageNum);
                }
            }
            else
            {
                sql.AppendFormat(" and t.rownumber>({0}*({1}-1)) ", PageSize, PageNum);
            }
            return m_dbo.GetDataSet(sql.ToString());
        }
        //网站后台管理使用
        public DataSet GetCountNum(string LeiBie, string Name,int BranchId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" ((BranchId={0} and IsPublic=0 and IsShelves=1 and ParentId<>1) or (IsPublic=1 and IsShelves=1 and ParentId<>1)) and ID not in(select GoodsId from View_GoodsBranch where BranchId={0}) ", BranchId);
            if (Name != "" && Name != null)
            {
                sql.AppendFormat(" and (DisplayName like '%{0}%' ", Name);
                int nam = 0;
                Int32.TryParse(Name, out nam);
                if (nam > 0)
                {
                    sql.AppendFormat("  or ID={0} ", Name);
                }
                sql.AppendFormat(" ) ");
            }
                if (LeiBie != "-1")
                {
                    sql.AppendFormat(" and TypeId in({0}) ", LeiBie);
                }

                return GetCountNum(sql.ToString());
        }
        
        //网站后台管理使用
        public DataSet GetGoodsByPage(int PageSize, int PageNum, string LeiBie, string Name,int BranchId)
        {
            int pageCount = 0;
            int count = 0;
            DataTable dt = GetCountNum(LeiBie, Name,BranchId).Tables[0];
            Int32.TryParse(dt.Rows[0]["countnum"].ToString(), out count);
            if (PageSize > 0)
            {
                double d = Convert.ToDouble(count) / Convert.ToDouble(PageSize);
                pageCount = Convert.ToInt32(Math.Ceiling(d));
            }
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" ((BranchId={0} and IsPublic=0 and IsShelves=1 and ParentId<>1) or (IsPublic=1 and IsShelves=1 and ParentId<>1)) and ID not in(select GoodsId from View_GoodsBranch where BranchId={0}) ", BranchId);
            if (Name != "" && Name != null)
            {
                sql.AppendFormat(" and (DisplayName like '%{0}%' ", Name);
                int nam = 0;
                Int32.TryParse(Name, out nam);
                if (nam > 0)
                {
                    sql.AppendFormat("  or ID={0} ", Name);
                }
                sql.AppendFormat(" ) ");
            }
                if (LeiBie != "-1")
                {
                    sql.AppendFormat(" and TypeId in ({0}) ", LeiBie);
                }
            return GetGoodsByPage(PageSize,PageNum,pageCount,sql.ToString());
        }

        /// <summary>
        /// 获取专柜用户常购商品数据
        /// add cml2070628
        /// </summary>
        /// <param name="memberid"></param>
        /// <returns></returns>
        public DataSet GetAlwaysBuyData(int memberid)
        {
            DataSet ds = new DataSet();
            string sqlStr = string.Format(@"select GoodsId,DisplayName,Unit, dbo.Goods.Price,dbo.Goods.HomeImage,package, count(num) as BuyNum, sum                        (num) as Count,SUM(amount) as SumMoney ,[Order].DeptId from dbo.[Order]  join dbo.OrderDetail on dbo.[Order].Id=dbo.OrderDetail.OrderId join dbo.Member  on dbo.[Order].MemberId =dbo.Member.Id join dbo.Goods on dbo.OrderDetail.GoodsId=dbo.Goods.ID where  dbo.[Order].ComId =(select ComId from dbo.Member where Member.Id={0}) group by GoodsId,DisplayName,Unit,dbo.Goods.Price,dbo.Goods.HomeImage,package ,[Order].DeptId
        order by BuyNum desc,SumMoney desc ", memberid);
            if (m_dbo.GetDataSet(sqlStr) != null)
            {
                ds = m_dbo.GetDataSet(sqlStr);
            }
            return ds;
        }
    }
}