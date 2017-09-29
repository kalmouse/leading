using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class TPI_Goods
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public string SkuName { get; set; }
        public double ReferencePrice { get; set; }
        public double AgreementPrice { get; set; }
        public int GoodsId { get; set; }
        public int IshaveImage { get; set; }
        public int IsUnitAgreement { get; set; }
        public int State { get; set; }
        public int ProjectId { get; set; }
        public int Stock { get; set; }
        public int PushState { get; set; }//-1表示未推送的；0推送失败；1推送成功
        public string ResultMsg { get; set; }//推送返回结果
        public DateTime AddTime { get; set; }//商品添加的时间 或 商品推送时间
        private DBOperate m_dbo;

        public TPI_Goods()
        {
            Id = 0;
            Sku = "";
            SkuName = "";
            ReferencePrice = 0;
            AgreementPrice = 0;
            GoodsId = 0;
            IshaveImage = 0;
            IsUnitAgreement = 0;
            State = 0;
            ProjectId = 0;
            Stock = 0;
            PushState = -2;
            ResultMsg = "";
            AddTime = DateTime.Now;
            m_dbo = new DBOperate();
        }

        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@Sku", Sku));
            arrayList.Add(new SqlParameter("@SkuName", SkuName));
            arrayList.Add(new SqlParameter("@ReferencePrice", ReferencePrice));
            arrayList.Add(new SqlParameter("@AgreementPrice", AgreementPrice));
            arrayList.Add(new SqlParameter("@GoodsId", GoodsId));
            arrayList.Add(new SqlParameter("@IshaveImage", IshaveImage));
            arrayList.Add(new SqlParameter("@IsUnitAgreement", IsUnitAgreement));
            arrayList.Add(new SqlParameter("@State", State));
            arrayList.Add(new SqlParameter("@ProjectId", ProjectId));
            arrayList.Add(new SqlParameter("@Stock", Stock));
            arrayList.Add(new SqlParameter("@PushState", PushState));
            arrayList.Add(new SqlParameter("@ResultMsg", ResultMsg));
            arrayList.Add(new SqlParameter("@AddTime", AddTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("TPI_Goods", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("TPI_Goods", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from TPI_Goods where GoodsId={0} and ProjectId={1}", GoodsId, ProjectId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                Sku = DBTool.GetStringFromRow(row, "Sku", "");
                SkuName = DBTool.GetStringFromRow(row, "SkuName", "");
                ReferencePrice = DBTool.GetDoubleFromRow(row, "ReferencePrice", 0);
                AgreementPrice = DBTool.GetDoubleFromRow(row, "AgreementPrice", 0);
                GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                IshaveImage = DBTool.GetIntFromRow(row, "IshaveImage", 0);
                IsUnitAgreement = DBTool.GetIntFromRow(row, "IsUnitAgreement", 0);
                State = DBTool.GetIntFromRow(row, "State", 0);
                ProjectId = DBTool.GetIntFromRow(row, "ProjectId", 0);
                Stock = DBTool.GetIntFromRow(row, "Stock", 0);
                PushState = DBTool.GetIntFromRow(row, "PushState", 0);
                ResultMsg = DBTool.GetStringFromRow(row, "ResultMsg", "");
                AddTime = DBTool.GetDateTimeFromRow(row, "AddTime");
                return true;
            }
            return false;
        }
        /// <summary>
        /// 商品详情-国泰新点
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public DataSet ReadGoodsBy_gtxd(int projectId, int goodsId)
        {
            string sql = string.Format(@"with GoodsInfo as
  (select g.ID as GoodsId ,g.SN,g.GoodsName,g.DisplayName,g.Unit,g.Model,g.Package,g.Price,gt.TypeName,g.BranchId,g.BrandId,b.Name as BrandName,gt.ID as typeId,
 g.HomeImage,g.BarCodeNum,g.IsShelves, g.Feature ,tc.Category,tc.CategoryCode,g.Description,g.ParentId
   from Goods g join TPI_Category tc on g.TypeId = tc.TypeId join GoodsType gt on gt.ID = g.TypeId join 
 Brand b on g.BrandId = b.Id where tc.ProjectId={0} and g.ID ={1}  )
select GoodsInfo.*,gp.Path,gp.PhotoName,gp.IsDetailPhoto,tg.Weight,tg.State,tg.Sku,tg.AgreementPrice,tg.ProjectId,tg.SkuName,tg.Id,tg.ReferencePrice ,tg.DiscountRate from GoodsInfo join 
TPI_Goods tg on GoodsInfo.GoodsId = tg.GoodsId left join (select * from GoodsPhoto where IsDetailPhoto=1) gp on GoodsInfo.GoodsId= gp.GoodsId where tg.ProjectId={0}", projectId, goodsId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 根据goodsId批量读取数据_WPL
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public DataSet ReadGoods(List<string> goodsId)
        {
            string sql = string.Format("select Sku,SkuName,State,GoodsId,AgreementPrice,Price,IsShelves,ReferencePrice from TPI_Goods join Goods on TPI_Goods.GoodsId=Goods.ID where 1=1 and State=1");
            if (goodsId != null)
            {
                sql += "and GoodsId in (";
                for (int i = 0; i < goodsId.Count; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", goodsId[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", goodsId[i]);
                }
                sql += " ) ";
                sql += string.Format("and ProjectId={0}", ProjectId);
            }
            return m_dbo.GetDataSet(sql);
        }

        #region 对TPI_Goods的修改/删除动作

        /// <summary>
        /// 直接修改商城售价
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="projectId"></param>
        /// <param name="newPrice"></param>
        /// <returns></returns>
        public bool ModifyPrice(int goodsId,string Sku, int projectId, double newPrice)
        {
            string sql = string.Format("update TPI_Goods set AgreementPrice ={0},sku='{1}' where  ProjectId ={2} and GoodsId ={3} ", newPrice,Sku, projectId, goodsId);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 批量修改上下架状态
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="projectId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool ModifyState(int[] goodsId, int projectId, int state)
        {
            string sql = "";
            if (goodsId != null)
            {
                sql = string.Format("update TPI_Goods set state ={0} ", state);
                if (state == 0)
                {
                    sql += " , Stock=0";
                }
                sql += " where 1=1 and GoodsId in ( ";
                for (int i = 0; i < goodsId.Length; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", goodsId[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", goodsId[i]);
                }
                sql += " ) ";
                sql += string.Format("and  ProjectId ={0}", projectId);
            }
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 批量修改库存
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="projectId"></param>
        /// <param name="stock"></param>
        /// <returns></returns>
        public bool ModifyStock(List<TPI_Goods> tpi_goodsList)
        {
            string sql = "";
            if (tpi_goodsList != null)
            {
                for (int i = 0; i < tpi_goodsList.Count; i++)
                {
                    sql += string.Format("update TPI_Goods set Stock ={0} where  ProjectId ={1} and GoodsId ={2} ;", tpi_goodsList[i].Stock, tpi_goodsList[i].ProjectId, tpi_goodsList[i].GoodsId);
                }
            }
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 批量删除多个商品
        /// </summary>
        /// <param name="GoodsId"></param>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public bool Delete(int[] GoodsId, int ProjectId)
        {
            string sql = "";
            if (GoodsId != null)
            {
                sql = string.Format("Delete from TPI_Goods where ProjectId={0} ", ProjectId);
                sql += " and GoodsId in ( ";
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
            return m_dbo.ExecuteNonQuery(sql);
        }
        #endregion

    }
}