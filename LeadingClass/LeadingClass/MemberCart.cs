using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class MemberCart
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int GoodsId { get; set; }
        public string Model { get; set; }
        public int Num { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public MemberCart()
        {
            Id = 0;
            MemberId = 0;
            GoodsId = 0;
            Model = "";
            Num = 0;
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
            arrayList.Add(new SqlParameter("@MemberId", MemberId));
            arrayList.Add(new SqlParameter("@GoodsId", GoodsId));
            arrayList.Add(new SqlParameter("@Model", Model));
            arrayList.Add(new SqlParameter("@Num", Num));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("MemberCart", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("MemberCart", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }


        /// <summary>
        /// 更新商品数量,并且返回当期更新的购物车商品信息，cart.cshtml的加减数量操作 quxiaoshan 2015-5-29
        /// **********************千万不要随便修改update的sql，不然数据库就都错了************************
        /// </summary>
        /// <returns></returns>
        public DataRow UpdateCartGoodsNumById(MemberCart mc,int isvip) 
        {
            string sql = "";
            if (mc.Id > 0 && mc.GoodsId>0 && mc.MemberId>0)
            {
                sql = string.Format(@" update dbo.MemberCart set num={0} where 1=1 and  Id ={1} and memberId={2} and GoodsId ={3} and model ='{4}' ;", mc.Num, mc.Id, mc.MemberId, mc.GoodsId, mc.Model);
                if (isvip == 0)
                {
                    sql += string.Format(" select dbo.Goods.Price as Price ,dbo.Goods.Rate as Rate,dbo.MemberCart.Id as MemberCartId ,* from MemberCart join dbo.Goods on dbo.MemberCart.GoodsId=dbo.Goods.ID where  dbo.MemberCart.Id={0}  and  GoodsId={1} ", mc.Id, mc.GoodsId);
                }
                else if (isvip == 1)//专柜商品只插入了母商品，使用with子句关联到自己商品后，在和MemberCart中的购物车信息匹配
                {
                    sql += string.Format(@" declare @discount decimal(18,2)= 1;
                                            declare @counterId int =0;
                                            select @discount=discount,  @CounterId= CounterId  from dbo.VIPCounterCompany where ComId= (select ComId from dbo.Member where Id ={0}); 
                                            with CounterDetailContainParent as(
                                             select dbo.Goods.ID as GoodsId,CounterId,VIPPrice  from dbo.VIPCounterDetail 
                                             left join dbo.Goods on 
                                             dbo.VIPCounterDetail.GoodsId=dbo.Goods.ID or 
                                             dbo.VIPCounterDetail.GoodsId=dbo.Goods.ParentId
                                             where dbo.VIPCounterDetail.CounterId =@CounterId 
                                             ) 
                                            select dbo.MemberCart.*,dbo.MemberCart.Id as MemberCartId,CONVERT(decimal(18,2),@discount*VIPPrice) as VIPPrice,dbo.Goods.* from dbo.MemberCart
                                            left join CounterDetailContainParent  on dbo.MemberCart.GoodsId=CounterDetailContainParent.GoodsId
                                            left join dbo.Goods on CounterDetailContainParent.GoodsId=dbo.Goods.ID 
                                            where  dbo.MemberCart.Id={1}  and  dbo.MemberCart.GoodsId= {2} and MemberId={0} and 
                                            CounterId = @counterId
                                            order by dbo.MemberCart.Id Desc   ", mc.MemberId, mc.Id, mc.GoodsId);
                }
            }

            DataSet ds =  m_dbo.GetDataSet(sql);
            DataRow row = null;
            if(ds.Tables[0].Rows.Count>0)
            {
                row= ds.Tables[0].Rows[0];
            }
            return row;
        }

        public bool Load()
        {
            string sql = string.Format("select *  from MemberCart where Id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);
                GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                Model = DBTool.GetStringFromRow(row, "Model", "");
                Num = DBTool.GetIntFromRow(row, "Num", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }

        public DataSet ReadCartsByMemberId(int memberId)
        {
            string sql = "";
            if (memberId > 0)
            {
                sql = string.Format("select * from dbo.MemberCart where MemberId={0}", memberId);
            }
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 通过MemberId读取该用户的购物车
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public DataSet ReadCartsInfoByMemberId(int memberId) 
        {
            string sql = "";
            if (memberId > 0)
            {
                sql = string.Format("select dbo.Goods.Price as Price ,dbo.Goods.Rate as Rate,dbo.MemberCart.Id as MemberCartId ,* from MemberCart join dbo.Goods on dbo.MemberCart.GoodsId=dbo.Goods.ID where MemberId={0}", memberId);
            }
            return m_dbo.GetDataSet(sql);
        }

        public DataSet ReadCartDetailByMemberId(int memberId)
        {
            string sql = "";
            if (memberId > 0)
            { 
            
            }
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取VIP用户的购物车
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="comId"></param>
        /// <param name="counterId">装柜Id</param>
        /// <returns></returns>
        public DataSet ReadVIPCartsInfoByMemberId(int memberId)
        {
            string sql = "";
            if (memberId > 0)
            {
                sql = string.Format(@"declare @discount decimal(18,2)= 1;
                                      declare @counterId int =0;
                                      select @discount=discount, @CounterId= CounterId  from dbo.VIPCounterCompany where ComId= (select ComId from dbo.Member where Id ={0}); 
                                      with CounterDetailContainParent as(
                                      select dbo.Goods.ID as GoodsId,CounterId,VIPPrice  from dbo.VIPCounterDetail 
                                      left join dbo.Goods on 
                                      dbo.VIPCounterDetail.GoodsId=dbo.Goods.ID or 
                                      dbo.VIPCounterDetail.GoodsId=dbo.Goods.ParentId
                                      where dbo.VIPCounterDetail.CounterId =@CounterId 
                                      ) 
                                      select dbo.MemberCart.*,dbo.MemberCart.Id as MemberCartId, CONVERT(decimal(18,2),@discount*VIPPrice) as VIPPrice,dbo.Goods.* from 
                                      dbo.MemberCart
                                      left join CounterDetailContainParent  on dbo.MemberCart.GoodsId = CounterDetailContainParent.GoodsId
                                      left join dbo.Goods on CounterDetailContainParent.GoodsId = dbo.Goods.ID where 
                                      MemberId={0} and 
                                      CounterId =@counterId
                                      order by dbo.MemberCart.Id Desc ;", memberId);
           }
            return m_dbo.GetDataSet(sql);
        }

        public bool Delete()
        {
            string sql = string.Format("Delete from MemberCart where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 删除GoodsId和Model满足对应的购物车 
        /// </summary>
        /// <returns></returns>
        public bool DeleteByGoodsIdModel (MemberCart mc)
        {
            string sql = "";
            if (mc.Id > 0 && mc.GoodsId > 0 && mc.MemberId > 0)
            {
                sql += string.Format(" delete dbo.MemberCart where Id ={0} and memberId={1} and GoodsId ={2} and model ='{3}' ",mc.Id,mc.MemberId,mc.GoodsId,mc.Model);
            }
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 将购物车的商品信息批量插入MemberCart表 quxiaoshan 2015-5-26
        /// </summary>
        /// <returns></returns>
        public bool InsertMemberCarts(List<MemberCart> list) 
        {
            string sql = " insert into MemberCart ( MemberId,GoodsId,Model,Num,UpdateTime ) ";
            for (int i = 0; i < list.Count; i++)
            {
                if (i != 0)
                {
                    sql += " union all ";
                }
                sql += string.Format(" select {0},{1},'{2}',{3},'{4}' ", list[i].MemberId, list[i].GoodsId, list[i].Model, list[i].Num, DateTime.Now);
            }
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 删除当期用户的购物车信息 quxiaoshan 2015-5-27
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public bool DeleteMemberCartsByMemberId(int memberId) 
        {
            string sql = string.Format(" delete from dbo.MemberCart where ID in ( select Id from dbo.MemberCart where MemberId ={0} )",memberId);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 获取购物车中是否存在已添加过的商品(GoodsId和Model) quxiaoshan 2015-5-27
        /// </summary>
        /// <returns></returns>
        public DataSet ReadCartByGoodsId(MemberCart memberCart) 
        {
            string sql = string.Format("select ID,  Num from dbo.MemberCart where 1=1 ");
            if (memberCart.MemberId > 0)
            {
                sql += string.Format(" and MemberId={0} ",memberCart.MemberId);
            }
            if (memberCart.GoodsId > 0)
            {
                sql +=string.Format( " and GoodsId ={0} ",memberCart.GoodsId);
            }
            //if (memberCart.Model != "")
            //{
            //    sql += string.Format(" and Model= '{0}' ",memberCart.Model);
            //}
            sql += " order by Id desc ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取客户选的专柜外的商品
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public DataSet ReadCartGoodsNotInVipCart(List<int> goodsId)
        {
            string sql = string.Format("select MemberCart.Id,MemberId,GoodsId,Num,DisplayName,HomeImage,Unit,Price from MemberCart inner join Goods on MemberCart.GoodsId=Goods.ID where MemberId ={0} ", MemberId);
            if (goodsId != null)
            {
                sql += " and GoodsId not in (";
                for (int i = 0; i < goodsId.Count; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", goodsId[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", goodsId[i]);
                }
                sql += " ) ";
            }
            return m_dbo.GetDataSet(sql);
        }
    }
}
