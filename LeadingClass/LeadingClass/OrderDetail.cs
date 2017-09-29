using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int GoodsId { get; set; }
        public string Model { get; set; }
        public int Num { get; set; }
        public double Discount { get; set; }
        public double SalePrice { get; set; }
        public double Price { get; set; }
        public int IsGift { get; set; }
        public double InPrice { get; set; }
        public double TaxInPrice { get; set; }
        public double AC { get; set; }
        public double Point { get; set; }
        public double Amount { get; set; }
        public int IsLack { get; set; }
        public string PurchaseStatus { get; set; }
        public string PurchaseMemo { get; set; }
        public int PickNum { get; set; }
        public int OldGoodsId { get; set; }
        public string SkuName { get; set; }
        public int GroupParentId { get; set; }
        public int IsShow { get; set; }
        public int IsCalc { get; set; }      
        public double TaxAC { get; set; }
        private DBOperate m_dbo;

        public OrderDetail()
        {
            Id = 0;
            OrderId = 0;
            GoodsId = 0;
            Model = "";
            Num = 0;
            Discount = 0;
            SalePrice = 0;
            Price = 0;
            IsGift = 0;
            InPrice = 0;
            TaxInPrice = 0;
            AC = 0;
            Point = 0;
            Amount = 0;
            IsLack = 0;
            PurchaseStatus = "";
            PurchaseMemo = "";
            PickNum = 0;
            OldGoodsId = 0;
            SkuName = "";
            GroupParentId = 0;
            IsShow = 0;
            IsCalc = 0;           
            TaxAC = 0;
            m_dbo = new DBOperate();
        }
        public OrderDetail(int Id)
            : this()
        {
            this.Id = Id;
            this.Load();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@OrderId", OrderId));
            arrayList.Add(new SqlParameter("@GoodsId", GoodsId));
            arrayList.Add(new SqlParameter("@Model", Model));
            arrayList.Add(new SqlParameter("@Num", Num));
            arrayList.Add(new SqlParameter("@Discount", Discount));
            arrayList.Add(new SqlParameter("@SalePrice", SalePrice));
            arrayList.Add(new SqlParameter("@Price", Price));
            arrayList.Add(new SqlParameter("@IsGift", IsGift));
            arrayList.Add(new SqlParameter("@InPrice", InPrice));
            arrayList.Add(new SqlParameter("@TaxInPrice", TaxInPrice));
            arrayList.Add(new SqlParameter("@AC", AC));
            arrayList.Add(new SqlParameter("@Point", Point));
            arrayList.Add(new SqlParameter("@Amount", Amount));
            arrayList.Add(new SqlParameter("@IsLack", IsLack));
            arrayList.Add(new SqlParameter("@PurchaseStatus", PurchaseStatus));
            arrayList.Add(new SqlParameter("@PurchaseMemo", PurchaseMemo));
            arrayList.Add(new SqlParameter("@PickNum", PickNum));
            arrayList.Add(new SqlParameter("@OldGoodsId", OldGoodsId));
            arrayList.Add(new SqlParameter("@SkuName", SkuName));
            arrayList.Add(new SqlParameter("@GroupParentId", GroupParentId));
            arrayList.Add(new SqlParameter("@IsShow", IsShow));
            arrayList.Add(new SqlParameter("@IsCalc", IsCalc));
            arrayList.Add(new SqlParameter("@TaxAC", TaxAC));           
            if (this.Id > 0)
            {
                m_dbo.UpdateData("OrderDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("OrderDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from OrderDetail where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                OrderId = DBTool.GetIntFromRow(row, "OrderId", 0);
                GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                Model = DBTool.GetStringFromRow(row, "Model", "");
                Num = DBTool.GetIntFromRow(row, "Num", 0);
                Discount = DBTool.GetDoubleFromRow(row, "Discount", 0);
                SalePrice = DBTool.GetDoubleFromRow(row, "SalePrice", 0);
                Price = DBTool.GetDoubleFromRow(row, "Price", 0);
                IsGift = DBTool.GetIntFromRow(row, "IsGift", 0);
                InPrice = DBTool.GetDoubleFromRow(row, "InPrice", 0);
                TaxInPrice = DBTool.GetDoubleFromRow(row, "TaxInPrice", 0);
                AC = DBTool.GetDoubleFromRow(row, "AC", 0);
                Point = DBTool.GetDoubleFromRow(row, "Point", 0);
                Amount = DBTool.GetDoubleFromRow(row, "Amount", 0);
                IsLack = DBTool.GetIntFromRow(row, "IsLack", 0);
                PurchaseStatus = DBTool.GetStringFromRow(row, "PurchaseStatus", "");
                PurchaseMemo = DBTool.GetStringFromRow(row, "PurchaseMemo", "");
                PickNum = DBTool.GetIntFromRow(row, "PickNum", 0);
                OldGoodsId = DBTool.GetIntFromRow(row, "OldGoodsId", 0);
                SkuName = DBTool.GetStringFromRow(row, "SkuName", "");
                GroupParentId = DBTool.GetIntFromRow(row, "GroupParentId", 0);
                IsShow = DBTool.GetIntFromRow(row, "IsShow", 0);
                IsCalc = DBTool.GetIntFromRow(row, "IsCalc", 0);
                TaxAC = DBTool.GetDoubleFromRow(row, "TaxAC", 0);               
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            //先删除组合商品的明细，然后再删除此商品
            string sql = string.Format(@"
delete from OrderDetail where OrderId = (select OrderId from OrderDetail where Id={0}) and GroupParentId=(select GoodsId from OrderDetail where Id={0});
Delete from OrderDetail where id={0}; ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        public bool Load(int OrderId, int GoodsId)
        {
            string sql = string.Format("select * from OrderDetail where OrderId={0} and GoodsId={1}", OrderId, GoodsId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                OrderId = DBTool.GetIntFromRow(row, "OrderId", 0);
                GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                Model = DBTool.GetStringFromRow(row, "Model", "");
                Num = DBTool.GetIntFromRow(row, "Num", 0);
                Discount = DBTool.GetDoubleFromRow(row, "Discount", 0);
                SalePrice = DBTool.GetDoubleFromRow(row, "SalePrice", 0);
                Price = DBTool.GetDoubleFromRow(row, "Price", 0);
                IsGift = DBTool.GetIntFromRow(row, "IsGift", 0);
                InPrice = DBTool.GetDoubleFromRow(row, "InPrice", 0);
                TaxInPrice = DBTool.GetDoubleFromRow(row, "TaxInPrice", 0);
                AC = DBTool.GetDoubleFromRow(row, "AC", 0);
                Point = DBTool.GetDoubleFromRow(row, "Point", 0);
                Amount = DBTool.GetDoubleFromRow(row, "Amount", 0);
                IsLack = DBTool.GetIntFromRow(row, "IsLack", 0);
                PurchaseStatus = DBTool.GetStringFromRow(row, "PurchaseStatus", "");
                PurchaseMemo = DBTool.GetStringFromRow(row, "PurchaseMemo", "");
                PickNum = DBTool.GetIntFromRow(row, "PickNum", 0);
                OldGoodsId = DBTool.GetIntFromRow(row, "OldGoodsId", 0);
                SkuName = DBTool.GetStringFromRow(row, "SkuName", "");
                GroupParentId = DBTool.GetIntFromRow(row, "GroupParentId", 0);
                IsShow = DBTool.GetIntFromRow(row, "IsShow", 0);
                IsCalc = DBTool.GetIntFromRow(row, "IsCalc", 0);
                TaxAC = DBTool.GetDoubleFromRow(row, "TaxAC", 0);               
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public DataSet Readorderdetail(int OrderId) 
        {
            string sql = string.Format("select * from OrderDetail where OrderId ={0}",OrderId);
            return m_dbo.GetDataSet(sql);
        }
        public bool Revoke(int orderId)
        {
            string sql = string.Format("update OrderDetail set PickNum=0 where OrderId={0}", orderId);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 根据orderId 查询 该订单商品是否生成了采购单
        /// </summary>
        /// <param name="OrderId">订单号</param>
        /// <returns></returns>
        public bool ReadOrderDetail(int OrderId)
        {
            string sql = string.Format("select * from OrderDetail where OrderId ={0} and GoodsId in (select GoodsId  from View_PurchaseDetail where NeedToPurchaseId=(select NeedToPurchaseId from NeedToPurchaseOrder where OrderId ={0}))", OrderId);
             DataSet ds = m_dbo.GetDataSet(sql);
             if (ds.Tables[0].Rows.Count > 0)
             {
                 return true;
             }
             else return false;
        }
        public bool UpdateNum(int orderId,int num,int goodsId)
        {
            string sql = string.Format("update OrderDetail set Num={0},Amount={1} where OrderId={2} and goodsId={3}", num, SalePrice * num, orderId,goodsId);
            return m_dbo.ExecuteNonQuery(sql);
        }

        public int DetailCount(int orderId)
        {
            string sql = string.Format(" select count(*) from OrderDetail where OrderId={0}",orderId);
            object o = m_dbo.ExecuteScalar(sql);
            try
            {
               return  Convert.ToInt32(o);             
            }
            catch
            {
                return 0;
            }
        }
    }
}
