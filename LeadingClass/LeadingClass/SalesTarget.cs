using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class Salestarget
    {
        public int Id { get; set; }
        public int SalesId { get; set; }
        public DateTime YearMonth { get; set; }
        public int Target { get; set; }
        public double Finish { get; set; }
        public double FinishRatio { get; set; }
        public double NewCustomer { get; set; }
        public double OldCustormer { get; set; }
        public double NewGrossProfit { get; set; }
        public double OldGrossProfit { get; set; }
        public double GrossProfit { get; set; }
        public double NormalCost { get; set; }
        public double DLHSCost { get; set; }
        public double SaleCost { get; set; }
        public double ProfitRatio { get; set; }
        public double ProfitFactor { get; set; }
        public double Commission { get; set; }
        public int CommissionConfirm { get; set; }
        public DateTime DeadLine { get; set; }
        public double OverdueFactor { get; set; }
        public double Ammount { get; set; }
        public int AmmountConfirm { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public Salestarget()
        {
            Id = 0;
            SalesId = 0;
            YearMonth = new DateTime(1900, 1, 1);
            Target = 0;
            Finish = 0;
            FinishRatio = 0;
            NewCustomer = 0;
            OldCustormer = 0;
            NewGrossProfit = 0;
            OldGrossProfit = 0;
            GrossProfit = 0;
            NormalCost = 0;
            DLHSCost = 0;
            SaleCost = 0;
            ProfitRatio = 0;
            ProfitFactor = 0;
            Commission = 0;
            CommissionConfirm = 0;
            DeadLine = new DateTime(1900, 1, 1);
            OverdueFactor = 0;
            Ammount = 0;
            AmmountConfirm = 0;
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
            arrayList.Add(new SqlParameter("@SalesId", SalesId));
            arrayList.Add(new SqlParameter("@YearMonth", YearMonth));
            arrayList.Add(new SqlParameter("@Target", Target));
            arrayList.Add(new SqlParameter("@Finish", Finish));
            arrayList.Add(new SqlParameter("@FinishRatio", FinishRatio));
            arrayList.Add(new SqlParameter("@NewCustomer", NewCustomer));
            arrayList.Add(new SqlParameter("@OldCustormer", OldCustormer));
            arrayList.Add(new SqlParameter("@NewGrossProfit", NewGrossProfit));
            arrayList.Add(new SqlParameter("@OldGrossProfit", OldGrossProfit));
            arrayList.Add(new SqlParameter("@GrossProfit", GrossProfit));
            arrayList.Add(new SqlParameter("@NormalCost", NormalCost));
            arrayList.Add(new SqlParameter("@DLHSCost", DLHSCost));
            arrayList.Add(new SqlParameter("@SaleCost", SaleCost));
            arrayList.Add(new SqlParameter("@ProfitRatio", ProfitRatio));
            arrayList.Add(new SqlParameter("@ProfitFactor", ProfitFactor));
            arrayList.Add(new SqlParameter("@Commission", Commission));
            arrayList.Add(new SqlParameter("@CommissionConfirm", CommissionConfirm));
            arrayList.Add(new SqlParameter("@DeadLine", DeadLine));
            arrayList.Add(new SqlParameter("@OverdueFactor", OverdueFactor));
            arrayList.Add(new SqlParameter("@Ammount", Ammount));
            arrayList.Add(new SqlParameter("@AmmountConfirm", AmmountConfirm));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("SalesTarget", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("SalesTarget", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        private bool LoadFromRow(DataRow row)
        {
            Id = DBTool.GetIntFromRow(row, "Id", 0);
            SalesId = DBTool.GetIntFromRow(row, "SalesId", 0);
            YearMonth = DBTool.GetDateTimeFromRow(row, "YearMonth");
            Target = DBTool.GetIntFromRow(row, "Target", 0);
            Finish = DBTool.GetDoubleFromRow(row, "Finish", 0);
            FinishRatio = DBTool.GetDoubleFromRow(row, "FinishRatio", 0);
            NewCustomer = DBTool.GetDoubleFromRow(row, "NewCustomer", 0);
            OldCustormer = DBTool.GetDoubleFromRow(row, "OldCustormer", 0);
            NewGrossProfit = DBTool.GetDoubleFromRow(row, "NewGrossProfit", 0);
            OldGrossProfit = DBTool.GetDoubleFromRow(row, "OldGrossProfit", 0);
            GrossProfit = DBTool.GetDoubleFromRow(row, "GrossProfit", 0);
            NormalCost = DBTool.GetDoubleFromRow(row, "NormalCost", 0);
            DLHSCost = DBTool.GetDoubleFromRow(row, "DLHSCost", 0);
            SaleCost = DBTool.GetDoubleFromRow(row, "SaleCost", 0);
            ProfitRatio = DBTool.GetDoubleFromRow(row, "ProfitRatio", 0);
            ProfitFactor = DBTool.GetDoubleFromRow(row, "ProfitFactor", 0);
            Commission = DBTool.GetDoubleFromRow(row, "Commission", 0);
            CommissionConfirm = DBTool.GetIntFromRow(row, "CommissionConfirm", 0);
            DeadLine = DBTool.GetDateTimeFromRow(row, "DeadLine");
            OverdueFactor = DBTool.GetDoubleFromRow(row, "OverdueFactor", 0);
            Ammount = DBTool.GetDoubleFromRow(row, "Ammount", 0);
            AmmountConfirm = DBTool.GetIntFromRow(row, "AmmountConfirm", 0);
            UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
            return true;
        }
        public bool Load()
        {
            string sql = string.Format("select * from SalesTarget where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                return LoadFromRow(row);
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from SalesTarget where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        public bool Load(DateTime YearMonth,int salesId)
        {
            string sql = string.Format("select * from SalesTarget where SalesId={0} and YearMonth='{1}' ", salesId,YearMonth.ToShortDateString());
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                return LoadFromRow(row);
            }
            return false;
        }


    }

    public class SalesTargetOption
    {
        public int SalesId { get; set; }
        public int DeptId { get; set; }
        public int BranchId { get; set; }
        public DateTime StartMonth { get; set; }
        public DateTime EndMonth { get; set; }
       

        public SalesTargetOption()
        {
            SalesId = 0;
            DeptId = 0;
            BranchId = 0;
            StartMonth = new DateTime(1900, 1, 1);
            EndMonth = new DateTime(1900, 1, 1);
        }
    }
    
    public class SalesTargetManager
    {
        private DBOperate m_dbo;
        public SalesTargetManager()
        {
            m_dbo = new DBOperate();
        }

        /// <summary>
        /// 为业务员初始化业绩数据 为0
        /// </summary>
        /// <param name="salesId"></param>
        /// <returns></returns>
        public int InitSalesTarget(int Year, int salesId)
        {
            string s = new DateTime(Year,1,1).ToShortDateString();
            string e = new DateTime(Year + 1, 1, 1).ToShortDateString();
            string sql = string.Format(" delete from SalesTarget where SalesId={0} and YearMonth >='{1}' and YearMonth <'{2}' ", salesId, s, e);
            int OK = 0;
            if (m_dbo.ExecuteNonQuery(sql))
            {
                for (int i = 1; i < 13; i++)
                {
                    Salestarget st = new Salestarget();
                    st.SalesId = salesId;
                    st.YearMonth = new DateTime(Year, i, 1);
                    if (st.Save() > 0)
                    {
                        OK += 1;
                    }
                }
            }
            return OK;
        }

        /// <summary>
        /// 读取销售业绩表
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadSalesTarget(SalesTargetOption option)
        {
            string sql = " select * from view_SalesTarget where 1=1 ";
            if (option.BranchId > 0)
            {
                sql += string.Format(" and BranchId={0}", option.BranchId);
            }
            if (option.SalesId > 0)
            {
                sql += string.Format(" and SalesId={0} ", option.SalesId);
            }
            if (option.DeptId > 0)
            {
                sql += string.Format(" and DeptId={0} ", option.DeptId);
            }
            if (option.StartMonth != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and YearMonth >= '{0}' ", option.StartMonth.ToShortDateString());
            }
            if (option.EndMonth != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and YearMonth <='{0}' ", option.EndMonth.AddDays(1).ToShortDateString());
            }
            sql += " order by SalesId,YearMonth ";
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 保存销售目标
        /// </summary>
        /// <param name="targets"></param>
        /// <returns></returns>
        public int SaveSalesTarget(Salestarget[] targets)
        {
            int OK = 0;
            for (int i = 0; i < targets.Length;i++ )
            {
                Salestarget st = new Salestarget();
                st.Id = targets[i].Id;
                st.Load();
                st.Target = targets[i].Target;
                st.Save();
                OK += 1;
            }
            return OK;
        }
        /// <summary>
        /// 保存销售费用
        /// </summary>
        /// <param name="targets"></param>
        /// <returns></returns>
        public int SaveSalesCost(Salestarget[] targets)
        {
            int OK = 0;
            for (int i = 0; i < targets.Length; i++)
            {
                Salestarget st = new Salestarget();
                st.Id = targets[i].Id;
                st.Load();
                st.NormalCost = targets[i].NormalCost;
                st.DLHSCost = targets[i].DLHSCost;
                st.SaleCost = targets[i].SaleCost;
                st.Save();
                OK += 1;
            }
            return OK;
        }

        /// <summary>
        /// 计算业务员的业绩数据
        /// </summary>
        /// <param name="YearMonth">所在月度</param>
        /// <param name="salesId">业务员Id</param>
        /// <returns>当月业绩</returns>
        public Salestarget CalcCommission(DateTime YearMonth, int salesId)
        {
            //计算 提成，返回计算结果

            Salestarget st = new Salestarget();

            st.Load(YearMonth, salesId);

            //读取销售业绩
            DataTable dt = GetSalesData(YearMonth, salesId).Tables[0];
            //st.Target =

            st.Finish = CommenClass.MathTools.ToDouble(dt.Compute("Sum(SumMoney)", "true").ToString());//总销售业绩
            st.GrossProfit = CommenClass.MathTools.ToDouble(dt.Compute("Sum(GrossProfit)", "true").ToString());//总销售毛利
            st.NewCustomer = CommenClass.MathTools.ToDouble(dt.Compute("Sum(SumMoney)", "DiffMonth<=12 and IsSPAssess=0").ToString());//新客户销售业绩
            st.NewGrossProfit = CommenClass.MathTools.ToDouble(dt.Compute("Sum(GrossProfit)", "DiffMonth<=12  and IsSPAssess=0").ToString());//新客户销售毛利
            st.OldCustormer = CommenClass.MathTools.ToDouble(dt.Compute("Sum(SumMoney)", "DiffMonth>12  and IsSPAssess=0").ToString());//老客户销售业绩
            st.OldGrossProfit = CommenClass.MathTools.ToDouble(dt.Compute("Sum(GrossProfit)", "DiffMonth >12  and IsSPAssess=0").ToString());//老客户销售毛利

            SalesTargetFactor stf = new SalesTargetFactor(st);
            st.Finish = stf.Finish;
            st.FinishRatio = stf.FinishRatio;
            st.ProfitFactor = stf.ProfitFactor;
            st.ProfitRatio = stf.ProfitRatio;
            st.Commission = stf.Commission;
            return st;
        }

        private DataSet GetSalesData(DateTime YearMonth, int salesId)
        {
            string sql = string.Format(@"select ComId,Company,Count(OrderId) as OrderCount, DATEDIFF(MONTH,addtime,StoreFinishTime) as DiffMonth,IsSPAssess, SalesName,SalesId, 
SUM(SumMoney) as SumMoney,SUM(GrossProfit) as GrossProfit 
from View_Order where RawOrderId=0 and  IsDelete=0 and StoreFinishTime >='{0}' and StoreFinishTime <'{1}' and salesId={2}
group by ComId,Company,SalesName,SalesId,DATEDIFF(MONTH,addtime,StoreFinishTime),IsSPAssess
order by SalesName,DiffMonth desc", YearMonth.ToShortDateString(), YearMonth.AddMonths(1).ToShortDateString(), salesId);
            return m_dbo.GetDataSet(sql);

        }
        

        /// <summary>
        /// 读取逾期账款
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadOverdue(SalesTargetOption option)
        {
            //标准：按照月度计算
            return null;
        }
    }

    public class SalesTargetFactor
    {
        /*本类中所有的 比例均按照 小数格式输出，例如：10%=0.1 */

        private double target;
        private double finish;
        private double grossProfit;
        private double newCustomerSalesAmount;
        private double oldCustomerSalesAmount;
        private double newCustomerGrossProfit;
        private double oldCustomerGrossProfit;
        private double normalCost;
        private double dlhsCost;

        private const double MaxPF = 0.25;//销售毛利 分水岭 销售毛利超过 25%的部分提成系数 1.2 用到的常量
        private const double FY = 0.09;//销售固定费用率

        public double Target { get { return target; } }//销售业绩
        public double Finish { get { return finish; } }//总销售业绩(包括：超低毛利的客户)
        public double GrossProfit { get { return grossProfit; } }//毛利润=销售差价
        public double NewCustomerSalesAmount { get { return newCustomerSalesAmount; } }//新客户销售业绩（不包括超低毛利客户）
        public double OldCustomerSalesAmount { get { return oldCustomerSalesAmount; } }//老客户销售业绩（不包括超低毛利客户）
        public double SalesAmount { get { return newCustomerSalesAmount + oldCustomerSalesAmount; } }//销售业绩（不包括超低毛利客户） 应该 小于等于 Finish
        public double NewCustomerGrossProfit { get { return newCustomerGrossProfit; } }//新客户销售毛利（不包括超低毛利客户）
        public double OldCustomerGrossProfit { get { return oldCustomerGrossProfit; } }//老客户销售毛利（不包括超低毛利客户）
        public double NormalCost { get { return normalCost; } }//新老客户销售费用（不包括超低毛利客户）
        public double DLHSCost { get { return dlhsCost; } }//独立核算客户销售费用（超低毛利客户）
        public double SaleCost { get { return normalCost + dlhsCost; } }//销售费用（所有销售费用）
        public double NormalSaleCostRatio { get { return NormalCost / SalesAmount; } }//常规销售费用率

        public double FinishRatio { get { return Finish / Target; } }//业绩完成率
        public double NewCustomerFinishFactor { get { return GetTargetFactor(IsNewCustomer: true); } }
        public double OldCustomerFinishFactor { get { return GetTargetFactor(IsNewCustomer: false); } }
        public double NewCustomerProfit { get { return GetProfit(IsNewCustomer: true); } }//新客户销售利润基数=销售差价-销售额*(0.09 + 销售费用率)
        public double OldCustomerProfit { get { return GetProfit(IsNewCustomer: false); } }//老客户销售利润基数
        public double Profit { get { return NewCustomerProfit + OldCustomerProfit; } }//销售利润基数
        public double ProfitRatio { get { return GetProfitRatio(); } } //销售利润率=（销售毛利-其他销售费用）/销售额-（税金和送配费用0.09）

        public double ProfitFactor { get { return GetProfitFactor(); } }//销售利润率提成系数

        public double Commission { get { return GetCommission(); } }//当月应发提成（没有计算 应收账款系数）

        public SalesTargetFactor(Salestarget st)
        {
            this.target = st.Target;
            this.finish = st.Finish;
            this.grossProfit = st.GrossProfit;
            this.newCustomerSalesAmount = st.NewCustomer;
            this.oldCustomerSalesAmount = st.OldCustormer;
            this.newCustomerGrossProfit = st.NewGrossProfit;
            this.oldCustomerGrossProfit = st.OldGrossProfit;
            this.normalCost = st.NormalCost;
            this.dlhsCost = st.DLHSCost;
        }

        private double GetProfit(bool IsNewCustomer)
        {
            if (IsNewCustomer)
            {
                //销售利润基数=销售差价-销售额*(0.09 + 销售费用率)
                return newCustomerGrossProfit - newCustomerSalesAmount * (0.09 + NormalSaleCostRatio);
            }
            else
            {
                return oldCustomerGrossProfit - oldCustomerSalesAmount * (0.09 + NormalSaleCostRatio);
            }
        }
        /// <summary>
        /// 销售考核利润率基数
        /// </summary>
        /// <returns></returns>
        private double GetProfitRatio()
        {
            return Profit / SalesAmount;
        }
        /// <summary>
        /// 计算 利润率提成系数 >1时，是超额部分，按照1.2的系数进行计算
        /// </summary>
        /// <returns></returns>
        private double GetProfitFactor()
        {
            double pf = ProfitRatio;
            if (pf > 0.16)
                return 1.2;
            else if (pf > 0.11)
                return 1;
            else if (pf > 0.09)
                return 0.8;
            else if (pf > 0.06)
                return 0.5;
            else return 0;
        }

        /// <summary>
        /// 新老客户 业绩提成系数
        /// </summary>
        /// <param name="IsNewCustomer">是否是新客户</param>
        /// <returns></returns>
        private double GetTargetFactor(bool IsNewCustomer)
        {
            double finishRatio = FinishRatio;
            if (IsNewCustomer)
            {
                if (finishRatio >= 1)
                    return 0.25;
                else if (finishRatio >= 0.8)
                    return 0.2;
                else if (finishRatio >= 0.6)
                    return 0.15;
                else return 0.1;
            }
            else
            {
                if (finishRatio >= 1)
                    return 0.1;
                else if (finishRatio >= 0.8)
                    return 0.07;
                else if (finishRatio >= 0.6)
                    return 0.05;
                else return 0.03;
            }
        }

        /// <summary>
        /// 计算业务员提成的核心公式：未考虑 回款率的影响
        /// </summary>
        /// <returns></returns>
        private double GetCommission()
        {
            double result = 0;

            //毛利率系数小于等于1时，直接计算
            if (this.ProfitFactor <= 1)
            {
                result = (this.OldCustomerProfit * this.OldCustomerFinishFactor + this.NewCustomerProfit * this.NewCustomerFinishFactor) * this.ProfitFactor;
            }
            else
            {
                result = this.OldCustomerProfit * this.OldCustomerFinishFactor + this.NewCustomerProfit * this.NewCustomerFinishFactor;
                result += this.Profit * (this.ProfitRatio - MaxPF + FY) * 0.2;
            }
            return result;
        }

        #region 业绩考核的各参数

        /// <summary>
        /// 获取 逾期账款系数--OK
        /// </summary>
        /// <param name="OverdueRatio"></param>
        /// <returns></returns>
        public static double GetOverdueFactor(double OverdueRatio)
        {
            if (OverdueRatio <= 0.15)
                return 1;
            else if (OverdueRatio <= 0.3)
                return 0.8;
            else return 0.6;
        }

        /// <summary>
        /// 获取 逾期账款系数
        /// </summary>
        /// <param name="Overdue">逾期账款</param>
        /// <param name="AR">应收账款</param>
        /// <returns></returns>
        public static double GetOverdueFactor(double Overdue, double AR)
        {
            return GetOverdueFactor(Overdue / AR);
        }

        #endregion
    }

}
