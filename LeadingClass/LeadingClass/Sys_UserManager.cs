using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;

namespace LeadingClass
{
    public class sys_UserOption
    {
        public int BranchId { get; set; }
        public int DeptId { get; set; }
        public string Code { get; set; }
        public int RoleId { get; set; }
        public int IsSales{get;set;}
        public string Name { get; set; }
    }

    public class Sys_UserManager
    {
        private DBOperate m_dbo;
        public Sys_UserManager()
        {
            m_dbo = new DBOperate();
        }

         /// <summary>
        /// 调用ERP：FOrderAlter.cs 、FSalesAmount.cs、FSalesReport.cs 、FSearchOrder.cs、FPurchase.cs
        /// 视图：用到Sys_Dept（合作伙伴部门）Sys_Users（系统用户表）Sys_Branch（合作伙伴主表）
         /// </summary>
         /// <param name="option"></param>
         /// <returns></returns>
        /// <summary>
        /// FWSearchOrders.cs 中调用
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadSysUsers(sys_UserOption option)
        {
            if (option.RoleId > 0)//根据角色选择
            {
                Sys_Role role = new Sys_Role();
                return role.ReadUsers(option.BranchId, option.RoleId);
            }
            else//根据其他条件选择
            {
                string sql = "select  * from View_SysUsers where 1=1";
                if (option.BranchId>0)
                {
                    sql += string.Format(" and BranchId={0}", option.BranchId);
                }
                if (option.DeptId > 0)
                {
                    sql += string.Format(" and DeptId={0} ", option.DeptId);
                }
                if (option.Code != "" && option.Code != null)
                {
                    sql += string.Format(" and Code like '{0}%' ", option.Code);
                }
                if (option.IsSales > 0)
                {
                    sql += string.Format(" and IsSales={0} ", option.IsSales);
                }
                if (option.Name != "" && option.Name != null)
                {
                    sql += string.Format(" and Name like '%{0}%'", option.Name);
                }
                sql += " order by Code,Name ";
                return m_dbo.GetDataSet(sql);
            }
        }
    }
}
