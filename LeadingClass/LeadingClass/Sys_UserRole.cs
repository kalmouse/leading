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
    public class Sys_UserRole
    {
        private int m_Id;
        private int m_UserId;
        private int m_RoleId;
        private DateTime m_UpdateTime;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public int RoleId { get { return m_RoleId; } set { m_RoleId = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public Sys_UserRole()
        {
            m_Id = 0;
            m_UserId = 0;
            m_RoleId = 0;
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
            arrayList.Add(new SqlParameter("@UserId", m_UserId));
            arrayList.Add(new SqlParameter("@RoleId", m_RoleId));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("Sys_UserRole", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Sys_UserRole", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Sys_UserRole where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_RoleId = DBTool.GetIntFromRow(row, "RoleId", 0);
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        /// <summary>
        /// 读取用户的角色
        /// ERP:FGoods.cs
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet ReadUserRole(int userId)
        {
            string sql = string.Format(" select * from sys_userrole where UserId={0} ", userId);
            return m_dbo.GetDataSet(sql);
        }
        public bool RemoveUserRole(int userId)
        {
            string sql = string.Format(" delete  from sys_userrole where UserId={0} ", userId);
            return m_dbo.ExecuteNonQuery(sql);
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
