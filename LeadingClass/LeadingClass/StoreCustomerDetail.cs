using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class StoreCustomerDetail
    {
        public int Id { get; set; }
        public int DeptId { get; set; }
        public int ComId { get; set; }
        public int StoreId { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public StoreCustomerDetail()
        {
            Id = 0;
            DeptId = 0;
            ComId = 0;
            StoreId = 0;
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
            arrayList.Add(new SqlParameter("@DeptId", DeptId));
            arrayList.Add(new SqlParameter("@ComId", ComId));
            arrayList.Add(new SqlParameter("@StoreId", StoreId));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("StoreCustomerDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("StoreCustomerDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from StoreCustomerDetail where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                DeptId = DBTool.GetIntFromRow(row, "DeptId", 0);
                ComId = DBTool.GetIntFromRow(row, "ComId", 0);
                StoreId = DBTool.GetIntFromRow(row, "StoreId", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from StoreCustomerDetail where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}
