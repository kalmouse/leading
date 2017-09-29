using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class TPI_OperationLog
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Url { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Operation { get; set; }
        public string PushState { get; set; }
        public string ReturnMsg { get; set; }
        public string ObjectId { get; set; }
        public int UserId { get; set; }
        private DBOperate m_dbo;

        public TPI_OperationLog()
        {
            Id = 0;
            ProjectId = 0;
            Url = "";
            UpdateTime = DateTime.Now;
            Operation = "";
            PushState = "";
            ReturnMsg = "";
            ObjectId = "";
            UserId = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@ProjectId", ProjectId));
            arrayList.Add(new SqlParameter("@Url", Url));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@Operation", Operation));
            arrayList.Add(new SqlParameter("@PushState", PushState));
            arrayList.Add(new SqlParameter("@ReturnMsg", ReturnMsg));
            arrayList.Add(new SqlParameter("@ObjectId", ObjectId));
            arrayList.Add(new SqlParameter("@UserId", UserId));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("TPI_OperationLog", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("TPI_OperationLog", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from TPI_OperationLog where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                ProjectId = DBTool.GetIntFromRow(row, "ProjectId", 0);
                Url = DBTool.GetStringFromRow(row, "Url", "");
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                Operation = DBTool.GetStringFromRow(row, "Operation", "");
                PushState = DBTool.GetStringFromRow(row, "PushState", "");
                ReturnMsg = DBTool.GetStringFromRow(row, "ReturnMsg", "");
                ObjectId = DBTool.GetStringFromRow(row, "ObjectId", "");
                UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from TPI_OperationLog where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}
