using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class OrderPicking
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int PickingUserId { get; set; }
        public int DistributeUserId { get; set; }
        public DateTime UpdateTime { get; set; }
        public int PrintNum { get; set; }
        public int PackageUserId { get; set; }
        public DateTime PrintTime { get; set; }
        private DBOperate m_dbo;
        
        public OrderPicking()
        {
            Id = 0;
            OrderId = 0;
            PickingUserId = 0;
            DistributeUserId = 0;
            UpdateTime = DateTime.Now;
            PrintNum = 0;
            PackageUserId = 0;
            PrintTime = DateTime.Now;
            m_dbo = new DBOperate(); 
        }
        public OrderPicking(int OrderId)
            : this()
        {
            LoadFromOrderId(OrderId);
        }

        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@OrderId", OrderId));
            arrayList.Add(new SqlParameter("@PickingUserId", PickingUserId));
            arrayList.Add(new SqlParameter("@DistributeUserId", DistributeUserId));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@PrintNum", PrintNum));
            arrayList.Add(new SqlParameter("@PackageUserId", PackageUserId));
            arrayList.Add(new SqlParameter("@PrintTime", PrintTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("OrderPicking", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("OrderPicking", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
                OrderStatus os = new OrderStatus();
                os.LoadFromOrderId(this.OrderId);
                os.UpdateStatus(CommenClass.OrderStatusType.仓库, CommenClass.Order_Status.已接受, PickingUserId);
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from OrderPicking where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count ==1)
            {
                return LoadFromRow(ds.Tables[0].Rows[0]);
            }
            return false;
        }
        public bool LoadFromOrderId(int orderId)
        {
            string sql = string.Format("select * from OrderPicking where orderId={0} ", orderId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return LoadFromRow(ds.Tables[0].Rows[0]);
            }
            return false;
        }
        private bool LoadFromRow(DataRow row)
        {
            Id = DBTool.GetIntFromRow(row, "Id", 0);
            OrderId = DBTool.GetIntFromRow(row, "OrderId", 0);
            PickingUserId = DBTool.GetIntFromRow(row, "PickingUserId", 0);
            DistributeUserId = DBTool.GetIntFromRow(row, "DistributeUserId", 0);
            UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
            PrintNum = DBTool.GetIntFromRow(row, "PrintNum", 0);
            PackageUserId = DBTool.GetIntFromRow(row, "PackageUserId", 0);
            PrintTime = DBTool.GetDateTimeFromRow(row, "PrintTime");
            return true;
        }

        public bool Delete()
        {
            this.Load();
            string sql = string.Format(" delete from OrderPicking where Id = {0} ", Id);
            if (m_dbo.ExecuteNonQuery(sql))
            {
                OrderStatus os = new OrderStatus();
                os.LoadFromOrderId(this.OrderId);
                os.UpdateStatus(CommenClass.OrderStatusType.仓库, CommenClass.Order_Status.未处理,0);
                return true;
            }
            else return false;
        }
    }

    public class OrderPickingManager
    {
        /// <summary>
        /// 读取当前 未完成、已分配的订单统计
        /// </summary>
        /// <returns></returns>
        public static DataSet ReadCurrentPicking(int branchId)
        {
            string sql = string.Format(@"select PickingName,COUNT(PickingName) as OrderNum,SUM(RowNum) as RowNum,SUM(SumMoney) as SumMoney
                                        from View_OrderPicking where StoreStatus = '已接受' and BranchId ={0} group by PickingName", branchId);
            DBOperate dbo = new DBOperate();
            return dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取未打印的理货单
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public static DataSet ReadCurrentNotPrintPicking(int branchId)
        {
            string sql = string.Format(@"select * from View_OrderPicking where StoreStatus <> '已完成' and PrintNum =0 and BranchId={0} order by updatetime ", branchId);
            DBOperate dbo = new DBOperate();
            return dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取自己需要拣货的订单
        /// </summary>
        /// <param name="userId">当前拣货员的Id</param>
        /// <returns></returns>
        public static DataSet ReadNeedToPicking(int branchId,int storeId,int userId)
        {
            DBOperate dbo = new DBOperate();
            string sql = string.Format(@"
                                   select OrderId,Company,RowNum,OrderTime,StoreStatus,Emergency,level,Address,Memo,(
	                select COUNT(*)
	                    from (
			                select num 	from OrderDetail where OrderId=View_OrderPicking.OrderId and IsShow=1 and IsCalc=1 
			                and ((select SUM(Num) from GoodsStore where GoodsId=OrderDetail.GoodsId and BranchId={0} and StoreId={1} )>=(Num-PickNum))
		                ) TL) as OkNum,(
			                select  COUNT(*) from OrderDetail where OrderId=View_OrderPicking.OrderId and IsShow=1 and IsCalc=1 
			                and Num<>PickNum
		                ) as DiffNum
                from View_OrderPicking  
                where PickingUserId = {2} and ( StoreStatus='配货中' or StoreStatus='已接受' or StoreStatus='已暂停') and IsDelete=0
                order by OrderId desc
            ", branchId,storeId,userId, CommenClass.Order_Status.配货中.ToString(), CommenClass.Order_Status.已接受.ToString(), CommenClass.Order_Status.已暂停.ToString());
            return dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取个人待打印订单：订单打印页面用
        /// </summary>
        /// <param name="pickingUserId">拣货员Id</param>
        /// <returns></returns>
        public static DataSet ReadNeedToPrintOrder(int pickingUserId)
        {
            DBOperate dbo = new DBOperate();
            string sql = string.Format(@"select * from View_OrderPicking 
                where PickingUserId = {0} and StoreStatus='已配齐' order by UpdateTime", pickingUserId);
            return dbo.GetDataSet(sql);
        }
    }

}
