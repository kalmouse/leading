using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using CommenClass;

using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;
namespace LeadingClass
{
    public class Sys_Users
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string PassWord { get; set; }
        public string Name { get; set; }
        public int BranchId { get; set; }
        public int DeptId { get; set; }
        public int IsSales { get; set; }
        public string Telphone { get; set; }
        public string Mobile { get; set; }
        public DateTime BirthDay { get; set; }
        public int IsYinLi { get; set; }
        public DateTime JoinDate { get; set; }
        public string IDCard { get; set; }
        public DateTime ModifyTime { get; set; }
        public int IsValid { get; set; }
        public string Position { get; set; }
        public string SN1 { get; set; }
        public string SN2 { get; set; }
        public int IsAdmin { get; set; }
        public string token { get; set; }
        public DateTime tokenEndDate { get; set; } 
        private DBOperate m_dbo;

        public Sys_Users()
        {
            Id = 0;
            LoginName = "";
            PassWord = "";
            Name = "";
            BranchId = 0;
            DeptId = 0;
            IsSales = 0;
            Telphone = "";
            Mobile = "";
            BirthDay = DateTime.Now;
            IsYinLi = 0;
            JoinDate = DateTime.Now;
            IDCard = "";
            ModifyTime = DateTime.Now;
            IsValid = 0;
            Position = "";
            SN1 = "";
            SN2 = "";
            IsAdmin = 0;
            token = "";
            tokenEndDate = new DateTime(1900, 1, 1);     
            m_dbo = new DBOperate();
        }
        public Sys_Users(int Id)
            : this()
        {
            this.Id = Id;
            this.Load();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@LoginName", LoginName));
            arrayList.Add(new SqlParameter("@PassWord", PassWord));
            arrayList.Add(new SqlParameter("@Name", Name));
            arrayList.Add(new SqlParameter("@BranchId", BranchId));
            arrayList.Add(new SqlParameter("@DeptId", DeptId));
            arrayList.Add(new SqlParameter("@IsSales", IsSales));
            arrayList.Add(new SqlParameter("@Telphone", Telphone));
            arrayList.Add(new SqlParameter("@Mobile", Mobile));
            arrayList.Add(new SqlParameter("@BirthDay", BirthDay));
            arrayList.Add(new SqlParameter("@IsYinLi", IsYinLi));
            arrayList.Add(new SqlParameter("@JoinDate", JoinDate));
            arrayList.Add(new SqlParameter("@IDCard", IDCard));
            arrayList.Add(new SqlParameter("@ModifyTime", ModifyTime));
            arrayList.Add(new SqlParameter("@IsValid", IsValid));
            arrayList.Add(new SqlParameter("@Position", Position));
            arrayList.Add(new SqlParameter("@SN1", SN1));
            arrayList.Add(new SqlParameter("@SN2", SN2));
            arrayList.Add(new SqlParameter("@IsAdmin", IsAdmin));
            arrayList.Add(new SqlParameter("@token", token));
            arrayList.Add(new SqlParameter("@tokenEndDate", tokenEndDate));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Sys_Users", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Sys_Users", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        /// <summary>
        /// ERP  FSysUserInfo.cs 中调用
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            string sql = string.Format("select * from Sys_Users inner join Sys_Branch on Sys_Users.BranchId = Sys_Branch.Id where Sys_Users.Id={0} ", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                LoginName = DBTool.GetStringFromRow(row, "LoginName", "");
                PassWord = DBTool.GetStringFromRow(row, "PassWord", "");
                Name = DBTool.GetStringFromRow(row, "Name", "");
                BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                DeptId = DBTool.GetIntFromRow(row, "DeptId", 0);
                IsSales = DBTool.GetIntFromRow(row, "IsSales", 0);
                Telphone = DBTool.GetStringFromRow(row, "Telphone", "");
                Mobile = DBTool.GetStringFromRow(row, "Mobile", "");
                BirthDay = DBTool.GetDateTimeFromRow(row, "BirthDay");
                IsYinLi = DBTool.GetIntFromRow(row, "IsYinLi", 0);
                JoinDate = DBTool.GetDateTimeFromRow(row, "JoinDate");
                IDCard = DBTool.GetStringFromRow(row, "IDCard", "");
                ModifyTime = DBTool.GetDateTimeFromRow(row, "ModifyTime");
                IsValid = DBTool.GetIntFromRow(row, "IsValid", 0);
                Position = DBTool.GetStringFromRow(row, "Position", "");
                SN1 = DBTool.GetStringFromRow(row, "SN1", "");
                SN2 = DBTool.GetStringFromRow(row, "SN2", "");
                IsAdmin = DBTool.GetIntFromRow(row, "IsAdmin", 0);
                token = DBTool.GetStringFromRow(row, "token", "");
                tokenEndDate = DBTool.GetDateTimeFromRow(row, "tokenEndDate");               
                return true;
            }
            return false;
        }
        
        public bool Delete()
        {
            string sql = string.Format("Delete from Sys_Users where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        } 
 
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public UserInfo IsLogin(string loginName, string passWord)
        {
            UserInfo info = new UserInfo();
            string sql = string.Format(" select * from view_sysusers where loginName='{0}' and IsValid = 1 ", loginName);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count ==1)
            {
                if (MD5.GetMD5(passWord) == DBTool.GetStringFromRow(ds.Tables[0].Rows[0], "PassWord", ""))
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    LoadUserInfoFromRow(info, row);
                    this.Id = info.Id;
                    this.Load();
                    if (this.tokenEndDate < DateTime.Now)
                    {
                        this.token = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
                        this.tokenEndDate = DateTime.Now.AddMonths(1);
                        if (this.Save() > 0)
                        {
                            info.token = this.token;
                            info.tokenEndDate = this.tokenEndDate;
                        }
                    }                   
                    return info;
                }
                else return null;
            }
            else return null;
        }
   
        private void LoadUserInfoFromRow(UserInfo info, DataRow row)
        {
            info.Id = DBTool.GetIntFromRow(row, "Id", 0);
            info.Name = DBTool.GetStringFromRow(row, "Name", "");
            info.DeptId = DBTool.GetIntFromRow(row, "DeptId", 0);
            info.DeptName = DBTool.GetStringFromRow(row, "DeptName", "");
            info.authority = ReadAuthority(info.Id).Tables[0];
            info.BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
            info.BranchName = DBTool.GetStringFromRow(row, "BranchName", "");
            info.SN1 = DBTool.GetStringFromRow(row, "SN1", "");
            info.SN2 = DBTool.GetStringFromRow(row, "SN2", "");
            info.IsSales = DBTool.GetIntFromRow(row, "IsSales", 0);
            info.DefaultStoreId = GetDefaultStoreId(info.BranchId);//获取默认的仓库ID
            info.token = DBTool.GetStringFromRow(row, "token", "");
            info.tokenEndDate = DBTool.GetDateTimeFromRow(row, "tokenEndDate");
        }
        /// <summary>
        /// 验证用户身份--每次webservice调用都弄到此方法
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public bool CheckUser(string loginName, string passWord)
        {
            string sql = string.Format(" select * from view_sysusers where loginName='{0}' and IsValid = 1 and PassWord='{1}' ", loginName, MD5.GetMD5(passWord));
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否是系统管理员：LeaingManager调用
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public bool IsSysAdmin(string admin, string pass)
        {
            string sql = string.Format(" select * from view_sysusers where loginName='{0}' and IsValid = 1 and branchId=1 and IsAdmin=1",admin);
            DataSet ds = m_dbo.GetDataSet(sql);

            if (ds.Tables[0].Rows.Count >0)
            {
                if (MD5.GetMD5(pass) == DBTool.GetStringFromRow(ds.Tables[0].Rows[0], "PassWord", ""))
                {
                    return true;
                }
            }
            return false;
        }      
        /// <summary>
        /// 登录名是否可用
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        public bool CheckLoginName(string LoginName)
        {
            string sql = string.Format(" select * from sys_users where loginName='{0}' ", LoginName);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 读取 客户的权限列表
        /// </summary>
        /// <returns></returns>
        public DataSet ReadAuthority(int userId)
        {
            string sql = string.Format("select distinct * from View_UserRoleAuthority where UserId={0} ", userId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="newPass"></param>
        /// <returns></returns>
        public bool ChangePass(string newPass)
        {
            string sql = string.Format("update Sys_Users set PassWord='{0}' where Id={1} ", MD5.GetMD5(newPass), this.Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 设置 用户终端的硬盘-CPU序列号
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public bool SetSN(string SN1,string SN2)
        {
            string sql = string.Format("Update Sys_Users set SN1='{0}',SN2='{1}' where Id={2} ", SN1,SN2,this.Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 根据用户的token直接登录
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UserInfo LoginByToken(string token)
        {
            UserInfo info = new UserInfo();
            string sql = string.Format(" select * from view_sysusers where token='{0}' and IsValid = 1 ", token);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                if (DBTool.GetDateTimeFromRow(row, "tokenEndDate") > DateTime.Now)
                {
                    LoadUserInfoFromRow(info, row);
                    return info;
                }
                else return null;
            }
            else return null;
        }

        /// <summary>
        /// 以后有可能会根据用户制定仓库
        /// </summary>
        /// <returns></returns>
        private int GetDefaultStoreId(int branchId)
        {
            string sql = string.Format("select * from Store where BranchId={0} and IsDefault =1", branchId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return DBTool.GetIntFromRow(ds.Tables[0].Rows[0], "Id", 0);
            }
            else return 0;
        }
        //查询出Sys_User的信息
        public DataSet Sys_User() 
        {
            string sql = string.Format(" select * from Sys_Users ");
            return m_dbo.GetDataSet(sql);        
        }
        //查出用户城市
        public string ReadCity(string name) 
        {
            string sql = string.Format("select * from Sys_Branch where Id=(select BranchId from Sys_Users where Name ='{0}')", name);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return DBTool.GetStringFromRow(ds.Tables[0].Rows[0], "City", "0");
            }
            else return "0"; 
        }
        /// <summary>
        /// 根据branchId读取全部人员
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public DataSet GetUsersByBranchId(int branchId)
        {
            string sql = string.Format("select * from  View_SysUsers where BranchId={0} and IsValid=1  order by Name", branchId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 根据branchId,userId读取人员信息
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public DataSet GetUsers(int branchId, int userId)
        {
            string sql = string.Format("select * from  View_SysUsers where BranchId={0} and IsValid=1 and Id={1}", branchId, userId);
            return m_dbo.GetDataSet(sql);
        }
    }

    public class UserInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public string SN1 { get; set; }
        public string SN2 { get; set; }
        public DataTable authority { get; set; }
        public int IsSales { get; set; }
        public string token { get; set; }
        public DateTime tokenEndDate { get; set; }
        public int DefaultStoreId { get; set; }    
    }
}



