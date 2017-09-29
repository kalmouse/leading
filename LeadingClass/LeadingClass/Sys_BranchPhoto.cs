using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace LeadingClass
{
    public class Sys_BranchPhoto
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int UserId { get; set; }
        public int Sort { get; set; }
        public string Type { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public Sys_BranchPhoto()
        {
            Id = 0;
            BranchId = 0;
            Name = "";
            Path = "";
            UserId = 0;
            Sort = 0;
            Type = "";
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
            arrayList.Add(new SqlParameter("@BranchId",BranchId));
            arrayList.Add(new SqlParameter("@Name", Name));
            arrayList.Add(new SqlParameter("@Path", Path));
            arrayList.Add(new SqlParameter("@UserId", UserId));
            arrayList.Add(new SqlParameter("@Sort", Sort));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@Type",Type));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Sys_BranchPhoto", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Sys_BranchPhoto", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Sys_BranchPhoto where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                BranchId = DBTool.GetIntFromRow(row,"BranchId",0);
                Name = DBTool.GetStringFromRow(row, "Name", "");
                Path = DBTool.GetStringFromRow(row, "Path", "");
                UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                Sort = DBTool.GetIntFromRow(row, "Sort", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                Type = DBTool.GetStringFromRow(row,"Type","");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from Sys_BranchPhoto where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        
    }
}
