using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CommenClass;

namespace LeadingClass
{
    
    public class OrderFeedBackOption
    {
        private int m_OrderIdS;
        private int m_OrderIdE;
        private int m_ComId;
        private DateTime m_PlanDateS;
        private DateTime m_PlanDateE;
        private string m_DeliveryStatus;
        private string m_FeedBackStatus;
        private int m_BranchId;
        public int ComId { get { return m_ComId; } set { m_ComId = value; } }
        public int OrderIdS { get { return m_OrderIdS; } set { m_OrderIdS = value; } }
        public int OrderIdE { get { return m_OrderIdE; } set { m_OrderIdE = value; } }
        public DateTime PlanDateS { get { return m_PlanDateS; } set { m_PlanDateS = value; } }
        public DateTime PlanDateE { get { return m_PlanDateE; } set { m_PlanDateE = value; } }
        public string DeliveryStatus { get { return m_DeliveryStatus; } set { m_DeliveryStatus = value; } }
        public string FeedBackStatus { get { return m_FeedBackStatus; } set { m_FeedBackStatus = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
    }

    /// <summary>
    /// 读取未完成的回单数据
    /// </summary>
    public  class OrderFeedBackManager
    {
        private DBOperate m_dbo;
        private OrderFeedBackOption m_option;
        public OrderFeedBackOption option { get { return m_option; } set { m_option = value; } }
        public OrderFeedBackManager()
        {
            m_dbo = new DBOperate();
            m_option = new OrderFeedBackOption();
        }
        /// <summary>
        /// 查询回单数据，1订单号，2订单范围，3订单日期范围，4公司名称
        /// </summary>
        /// <returns></returns>
        public DataSet ReadOrdersFeedBack()
        {
            string sql = "select * from View_Order where IsDelete=0 ";
            if (option.BranchId != 0)
            {
                sql += string.Format(" and BranchId={0} ", option.BranchId);
            }
            if (option.OrderIdS > 0 && option.OrderIdE == 0)
            {
                sql += string.Format(" and orderId={0} ", option.OrderIdS);
            }
            else
            {
                sql += " and FeedBackStatus <> '已完成' ";
                if (option.OrderIdS >= 0)
                {
                    sql += string.Format(" and OrderId>={0} ", option.OrderIdS);
                }
                if (option.OrderIdE > 0)
                {
                    sql += string.Format(" and OrderId<={0} ", option.OrderIdE);
                }
                if (option.ComId > 0)
                {
                    sql += string.Format(" and ComId={0} ", option.ComId);
                }
                if (option.PlanDateS > new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and PlanDate >= '{0}' ", option.PlanDateS.ToShortDateString());
                }
                if (option.PlanDateE > new DateTime(1900, 1, 1))
                {
                    sql += string.Format(" and PlanDate < '{0}' ", option.PlanDateE.AddDays(1).ToShortDateString());

                }
                sql += " order by OrderId ";
            }
           
            return m_dbo.GetDataSet(sql);
        }
    }
}
