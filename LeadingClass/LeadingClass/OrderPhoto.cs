using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class OrderPhoto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int Page { get; set; }
        public string PhotoUrl { get; set; }
        public int SaveNum { get; set; }
        public int UserId { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public OrderPhoto()
        {
            Id = 0;
            OrderId = 0;
            Page = 0;
            PhotoUrl = "";
            SaveNum = 0;
            UserId = 0;
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
            arrayList.Add(new SqlParameter("@OrderId", OrderId));
            arrayList.Add(new SqlParameter("@Page", Page));
            arrayList.Add(new SqlParameter("@PhotoUrl", PhotoUrl));
            arrayList.Add(new SqlParameter("@SaveNum", SaveNum));
            arrayList.Add(new SqlParameter("@UserId", UserId));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("OrderPhoto", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("OrderPhoto", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from OrderPhoto where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }
        public bool Load(int orderId, int page)
        {
            string sql = string.Format("select * from OrderPhoto where OrderId={0} and page={1}", orderId, page);
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
            Page = DBTool.GetIntFromRow(row, "Page", 0);
            PhotoUrl = DBTool.GetStringFromRow(row, "PhotoUrl", "");
            SaveNum = DBTool.GetIntFromRow(row, "SaveNum", 0);
            UserId = DBTool.GetIntFromRow(row, "UserId", 0);
            UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from OrderPhoto where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 通过订单Id 获取所有归档的订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public DataSet GetPhotoByOrderId(int[] orderIds)
        {
            string sql = "select * from OrderPhoto where 1=1";
            if (orderIds != null)
            {
                sql += " and OrderId in ( ";
                for (int i = 0; i < orderIds.Length; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", orderIds[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", orderIds[i]);
                }
                sql += " ) ";
            }
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取订单有多少 图片归档
        /// </summary>
        /// <returns></returns>
        public int ReadPhotoNum()
        {
            string sql = string.Format("select COUNT(OrderId) as num from orderphoto where OrderId={0} ", this.OrderId);
            DataSet ds = m_dbo.GetDataSet(sql);
            return DBTool.GetIntFromRow(ds.Tables[0].Rows[0], "num", 0);
        }
    }


}
