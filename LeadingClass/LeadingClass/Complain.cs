using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class Complain
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Telphone { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime UpdateTime { get; set; }
        public string ImageMethod { get; set; }
        private DBOperate m_dbo;

        public Complain()
        {
            Id = 0;
            Content = "";
            Telphone = "";
            Email = "";
            Name = "";
            UpdateTime = DateTime.Now;
            ImageMethod = "";
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@Content", Content));
            arrayList.Add(new SqlParameter("@Telphone", Telphone));
            arrayList.Add(new SqlParameter("@Email", Email));
            arrayList.Add(new SqlParameter("@Name", Name));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@ImageMethod", ImageMethod));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Complain", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Complain", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Complain where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                Content = DBTool.GetStringFromRow(row, "Content", "");
                Telphone = DBTool.GetStringFromRow(row, "Telphone", "");
                Email = DBTool.GetStringFromRow(row, "Email", "");
                Name = DBTool.GetStringFromRow(row, "Name", "");
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                ImageMethod = DBTool.GetStringFromRow(row, "ImageMethod", "");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from Complain where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}