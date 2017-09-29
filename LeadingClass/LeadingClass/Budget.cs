using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.SqlClient;

namespace LeadingClass
{
    //预算设置规则
    public class Budget
    {
        public int Id { get; set; }
        public int ComId { get; set; }
        public int DeptId { get; set; }
        public int MemberId { get; set; }
        public int IsDept { get; set; }
        public int IsPersonal { get; set; }
        public double YearBudget { get; set; }
        public double MonthBudget { get; set; }
        public int MaxOrderNum { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public Budget()
        {
            Id = 0;
            ComId = 0;
            DeptId = 0;
            MemberId = 0;
            IsDept = 0;
            IsPersonal = 0;
            YearBudget = 0;
            MonthBudget = 0;
            MaxOrderNum = 0;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
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
            arrayList.Add(new SqlParameter("@ComId", ComId));
            arrayList.Add(new SqlParameter("@DeptId", DeptId));
            arrayList.Add(new SqlParameter("@MemberId", MemberId));
            arrayList.Add(new SqlParameter("@IsDept", IsDept));
            arrayList.Add(new SqlParameter("@IsPersonal", IsPersonal));
            arrayList.Add(new SqlParameter("@Budget", YearBudget));
            arrayList.Add(new SqlParameter("@MonthBudget", MonthBudget));
            arrayList.Add(new SqlParameter("@MaxOrderNum", MaxOrderNum));
            arrayList.Add(new SqlParameter("@StartDate", StartDate));
            arrayList.Add(new SqlParameter("@EndDate", EndDate));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Budget", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Budget", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Budget where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取公司的预算设置
        /// </summary>
        /// <param name="ComId"></param>
        /// <param name="DeptId"></param>
        /// <param name="MemberId"></param>
        /// <param name="IsDept"></param>
        /// <param name="IsPersonal"></param>
        /// <returns></returns>
        public bool Load(int ComId, int DeptId, int MemberId, int IsDept, int IsPersonal)
        {
            string sql = "select * from Budget where 1=1";
            if (ComId > 0)
            {
                sql += string.Format(" and ComId={0} and DeptId={1} and MemberId={2} and IsDept={3} and IsPersonal={4}",ComId,DeptId,MemberId,IsDept,IsPersonal);
            }
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count ==1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }
        public void LoadFromRow(DataRow row)
        {
            Id = DBTool.GetIntFromRow(row, "Id", 0);
            ComId = DBTool.GetIntFromRow(row, "ComId", 0);
            DeptId = DBTool.GetIntFromRow(row, "DeptId", 0);
            MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);
            IsDept = DBTool.GetIntFromRow(row, "IsDept", 0);
            IsPersonal = DBTool.GetIntFromRow(row, "IsPersonal", 0);
            YearBudget = DBTool.GetDoubleFromRow(row, "Budget", 0);
            MonthBudget = DBTool.GetDoubleFromRow(row, "MonthBudget", 0);
            MaxOrderNum = DBTool.GetIntFromRow(row, "MaxOrderNum", 0);
            StartDate = DBTool.GetDateTimeFromRow(row, "StartDate");
            EndDate = DBTool.GetDateTimeFromRow(row, "EndDate");
            UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from Budget where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
    
    //人员预算明细（使用情况）
    public class BudgetMemberBalance
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string Type { get; set; }
        public double Get { get; set; }
        public double Spend { get; set; }
        public double Balance { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public BudgetMemberBalance()
        {
            Id = 0;
            MemberId = 0;
            Type = "";
            Get = 0;
            Spend = 0;
            Balance = 0;
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
            arrayList.Add(new SqlParameter("@Type", Type));
            arrayList.Add(new SqlParameter("@Get", Get));
            arrayList.Add(new SqlParameter("@Spend", Spend));
            arrayList.Add(new SqlParameter("@Balance", Balance));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("BudgetMemberBalance", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("BudgetMemberBalance", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from BudgetMemberBalance where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);
                Type = DBTool.GetStringFromRow(row, "Type", "");
                Get = DBTool.GetDoubleFromRow(row, "Get", 0);
                Spend = DBTool.GetDoubleFromRow(row, "Spend", 0);
                Balance = DBTool.GetDoubleFromRow(row, "Balance", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from BudgetMemberBalance where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }


    public class BudgetManager
    {
        private DBOperate m_dbo;
        public BudgetManager()
        {
            m_dbo = new DBOperate();
        }
        /// <summary>
        /// 读取公司所有的预算设置，供管理员查看
        /// </summary>
        /// <param name="ComId"></param>
        /// <returns></returns>
        public DataSet ReadCompanyBudget(int ComId)
        {
            string sql = string.Format(@"select * from Budget where ComId={0} and IsPersonal=1;
            select * from Budget where ComId={0} and IsDept=1;
            select * from Budget where ComId={0} and IsPersonal=0 and IsDept=0 and MemberId=0 and DeptId=0",ComId);
            return m_dbo.GetDataSet(sql);
        }
        
        /// <summary>
        /// 读取部门所有的预算设置
        /// </summary>
        /// <param name="ComId"></param>
        /// <returns></returns>
        public DataSet SetDeptBudget(int ComId)
        {
            string sql = string.Format(@"select distinct d.Id,d.Name,d.Code,d.PCode,d.level,Budget, MonthBudget from Dept d left join Budget b on d.Id=b.DeptId where  d.ComId={0} order by Code ", ComId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 设置人员的预算
        /// </summary>
        /// <param name="MemberId"></param>
        /// <returns></returns>
        public DataSet SetMemberBudget(int ComId)
        {
            string sql = string.Format(@"select m.Id as MemberId,m.LoginName,m.RealName,Budget , MonthBudget,m.DeptId,d.Code from Member m left join Budget b on m.Id = b.MemberId join Dept d on m.DeptId = d.Id where m.ComId= {0} order by Code", ComId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 返回是否是按人员管理
        /// </summary>
        /// <param name="ComId"></param>
        /// <returns></returns>
        public int CheckIsDept(int ComId)
        {
            string sql = string.Format("select * from Budget where ComId={0} and (IsDept =1 or IsPersonal=1) order by IsDept desc",ComId);
            DataSet ds = m_dbo.GetDataSet(sql);
            int IsDept = 0;
            if (ds.Tables[0].Rows.Count == 1)
            {
                IsDept = Convert.ToInt32(ds.Tables[0].Rows[0]["IsDept"]);
            }
            return IsDept;
        }


        /// <summary>
        /// 读取可用预算余额
        /// </summary>
        /// <param name="MemberId"></param>
        /// <returns></returns>
        public double ReadMemberBudgetBalance(int MemberId)
        {
            //每月1日，系统自动为所有启用预算的单位，每个member增加相应的额度，记录在 BudgetMemberBalance？自动还是手动呢？管理员可以重置预算金额。
            {
                //自动增加预算余额的算法
            }

            //用户登录后从BudgetMemberBalance读取可用余额，如果记录数为0，说明是新用户？这时候自动增加本周期可用额度，还是添加用时获取额度呢（有可能是我们帮客户管理用户）？

            //读取该用户的带审核订单金额，balance要减去此金额==可用额度

            //return balance-待审核额度

            return 0;
        }

        /// <summary>
        /// 读取用户适用的预算设置
        /// </summary>
        /// <param name="MemberId"></param>
        /// <returns></returns>
        public DataSet ReadMemberBudget(int memberId)
        {
            string sql = string.Format(@"select * from Budget where MemberId={0};select * from Budget where deptId =(select DeptId from Member where Id={0} );select * from Budget where ComId=(select ComId from Member where Id={0}) and ( IsPersonal=1 or IsDept=1);", memberId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 清空 特殊设置（部门 人员）
        /// </summary>
        /// <param name="comId"></param>
        /// <param name="isdept"></param>
        /// <returns></returns>
        public bool clearBudget(int comId,int isdept)
        {
            string sql = string.Format("delete from Budget where ComId={0}", comId);
            if (isdept == 1)
            {
                sql += " and DeptId<>0 ";
            }
            else {
                sql += " and MemberId<>0 ";
            }
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}
