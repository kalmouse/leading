using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace LeadingClass
{
    /// <summary>
    /// 打印订单用到的数据类
    /// </summary>
    public class OrderPrintView
    {
        public int OrderId { get; set; }
        public double SumMoney { get; set; }
        public int Emergency { get; set; }
        public DateTime OrderTime { get; set; }//下单日期
        public DateTime PlanDate { get; set; }//送货日期
        public DateTime StoreFinishTime { get; set; }//出库时间
        public DateTime PrintTime { get; set; }//打印时间
        public int RowNum { get; set; }//行数
        public int PackageNum { get; set; }//件数
        public string Company { get; set; }
        public int ComId { get; set; }
        public string RealName { get; set; }
        public string DeptName { get; set; }
        public string Mobile { get; set; }
        public string Telphone { get; set; }
        public string Address { get; set; }
        public string SalesName { get; set; }
        public string ServiceName { get; set; }
        public int PickingUserId { get; set; }
        public string PickingName { get; set; }
        public string PayMethod { get; set; }//付款方式
        public string Memo { get; set; }
        public string StoreName { get; set; }//仓库名称
        public string ComplainUrl { get; set; }//投诉建议url
        public int IsArchive { get; set; }//是否独立归档

        public DataTable Details{get;set;}//订单明细
        public OrderPrintView()
        {
            Details = new DataTable();
        }
        public OrderPrintView(int orderId)
            : this()
        {
            LeadingClass.Order order = new Order(orderId);
            order.PrintDateTime = DateTime.Now;
            order.Save();
            this.ComplainUrl = CommenClass.SiteUrl.ComplainUrl() + order.GUID;

            Customer customer = new Customer(order.ComId);
            this.IsArchive = customer.IsArchive;

            DBOperate dbo = new DBOperate();
            string sql = string.Format(" select * from view_order where orderId={0} ", orderId);
            DataSet ds = dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                this.OrderId = DBTool.GetIntFromRow(row, "OrderId", 0);
                this.SumMoney = DBTool.GetDoubleFromRow(row, "SumMoney", 0);
                this.Emergency = DBTool.GetIntFromRow(row, "Emergency", 0);
                this.OrderTime = DBTool.GetDateTimeFromRow(row, "OrderTime");
                this.PlanDate = DBTool.GetDateTimeFromRow(row, "PlanDate");
                this.StoreFinishTime = DBTool.GetDateTimeFromRow(row, "StoreFinishTime");
                this.PrintTime = DBTool.GetDateTimeFromRow(row, "PrintDateTime");
                this.RowNum = DBTool.GetIntFromRow(row, "RowNum", 0);
                this.PackageNum = DBTool.GetIntFromRow(row, "PackageNum", 0);
                this.Company = DBTool.GetStringFromRow(row, "Company", "");
                this.ComId = DBTool.GetIntFromRow(row, "ComId", 0);
                this.RealName = DBTool.GetStringFromRow(row, "RealName", "");
                this.DeptName = DBTool.GetStringFromRow(row, "DeptName", "");
                this.Mobile = DBTool.GetStringFromRow(row, "Mobile", "");
                this.Telphone = DBTool.GetStringFromRow(row, "Telphone", "");
                this.Address = DBTool.GetStringFromRow(row, "Address", "");
                this.SalesName = DBTool.GetStringFromRow(row, "SalesName", "");
                this.ServiceName = DBTool.GetStringFromRow(row, "ServiceName", "");
                this.PickingUserId = DBTool.GetIntFromRow(row, "PickingUserId", 0);
                this.PickingName = DBTool.GetStringFromRow(row, "PickingName", "");
                this.PayMethod = DBTool.GetStringFromRow(row, "Pay_Method", "");
                this.Memo = DBTool.GetStringFromRow(row, "Memo", "");


                StoreManager sm = new StoreManager();
                Store store = new Store(sm.GetUserDefaultStore(this.PickingUserId));
                this.StoreName = store.Name;

                sql = string.Format(@" select ROW_NUMBER() over(order by Id) as RowNumber,GoodsId,Left(DisplayName,40) as DisplayName,Unit,Num,SalePrice,Amount 
                    from View_OrderDetail 
                    where OrderId={0} and IsShow=1 order by Id ", this.OrderId);
                Details = dbo.GetDataSet(sql).Tables[0];
            }
        }
    }

    public class OrderPrintManager
    {
        /// <summary>
        /// 获取订单打印所需数据
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="packageNum"></param>
        /// <param name="pickingUserId"></param>
        /// <returns></returns>
        public static OrderPrintView GetOrderPrintView(int orderId, int packageNum, int pickingUserId)
        {

            OrderPicking op = new OrderPicking(orderId);
            if (op.PickingUserId != pickingUserId)//验证是否是自己的拣货单
            {
                return null;
            }
            else
            {
                OrderStatus os = new OrderStatus(orderId);
                os.StoreStatus = CommenClass.Order_Status.已完成.ToString();
                os.StoreFinishTime = DateTime.Now;
                os.PackageNum = packageNum;
                os.Save();
            }
            OrderPrintView orderPrintView = new OrderPrintView(orderId);
            Order order = new Order(orderId);
            order.CreateBox(packageNum);//创建包裹记录

            return orderPrintView;
        }
    }
}
