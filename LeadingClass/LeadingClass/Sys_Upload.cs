using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace LeadingClass
{
    public class Sys_Upload
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; }
        public string ImagePath { get; set; }
        public string Coutent { get; set; }
        public DateTime UpdateTime { get; set; }
        public string BranchId { get; set; }
        public string VideoPath { get; set; }
        private DBOperate m_dbo;

        public Sys_Upload()
        {
            Id = 0;
            Titel = "";
            UserId = 0;
            Type = "";
            ImagePath = "";
            Coutent = "";
            UpdateTime = DateTime.Now;
            BranchId = "";
            VideoPath = "";
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@Titel", Titel));
            arrayList.Add(new SqlParameter("@UserId", UserId));
            arrayList.Add(new SqlParameter("@Type", Type));
            arrayList.Add(new SqlParameter("@ImagePath", ImagePath));
            arrayList.Add(new SqlParameter("@Coutent", Coutent));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@BranchId", BranchId));
            arrayList.Add(new SqlParameter("@VideoPath", VideoPath));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Sys_UpLoad", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Sys_UpLoad", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Sys_UpLoad where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                Titel = DBTool.GetStringFromRow(row, "Titel", "");
                UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                Type = DBTool.GetStringFromRow(row, "Type", "");
                ImagePath = DBTool.GetStringFromRow(row, "ImagePath", "");
                Coutent = DBTool.GetStringFromRow(row, "Coutent", "");
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                BranchId = DBTool.GetStringFromRow(row, "BranchId", "");
                VideoPath = DBTool.GetStringFromRow(row, "VideoPath","");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from Sys_UpLoad where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        public DataSet ReadFile(string Imagepath)
        {
            string  sql = string.Format("select * from Sys_UpLoad where Imagepath='{0}'", Imagepath);
            return m_dbo.GetDataSet(sql);
        }
    }
}
