using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
namespace LeadingClass
{
    public class Crawltable
    {
        public int Id { get; set; }
        public int BId { get; set; }
        public string Crawltitle { get; set; }
        public string Crawlurl { get; set; }
        public DateTime Addtime { get; set; }
        public int IsSend { get; set; }
        private DBOperate m_dbo;

        public Crawltable()
        {
            Id = 0;
            BId = 0;
            Crawltitle = "";
            Crawlurl = "";
            Addtime = DateTime.Now;
            IsSend = 0;
            m_dbo = new DBOperate("System.Configuration.ConfigurationManager.ConnectionStrings[\"url\"].ConnectionString");
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@BId", BId));
            arrayList.Add(new SqlParameter("@Crawltitle", Crawltitle));
            arrayList.Add(new SqlParameter("@Crawlurl", Crawlurl));
            arrayList.Add(new SqlParameter("@Addtime", Addtime));
            arrayList.Add(new SqlParameter("@IsSend", IsSend));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("CrawlTable", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("CrawlTable", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from CrawlTable where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                BId = DBTool.GetIntFromRow(row, "BId", 0);
                Crawltitle = DBTool.GetStringFromRow(row, "Crawltitle", "");
                Crawlurl = DBTool.GetStringFromRow(row, "Crawlurl", "");
                Addtime = DBTool.GetDateTimeFromRow(row, "Addtime");
                IsSend = DBTool.GetIntFromRow(row, "IsSend", 0);
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from CrawlTable where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}
