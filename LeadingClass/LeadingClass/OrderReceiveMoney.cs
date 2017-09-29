using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class OrderReceiveMoney
    {
        private int m_Id;
        private int m_BranchId;
        private int m_OrderStatementId;
        private double m_ReceiveMoney;
        private string m_PayStatus;
        private int m_BankAccountId;
        private string m_PayType;
        private string m_Memo;
        private int m_UserId;
        private DateTime m_ReceiveDate;
        private DateTime m_UpdateTime;
        private double m_ChargeOff;//add by quxiaoshan 2014-11-07
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public int OrderStatementId { get { return m_OrderStatementId; } set { m_OrderStatementId = value; } }
        public double ReceiveMoney { get { return m_ReceiveMoney; } set { m_ReceiveMoney = value; } }
        public string PayStatus { get { return m_PayStatus; } set { m_PayStatus = value; } }
        public int BankAccountId { get { return m_BankAccountId; } set { m_BankAccountId = value; } }
        public string PayType { get { return m_PayType; } set { m_PayType = value; } }
        public string Memo { get { return m_Memo; } set { m_Memo = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public DateTime ReceiveDate { get { return m_ReceiveDate; } set { m_ReceiveDate = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public double ChargeOff { get { return m_ChargeOff; } set { m_ChargeOff = value; } }//add by quxiaoshan 2014-11-07
        public OrderReceiveMoney()
        {
            m_Id = 0;
            m_BranchId = 0;
            m_OrderStatementId = 0;
            m_ReceiveMoney = 0;
            //m_YouHui = 0;
            m_PayStatus = "";
            m_BankAccountId = 0;
            m_PayType = "";
            m_Memo = "";
            m_UserId = 0;
            m_UpdateTime = DateTime.Now;
            m_ChargeOff = 0;
            m_dbo = new DBOperate();
        }
        /// <summary>
        /// ERP FOrderReceiveMoney.cs 中调用
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@BranchId", m_BranchId));
            arrayList.Add(new SqlParameter("@OrderStatementId", m_OrderStatementId));
            arrayList.Add(new SqlParameter("@ReceiveMoney", Math.Round(m_ReceiveMoney, 2)));
            arrayList.Add(new SqlParameter("@PayStatus", m_PayStatus));
            arrayList.Add(new SqlParameter("@BankAccountId", m_BankAccountId));
            arrayList.Add(new SqlParameter("@PayType", m_PayType));
            arrayList.Add(new SqlParameter("@Memo", m_Memo));
            arrayList.Add(new SqlParameter("@UserId", m_UserId));
            arrayList.Add(new SqlParameter("@ReceiveDate", m_ReceiveDate));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            arrayList.Add(new SqlParameter("@ChargeOff", m_ChargeOff));//add by quxiaoshan 2014-11-07

            if (this.Id > 0)
            {
                m_dbo.UpdateData("OrderReceiveMoney", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("OrderReceiveMoney", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }

            if (this.Id > 0)
            {
                OrderStatement os = new OrderStatement();
                os.Id = this.OrderStatementId;
                os.UpdateStatus();
            }
            return this.Id;
        }
        /// <summary>
        /// ERP FOrderReceiveMoney.cs 中调用
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            string sql = string.Format("select * from OrderReceiveMoney where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                m_OrderStatementId = DBTool.GetIntFromRow(row, "OrderStatementId", 0);
                m_ReceiveMoney = DBTool.GetDoubleFromRow(row, "ReceiveMoney", 0);
                m_PayStatus = DBTool.GetStringFromRow(row, "PayStatus", "");
                m_BankAccountId = DBTool.GetIntFromRow(row, "BankAccountId", 0);
                m_PayType = DBTool.GetStringFromRow(row, "PayType", "");
                m_Memo = DBTool.GetStringFromRow(row, "Memo", "");
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_ReceiveDate = DBTool.GetDateTimeFromRow(row, "ReceiveDate");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_ChargeOff = DBTool.GetDoubleFromRow(row, "ChargeOff", 0);//add by quxiaoshan 2014-11-07
                return true;
            }
            return false;
        }
        /// <summary>
        /// ERP FOrderReceiveMoney.cs 中调用
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            this.Load();
            OrderStatement os = new OrderStatement();
            os.Id = this.OrderStatementId;

            string sql = string.Format(" delete from OrderReceiveMoney where Id={0} ", this.Id);
            if (m_dbo.ExecuteNonQuery(sql))
            {
                os.UpdateStatus();
                return true;
            }
            else return false;

        }
    }

    public class OrderReceiveMoneyOption
    {
        private int m_BranchId;
        private int m_OrderStatementId;
        private string m_PayStatus;
        private int m_BankAccountId;
        private int m_ComId;

        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public int OrderStatementId { get { return m_OrderStatementId; } set { m_OrderStatementId = value; } }
        public string PayStatus { get { return m_PayStatus; } set { m_PayStatus = value; } }
        public int BankAccountId { get { return m_BankAccountId; } set { m_BankAccountId = value; } }
        public int ComId { get { return m_ComId; } set { m_ComId = value; } }
        public OrderReceiveMoneyOption()
        {
            m_BranchId = 0;
            m_OrderStatementId = 0;
            m_PayStatus = "";
            m_BankAccountId = 0;
            m_ComId = 0;

        }
 
    }

    public class OrderReceiveMoneyManager
    {
        private DBOperate m_dbo;
        public OrderReceiveMoneyManager()
        {
            m_dbo = new DBOperate();
        }
        /// <summary>
        /// 读取收款列表
        /// 调用：ERP：FOrderStatement.cs
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadOrderReceiveMoney(OrderReceiveMoneyOption option)
        {
            string sql = " select * from view_OrderReceiveMoney where 1=1 ";
            if (option.BankAccountId > 0)
            {
                sql += string.Format(" and BankAccountId = {0} ", option.BankAccountId);
            }
            if (option.BranchId > 0)
            {
                sql += string.Format(" and BranchId = {0} ", option.BranchId);
            }
            if (option.ComId > 0)
            {
                sql += string.Format(" and ComId={0} ", option.ComId);
            }
            if (option.OrderStatementId > 0)
            {
                sql += string.Format(" and OrderStatementId ={0} ", option.OrderStatementId);
            }
            if (option.PayStatus != "")
            {
                if (option.PayStatus.IndexOf("|") < 0)
                {
                    sql += string.Format(" and PayStatus = '{0}' ", option.PayStatus);
                }
                else
                {
                    string[] ps = option.PayStatus.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    sql += " and ( ";
                    for (int i = 0; i < ps.Length; i++)
                    {
                        if (i == 0)
                        {
                            sql += string.Format(" PayStatus ='{0}' ", ps[i]);
                        }
                        else
                        {
                            sql += string.Format(" or PayStatus='{0}' ", ps[i]);
                        }

                    }
                    sql += " ) ";
                }
            }
            sql += " order by updateTime ";
            return m_dbo.GetDataSet(sql);
        }
    }
}
