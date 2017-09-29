using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class ExportRecord
    {
        public int Id { get; set; }
        public string RealName { get; set; }
        public int OrderRows { get; set; }
        public double SumMoney { get; set; }
        public DateTime UpdateTime { get; set; }
        public int Count { get; set; }
        public int RecordId { get; set; }
        private DBOperate m_dbo;

        public ExportRecord()
        {
            Id = 0;
            RealName = "";
            OrderRows = 0;
            SumMoney = 0;
            UpdateTime = DateTime.Now;
            Count = 0;
            RecordId = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@RealName", RealName));
            arrayList.Add(new SqlParameter("@OrderRows", OrderRows));
            arrayList.Add(new SqlParameter("@SumMoney", SumMoney));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@Count", Count));
            arrayList.Add(new SqlParameter("@RecordId", RecordId));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("ExportRecord", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("ExportRecord", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from ExportRecord where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                RealName = DBTool.GetStringFromRow(row, "RealName", "");
                OrderRows = DBTool.GetIntFromRow(row, "OrderRows", 0);
                SumMoney = DBTool.GetDoubleFromRow(row, "SumMoney", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                Count = DBTool.GetIntFromRow(row, "Count", 0);
                RecordId = DBTool.GetIntFromRow(row, "RecordId", 0);
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from ExportRecord where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 读取当前批量下单的下载记录
        /// </summary>
        /// <returns></returns>
        public DataSet ReadOrderExportRecord()
        {
            string sql = "select top 20 * from dbo.ExportRecord order by UpdateTime desc";
            return m_dbo.GetDataSet(sql);
        }
    }
}
