using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class GoodsBranch
    {
        public int Id { get; set; }
        public int GoodsId { get; set; }
        public int BranchId { get; set; }
        public int IsVisible { get; set; }
        public int IsAllow { get; set; }
        public int IsPublic { get; set; }
        public int UserId { get; set; }
        public DateTime UpdateTime { get; set; }
        public int IsWeb { get;set;}
        public decimal Price { get;set;}
        public decimal PriceRate { get;set;}
        public int IsChangePrice { get; set; }
        public int IsChangeRecommend { get; set; }
        public int Recommend { get; set; }
        private DBOperate m_dbo;

        public GoodsBranch()
        {
            Id = 0;
            GoodsId = 0;
            BranchId = 0;
            IsVisible = 0;
            IsAllow = 0;
            IsPublic = 0;
            UserId = 0;
            UpdateTime = DateTime.Now;
            IsWeb=0;
            Price=0;
            PriceRate=0;
            IsChangePrice = 0;
            IsChangeRecommend = 0;
            Recommend = 1;
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
            arrayList.Add(new SqlParameter("@BranchId", BranchId));
            arrayList.Add(new SqlParameter("@IsVisible", IsVisible));
            arrayList.Add(new SqlParameter("@IsAllow", IsAllow));
            arrayList.Add(new SqlParameter("@IsPublic", IsPublic));
            arrayList.Add(new SqlParameter("@UserId", UserId));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@IsWeb", IsWeb));
            arrayList.Add(new SqlParameter("@Price", Price));
            arrayList.Add(new SqlParameter("@PriceRate", PriceRate));
            arrayList.Add(new SqlParameter("@IsChangePrice", IsChangePrice));
            arrayList.Add(new SqlParameter("@IsChangeRecommend", IsChangeRecommend));
            arrayList.Add(new SqlParameter("@Recommend",Recommend));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("GoodsBranch", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("GoodsBranch", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from GoodsBranch where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }
        public bool LoadByBranchIdGoodsId(int branchId,int goodsId)
        {
            string sql = string.Format("select * from GoodsBranch where BranchId={0} and GoodsId={1}", branchId,goodsId);
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
            BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
            IsVisible = DBTool.GetIntFromRow(row, "IsVisible", 0);
            IsAllow = DBTool.GetIntFromRow(row, "IsAllow", 0);
            IsPublic = DBTool.GetIntFromRow(row, "IsPublic", 0);
            UserId = DBTool.GetIntFromRow(row, "UserId", 0);
            UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
            IsWeb = DBTool.GetIntFromRow(row, "IsWeb", 0);
            Price = DBTool.GetDecimalFromRow(row, "Price", 0);
            PriceRate = DBTool.GetDecimalFromRow(row, "PriceRate", 0);
            IsChangePrice = DBTool.GetIntFromRow(row, "IsChangePrice", 0);
            IsChangeRecommend = DBTool.GetIntFromRow(row, "IsChangeRecommend", 0);
            Recommend = DBTool.GetIntFromRow(row, "Recommend", 0);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            string sql = string.Format("Delete from GoodsBranch where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        //获取总数
        public DataSet GetCountNum(string LeiBie,string Name,int BranchId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" select COUNT(Id) as countnum from dbo.View_GoodsBranch where BranchId={0} ",BranchId);
            if (Name != "" && Name != null)
            {
                sql.AppendFormat(" and (DisplayName like '%{0}%' ", Name);
                int nam = 0;
                Int32.TryParse(Name, out nam);
                if (nam > 0)
                {
                    sql.AppendFormat("  or GoodsId={0} ", Name);
                }
                sql.AppendFormat(" ) ");
            }
                if (LeiBie != "-1")
                {
                    sql.AppendFormat(" and TypeId in ({0}) ", LeiBie);
                }
            return m_dbo.GetDataSet(sql.ToString());
        }
        //分页
        public DataSet ReadGoodsByPage(int PageSize,int PageNum,string LeiBie,string Name,int BranchId)
        {
            int pageCount = 0;
            int count = 0;
            DataTable dt=GetCountNum(LeiBie, Name,BranchId).Tables[0];
            Int32.TryParse(dt.Rows[0]["countnum"].ToString(), out count);
            if (PageSize>0)
            {
                double d = Convert.ToDouble(count) / Convert.ToDouble(PageSize);
                pageCount = Convert.ToInt32(Math.Ceiling(d));
            }
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from (select *,ROW_NUMBER() over(order by Id) as rownumber from  dbo.View_GoodsBranch where BranchId={0} ",BranchId);
            if (Name!=""&&Name!=null)
            {
                sql.AppendFormat(" and (DisplayName like '%{0}%' ",Name);
                int nam = 0;
                Int32.TryParse(Name, out nam);
                if (nam > 0)
                {
                    sql.AppendFormat("  or GoodsId={0} ", Name);
                }
                sql.AppendFormat(" ) ");
            }
            if (LeiBie!="-1")
            {
                    sql.AppendFormat(" and TypeId in ({0}) ",LeiBie);
                
            }
            sql.AppendFormat(" )t where ");  
            if (PageNum != pageCount)
            {
                if (PageNum == 1)
                {
                    sql.AppendFormat(" t.rownumber between 1 and {0}*{1} ", PageSize, PageNum);
                }
                else
                {
                    sql.AppendFormat(" t.rownumber between {0}*({1}-1)+1 and {0}*{1} ",PageSize,PageNum);
                }
            }
            else
            {
                sql.AppendFormat(" t.rownumber>({0}*({1}-1)) ",PageSize,PageNum);
            }
            
            return m_dbo.GetDataSet(sql.ToString());
        }
        public int SaveAll(string Gid,string BranchId,string UserId)
        {
            int result = 0;
            string[] str = Gid.Split(',');
            for (int i = 0; i < str.Length; i++)
			{
                if(str[i]!="" && str[i]!="all")
                {
                    GoodsBranch branch = new GoodsBranch();
                    branch.GoodsId = Convert.ToInt32(str[i]);
                    LeadingClass.Goods manager = new Goods();
                    manager.Id = Convert.ToInt32(str[i]);
                    manager.Load();
                    branch.Price = Convert.ToDecimal(manager.Price);
                    branch.IsWeb = 1;
                    branch.BranchId = Convert.ToInt32(BranchId);
                    branch.UserId = Convert.ToInt32(UserId);
                    branch.PriceRate = 0;
                    branch.Save();
                    result = branch.Id;
                }
			}
            return result;
        }
        /// <summary>
        /// 网站显示合作伙伴独有商品的价格，上下架
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public DataSet GetGoodsInfoByBranchId(int branchId,int goodsId)
        {
            string sql = string.Format("select Price from GoodsBranch where BranchId={0} and GoodsId={1}",branchId,goodsId);
            return m_dbo.GetDataSet(sql);
        }

    }
}
