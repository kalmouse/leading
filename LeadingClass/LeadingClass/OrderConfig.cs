using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class OrderConfig
    {
        public int Id { get; set; }
        public string ConfigType { get; set; }
        public string ConfigValue { get; set; }
        public int MemberId { get; set; }
        public int DeptId { get; set; }
        public int ComId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public OrderConfig()
        {
            Id = 0;
            ConfigType = "";
            ConfigValue = "";
            MemberId = 0;
            DeptId = 0;
            ComId = 0;
            StartTime = Convert.ToDateTime("1900-01-01");
            EndTime = Convert.ToDateTime("1900-01-01");
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
            arrayList.Add(new SqlParameter("@ConfigType", ConfigType));
            arrayList.Add(new SqlParameter("@ConfigValue", ConfigValue));
            arrayList.Add(new SqlParameter("@MemberId", MemberId));
            arrayList.Add(new SqlParameter("@DeptId", DeptId));
            arrayList.Add(new SqlParameter("@ComId", ComId));
            arrayList.Add(new SqlParameter("@StartTime", StartTime));
            arrayList.Add(new SqlParameter("@EndTime", EndTime));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("OrderConfig", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("OrderConfig", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from OrderConfig where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                ConfigType = DBTool.GetStringFromRow(row, "ConfigType", "");
                ConfigValue = DBTool.GetStringFromRow(row, "ConfigValue", "");
                MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);
                DeptId = DBTool.GetIntFromRow(row, "DeptId", 0);
                ComId = DBTool.GetIntFromRow(row, "ComId", 0);
                StartTime = DBTool.GetDateTimeFromRow(row, "StartTime");
                EndTime = DBTool.GetDateTimeFromRow(row, "EndTime");
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from OrderConfig where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}
