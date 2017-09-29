using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Collections;

namespace LeadingClass
{
    public  class Fav
    {
        private int m_Id;
        private int m_MemberId;
        private int m_GoodsId;
        private DateTime m_UpdateTime;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int MemberId { get { return m_MemberId; } set { m_MemberId = value; } }
        public int GoodsId { get { return m_GoodsId; } set { m_GoodsId = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public PageModel pageModel { get; set; }//add by quxiaoshan 2015-3-3
        public Fav()
        {
            m_Id = 0;
            m_MemberId = 0;
            m_GoodsId = 0;
            m_UpdateTime = DateTime.Now;
            pageModel=new PageModel();
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@MemberId", m_MemberId));
            arrayList.Add(new SqlParameter("@GoodsId", m_GoodsId));
            arrayList.Add(new SqlParameter("@UpdateTime",m_UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Fav", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Fav", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Fav where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);
                m_GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 读取所有收藏的排名--有待优化
        /// </summary>
        /// <returns></returns>
        public DataSet ReadFav() 
        {
            string sql = string.Format(@"select   distinct dbo.Goods.ID,dbo.Goods.TypeId,dbo.Goods.DisplayName,dbo.goods.Price,HomeImage, ( select COUNT(dbo.fav.id) from dbo.fav where dbo.fav.goodsid=dbo.goods.ID ) as FavNum, dbo.fav.updatetime
 from dbo.fav join dbo.Goods on dbo.fav.GoodsId=dbo.Goods.ID 
 group by dbo.Goods.ID,dbo.Goods.TypeId,dbo.Goods.DisplayName,dbo.goods.Price,HomeImage, dbo.fav.updatetime
 order by dbo.fav.updatetime desc;");
            return  m_dbo.GetDataSet(sql);
        }

        #region 我的收藏 add by quxiaoshan 2015-3-3

        /// <summary>
        /// 会员读取自己收藏的商品
        /// </summary>
        /// <returns></returns>
        public DataSet ReadFavByMemberId()
        {
            string sql = string.Format("select * from view_fav where memberId={0} order by UpdateTime desc", MemberId);//displayname 我也忘记了当时为什么要依照displayname 排序
            DataSet ds = m_dbo.GetDataSet(sql, (pageModel.CurrentPage-1) * pageModel.PageSize, pageModel.PageSize);
            return ds;
        }

        /// <summary>
        /// 获取当前用户收藏商品的个数
        /// </summary>
        /// <returns></returns>
        public int GetFavTotalRows()
        {
            string sql = string.Format("select COUNT(*) from dbo.View_Fav where MemberId={0}", MemberId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }
            return 0;
        }

        /// <summary>
        /// 该用户是否收藏过该商品
        /// </summary>
        /// <returns></returns>
        public bool IsFavByMemberIdGoodsId()
        {
            string sql = string.Format("select * from dbo.View_Fav where MemberId={0} and GoodsId={1}", m_MemberId, m_GoodsId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 删除收藏
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public bool DeleteFav(int goodsId)
        {
            string sql = string.Format(" delete from Fav where memberId={0}  ", MemberId);
            if (goodsId > 0)
            {
                sql += string.Format(" and GoodsId={0} ", goodsId);
            }
            return m_dbo.ExecuteNonQuery(sql);
        }

        #endregion
    }
}
