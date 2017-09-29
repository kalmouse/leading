using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class OrderStatement
    {
        private int m_Id;
        private int m_BranchId;
        private int m_ComId;
        private string m_Memo;
        private double m_SumMoney;
        private double m_PaidMoney;
        private string m_PayStatus;
        private int m_UserId;
        private int m_PrintNum;
        private DateTime m_UpdateTime;
        private double m_InvoiceMoney;
        private double m_NeedToInvoice;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public int ComId { get { return m_ComId; } set { m_ComId = value; } }
        public string Memo { get { return m_Memo; } set { m_Memo = value; } }
        public double SumMoney { get { return m_SumMoney; } set { m_SumMoney = value; } }
        public double PaidMoney { get { return m_PaidMoney; } set { m_PaidMoney = value; } }
        public string PayStatus { get { return m_PayStatus; } set { m_PayStatus = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public int PrintNum { get { return m_PrintNum; } set { m_PrintNum = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public double InvoiceMoney { get { return m_InvoiceMoney; } set { m_InvoiceMoney = value; } }
        public double NeedToInvoice { get { return m_NeedToInvoice; } set { m_NeedToInvoice = value; } }
        public OrderStatement()
        {
            m_Id = 0;
            m_BranchId = 0;
            m_ComId = 0;
            m_Memo = "";
            m_SumMoney = 0;
            m_PaidMoney = 0;
            m_PayStatus = "";
            m_UserId = 0;
            m_PrintNum = 0;
            m_UpdateTime = DateTime.Now;
            m_InvoiceMoney = 0;
            m_NeedToInvoice = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@BranchId", m_BranchId));
            arrayList.Add(new SqlParameter("@ComId", m_ComId));
            arrayList.Add(new SqlParameter("@Memo", m_Memo));
            arrayList.Add(new SqlParameter("@SumMoney", Math.Round(m_SumMoney, 2)));
            arrayList.Add(new SqlParameter("@PaidMoney", m_PaidMoney));
            arrayList.Add(new SqlParameter("@PayStatus", m_PayStatus));
            arrayList.Add(new SqlParameter("@UserId", m_UserId));
            arrayList.Add(new SqlParameter("@PrintNum", m_PrintNum));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            arrayList.Add(new SqlParameter("@InvoiceMoney", m_InvoiceMoney));
            arrayList.Add(new SqlParameter("@NeedToInvoice", m_NeedToInvoice));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("OrderStatement", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("OrderStatement", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        /// <summary>
        /// ERP FOrderReceiveMoney.cs 中调用
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            string sql = string.Format("select * from OrderStatement where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                m_ComId = DBTool.GetIntFromRow(row, "ComId", 0);
                m_Memo = DBTool.GetStringFromRow(row, "Memo", "");
                m_SumMoney = DBTool.GetDoubleFromRow(row, "SumMoney", 0);
                m_PaidMoney = DBTool.GetDoubleFromRow(row, "PaidMoney", 0);
                m_PayStatus = DBTool.GetStringFromRow(row, "PayStatus", "");
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_PrintNum = DBTool.GetIntFromRow(row, "PrintNum", 0);
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_NeedToInvoice = DBTool.GetDoubleFromRow(row, "NeedToInvoice", 0);
                m_InvoiceMoney = DBTool.GetDoubleFromRow(row, "InvoiceMoney", 0);
                return true;
            }
            return false;
        }

        public bool Delete()
        {
            string sql = string.Format(@"update [Order] set PayStatus='未付款' where Id in (select OrderId from OrderStatementDetail where OrderStatementId={0}) ;
delete from OrderStatementDetail where OrderStatementId={0} ;
delete from OrderStatement where Id ={0} ; ", this.Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 添加新的收款记录后，更改对账单的付款金额和状态 add by quxiaoshan 2014-11-10
        /// 更改此对账单所有相关订单的付款状态paystatus、付款金额paidmoney、销账金额chargeoff
        /// </summary>
        /// <returns></returns>
        public int UpdateStatus()
        {
            this.Load();
            string sql = string.Format("select SUM(ChargeOff) as ChargeOff,SUM(ReceiveMoney) as ReceiveMoney from OrderReceiveMoney where OrderStatementId={0} ", this.Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            double chargeOff = Math.Round(DBTool.GetDoubleFromRow(ds.Tables[0].Rows[0], "ChargeOff", 0), 2);
            double receiveMoney = Math.Round(DBTool.GetDoubleFromRow(ds.Tables[0].Rows[0], "ReceiveMoney", 0), 2);
            this.PaidMoney = receiveMoney;
            if (chargeOff >= this.SumMoney)
            {
                if (chargeOff == receiveMoney)
                {
                    this.PayStatus = CommenClass.PayStatus.全额付清.ToString();
                }
                else if (chargeOff > receiveMoney)
                {
                    this.PayStatus = CommenClass.PayStatus.优惠付清.ToString();
                }
                else if (chargeOff < receiveMoney)
                {
                    this.PayStatus = CommenClass.PayStatus.超额付清.ToString();
                }
            }
            else if (chargeOff > 0)
            {
                this.PayStatus = CommenClass.PayStatus.部分付款.ToString();
            }
            else if (chargeOff == 0)
            {
                this.PayStatus = CommenClass.PayStatus.未付款.ToString();
            }
            int statementId = this.Save();
            if (statementId > 0)
            {
                UpdateOrdersPayStatus(chargeOff, receiveMoney, this.PayStatus);
            }
            return statementId;
        }
        /// <summary>
        /// 根据对账单收款情况，更新订单收款情况！
        /// </summary>
        private void UpdateOrdersPayStatus(double ChargeOff, double ReceiveMoney, string PayStatus)
        {
            switch (PayStatus)
            {
                case "全额付清":
                    UpdateOrderPayStatusQEFQ();
                    break;
                case "优惠付清":
                    UpdateOrderPayStatusYHFQ(ChargeOff, ReceiveMoney);
                    break;
                case "部分付款":
                    UpdateOrderPayStatusBFFK(ChargeOff, ReceiveMoney);
                    break;
                case "超额付清":
                    UpdateOrderPayStatysCEFQ(ChargeOff, ReceiveMoney);
                    break;
            }
        }
        /// <summary>
        /// 全额付清
        /// </summary>
        private void UpdateOrderPayStatusQEFQ()
        {
            //将该对账单所有的订单收款状态初始化
            string sql = string.Format(@"update [Order] set PayStatus='已对账',PaidMoney=0,ChargeOff=0 
            where Id in( select OrderId from OrderStatementDetail where OrderStatementId={0})", this.Id);
            m_dbo.ExecuteNonQuery(sql);

            //读取改对账单对应的所用订单列表
            sql = string.Format("select * from View_OrderStatementDetail  where OrderStatementId={0} order by OrderId ", this.Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            //循环 订单列表，按照时间先后进行销账处理
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];
                int orderId = DBTool.GetIntFromRow(row, "OrderId", 0);
                double summoney = Math.Round(DBTool.GetDoubleFromRow(row, "SumMoney", 0), 2);
                string paystatus = CommenClass.PayStatus.已付款.ToString();
                sql = string.Format("update [Order] set PayStatus='{0}',PaidMoney={1},ChargeOff={2} where Id={3}", paystatus, summoney, summoney, orderId);
                m_dbo.ExecuteNonQuery(sql);
            }
        }
        /// <summary>
        /// 优惠付清
        /// </summary>
        /// <param name="ChargeOff"></param>
        /// <param name="ReceiveMoney"></param>
        private void UpdateOrderPayStatusYHFQ(double ChargeOff, double ReceiveMoney)
        {
            //将该对账单所有的订单收款状态初始化
            string sql = string.Format(@"update [Order] set PayStatus='已对账',PaidMoney=0,ChargeOff=0 
            where Id in( select OrderId from OrderStatementDetail where OrderStatementId={0})", this.Id);
            m_dbo.ExecuteNonQuery(sql);

            //读取改对账单对应的所用订单列表
            sql = string.Format("select * from View_OrderStatementDetail  where OrderStatementId={0} order by OrderId ", this.Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            //循环 订单列表，按照时间先后进行销账处理
            double allreceivemoney = ReceiveMoney;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];
                int orderId = DBTool.GetIntFromRow(row, "OrderId", 0);
                double summoney = Math.Round(DBTool.GetDoubleFromRow(row, "SumMoney", 0), 2);
                string paystatus = CommenClass.PayStatus.已付款.ToString();
                double paidmoney = summoney;
                if (Math.Round(allreceivemoney) == 0)
                {
                    paidmoney = 0;
                }
                else
                {
                    if (ChargeOff >= 0)//正数对账单
                    {
                        //if (summoney >= 0)
                        //{
                        if (Math.Round(allreceivemoney) < Math.Round(summoney))
                        {
                            paidmoney = Math.Round(allreceivemoney, 2);
                        }
                        //}
                        //else paidmoney = summoney;
                    }
                    else//负数对账单
                    {
                        //if (summoney <= 0)
                        //{
                        if (Math.Round(allreceivemoney) > Math.Round(summoney))
                        {
                            paidmoney = Math.Round(allreceivemoney, 2);
                        }
                        //}
                        //else paidmoney = summoney;
                    }
                    allreceivemoney -= paidmoney;
                }
                sql = string.Format("update [Order] set PayStatus='{0}',PaidMoney={1},ChargeOff={2} where Id={3}", paystatus, paidmoney, summoney, orderId);
                m_dbo.ExecuteNonQuery(sql);
            }

        }
        
        /// <summary>
        /// 超额付清
        /// </summary>
        /// <param name="ChargeOff"></param>
        /// <param name="ReceiveMoney"></param>
        private void UpdateOrderPayStatysCEFQ(double ChargeOff, double ReceiveMoney)
        {
            //将该对账单所有的订单收款状态初始化
            string sql = string.Format(@"update [Order] set PayStatus='已对账',PaidMoney=0,ChargeOff=0 
            where Id in( select OrderId from OrderStatementDetail where OrderStatementId={0})", this.Id);
            m_dbo.ExecuteNonQuery(sql);

            //读取改对账单对应的所用订单列表
            sql = string.Format("select * from View_OrderStatementDetail  where OrderStatementId={0} order by OrderId ", this.Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            //循环 订单列表，按照时间先后进行销账处理
            double allchargeoff = ChargeOff;
            double allreceivemoney = ReceiveMoney;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];
                int orderId = DBTool.GetIntFromRow(row, "OrderId", 0);
                double summoney = Math.Round(DBTool.GetDoubleFromRow(row, "SumMoney", 0), 2);
                string paystatus = CommenClass.PayStatus.已付款.ToString();
                double chargeoff = summoney;
                double paidmoney = summoney;
                allchargeoff -= chargeoff;
                allreceivemoney -= paidmoney;
                if(i==ds.Tables[0].Rows.Count -1)
                {
                    paidmoney = summoney + allreceivemoney;
                }
                sql = string.Format("update [Order] set PayStatus='{0}',PaidMoney={1},ChargeOff={2} where Id={3}", paystatus, paidmoney, chargeoff, orderId);
                m_dbo.ExecuteNonQuery(sql);
            }
        }
        /// <summary>
        /// 部分付款
        /// </summary>
        /// <param name="ChargeOff"></param>
        /// <param name="ReceiveMoney"></param>
        private void UpdateOrderPayStatusBFFK(double ChargeOff, double ReceiveMoney)
        {
            //将该对账单所有的订单收款状态初始化
            string sql = string.Format(@"update [Order] set PayStatus='已对账',PaidMoney=0,ChargeOff=0 
            where Id in( select OrderId from OrderStatementDetail where OrderStatementId={0})", this.Id);
            m_dbo.ExecuteNonQuery(sql);

            //如果销账金额为0，直接退出
            if (PayStatus == CommenClass.PayStatus.未付款.ToString())
            {
                return;
            }
            //读取改对账单对应的所用订单列表
            sql = string.Format("select * from View_OrderStatementDetail  where OrderStatementId={0} order by OrderId ", this.Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            //循环 订单列表，按照时间先后进行销账处理
            double allchargeoff = ChargeOff;
            double allreceivemoney = ReceiveMoney;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];
                int orderId = DBTool.GetIntFromRow(row, "OrderId", 0);
                double summoney = Math.Round(DBTool.GetDoubleFromRow(row, "SumMoney", 0), 2);
                string paystatus = CommenClass.PayStatus.已付款.ToString();
                double chargeoff = summoney;
                double paidmoney = summoney;
                if (ChargeOff >= 0)//正数对账单
                {
                    if (allchargeoff <= 0)
                    {
                        break;
                    }
                    if (Math.Round(allchargeoff, 2) < summoney)
                    {
                        chargeoff = Math.Round(allchargeoff, 2);
                        paystatus = CommenClass.PayStatus.部分付款.ToString();
                    }
                    if (Math.Round(allreceivemoney) < Math.Round(summoney))
                    {
                        paidmoney = Math.Round(allreceivemoney, 2);
                    }
                }
                else//负数对账单
                {
                    if (allchargeoff >= 0)
                    {
                        break;
                    }
                    if (Math.Round(allchargeoff, 2) > summoney)
                    {
                        chargeoff = Math.Round(allchargeoff, 2);
                        paystatus = CommenClass.PayStatus.部分付款.ToString();
                    }
                    if (Math.Round(allreceivemoney) > Math.Round(summoney))
                    {
                        paidmoney = Math.Round(allreceivemoney, 2);
                    }
                }
                allchargeoff -= chargeoff;
                allchargeoff = Math.Round(allchargeoff, 2);
                allreceivemoney -= paidmoney;
                allreceivemoney = Math.Round(allreceivemoney, 2);
                sql = string.Format("update [Order] set PayStatus='{0}',PaidMoney={1},ChargeOff={2} where Id={3}", paystatus, paidmoney, chargeoff, orderId);
                m_dbo.ExecuteNonQuery(sql);
            }
        }
    }
    

    public class OrderStatementDetail
    {
        private int m_Id;
        private int m_OrderStatementId;
        private int m_OrderId;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int OrderStatementId { get { return m_OrderStatementId; } set { m_OrderStatementId = value; } }
        public int OrderId { get { return m_OrderId; } set { m_OrderId = value; } }
        public OrderStatementDetail()
        {
            m_Id = 0;
            m_OrderStatementId = 0;
            m_OrderId = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@OrderStatementId", m_OrderStatementId));
            arrayList.Add(new SqlParameter("@OrderId", m_OrderId));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("OrderStatementDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("OrderStatementDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }

            if (this.Id > 0)
            {
                Order order = new Order();
                order.Id = this.OrderId;
                order.Load();
                order.PayStatus = CommenClass.PayStatus.已对账.ToString();
                order.Save();
            }

            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from OrderStatementDetail where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_OrderStatementId = DBTool.GetIntFromRow(row, "OrderStatementId", 0);
                m_OrderId = DBTool.GetIntFromRow(row, "OrderId", 0);
                return true;
            }
            return false;
        }
 
    }

    public class OrderStatementOption
    {
        private int m_BranchId;
        private int m_ComId;
        private string m_PayStatus;

        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public int ComId { get { return m_ComId; } set { m_ComId = value; } }
        public string PayStatus { get { return m_PayStatus; } set { m_PayStatus = value; } }
        public OrderStatementOption()
        {
            m_BranchId = 0;
            m_ComId = 0;
            m_PayStatus = "";
        }
 
    }

    public class OrderStatementManager
    {
        private DBOperate m_dbo;

        public OrderStatementManager()
        {
            m_dbo = new DBOperate();
        }
        /// <summary>
        /// 保存对账
        /// </summary>
        /// <param name="os"></param>
        /// <param name="osds"></param>
        /// <returns></returns>
        public int SaveOrderStatement(OrderStatement os, OrderStatementDetail[] osds)
        {
            OrderStatement orderstatement = new OrderStatement();
            orderstatement.BranchId = os.BranchId;
            orderstatement.ComId = os.ComId;
            orderstatement.Id = os.Id;
            orderstatement.Memo = os.Memo;
            orderstatement.PaidMoney = os.PaidMoney;
            orderstatement.PayStatus = os.PayStatus;
            orderstatement.PrintNum = os.PrintNum;
            orderstatement.SumMoney = os.SumMoney;
            orderstatement.UpdateTime = DateTime.Now;
            orderstatement.UserId = os.UserId;
            orderstatement.InvoiceMoney = os.InvoiceMoney;
            orderstatement.NeedToInvoice = os.NeedToInvoice;
            int osId = orderstatement.Save();
            if (osId > 0)
            {
                for (int i = 0; i < osds.Length; i++)
                {
                    OrderStatementDetail osd = new OrderStatementDetail();
                    osd.OrderStatementId = osId;
                    osd.OrderId = osds[i].OrderId;
                    osd.Save();
                }
            }
            return osId;
        }
        /// <summary>
        /// 读取对账单列表
        /// 调用：ERP FOrderStatement.cs
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadOrderStatement(OrderStatementOption option)
        {
            string sql = "select * from View_OrderStatement where 1=1";
            if (option.BranchId > 0)
            {
                sql += string.Format(" and BranchId={0} ", option.BranchId);
            }
            if (option.ComId > 0)
            {
                sql += string.Format(" and ComId={0} ", option.ComId);
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
                            sql += string.Format(" or PayStatus='{0}' ",ps[i]);
                        }

                    }
                    sql += " ) ";
                }
            }
            sql += " order by comId,updatetime ";
            return m_dbo.GetDataSet(sql);
        }
        public DataSet ReadStatement(OrderStatementOption option)
        {
            string sql = "select Id,SumMoney,PaidMoney,NeedToPay,PayStatus,UpdateTime,Memo,ComId from View_OrderStatement where 1=1";
            if (option.BranchId > 0)
            {
                sql += string.Format(" and BranchId={0} ", option.BranchId);
            }
            if (option.ComId > 0)
            {
                sql += string.Format(" and ComId={0} ", option.ComId);
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
            sql += " order by comId,updatetime ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取对账单明细
        /// ERP FOrderReceiveMoney.cs 中调用
        /// </summary>
        /// <param name="osId"></param>
        /// <returns></returns>
        public DataSet ReadOrderStatementDetail(int osId)
        {
            string sql = string.Format(" select * from view_orderstatementDetail where orderstatementId={0} order by OrderId ",osId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取所有对账单对应的明细
        /// </summary>
        /// <param name="osId"></param>
        /// <returns></returns>
        public DataSet ReadOrderStatementDetail(List<int> Ids)
        {
            string sql = string.Format(" select * from view_orderstatementDetail where 1=1");
            if (Ids != null)
            {
                sql += " and orderstatementId in ( ";
                for (int i = 0; i < Ids.Count; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", Ids[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", Ids[i]);
                }
                sql += " ) ";
            }
            sql += " order by OrderId ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取对账单 商品明细
        ///  调用：ERP：FNeedToBuy.cs
        ///  用到的表;Order（订单表）、 Customer（客户详细信息）、GoodsType（商品类别）、OrderDetail（订单商品明细）、Sys_Users（系统用户表)、OrderStatus（订单流程状态表）、Company（客户公司信息表）
        /// </summary>
        /// <param name="osId"></param>
        /// <returns></returns>
        public DataSet ReadOrderStatementGoodsDetail(int osId)
        {
            string sql = string.Format(@"select PlanDate,DeptName,RealName, OrderId,GoodsId,DisplayName,Model,SalePrice,Num,Unit,Amount
from View_OrderDetail where OrderId in (select OrderId from OrderStatementDetail where OrderStatementId={0})
order by OrderId,Id", osId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取对账单 全部商品明细
        /// </summary>
        /// <param name="osId"></param>
        /// <returns></returns>
        public DataSet ReadOrderStatementGoodsDetail(List<int> Ids)
        {
            string sql = string.Format(@"select PlanDate,DeptName,RealName, OrderId,GoodsId,DisplayName,Model,SalePrice,Num,Unit,Amount
from View_OrderDetail where OrderId in (select OrderId from OrderStatementDetail where 1=1");
            if (Ids != null)
            {
                sql += " and OrderStatementId in ( ";
                for (int i = 0; i < Ids.Count; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", Ids[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", Ids[i]);
                }
                sql += " )) ";
            }
            sql += " order by OrderId,Id ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取对账单 商品汇总
        /// </summary>
        /// <param name="osId"></param>
        /// <returns></returns>
        public DataSet ReadOrderStatementGoodsSum(int osId)
        {
            string sql = string.Format(@"select GoodsId,DisplayName,Model,SalePrice,SUM( Num) as Num,Unit,SUM(Amount) as Amount
from View_OrderDetail where OrderId in (select OrderId from OrderStatementDetail where OrderStatementId={0})
group by GoodsId,DisplayName,Model,SalePrice,Unit
order by DisplayName", osId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 更改对账单 开票金额
        /// </summary>
        /// <param name="osId"></param>
        /// <returns></returns>
        public bool UpdateOrderStatementInvoiceData(int osId)
        {
            string sql = string.Format(@"select InvoiceStatus,Sum(invoiceAmount) as InvoiceAmount  
    from InvoiceRequire where StatementId={0} group by InvoiceStatus ", osId);
            DataSet ds = m_dbo.GetDataSet(sql);
            OrderStatement os = new OrderStatement();
            os.Id = osId;
            os.Load();
            os.InvoiceMoney = 0;
            os.NeedToInvoice = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (DBTool.GetStringFromRow(row, "InvoiceStatus", "") == CommenClass.InvoiceStatus.待开票.ToString())
                    {
                        os.NeedToInvoice = DBTool.GetDoubleFromRow(row, "InvoiceAmount", 0);
                    }
                    else if (DBTool.GetStringFromRow(row, "InvoiceStatus", "") == CommenClass.InvoiceStatus.已开票.ToString())
                    {
                        os.InvoiceMoney = DBTool.GetDoubleFromRow(row, "InvoiceAmount", 0);
                    }
                }
            }
            if (os.Save() > 0)
                return true;
            else return false;
        }

        /// <summary>
        /// 通过对账单Id获取订单
        /// </summary>
        /// <returns></returns>
        public DataSet GetOrderByOrderStatementId(int OrderStatementId)
        {
            string sql = string.Format(@"select Id,GUID from [Order] where Id in(select OrderId from OrderStatementDetail where OrderStatementId={0}) order by GUID desc", OrderStatementId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 详细每个月份的账单 -- 平台使用
        /// 调用：ERP：Forder.cs
        /// </summary>
        /// <param name="comId"></param>
        /// <returns></returns>
        public DataSet GetOrderTimePlatForm(int comId)
        {
            string sql = string.Format(@"select DATEPART( YEAR,StoreFinishTime) as 年份,
DATEPART(MONTH,StoreFinishTime) as 月份,
Company as 公司,
DeptName as 部门, 
RealName as 联系人,
PayStatus as 付款状态,
Sum(SumMoney) as 总金额,
ChargeOff as 已付款,
SUM(SumMoney-ChargeOff) as 需付款,
CreditDays as 账期 
from View_Order where IsDelete=0 and RawOrderId=0 and PayStatus <> '已付款'
and StoreFinishTime>'2014-1-1'
and ComId={0}
and StoreStatus='已完成'
group by DATEPART(YEAR,StoreFinishTime),DATEPART(MONTH,StoreFinishTime),CreditDays, DeptName,RealName,chargeoff,PayStatus,Company
order by 年份,月份", comId);
            return m_dbo.GetDataSet(sql);
        }



        /// <summary>
        /// 读取待回款账单明细
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="deptName"></param>
        /// <returns></returns>
        public DataSet GetOverdueOrderDetail(int Id,int comId)
        {
            string sql = string.Format(@"select DATEPART( YEAR,StoreFinishTime) as 年份,
DATEPART(MONTH,StoreFinishTime) as 月份,
Company as 公司,
DeptName as 部门, 
RealName as 联系人,
ServiceId as 客服Id,
salesId as 销售Id,
PayStatus as 付款状态,
Sum(SumMoney) as 总金额,
ChargeOff as 已付款,
SUM(SumMoney-ChargeOff) as 需付款,
CreditDays as 账期 
from View_Order where IsDelete=0 and RawOrderId=0 and PayStatus <> '已付款'
and StoreFinishTime>'2014-1-1'
and StoreStatus='已完成' and (ServiceId = {0} or salesId = {0}) and ComId = {1} group by DATEPART(YEAR,StoreFinishTime),DATEPART(MONTH,StoreFinishTime),CreditDays, DeptName,RealName,chargeoff,PayStatus,Company,ServiceId,salesId
order by 年份,月份",Id,comId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取待回款账单
        /// 调用：ERP：FReceivableReminder.cs
        ///  OrderPicking（订单拣货分配） OrderStatus（订单流程状态表）
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DataSet GetOverdueOrder(int Id)
        {
            string sql = string.Format(@"select COUNT(OrderId) as orderCount,
       Company,sum (SumMoney) as SumMoney, 
       sum(ChargeOff)as 已付款, 
       SUM(SumMoney-ChargeOff) as 需付款
       from View_Order 
       where IsDelete=0
       and RawOrderId=0
       and PayStatus <> '已付款'
       and StoreStatus='已完成' 
       and StoreFinishTime>'2014-1-1' 
       and (ServiceId = {0} or salesId = {0})
       group by Company 
       order by SumMoney desc", Id);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取待回款账单
        /// 调用：ERP：FReceivableReminder.cs       
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DataSet GetOverdueOrder(int Id,int comId)
        {
            string sql = string.Format(@"select COUNT(OrderId) as orderCount,
       Company,sum (SumMoney) as SumMoney,
       sum(ChargeOff)as 已付款,
       SUM(SumMoney-ChargeOff) as 需付款
       from View_Order 
       where IsDelete=0 
       and RawOrderId=0 
       and PayStatus <> '已付款'
       and StoreStatus='已完成' 
       and StoreFinishTime>'2014-1-1' 
       and (ServiceId = {0} or salesId = {0}) and ComId = {1}
       group by Company 
       order by SumMoney desc", Id,comId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 合计每个月份的账单
        /// </summary>
        /// <param name="comId"></param>
        /// <returns></returns>
        public DataSet GetDateOrder(int comId)
        {
            string sql = string.Format(@"select DATEPART( YEAR,StoreFinishTime) as 年份,
DATEPART( MONTH,StoreFinishTime) as 月份,
SUM(summoney-chargeoff) as 总金额,
CreditDays as 账期 
from View_Order where IsDelete=0 and RawOrderId=0 and PayStatus <> '已付款'
and StoreFinishTime>'2014-1-1'
and ComId={0}
and StoreStatus='已完成'
group by  DATEPART( YEAR,StoreFinishTime),DATEPART(MONTH,StoreFinishTime),CreditDays
order by 年份,月份 ", comId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 详细每个月份的账单
        /// </summary>
        /// <param name="comId"></param>
        /// <returns></returns>
        public DataSet GetOrderTime(int comId)
        {
            string sql = string.Format(@"select DATEPART( YEAR,StoreFinishTime) as 年份,
DATEPART(MONTH,StoreFinishTime) as 月份,
DeptName as 部门, 
RealName as 联系人,
PayStatus as 付款状态,
Sum(SumMoney) as 总金额,
ChargeOff as 已付款,
SUM(SumMoney-ChargeOff) as 需付款,
CreditDays as 账期 
from View_Order where IsDelete=0 and RawOrderId=0 and PayStatus <> '已付款'
and StoreFinishTime>'2014-1-1'
and ComId={0}
and StoreStatus='已完成'
group by DATEPART(YEAR,StoreFinishTime),DATEPART(MONTH,StoreFinishTime),CreditDays, DeptName,RealName,chargeoff,PayStatus
order by 年份,月份", comId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 显示一个月内账单的金额情况
        /// </summary>
        /// <param name="comId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public DataSet GetAccountDate(int comId, string startTime, string endTime)
        {
            string sql = string.Format(@"select DATEPART( YEAR,StoreFinishTime) as 年份,
DATEPART(MONTH,StoreFinishTime) as 月份,
DeptName as 部门, 
MemberId,
RealName as 联系人,
PayStatus as 付款状态,
Sum(SumMoney) as 总金额,
ChargeOff as 已付款,
SUM(SumMoney-ChargeOff) as 需付款,
CreditDays as 账期 from View_Order where IsDelete=0 and RawOrderId=0 and PayStatus <> '已付款'
and StoreStatus='已完成' and StoreFinishTime>='{1}' and StoreFinishTime<'{2}'
and ComId={0}
group by DATEPART( YEAR,StoreFinishTime),DATEPART(MONTH,StoreFinishTime),CreditDays, DeptName,MemberId,RealName,chargeoff,PayStatus", comId, startTime, endTime);
            return m_dbo.GetDataSet(sql);
        }


        /// <summary>
        /// 根据memberId和订单的时间(一个月内)显示此人的所有订单
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public DataSet GetAccountDateDept(int memberId, string startTime, string endTime)
        {
            string sql = string.Format(@"select OrderId,SumMoney,DeptName,RealName,PlanDate
from view_order where
MemberId ={0}  
and IsDelete=0 
and RawOrderId=0
and PayStatus <> '已付款'
and StoreStatus='已完成'
and StoreFinishTime>='{1}' 
and StoreFinishTime<'{2}' ", memberId, startTime, endTime);
            return m_dbo.GetDataSet(sql);
        }
    }
}
