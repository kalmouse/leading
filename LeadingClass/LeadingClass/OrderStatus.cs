using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using CommenClass;
namespace LeadingClass
{
    public class OrderStatus
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceStatus { get; set; }
        public DateTime ServiceFinishTime { get; set; }
        public string PurchaseStatus { get; set; }
        public DateTime PurchaseFinishTime { get; set; }
        public string StoreStatus { get; set; }
        public DateTime StoreFinishTime { get; set; }
        public int PackageNum { get; set; }
        public string FeedBackStatus { get; set; }
        public DateTime FeedBackFinishTime { get; set; }
        public string DeliveryStatus { get; set; }
        public DateTime DeliveryFinishTime { get; set; }
        public string OrderMemo { get; set; }
        public int OrderMemoUserId { get; set; }
        public DateTime OrderMemoUpdateTime { get; set; }
        private DBOperate m_dbo;

        public OrderStatus()
        {
            Id = 0;
            OrderId = 0;
            ServiceId = 0;
            ServiceStatus = "未处理";
            ServiceFinishTime = new DateTime(1900, 1, 1);
            PurchaseStatus = "未处理";
            PurchaseFinishTime = new DateTime(1900, 1, 1);
            StoreStatus = "未处理";
            StoreFinishTime = new DateTime(1900, 1, 1);
            PackageNum = 0;
            FeedBackStatus = "未处理";
            FeedBackFinishTime = new DateTime(1900, 1, 1);
            DeliveryStatus = "未处理";
            DeliveryFinishTime = new DateTime(1900, 1, 1);
            OrderMemo = "";
            OrderMemoUserId = 0;
            OrderMemoUpdateTime = new DateTime(1900, 1, 1);
            m_dbo = new DBOperate();
        }

        public OrderStatus(int OrderId)
            : this()
        {
            LoadFromOrderId(OrderId);
        }
        /// <summary>
        /// ERP   FWSearchOrder.cs  中调用
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@OrderId", OrderId));
            arrayList.Add(new SqlParameter("@ServiceId", ServiceId));
            arrayList.Add(new SqlParameter("@ServiceStatus", ServiceStatus));
            arrayList.Add(new SqlParameter("@ServiceFinishTime", ServiceFinishTime));
            arrayList.Add(new SqlParameter("@PurchaseStatus", PurchaseStatus));
            arrayList.Add(new SqlParameter("@PurchaseFinishTime", PurchaseFinishTime));
            arrayList.Add(new SqlParameter("@StoreStatus", StoreStatus));
            arrayList.Add(new SqlParameter("@StoreFinishTime", StoreFinishTime));
            arrayList.Add(new SqlParameter("@PackageNum", PackageNum));
            arrayList.Add(new SqlParameter("@FeedBackStatus", FeedBackStatus));
            arrayList.Add(new SqlParameter("@FeedBackFinishTime", FeedBackFinishTime));
            arrayList.Add(new SqlParameter("@DeliveryStatus", DeliveryStatus));
            arrayList.Add(new SqlParameter("@DeliveryFinishTime", DeliveryFinishTime));
            arrayList.Add(new SqlParameter("@OrderMemo", OrderMemo));
            arrayList.Add(new SqlParameter("@OrderMemoUserId", OrderMemoUserId));
            arrayList.Add(new SqlParameter("@OrderMemoUpdateTime", OrderMemoUpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("OrderStatus", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("OrderStatus", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }

        public bool Load()
        {
            string sql = string.Format("select * from OrderStatus where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 读取订单的状态信息
        /// ERP FFeedBace.cs 中调用
        ///     FWSearchOrder.cs 中调用
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public bool LoadFromOrderId(int OrderId)
        {
            string sql = string.Format("select * from OrderStatus where OrderId={0}", OrderId);
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
            OrderId = DBTool.GetIntFromRow(row, "OrderId", 0);
            ServiceId = DBTool.GetIntFromRow(row, "ServiceId", 0);
            ServiceStatus = DBTool.GetStringFromRow(row, "ServiceStatus", "");
            ServiceFinishTime = DBTool.GetDateTimeFromRow(row, "ServiceFinishTime");
            PurchaseStatus = DBTool.GetStringFromRow(row, "PurchaseStatus", "");
            PurchaseFinishTime = DBTool.GetDateTimeFromRow(row, "PurchaseFinishTime");
            StoreStatus = DBTool.GetStringFromRow(row, "StoreStatus", "");
            StoreFinishTime = DBTool.GetDateTimeFromRow(row, "StoreFinishTime");
            PackageNum = DBTool.GetIntFromRow(row, "PackageNum", 0);
            FeedBackStatus = DBTool.GetStringFromRow(row, "FeedBackStatus", "");
            FeedBackFinishTime = DBTool.GetDateTimeFromRow(row, "FeedBackFinishTime");
            DeliveryStatus = DBTool.GetStringFromRow(row, "DeliveryStatus", "");
            DeliveryFinishTime = DBTool.GetDateTimeFromRow(row, "DeliveryFinishTime");
            OrderMemo = DBTool.GetStringFromRow(row, "OrderMemo", "");
            OrderMemoUserId = DBTool.GetIntFromRow(row, "OrderMemoUserId", 0);
            OrderMemoUpdateTime = DBTool.GetDateTimeFromRow(row, "OrderMemoUpdateTime");
        }
        /// <summary>
        ///修改订单的状态
        /// ERP FFeedBack.cs 中调用
        /// </summary>
        /// <param name="StatusType1"></param>
        /// <param name="StatusValue1"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool UpdateStatus(OrderStatusType StatusType1, Order_Status StatusValue1, int UserId)
        {
            string StatusType = StatusType1.ToString();
            string StatusValue = StatusValue1.ToString();
            this.Id = Id;
            this.Load();
            string Status = string.Empty;
            int IsInner = -1;
            int statusId = 0;
            switch (StatusType)
            {
                case "客服":
                    this.ServiceStatus = StatusValue;
                    this.ServiceId = UserId;
                    if (StatusValue == "已完成")
                    {
                        this.ServiceFinishTime = DateTime.Now;
                        Status = "您的订单已确认";
                        IsInner = 0;
                    }
                    else
                    {
                        IsInner = -1;
                        this.ServiceFinishTime = new DateTime(1900, 1, 1);
                    }
                    break;
                case "采购":
                    this.PurchaseStatus = StatusValue;
                    if (StatusValue == "已完成")
                    {
                        this.PurchaseFinishTime = DateTime.Now;

                    }
                    else if (StatusValue == "已接受")
                    {
                        IsInner = 1;
                        Status = "采购已接受！";
                        statusId = 3;
                        this.PurchaseFinishTime = new DateTime(1900, 1, 1);
                    }
                    else
                    {
                        IsInner = -1;
                        this.PurchaseFinishTime = new DateTime(1900, 1, 1);
                    }
                    break;
                case "仓库":
                    this.StoreStatus = StatusValue;
                    if (StatusValue == "已完成")
                    {
                        this.StoreFinishTime = DateTime.Now;
                        statusId = 4;
                        Status = "您商品已出库";
                        IsInner = 1;
                    }
                    else if (StatusValue == "已接受")
                    {
                        IsInner = 0;
                        Status = "您的订单正在分拣！";
                        statusId = 3;
                        this.StoreFinishTime = new DateTime(1900, 1, 1);
                    }
                    else
                    {
                        IsInner = -1;
                        this.StoreFinishTime = new DateTime(1900, 1, 1);
                    }
                    break;
                case "配送":
                    this.DeliveryStatus = StatusValue;
                    if (StatusValue == "已完成")
                    {
                        this.DeliveryFinishTime = DateTime.Now;
                        Status = "您的订单已配送完成！";
                        statusId = 5;
                        IsInner = 0;
                    }
                    else if (StatusValue == "已接受")
                    {
                        IsInner = 0;
                        statusId = 4;
                        Status = "您的订单已分配到配送员,我们会尽快送达!";
                        this.DeliveryFinishTime = new DateTime(1900, 1, 1);
                    }
                    else
                    {
                        IsInner = -1;
                        this.DeliveryFinishTime = new DateTime(1900, 1, 1);
                    }
                    break;
                case "回单":
                    this.FeedBackStatus = StatusValue;

                    if (StatusValue == "已完成")
                    {
                        IsInner = 1;
                        Status = "订单已完成！";
                        statusId = 6;
                        this.FeedBackFinishTime = DateTime.Now;
                    }
                    else
                    {
                        IsInner = -1;
                        this.FeedBackFinishTime = new DateTime(1900, 1, 1);
                    }
                    break;
            }
            if (this.Save() > 0)
            {
                if (IsInner >= 0)
                {
                    //添加订单状态明细
                    OrderStatusDetail osd = new OrderStatusDetail();
                    osd.IsInner = IsInner;
                    osd.OrderId = this.OrderId;
                    osd.Status = Status;
                    osd.UserId = UserId;
                    osd.StatusId = statusId;
                    osd.TPIStatus = CommenClass.Order_Status.已完成.ToString();
                    if (osd.UserId == 0)
                    {
                        osd.UserName = "系统";
                    }
                    else
                    {
                        Sys_Users sys_users = new Sys_Users();
                        sys_users.Id = osd.UserId;
                        if (sys_users.Load())
                        {
                            osd.UserName = sys_users.Name;
                        }
                    }
                    osd.Save();
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 修改订单状态
        /// ERP FFeedBack.cs 中调用
        /// </summary>
        /// <param name="StatusType1"></param>
        /// <param name="StatusValue1"></param>
        /// <param name="deliveryFinishTime"></param>
        /// <returns></returns>
        public bool UpdateStatus(OrderStatusType StatusType1, Order_Status StatusValue1, DateTime deliveryFinishTime)
        {
            string StatusType = StatusType1.ToString();
            string StatusValue = StatusValue1.ToString();
            this.Id = Id;
            this.Load();
            switch (StatusType)
            {
                case "客服":
                    this.ServiceStatus = StatusValue;
                    if (StatusValue == "已完成")
                    {
                        this.ServiceFinishTime = DateTime.Now;
                    }
                    else this.ServiceFinishTime = new DateTime(1900, 1, 1);
                    break;
                case "采购":
                    this.PurchaseStatus = StatusValue;
                    if (StatusValue == "已完成")
                    {
                        this.PurchaseFinishTime = DateTime.Now;
                    }
                    else this.PurchaseFinishTime = new DateTime(1900, 1, 1);
                    break;
                case "仓库":
                    this.StoreStatus = StatusValue;
                    if (StatusValue == "已完成")
                    {
                        this.StoreFinishTime = DateTime.Now;
                    }
                    else this.StoreFinishTime = new DateTime(1900, 1, 1);
                    break;
                case "配送":
                    this.DeliveryStatus = StatusValue;
                    if (StatusValue == "已完成")
                    {
                        this.DeliveryFinishTime = deliveryFinishTime;
                    }
                    else
                    {
                        this.DeliveryFinishTime = new DateTime(1900, 1, 1);
                    }
                    break;
                case "回单":
                    this.FeedBackStatus = StatusValue;
                    if (StatusValue == "已完成")
                    {
                        this.FeedBackFinishTime = DateTime.Now;
                    }
                    else
                    {
                        this.FeedBackFinishTime = new DateTime(1900, 1, 1);
                    }
                    break;
            }
            if (this.Save() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateStatusByOrderId(OrderStatusType StatusType1, Order_Status StatusValue1, DateTime deliveryFinishTime,string orderIds)
        {
            string StatusType = StatusType1.ToString();
            string StatusValue = StatusValue1.ToString();
            bool result = false;
            string sql = "";
            switch (StatusType)
            {
                case "配送":
                    sql = string.Format("update OrderStatus set DeliveryStatus='{0}',DeliveryFinishTime = '{1}'  where OrderId in ({2})", StatusValue, deliveryFinishTime, orderIds);
                    result = m_dbo.ExecuteNonQuery(sql);
                    break;
                case "回单":
                    sql = string.Format("update OrderStatus set FeedBackStatus='{0}',FeedBackFinishTime = '{1}'  where OrderId in ({2})", StatusValue, DateTime.Now, orderIds);
                    result = m_dbo.ExecuteNonQuery(sql);
                    break;
            }
            return result;
        }

        /// <summary>
        /// 扫码开单 -- 改变订单状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="PackageNum"></param>
        /// <returns></returns>
        public int UpdateStatus(int OrderId,int num)
        {
            //执行前提，库存满足需求
            string sql = string.Format("update OrderStatus set ServiceStatus='已完成',PurchaseStatus='已完成',StoreStatus='已完成',DeliveryStatus='配送中' where OrderId={0}",OrderId);
            DateTime ServiceFinishTime = DateTime.Now;
            if (m_dbo.ExecuteNonQuery(sql))
            {
                string sql1 = string.Format("update OrderDetail set PickNum = {0} where OrderId={1} ", num, OrderId);
                if (m_dbo.ExecuteNonQuery(sql1))
                {
                    return 1;
                }
            }
            return 0;
        }
        public int SetPackageNum(int orderId, int PackageNum)
        {
            this.LoadFromOrderId(orderId);
            this.PackageNum = PackageNum;
            this.StoreStatus = CommenClass.Order_Status.已配齐.ToString();
            return this.Save();
        }
        /// <summary>
        /// 批量修改网单的状态
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="os"></param>
        /// <param name="ServiceId"></param>
        /// <returns></returns>
        public int  UpdateServiceStatus(int[] OrderId,Order_Status os, int ServiceId)
        {
            int num = 0;
            DateTime ServiceFinishTime = DateTime.Now;
            string username = "系统";
            if (ServiceId >0)
            {
                Sys_Users sys_users = new Sys_Users();
                sys_users.Id = ServiceId;
                if (sys_users.Load())
                {
                    username = sys_users.Name;
                }
            }
            for (int i = 0; i < OrderId.Length; i++)
            {
                string sql = string.Format("update OrderStatus set ServiceStatus ='{0}' ,ServiceFinishTime='{1}' where OrderId ='{2}'", os, ServiceFinishTime, OrderId[i]);
                if (m_dbo.ExecuteNonQuery(sql))
                {
                    //修改订单的UserId
                    Order order = new Order();
                    order.Id = OrderId[i];
                    order.Load();
                    order.UserId = ServiceId;
                    order.Save();

                    //添加订单状态明细
                    OrderStatusDetail osd = new OrderStatusDetail();
                    osd.IsInner = 0;
                    osd.OrderId = OrderId[i];
                    osd.Status = "您的订单已通过审核!";
                    osd.UserId = ServiceId;
                    osd.StatusId = 2;
                    osd.TPIStatus = CommenClass.Order_Status.已接受.ToString();
                    osd.UserName = username;
                    if (osd.Save() > 0)
                    {
                        num += 1;
                    }
                }
            }
            return num;
        }

    }
}
