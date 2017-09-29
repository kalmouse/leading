using System.Data;

namespace LeadingClass
{
    public class AccessControl
    {
        /// <summary>
        /// 判断行者用户权限 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool CheckRoleAuthority(int userId, string controllerName, string actionName)
        {
            Sys_UserRole role = new Sys_UserRole();

            bool result = false;

            DataSet roleDs = role.ReadUserRole(userId); //读取用户角色(一对多)

            if (roleDs.Tables[0].Rows.Count > 0)
            {
                Sys_RoleMenu menu = new Sys_RoleMenu();

                DataSet menuDs = menu.ReadMenuByRoleIds(roleDs.Tables[0]);//读取角色对应显示的菜单(多对多)

                if (menuDs.Tables[0].Rows.Count > 0)
                {
                    string route = CreateRoute(controllerName, actionName);
                    for (int i = 0; i < menuDs.Tables[0].Rows.Count; i++)
                    {
                        if (route == menuDs.Tables[0].Rows[i]["Route"].ToString())
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 构造Route路径
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        private static string CreateRoute(string controllerName, string actionName)
        {
            if (controllerName != "" && actionName != "")
            {
                return "/" + controllerName + "/" + actionName;
            }
            return "";
        }
    }
}
