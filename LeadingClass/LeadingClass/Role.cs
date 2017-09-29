using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.SqlClient;

namespace LeadingClass
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Memo { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public Role()
        {
            Id = 0;
            Name = "";
            Memo = "";
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
            arrayList.Add(new SqlParameter("@Name", Name));
            arrayList.Add(new SqlParameter("@Memo", Memo));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Role", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Role", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Role where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                Name = DBTool.GetStringFromRow(row, "Name", "");
                Memo = DBTool.GetStringFromRow(row, "Memo", "");
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from Role where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }

    public class MemberRole
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int RoleId { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public MemberRole()
        {
            Id = 0;
            MemberId = 0;
            RoleId = 0;
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
            arrayList.Add(new SqlParameter("@MemberId", MemberId));
            arrayList.Add(new SqlParameter("@RoleId", RoleId));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("MemberRole", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("MemberRole", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from MemberRole where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);
                RoleId = DBTool.GetIntFromRow(row, "RoleId", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from MemberRole where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 找到本公司的系统管理员
        /// </summary>
        /// <param name="comId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public DataSet GetSysAdmin(int comId)
        {
            string sql = "select MemberId from View_MemberRole where RoleId=1";
            if (comId > 0)
            {
                sql += string.Format(" and ComId={0}",comId);
            }
            return m_dbo.GetDataSet(sql);
        }
    }

    public class RoleManager
    {
        private DBOperate m_dbo;

        public RoleManager()
        {
            m_dbo = new DBOperate();
        }

        /// <summary>
        /// 读取所有的角色列表
        /// </summary>
        /// <returns></returns>
        public DataSet ReadRoles()
        {
            string sql = " select * from Role order by Id ";
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取会员的角色列表
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public DataSet ReadMemberRoles(int memberId)
        {
            string sql = string.Format(" select * from view_MemberRole where MemberId={0} order by RoleId ",memberId );
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="roleIds"></param>
        /// <returns>成更新的条数</returns>
        public int UpdateMemberRole(int memberId, List<int> roleIds)
        {
            int result =0;
            string sql = string.Format(" delete from MemberRole where MemberId={0} ",memberId);
            if(m_dbo.ExecuteNonQuery(sql))
            {
                foreach(int roleId in roleIds)
                {

                    MemberRole mr = new MemberRole();
                    mr.MemberId = memberId;
                    mr.RoleId  = roleId;
                    if(mr.Save() >0)
                    {
                        result += 1;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 用户是否对应传入角色的权限 quxiaoshan 2015-6-18
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        //public Dictionary<int, bool> ContainRolesByRolesId(int memberId, int[] roleIds) 
        //{
        //    //查询该用户对应的角色
        //    RoleManager roleManager = new RoleManager();
        //    DataSet roleDataSet = roleManager.ReadMemberRoles(memberId);
        //    Dictionary<int, bool> roleDic = new Dictionary<int, bool>();
        //    if (roleDataSet.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (var roleid in roleIds)
        //        {
        //            DataRow[] role = roleDataSet.Tables[0].Select("RoleId="+roleid);
        //            if (role.Count()>0)
        //            {
        //                if (!roleDic.ContainsKey(roleid))
        //                {
        //                    roleDic.Add(roleid, true);
        //                }
        //            }
        //            else 
        //            {
        //                roleDic.Add(roleid,false);
        //            }
        //        }
        //    }
        //    return roleDic;
        //}
    }
}
