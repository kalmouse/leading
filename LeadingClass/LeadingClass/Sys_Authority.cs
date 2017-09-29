using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;

namespace LeadingClass
{
    public class Sys_Authority
    {
        private int m_Id;
        private string m_Name;
        private string m_GroupName;
        private string m_Memo;
        private DateTime m_UpdateTime;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public string Name { get { return m_Name; } set { m_Name = value; } }
        public string GroupName { get { return m_GroupName; } set { m_GroupName = value; } }
        public string Memo { get { return m_Memo; } set { m_Memo = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public Sys_Authority()
        {
            m_Id = 0;
            m_Name = "";
            m_GroupName = "";
            m_Memo = "";
            m_UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@Name", m_Name));
            arrayList.Add(new SqlParameter("@GroupName", m_GroupName));
            arrayList.Add(new SqlParameter("@Memo", m_Memo));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("Sys_Authority", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Sys_Authority", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Sys_Authority where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_Name = DBTool.GetStringFromRow(row, "Name", "");
                m_GroupName = DBTool.GetStringFromRow(row, "GroupName", "");
                m_Memo = DBTool.GetStringFromRow(row, "Memo", "");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        /// <summary>
        /// 读取权限
        /// </summary>
        /// <param name="roleId">如果roleId=0，读取全部权限</param>
        /// <returns></returns>
        public DataSet ReadAuthority()
        {
            string sql = "select * from sys_authority where 1=1 ";
            
            sql += " order by GroupName,Name ";
            return m_dbo.GetDataSet(sql);
        }

        //public DataSet ReadAuthorityByRoleId(int RoleId)
        public DataSet ReadAuthorityByRoleId()
        {
            //string sql = string.Format("select * from Sys_Authority where Id in(select AuthorityId from dbo.Sys_RoleAuthority where RoleId={0})",RoleId);
            //string sql = "select r.GroupName as RoleGroup,r.Name as RoleName,a.GroupName as AuthorityGroup,a.Name as AuthorityName from sys_Role r join dbo.Sys_RoleAuthority ra on r.Id=ra.RoleId join Sys_Authority a on ra.AuthorityId=a.Id";
            string sql = "select * from dbo.Sys_Role;select * from dbo.Sys_RoleAuthority ra join Sys_Authority a on ra.AuthorityId=a.Id";
            return m_dbo.GetDataSet(sql);
        }
    }
}
