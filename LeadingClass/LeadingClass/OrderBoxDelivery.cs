using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class OrderBoxDelivery
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public int CarUserId { get; set; }
        public int StockOutUserId { get; set; }
        public string Status { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public OrderBoxDelivery()
        {
            Id = 0;
            CarId = 0;
            CarUserId = 0;
            StockOutUserId = 0;
            Status = "";
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
            arrayList.Add(new SqlParameter("@CarId", CarId));
            arrayList.Add(new SqlParameter("@CarUserId", CarUserId));
            arrayList.Add(new SqlParameter("@StockOutUserId", StockOutUserId));
            arrayList.Add(new SqlParameter("@Status", Status));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("OrderBoxDelivery", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("OrderBoxDelivery", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from OrderBoxDelivery where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                CarId = DBTool.GetIntFromRow(row, "CarId", 0);
                CarUserId = DBTool.GetIntFromRow(row, "CarUserId", 0);
                StockOutUserId = DBTool.GetIntFromRow(row, "StockOutUserId", 0);
                Status = DBTool.GetStringFromRow(row, "Status", "");
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from OrderBoxDelivery where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 读取出库员正在装车的记录
        /// </summary>
        /// <returns></returns>
        public DataSet ReadLoadingNow()
        {
            string sql = @"select obd.*,su1.Name as CarUserName,su2.Name as StockOutUserName,Plate
            from OrderBoxDelivery obd join Sys_Users su1 on obd.CarUserId =su1.Id join Sys_Users su2 on obd.StockOutUserId = su2.Id  join Sys_Car on CarId=Sys_Car.Id
            where 1=1 and Status='正在装车'";
            if (StockOutUserId > 0)
            {
                sql += string.Format(" and StockOutUserId={0}",StockOutUserId);
            }
            if (CarUserId > 0)
            {
                sql += string.Format(" and CarUserId={0}",CarUserId);
            }
            if (CarId > 0)
            {
                sql += string.Format(" and CarId={0}",CarId);
            }
            return m_dbo.GetDataSet(sql);
        }


        public DataSet GetNewLoading(int carId)
        {
            DataSet newDs = null;
            string sql = string.Format("select * from OrderBoxDelivery where CarId={0} and Status<>'装车完成'",carId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                OrderBoxDeliveryDetail obd = new OrderBoxDeliveryDetail();
                DataSet d = obd.ReadOrderBoxInCar(Convert.ToInt32(ds.Tables[0].Rows[0]["Id"]));
                if (d.Tables[0].Rows.Count > 0)
                {
                    newDs = d;
                }
            }
            return newDs;
        }
    }

    public class OrderBoxDeliveryDetail
    {
        public int Id { get; set; }
        public int OrderBoxDeliveryId { get; set; }
        public int OrderBoxId { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public OrderBoxDeliveryDetail()
        {
            Id = 0;
            OrderBoxDeliveryId = 0;
            OrderBoxId = 0;
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
            arrayList.Add(new SqlParameter("@OrderBoxDeliveryId", OrderBoxDeliveryId));
            arrayList.Add(new SqlParameter("@OrderBoxId", OrderBoxId));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("OrderBoxDeliveryDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("OrderBoxDeliveryDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from OrderBoxDeliveryDetail where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                OrderBoxDeliveryId = DBTool.GetIntFromRow(row, "OrderBoxDeliveryId", 0);
                OrderBoxId = DBTool.GetIntFromRow(row, "OrderBoxId", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from OrderBoxDeliveryDetail where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 读取已装车的包裹
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public DataSet ReadOrderBoxInCar(int OrderBoxDeliveryId, int orderId)
        {
            string sql = "select * from dbo.View_OrderBoxDelivery where 1=1 ";
            if (OrderBoxDeliveryId > 0)
            {
                sql += string.Format(" and OrderBoxDeliveryId={0}",OrderBoxDeliveryId);
            }
            if (orderId > 0)
            {
                sql += string.Format(" and OrderId={0}", orderId);
            }
            sql += " order By UpdateTime desc";
            return m_dbo.GetDataSet(sql);
        }

        public bool LoadbyOrderboxId()
        {
            string sql = string.Format("select * from OrderBoxDeliveryDetail where OrderboxId={0}", OrderBoxId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        public DataSet ReadOrderBoxInCar(int OrderBoxDeliveryId)
        {
            string sql = "select * from dbo.View_OrderBoxDelivery where 1=1 ";
            if (OrderBoxDeliveryId > 0)
            {
                sql += string.Format(" and OrderBoxDeliveryId={0}", OrderBoxDeliveryId);
            }
            sql += " order By UpdateTime desc";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// where CarUserId= {0} where CarUserId={0}, carUserId)int carUserId
        /// </summary>
        /// <returns></returns>
        public DataSet ReadOrderBoxInCar()
        {
            string sql = string.Format(@"select OrderId,COUNT(orderId) as count from dbo.View_OrderBoxDelivery  where  OrderBoxDeliveryId ={0} group by OrderId;select OrderId,COUNT(OrderId) as count from OrderBox where OrderId in(select orderId from dbo.View_OrderBoxDelivery where  OrderBoxDeliveryId ={0} )  group by OrderId", OrderBoxDeliveryId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取配送完成的订单量(当天，本周，本月)
        /// </summary>
        /// <param name="CarUsersId"></param>
        /// <param name="today"></param>
        /// <param name="startWeek"></param>
        /// <param name="endWeek"></param>
        /// <param name="startMonth"></param>
        /// <param name="endMonth"></param>
        /// <returns></returns>
        public DataSet ReadDeliveryOkCount(int CarUsersId, DateTime today, DateTime startWeek, DateTime endWeek, DateTime startMonth, DateTime endMonth)
        {
            string sql = string.Format(@" select Company,RealName,OrderId,DeliveryFinishTime,DeliveryStatus,sumMoney from View_OrderBoxDelivery where CarUserId ={0} and DeliveryStatus='已完成' and DeliveryFinishTime>'{1}' and DeliveryFinishTime<'{2}' group by OrderId,Company,RealName,DeliveryFinishTime,DeliveryStatus,sumMoney;
 select Company,RealName,OrderId,DeliveryFinishTime,DeliveryStatus,sumMoney from View_OrderBoxDelivery where CarUserId ={0} and DeliveryStatus='已完成' and DeliveryFinishTime>'{3}' and DeliveryFinishTime<'{4}' group by OrderId,Company,RealName,DeliveryFinishTime,DeliveryStatus,sumMoney;
 select Company,RealName,OrderId,DeliveryFinishTime,DeliveryStatus,sumMoney from View_OrderBoxDelivery where CarUserId ={0} and DeliveryStatus='已完成' and DeliveryFinishTime>'{5}' and DeliveryFinishTime<'{6}' group by OrderId,Company,RealName,DeliveryFinishTime,DeliveryStatus,sumMoney;", CarUsersId, today.ToShortDateString(), today.AddDays(1).ToShortDateString(), startWeek, endWeek, startMonth, endMonth);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 确认收货时的订单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public DataSet ReadOrderInCar(int userId,string status)
        {
            string sql = " select Company,RealName,OrderId,DeliveryFinishTime,DeliveryStatus,SumMoney,COUNT(OrderId) as BoxCount  from View_OrderBoxDelivery where 1=1 ";
            if (userId > 0)
            {
                sql += string.Format(" and CarUserId ={0}",userId);
            }
            if (!string.IsNullOrEmpty(status))
            {
                sql += string.Format(" and DeliveryStatus='{0}'",status);
            }
            sql += " group by OrderId,Company,RealName,DeliveryFinishTime,DeliveryStatus,SumMoney ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 检查当前车辆是否可以解锁(OrderBoxDelivery表的carid最大的一条记录(找到最近装这个车)，并且单子中没有配送完成的)
        /// </summary>
        /// <returns></returns>
        public int IsUnlock(int carId)
        {
            int result = 0;
            string sql = "";
            if (carId > 0)
            {
                sql += string.Format(@"select top 1 Id from OrderBoxDelivery where CarId ={0} order by Id desc;
select * from View_OrderBoxDelivery where OrderBoxDeliveryId=(select top 1 Id from OrderBoxDelivery where CarId ={0} order by Id desc) and DeliveryStatus in ('已完成')", carId);
                DataSet ds = m_dbo.GetDataSet(sql);
                if (ds.Tables[1].Rows.Count == 0)
                {
                    if (ds.Tables[0].Rows.Count>0) 
                        result = Convert.ToInt32(ds.Tables[0].Rows[0]["Id"]);
                }
            }
            return result;
        }
        
    }

    public class OrderBoxDeliveryOption
    {
        public int BranchId { get; set; } 
        public string DeliveryStutas { get; set; } //配送状态
        public int CarUsersId { get; set; }//配送司机Id
        public string CarUsersName { get; set; }//配送司机name
        public int StockOutUserId { get; set; }//出库员Id
        public string StockOutUserName { get; set; }//出库员name
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public OrderBoxDeliveryOption()
        {
            BranchId = 0;
            DeliveryStutas = "未处理";
            CarUsersId = 0;
            CarUsersName = ""; ;
            StockOutUserId = 0;
            StockOutUserName = "";
            startDate = new DateTime(1900, 1, 1);
            endDate = new DateTime(1900, 1, 1);
        }
    }
    public class OrderBoxDeliveryManager
    {
        private OrderBoxDeliveryOption m_option;
        private DBOperate m_dbo;
        public OrderBoxDeliveryOption option { get { return m_option; } set { m_option = value; } }
        public OrderBoxDeliveryManager()
        {
            m_option = new OrderBoxDeliveryOption();
            m_dbo = new DBOperate();
        }
        /// <summary>
        /// 读取配送报表（使用行者系统的）
        /// </summary>
        /// <returns></returns>
        public DataSet ReadDeliveryReport()
        {
            string sql = " select Company,OrderId,COUNT(OrderId) as BoxNum,CarUserName,DeliveryFinishTime from View_OrderBoxDelivery where 1=1 ";
            if (option.BranchId > 0)
            {
                sql += string.Format(" and BranchId={0}", option.BranchId);
            }
            if (option.DeliveryStutas != "")
            {
                sql += string.Format(" and DeliveryStatus='{0}'", option.DeliveryStutas);
            }
            if (option.CarUsersId > 0)
            {
                sql += string.Format(" and CarUserId={0}", option.CarUsersId);
            }
            if (option.StockOutUserId > 0)
            {
                sql += string.Format(" and StockOutUserId={0}", option.StockOutUserId);
            }
            if (option.endDate > new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and DeliveryFinishTime <'{0}' ", option.endDate.AddDays(1).ToShortDateString());
            }
            if (option.startDate > new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and DeliveryFinishTime >= '{0}' ", option.startDate.ToShortDateString());
            }
            sql += " group by OrderId,CarUserName,DeliveryFinishTime,Company order by OrderId";
            return m_dbo.GetDataSet(sql);
        }
    }
}
