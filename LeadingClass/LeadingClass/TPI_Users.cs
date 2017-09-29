using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class TPI_Users
    {
        public int Id { get; set; }
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        public string Sign { get; set; }
        public string InterfaceVersion { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public int State { get; set; }
        public DateTime CallDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProjectId { get; set; }
        public string AppPassword { get; set; }
        public string ClientSecret { get; set; }

        private DBOperate m_dbo;
        public TPI_Users()
        {
            Id = 0;
            AppKey = "";
            AppSecret = "";
            Sign = "";
            InterfaceVersion = "";
            Name = "";
            Token = "";
            State = 0;
            CallDate = DateTime.Now;
            EndDate = DateTime.Now;
            ProjectId = 0;
            AppPassword = "";
            ClientSecret = "";
            m_dbo = new DBOperate();
        }

        /// <summary>
        /// Load方法：参数值：用户名/密钥/接口版本
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            string sql = "select * from TPI_Users where State=1 ";
            if (AppKey != null && AppKey != "")
            {
                sql += string.Format("and AppKey='{0}'", AppKey);
            }
            if (AppPassword != null && AppPassword != "")
            {
                sql += string.Format("and AppPassword='{0}'", AppPassword);
            }
            if (Token != null && Token != "")
            {
                sql += string.Format("and Token ='{0}'", Token);
            }
            if (InterfaceVersion != null && InterfaceVersion != "")
            {
                sql += string.Format("and InterfaceVersion like '%{0}%'", InterfaceVersion);
            }
            if (Sign != null && Sign != "")
            {
                sql += string.Format("and Sign='{0}'", Sign);
            }
            if (ProjectId >0)
            {
                sql += string.Format("and ProjectId={0}", ProjectId);
            }
            if (AppSecret != null && AppSecret != "")
            {
                sql += string.Format("and AppSecret='{0}'", AppSecret);
            }
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                AppKey = DBTool.GetStringFromRow(row, "AppKey", "");
                AppSecret = DBTool.GetStringFromRow(row, "AppSecret", "");
                Sign = DBTool.GetStringFromRow(row, "Sign", "");
                InterfaceVersion = DBTool.GetStringFromRow(row, "InterfaceVersion", "");
                Name = DBTool.GetStringFromRow(row, "Name", "");
                Token = DBTool.GetStringFromRow(row, "Token", "");
                State = DBTool.GetIntFromRow(row, "State", 0);
                CallDate = DBTool.GetDateTimeFromRow(row, "CallDate");
                EndDate = DBTool.GetDateTimeFromRow(row, "EndDate");
                ProjectId = DBTool.GetIntFromRow(row, "ProjectId", 0);
                AppPassword = DBTool.GetStringFromRow(row, "AppPassword", "");
                ClientSecret = DBTool.GetStringFromRow(row, "ClientSecret", "");
                return true;
            }
            return false;
        }
        public int Save()
            {

            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
        }
            arrayList.Add(new SqlParameter("@AppKey", AppKey));
            arrayList.Add(new SqlParameter("@AppSecret", AppSecret));
            arrayList.Add(new SqlParameter("@Sign", Sign));
            arrayList.Add(new SqlParameter("@InterfaceVersion", InterfaceVersion));
            arrayList.Add(new SqlParameter("@Name", Name));
            arrayList.Add(new SqlParameter("@Token", Token));
            arrayList.Add(new SqlParameter("@CallDate", CallDate));
            arrayList.Add(new SqlParameter("@EndDate", EndDate));
            arrayList.Add(new SqlParameter("@State", State));
            arrayList.Add(new SqlParameter("@ProjectId", ProjectId));
            arrayList.Add(new SqlParameter("@AppPassword", AppPassword));
            arrayList.Add(new SqlParameter("@ClientSecret", ClientSecret));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("TPI_Users", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
    }
}
