using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class TPI_ProjectOption
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int BranchId { get; set; }
        public int IsUseTypeCompare { get; set; }
        public string Developers { get; set; }
        public string AppKey { get; set; }
        public string AppPassword { get; set; }
        public string AppSecret { get; set; }
        public string Token { get; set; }
        public DateTime CallDate { get; set; }
        public DateTime EndDate { get; set; }
        public int State { get; set; }
        public string interfaceVersion { get; set; }
        public string Sign { get; set; }
    }
    public class TPI_ProjectManager
    {
        private DBOperate m_dbo;
        public TPI_ProjectOption Option { get; set; }
        public TPI_ProjectManager()
        {
            m_dbo = new DBOperate();
            Option = new TPI_ProjectOption();
        }
        /// <summary>
        /// 查询某项目的账号密码和开关设置等。
        /// </summary>
        /// <returns></returns>
        public DataTable GetTPI_Users()
        {
            string sql = "select tp.Id as ProjectId,tp.Name as ProjectName,BranchId,IsUseTypeCompare,Developers,AppKey,AppPassword,AppSecret,Token,CallDate,EndDate,State,InterfaceVersion,Sign from TPI_Users tu join TPI_Project tp on tu.ProjectId=tp.Id where State=1";
            if (Option.AppKey != "" && Option.AppKey != null)
            {
                sql += string.Format("and appKey ='{0}'", Option.AppKey);
            }
            if (Option.AppPassword != "" && Option.AppPassword != null)
            {
                sql += string.Format("and AppPassword ='{0}'", Option.AppPassword);
            }
            if (Option.interfaceVersion != "" && Option.interfaceVersion != null)
            {
                sql += string.Format("and InterfaceVersion like '%{0}%'", Option.interfaceVersion);
            }
            if (Option.AppSecret != "" && Option.AppSecret != null)
            {
                sql += string.Format("and AppSecret ='{0}'", Option.AppSecret);
            }
            if (Option.ProjectId > 0)
            {
                sql += string.Format("and ProjectId ={0}", Option.ProjectId);
            }
            if (Option.Sign != "" && Option.Sign != null)
            {
                sql += string.Format("and Sign='{0}'", Option.Sign);
            }
            if (Option.Token != "" && Option.Token != null)
            {
                sql += string.Format("and Token='{0}'", Option.Token);
            }
            DataTable dt = m_dbo.GetDataSet(sql).Tables[0];
            return dt;
        }
        /// <summary>tpi_project
        /// 验证某项目的账号密码和开关设置等。
        /// </summary>
        /// <returns></returns>
        public bool LoadTPI_Users()
        {
            DataTable dt = GetTPI_Users();
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                Option.ProjectId = DBTool.GetIntFromRow(row, "ProjectId", 0);
                Option.ProjectName = DBTool.GetStringFromRow(row, "ProjectName", "");
                Option.BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                Option.IsUseTypeCompare = DBTool.GetIntFromRow(row, "IsUseTypeCompare", 0);
                Option.Developers = DBTool.GetStringFromRow(row, "Developers", "");
                Option.AppKey = DBTool.GetStringFromRow(row, "AppKey", "");
                Option.AppPassword = DBTool.GetStringFromRow(row, "AppPassword", "");
                Option.AppSecret = DBTool.GetStringFromRow(row, "AppSecret", "");
                Option.Token = DBTool.GetStringFromRow(row, "Token", "");
                Option.CallDate = DBTool.GetDateTimeFromRow(row, "CallDate");
                Option.EndDate = DBTool.GetDateTimeFromRow(row, "EndDate");
                Option.State = DBTool.GetIntFromRow(row, "State", 0);
                Option.interfaceVersion = DBTool.GetStringFromRow(row, "InterfaceVersion", "");
                Option.Sign = DBTool.GetStringFromRow(row, "Sign", "");
                return true;
            }
            return false;
        }
    }
}
