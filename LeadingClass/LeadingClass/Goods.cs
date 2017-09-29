using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class Goods
    {
        private int m_Id;
        private int m_BranchId;
        private int m_BrandId;
        private string m_SN;
        private int m_TypeId;
        private int m_SupplierId;
        private string m_GoodsName;
        private string m_EnGoodsName;
        private string m_DisplayName;
        private string m_EnDisplayName;
        private string m_PY;
        private string m_Feature;
        private string m_Unit;
        private string m_Model;//同种商品 区分 颜色，子型号等
        private string m_Package;//  例如：1/支/1|12/盒/0|144/大盒/0  
        private double m_MarketPrice;
        private double m_Price;
        private double m_InPrice;
        private double m_TaxInPrice;
        private string m_Description;
        private string m_PackingList;
        private string m_Service;
        private int m_IsVisible;
        private int m_Recommend;
        private DateTime m_RecommendDate;
        private double m_Cuxiao;
        private DateTime m_CuxiaoDate;
        private int m_IsNew;
        private int m_IsHot;
        private int m_IsSoldOutStop;//售完即止
        private int m_Rate;
        private string m_HomeImage;
        private DateTime m_AddTime;
        private DateTime m_UpdateTime;
        private double m_Maolilv;//2014-5-9 毛利率，计算得出
        private int m_PhotoNum;//2014-5-9 图片数量，保存图片是跟新
        private int m_BarCodeNum;//2014-5-9 已有条码数量，保存条码是更新
        private int m_AllBarCodeNum;//2014-5-9 应该有的条码数量，计算得出
        private int m_IsAllow;
        private int m_IsPublic;
        private int m_ParentId;//0:普通商品；1：组合商品；2：母商品
        private int m_ModelId;
        private int m_IsShelves;
        private int m_UserId;
        private int m_SaleNumber;//总销量，每日0：00 数据库自动更新
        private int m_SaleCount;//总销售次数
        private string m_Memo;
        private string m_GoodsRank;
        private DBOperate m_dbo;
        private string m_Origin;//产地
        private string m_ShelfLife;//保质期
        private string m_JD_Link;
        private double m_JD_Price;


        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public int BrandId { get { return m_BrandId; } set { m_BrandId = value; } }
        public string SN { get { return m_SN; } set { m_SN = value; } }
        public int TypeId { get { return m_TypeId; } set { m_TypeId = value; } }
        public int SupplierId { get { return m_SupplierId; } set { m_SupplierId = value; } }
        public string GoodsName { get { return m_GoodsName; } set { m_GoodsName = value; } }
        public string EnGoodsName { get { return m_EnGoodsName; } set { m_EnGoodsName = value; } }
        public string DisplayName { get { return m_DisplayName; } set { m_DisplayName = value; } }
        public string EnDisplayName { get { return m_EnDisplayName; } set { m_EnDisplayName = value; } }
        public string PY { get { return m_PY; } set { m_PY = value; } }
        public string Feature { get { return m_Feature; } set { m_Feature = value; } }
        public string Unit { get { return m_Unit; } set { m_Unit = value; } }
        public string Model { get { return m_Model; } set { m_Model = value; } }
        public string Package { get { return m_Package; } set { m_Package = value; } }
        public double MarketPrice { get { return m_MarketPrice; } set { m_MarketPrice = value; } }
        public double Price { get { return m_Price; } set { m_Price = value; } }
        public double InPrice { get { return m_InPrice; } set { m_InPrice = value; } }
        public double TaxInPrice { get { return m_TaxInPrice; } set { m_TaxInPrice = value; } }
        public string Description { get { return m_Description; } set { m_Description = value; } }
        public string PackingList { get { return m_PackingList; } set { m_PackingList = value; } }
        public string Service { get { return m_Service; } set { m_Service = value; } }
        public int IsVisible { get { return m_IsVisible; } set { m_IsVisible = value; } }
        public int Recommend { get { return m_Recommend; } set { m_Recommend = value; } }
        public DateTime RecommendDate { get { return m_RecommendDate; } set { m_RecommendDate = value; } }
        public double Cuxiao { get { return m_Cuxiao; } set { m_Cuxiao = value; } }
        public DateTime CuxiaoDate { get { return m_CuxiaoDate; } set { m_CuxiaoDate = value; } }
        public int IsNew { get { return m_IsNew; } set { m_IsNew = value; } }
        public int IsHot { get { return m_IsHot; } set { m_IsHot = value; } }
        public int IsSoldOutStop { get { return m_IsSoldOutStop; } set { m_IsSoldOutStop = value; } }
        public int Rate { get { return m_Rate; } set { m_Rate = value; } }
        public string HomeImage { get { return m_HomeImage; } set { m_HomeImage = value; } }
        public DateTime AddTime { get { return m_AddTime; } set { m_AddTime = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public double Maolilv { get { return m_Maolilv; } set { m_Maolilv = value; } }
        public int PhotoNum { get { return m_PhotoNum; } set { m_PhotoNum = value; } }
        public int BarCodeNum { get { return m_BarCodeNum; } set { m_BarCodeNum = value; } }
        public int AllBarCodeNum { get { return m_AllBarCodeNum; } set { m_AllBarCodeNum = value; } }
        public int IsAllow { get { return m_IsAllow; } set { m_IsAllow = value; } }
        public int IsPublic { get { return m_IsPublic; } set { m_IsPublic = value; } }
        public int ParentId { get { return m_ParentId; } set { m_ParentId = value; } }
        public int ModelId { get { return m_ModelId; } set { m_ModelId = value; } }
        public int IsShelves { get { return m_IsShelves; } set { m_IsShelves = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public int SaleNumber { get { return m_SaleNumber; } }
        public int SaleCount { get { return m_SaleCount; } }
        public string Memo { get { return m_Memo; } set { m_Memo = value; } }
        public string GoodsRank { get { return m_GoodsRank; } set { m_GoodsRank = value; } }
        public string Origin { get { return m_Origin; } set { m_Origin = value; } }
        public string ShelfLife { get { return m_ShelfLife; } set { m_ShelfLife = value; } }
        public string ReJectMsg { get; set; }
        public string JD_Link { get{return m_JD_Link;} set{m_JD_Link = value;} }
        public double JD_Price { get { return m_JD_Price; } set { m_JD_Price = value; } }
        public Goods()
        {
            m_Id = 0;
            m_BranchId = 0;
            m_BrandId = 0;
            m_SN = "";
            m_TypeId = 0;
            m_SupplierId = 0;
            m_GoodsName = "";
            m_EnGoodsName = "";
            m_DisplayName = "";
            m_EnDisplayName = "";
            m_PY = "";
            m_Feature = "";
            m_Unit = "";
            m_Model = "";
            m_Package = "";
            m_MarketPrice = 0;
            m_Price = 0;
            m_InPrice = 0;
            m_TaxInPrice = 0;
            m_Description = "";
            m_Origin = "";
            m_ShelfLife = "";
            m_PackingList = "";
            m_Service = "";
            m_IsVisible = 0;
            m_Recommend = 0;
            m_RecommendDate = DateTime.Parse("1900-01-01");
            m_Cuxiao = 0;
            m_CuxiaoDate = DateTime.Parse("1900-01-01");
            m_IsNew = 0;
            m_IsHot = 0;
            m_IsSoldOutStop = 0;
            m_Rate = 0;
            m_HomeImage = "";
            m_AddTime = DateTime.Now;
            m_UpdateTime = DateTime.Now;
            m_Maolilv = 0;
            m_PhotoNum = 0;
            m_BarCodeNum = 0;
            m_IsAllow = 0;
            m_IsPublic = 0;
            m_ParentId = 0;
            m_ModelId = 0;
            m_IsShelves = 1;
            m_UserId = 0;
            m_SaleCount = 0;
            m_SaleNumber = 0;
            m_Memo = "";
            m_GoodsRank = "";
            ReJectMsg = "";
            m_JD_Link = "";
            m_JD_Price = 0;

            m_dbo = new DBOperate();
        }
        public Goods(int Id)
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
            arrayList.Add(new SqlParameter("@BrandId", m_BrandId));
            arrayList.Add(new SqlParameter("@SN", m_SN));
            arrayList.Add(new SqlParameter("@TypeId", m_TypeId));
            arrayList.Add(new SqlParameter("@SupplierId", m_SupplierId));
            arrayList.Add(new SqlParameter("@GoodsName", m_GoodsName));
            arrayList.Add(new SqlParameter("@EnGoodsName", m_EnGoodsName));
            arrayList.Add(new SqlParameter("@DisplayName", m_DisplayName));
            arrayList.Add(new SqlParameter("@EnDisplayName", m_EnDisplayName));
            arrayList.Add(new SqlParameter("@PY", CommenClass.StringTools.GetFirstPYLetter(m_DisplayName)));
            arrayList.Add(new SqlParameter("@Feature", m_Feature));
            arrayList.Add(new SqlParameter("@Unit", m_Unit));
            arrayList.Add(new SqlParameter("@Model", m_Model));
            arrayList.Add(new SqlParameter("@Package", m_Package));
            arrayList.Add(new SqlParameter("@MarketPrice", m_MarketPrice));
            arrayList.Add(new SqlParameter("@Price", Math.Round(m_Price, 2)));
            arrayList.Add(new SqlParameter("@InPrice", m_InPrice));
            arrayList.Add(new SqlParameter("@TaxInPrice", m_TaxInPrice));
            arrayList.Add(new SqlParameter("@Origin", m_Origin));
            arrayList.Add(new SqlParameter("@ShelfLife", m_ShelfLife));
            arrayList.Add(new SqlParameter("@Memo", m_Memo));
            arrayList.Add(new SqlParameter("@GoodsRank", m_GoodsRank));
            arrayList.Add(new SqlParameter("@ReJectMsg", ReJectMsg));
            double maolilv = 0;
            if (m_Price != 0 && m_TaxInPrice != 0)
            {
                maolilv = 100 * (m_Price - m_TaxInPrice) / m_Price;
            }
            arrayList.Add(new SqlParameter("@maolilv", maolilv));
            arrayList.Add(new SqlParameter("@Description", m_Description));
            arrayList.Add(new SqlParameter("@PackingList", m_PackingList));
            arrayList.Add(new SqlParameter("@Service", m_Service));
            arrayList.Add(new SqlParameter("@IsVisible", m_IsVisible));
            arrayList.Add(new SqlParameter("@Recommend", m_Recommend));
            arrayList.Add(new SqlParameter("@RecommendDate", m_RecommendDate));
            arrayList.Add(new SqlParameter("@Cuxiao", m_Cuxiao));
            arrayList.Add(new SqlParameter("@CuxiaoDate", m_CuxiaoDate));
            arrayList.Add(new SqlParameter("@IsNew", m_IsNew));
            arrayList.Add(new SqlParameter("@IsHot", m_IsHot));
            arrayList.Add(new SqlParameter("@IsSoldOutStop", m_IsSoldOutStop));
            arrayList.Add(new SqlParameter("@Rate", m_Rate));
            arrayList.Add(new SqlParameter("@HomeImage", m_HomeImage));
            arrayList.Add(new SqlParameter("@AddTime", m_AddTime));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            //arrayList.Add(new SqlParameter("@AllBarCodeNum", GetAllBarCodeNum()));
            arrayList.Add(new SqlParameter("@ParentId", m_ParentId));
            arrayList.Add(new SqlParameter("@ModelId", m_ModelId));
            arrayList.Add(new SqlParameter("@JD_Link", m_JD_Link));
            arrayList.Add(new SqlParameter("@JD_Price", m_JD_Price));
            StringBuilder srb = new StringBuilder();
            //不保存 图片数量和 条码数量
            if (this.Id > 0)
            {
                double oldPrice = GetOldPrice();

                if (this.Price != oldPrice)
                {
                    WriteAdjustPrice(oldPrice, this.Price);
                }

                m_dbo.UpdateData("Goods", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
                srb.Append(string.Format("update Goods set IsVisible={0},BrandId='{1}',TypeId='{2}',UpdateTime='{3}',JD_Link = '{5}',JD_Price ={6} where ParentId = {4};", m_IsVisible, m_BrandId, m_TypeId, m_UpdateTime, this.Id.ToString(),m_JD_Link,m_JD_Price));
                m_dbo.ExecuteNonQuery(srb.ToString());
            }
            else
            {
                arrayList.Add(new SqlParameter("@BranchId", m_BranchId));
                arrayList.Add(new SqlParameter("@IsAllow", m_IsAllow));
                arrayList.Add(new SqlParameter("@IsPublic", m_IsPublic));
                arrayList.Add(new SqlParameter("@IsShelves", m_IsShelves));
                arrayList.Add(new SqlParameter("@UserId", m_UserId));
                this.Id = m_dbo.InsertData("Goods", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
                if (Id > 0 && m_BranchId != 1 && m_IsVisible == 1 && this.Price > 0)
                {
                    GoodsBranch gb = new GoodsBranch();
                    gb.GoodsId = Id;
                    gb.BranchId = m_BranchId;
                    gb.IsVisible = 1;
                    gb.IsAllow = 1;
                    gb.IsPublic = m_IsPublic;
                    gb.UserId = m_UserId;
                    gb.IsWeb = 1;
                    gb.Price = Convert.ToDecimal(m_Price);
                    gb.PriceRate = 0;
                    gb.IsChangePrice = 0;
                    gb.IsChangeRecommend = 0;
                    gb.Recommend = m_Recommend;
                    gb.Save();
                }
            }
           
            
            return this.Id;
        }

        public bool Load()
        {
            string sql = string.Format("select * from Goods where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                m_BrandId = DBTool.GetIntFromRow(row, "BrandId", 0);
                m_SN = DBTool.GetStringFromRow(row, "SN", "");
                m_TypeId = DBTool.GetIntFromRow(row, "TypeId", 0);
                m_SupplierId = DBTool.GetIntFromRow(row, "SupplierId", 0);
                m_GoodsName = DBTool.GetStringFromRow(row, "GoodsName", "");
                m_EnGoodsName = DBTool.GetStringFromRow(row, "EnGoodsName", "");
                m_DisplayName = DBTool.GetStringFromRow(row, "DisplayName", "");
                m_EnDisplayName = DBTool.GetStringFromRow(row, "EnDisplayName", "");
                m_PY = DBTool.GetStringFromRow(row, "PY", "");
                m_Feature = DBTool.GetStringFromRow(row, "Feature", "");
                m_Unit = DBTool.GetStringFromRow(row, "Unit", "");
                m_Model = DBTool.GetStringFromRow(row, "Model", "");
                m_Package = DBTool.GetStringFromRow(row, "Package", "");
                m_MarketPrice = DBTool.GetDoubleFromRow(row, "MarketPrice", 0);
                m_Price = DBTool.GetDoubleFromRow(row, "Price", 0);
                m_InPrice = DBTool.GetDoubleFromRow(row, "InPrice", 0);
                m_TaxInPrice = DBTool.GetDoubleFromRow(row, "TaxInPrice", 0);
                m_Maolilv = DBTool.GetDoubleFromRow(row, "maolilv", 0);
                m_Description = DBTool.GetStringFromRow(row, "Description", "");
                m_PackingList = DBTool.GetStringFromRow(row, "PackingList", "");
                m_Service = DBTool.GetStringFromRow(row, "Service", "");
                m_IsVisible = DBTool.GetIntFromRow(row, "IsVisible", 0);
                m_Recommend = DBTool.GetIntFromRow(row, "Recommend", 0);
                m_RecommendDate = DBTool.GetDateTimeFromRow(row, "RecommendDate");
                m_Cuxiao = DBTool.GetDoubleFromRow(row, "Cuxiao", 0);
                m_CuxiaoDate = DBTool.GetDateTimeFromRow(row, "CuxiaoDate");
                m_IsNew = DBTool.GetIntFromRow(row, "IsNew", 0);
                m_IsHot = DBTool.GetIntFromRow(row, "IsHot", 0);
                m_IsSoldOutStop = DBTool.GetIntFromRow(row, "IsSoldOutStop", 0);
                m_Rate = DBTool.GetIntFromRow(row, "Rate", 0);
                m_HomeImage = DBTool.GetStringFromRow(row, "HomeImage", "");
                m_AddTime = DBTool.GetDateTimeFromRow(row, "AddTime");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_Maolilv = DBTool.GetDoubleFromRow(row, "maolilv", 0);
                m_AllBarCodeNum = DBTool.GetIntFromRow(row, "AllBarCodeNum", 0);
                m_BarCodeNum = DBTool.GetIntFromRow(row, "BarCodeNum", 0);
                m_PhotoNum = DBTool.GetIntFromRow(row, "PhotoNum", 0);
                m_IsAllow = DBTool.GetIntFromRow(row, "IsAllow", 0);
                m_IsPublic = DBTool.GetIntFromRow(row, "IsPublic", 0);
                m_ParentId = DBTool.GetIntFromRow(row, "ParentId", 0);
                m_ModelId = DBTool.GetIntFromRow(row, "ModelId", 0);
                m_IsShelves = DBTool.GetIntFromRow(row, "IsShelves", 0);
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_SaleNumber = DBTool.GetIntFromRow(row, "SaleNumber", 0);
                m_SaleCount = DBTool.GetIntFromRow(row, "SaleCount", 0);
                m_Origin = DBTool.GetStringFromRow(row, "Origin", "");
                m_ShelfLife = DBTool.GetStringFromRow(row, "ShelfLife", "");
                m_Memo = DBTool.GetStringFromRow(row, "Memo", "");
                m_GoodsRank = DBTool.GetStringFromRow(row, "GoodsRank", "");
                ReJectMsg = DBTool.GetStringFromRow(row, "ReJectMsg", "");
                m_JD_Link = DBTool.GetStringFromRow(row, "JD_Link", "");
                m_JD_Price = DBTool.GetDoubleFromRow(row, "JD_Price", 0);
                return true;
            }
            return false;
        }

        #region 针对goods表更新等操作

        /// <summary>
        /// 保存母商品后保存子商品
        /// </summary>
        /// <param name="PGoodsId"></param>
        /// <returns></returns>
        public int SaveChildGoodsAfterGoods(Goods PGoods)
        {
            int result = 0;
            if (PGoods != null)
            {
                if (PGoods.Id > 0)
                {
                    string csql = "select * from Goods where ParentId=" + PGoods.Id;
                    DataTable cdt = m_dbo.GetDataSet(csql).Tables[0];
                    if (cdt.Rows.Count > 0)
                    {
                        foreach (DataRow item in cdt.Rows)
                        {
                            Goods CGoods = new Goods();
                            CGoods.Id = Convert.ToInt32(item["ID"].ToString());
                            CGoods.BranchId = PGoods.BranchId;
                            CGoods.BrandId = PGoods.BrandId;
                            CGoods.SN = PGoods.SN;
                            CGoods.TypeId = PGoods.TypeId;   //TODO
                            CGoods.SupplierId = PGoods.SupplierId;
                            CGoods.GoodsName = PGoods.GoodsName;
                            CGoods.DisplayName = item["DisplayName"].ToString();
                            CGoods.PY = item["PY"].ToString();
                            CGoods.Feature = PGoods.Feature;
                            CGoods.Unit = PGoods.Unit;
                            CGoods.Model = item["Model"].ToString();
                            CGoods.Package = PGoods.Package;
                            CGoods.MarketPrice = PGoods.MarketPrice;
                            CGoods.Price = PGoods.Price;
                            CGoods.InPrice = Convert.ToDouble(item["InPrice"]);
                            CGoods.TaxInPrice = PGoods.TaxInPrice;
                            CGoods.Description = PGoods.Description;
                            CGoods.PackingList = PGoods.PackingList;
                            CGoods.Service = PGoods.Service;
                            CGoods.IsVisible = PGoods.IsVisible;
                            CGoods.Recommend = PGoods.Recommend;
                            CGoods.RecommendDate = PGoods.RecommendDate;
                            CGoods.Cuxiao = PGoods.Cuxiao;
                            CGoods.CuxiaoDate = PGoods.CuxiaoDate;
                            CGoods.IsNew = PGoods.IsNew;
                            CGoods.IsHot = PGoods.IsHot;
                            CGoods.Rate = PGoods.Rate;
                            CGoods.HomeImage = PGoods.HomeImage;
                            CGoods.AddTime = Convert.ToDateTime(item["AddTime"].ToString());
                            CGoods.UpdateTime = DateTime.Now;
                            CGoods.Maolilv = Convert.ToDouble(item["Maolilv"]);
                            CGoods.AllBarCodeNum = Convert.ToInt32(item["AllBarCodeNum"].ToString());
                            CGoods.BarCodeNum = Convert.ToInt32(item["BarCodeNum"].ToString());
                            CGoods.PhotoNum = Convert.ToInt32(item["PhotoNum"].ToString());
                            CGoods.IsShelves = Convert.ToInt32(item["IsShelves"].ToString());
                            CGoods.IsAllow = PGoods.IsAllow;
                            CGoods.IsPublic = Convert.ToInt32(item["IsPublic"].ToString());
                            CGoods.ParentId = Convert.ToInt32(item["ParentId"].ToString());
                            CGoods.ModelId = Convert.ToInt32(item["ModelId"].ToString());
                            CGoods.UserId = Convert.ToInt32(item["UserId"].ToString());
                            CGoods.EnGoodsName = item["EnGoodsName"].ToString();
                            CGoods.EnDisplayName = item["EnDisplayName"].ToString();
                            CGoods.IsSoldOutStop = PGoods.IsSoldOutStop;
                            CGoods.Memo = item["Memo"].ToString();
                            CGoods.GoodsRank = item["GoodsRank"].ToString();
                            CGoods.Origin = item["Origin"].ToString();
                            CGoods.ShelfLife = item["ShelfLife"].ToString();
                            if (CGoods.Save() > 0)
                            {
                                result = result + 1;
                            }
                        }
                        if (result != cdt.Rows.Count)
                        {
                            result = 0;
                        }
                    }
                }
            }
            return result;
        }



        /// <summary>
        /// 获取原来的价格
        /// </summary>
        /// <returns></returns>
        private double GetOldPrice()
        {
            string sql = string.Format(" select Price from goods where Id = {0} ", this.m_Id.ToString());
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return DBTool.GetDoubleFromRow(ds.Tables[0].Rows[0], "Price", 0);
            }
            else return 0;
        }
        /// <summary>
        /// 记录调价记录
        /// </summary>
        /// <param name="oldPrice"></param>
        /// <param name="newPrice"></param>
        private void WriteAdjustPrice(double oldPrice, double newPrice)
        {
            string sql = string.Format("insert into GoodsPrice(GoodsId,oldPrice,newPrice,BranchId) values({0},{1},{2},{3}) ", this.m_Id, oldPrice, newPrice, this.BranchId);
            m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 更新包装列表
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public void InitPackage()
        {
            string[] packages = this.Package.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (packages.Length == 3)
            {
                //处理小包装
                string[] small = packages[0].Split(new char[] { '/' });
                if (small[0] != "0")
                {
                    GoodsPackage gp = new GoodsPackage();
                    gp.Load(this.Id, CommenClass.PackageType.小.ToString());
                    gp.GoodsId = this.Id;
                    gp.Type = CommenClass.PackageType.小.ToString();
                    gp.Num = int.Parse(small[0]);
                    gp.Name = small[1];
                    gp.IsDefault = int.Parse(small[2]);
                    gp.Save();
                }
                else
                {
                    GoodsPackage gp = new GoodsPackage();
                    gp.Delete(this.Id, CommenClass.PackageType.小.ToString());
                }

                //处理中包装
                string[] middle = packages[1].Split(new char[] { '/' });
                if (middle[0] != "0")
                {
                    GoodsPackage gp = new GoodsPackage();
                    gp.Load(this.Id, CommenClass.PackageType.中.ToString());
                    gp.GoodsId = this.Id;
                    gp.Type = CommenClass.PackageType.中.ToString();
                    gp.Num = int.Parse(middle[0]);
                    gp.Name = middle[1];
                    gp.IsDefault = int.Parse(middle[2]);
                    gp.Save();
                }
                else
                {
                    GoodsPackage gp = new GoodsPackage();
                    gp.Delete(this.Id, CommenClass.PackageType.中.ToString());
                }
                //处理大包装
                string[] large = packages[2].Split(new char[] { '/' });
                if (large[0] != "0")
                {
                    GoodsPackage gp = new GoodsPackage();
                    gp.Load(this.Id, CommenClass.PackageType.大.ToString());
                    gp.GoodsId = this.Id;
                    gp.Type = CommenClass.PackageType.大.ToString();
                    gp.Num = int.Parse(large[0]);
                    gp.Name = large[1];
                    gp.IsDefault = int.Parse(large[2]);
                    gp.Save();
                }
                else
                {
                    GoodsPackage gp = new GoodsPackage();
                    gp.Delete(this.Id, CommenClass.PackageType.大.ToString());
                }

            }

        }
        /// <summary>
        /// 根据现有的Package更新 goods的packages 字段
        /// </summary>
        public void UpdatePackage()
        {
            DataTable dt = ReadPackage().Tables[0];
            string packages = "";
            packages += getpackagestr(dt, CommenClass.PackageType.小);
            packages += getpackagestr(dt, CommenClass.PackageType.中);
            packages += getpackagestr(dt, CommenClass.PackageType.大);
            this.Package = packages;
            this.Save();

        }
        private string getpackagestr(DataTable dt, CommenClass.PackageType type)
        {
            DataRow[] r = dt.Select(string.Format("type = '{0}'", type.ToString()));
            if (r.Length == 1)
            {
                if (type != CommenClass.PackageType.大)
                    return string.Format("{0}/{1}/{2}|", DBTool.GetIntFromRow(r[0], "num", 0), DBTool.GetStringFromRow(r[0], "name", ""), DBTool.GetIntFromRow(r[0], "IsDefault", 0));
                else
                    return string.Format("{0}/{1}/{2}", DBTool.GetIntFromRow(r[0], "num", 0), DBTool.GetStringFromRow(r[0], "name", ""), DBTool.GetIntFromRow(r[0], "IsDefault", 0));

            }
            else
            {
                if (type != CommenClass.PackageType.大)
                    return "0//0|";
                else return "0//0";
            }

        }

        private void InitStoreZone(int StoreId)
        {
            GoodsStore gs = new GoodsStore();
            gs.AC = this.InPrice;
            gs.GoodsId = this.Id;
            gs.StoreId = StoreId;
            gs.Save();
        }
        /// <summary>
        /// 修改商品价格-------------------有问题，暂议
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public int ModifyPrice(int goodsId, double price)
        {
            this.Id = goodsId;
            this.Load();
            this.Price = price;
            return this.Save();
        }

        /// <summary>
        /// 商品下架
        /// </summary>
        /// <returns></returns>
        public bool UpdateIsShelves(int IsShelves, int GoodsId)
        {
            if (GoodsId > 0)
            {
                string sql = string.Format("update Goods set IsShelves={0},UpdateTime='{2}' where ID in (select ID from Goods where ID={1} UNION all  select ID from Goods where ParentId={1})", IsShelves, GoodsId, DateTime.Now);
                return m_dbo.ExecuteNonQuery(sql);

            }
            else return false;
        }

        /// <summary>
        /// 修改商品类型（基本商品与母商品）
        /// </summary>
        /// <param name="GoodsId"></param>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public bool UpdateParentId(int GoodsId, int ParentId)
        {
            string sql = string.Format(" update Goods set ParentId={0},UpdateTime='{2}' where ID={1}", ParentId, GoodsId, DateTime.Now);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 更新图片数量
        /// </summary>
        /// <returns></returns>
        public bool UpdatePhotoNum()
        {
            //string sql0 = string.Format(" select COUNT(*) as count from GoodsPhoto  where GoodsId = {0} ", this.Id);
            //DataSet ds = m_dbo.GetDataSet(sql0);
            //int count = DBTool.GetIntFromRow(ds.Tables[0].Rows[0], "count", 0);

            //string sql1 = string.Format(" update goods set PhotoNum = {0} where Id={1} or ParentId={1} ", count, this.Id);
            //return m_dbo.ExecuteNonQuery(sql1);

            string sql = string.Format(@"update goods set PhotoNum = (select COUNT(*) as count from GoodsPhoto  where GoodsId = {0}) where Id={0} or ParentId={0}", this.Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 获取选中规格下对应的GoodsId q
        /// </summary>
        /// <param name="goodsModelId">初始的ModelId</param>
        /// <param name="modellevel">选中某一规格的级别</param>
        /// <param name="modelId">选中某一规格的Id</param>
        /// <returns></returns>
        public int LoadGoodsByModelIds(int goodsModelId, int modellevel, int modelId)
        {
            string sql = string.Format(@"declare @GoodsModelId int={0};
							            declare @Model1Id int ;
                                        declare @Model2Id int ;
                                        declare @Model3Id int ;
                                        declare @ModelLevel int={1};
                                        declare @ModelIdNew int={2};
							            select top 1 @Model1Id=Model1Id ,@Model2Id=Model2Id,@Model3Id=Model3Id from dbo.GoodsModel where Id=@GoodsModelId;
                                        set @Model{1}Id=@ModelIdNew;
							            select top 1 Id from dbo.Goods where ModelId =(  select top 1 Id from dbo.GoodsModel where Model1Id = @Model1Id and Model2Id =@Model2Id and Model3Id =@Model3Id);
							            select top 1 Id from dbo.Goods where ModelId=@GoodsModelId;", goodsModelId, modellevel, modelId);
            DataSet ds = m_dbo.GetDataSet(sql);
            int goodsId;
            if (ds.Tables[0].Rows.Count > 0)
            {
                goodsId = (!ds.Tables[0].Rows[0].IsNull(0) ? Convert.ToInt32(ds.Tables[0].Rows[0][0]) : 0); //存在选定的规格的商品
            }
            else
            {
                goodsId = (!ds.Tables[1].Rows[0].IsNull(0) ? Convert.ToInt32(ds.Tables[1].Rows[0][0]) : 0);//不存在选定的规格的商品，返回之前商品的信息
            }
            return goodsId;
        }

        /// <summary>
        /// 删除商品（显示状态）
        /// </summary>
        /// <param name="GoodsId"></param>
        /// <returns></returns>
        public bool UpdateIsVisible(int GoodsId)
        {
            string sql = string.Format("update Goods set IsVisible=-1 where ID={0} ", GoodsId);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 审核商品（修改商品公有性）
        /// </summary>
        /// <param name="GoodsId"></param>
        /// <param name="IsPublic"></param>
        /// <returns></returns>
        public bool UpdeteIsAllow(int GoodsId, int IsPublic, int Parent, int IsAllow,string ReJectMsg)
        {
            StringBuilder stb = new StringBuilder();
            stb.Append(string.Format("update Goods set IsAllow={3}, IsPublic='{0}',UpdateTime='{2}',ReJectMsg ='{4}' where ID ='{1}' ;", IsPublic, GoodsId, DateTime.Now, IsAllow,ReJectMsg));
            if (Parent != 0)
            {
                stb.Append(string.Format("update Goods set IsAllow={3}, IsPublic='{0}',UpdateTime='{2}',ReJectMsg ='{4}' where ParentId ='{1}' ;", IsPublic, GoodsId, DateTime.Now, IsAllow,ReJectMsg));
            }
            return m_dbo.ExecuteNonQuery(stb.ToString());

        }

        #endregion

        #region 读取有关商品信息

        /// <summary>
        /// 批量根据商品ID读取商品名称
        /// </summary>
        /// <returns></returns>
        public DataSet BatchGetGoodsNameById(int[] goodsId)
        {
            string sql = "select GoodsName from goods where Id in ( ";
            if (goodsId.Length > 0)
            {
                foreach (var item in goodsId)
                {
                    sql += item.ToString();
                    if (item < goodsId.Length - 1)
                    {
                        sql += ",";
                    }
                }
            }
            sql += ")";
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取包装列表,如果没有列表的进行初始化  //位置有问题
        /// </summary>
        /// <returns></returns>
        public DataSet ReadPackage()
        {
            string sql = string.Format("select * from GoodsPackage where GoodsId={0} order by Num ", this.Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 0)
            {
                InitPackage();
                ds = m_dbo.GetDataSet(sql);
            }
            return ds;
        }

        /// <summary>
        /// 读取商品在特定仓库的库位码信息----------------位置
        /// </summary>
        /// <param name="StoreId"></param>
        /// <returns></returns>
        public DataSet ReadStoreZone(int StoreId)
        {
            string sql = string.Format(" select * from View_GoodsStore where GoodsId={0} and StoreId={1} order by StoreZone", this.Id, StoreId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 0)
            {
                InitStoreZone(StoreId);
                ds = m_dbo.GetDataSet(sql);
            }
            return ds;
        }

        /// <summary>
        /// 读取 商品在下单时的动态信息  
        /// </summary>
        /// <param name="comId"></param>
        /// <param name="GoodsId"></param>
        /// <param name="Model"></param>
        /// <param name="BranchId"></param>
        /// <returns>上次售价，协议价格，可用库存</returns>
        public GoodsInfoForOrder ReadGoodsInfoForOrder(int ComId, int GoodsId, int ParentId, int BranchId)
        {
            GoodsInfoForOrder info = new GoodsInfoForOrder();
            info.GoodsId = GoodsId;
            string sql = string.Format(@"select top 1 SalePrice from OrderDetail join [Order] on OrderId =[Order].Id
                                             where GoodsId={0} and ComId={1} and SalePrice >0 order by orderTime desc;
                                         select VIPPrice from View_VIPCounter where GoodsId = {2} and ComId={1};
                                         select SUM(Num) as StockNum  from view_GoodsStore where GoodsId={0} and BranchId={3} 
                                             and IsDefault=1 and StoreZone <>'收货区' group by GoodsId,BranchId;
                                         select dbo.GetReSaleGoods({0},{3}) as ResaleNumber;
                                         select top 1 TaxInPrice from PurchaseDetail join Purchase on PurchaseId=purchase.Id join Store on purchase.StoreId=store.ID 
                                         where GoodsId ={0} and Purchase.BranchId={3} and IsDefault=1 and TaxInPrice>0 and Num >0 and PurchaseStatus='已入库' order by Purchase.UpdateTime desc;", GoodsId, ComId, ParentId, BranchId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                info.LastPrice = DBTool.GetDoubleFromRow(ds.Tables[0].Rows[0], "SalePrice", 0);
            }
            else info.LastPrice = 0;
            if (ds.Tables[1].Rows.Count == 1)
            {
                info.ContractPrice = DBTool.GetDoubleFromRow(ds.Tables[1].Rows[0], "VIPPrice", 0);
            }
            else info.ContractPrice = 0;
            if (ds.Tables[2].Rows.Count == 1)
            {
                info.StockNumber = DBTool.GetIntFromRow(ds.Tables[2].Rows[0], "StockNum", 0);
            }
            else info.StockNumber = 0;
            if (ds.Tables[3].Rows.Count == 1)
            {
                info.ResaleNumber = DBTool.GetIntFromRow(ds.Tables[3].Rows[0], "ResaleNumber", 0);
            }
            else info.ResaleNumber = 0;
            if (ds.Tables[4].Rows.Count == 1)
            {
                info.TaxInPrice = DBTool.GetDoubleFromRow(ds.Tables[4].Rows[0], "TaxInPrice", 0);
            }
            else info.TaxInPrice = 0;
            return info;
        }
        /// <summary>
        /// 采购所需的  最新进价，成本价，库存量  根据branchId 查最新进价 不准算天宁寺仓
        /// </summary>
        /// <param name="GoodsId"></param>
        /// <param name="Model"></param>
        /// <param name="BranchId"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public GoodsInfoForPurchase ReadGoodsInfoForPurchase(int GoodsId, int BranchId, int storeId)
        {
            GoodsInfoForPurchase info = new GoodsInfoForPurchase();
            info.BranchId = BranchId;
            info.GoodsId = GoodsId;
            info.Model = Model;
            info.StoreId = storeId;
            string sql = string.Format(@"select top 1 BillsPrice from view_PurchaseDetail where branchId={0} and GoodsId={1}  and (PurchaseType = '现金采购' or PurchaseType = '签单收货')  and PurchaseStatus='已入库' and IsDefault=1 and Num>0 and BillsPrice >0  order by UpdateTime desc;
select AC,Num  from GoodsStore where StoreId={2} and GoodsId={1} and StoreZone<>'收货区';
select dbo.GetReSaleGoods({1},{0}) as ResaleNumber;
select Goods_AC from GoodsAC where BranchId={0} and GoodsId={1} ", BranchId, GoodsId, storeId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                info.LastInPrice = DBTool.GetDoubleFromRow(ds.Tables[0].Rows[0], "BillsPrice", 0);
            }
            else info.LastInPrice = 0;
            if (ds.Tables[1].Rows.Count == 1)
            {
                info.AvgInPrice = DBTool.GetDoubleFromRow(ds.Tables[1].Rows[0], "AC", 0);
                info.StockNum = DBTool.GetIntFromRow(ds.Tables[1].Rows[0], "Num", 0);
            }
            else
            {
                info.AvgInPrice = 0;
                info.StockNum = 0;
            }
            if (ds.Tables[2].Rows.Count == 1)
            {
                info.ResalesNum = DBTool.GetIntFromRow(ds.Tables[2].Rows[0], "ResaleNumber", 0);
            }
            if (ds.Tables[3].Rows.Count == 1)
            {
                info.StandingAC = DBTool.GetDoubleFromRow(ds.Tables[3].Rows[0], "Goods_AC", 0);
            }
            return info;
        }
        /// <summary>
        ///  读取商品的库存信息
        /// </summary>
        /// <param name="GoodsId"></param>
        /// <param name="Model"></param>
        /// <param name="BranchId"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public DataSet ReadGoodsStoreInfo(int GoodsId, int BranchId, int storeId)
        {
            string sql = string.Format(@" select *,dbo.GetReSaleGoods(GoodsId,{0}) as resale
    from view_GoodsStore 
    where num <>0 and BranchId={0} ", BranchId);

            this.Id = GoodsId;
            this.Load();
            if (this.ParentId != 2)
            {
                sql += string.Format("  and GoodsId = {0} ", GoodsId);
            }
            else//如果是目商品的话，读取所有子商品的库存数量
            {
                sql += string.Format(" and ParentId={0} ", GoodsId);
            }
            if (storeId > 0)
            {
                sql += string.Format(" and storeId={0} ", storeId);
            }
            sql += " order by storeId ";

            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取最近 n次采购记录   
        /// </summary>
        /// <param name="GoodsId"></param>
        /// <param name="BranchId"></param>
        /// <returns></returns>
        public DataSet ReadPurchaseInfo(int GoodsId, int BranchId, int topNum)
        {
            string sql = string.Format(@" select top {2}  ShortName,PurchaseDate,InPrice,TaxInPrice,Num,LinkMan1,Telphone1,Mobile1,View_PurchaseDetail.SupplierId from View_PurchaseDetail 
where GoodsId ={0} and BranchId={1}  and (PurchaseType = '现金采购' or PurchaseType = '签单收货')  and PurchaseStatus='已入库' and IsDefault=1 and Num>0 and BillsPrice >0 order by UpdateTime desc ", GoodsId, BranchId, topNum);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 获取销售信息   --------位置不对
        /// </summary>
        /// <returns></returns>
        public GoodsSaleInfo ReadSaleInfo()
        {
            string sql = string.Format("select COUNT(Num) as Count,SUM(Num) as Num from OrderDetail where GoodsId={0}", this.Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                GoodsSaleInfo gsi = new GoodsSaleInfo();
                gsi.SaleNumAll = DBTool.GetIntFromRow(row, "Num", 0);
                gsi.SaleCountAll = DBTool.GetIntFromRow(row, "Count", 0);
                return gsi;
            }
            else return null;
        }

        /// <summary>
        /// 调拨单的Goods信息，AC平均成本价 quxiaoshan 2015-5-16           ---------暂定
        /// </summary>
        /// <returns></returns>
        public DataSet ReadGoodsInfoForTransferDetail(int goodsId, string model, int storeId)
        {
            string sql = string.Format(@" select dbo.Goods.* ,dbo.GoodsStore.AC  from dbo.Goods 
                                        join dbo.GoodsStore on dbo.Goods.ID=dbo.GoodsStore.GoodsId 
                                        where GoodsId={0} and dbo.GoodsStore.model='{1}'
                                        and StoreId ={3} ");
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 通过GoodsId读取商品规格信息 add by luochunhui
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public DataSet ReadGoodsByGoodsId(int goodsId)
        {
            this.Id = goodsId;
            this.Load();
            this.ParentId = ParentId;
            string sql = string.Format("select top 1 * from Goods where 1=1 ");
            if (ParentId == 0)
            {
                sql += string.Format(" and Id={0};", goodsId);
            }
            else if (ParentId == 2)
            {
                sql += string.Format(" and ParentId={0} and IsShelves=1;", goodsId);
                sql += string.Format(" select *,(select top 1 ModelLevel from GoodsModelType where GoodsModelType.GoodsId=vg.GoodsParentId)as Modellevel from  dbo.View_GoodsModel as vg where GoodsParentId={0};select * from GoodsModelType where GoodsId={0} order by  ModelLevel,Sort", goodsId);
            }
            else
            {
                sql += string.Format(" and ID={0};", goodsId);
                sql += string.Format(" select *,(select top 1 ModelLevel from GoodsModelType where GoodsModelType.GoodsId=vg.GoodsParentId)as Modellevel from  dbo.View_GoodsModel as vg where GoodsParentId=(select ParentId from Goods where 1=1  and ID={0});select * from GoodsModelType where GoodsId=(select ParentId from Goods where 1=1  and ID={0}) order by  ModelLevel,Sort", goodsId);
            }
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 保密用户加入专柜不能是默认子商品          ----------部署项目可能用到
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public DataSet ScyReadGoodsByGoodsId(int goodsId)
        {
            string sql = string.Format("select top 1 * from Goods where 1=1 ");
            sql += string.Format(" and ID={0};", goodsId);

            sql += string.Format("select *,(select top 1 ModelLevel from GoodsModelType where GoodsModelType.GoodsId=vg.GoodsParentId)as Modellevel from  dbo.View_GoodsModel as vg where GoodsParentId=(select ParentId from Goods where 1=1  and ID={0});select * from GoodsModelType where GoodsId=(select ParentId from Goods where 1=1  and ID={0}) order by  ModelLevel,Sort", goodsId);
            return m_dbo.GetDataSet(sql);

        }

        /// <summary>
        /// 用于展示加入专柜的子商品的单一规格
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public DataSet ReadVIPCounterChildGoods(int goodsId, int goodsModelId)
        {
            string sql = string.Format("select top 1 * from Goods where ID={0};select *,(select top 1 ModelLevel from GoodsModelType where GoodsModelType.GoodsId=vg.GoodsParentId)as Modellevel from  dbo.View_GoodsModel as vg where GoodsModelId={1};select * from GoodsModelType where Id =(select Model1Id from View_GoodsModel where GoodsModelId={1}) or GoodsId=(select GoodsParentId from View_GoodsModel where GoodsModelId={1}) and Sort=0 order by  ModelLevel,Sort", goodsId, goodsModelId);
            return m_dbo.GetDataSet(sql);
        }

        public DataSet ReadSubGoods(int GoodsId)
        {
            string sql = string.Format(@"select dbo.Goods.ID as SubGoodsId,gm.*,Goods.* ,(select Name from GoodsModelType g where g.Id=gm.Model1Id) as Modelvalue1,(select Name from GoodsModelType g where g.Id=gm.Model2Id) as Modelvalue2,(select Name from GoodsModelType g where g.Id=gm.Model3Id) as Modelvalue3
from dbo.Goods join dbo.goodsmodel as gm on dbo.Goods.ParentId=gm.GoodsId and dbo.Goods.ModelId=gm.Id 
where dbo.Goods.ParentId={0};", GoodsId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取该商品下对应的规格Model q
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public DataSet ReadGoodsModelByGoodsId(int goodsModelId)
        {
            string sql = string.Format(@"select dbo.GoodsModel.GoodsId as GoodsParentId, dbo.Goods.ID as GoodsId ,dbo.GoodsModel.Id as GoodsModelId,Model1Id,Model2Id,Model3Id from dbo.GoodsModel 
                                        join dbo.Goods on dbo.GoodsModel.GoodsId=dbo.Goods.ParentId and dbo.GoodsModel.Id=dbo.Goods.ModelId
                                        where dbo.Goods.ParentId = (  select top 1 GoodsId from dbo.GoodsModel where Id = {0});", goodsModelId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 通过goodsId读取基本商品对应的子商品规格 q 
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public DataSet ReadGoodsModelsByGoodsId(int goodsId)
        {
            string sql = string.Format(@"select 
                                        (convert( varchar(50),( case when Name1 is null then '' else Name1  end )  ) +' '+
                                        convert( varchar(50),( case when Name2 is null then '' else Name2  end )  )+' '+
                                        convert( varchar(50),( case when Name3 is null then '' else Name3  end )  ) ) as Models ,
                                        (convert( varchar(50),( case when EnName1 is null then '' else EnName1  end )  ) +' '+
                                         convert( varchar(50),( case when EnName2 is null then '' else EnName2  end )  )+' '+
                                         convert( varchar(50),( case when EnName3 is null then '' else EnName3  end ) ) ) as EnModels ,
                                        * from dbo.view_GoodsModel 
                                         where IsShelves<>0 and GoodsParentId={0}; ", goodsId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 根据类别和品牌读取型号（政采云）
        /// </summary>
        /// <returns></returns>
        public DataSet Read_GoodsSN(int categoryId, int brandId, string specificationMatch)
        {
            string sql = string.Format("select * from Goods where  TypeId ={0} and  BrandId={1} and SN like  '%{2}%'", categoryId, brandId, specificationMatch);
            return m_dbo.GetDataSet(sql);

        }
        #endregion

    }
    public class GoodsInfoForOrder
    {
        private double m_LastPrice;
        private double m_ContractPrice;
        private int m_StockNumber;
        private int m_ReSaleNumber;
        private int m_GoodsId;
        public double LastPrice { get { return m_LastPrice; } set { m_LastPrice = value; } }
        public double ContractPrice { get { return m_ContractPrice; } set { m_ContractPrice = value; } }
        public int StockNumber { get { return m_StockNumber; } set { m_StockNumber = value; } }
        public int ResaleNumber { get { return m_ReSaleNumber; } set { m_ReSaleNumber = value; } }
        public int GoodsId { get { return m_GoodsId; } set { m_GoodsId = value; } }
        public double TaxInPrice { get; set; }
        public GoodsInfoForOrder()
        {
            m_LastPrice = 0;
            m_ContractPrice = 0;
            m_StockNumber = 0;
            m_ReSaleNumber = 0;
            m_GoodsId = 0;
            TaxInPrice = 0;
        }

    }

    public class GoodsInfoForPurchase
    {
        public double LastInPrice { get; set; }
        public double AvgInPrice { get; set; }
        public double StandingAC { get; set; }
        public int StoreId { get; set; }
        public int StockNum { get; set; }
        public int GoodsId { get; set; }
        public string Model { get; set; }
        public int BranchId { get; set; }
        public int ResalesNum { get; set; }

        public GoodsInfoForPurchase()
        {
            LastInPrice = 0;
            AvgInPrice = 0;
            StandingAC = 0;
            StoreId = 0;
            StockNum = 0;
            GoodsId = 0;
            Model = "";
            BranchId = 0;
            ResalesNum = 0;
        }
    }

    public class GoodsSaleInfo
    {
        public int SaleNumAll { get; set; }
        public int SaleNum3Month { get; set; }
        public int SaleNum1Month { get; set; }
        public int SaleCountAll { get; set; }
        public int SaleCount3Month { get; set; }
        public int SaleCount1Month { get; set; }
        public int FavNum { get; set; }
        public int ViewNum { get; set; }
    }

    /// <summary>
    /// 商品参数
    /// </summary>
    public class GoodsModel
    {
        public int Id { get; set; }
        public int GoodsId { get; set; }
        public int Model1Id { get; set; }
        public int Model2Id { get; set; }
        public int Model3Id { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public GoodsModel()
        {
            Id = 0;
            GoodsId = 0;
            Model1Id = 0;
            Model2Id = 0;
            Model3Id = 0;
            UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@GoodsId", GoodsId));
            arrayList.Add(new SqlParameter("@Model1Id", Model1Id));
            arrayList.Add(new SqlParameter("@Model2Id", Model2Id));
            arrayList.Add(new SqlParameter("@Model3Id", Model3Id));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("GoodsModel", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("GoodsModel", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from GoodsModel where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                Model1Id = DBTool.GetIntFromRow(row, "Model1Id", 0);
                Model2Id = DBTool.GetIntFromRow(row, "Model2Id", 0);
                Model3Id = DBTool.GetIntFromRow(row, "Model3Id", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from GoodsModel where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }

    /// <summary>
    /// 商品参数类型
    /// </summary>
    public class GoodsModelType
    {
        public int Id { get; set; }
        public int GoodsId { get; set; }
        public string Name { get; set; }
        public string EnName { get; set; }
        public int ModelLevel { get; set; }
        public int Sort { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public GoodsModelType()
        {
            Id = 0;
            GoodsId = 0;
            Name = "";
            EnName = "";
            ModelLevel = 0;
            Sort = 0;
            UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@GoodsId", GoodsId));
            arrayList.Add(new SqlParameter("@Name", Name));
            arrayList.Add(new SqlParameter("@EnName", EnName));
            arrayList.Add(new SqlParameter("@ModelLevel", ModelLevel));
            arrayList.Add(new SqlParameter("@Sort", Sort));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("GoodsModelType", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("GoodsModelType", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from GoodsModelType where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                Name = DBTool.GetStringFromRow(row, "Name", "");
                EnName = DBTool.GetStringFromRow(row, "EnName", "");
                ModelLevel = DBTool.GetIntFromRow(row, "ModelLevel", 0);
                Sort = DBTool.GetIntFromRow(row, "Sort", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from GoodsModelType where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        public int Savesubmodeltype(int GoodsId, GoodsModelType[] GoodsModel)//添加属性值
        {
            int i = 0;
            foreach (GoodsModelType gmt in GoodsModel)
            {
                GoodsModelType gmtype = new GoodsModelType();
                gmtype.Id = gmt.Id;
                gmtype.Load();
                gmtype.GoodsId = GoodsId;
                gmtype.Name = gmt.Name;
                gmtype.ModelLevel = gmt.ModelLevel;
                gmtype.Sort = gmt.Sort;
                gmtype.UpdateTime = DateTime.Now;
                if (gmtype.Save() > 0)
                {
                    i += 1;
                }
            }
            return i;
        }

        public DataSet ReadModelType(int GoodsId) //获取属性
        {
            string sql = string.Format("select * from GoodsModelType where GoodsId={0} order by Sort", GoodsId);
            return m_dbo.GetDataSet(sql);
        }
    }

    /// <summary>
    /// 商品组合明细
    /// </summary>
    public class GoodsGroupDetail
    {
        public int Id { get; set; }
        public int GoodsGroupId { get; set; }
        public int GoodsId { get; set; }
        public int PackageId { get; set; }
        public int Num { get; set; }
        public int UserId { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public GoodsGroupDetail()
        {
            Id = 0;
            GoodsGroupId = 0;
            GoodsId = 0;
            PackageId = 0;
            Num = 0;
            UserId = 0;
            UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@GoodsGroupId", GoodsGroupId));
            arrayList.Add(new SqlParameter("@GoodsId", GoodsId));
            arrayList.Add(new SqlParameter("@PackageId", PackageId));
            arrayList.Add(new SqlParameter("@Num", Num));
            arrayList.Add(new SqlParameter("@UserId", UserId));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("GoodsGroupDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("GoodsGroupDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from GoodsGroupDetail where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                return LoadFromRow(row);
            }
            return false;
        }
        public bool Load(int GoodsGroupId, int GoodsId)
        {
            string sql = string.Format("select * from GoodsGroupDetail where GoodsGroupId={0} and GoodsId={1}", GoodsGroupId, GoodsId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                return LoadFromRow(row);
            }
            return false;
        }
        private bool LoadFromRow(DataRow row)
        {
            Id = DBTool.GetIntFromRow(row, "Id", 0);
            GoodsGroupId = DBTool.GetIntFromRow(row, "GoodsGroupId", 0);
            GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
            PackageId = DBTool.GetIntFromRow(row, "PackageId", 0);
            Num = DBTool.GetIntFromRow(row, "Num", 0);
            UserId = DBTool.GetIntFromRow(row, "UserId", 0);
            UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
            return true;
        }

        public bool Delete()
        {
            string sql = string.Format("Delete from GoodsGroupDetail where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        public DataSet GetGoodsGroupDetail(int GoodsId)//获取组合商品
        {
            string sql = string.Format("select * from View_GoodsGroupDetail where GoodsGroupId={0} ", GoodsId);
            return m_dbo.GetDataSet(sql);
        }




    }
    /// <summary>
    /// 商品所有子规格类别
    /// </summary>
    public class GoodsModelList
    {
        private int GoodsId;
        private string GoodsName;
        public string mGoodsName
        {
            get { return GoodsName; }
            set { GoodsName = value; }
        }
        private int ModelId;
        public int mModelId
        {
            get { return ModelId; }
            set { ModelId = value; }
        }
        private int Model1Id;
        private int Model2Id;
        private int Model3Id;
        private decimal Price;
        private int IsVisible;
        private int ParentId;
        private decimal InPrice;

        public int mModel1Id
        {
            get { return Model1Id; }
            set { Model1Id = value; }
        }
        public int mModel2Id
        {
            get { return Model2Id; }
            set { Model2Id = value; }
        }
        public int mModel3Id
        {
            get { return Model3Id; }
            set { Model3Id = value; }
        }
        public int mGoodsId
        {
            get { return GoodsId; }
            set { GoodsId = value; }
        }
        public decimal mPrice
        {
            get { return Price; }
            set { Price = value; }
        }
        public int mIsVisible
        {
            get { return IsVisible; }
            set { IsVisible = value; }
        }
        public int mParentId
        {
            get { return ParentId; }
            set { ParentId = value; }
        }
        public decimal mInPrice
        {
            get { return InPrice; }
            set { InPrice = value; }
        }

    }

    #region 废弃重复、没有引用的代码或者

    ///// <summary>
    ///// 查询spu   （政采云）
    ///// </summary>
    ///// <param name="categoryId"></param>
    ///// <param name="brandId"></param>
    ///// <param name="specificationMatch"></param>
    ///// <returns></returns>
    //public DataSet Read_GoodsSN(int categoryId, int brandId, string specificationMatch, int ProjectId)
    //{
    //    string sql = string.Format("select * from Goods inner  join TPI_Goods on Goods.ID= TPI_Goods.GoodsId  where  TypeId ={0} and  BrandId={1} and Goods.SN like  '%{2}%' and ProjectId={3}", categoryId, brandId, specificationMatch, ProjectId);
    //    return m_dbo.GetDataSet(sql);

    //}

    ////根据商品商品名称找到商品编号  //为查找到引用
    //public DataSet Read_goodsId(string GoodsName)
    //{
    //    string sql = string.Format("select * from  Goods where GoodsName ='{0}' and BranchId=196", GoodsName);
    //    return m_dbo.GetDataSet(sql);
    //}

    ///// <summary>
    /////读取所有商品分类，使用DataTable的select查询机制 //为查找到引用
    ///// </summary>
    ///// <returns></returns>
    //public DataSet Read_GoodsType()
    //{
    //    string sql = "select * from GoodsType where IsVisible=1 order by Code";
    //    return m_dbo.GetDataSet(sql);
    //}
    //为查找到引用
    //public bool CheckIsHave(string goodsName)
    //{
    //    string sql =string.Format( "select * from Goods where GoodsName='{0}'",goodsName);
    //    DataSet ds = m_dbo.GetDataSet(sql);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    { 
    //        return true;
    //    }else
    //    {
    //        return false;
    //    }
    //}

    //为查找到引用
    //public DataSet Loads()
    //{
    //    string sql = string.Format("select * from Goods where id={0}", Id);
    //    return m_dbo.GetDataSet(sql);
    //}

    //public DataSet BatchGetGoodsById(int[] goodsId)
    //{
    //    string sql = "select * from goods where Id in ( ";
    //    if (goodsId.Length > 0)
    //    {
    //        foreach (var item in goodsId)
    //        {
    //            sql += item.ToString();
    //            if (item < goodsId.Length - 1)
    //            {
    //                sql += ",";
    //            }
    //        }
    //    }
    //    sql += ")";
    //    return m_dbo.GetDataSet(sql);
    //}

    ///// <summary>
    ///// 读取数据goods表
    ///// </summary>
    ///// <returns></returns>
    //public DataSet ReadGoodsId()
    //{
    //    string sql = string.Format("select* from Goods where ID ={0}");
    //    return m_dbo.GetDataSet(sql);

    //}
    #endregion




}
