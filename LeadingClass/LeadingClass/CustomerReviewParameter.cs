using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;


namespace LeadingClass
{
    public class CustomerReviewParameter
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ReviewDay { get; set; }
        public DateTime UpdateTime { get; set; }
        public int UpdatePerson { get; set; }
        private DBOperate m_dbo;

        public CustomerReviewParameter()
        {
            Id = 0;
            UserId = 0;
            ReviewDay = 0;
            UpdateTime = DateTime.Now;
            UpdatePerson = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@UserId", UserId));
            arrayList.Add(new SqlParameter("@ReviewDay", ReviewDay));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@UpdatePerson", UpdatePerson));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("CustomerReviewParameter", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("CustomerReviewParameter", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from CustomerReviewParameter where UserId={0}", UserId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                ReviewDay = DBTool.GetIntFromRow(row, "ReviewDay", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                UpdatePerson = DBTool.GetIntFromRow(row, "UpdatePerson", 0);
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from CustomerReviewParameter where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}
