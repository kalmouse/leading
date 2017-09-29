//2017-4-18 yanghaiyang   需求008
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class GoodsType
    {
        private int m_Id;
        private string m_TypeName;
        private string m_EnTypeName;
        private string m_Code;
        private string m_PCode;
        private int m_Level;
        private int m_PriceRate;
        private string m_Alias;
        private int m_IsVisible;
        private int m_Rate;
        private string m_GuiGe;
        private string m_JiaGe;
        private string m_BieMing;
        private string m_PY;
        private DateTime m_UpdateTime;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public string TypeName { get { return m_TypeName; } set { m_TypeName = value; } }
        public string EnTypeName { get { return m_EnTypeName; } set { m_EnTypeName = value; } }
        public string Code { get { return m_Code; } set { m_Code = value; } }
        public string PCode { get { return m_PCode; } set { m_PCode = value; } }
        public int Level { get { return m_Level; } set { m_Level = value; } }
        public int PirceRate { get { return m_PriceRate; } set { m_PriceRate = value; } }//PriceRate 写成了PirceRate update by quxiaoshan 2014-12-15
        public string Alias { get { return m_Alias; } set { m_Alias = value; } }
        public int IsVisible { get { return m_IsVisible; } set { m_IsVisible = value; } }
        public int Rate { get { return m_Rate; } set { m_Rate = value; } }
        public string GuiGe { get { return m_GuiGe; } set { m_GuiGe = value; } }
        public string JiaGe { get { return m_JiaGe; } set { m_JiaGe = value; } }
        public string BieMing { get { return m_BieMing; } set { m_BieMing = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public string PY { get { return m_PY; } set { m_PY = value; } }
        public PageModel pageModel { get; set; }

        public GoodsType()
        {
            m_Id = 0;
            m_TypeName = "";
            m_EnTypeName = "";
            m_Code = "";
            m_PCode = "";
            m_Level = 0;
            m_PriceRate = 100;
            m_Alias = "";
            m_IsVisible = 1;
            m_Rate = 100;
            m_BieMing = "";
            m_GuiGe = "";
            m_JiaGe = "";
            m_UpdateTime = DateTime.Now;
            pageModel = new PageModel();
            m_PY = "";
            m_dbo = new DBOperate();
        }
        public GoodsType(string connectionString)
        {
            m_Id = 0;
            m_TypeName = "";
            m_EnTypeName = "";
            m_Code = "";
            m_PCode = "";
            m_Level = 0;
            m_PriceRate = 100;
            m_Alias = "";
            m_IsVisible = 1;
            m_Rate = 100;
            m_BieMing = "";
            m_GuiGe = "";
            m_JiaGe = "";
            m_UpdateTime = DateTime.Now;
            m_PY = "";
            pageModel = new PageModel();
            m_dbo = new DBOperate(connectionString);
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@TypeName", m_TypeName));
            arrayList.Add(new SqlParameter("@EnTypeName", m_EnTypeName));
            arrayList.Add(new SqlParameter("@Code", m_Code));
            arrayList.Add(new SqlParameter("@PCode", m_PCode));
            arrayList.Add(new SqlParameter("@Level", m_Level));
            arrayList.Add(new SqlParameter("@PriceRate", m_PriceRate));
            arrayList.Add(new SqlParameter("@Alias", m_Alias));
            arrayList.Add(new SqlParameter("@IsVisible", m_IsVisible));
            arrayList.Add(new SqlParameter("@Rate", m_Rate));
            arrayList.Add(new SqlParameter("@GuiGe", m_GuiGe));
            arrayList.Add(new SqlParameter("@JiaGe", m_JiaGe));
            arrayList.Add(new SqlParameter("@BieMing", m_BieMing));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            arrayList.Add(new SqlParameter("@PY", CommenClass.StringTools.GetFirstPYLetter("("+m_TypeName+")"+m_EnTypeName)));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("GoodsType", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("GoodsType", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }

        public bool Delete(int Id)
        {
            string sql = string.Format("Delete from GoodsType where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        } 
        public bool Load()
        {
            string sql = string.Format("select * from GoodsType where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return LoadFromRow(ds.Tables[0].Rows[0]);
                }
            }
            return false;
        }
        public bool Load(int id)
        {
            this.Id = id;
            return Load();
        }
        public bool Load(string Code)
        {
            string sql = string.Format("select * from GoodsType where Code='{0}'", Code);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
               return LoadFromRow(ds.Tables[0].Rows[0]);
            }
            return false;
        }
        private bool LoadFromRow(DataRow row)
        {
            m_Id = DBTool.GetIntFromRow(row, "Id", 0);
            m_TypeName = DBTool.GetStringFromRow(row, "TypeName", "");
            m_EnTypeName = DBTool.GetStringFromRow(row, "EnTypeName", "");
            m_Code = DBTool.GetStringFromRow(row, "Code", "");
            m_PCode = DBTool.GetStringFromRow(row, "PCode", "");
            m_Level = DBTool.GetIntFromRow(row, "Level", 0);
            m_PriceRate = DBTool.GetIntFromRow(row, "PriceRate", 0);
            m_Alias = DBTool.GetStringFromRow(row, "Alias", "");
            m_IsVisible = DBTool.GetIntFromRow(row, "IsVisible", 0);
            m_Rate = DBTool.GetIntFromRow(row, "Rate", 0);
            m_BieMing = DBTool.GetStringFromRow(row, "BieMing", "");
            m_GuiGe = DBTool.GetStringFromRow(row, "GuiGe", "");
            m_JiaGe = DBTool.GetStringFromRow(row, "JiaGe", "");
            m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
            return true;

        }

        ///// <summary>
        /////读取所有商品分类，使用DataTable的select查询机制 add by quxiaoshan 2015-4-21
        ///// </summary>
        ///// <returns></returns>
        //public DataSet ReadGoodsType() 
        //{
        //    string sql = "select * from GoodsType where IsVisible=1 order by Code";
        //    return m_dbo.GetDataSet(sql);
        //}
        ///// <summary>
        ///// 读取所有商品分类，首页菜单栏显示14个商品大类
        ///// </summary>
        ///// <returns></returns>
        //public DataSet ReadGoodsTypeByLevel()
        //{
        //    string sql = "select * from GoodsType where IsVisible=1 and level=1";
        //    return m_dbo.GetDataSet(sql);
        //}


        //public DataTable GetTwoLeveGoodsType()
        //{
        //    string sql = "select * from GoodsType where IsVisible=1  and level=2";
        //    return m_dbo.GetDataSet(sql).Tables[0];
        //}

        //public DataTable GetThreeLeveGoodsType()
        //{
        //    string sql = "select * from GoodsType where IsVisible=1  and level=3";
        //    return m_dbo.GetDataSet(sql).Tables[0];
        //}

        /// <summary>
        /// 读取子分类 
        /// </summary>
        /// <returns></returns>
        public DataSet ReadSubTypes()
        {
            string sql = " select * from GoodsType where 1=1";
            if (this.Code != "")
            {
                sql += string.Format(" and Code like '{0}%'", this.Code);
            }
            if (this.Level > 0)
            {
                sql += string.Format(" and Level={0}", this.Level);
            }
            if (this.PCode != "")
            {
                sql += string.Format(" or PCode like '{0}%'", this.PCode);
            }
            if (this.IsVisible > 0)
            {
                sql += " and IsVisible=1 ";
            }
            sql += " order by Code";
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取商品类别
        /// </summary>
        /// <returns></returns>
        public DataSet ReadGoodsType()
        {
            string sql = " select * from GoodsType where 1=1";
            if (this.Id > 0)
            {
                sql += string.Format(" and Id = {0}", this.Id);
            }
            if (this.Code != "")
            {
                sql += string.Format(" and Code = '{0}'", this.Code);
            }
            if (this.Level > 0)
            {
                sql += string.Format(" and Level={0}", this.Level);
            }
            if (this.PCode != "")
        {
                sql += string.Format(" and PCode = '{0}'", this.PCode);
        }
            if (this.TypeName != "")
        {
                sql += string.Format(" and TypeName = '{0}'", this.TypeName);
        }
            if (this.IsVisible > 0)
        {
                sql += " and IsVisible=1 ";
            }
            sql += " order by Code";
            return m_dbo.GetDataSet(sql);
        }
        ///// <summary>
        ///// 读取同级所有分类
        ///// </summary>
        ///// <returns></returns>
        //public DataSet ReadSibings()
        //{
        //    string sql = string.Format(" select * from GoodsType where Code like '{0}%' and Level={1} and IsVisible=1 order by Code ", this.PCode, this.Level);
        //    return m_dbo.GetDataSet(sql);
        //}
        ///// <summary>
        ///// 读取所有子分类
        ///// </summary>
        ///// <returns></returns>
        //public DataSet ReadAllSubTypes(string code)
        //{
        //    string sql = string.Format(" select * from GoodsType where Code like '{0}%' and IsVisible=1 order by Code ", code);
        //    return m_dbo.GetDataSet(sql);
        //}
        ///// <summary>
        ///// 读取所有三级子分类
        ///// </summary>
        ///// <returns></returns>
        //public DataSet ReadAll3Types()
        //{
        //    string sql = string.Format(" select * from GoodsType where Code like '{0}%' and level=3 and IsVisible=1 order by Code ", m_Code);
        //    return m_dbo.GetDataSet(sql);
        //}
        /// <summary>
        /// 读取本类销售排行
        /// </summary>
        /// <returns></returns>
        public DataSet ReadSaleTopN(int topnum)
        {
            string sql = string.Format("select top {1} GoodsId,SN,GoodsName,HomeImage,Price,Unit,sum(num) as Num from web_orderdetail where Code='{0}'  and IsVisible=1  and plandate >'{2}' group by GoodsId,SN,GoodsName,Price,Unit,HomeImage order by Num Desc", this.Code, topnum, DateTime.Now.AddMonths(-3).ToShortDateString());
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取本类品牌
        /// </summary>
        /// <returns></returns>
        public DataSet ReadBrands()
        {
            string sql = string.Format("select BrandId,BrandName,EnglishName,count(goodsId) as Num from view_goods where Code like '{0}%' and IsVisible=1 and BrandName is not null group by BrandId,BrandName,EnglishName order by Num desc", this.Code);
            return m_dbo.GetDataSet(sql);
        }
 
        /// <summary>
        /// 生成商品分类的列表，包含一级类别对应的二级和三级类别 add by quxiaoshan 2014-12-16
        /// </summary>
        /// <returns></returns>
        public DataSet ReadAllGoodsType(int typeId)
        {
            this.Id = typeId;
            if (!this.Load())
                return null;

            string sql = "select * from dbo.GoodsType where  Level != 1 ";
            sql += string.Format(" and Code like '{0}%' ", this.Code.Substring(0, 2));
            sql += " order by code";
            return m_dbo.GetDataSet(sql);
        }

        #region 促销品

        /// <summary>
        /// 查询促销商品的类别 2014-1-16 add by quxiaoshan 
        /// </summary>
        /// <returns></returns>
        public DataSet ReadPriceOffGoodsType(int level)
        {
            string where = "and cuxiao >0 and cuxiaoDate>GETDATE() and dbo.Goods.IsVisible=1";
            string selectCode = "";
            string goodsnumWhere = "";
            string orderby = "";
            if (level == 2)
            {
                selectCode = "PCode";
            }
            else if (level == 3)
            {
                selectCode = "Code";
                goodsnumWhere += string.Format("( select COUNT(ID) from dbo.Goods where TypeId=dbo.GoodsType.ID  {0} ) as GoodsNum,", where);//因为二级类别未对应商品的个数，无发查询出GoodsNum
                orderby += " order by GoodsNum desc";
            }
            
            string sql = string.Format(@"select ID,TypeName, pcode,Code,[Level],{0}
                 IsVisible from  dbo.GoodsType where 
code in  (select {1} from dbo.GoodsType join dbo.Goods on dbo.GoodsType.ID =dbo.Goods.TypeId where  1=1 {2}) {3} ", goodsnumWhere, selectCode, where, orderby);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 通过商品的类别查找出,相应的促销商品 2014-1-17 add by quxiaoshan 
        /// </summary>
        /// <returns></returns>
        public DataSet ReadPriceOffByGoodsType()
        {
            string sqlWhere = " and cuxiao >0 and cuxiaoDate>GETDATE() and dbo.Goods.IsVisible=1";
            string sql = string.Format(" select *,dbo.Goods.ID as GoodsId from dbo.GoodsType join dbo.Goods on dbo.GoodsType.ID=dbo.Goods.TypeId  where  1=1  {0}", sqlWhere);
            if (m_Id != 0)
            {
                GoodsType gt = new GoodsType();
                gt.Id = m_Id;
                gt.Load();
                sql += string.Format(" and Code like '{0}%' ", gt.Code);
            }
            sql += " order by dbo.Goods.Recommend desc  ";
            return m_dbo.GetDataSet(sql, (pageModel.CurrentPage - 1) * pageModel.PageSize, pageModel.PageSize);
        }

        /// <summary>
        /// 促销品的商品总条数
        /// </summary>
        /// <returns></returns>
        public int ReadPriceOffTotalRows()
        {
            string sqlWhere = " and cuxiao >0 and cuxiaoDate>GETDATE() and dbo.Goods.IsVisible=1";
            string sql = string.Format("select COUNT(*) from dbo.GoodsType join dbo.Goods on dbo.GoodsType.ID=dbo.Goods.TypeId  where 1=1 {0}", sqlWhere);
            if (m_Id != 0)
            {
                GoodsType gt = new GoodsType();
                gt.Id = m_Id;
                gt.Load();
                sql += string.Format(" and Code like '{0}%' ", gt.Code);
            }
            DataSet ds = m_dbo.GetDataSet(sql);
            return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        }

        #endregion

        #region 畅销区

        /// <summary>
        /// 查询畅销商品的类别 2014-1-20 add by quxiaoshan 
        /// </summary>
        /// <returns></returns>
        public DataSet ReadHotGoodsType(int level)
        {
            string where = " and Recommend=6  and dbo.Goods.IsVisible=1  ";
            string selectCode = "";
            string goodsnumWhere = "";
            string orderby = "";
            if (level == 2)
            {
                selectCode = "PCode";
            }
            else if (level == 3)
            {
                selectCode = "Code";
                goodsnumWhere += string.Format(" ( select COUNT(ID) from dbo.Goods where TypeId=dbo.GoodsType.ID {0}  ) as GoodsNum ,", where);//因为二级类别未对应商品的个数，无发查询出GoodsNum
                orderby += " order by GoodsNum desc";
            }

            string sql = string.Format(@" select ID,TypeName, pcode,Code,[Level], {0} 
    IsVisible  from dbo.GoodsType where Code in 
(
  select {1} from dbo.GoodsType  join dbo.Goods on dbo.Goods.TypeId=dbo.GoodsType.ID where 1=1 {2}
 ) {3}", goodsnumWhere, selectCode, where, orderby);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 通过商品的类别查找出,相应的畅销商品 2014-1-20 add by quxiaoshan 
        /// </summary>
        /// <returns></returns>
        public DataSet ReadHotByGoodsType()
        {
            string sqlWhere = " and Recommend=6  and dbo.Goods.IsVisible=1  ";
            string sql = string.Format("select *,dbo.Goods.ID as GoodsId from dbo.GoodsType join dbo.Goods on dbo.GoodsType.ID=dbo.Goods.TypeId  where  1=1  {0}", sqlWhere);
            if (m_Id != 0)
            {
                GoodsType gt = new GoodsType();
                gt.Id = m_Id;
                gt.Load();
                sql += string.Format(" and Code like '{0}%' ", gt.Code);
            }
            sql += " order by dbo.Goods.Recommend desc  ";
            return m_dbo.GetDataSet(sql, (pageModel.CurrentPage - 1) * pageModel.PageSize, pageModel.PageSize);
        }

        /// <summary>
        /// 畅销品的商品总条数
        /// </summary>
        /// <returns></returns>
        public int ReadHotTotalRows()
        {
            string sqlWhere = " and Recommend=6  and dbo.Goods.IsVisible=1 ";
            string sql = string.Format("select COUNT(*) from dbo.GoodsType join dbo.Goods on dbo.GoodsType.ID=dbo.Goods.TypeId  where 1=1 {0}", sqlWhere);
            if (m_Id != 0)
            {
                GoodsType gt = new GoodsType();
                gt.Id = m_Id;
                gt.Load();
                sql += string.Format(" and Code like '{0}%' ", gt.Code);
            }
            DataSet ds = m_dbo.GetDataSet(sql);
            return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        }

        #endregion
        /// <summary>
        /// 读取一级二级三级类别
        /// </summary>
        /// <returns></returns>
        public DataSet Read_Goodstype() //int typeId and TypeId ={0}
        {
            string sql = string.Format(@"with GetGoodsInfo as 
	(
	select Goods.ID as GoodsId,Goods.DisplayName,Goods.Price,GoodsType.Code,GoodsType.ID as TypeId,Goods.GoodsRank,TypeName
	from  Goods   inner  join  GoodsType  on Goods.TypeId=GoodsType.ID 
	where  BranchId=1 and  IsShelves =1 and  goods.IsVisible =1 
	)
	select info.GoodsId as 商品编号,info.DisplayName as 商品名称, info.Price as 商品价格,info.GoodsRank as aa,
	(select top 1 TypeName from GoodsType where Code=(SELECT SUBSTRING(vg.Code,1,2))) as 第一类别,
	(select top 1 ID from GoodsType where Code=(SELECT SUBSTRING(vg.Code,1,2))) as 第一类别编号,
	(select top 1 TypeName from GoodsType where Code=(SELECT SUBSTRING(vg.Code,1,4))) as 第二类别,
	(select top 1 ID from GoodsType where Code=(SELECT SUBSTRING(vg.Code,1,4))) as 第二类别编号,
	(select top 1 TypeName from GoodsType where Code=vg.Code) as 第三类别,
	(select top 1 ID from GoodsType where Code=vg.Code) as 第三类别编号
	from View_Goods vg join GetGoodsInfo info on info.GoodsId = vg.GoodsId 
");
            return m_dbo.GetDataSet(sql);
        
        }

    }

    public class GoodsTypeManager
    {
        private DBOperate m_dbo;
        public GoodsTypeManager()
        {
            m_dbo = new DBOperate();
        }
        /// <summary>
        /// 读取商品分类
        /// </summary>
        /// <param name="IsHuancun">1：为网站缓存数据</param>
        /// <param name="IsVisible">1：可见，0：不可见;-1:全部状态;</param>
        /// <returns></returns>
        public DataSet ReadGoodsTypes()
        {
            string sql = " select *, displyName =CASE [level] when 1 then '1-'+TypeName when 2 then '2-'+TypeName   when 3 then '3-'+TypeName END from GoodsType order by Code ";           

            return m_dbo.GetDataSet(sql);
        }

        public DataSet ReadGoodsTypeLevel1() 
        {
            string sql = "select * from GoodsType where Level =1 order by Code";
            return m_dbo.GetDataSet(sql);
        }

        public bool ReadTypeCountGoods(int typeId)
        {
            string sql = string.Format("select COUNT(ID) from Goods where TypeId='{0}'", typeId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
       /// <summary>
       /// 读取类别Id  天津中环
       /// </summary>
       /// <returns></returns>
        public DataSet ReadtypeId() 
        {
            string sql = string.Format("select * from GoodsType  where TypeName  like '%{0}%'");
            return m_dbo.GetDataSet(sql);
           
        }
    }
}
