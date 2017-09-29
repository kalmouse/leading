using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
namespace LeadingClass
{
    public class Sys_Role
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
        public Sys_Role()
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
                m_dbo.UpdateData("Sys_Role", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Sys_Role", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Sys_Role where id={0}", m_Id);
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
        public bool Delete()
        {
            string sql = string.Format(" delete from sys_role where id={0} ", this.Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        public DataSet ReadRoles()
        {
            string sql = "select * from sys_Role order by GroupName,Name ";
            return m_dbo.GetDataSet(sql);
        }
        public DataSet ReadUsers(int branchId, int roleId)
        {
            string sql = string.Format(@"select su.DeptId, sd.Name as deptname,su.Id, su.LoginName,su.Name,su.BranchId,su.IsSales 
from Sys_Dept sd left outer join  Sys_Users su on sd.Id=su.DeptId  inner join Sys_UserRole sur on  su.Id = sur.UserId 
where sur.RoleId ={0} and su.BranchId={1} order by sd.Name ", roleId, branchId);
            return m_dbo.GetDataSet(sql);
        }
    }
}
