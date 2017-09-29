using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class OrderBox
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int BoxId { get; set; }
        public string Status { get; set; }
        public int PrintNum { get; set; }
        public string StoreZone { get; set; }
        public string OrderBoxStatus { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public OrderBox()
        {
            Id = 0;
            OrderId = 0;
            BoxId = 0;
            Status = "";
            PrintNum = 0;
            StoreZone = "";
            OrderBoxStatus = "未处理";
            UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public OrderBox(int Id)
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
            arrayList.Add(new SqlParameter("@BoxId", BoxId));
            arrayList.Add(new SqlParameter("@Status", Status));
            arrayList.Add(new SqlParameter("@PrintNum", PrintNum));
            arrayList.Add(new SqlParameter("@StoreZone", StoreZone));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@OrderBoxStatus", OrderBoxStatus));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("OrderBox", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("OrderBox", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from OrderBox where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }
        public bool Load(int OrderId, int BoxId)
        {
            string sql = string.Format("select * from OrderBox where OrderId={0} and BoxId={1}", OrderId, BoxId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }

        public void LoadFromRow(DataRow row)
        {
            Id = DBTool.GetIntFromRow(row, "Id", 0);
            OrderId = DBTool.GetIntFromRow(row, "OrderId", 0);
            BoxId = DBTool.GetIntFromRow(row, "BoxId", 0);
            Status = DBTool.GetStringFromRow(row, "Status", "");
            PrintNum = DBTool.GetIntFromRow(row, "PrintNum", 0);
            StoreZone = DBTool.GetStringFromRow(row, "StoreZone", "");
            UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
            OrderBoxStatus = DBTool.GetStringFromRow(row, "OrderBoxStatus", "");
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from OrderBox where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        public void AddPrintNum()
        {
            this.PrintNum += 1;
            this.UpdateTime = DateTime.Now;
            this.Save();
        }
        public int ReadBoxCountByOrderId(int OrderId)
        {
            string sql = string.Format("select * from OrderBox where OrderId={0}", OrderId);
            DataSet ds = m_dbo.GetDataSet(sql);
            return ds.Tables[0].Rows.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public bool IsBoxOrder(int OrderId)
        {
            string sql = string.Format("select COUNT(Id) from OrderBox where OrderId={0}", OrderId);
            object o = m_dbo.ExecuteScalar(sql);
            try
            {
                int count = Convert.ToInt32(o);
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }


        }
        /// <summary>
        /// 查找一个订单以装车的包裹
        /// </summary>
        /// <returns></returns>
        public int GetrBoxForDelivery()
        {
            string sql = string.Format("select COUNT(*)  from OrderBox where OrderBoxStatus='待配送' and OrderId={0}", OrderId);
            object o = m_dbo.ExecuteScalar(sql);
            try
            {
                return Convert.ToInt32(o);
            }
            catch
            {
                return 0;
            }

        }

        public DataSet GetNotLoadingBox(int orderId, string status)
        {
            string sql = " select * from OrderBox where 1=1";
            if (orderId > 0)
            {
                sql += string.Format(" and OrderId={0}", orderId);
            }
            if (status != "")
            {
                sql += string.Format(" and OrderBoxStatus='{0}'", status);
            }
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取订单所有包裹
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public DataSet GetOrderBoxs(int OrderId)
        {
            string sql = string.Format("select * from OrderBox where OrderId={0}", OrderId);
            return m_dbo.GetDataSet(sql);
        }
    }
}
