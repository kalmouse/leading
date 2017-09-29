using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class  Sys_RoleMenu
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int MenuId { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public Sys_RoleMenu()
        {
            Id = 0;
            RoleId = 0;
            MenuId = 0;
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
            arrayList.Add(new SqlParameter("@RoleId", RoleId));
            arrayList.Add(new SqlParameter("@MenuId", MenuId));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("Sys_RoleMenu", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Sys_RoleMenu", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Sys_RoleMenu where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                RoleId = DBTool.GetIntFromRow(row, "RoleId", 0);
                MenuId = DBTool.GetIntFromRow(row, "MenuId", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        /// <summary>
        /// 根据roleId读取行者菜单项
        /// </summary>
        /// <returns></returns>
        public DataSet ReadMenuByRoleId(int roleId)
        {
            string sql = string.Format("select * from Sys_RoleMenu where RoleId={0}", roleId);
            return m_dbo.GetDataSet(sql);
        }

        public bool DeteleMenuByRoleId(int roleId)
        {
            string sql = string.Format("delete from Sys_RoleMenu where RoleId={0} ", roleId);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 根据用户角色批量读取菜单项
        /// </summary>
        /// <returns></returns>
        public DataSet ReadMenuByRoleIds(DataTable roleDt)
        {
            string sql = "select  distinct Route,Title,Indexed,MenuId from View_UserRoleMenu where 1=1"; 
            if (roleDt.Rows.Count > 0)
            {
                sql += "and RoleId in(";
                for (int i = 0; i < roleDt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format("'{0}'", roleDt.Rows[i]["RoleId"]);
                    }
                    else
                    {
                        sql += string.Format(" ,'{0}' ", roleDt.Rows[i]["RoleId"]);
                    }
                }
            }
            sql += ")";
            sql += "Order by Indexed";
            return m_dbo.GetDataSet(sql);
        }
    }
}
