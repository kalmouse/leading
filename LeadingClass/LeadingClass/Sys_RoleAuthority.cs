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
    public class Sys_RoleAuthority
    {
        private int m_Id;
        private int m_RoleId;
        private int m_AuthorityId;
        private DateTime m_UpdateTime;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int RoleId { get { return m_RoleId; } set { m_RoleId = value; } }
        public int AuthorityId { get { return m_AuthorityId; } set { m_AuthorityId = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public Sys_RoleAuthority()
        {
            m_Id = 0;
            m_RoleId = 0;
            m_AuthorityId = 0;
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
            arrayList.Add(new SqlParameter("@RoleId", m_RoleId));
            arrayList.Add(new SqlParameter("@AuthorityId", m_AuthorityId));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("Sys_RoleAuthority", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Sys_RoleAuthority", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Sys_RoleAuthority where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_RoleId = DBTool.GetIntFromRow(row, "RoleId", 0);
                m_AuthorityId = DBTool.GetIntFromRow(row, "AuthorityId", 0);
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }

        public DataSet RoleReadAuthority(int roleId)
        {
            string sql = string.Format(" select * from sys_RoleAuthority where RoleId ={0} ", roleId);
            return m_dbo.GetDataSet(sql);
        }
        public bool RemoveAuthority(int roleId)
        {
            string sql = string.Format(" delete from sys_RoleAuthority where RoleId={0} ", roleId);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}
