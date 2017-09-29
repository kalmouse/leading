using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using LeadingClass;

namespace LeadingClass
{
    public class GoodsStore
    {
        public int Id { get; set; }
        public int GoodsId { get; set; }
        public int StoreId { get; set; }
        public int Num { get; set; }
        public double AC { get; set; }
        public DateTime UpdateTime { get; set; }
        public string StoreZone { get; set; }
        public int CargoNum { get; set; }
        public double TaxAC { get; set; }
        private DBOperate m_dbo;

        public GoodsStore()
        {
            Id = 0;
            GoodsId = 0;
            StoreId = 0;
            Num = 0;
            AC = 0;
            UpdateTime = DateTime.Now;
            StoreZone = "";
            CargoNum = 0;
            TaxAC = 0;
            m_dbo = new DBOperate();
        }
        public GoodsStore(int Id)
            : this()
        {
            this.Id = Id;
            this.Load();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@GoodsId", GoodsId));
            arrayList.Add(new SqlParameter("@StoreId", StoreId));
            arrayList.Add(new SqlParameter("@Num", Num));
            arrayList.Add(new SqlParameter("@AC", AC));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@StoreZone", StoreZone));
            arrayList.Add(new SqlParameter("@CargoNum", CargoNum));
            arrayList.Add(new SqlParameter("@TaxAC", TaxAC));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("GoodsStore", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("GoodsStore", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from GoodsStore where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from GoodsStore where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        public bool Load(int StoreId, int GoodsId)
        {
            string sql = string.Format("select * from GoodsStore where StoreId={0} and GoodsId={1} and StoreZone<>'收货区'", StoreId, GoodsId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;

        }

        /// <summary>
        /// 按照库位查询
        /// </summary>
        /// <param name="StoreId"></param>
        /// <param name="GoodsId"></param>
        /// <param name="StoreZone">库位</param>
        /// <returns></returns>
        public bool Load(int StoreId, int GoodsId, string StoreZone)
        {
            string sql = string.Format("select * from GoodsStore where StoreId={0} and GoodsId={1} and StoreZone='{2}'", StoreId, GoodsId, StoreZone);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 查询商品库存,返回收货区和货架号上最大的AC、以及数量总和
        /// </summary>
        /// <param name="StoreId"></param>
        /// <param name="GoodsId"></param>
        /// <returns></returns>
        public DataSet GetGoodsStore(int goodsId, int storeId)
        {
            string sql = string.Format("select GoodsId, MAX(ac) AC,SUM(Num) Num from GoodsStore where GoodsId ={0} and StoreId={1} and Num>0  group by GoodsId ", goodsId, storeId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取商品库存明细
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public DataSet GetGoodsStoreDetail(DataTable dt, int storeId)
        {
            string sql = "select * from GoodsStore where 1=1 ";
            if (dt.Rows.Count > 0)
            {
                string ids = " and GoodsId in ( ";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ids += dt.Rows[i]["GoodsId"].ToString();
                    if (i < dt.Rows.Count - 1)
                    {
                        ids += ",";
                    }
                }
                ids += " ) ";
                sql += ids;
            }
            sql += string.Format("and StoreId={0}  and Num<>0 Order by Id", storeId);
            return m_dbo.GetDataSet(sql);
        }
        public bool UpdateGoodsAc(double ac, int storeId, int goodsId)
        {
            string sql = string.Format("update GoodsStore set AC={0} where StoreId={1} and GoodsId={2}", ac, storeId, goodsId);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 读取仓库待上架商品
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public DataSet ReadNeedToPutAway(int storeId, int userId)
        {
            string sql = "";
            if (userId == 77)
            {
                sql = string.Format(@"select gs.Id,(gs.Num-CargoNum) as StoreNum,gs.StoreId,gs.StoreZone,gs.GoodsId,g.DisplayName,CargoNum,g.Unit,s.Name
from GoodsStore  gs inner join goods g on gs.GoodsId =g.ID inner join Store  s on gs.StoreId=s.ID  
 where StoreId in({0},2) and CargoNum<>0 
order by gs.StoreZone", storeId);
            }
            else
            {
                sql = string.Format(@"select gs.Id,(gs.Num-CargoNum) as StoreNum,gs.StoreId,gs.StoreZone,gs.GoodsId,g.DisplayName,CargoNum,g.Unit,s.Name
from GoodsStore  gs inner join goods g on gs.GoodsId =g.ID  inner join Store  s on gs.StoreId=s.ID 
 where StoreId in({0}) and CargoNum<>0 
order by gs.StoreZone", storeId);
            }
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取商品库位码
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="GoodsId"></param>
        /// <param name="StoreZone"></param>
        /// <returns></returns>
        public DataSet ReadGoodsStoreDetai(int storeId, int GoodsId, string GoodsName, string StoreZone)
        {
            string sql = string.Format("select * from View_GoodsStore where StoreId={0} ", storeId);
            if (GoodsId > 0)
            {
                sql += string.Format(" and GoodsId={0} ", GoodsId);
            }
            if (GoodsName != "")
            {
                sql += string.Format(" and DisplayName like'%{0}%'", GoodsName);
            }
            if (StoreZone != "")
            {
                sql += string.Format(" and StoreZone  like '%{0}%'", StoreZone);
            }
            return m_dbo.GetDataSet(sql);
        }


        /// <summary>
        /// 读取商品的库存位置
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public bool PutAway(int goodsId, int storeId)
        {
            string sql = string.Format("select * from GoodsStore where GoodsId={0} and StoreId= {1} and StoreZone!='收货区'", goodsId, storeId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }

        private void LoadFromRow(DataRow row)
        {
            Id = DBTool.GetIntFromRow(row, "Id", 0);
            GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
            StoreId = DBTool.GetIntFromRow(row, "StoreId", 0);
            Num = DBTool.GetIntFromRow(row, "Num", 0);
            AC = DBTool.GetDoubleFromRow(row, "AC", 0);
            UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
            StoreZone = DBTool.GetStringFromRow(row, "StoreZone", "");
            CargoNum = DBTool.GetIntFromRow(row, "CargoNum", 0);
            TaxAC = DBTool.GetDoubleFromRow(row, "TaxAC", 0);
        }

        public DataSet GetGoodsStore(int storeId)
        {
            string sql = string.Format("select GoodsId,DisplayName from View_GoodsStore where StoreId={0}", storeId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// Solr生成库存商品索引
        /// </summary>
        /// <returns></returns>
        public DataSet SearchGoodsStoreGenerateSolr()
        {
            string sql = string.Format("select StoreId,GoodsId,DisPlayName from View_GoodsStore");
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 采购入库计算AC
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="GoodsId"></param>
        /// <param name="Pnum"></param>
        /// <param name="PInPrice"></param>
        /// <returns></returns>
        public double GetAC(int storeId, int goodsId, int pNum, double pInPrice)
        {
            DataTable dt = GetGoodsStore(goodsId, storeId).Tables[0];
            double AC = 0;
            if (dt.Rows.Count > 0)
            {
                double kAc = DBTool.GetDoubleFromRow(dt.Rows[0], "AC", 0);
                int kNum = DBTool.GetIntFromRow(dt.Rows[0], "Num", 0);
                AC = (kAc * kNum + pNum * pInPrice) / (kNum + pNum);
            }
            else
            {
                AC = pInPrice;
            }
            return AC;
        }

        #region 新版入库方式
        /// <summary>
        /// 计算Ac
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="storeId"></param>
        /// <param name="goodsId"></param>
        /// <param name="pNum"></param>
        /// <param name="pInPrice"></param>
        /// <returns></returns>
        public double GetAc(int branchId, int storeId, int goodsId, int pNum, double pInPrice)
        {
            double Ac = 0;
            GoodsStore gs = new GoodsStore();
            if (gs.GetGoodsStorePosition(branchId, storeId, goodsId))
            {
                Ac = (gs.AC * gs.Num + pNum * pInPrice) / (gs.Num + pNum);
            }
            else
            {
                Ac = pInPrice;
            }
            return Ac;
        }
        /// <summary>
        /// 采购单入库,支持批量操作,PDA调用
        /// </summary>
        /// <param name="purchaseArr">采购单id集合</param>
        /// <param name="branchId">branchId</param>
        /// <param name="userId">操作人id</param>
        /// <returns></returns>
        public bool SetGoodsStore(int[] purchaseArr, int branchId, int userId)
        {
            bool result = false;
            //首先判断集合中是否有值
            if (purchaseArr.Length > 0)
            {
                int count = 0;
                for (int i = 0; i < purchaseArr.Length; i++)
                {
                    int id = purchaseArr[i];
                    Purchase purchase = new Purchase(id);
                    if (purchase.PurchaseStatus == CommenClass.PurchaseStatus.已入库.ToString())
                    {
                        continue;
                    }
                    else
                    {
                        PurchaseManager pm = new PurchaseManager();
                        if (PurchaseDetailSave(pm.ReadPurchaseDetail(id), branchId, userId))
                        {
                            purchase.PurchaseStatus = CommenClass.PurchaseStatus.已入库.ToString();
                            purchase.UpdateTime = DateTime.Now;                           
                            purchase.UserId = userId;
                            if (purchase.Save() > 0)
                            {
                                count++;
                            }
                        }
                    }
                }
                if (count == purchaseArr.Length)
                {
                    result = true;
                }
            }
            return result;
        }
        ///<summary>
        ///采购单明细入库库存数量操作
        ///</summary>
        ///<returns></returns>
        private bool PurchaseDetailSave(DataSet pdset, int branchId, int userId)
        {
            bool result = false;
            int count = 0;
            for (int i = 0; i < pdset.Tables[0].Rows.Count; i++)
            {
                int storeId = Convert.ToInt32(pdset.Tables[0].Rows[i]["StoreId"]);
                int goodsId = Convert.ToInt32(pdset.Tables[0].Rows[i]["GoodsId"]);
                int num = Convert.ToInt32(pdset.Tables[0].Rows[i]["Num"]);
                string purchaseType = pdset.Tables[0].Rows[i]["PurchaseType"].ToString();
                double inPrice = Convert.ToDouble(pdset.Tables[0].Rows[i]["InPrice"]);
                double taxInPrice = Convert.ToDouble(pdset.Tables[0].Rows[i]["TaxInPrice"]);
                int purchaseId = Convert.ToInt32(pdset.Tables[0].Rows[i]["PurchaseId"]);
                GoodsStore gs = new GoodsStore();
                //读取商品库存位置，有库存说明之前采购过，此时入库需要加权平均；否则不需加权平均
                if (gs.Load(storeId, goodsId))
                {
                    if (inPrice != 0 && num != 0)   //inPrice=0:赠品；num=0:错误数据
                    {
                        if (gs.Num + num != 0)  //防止gs.Num为负值（错误数据）
                        {
                            gs.AC = (gs.AC * gs.Num + num * inPrice) / (gs.Num + num);
                            gs.TaxAC = (gs.TaxAC * gs.Num + num * taxInPrice) / (gs.Num + num);
                        }
                        else
                        {
                            gs.AC = inPrice;
                            gs.TaxAC = taxInPrice;
                        }
                    }
                    gs.Num += num;
                    gs.CargoNum += num;
                    gs.UpdateTime = DateTime.Now;
                }
                else
                {
                    gs.CargoNum = num;
                    gs.GoodsId = goodsId;
                    gs.StoreId = storeId;
                    gs.Num = num;
                    gs.UpdateTime = DateTime.Now;
                    gs.AC = inPrice;
                    gs.TaxAC = taxInPrice;
                }
                if (gs.Save() > 0)
                {
                    //记录商品库存变化明细
                    GoodsStoreDetail gsd = new GoodsStoreDetail();
                    gsd.GoodsId = goodsId;
                    gsd.OldNum = gs.Num - num;
                    gsd.Num = num;
                    gsd.NewNum = gs.Num;
                    gsd.Operate = "PDA商品入库";
                    gsd.RelationId = purchaseId;
                    gsd.StoreId = storeId;
                    gsd.UpdateTime = DateTime.Now;
                    gsd.UserId = userId;
                    gsd.AC = gs.AC;
                    gsd.TaxAC = gs.TaxAC;
                    gsd.Save();
                    count++;
                }
            }
            if (count == pdset.Tables[0].Rows.Count)
            {
                result = true;
            }
            return result;
        }


        /// <summary>
        /// 读取商品库存位置
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="storeId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public bool GetGoodsStorePosition(int branchId, int storeId, int goodsId)
        {
            DBOperate m_dbo = new DBOperate();
            //string sql = string.Format("select * from View_GoodsStore where BranchId={0} and StoreId={1} and GoodsId={2} and StoreZone<>'收货区'", branchId, storeId, goodsId);

            string sql = string.Format(@"
        SELECT     GoodsStore.TaxAC  ,   dbo.GoodsStore.GoodsId, dbo.GoodsStore.StoreId, dbo.Store.BranchId, dbo.Store.Name, dbo.Store.IsAvalible, dbo.GoodsStore.Num, dbo.GoodsStore.AC, dbo.Goods.DisplayName, dbo.Goods.PY, dbo.Goods.Unit, dbo.Goods.InPrice, dbo.Goods.TypeId, dbo.GoodsType.TypeName, dbo.GoodsType.Code, dbo.Goods.IsPublic, dbo.Goods.ParentId, dbo.Store.IsDefault, 
        dbo.GoodsStore.UpdateTime, dbo.GoodsStore.Id, dbo.Goods.GoodsRank, dbo.Goods.Memo, dbo.GoodsStore.StoreZone, dbo.GoodsStore.CargoNum
        FROM         dbo.Goods INNER JOIN
        dbo.GoodsStore ON dbo.Goods.ID = dbo.GoodsStore.GoodsId INNER JOIN
        dbo.Store ON dbo.GoodsStore.StoreId = dbo.Store.ID INNER JOIN
        dbo.GoodsType ON dbo.Goods.TypeId = dbo.GoodsType.ID
        where Store.BranchId={0} and StoreId={1} and GoodsId={2} and StoreZone<>'收货区'", branchId, storeId, goodsId);

            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }
        #endregion
    }


    public class GoodsStoreDetail
    {
        private int m_Id;
        private int m_StoreId;
        private int m_GoodsId;
        private string m_Model;
        private int m_OldNum;
        private int m_Num;
        private int m_NewNum;
        private string m_Operate;
        private int m_RelationId;
        private string m_Memo;
        private int m_UserId;
        private double m_AC;
        
        private DateTime m_UpdateTime;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int StoreId { get { return m_StoreId; } set { m_StoreId = value; } }
        public int GoodsId { get { return m_GoodsId; } set { m_GoodsId = value; } }
        public string Model { get { return m_Model; } set { m_Model = value; } }
        public int OldNum { get { return m_OldNum; } set { m_OldNum = value; } }
        public int Num { get { return m_Num; } set { m_Num = value; } }
        public int NewNum { get { return m_NewNum; } set { m_NewNum = value; } }
        public string Operate { get { return m_Operate; } set { m_Operate = value; } }
        public int RelationId { get { return m_RelationId; } set { m_RelationId = value; } }
        public string Memo { get { return m_Memo; } set { m_Memo = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public double AC { get { return m_AC; } set { m_AC = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public double TaxAC { get; set; }
        public GoodsStoreDetail()
        {
            m_Id = 0;
            m_StoreId = 0;
            m_GoodsId = 0;
            m_Model = "";
            m_OldNum = 0;
            m_Num = 0;
            m_NewNum = 0;
            m_Operate = "";
            m_RelationId = 0;
            m_Memo = "";
            m_UserId = 0;
            m_AC = 0;
            TaxAC = 0;
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
            arrayList.Add(new SqlParameter("@StoreId", m_StoreId));
            arrayList.Add(new SqlParameter("@GoodsId", m_GoodsId));
            arrayList.Add(new SqlParameter("@Model", m_Model));
            arrayList.Add(new SqlParameter("@OldNum", m_OldNum));
            arrayList.Add(new SqlParameter("@Num", m_Num));
            arrayList.Add(new SqlParameter("@NewNum", m_NewNum));
            arrayList.Add(new SqlParameter("@Operate", m_Operate));
            arrayList.Add(new SqlParameter("@RelationId", m_RelationId));
            arrayList.Add(new SqlParameter("@Memo", m_Memo));
            arrayList.Add(new SqlParameter("@UserId", m_UserId));
            arrayList.Add(new SqlParameter("@AC", m_AC));
            arrayList.Add(new SqlParameter("@TaxAC", TaxAC));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("GoodsStoreDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("GoodsStoreDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from GoodsStoreDetail where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_StoreId = DBTool.GetIntFromRow(row, "StoreId", 0);
                m_GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                m_Model = DBTool.GetStringFromRow(row, "Model", "");
                m_OldNum = DBTool.GetIntFromRow(row, "OldNum", 0);
                m_Num = DBTool.GetIntFromRow(row, "Num", 0);
                m_NewNum = DBTool.GetIntFromRow(row, "NewNum", 0);
                m_Operate = DBTool.GetStringFromRow(row, "Operate", "");
                m_RelationId = DBTool.GetIntFromRow(row, "RelationId", 0);
                m_Memo = DBTool.GetStringFromRow(row, "Memo", "");
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_AC = DBTool.GetDoubleFromRow(row, "AC", 0);
                TaxAC = DBTool.GetDoubleFromRow(row, "TaxAC", 0);
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }

    }

    public class GoodsStoreManager
    {
        public GoodsStoreManager()
        {

        }
        /// <summary>
        /// 读取商品库存数量
        /// </summary>
        /// <param name="StoreId"></param>
        /// <param name="GoodsId"></param>
        /// <param name="Model"></param>
        /// <returns></returns>
        public int GetGoodsStockNum(int StoreId, int GoodsId, string Model)
        {
            GoodsStore gs = new GoodsStore();
            gs.Load(StoreId, GoodsId);
            return gs.Num;
        }
        public int GetGoodsStockNum(int Id)
        {
            GoodsStore gs = new GoodsStore(Id);
            return gs.Num;
        }
        /// <summary>
        /// 出库一种商品,返回其 最新的成本价
        /// </summary>
        /// <param name="gsd1"></param>
        /// <returns></returns>
        public double OutBoundGoods(GoodsStoreDetail gsd1)
        {
            GoodsStore gs = new GoodsStore();
            if (!gs.Load(gsd1.StoreId, gsd1.GoodsId))//商品不在库存中时需要添加此商品
            {
                gs.GoodsId = gsd1.GoodsId;
                gs.StoreId = gsd1.StoreId;
                //gs.Model = gsd1.Model;
                gs.Num = 0;
                int Id = gs.Save();
                gs.Id = Id;
            }
            gs.Num = gs.Num - gsd1.Num;
            gs.UpdateTime = DateTime.Now;
            if (gs.Save() <= 0)
            {
                return -1;
            }
            GoodsStoreDetail gsd = new GoodsStoreDetail();
            gsd.GoodsId = gsd1.GoodsId;
            gsd.Id = 0;
            gsd.NewNum = gs.Num;
            gsd.Num = gsd1.Num;
            gsd.OldNum = gs.Num + gsd1.Num;
            gsd.Operate = gsd1.Operate;
            gsd.RelationId = gsd1.RelationId;
            gsd.StoreId = gsd1.StoreId;
            gsd.UpdateTime = DateTime.Now;
            gsd.UserId = gsd1.UserId;
            gsd.AC = gs.AC;
            gsd.TaxAC = gs.TaxAC;
            if (gsd.Save() > 0)
            {
                return gs.AC;
            }
            else return -1;
        }
        //根据goodsId读取商品库存.
        /// <summary>
        /// 系统接口用的
        /// </summary>
        /// <param name="GoodsId"></param>
        /// <returns></returns>
        public DataSet ReadGoodsStore(int[] GoodsId, int ProjectId)
        {
            DBOperate m_dbo = new DBOperate();
            string sql = string.Format(@"select Sku,TPI_Goods.GoodsId,ISNULL(SUM( Num),0) as Num,ProjectId from TPI_Goods left join 
            GoodsStore on GoodsStore.GoodsId=TPI_Goods.GoodsId  where 1=1");
            if (GoodsId != null)
            {
                sql += " and TPI_Goods.GoodsId in ( ";
                for (int i = 0; i < GoodsId.Length; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", GoodsId[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", GoodsId[i]);
                }
                sql += " ) ";
            }
            if (ProjectId > 0)
            {
                sql += string.Format(" and ProjectId={0}", ProjectId);
            }
            sql += " group by Sku,TPI_Goods.GoodsId,ProjectId;";
            return m_dbo.GetDataSet(sql);
        }
    }

    //盘点功能新加表 
    public class GoodStoreInventory
    {
        public int ID { get; set; }
        public int StoreId { get; set; }
        public string Title { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreatePerson { get; set; }
        public DateTime UpdateTime { get; set; }
        public int UpdatePerson { get; set; }
        public string Memo { get; set; }
        public int BranchId { get; set; }
        private DBOperate m_dbo;

        public GoodStoreInventory()
        {
            ID = 0;
            StoreId = 0;
            Title = "";
            CreateTime = DateTime.Now;
            CreatePerson = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (ID > 0)
            {
                arrayList.Add(new SqlParameter("@ID", ID));
            }
            arrayList.Add(new SqlParameter("@StoreId", StoreId));
            arrayList.Add(new SqlParameter("@Title", Title));
            arrayList.Add(new SqlParameter("@CreateTime", CreateTime));
            arrayList.Add(new SqlParameter("@CreatePerson", CreatePerson));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@UpdatePerson", UpdatePerson));
            arrayList.Add(new SqlParameter("@Memo", Memo));
            arrayList.Add(new SqlParameter("@BranchId", BranchId));

            if (this.ID > 0)
            {
                m_dbo.UpdateData("GoodStoreInventory", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.ID = m_dbo.InsertData("GoodStoreInventory", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.ID;
        }
        public bool Load()
        {
            string sql = string.Format("select * from GoodStoreInventory where id={0}", ID);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                ID = DBTool.GetIntFromRow(row, "ID", 0);
                StoreId = DBTool.GetIntFromRow(row, "StoreId", 0);
                Title = DBTool.GetStringFromRow(row, "Title", "");
                CreateTime = DBTool.GetDateTimeFromRow(row, "CreateTime");
                CreatePerson = DBTool.GetIntFromRow(row, "CreatePerson", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                UpdatePerson = DBTool.GetIntFromRow(row, "UpdatePerson", 0);
                Memo = DBTool.GetStringFromRow(row, "Memo", "");
                BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from GoodStoreInventory where  ID={0} ", ID);
            return m_dbo.ExecuteNonQuery(sql);
        }

        public DataSet ReadGSInventoryByBranch(int branchId)
        {
            string sql = string.Format("select * from GoodStoreInventory where 1=1 ");
            if (branchId > 0)
            {
                sql += string.Format(" and BranchId={0} ", branchId);
            }
            sql += " order by ID desc ,Title";
            return m_dbo.GetDataSet(sql);
        }
    }


    public class GoodStoreInventoryDetail
    {
        public int ID { get; set; }
        public int FK_GSDetail_ID { get; set; }
        public int FK_Goods_ID { get; set; }
        public int FK_Store_ID { get; set; }
        public int OldNum { get; set; }
        public int NewNum { get; set; }
        public double AC { get; set; }
        public int CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public double OldSumMoney { get; set; }
        public double NewSumMoney { get; set; }
        public int FK_GSInventory_ID { get; set; }

        private DBOperate m_dbo;

        public GoodStoreInventoryDetail()
        {
            ID = 0;
            FK_GSDetail_ID = 0;
            FK_Goods_ID = 0;
            FK_Store_ID = 0;
            OldNum = 0;
            NewNum = 0;
            AC = 0;
            CreateUser = 0;
            CreateTime = DateTime.Now;
            OldSumMoney = 0;
            NewSumMoney = 0;
            FK_GSInventory_ID = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (ID > 0)
            {
                arrayList.Add(new SqlParameter("@ID", ID));
            }
            arrayList.Add(new SqlParameter("@FK_GSDetail_ID", FK_GSDetail_ID));
            arrayList.Add(new SqlParameter("@FK_Goods_ID", FK_Goods_ID));
            arrayList.Add(new SqlParameter("@FK_Store_ID", FK_Store_ID));
            arrayList.Add(new SqlParameter("@OldNum", OldNum));
            arrayList.Add(new SqlParameter("@NewNum", NewNum));
            arrayList.Add(new SqlParameter("@AC", AC));
            arrayList.Add(new SqlParameter("@CreateUser", CreateUser));
            arrayList.Add(new SqlParameter("@CreateTime", CreateTime));
            arrayList.Add(new SqlParameter("@OldSumMoney", OldSumMoney));
            arrayList.Add(new SqlParameter("@NewSumMoney", NewSumMoney));
            arrayList.Add(new SqlParameter("@FK_GSInventory_ID", FK_GSInventory_ID));
            if (this.ID > 0)
            {
                m_dbo.UpdateData("GoodStoreInventoryDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.ID = m_dbo.InsertData("GoodStoreInventoryDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.ID;
        }
        public bool Load()
        {
            string sql = string.Format("select * from GoodStoreInventoryDetail where id={0}", ID);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                ID = DBTool.GetIntFromRow(row, "ID", 0);
                FK_GSDetail_ID = DBTool.GetIntFromRow(row, "FK_GSDetail_ID", 0);
                FK_Goods_ID = DBTool.GetIntFromRow(row, "FK_Goods_ID", 0);
                FK_Store_ID = DBTool.GetIntFromRow(row, "FK_Store_ID", 0);
                OldNum = DBTool.GetIntFromRow(row, "OldNum", 0);
                NewNum = DBTool.GetIntFromRow(row, "NewNum", 0);
                AC = DBTool.GetDoubleFromRow(row, "AC", 0);
                CreateUser = DBTool.GetIntFromRow(row, "CreateUser", 0);
                CreateTime = DBTool.GetDateTimeFromRow(row, "CreateTime");
                OldSumMoney = DBTool.GetDoubleFromRow(row, "OldSumMoney", 0);
                NewSumMoney = DBTool.GetDoubleFromRow(row, "NewSumMoney", 0);
                FK_GSInventory_ID = DBTool.GetIntFromRow(row, "FK_GSInventory_ID", 0);

                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from GoodStoreInventoryDetail where ID={0} ", ID);
            return m_dbo.ExecuteNonQuery(sql);
        }

        public DataSet ReadGSInventoryDetail(int InventoryId, int goodsId, string pdType)
        {
            string sql = string.Format(" select FK_Goods_ID as '商品编号',DisplayName as '商品名称',OldNum as '盘前数量',NewNum as '盘后数量',AC as '成本单价',OldSumMoney as '盘前总额' ,NewSumMoney as '盘后总额',(NewNum-OldNum)as '数量差',(NewSumMoney-OldSumMoney)as '金额差' from View_GoodStoreInventoryDetail where 1=1 ");
            if (InventoryId > 0)
            {
                sql += string.Format(" and FK_GSInventory_ID={0} ", InventoryId);
            }
            if (goodsId > 0)
            {
                sql += string.Format(" and FK_Goods_ID={0} ", goodsId);
            }
            if (pdType == "盘盈")
            {
                sql += " and NewNum-OldNum>0 ";
            }
            if (pdType == "盘亏")
            {
                sql += " and NewNum-OldNum<0 ";
            }
            sql += " order by FK_Goods_ID ,Title";
            return m_dbo.GetDataSet(sql);
        }
    }

}
