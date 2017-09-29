using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{

    public class VIPSiteInfo
    {
        public int Id { get; set; }
        public int ComId { get; set; }
        public string VIPUrl { get; set; }
        public string LogoUrl { get; set; }
        public string LoginImgUrl { get; set; }
        public int IsEnable { get; set; }
        public int IsMultLang { get; set; }//是否多语言站点
        public DateTime UpdateTime { get; set; }
        public string SystemName { get; set; }
        public string SystemEnName { get; set; }
        public string CounterIcon { get; set; }
        private DBOperate m_dbo;

        public VIPSiteInfo()
        {
            Id = 0;
            ComId = 0;
            VIPUrl = "";
            LogoUrl = "";
            LoginImgUrl = "";
            IsEnable = 0;
            IsMultLang = 0;
            UpdateTime = DateTime.Now;
            SystemName = "";
            SystemEnName = "";
            CounterIcon = "";
            m_dbo = new DBOperate();
        }
        public VIPSiteInfo(int Id)
            : this()
        {
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
            arrayList.Add(new SqlParameter("@ComId", ComId));
            arrayList.Add(new SqlParameter("@VIPUrl", VIPUrl));
            arrayList.Add(new SqlParameter("@LogoUrl", LogoUrl));
            arrayList.Add(new SqlParameter("@LoginImgUrl", LoginImgUrl));
            arrayList.Add(new SqlParameter("@IsEnable", IsEnable));
            arrayList.Add(new SqlParameter("@IsMultLang", IsMultLang));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@SystemName", SystemName));
            arrayList.Add(new SqlParameter("@SystemEnName", SystemEnName));
            arrayList.Add(new SqlParameter("@CounterIcon", CounterIcon));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("VIPSiteInfo", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("VIPSiteInfo", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from VIPSiteInfo where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }
        public bool LoadFromUrl(string VIPUrl)
        {
            string sql = string.Format("select top 1 * from VIPSiteInfo where VIPUrl='{0}' and IsEnable=1", VIPUrl);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count ==1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }
        public bool LoadFromComId(int comId)
        {
            string sql = string.Format("select * from VIPSiteInfo where comid={0} ", comId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }
        private void LoadFromRow(DataRow row)
        {
            Id = DBTool.GetIntFromRow(row, "Id", 0);
            ComId = DBTool.GetIntFromRow(row, "ComId", 0);
            VIPUrl = DBTool.GetStringFromRow(row, "VIPUrl", "");
            LogoUrl = DBTool.GetStringFromRow(row, "LogoUrl", "");
            LoginImgUrl = DBTool.GetStringFromRow(row, "LoginImgUrl", "");
            IsEnable = DBTool.GetIntFromRow(row, "IsEnable", 0);
            IsMultLang = DBTool.GetIntFromRow(row, "IsMultLang", 0);
            UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
            SystemName = DBTool.GetStringFromRow(row, "SystemName", "");
            SystemEnName = DBTool.GetStringFromRow(row, "SystemEnName", "");
            CounterIcon = DBTool.GetStringFromRow(row, "CounterIcon", "");

        }
        public bool Delete()
        {
            string sql = string.Format("Delete from VIPSiteInfo where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}
