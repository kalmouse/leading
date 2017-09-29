using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
namespace LeadingClass
{
    public class GoodsPhoto
    {
        private int m_Id;
        private int m_GoodsId;
        private string m_Path;
        private string m_PhotoName;
        private int m_IsHomeImage;
        private int m_IsDetailPhoto;
        private int m_Sort;
        private int m_UserId;
        private DateTime m_UpdateTime;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int GoodsId { get { return m_GoodsId; } set { m_GoodsId = value; } }
        public string Path { get { return m_Path; } set { m_Path = value; } }
        public string PhotoName { get { return m_PhotoName; } set { m_PhotoName = value; } }
        public int IsHomeImage { get { return m_IsHomeImage; } set { m_IsHomeImage = value; } }
        public int IsDetailPhoto { get { return m_IsDetailPhoto; } set { m_IsDetailPhoto = value; } }
        public int Sort { get { return m_Sort; } set { m_Sort = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public GoodsPhoto()
        {
            m_Id = 0;
            m_GoodsId = 0;
            m_Path = "";
            m_PhotoName = "";
            m_IsHomeImage = 0;
            m_IsDetailPhoto = 0;
            m_Sort = 10;
            m_UserId = 0;
            m_UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        /// <summary>
        /// ERP:FGoods.cs
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@GoodsId", m_GoodsId));
            arrayList.Add(new SqlParameter("@Path", m_Path));
            arrayList.Add(new SqlParameter("@PhotoName", m_PhotoName));
            arrayList.Add(new SqlParameter("@IsHomeImage", m_IsHomeImage));
            arrayList.Add(new SqlParameter("@IsDetailPhoto", m_IsDetailPhoto));
            arrayList.Add(new SqlParameter("@Sort", m_Sort));
            arrayList.Add(new SqlParameter("@UserId", m_UserId));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("GoodsPhoto", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("GoodsPhoto", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            //更新 homeimage
            if (IsHomeImage == 1 && this.Id >0)
            {
                string homeimage = Path +"/"+PhotoName;
                Goods gs = new Goods();
                gs.Id = this.GoodsId;
                gs.Load();
                StringBuilder stb = new StringBuilder();
                stb.Append(string.Format(" update Goods set HomeImage ='{0}' where Id={1} ", homeimage, this.GoodsId));
                if (gs.ParentId == 2)
                {
                    stb.Append(string.Format(" update Goods set HomeImage ='{0}' where ParentId={1} ", homeimage, this.GoodsId));
                }

                m_dbo.ExecuteNonQuery(stb.ToString());
            }
            //更新 图片数量
            Goods g = new Goods();
            g.Id = GoodsId;
            g.UpdatePhotoNum();
            return this.Id;
        }
        /// <summary>
        /// 调用：ERP：FGoods.cs
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            string sql = string.Format("select * from GoodsPhoto where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                m_Path = DBTool.GetStringFromRow(row, "Path", "");
                m_PhotoName = DBTool.GetStringFromRow(row, "PhotoName", "");
                m_IsHomeImage = DBTool.GetIntFromRow(row, "IsHomeImage", 0);
                m_IsDetailPhoto = DBTool.GetIntFromRow(row, "IsDetailPhoto", 0);
                m_Sort = DBTool.GetIntFromRow(row, "Sort", 0);
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }

        /// <summary>
        ///分表读取
        /// </summary>
        /// <param name="GoodsId"></param>
        /// <returns></returns>
        public DataSet ReadGoodsPhotos(int GoodsId)
        {
            string sql = string.Format(@" select * from GoodsPhoto where GoodsId ={0} and IsDetailPhoto!=1 order by IsHomeImage desc,Sort,PhotoName ;
                                          select * from GoodsPhoto where GoodsId ={0} and IsDetailPhoto=1 order by IsHomeImage desc,Sort,PhotoName ; ", GoodsId);
            return m_dbo.GetDataSet(sql);
        }

        public DataSet GetGoodsPhotos(int GoodsId)
        {
            string sql = string.Format(" select * from GoodsPhoto where GoodsId ={0} order by IsHomeImage desc,Sort,PhotoName ", GoodsId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 根据已有图片，自动生成 图片名称
        /// ERP:FGoods.cs
        /// </summary>
        /// <param name="GoodsId"></param>
        /// <returns></returns>
        public string GetAPhotoName(int GoodsId)
        {
            string sql = string.Format("  select top 1 PhotoName from GoodsPhoto where GoodsId={0} order by Id desc ", GoodsId);
            DataSet ds = m_dbo.GetDataSet(sql);
            try
            {
                string maxname = DBTool.GetStringFromRow(ds.Tables[0].Rows[0], "PhotoName", "");
                string ordernum = maxname.Substring(maxname.IndexOf("_") + 1, maxname.IndexOf(".") - maxname.IndexOf("_") - 1);
                int order = int.Parse(ordernum) + 1;
                return GoodsId.ToString() + "_" + order.ToString() + ".jpg";
            }
            catch
            {
                return GoodsId.ToString() + "_1.jpg";
            }
        }
        public bool Delete()
        {
            string sql1 = string.Format(" select GoodsId  from GoodsPhoto where Id={0} ", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql1);
            int goodsId = DBTool.GetIntFromRow(ds.Tables[0].Rows[0], "GoodsId", 0); 
            string sql = string.Format(" delete from GoodsPhoto where Id = {0} ", m_Id);
            bool result = m_dbo.ExecuteNonQuery(sql);
            if (result)
            {
                //更新 图片数量
                Goods g = new Goods();
                g.Id = GoodsId;
                g.UpdatePhotoNum();
            }
            return result;
        }
        //------------------------------------------------读取图片--------------------------------------------
        /// <summary>
        /// 读取商品的图片
        /// IsDetailPhoto!=1 读取轮播图
        /// IsDetailPhoto=1 读取详情图
        /// </summary>
        /// <param name="GoodsId">商品编号</param>
        /// <param name="IsDetailPhoto">是否是详情图 0主图和轮播图 1 详情图 -1是全部</param>
        /// <returns></returns>
        public DataSet ReadGoodsPhotoByGoodsId(List<int> GoodsId, int IsDetailPhoto)
        {
            string sql = "select * from GoodsPhoto where 1=1 ";
            if (GoodsId != null)
            {
                sql += "and GoodsId in (";
                for (int i = 0; i < GoodsId.Count; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", GoodsId[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", GoodsId[i]);
                }
                sql += " ) ";
            }
            if (IsDetailPhoto >= 0)
            {
                if (IsDetailPhoto == 0)
                {
                    sql += " and  IsDetailPhoto !=1";
                }
                else
                {
                    sql += " and  IsDetailPhoto =1";
                }
            }
            sql += " order by IsHomeImage desc,Sort,PhotoName";
            return m_dbo.GetDataSet(sql);
        }
    }
}
