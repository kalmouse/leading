using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace LeadingClass
{
    public  class SupplierPriceListManager
    {
        private DBOperate m_dbo;
        public SupplierPriceListManager()
        {
            m_dbo = new DBOperate();
        }

        public DataSet ReadByCompany(string supplierName)
        {
            string sql = string.Format(@"select SupplierPriceList.Id as SupplierPriceId,Supplier.ID as SupplierId ,
                (select  COUNT(*) from dbo.SupplierPriceListDetail where dbo.SupplierPriceListDetail.SupplierPriceListId=dbo.SupplierPriceList.Id) as GoodsCount ,* 
                from dbo.SupplierPriceList inner join dbo.Supplier on dbo.SupplierPriceList.SupplierId=dbo.Supplier.ID where Company   like '%{0}%' order by SupplierPriceList.UpdateTime", supplierName);

            return  m_dbo.GetDataSet(sql);
        }

        public DataSet ReadSupplierPriceDetail(int supplierPriceListId)
        {
            string sql = string.Format("select * from dbo.View_SupplierPriceListDetail where SupplierPriceListId={0} order by DetailUpdateTime", supplierPriceListId);

            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 通过SupplierId和GoodsId获得对应的DetailId，之后将DetailId对应的IsNews修改为0
        /// </summary>
        /// <returns></returns>
        public bool UpdateIsNewsBySupplierIdGoodsId(int supplierId ,int goodsId)
        {
            string sql =string.Format( @"update dbo.SupplierPriceListDetail   set IsNew =0 
where Id in (
select SupplierPriceDetailId from dbo.View_SupplierPriceListDetail   where SupplierId={0} and GoodsId={1} )" ,supplierId,goodsId) ;

            return m_dbo.ExecuteNonQuery(sql);
        }

        public bool DeleteSupplierPriceDetailById(int supplierPriceDetailId)
        {
            SqlParameter[] param=new SqlParameter[]
            {
                new SqlParameter("@Id", supplierPriceDetailId) 
            };
            return m_dbo.DeleteAt("SupplierPriceListDetail", param);   
        }

        //同一个报价单某个商品的个数
        public int SupplierPriceDetialGoodsCount(int supplierPriceListId,int goodsId)
        {
            //string sql = string.Format("select COUNT(*) from dbo.SupplierPriceListDetail where SupplierPriceListId={0} and GoodsId={1} ",supplierPriceListId, goodsId);

            //SqlParameter[] param = new SqlParameter[]
            //{
            //    new SqlParameter("@SupplierPriceListId", supplierPriceListId),
            //    new SqlParameter("@GoodsId", goodsId) 
            //};
            //return m_dbo.InsertData(sql, param);//返回的是首行首列，同一报价单的商品个数  InsertData会自动生成sql语句
            return 1;
        }
    }
}
