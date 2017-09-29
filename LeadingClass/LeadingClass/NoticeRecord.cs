using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class NoticeRecord
    {
        public int Id { get; set; }
        public int NoticeId { get; set; }
        public int UserId { get; set; }
        public int IsHaveSee { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;
        public NoticeRecord()
        {
            m_dbo = new DBOperate();
            Id = 0;
            NoticeId = 0;
            UserId = 0;
            IsHaveSee = 0;
            UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (this.Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", this.Id));
            }
            arrayList.Add(new SqlParameter("@NoticeId", this.NoticeId));
            arrayList.Add(new SqlParameter("@UserId", this.UserId));
            arrayList.Add(new SqlParameter("@IsHaveSee", this.IsHaveSee));
            arrayList.Add(new SqlParameter("@UpdateTime", this.UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("NoticeRecord", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("NoticeRecord", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;

        }
        public bool Load()
        {
            string sql = string.Format("select * from NoticeRecord where Id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                return LoadFromRow(row);
            }
            return false;
        }
        public bool Load(int noticeId,int userId)
        {
            string sql = string.Format("select * from NoticeRecord where noticeId={0} and userId={1}", noticeId,userId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                return LoadFromRow(row);
            }
            return false;
        }
        private bool LoadFromRow(DataRow row)
        {
            Id = DBTool.GetIntFromRow(row, "Id", 0);
            NoticeId = DBTool.GetIntFromRow(row, "NoticeId", 0);
            UserId = DBTool.GetIntFromRow(row, "UserId", 0);
            IsHaveSee = DBTool.GetIntFromRow(row, "IsHaveSee", 0);
            UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
            return true;
        }

        public bool GetNoticeByUser(int userId, int noticeId)
        {
            string sql = string.Format("select * from NoticeRecord  where UserId={0} and NoticeId={1}", userId, noticeId);
            if (m_dbo.GetDataSet(sql).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
