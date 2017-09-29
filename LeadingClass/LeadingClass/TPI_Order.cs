using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class TPI_Order
    {
        public int Id { get; set; }
        public string ThirdPartyOrderId { get; set; }
        public int ProjectId { get; set; }
        public string RealName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Town { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Remark { get; set; }
        public string InvoiceTitle { get; set; }
        public int InvoiceState { get; set; }
        public int InvoiceType { get; set; }
        public int InvoiceContent { get; set; }
        public decimal Amount { get; set; }
        public int Freight { get; set; }
        public string Payment { get; set; }
        public int ReviewId { get; set; }//确认订单  projectId=5
        public string OrderStatus { get; set; }
        public int IsTransformation { get; set; }
        public DateTime OrderTime { get; set; }
        public int RowNum { get; set; }
        public string ItemDeliveryId { get; set; } //发货需求清单id -- 政采云接口使用
        public string DeliveryCode { get; set; }//发货单号(客户发货单记录id) -- 政采云接口使用
        public string PrId { get; set; }//请购单编号--国网商城
        private DBOperate m_dbo;

        public TPI_Order()
        {
            Id = 0;
            ThirdPartyOrderId = "";
            ProjectId = 0;
            RealName = "";
            Province = "";
            City = "";
            County = "";
            Town = "";
            Address = "";
            Zip = "";
            Phone = "";
            Mobile = "";
            Email = "";
            Remark = "";
            InvoiceTitle = "";
            InvoiceState = 0;
            InvoiceType = 0;
            InvoiceContent = 0;
            Amount = 0;
            Freight = 0;
            Payment = "";
            ReviewId = 0;
            OrderStatus = "";
            IsTransformation = 0;
            OrderTime = DateTime.Now;
            RowNum = 1;
            ItemDeliveryId = "";
            DeliveryCode = "";
            PrId = "";
            m_dbo = new DBOperate();
        }
        public TPI_Order(int Id)
        {
            m_dbo = new DBOperate();
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
            arrayList.Add(new SqlParameter("@ThirdPartyOrderId", ThirdPartyOrderId));
            arrayList.Add(new SqlParameter("@ProjectId", ProjectId));
            arrayList.Add(new SqlParameter("@RealName", RealName));
            arrayList.Add(new SqlParameter("@Province", Province));
            arrayList.Add(new SqlParameter("@City", City));
            arrayList.Add(new SqlParameter("@County", County));
            arrayList.Add(new SqlParameter("@Town", Town));
            arrayList.Add(new SqlParameter("@Address", Address));
            arrayList.Add(new SqlParameter("@Zip", Zip));
            arrayList.Add(new SqlParameter("@Phone", Phone));
            arrayList.Add(new SqlParameter("@Mobile", Mobile));
            arrayList.Add(new SqlParameter("@Email", Email));
            arrayList.Add(new SqlParameter("@Remark", Remark));
            arrayList.Add(new SqlParameter("@InvoiceTitle", InvoiceTitle));
            arrayList.Add(new SqlParameter("@InvoiceState", InvoiceState));
            arrayList.Add(new SqlParameter("@InvoiceType", InvoiceType));
            arrayList.Add(new SqlParameter("@InvoiceContent", InvoiceContent));
            arrayList.Add(new SqlParameter("@Amount", Amount));
            arrayList.Add(new SqlParameter("@Freight", Freight));
            arrayList.Add(new SqlParameter("@Payment", Payment));
            arrayList.Add(new SqlParameter("@ReviewId", ReviewId));
            arrayList.Add(new SqlParameter("@OrderStatus", OrderStatus));
            arrayList.Add(new SqlParameter("@IsTransformation", IsTransformation));
            arrayList.Add(new SqlParameter("@OrderTime", OrderTime));
            arrayList.Add(new SqlParameter("@RowNum", RowNum));
            arrayList.Add(new SqlParameter("@ItemDeliveryId", ItemDeliveryId));
            arrayList.Add(new SqlParameter("@DeliveryCode", DeliveryCode));
            arrayList.Add(new SqlParameter("@PrId", PrId));
            if (Id > 0)
            {
                m_dbo.UpdateData("TPI_Order", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                Id = m_dbo.InsertData("TPI_Order", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from TPI_Order where id='{0}'", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }
        public bool Load(string ThirdPartyOrderId, int ProjectId)
        {
            string sql = string.Format("select * from TPI_Order where ThirdPartyOrderId ='{0}' and projectId ={1}", ThirdPartyOrderId,ProjectId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }
        public bool Load(string PrId, string ProjectId)
        {
            string sql = string.Format("select * from TPI_Order where PrId ='{0}' and projectId ={1}", PrId, ProjectId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from TPI_Order where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        private void LoadFromRow(DataRow row)
        {
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                ThirdPartyOrderId = DBTool.GetStringFromRow(row, "ThirdPartyOrderId", "");
                ProjectId = DBTool.GetIntFromRow(row, "ProjectId", 0);
                RealName = DBTool.GetStringFromRow(row, "RealName", "");
                Province = DBTool.GetStringFromRow(row, "Province", "");
                City = DBTool.GetStringFromRow(row, "City", "");
                County = DBTool.GetStringFromRow(row, "County", "");
                Town = DBTool.GetStringFromRow(row, "Town", "");
                Address = DBTool.GetStringFromRow(row, "Address", "");
                Zip = DBTool.GetStringFromRow(row, "Zip", "");
                Phone = DBTool.GetStringFromRow(row, "Phone", "");
                Mobile = DBTool.GetStringFromRow(row, "Mobile", "");
                Email = DBTool.GetStringFromRow(row, "Email", "");
                Remark = DBTool.GetStringFromRow(row, "Remark", "");
                InvoiceTitle = DBTool.GetStringFromRow(row, "InvoiceTitle", "");
                InvoiceState = DBTool.GetIntFromRow(row, "InvoiceState", 0);
                InvoiceType = DBTool.GetIntFromRow(row, "InvoiceType", 0);
                InvoiceContent = DBTool.GetIntFromRow(row, "InvoiceContent", 0);
                Amount = DBTool.GetDecimalFromRow(row, "Amount", 0);
                Freight = DBTool.GetIntFromRow(row, "Freight", 0);
                Payment = DBTool.GetStringFromRow(row, "Payment", "");
                ReviewId = DBTool.GetIntFromRow(row, "ReviewId", 0);
                OrderStatus = DBTool.GetStringFromRow(row, "OrderStatus", "");
                IsTransformation = DBTool.GetIntFromRow(row, "IsTransformation", 0);
                OrderTime = DBTool.GetDateTimeFromRow(row, "OrderTime");
                ItemDeliveryId = DBTool.GetStringFromRow(row, "ItemDeliveryId", "");
                DeliveryCode = DBTool.GetStringFromRow(row, "DeliveryCode", "");
                PrId = DBTool.GetStringFromRow(row, "PrId", "");
            }
        /// <summary>
        /// 根据TPI_OrderId查询Order    有问题
        /// </summary>
        /// <returns></returns>
        public DataSet ReadOrderByTPIOrderId()
        {
            string sql = string.Format("select OrderId,Num,GoodsId,Price,TPI_OrderId,SumMoney,ProjectId from [Order] join OrderDetail on [order].Id=OrderDetail.OrderId join TPI_Order on TPI_Order.Id=[order].TPI_OrderId where TPI_OrderId={0} and ProjectId={1}", Id, ProjectId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 查询订单详情商品售价及官网价格，折扣率
        /// </summary>
        /// <returns></returns>
        public DataSet ReadOrderByGoodsPrice()
        {
            string sql = string.Format("select OrderId,Num,GoodsId,OrderDetail.SalePrice ,Goods.Price,TPI_Category.Discount,TPI_OrderId,SumMoney,ProjectId from [Order] join OrderDetail on [order].Id=OrderDetail.OrderId  join Goods on OrderDetail.GoodsId=Goods.ID  join TPI_Category on Goods.TypeId=TPI_Category.TypeId  where TPI_OrderId={0} and ProjectId={1}", Id, ProjectId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 根据项目读取对应的订单
        /// </summary>
        /// <returns></returns>
        public DataSet ReadOrders(int projectId, int orderId, string ThirdPartyOrderId)
        {
            string sql = "select [Order].*,TPI_Order.ThirdPartyOrderId,TPI_Order.Id as TPI_OrderId  from  TPI_Order  inner join [Order]  on TPI_Order.Id= [Order] .TPI_OrderId where 1=1";
            if (projectId > 0)
            {
                sql += string.Format("  and ProjectId ={0}", projectId);
            }
            if (orderId > 0)
            {
                sql += string.Format("  and [Order].Id ={0}", orderId);
            }
            if (ThirdPartyOrderId != "")
            {
                sql += string.Format("  and TPI_Order.ThirdPartyOrderId ='{0}'", ThirdPartyOrderId);
            }
            return m_dbo.GetDataSet(sql);
        }
        
        /// <summary>
        /// 河南读取订单状态
        /// TPI_Order，Order，OrderStatusDetail 关联   ------------------使用少
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public DataTable ReadOrderStatus(string orderId,int projectId)
        {
            string sql = string.Format(@"select [Order].Id,TPI_Order.ThirdPartyOrderId ,OrderStatusDetail.Status,OrderStatusDetail.OperateTime from  TPI_Order join  
[Order] on  TPI_Order.Id=[Order] .TPI_OrderId join  OrderStatusDetail on [Order] .Id=OrderStatusDetail.OrderId where TPI_Order.ThirdPartyOrderId ='{0}' and   TPI_Order.ProjectId ={1}", orderId, projectId);
            return m_dbo.GetDataSet(sql).Tables[0];
        }
    }
}




