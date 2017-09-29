using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
namespace LeadingClass
{
    public class OrderDelivery
    {
        private int m_Id;
        private int m_OrderId;
        private int m_DeliveryUserId;
        private int m_DistributeUserId;
        private DateTime m_UpdateTime;
        private int m_PrintNum;
        private string m_Address;
        private string m_Latitude;
        private string m_Longitude;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int OrderId { get { return m_OrderId; } set { m_OrderId = value; } }
        public int DeliveryUserId { get { return m_DeliveryUserId; } set { m_DeliveryUserId = value; } }
        public int DistributeUserId { get { return m_DistributeUserId; } set { m_DistributeUserId = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public int PrintNum { get { return m_PrintNum; } set { m_PrintNum = value; } }
        public string Address { get { return m_Address; } set { m_Address = value; } }
        public string Latitude { get { return m_Latitude; } set { m_Latitude = value; } }
        public string Longitude { get { return m_Longitude; } set { m_Longitude = value; } }
        public OrderDelivery()
        {
            m_Id = 0;
            m_OrderId = 0;
            m_DeliveryUserId = 0;
            m_DistributeUserId = 0;
            m_UpdateTime = DateTime.Now;
            m_PrintNum = 0;
            m_Address = "";
            m_Latitude = "";
            m_Longitude = "";
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@OrderId", m_OrderId));
            arrayList.Add(new SqlParameter("@DeliveryUserId", m_DeliveryUserId));
            arrayList.Add(new SqlParameter("@DistributeUserId", m_DistributeUserId));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            arrayList.Add(new SqlParameter("@PrintNum", m_PrintNum));
            arrayList.Add(new SqlParameter("@Address", m_Address));
            arrayList.Add(new SqlParameter("@Latitude", m_Latitude));
            arrayList.Add(new SqlParameter("@Longitude", m_Longitude));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("OrderDelivery", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("OrderDelivery", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            OrderBox ob = new OrderBox();
            if (!ob.IsBoxOrder(this.OrderId))
            {
                if (this.Id > 0)//扫码装车不能在这里修改订单状态
                {
                    //修改订单状态 
                    OrderStatus os = new OrderStatus();
                    os.LoadFromOrderId(this.OrderId);
                    os.UpdateStatus(CommenClass.OrderStatusType.配送, CommenClass.Order_Status.已接受, m_DeliveryUserId);
                }
            }          
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from OrderDelivery where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_OrderId = DBTool.GetIntFromRow(row, "OrderId", 0);
                m_DeliveryUserId = DBTool.GetIntFromRow(row, "DeliveryUserId", 0);
                m_DistributeUserId = DBTool.GetIntFromRow(row, "DistributeUserId", 0);
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_PrintNum = DBTool.GetIntFromRow(row, "PrintNum", 0);
                m_Address = DBTool.GetStringFromRow(row, "Address","");
                m_Latitude = DBTool.GetStringFromRow(row, "Latitude", "");
                m_Longitude = DBTool.GetStringFromRow(row, "Longitude", "");
                return true;
            }
            return false;
        }
        public bool LoadFromOrderId(int orderId)
        {
            string sql = string.Format("select * from OrderDelivery where OrderId={0}", orderId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_OrderId = DBTool.GetIntFromRow(row, "OrderId", 0);
                m_DeliveryUserId = DBTool.GetIntFromRow(row, "DeliveryUserId", 0);
                m_DistributeUserId = DBTool.GetIntFromRow(row, "DistributeUserId", 0);
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_PrintNum = DBTool.GetIntFromRow(row, "PrintNum", 0);
                m_Address = DBTool.GetStringFromRow(row, "Address", "");
                m_Latitude = DBTool.GetStringFromRow(row, "Latitude", "");
                m_Longitude = DBTool.GetStringFromRow(row, "Longitude", "");
                return true;
            }
            return false;
        }
    }

    public class OrderDeliveryOpiton
    {
        private int m_DeliveryUserId;
        private DateTime m_StartTime;
        private DateTime m_EndTime;
        private int m_IsPrint;
        private int m_ComId;
        private DateTime m_OrderTImeS;
        private DateTime m_OrderTimeE;
        private DateTime m_PlanDateS;
        private DateTime m_PlanDateE;
        private DateTime m_FinishDateS;
        private DateTime m_FinishDateE;
        private int m_ServiceId;
        private string m_DeliveryStatus;
        private string m_Company;
        private int m_OrderId;//add by quxiaoshan 2014-11-7
        private int m_BranchId;//lin 2015-4-30
        public int DeliveryUserId { get { return m_DeliveryUserId; } set { m_DeliveryUserId = value; } }
        public DateTime StartTime { get { return m_StartTime; } set { m_StartTime = value; } }
        public DateTime EndTime { get { return m_EndTime; } set { m_EndTime = value; } }
        public int IsPrint { get { return m_IsPrint; } set { m_IsPrint = value; } }
        public int ComId { get { return m_ComId; } set { m_ComId = value; } }
        public DateTime OrderTImeS { get { return m_OrderTImeS; } set { m_OrderTImeS = value; } }
        public DateTime OrderTimeE { get { return m_OrderTimeE; } set { m_OrderTimeE = value; } }
        public DateTime PlanDateS { get { return m_PlanDateS; } set { m_PlanDateS = value; } }
        public DateTime PlanDateE { get { return m_PlanDateE; } set { m_PlanDateE = value; } }
        public DateTime FinishDateS { get { return m_FinishDateS; } set { m_FinishDateS = value; } }
        public DateTime FinishDateE { get { return m_FinishDateE; } set { m_FinishDateE = value; } }
        public int ServiceId { get { return m_ServiceId; } set { m_ServiceId = value; } }
        public string DeliveryStatus { get { return m_DeliveryStatus; } set { m_DeliveryStatus = value; } } 
        public string Company { get { return m_Company; } set { m_Company = value; } }
        public int OrderId { get { return m_OrderId; } set { m_OrderId = value; } }//add by quxiaoshan 2014-11-7
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public OrderDeliveryOpiton()
        {
            m_OrderId = 0;//add by quxiaoshan 2014-11-7
            m_DeliveryUserId = 0;
            m_StartTime = new DateTime(1900, 1, 1);
            m_EndTime = new DateTime(1900, 1, 1);
            m_IsPrint = 0;
            m_ComId = 0;
            m_OrderTImeS = new DateTime(1900, 1, 1);
            m_OrderTimeE = new DateTime(1900, 1, 1);
            m_PlanDateS = new DateTime(1900, 1, 1);
            m_PlanDateE = new DateTime(1900, 1, 1);
            m_FinishDateS = new DateTime(1900, 1, 1);
            m_FinishDateE = new DateTime(1900, 1, 1);
            m_ServiceId = 0;
            m_DeliveryStatus = "";
            m_Company = "";
            m_BranchId = 0;
        }

 
    }
    public class OrderDeliveryManager
    {
        private DBOperate m_dbo;
        private OrderDeliveryOpiton m_option;
        public OrderDeliveryOpiton option { get { return m_option; } set { m_option = value; } }

        public OrderDeliveryManager()
        {
            m_dbo = new DBOperate();
            option = new OrderDeliveryOpiton();
        }
        /// <summary>
        /// 删除配送分配记录，重新分配时用到
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool DeleteOrderDelivery(int orderId)
        {
            string sql = string.Format(" delete from OrderDelivery where OrderId={0} ",orderId);
            if (m_dbo.ExecuteNonQuery(sql))
            {
                OrderStatus os = new OrderStatus();
                os.LoadFromOrderId(orderId);
                return os.UpdateStatus(CommenClass.OrderStatusType.配送, CommenClass.Order_Status.未处理,0);
            }
            else return false;
               
        }
        /// <summary>
        /// 读取配送记录
        /// </summary>
        /// <returns></returns>
        public DataSet ReadDelivery()
        {
            string sql = " select * from View_OrderDelivery where 1=1 ";
            if (option.BranchId > 1)
            {
                sql += string.Format(" and BranchId={0}", option.BranchId);
            }
            if (option.OrderId > 0)
            {
                sql += string.Format(" and OrderId ={0} ",option.OrderId);
            }
            else if (option.OrderId == 0)
            {
                if (option.ComId != 0)
                {
                    sql += string.Format(" and ComId={0} ", option.ComId);
                }
                if (option.Company != "")
                {
                    sql += string.Format(" and Company like '%{0}%' ", option.Company);
                }
                if (option.DeliveryUserId != 0)
                {
                    sql += string.Format(" and DeliveryUserId = {0} ", option.DeliveryUserId);
                }
                if (option.EndTime != new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and UpdateTime <= '{0}' ", option.EndTime.AddDays(1).ToShortDateString());
                }
                if (option.StartTime != new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and UpdateTime > '{0}' ", option.StartTime.ToShortDateString());
                }
                if (option.FinishDateE != new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and UpdateTime <= '{0}' ", option.FinishDateE.AddDays(1).ToShortDateString());
                }
                if (option.FinishDateS != new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and UpdateTime > '{0}' ", option.FinishDateS.ToShortDateString());
                }
                if (option.IsPrint != 0)
                {
                    sql += " and PrintNum >0 ";
                }
                else sql += " and PrintNum =0";
                if (option.OrderTimeE != new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and UpdateTime <= '{0}' ", option.OrderTimeE.AddDays(1).ToShortDateString());
                }
                if (option.OrderTImeS != new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and UpdateTime > '{0}' ", option.OrderTImeS.ToShortDateString());
                }
                if (option.PlanDateE != new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and UpdateTime <= '{0}' ", option.PlanDateE.AddDays(1).ToShortDateString());
                }
                if (option.PlanDateS != new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and UpdateTime > '{0}' ", option.PlanDateS.ToShortDateString());
                }
                if (option.ServiceId != 0)
                {
                    sql += string.Format(" and ServiceId={0} ", option.ServiceId);
                }
                if (option.DeliveryStatus != "")
                {
                    sql += string.Format(" and DeliveryStatus = '{0}' ", option.DeliveryStatus);
                }   
            }
            sql += " order by Company,OrderId ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取当前配送情况
        /// </summary>
        /// <returns></returns>
        public DataSet ReadCurrentDelivery(int branchId)
        {
            string sql = @"select DeliveryName,COUNT(DeliveryName) as OrderNum,SUM(packageNum) as PackageNum,SUM(SumMoney) as SumMoney 
                            from view_orderdelivery where deliveryStatus = '已接受' ";
                           
            if (branchId > 1)
            {
                sql += string.Format(" and BranchId={0}", branchId);
            }
            sql += "  group by deliveryName  order by OrderNum desc ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取当前未完成订单统计表，用来打印
        /// </summary>
        /// <returns></returns>
        public DataSet ReadCurrentDeliveryNeedPrint(int branchId)
        {
            string sql = @"select DeliveryName,Company, COUNT(DeliveryName) as OrderNum,SUM(packageNum) as PackageNum,SUM(SumMoney) as SumMoney 
                        from view_orderdelivery where deliveryStatus = '已接受' and PrintNum=0  ";
                      
            if (branchId > 1)
            {
                sql += string.Format(" and BranchId={0}", branchId);
            }
            sql += "  group by deliveryName,Company order by deliveryName,Company";
            return m_dbo.GetDataSet(sql);
        }
    }
}
