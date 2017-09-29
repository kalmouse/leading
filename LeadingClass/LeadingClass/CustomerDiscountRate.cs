using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class CustomerDiscountRate
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int ComId { get; set; }
        public int Rate { get; set; }
        public int UserId { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public CustomerDiscountRate()
        {
            Id = 0;
            TypeId = 0;
            ComId = 0;
            Rate = 0;
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
            arrayList.Add(new SqlParameter("@TypeId", TypeId));
            arrayList.Add(new SqlParameter("@ComId", ComId));
            arrayList.Add(new SqlParameter("@Rate", Rate));
            arrayList.Add(new SqlParameter("@UserId", UserId));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("VIPCounterRate", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("VIPCounterRate", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from VIPCounterRate where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                TypeId = DBTool.GetIntFromRow(row, "TypeId", 0);
                ComId = DBTool.GetIntFromRow(row, "ComId", 0);
                Rate = DBTool.GetIntFromRow(row, "Rate", 0);
                UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from VIPCounterRate where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 获取商品对应的折扣率
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataSet ReadVipCounterRate(string code)
        {
            string sql = string.Format("select top 1 * from dbo.View_CustomerRate where  Code in('{0}','{1}','{2}') order by Level desc;",code.Substring(0,2),code.Substring(0,4),code);
            return m_dbo.GetDataSet(sql);
        }
    }
}
