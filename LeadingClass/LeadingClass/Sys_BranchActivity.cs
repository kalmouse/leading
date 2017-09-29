using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class Sys_BranchActivity
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public int IsAvailable { get; set; }
        public string PhotoPath { get; set; }
        public string Link { get; set; }
        public string Background { get; set; }
        private DBOperate m_dbo;

        public Sys_BranchActivity()
        {
            Id = 0;
            BranchId = 0;
            Title = "";
            Content = "";
            PhotoPath = "";
            StartDateTime = DateTime.Now.Date;
            EndDateTime = DateTime.Now.Date;
            UpdateDateTime = DateTime.Now;
            IsAvailable = 0;
            Link = "";
            Background = "";
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@BranchId", BranchId));
            arrayList.Add(new SqlParameter("@Title", Title));
            arrayList.Add(new SqlParameter("@Content", Content));
            arrayList.Add(new SqlParameter("@StartDateTime", StartDateTime));
            arrayList.Add(new SqlParameter("@EndDateTime", EndDateTime));
            arrayList.Add(new SqlParameter("@UpdateDateTime", UpdateDateTime));
            arrayList.Add(new SqlParameter("@IsAvailable", IsAvailable));
            arrayList.Add(new SqlParameter("@PhotoPath", PhotoPath));
            arrayList.Add(new SqlParameter("@Link", Link));
            arrayList.Add(new SqlParameter("@Background", Background));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Sys_BranchActivity", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Sys_BranchActivity", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Sys_BranchActivity where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                Title = DBTool.GetStringFromRow(row, "Title", "");
                Content = DBTool.GetStringFromRow(row, "Content", "");
                StartDateTime = DBTool.GetDateTimeFromRow(row, "StartDateTime");
                EndDateTime = DBTool.GetDateTimeFromRow(row, "EndDateTime");
                UpdateDateTime = DBTool.GetDateTimeFromRow(row, "UpdateDateTime");
                IsAvailable = DBTool.GetIntFromRow(row, "IsAvailable", 0);
                PhotoPath = DBTool.GetStringFromRow(row, "PhotoPath", "");
                Link = DBTool.GetStringFromRow(row, "Link", "");
                Background = DBTool.GetStringFromRow(row, "Background", "");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from Sys_BranchActivity where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        public DataSet ReadList()
        {
            string sql = string.Format("select * from Sys_BranchActivity where 1=1");
            if (BranchId > 0)
            {
                sql += " and BranchId=" + BranchId;
            }
            if (IsAvailable == 0)
            {
                sql += " and IsAvailable=0";
            }
            return m_dbo.GetDataSet(sql);
        }
        //读取前4条在活动期限内的最新的活动
        public DataSet ReadTopFour(int BranchId)
        {
            string sql = string.Format("select top 4 * from Sys_BranchActivity where BranchId={0} and StartDateTime<='{1}' and DATEADD(day,1,EndDateTime)>='{1}'  order by UpdateDateTime desc", BranchId,DateTime.Now);
            return m_dbo.GetDataSet(sql);
        }
    }
}
