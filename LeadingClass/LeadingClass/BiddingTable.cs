using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class Biddingtable
    {
        public int Id { get; set; }
        public int Mid { get; set; }
        public string NameUrl { get; set; }
        public string Url { get; set; }
        public string UrlArea { get; set; }
        public string Remark { get; set; }
        public int Standard { get; set; }
        private DBOperate m_dbo;

        public Biddingtable()
        {
            Id = 0;
            Mid = 0;
            NameUrl = "";
            Url = "";
            UrlArea = "";
            Remark = "";
            Standard = 0;
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
            arrayList.Add(new SqlParameter("@Mid", Mid));
            arrayList.Add(new SqlParameter("@NameUrl", NameUrl));
            arrayList.Add(new SqlParameter("@Url", Url));
            arrayList.Add(new SqlParameter("@UrlArea", UrlArea));
            arrayList.Add(new SqlParameter("@Remark", Remark));
            arrayList.Add(new SqlParameter("@Standard", Standard));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("BiddingTable", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("BiddingTable", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from BiddingTable where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                Mid = DBTool.GetIntFromRow(row, "Mid", 0);
                NameUrl = DBTool.GetStringFromRow(row, "NameUrl", "");
                Url = DBTool.GetStringFromRow(row, "Url", "");
                UrlArea = DBTool.GetStringFromRow(row, "UrlArea", "");
                Remark = DBTool.GetStringFromRow(row, "Remark", "");
                Standard = DBTool.GetIntFromRow(row, "Standard", 0);
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from BiddingTable where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        public DataSet ReadList()
        {
            string sql = string.Format("select * from BiddingTable");
            return m_dbo.GetDataSet(sql);
        }
    }
}
