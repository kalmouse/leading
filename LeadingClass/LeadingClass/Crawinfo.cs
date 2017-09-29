using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
namespace LeadingClass
{
    public class Crawinfo
    {
        public int Id { get; set; }
        public string BiddingName { get; set; }
        public string BiddingUrl { get; set; }
        public string Biddingcharset { get; set; }
        public string Brddingrecording { get; set; }
        public string CrawlUrl { get; set; }
        public string Crawtitle { get; set; }
        public int Istitle { get; set; }
        public string Crawarea { get; set; }
        public string CrawTime { get; set; }
        public string CrawPosition { get; set; }
        public int Crawfrequency { get; set; }
        public string Remark { get; set; }
        private DBOperate m_dbo;

        public Crawinfo()
        {
            Id = 0;
            BiddingName = "";
            BiddingUrl = "";
            Biddingcharset = "";
            Brddingrecording = "";
            CrawlUrl = "";
            Crawtitle = "";
            Istitle = 0;
            Crawarea = "";
            CrawTime = "";
            CrawPosition = "";
            Crawfrequency = 0;
            Remark = "";
            string m_ConnectionString = "";
            try
            {
                m_ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["bid"].ConnectionString;
            }
            catch
            {
                try
                {
                    m_ConnectionString = System.Configuration.ConfigurationManager.AppSettings["bid"].ToString();
                }
                catch { }
            }
            m_dbo = new DBOperate(m_ConnectionString);
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@BiddingName", BiddingName));
            arrayList.Add(new SqlParameter("@BiddingUrl", BiddingUrl));
            arrayList.Add(new SqlParameter("@Brddingrecording", Brddingrecording));
            arrayList.Add(new SqlParameter("@CrawlUrl", CrawlUrl));
            arrayList.Add(new SqlParameter("@Biddingcharset",Biddingcharset));
            arrayList.Add(new SqlParameter("@Crawtitle", Crawtitle));
            arrayList.Add(new SqlParameter("@Istitle", Istitle));
            arrayList.Add(new SqlParameter("@Crawarea", Crawarea));
            arrayList.Add(new SqlParameter("@CrawTime", CrawTime));
            arrayList.Add(new SqlParameter("@CrawPosition", CrawPosition));
            arrayList.Add(new SqlParameter("@Crawfrequency",Crawfrequency));
            arrayList.Add(new SqlParameter("@Remark", Remark));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("CrawInfo", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("CrawInfo", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from CrawInfo where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                BiddingName = DBTool.GetStringFromRow(row, "BiddingName", "");
                BiddingUrl = DBTool.GetStringFromRow(row, "BiddingUrl", "");
                Biddingcharset = DBTool.GetStringFromRow(row,"Biddingcharset","");
                Brddingrecording = DBTool.GetStringFromRow(row, "Brddingrecording", "");
                CrawlUrl = DBTool.GetStringFromRow(row, "CrawlUrl", "");
                Crawtitle = DBTool.GetStringFromRow(row, "Crawtitle", "");
                Istitle = DBTool.GetIntFromRow(row, "Istitle", 0);
                Crawarea = DBTool.GetStringFromRow(row, "Crawarea", "");
                CrawTime = DBTool.GetStringFromRow(row, "CrawTime", "");
                CrawPosition = DBTool.GetStringFromRow(row, "CrawPosition", "");
                Crawfrequency = DBTool.GetIntFromRow(row,"Crawfrequency",0);
                Remark = DBTool.GetStringFromRow(row, "Remark", "");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from CrawInfo where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        public DataSet ReadList()
        {
            string sql = string.Format("select * from CrawInfo");
            return m_dbo.GetDataSet(sql);
        }
    }
}
