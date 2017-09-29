using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class TPI_Orderdetail
    {
        public int Id { get; set; }
        public int TPI_OrderId { get; set; }
        public int GoodsId { get; set; }
        public string SkuName { get; set; }
        public string prDetailId { get; set; }//请购单明细编号
        public int Num { get; set; }
        public decimal AgreementPrice { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public int status { get; set; }
        public int InvoiceRequireId { get; set; }
        private DBOperate m_dbo;

        public TPI_Orderdetail()
        {
            Id = 0;
            TPI_OrderId = 0;
            GoodsId = 0;
            SkuName = "";
            prDetailId = "";
            Num = 0;
            AgreementPrice = 0;
            Price = 0;
            Amount = 0;
            status = 0;
            InvoiceRequireId = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@TPI_OrderId", TPI_OrderId));
            arrayList.Add(new SqlParameter("@GoodsId", GoodsId));
            arrayList.Add(new SqlParameter("@SkuName", SkuName));
            arrayList.Add(new SqlParameter("@prDetailId", prDetailId));
            arrayList.Add(new SqlParameter("@Num", Num));
            arrayList.Add(new SqlParameter("@AgreementPrice", AgreementPrice));
            arrayList.Add(new SqlParameter("@Price", Price));
            arrayList.Add(new SqlParameter("@Amount", Amount));
            arrayList.Add(new SqlParameter("@status", status));
            arrayList.Add(new SqlParameter("@InvoiceRequireId", InvoiceRequireId));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("TPI_OrderDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("TPI_OrderDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from TPI_OrderDetail where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                TPI_OrderId = DBTool.GetIntFromRow(row, "TPI_OrderId", 0);
                GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                SkuName = DBTool.GetStringFromRow(row, "SkuName", "");
                prDetailId = DBTool.GetStringFromRow(row, "prDetailId", "");
                Num = DBTool.GetIntFromRow(row, "Num", 0);
                AgreementPrice = DBTool.GetDecimalFromRow(row, "AgreementPrice", 0);
                Price = DBTool.GetDecimalFromRow(row, "Price", 0);
                Amount = DBTool.GetDecimalFromRow(row, "Amount", 0);
                status = DBTool.GetIntFromRow(row, "status", 0);
                InvoiceRequireId = DBTool.GetIntFromRow(row, "InvoiceRequireId", 0);
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from TPI_OrderDetail where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 根据tpi_OrderId读取订单明细
        /// </summary>
        /// <param name="tpi_OrderId"></param>
        /// <returns></returns>
        public DataSet ReadGoodsDetailById(int tpi_OrderId)
        {
            string sql = string.Format("select * from TPI_OrderDetail where TPI_OrderId={0}", tpi_OrderId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 根据prDetailId读取订单明细
        /// </summary>
        /// <param name="tpi_OrderId"></param>
        /// <returns></returns>
        public DataSet ReadorderDetailByPrDetailIds(string prDetailIds)
        {
            string sql = string.Format("select * from TPI_OrderDetail where prDetailId in({0})", prDetailIds);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 根据prDetailId删除订单明细
        /// </summary>
        /// <param name="tpi_OrderId"></param>
        /// <returns></returns>
        public bool DeleteorderDetailByPrDetailIds(string prDetailIds)
        {
            string sql = string.Format("delete from TPI_OrderDetail where prDetailId in({0})", prDetailIds);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}
