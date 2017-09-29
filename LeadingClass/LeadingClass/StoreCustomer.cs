using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class StoreCustomer
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public DateTime Updatetime { get; set; }
        private DBOperate m_dbo;

        public StoreCustomer()
        {
            Id = 0;
            StoreName = "";
            Updatetime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@StoreName", StoreName));
            arrayList.Add(new SqlParameter("@Updatetime", Updatetime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("StoreCustomer", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("StoreCustomer", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from StoreCustomer where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                StoreName = DBTool.GetStringFromRow(row, "StoreName", "");
                Updatetime = DBTool.GetDateTimeFromRow(row, "Updatetime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from StoreCustomer where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        ///// <summary>
        ///// 根据公司查看所有设置的仓库
        ///// </summary>
        ///// <returns></returns>
        //public DataSet GetStoreByComId()
        //{
        //    string sql = string.Format("select * from StoreCustomer where ComId={0} ", ComId);
        //    return m_dbo.GetDataSet(sql);
        //}
    }
}
