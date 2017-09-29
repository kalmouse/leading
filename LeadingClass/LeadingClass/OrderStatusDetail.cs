using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class OrderStatusDetail
    {
        private int m_Id;
        private int m_OrderId;
        private int m_UserId;
        private string m_UserName;
        private string m_Status;
        private int m_IsInner;
        private DateTime m_OperateTime;
        private int m_StatusId;
        private string m_TPIStatus;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int OrderId { get { return m_OrderId; } set { m_OrderId = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public string UserName { get { return m_UserName; } set { m_UserName = value; } } //add by luochunhui
        public string Status { get { return m_Status; } set { m_Status = value; } }
        public int IsInner { get { return m_IsInner; } set { m_IsInner = value; } }
        public DateTime OperateTime { get { return m_OperateTime; } set { m_OperateTime = value; } }
        public int StatusId { get { return m_StatusId; } set { m_StatusId = value; } }
        public string TPIStatus { get { return m_TPIStatus; } set { m_TPIStatus = value; } }
        public OrderStatusDetail()
        {
            m_Id = 0;
            m_OrderId = 0;
            m_UserId = 0;
            m_Status = "";
            m_IsInner = 0;
            m_OperateTime = DateTime.Now;
            m_StatusId = 0;
            m_TPIStatus = "";
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
            arrayList.Add(new SqlParameter("@UserId", m_UserId));
            arrayList.Add(new SqlParameter("@Status", m_Status));
            arrayList.Add(new SqlParameter("@IsInner", m_IsInner));
            arrayList.Add(new SqlParameter("@OperateTime", m_OperateTime));
            arrayList.Add(new SqlParameter("@UserName", m_UserName));
            arrayList.Add(new SqlParameter("@StatusId", m_StatusId));
            arrayList.Add(new SqlParameter("@TPIStatus", m_TPIStatus));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("OrderStatusDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("OrderStatusDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from OrderStatusDetail where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_OrderId = DBTool.GetIntFromRow(row, "OrderId", 0);
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_Status = DBTool.GetStringFromRow(row, "Status", "");
                m_IsInner = DBTool.GetIntFromRow(row, "IsInner", 0);
                m_OperateTime = DBTool.GetDateTimeFromRow(row, "OperateTime");
                m_StatusId = DBTool.GetIntFromRow(row, "StatusId", 0);
                m_TPIStatus = DBTool.GetStringFromRow(row, "TPIStatus", "");
                return true;
            }
            return false;
        }

        public bool Delete(int OrderId, int statusId)
        {
            string sql = string.Format("delete from OrderStatusDetail where OrderId ='{0}' and StatusId='{1}'", OrderId, statusId);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 读取订单物流 add by luochunhui 07-14
        /// </summary>
        /// <returns></returns>
        public DataSet ReadOrderStatusDetail(int OrderId, int IsInner)
        {
            string sql = string.Format("select * from dbo.OrderStatusDetail where OrderId={0}", OrderId);
            if (IsInner != -1)
            {
                sql += string.Format(" and IsInner = {0} ", IsInner);
            }
            sql += " order by Id ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 订单物流，订单详情
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="IsInner"></param>
        /// <returns></returns>
        public DataSet ReadOrderByStatusDetail(int orderId, int goodsId)
        {
            string sql = string.Format("select * from dbo.OrderStatusDetail  join OrderDetail on OrderStatusDetail.OrderId= OrderDetail.OrderId  where  OrderDetail.OrderId={0}  and IsInner = 0", orderId);
            if (goodsId>0)
            {
                sql += string.Format(" and goodsId = {0} ", goodsId);
            }
            sql += " order by OrderStatusDetail.Id ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取订单最新状态 add by wangpengliang 8-5
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="IsInner"></param>
        /// <returns></returns>
        public DataSet ReadOrderStatus(int OrderId, int IsInner)
        {
            string sql = string.Format("select top 1 * from dbo.OrderStatusDetail where OrderId={0}", OrderId);
            if (IsInner != -1)
            {
                sql += string.Format(" and IsInner = {0} ", IsInner);
            }
            sql += " order by OperateTime desc ";
            return m_dbo.GetDataSet(sql);
        }
        public string ReadMemberNewStatus(int memberId)
        {
            string sql = string.Format(@"select top 1 * from OrderStatusDetail where OrderId=(select top 1 Id from [Order] where MemberId ={0} order by OrderTime desc ) order by Status desc", memberId);
            DataSet ds = m_dbo.GetDataSet(sql);
            string str = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                str = ds.Tables[0].Rows[0]["OrderId"].ToString() + ds.Tables[0].Rows[0]["Status"].ToString();
            }
            return str;
        }
        /// <summary>
        /// 返回对应订单号的订单最新信息 add by zhangyuefeng 2017-8-30
        /// </summary>
        /// <param name="OrderIds"></param>
        /// <param name="IsInner"></param>
        /// <returns></returns>
        public DataSet ReadOrderStatus(List<int> OrderIds, int IsInner)
        {
            if (OrderIds.Count < 1)
            {
                return null;
            }
            string orderIds = "";
            foreach (var orderid in OrderIds)
            {
                orderIds += "," + orderid;
            }
            orderIds = orderIds.Substring(1);
            string sql = string.Format("select s.* from (select *, row_number() over (partition by orderid order by id desc) as group_idx from OrderStatusDetail ) s where s.group_idx = 1 and OrderId in({0})", orderIds);
            if (IsInner != -1)
            {
                sql += string.Format(" and IsInner = {0} ", IsInner);
            }
            sql += " order by Id ";
            return m_dbo.GetDataSet(sql);
        }
       
        /// <summary>
        /// 读取订单物流 add by zhangyuefeng 2017-8-30
        /// </summary>
        /// <param name="OrderIds"></param>
        /// <param name="IsInner"></param>
        /// <returns></returns>
        public DataSet ReadOrderStatusDetail(List<int> OrderIds, int IsInner)
        {
            if (OrderIds.Count < 1)
            {
                return null;
            }
            string orderIds = "";
            foreach (var orderid in OrderIds)
            {
                orderIds += "," + orderid;
            }
            orderIds = orderIds.Substring(1);
            string sql = string.Format("select * from dbo.OrderStatusDetail where OrderId in({0})", orderIds);
            if (IsInner != -1)
            {
                sql += string.Format(" and IsInner = {0} ", IsInner);
            }
            sql += " order by OrderId, Id";
            return m_dbo.GetDataSet(sql);
        }
    }
}
