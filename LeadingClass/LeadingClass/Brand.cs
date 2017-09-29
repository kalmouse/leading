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
    public class Brand:BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PY { get; set; }
        public string EnglishName { get; set; }
        public string Company { get; set; }
        public string ProductPlace { get; set; }
        public string WebSite { get; set; }
        public string Image { get; set; }
        public int Hot { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public Brand()
        {
            Id = 0;
            Name = "";
            PY = "";
            EnglishName = "";
            Company = "";
            ProductPlace = "";
            WebSite = "";
            Image = "";
            Hot = 0;
            UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        /// <summary>
        /// Brand Insert
        /// </summary>
        /// <returns></returns>
        public int Insert() {
            this.PY= CommenClass.StringTools.GetFirstPYLetter("("+Name+")"+EnglishName); 
            string sql = SqlHelper.GetInsertSQL(this);
            this.Id = m_dbo.InsertData(sql);
            return this.Id;
        }
        /// <summary>
        /// Brand Update
        /// </summary>
        /// <returns></returns>
        public bool Update() {
            bool result = false;
            this.PY = CommenClass.StringTools.GetFirstPYLetter("(" + Name + ")" + EnglishName);
            string sql = SqlHelper.GetUpdateSQL(this);
            result = m_dbo.UpdateData(sql);
            return result;
        }
        /// <summary>
        /// Get Brand By Id
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            string sql = SqlHelper.GetSelectSQL(this);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                Name = DBTool.GetStringFromRow(row, "Name", "");
                PY = DBTool.GetStringFromRow(row, "PY", "");
                EnglishName = DBTool.GetStringFromRow(row, "EnglishName", "");
                Company = DBTool.GetStringFromRow(row, "Company", "");
                ProductPlace = DBTool.GetStringFromRow(row, "ProductPlace", "");
                WebSite = DBTool.GetStringFromRow(row, "WebSite", "");
                Image = DBTool.GetStringFromRow(row, "Image", "");
                Hot = DBTool.GetIntFromRow(row, "Hot", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        /// <summary>
        /// Delete Brand by Id
        /// </summary>
        /// <returns></returns>
 
        public bool Delete()
        {
            string sql = SqlHelper.GetDeleteSQL(this);
            return m_dbo.ExecuteNonQuery(sql);
        }
       

        /// <summary>
        /// 根据品牌查询品牌编号
        /// </summary>
        /// <returns></returns>
        public DataSet GoodsName(string brand_name)
        {
            string sql = string.Format("select * from  dbo.Brand where Name like '%{0}%'",brand_name);
            return m_dbo.GetDataSet(sql);
        }
    }

    public class BrandOption
    {
        private int m_BrandId;
        private string m_BrandName;
        private string m_PPCode;
        private string m_PCode;
        private string m_Code;
        private PageModel m_PageModel;
        public int BrandId
        {
            get { return m_BrandId; }
            set { m_BrandId = value; }
        }

        public string BrandName
        {
            get { return m_BrandName; }
            set { m_BrandName = value; }
        }
        public string PPCode
        {
            get { return m_PPCode; }
            set { m_PPCode = value; }
        }
        public string PCode {
            get { return m_PCode; }
            set { m_PCode = value; }
        }
        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }

        public PageModel PageModel
        {
            get { return m_PageModel; }
            set { m_PageModel = value; }
        }

        public BrandOption()
        {
            BrandId = 0;
        }
    }

    public class BrandManager
    {
        private DBOperate m_dbo;
        public BrandOption brandOption;
        public PageModel pageModel;
        public BrandManager()
        {
            m_dbo = new DBOperate();
            brandOption =new BrandOption();
            pageModel=new PageModel();
        }

      

        /// <summary>
        /// 读取品牌对应的类别,查询出一级，二级，三级的类别名称，code，商品数量 add by quxiaoshan 
        /// </summary>
        /// <returns></returns>
        public DataSet ReadGoodsTypeByBrandId()
        {
            string sql = string.Format(@"select SUBSTRING ( PCode,1, 2) as PPCode ,(select TypeName from GoodsType where Code= SUBSTRING ( dbo.GoodsType.PCode,1, 2) ) as PPTypeName ,
 PCode,(select TypeName from GoodsType where Code=  dbo.GoodsType.PCode ) as PTypeName ,
Code,TypeName, COUNT(Code) as TypeNum
from dbo.Goods join dbo.GoodsType on dbo.Goods.TypeId=dbo.GoodsType.ID where BrandId={0} and IsShelves=1 and Goods.IsVisible =1
group by Code,PCode ,TypeName;", brandOption.BrandId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取品牌下的类别下的商品
        /// </summary>
        /// <returns></returns>
        public DataSet ReadGoodsByBrandIdCode()
        {
            string sql = string.Format("select dbo.Goods.ID as GoodsId,TypeId,DisplayName,Price,model,Package,Unit,HomeImage,BrandId,PCode,Code,ParentId from dbo.Goods join dbo.GoodsType on dbo.goods.TypeId=dbo.GoodsType.ID where 1=1  and IsShelves=1 and Goods.IsVisible =1");
            sql += sqlBrandIdCode();
            return m_dbo.GetDataSet(sql, (pageModel.CurrentPage - 1) * pageModel.PageSize, pageModel.PageSize);
        }

        /// <summary>
        /// 读取通过brandId，code相应商品的个数
        /// </summary>
        /// <returns></returns>
        public int ReadGoodsCountByBrandIdCode()
        {
            string sql = "select COUNT(*) as count from dbo.Goods join dbo.GoodsType on dbo.goods.TypeId=dbo.GoodsType.ID where 1=1 and IsShelves=1 and Goods.IsVisible =1";
            sql+= sqlBrandIdCode();
            DataRowCollection collection= m_dbo.GetDataSet(sql).Tables[0].Rows;
            if (collection.Count > 0)
            {
                return DBTool.GetIntFromRow(collection[0], "count", 0);
            }
            return 0;
        }

        /// <summary>
        /// 通过brandid，code等条件组成的查询语句
        /// </summary>
        /// <returns></returns>
        private string sqlBrandIdCode()
        {
            string sql = "";
            if (brandOption.BrandId > 0)
            {
                sql += string.Format(" and BrandId ={0} ", brandOption.BrandId);
            }
            if (brandOption.Code != "0")
            {
                sql += string.Format(" and Code like '{0}%' ", brandOption.Code);
            }
            else if (brandOption.PCode != "0")
            {
                sql += string.Format(" and Code like '{0}%' ", brandOption.PCode);
            }
            else if (brandOption.PPCode != "0")
            {
                sql += string.Format(" and Code like '{0}%' ", brandOption.PPCode);
            }
            return sql;
        }
     
        
    }
}

