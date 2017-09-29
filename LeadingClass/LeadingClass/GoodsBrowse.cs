using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class GoodsBrowse
    {
        public int Id { get; set; }
        public int GoodsId { get; set; }
        public string LoginName { get; set; }
        public string Company { get; set; }
        public int IsIntheCart { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string BrowseType { get; set; }
        public string ClientMark { get; set; }
        public string Guid { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;
        public GoodsBrowse()
        {
            Id = 0;
            GoodsId = 0;
            LoginName = "";
            Company = "";            
            IsIntheCart = 0;
            Ip = "";
            Port = "";
            Province = "";
            City = "";
            BrowseType = "";
            ClientMark = "";
            Guid = "";
            UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public GoodsBrowse(int Id)
        {
            m_dbo = new DBOperate();
            this.Id = Id;
            this.Load();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id)); 
            }
            arrayList.Add(new SqlParameter("@GoodsId", GoodsId));
            arrayList.Add(new SqlParameter("@LoginName", LoginName));
            arrayList.Add(new SqlParameter("@Company", Company));            
            arrayList.Add(new SqlParameter("@IsIntheCart", IsIntheCart));
            arrayList.Add(new SqlParameter("@Ip", Ip));
            arrayList.Add(new SqlParameter("@Port", Port));
            arrayList.Add(new SqlParameter("@Province", Province));
            arrayList.Add(new SqlParameter("@City", City));
            arrayList.Add(new SqlParameter("@BrowseType", BrowseType));
            arrayList.Add(new SqlParameter("@ClientMark", ClientMark));
            arrayList.Add(new SqlParameter("@Guid", Guid));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            this.Id = m_dbo.InsertData("GoodsBrowse", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from GoodsBrowse where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                LoginName = DBTool.GetStringFromRow(row, "LoginName", "");
                Company = DBTool.GetStringFromRow(row, "Company", "");                
                IsIntheCart = DBTool.GetIntFromRow(row, "IsIntheCart", 0);
                Ip = DBTool.GetStringFromRow(row, "Ip", "");
                Port = DBTool.GetStringFromRow(row, "Port", "");
                Province = DBTool.GetStringFromRow(row, "Province", "");
                City = DBTool.GetStringFromRow(row, "City", "");
                BrowseType = DBTool.GetStringFromRow(row, "BrowseType", "");
                ClientMark = DBTool.GetStringFromRow(row, "ClientMark", "");
                Guid = DBTool.GetStringFromRow(row, "Guid", "");
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");

                return true;
            }
            return false;
        }
    }
}
