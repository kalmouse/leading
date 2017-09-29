using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class Sys_Menu
    {
        public int Id { get; set; }
        public string Route { get; set; }
        public string Title { get; set; }
        public string GroupName { get; set; }
        public string Memo { get; set; }
        public int Indexed { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public Sys_Menu()
        {
            Id = 0;
            Route = "";
            Title = "";
            GroupName = "";
            Memo = "";
            Indexed = 1;            
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
            arrayList.Add(new SqlParameter("@Route", Route));
            arrayList.Add(new SqlParameter("@Title", Title));
            arrayList.Add(new SqlParameter("@GroupName", GroupName));
            arrayList.Add(new SqlParameter("@Memo", Memo));
            arrayList.Add(new SqlParameter("@Indexed", Indexed));            
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("Sys_Menu", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Sys_Menu", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Sys_Menu where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                Route = DBTool.GetStringFromRow(row, "Route", "");
                Title = DBTool.GetStringFromRow(row, "Title", "");
                GroupName = DBTool.GetStringFromRow(row, "GroupName", "");
                Memo = DBTool.GetStringFromRow(row, "Memo", "");
                Indexed = DBTool.GetIntFromRow(row, "Indexed", 0);                
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        /// <summary>
        /// 读取行者所有菜单项
        /// </summary>
        /// <returns></returns>
        public DataSet ReadMenu()
        {
            string sql = "select  distinct Route,Id,Title,GroupId,Memo,Indexed from Sys_Menu order by Indexed";
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 根据分组名称读取分组内菜单
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public DataSet ReadMenuByGroupName(string groupName)
        {
            string sql = string.Format("select * from Sys_Menu where groupName='{0}'", groupName);
            return m_dbo.GetDataSet(sql);
        }
    }
}

