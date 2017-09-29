using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class Pointdetail
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string Type { get; set; }
        public int RelationId { get; set; }
        public double Income { get; set; }
        public double Spend { get; set; }
        public double Balance { get; set; }
        public string Memo { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public Pointdetail()
        {
            Id = 0;
            MemberId = 0;
            Type = "";
            RelationId = 0;
            Income = 0;
            Spend = 0;
            Balance = 0;
            Memo = "";
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
            arrayList.Add(new SqlParameter("@MemberId", MemberId));
            arrayList.Add(new SqlParameter("@Type", Type));
            arrayList.Add(new SqlParameter("@RelationId", RelationId));
            arrayList.Add(new SqlParameter("@Income", Income));
            arrayList.Add(new SqlParameter("@Spend", Spend));
            arrayList.Add(new SqlParameter("@Balance", Balance));
            arrayList.Add(new SqlParameter("@Memo", Memo));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("PointDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("PointDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from PointDetail where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);
                Type = DBTool.GetStringFromRow(row, "Type", "");
                RelationId = DBTool.GetIntFromRow(row, "RelationId", 0);
                Income = DBTool.GetDoubleFromRow(row, "Income", 0);
                Spend = DBTool.GetDoubleFromRow(row, "Spend", 0);
                Balance = DBTool.GetDoubleFromRow(row, "Balance", 0);
                Memo = DBTool.GetStringFromRow(row, "Memo", "");
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from PointDetail where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        public DataSet ReadByRelationId(int RelationId)
        {
            string sql = string.Format("select * from PointDetail  where RelationId={0}",RelationId);
            return m_dbo.GetDataSet(sql);
        }
    }
}
