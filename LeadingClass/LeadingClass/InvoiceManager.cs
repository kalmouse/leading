using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
namespace LeadingClass
{
    public class InvoiceOption
    {
        private int m_Id;
        private int m_BranchId;
        private int m_OrderStatementId;
        private int m_InvoiceRequireId;
        private string m_InvoiceNo;
        private string m_InvoiceType;
        private double m_InvoiceMoneyS;
        private double m_InvoiceMoneyE;
        private DateTime m_InvoiceDateS;
        private DateTime m_InvoiceDateE;
        private string m_InvoiceName;
        private string m_InvoiceContent;
        private string m_InvoiceMemo;
        private string m_Company;

        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public int OrderStatementId { get { return m_OrderStatementId; } set { m_OrderStatementId = value; } }
        public int InvoiceRequireId { get { return m_InvoiceRequireId; } set { m_InvoiceRequireId = value; } }
        public string InvoiceNo { get { return m_InvoiceNo; } set { m_InvoiceNo = value; } }
        public string InvoiceType { get { return m_InvoiceType; } set { m_InvoiceType = value; } }
        public double InvoiceMoneyS { get { return m_InvoiceMoneyS; } set { m_InvoiceMoneyS = value; } }
        public double InvoiceMoneyE { get { return m_InvoiceMoneyE; } set { m_InvoiceMoneyE = value; } }
        public DateTime InvoiceDateS { get { return m_InvoiceDateS; } set { m_InvoiceDateS = value; } }
        public DateTime InvoiceDateE { get { return m_InvoiceDateE; } set { m_InvoiceDateE = value; } }
        public string InvoiceName { get { return m_InvoiceName; } set { m_InvoiceName = value; } }
        public string InvoiceContent { get { return m_InvoiceContent; } set { m_InvoiceContent = value; } }
        public string InvoiceMemo { get { return m_InvoiceMemo; } set { m_InvoiceMemo = value; } }
        public string Company { get { return m_Company; } set { m_Company = value; } }
        public InvoiceOption()
        {
            m_Id = 0;
            m_BranchId = 0;
            m_OrderStatementId = 0;
            m_InvoiceRequireId = 0;
            m_InvoiceNo = "";
            m_InvoiceType = "";
            m_InvoiceMoneyS = 0;
            m_InvoiceMoneyE = 0;
            m_InvoiceDateS = new DateTime(1900, 1, 1);
            m_InvoiceDateE = new DateTime(1900, 1, 1);
            m_InvoiceName = "";
            m_InvoiceContent = "";
            m_InvoiceMemo = "";
            m_Company = "";

        }
    }

    public class InvoiceManager
    {
        private DBOperate m_dbo;
        private InvoiceOption m_Option;
        public InvoiceOption Option { get { return m_Option; } set { m_Option = value; } }
        public InvoiceManager()
        {
            m_Option = new InvoiceOption();
            m_dbo = new DBOperate();
        }

        /// <summary>
        /// 读取所有待开票记录
        /// </summary>
        /// <returns></returns>
        public DataSet ReadInvoiceRequire()
        {
            string sql =string.Format(@"select * from view_InvoiceRequire where branchId={0} and InvoiceType='增票' and InvoiceStatus='待开票' order by Id ;
                        select * from view_InvoiceRequire where branchId={0} and InvoiceType='普票' and  InvoiceStatus='待开票' order by Id ;
                        select * from view_InvoiceRequire where branchId={0} and InvoiceType='收据' and  InvoiceStatus='待开票' order by Id ;", Option.BranchId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取对账单的 开票申请
        /// ERP FInvoiceRequire.cs 中调用
        /// </summary>
        /// <param name="statementId"></param>
        /// <returns></returns>
        public DataSet ReadStatementInvoiceRequire(int statementId)
        {
            string sql = string.Format("select * ,(select Name from Sys_Users us where us.Id = inv.UserId) as UserName from InvoiceRequire inv where StatementId = {0} order by Id desc ", statementId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 查询发票
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadInvoice(InvoiceOption option)
        {
            string sql = " select * from view_Invoice where 1=1 ";
            if (option.BranchId > 0)
            {
                sql += string.Format(" and BranchId={0} ", option.BranchId);
            }
            if (option.InvoiceContent != "")
            {
                sql += string.Format(" and InvoiceContent = '{0}' ", option.InvoiceContent);
            }
            if (option.InvoiceDateS != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and InvoiceDate >= '{0}' ", option.InvoiceDateS.ToShortDateString());
            }
            if (option.InvoiceDateE != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and InvoiceDate < '{0}' ", option.InvoiceDateE.AddDays(1).ToShortDateString());
            }
            if (option.InvoiceMemo != "")
            {
                sql += string.Format(" and InvoiceMemo like '%{0}%' ", option.InvoiceMemo);
            }
            if (option.InvoiceMoneyS > 0)
            {
                sql += string.Format(" and InvoiceMoney >={0} ", option.InvoiceMoneyS);
            }
            if (option.InvoiceMoneyE > 0)
            {
                sql += string.Format(" and InvoiceMoney <={0} ", option.InvoiceMoneyE);
            }
            if (option.InvoiceName != "")
            {
                sql += string.Format(" and InvoiceName like '%{0}%' ", option.InvoiceName);
            }
            if (option.InvoiceNo != "")
            {
                sql += string.Format(" and InvoiceNo like '%{0}%' ", option.InvoiceNo);
            }
            if (option.InvoiceRequireId > 0)
            {
                sql += string.Format(" and InvoiceRequierId = {0} ", option.InvoiceRequireId);
            }
            if (option.InvoiceType != "")
            {
                sql += string.Format(" and InvoiceType = '{0}' ", option.InvoiceType);
            }
            if (option.OrderStatementId > 0)
            {
                sql += string.Format(" and OrderStatementId = {0} ", option.OrderStatementId);
            }
            if (option.Company != "")
            {
                sql += string.Format(" and Company like '%{0}%' ", option.Company);
            }
            sql += " order by UpdateTime desc ";
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 获取 最新的发票号
        /// </summary>
        /// <param name="InvoiceType"></param>
        /// <returns></returns>
        public string GetNextInvoiceNo(string InvoiceType,int BranchId)
        {
            string sql = string.Format(" select top 1 InvoiceNo from Invoice where InvoiceType='{0}' and BranchId='{1}' order by UpdateTime desc ", InvoiceType,BranchId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                string invoiceno = DBTool.GetStringFromRow(ds.Tables[0].Rows[0], "InvoiceNo", "");
                if (invoiceno != "")
                {
                    if (CommenClass.MathTools.IsInt(invoiceno))
                    {
                        string r = (int.Parse(invoiceno)+1).ToString();
                        int len = r.Length;
                        if (len < 8)
                        {
                            for (int i = 0; i < (8 - len); i++)
                            {
                                r = "0" + r;
                            }
                        }
                        return r;
                    }
                    else return "";
                }
                else return "";
            }
            else return "";
        }

        /// <summary>
        /// 保存开票申请
        /// </summary>
        /// <param name="require"></param>
        /// <returns></returns>
        public int AddInvoiceRequire(InvoiceRequire require)
        {
            InvoiceRequire ir = new InvoiceRequire();
            ir.ComId = require.ComId;
            ir.Id = require.Id;
            ir.InvoiceAmount = require.InvoiceAmount;
            ir.InvoiceContent = require.InvoiceContent;
            ir.InvoiceMemo = (require.InvoiceMemo != null) ? require.InvoiceMemo : "";
            ir.InvoiceName = require.InvoiceName;
            ir.InvoiceType = require.InvoiceType;
            ir.StatementId = require.StatementId;
            ir.UpdateTime = DateTime.Now;
            ir.UserId = require.UserId;
            ir.InvoiceStatus = require.InvoiceStatus;
            ir.MemberInvoiceId = require.MemberInvoiceId;
            int r = ir.Save();
            if (r > 0)
            {
                //重置对账单发票状态
                OrderStatementManager osm = new OrderStatementManager();
                osm.UpdateOrderStatementInvoiceData(require.StatementId);

                //记录最新的客户开票信息
                Customer customer = new Customer();
                customer.Load(require.ComId);
                customer.Invoice_Content = require.InvoiceContent;
                customer.Invoice_Name = require.InvoiceName;
                customer.InvoiceType = require.InvoiceType;
                customer.Save();
            }
            return r;
        }
    }
}
