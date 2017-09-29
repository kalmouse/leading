using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class Notice
    {
        public int Id { get; set; }
        public string ReleaseName { get; set; }
        public string MessageContent { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;
        public Notice()
        {
            m_dbo = new DBOperate();
            Id = 0;
            ReleaseName = "";
            MessageContent = "";
            UpdateTime = DateTime.Now;
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (this.Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", this.Id));
            }
            arrayList.Add(new SqlParameter("@ReleaseName", this.ReleaseName));
            arrayList.Add(new SqlParameter("@MessageContent", this.MessageContent));
            arrayList.Add(new SqlParameter("@UpdateTime", this.UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("Notice", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Notice", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;

        }
        public bool Load()
        {
            string sql = string.Format("select * from Notice where Id={0}", Id);
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
            ReleaseName = DBTool.GetStringFromRow(row, "ReleaseName", "");
            MessageContent = DBTool.GetStringFromRow(row, "MessageContent", "");
            UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
            return true;
        }
        /// <summary>
        /// 读取通知列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet GetNoticeList()
        {
            string sql = string.Format("select * from Notice");
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 没读过的通知行数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetNewMessageCount(int userId)
        {
            string sql = string.Format("select COUNT(*) Id from Notice");
            int count = Convert.ToInt32(m_dbo.ExecuteScalar(sql));
            string sqls = string.Format("select COUNT(*) Id from NoticeRecord  where UserId={0}",userId);
            int counts = Convert.ToInt32(m_dbo.ExecuteScalar(sqls));
            return count - counts;
        }
    }
}
