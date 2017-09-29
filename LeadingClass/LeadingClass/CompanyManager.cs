//2017-4-18 yanghaiyang   需求008
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class CompanyOption
    {
        private int m_TypeId;
        private int m_SalesId;
        private string m_Name;
        private string m_MemberName;
        private string m_Address;
        private string m_Telphone;
        private int m_BranchId;
        public string m_RegisterMethod;
        public int m_ComId;
        public int TypeId { get { return m_TypeId; } set { m_TypeId = value; } }
        public int SalesId { get { return m_SalesId; } set { m_SalesId = value; } }
        public string Name { get { return m_Name; } set { m_Name = value; } }
        public string MemberName { get { return m_MemberName; } set { m_MemberName = value; } }
        public string Address { get { return m_Address; } set { m_Address = value; } }
        public string Telphone { get { return m_Telphone; } set { m_Telphone = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public string RegisterMethod { get { return m_RegisterMethod; } set { m_RegisterMethod = value; } }
        public int ComId { get { return m_ComId; } set { m_ComId = value; } }
        public CompanyOption()
        {
            m_TypeId = 0;
            m_SalesId = 0;
            m_Name = "";
            m_MemberName = "";
            m_Address = "";
            m_Telphone = "";
            m_BranchId = 0;//按照加盟商 lin2015-4-30
            m_RegisterMethod = "";//注册方式
            m_ComId = 0;
        }
    }
    public class CompanyManager
    {
        private CompanyOption m_option;
        private DBOperate m_dbo;
        public CompanyOption option { get { return m_option; } set { m_option = value; } }

        public CompanyManager()
        {
            m_option = new CompanyOption();
            m_dbo = new DBOperate();
        }

        public DataSet ReadCompany()
        {
            string sql = " select * from view_company where 1=1 ";
            if (option.BranchId > 0)
            {
                sql += string.Format(" and BranchId={0}", option.BranchId);
            }
            if (option.Address != "")
            {
                sql += string.Format(" and Address like '%{0}%' ", option.Address);
            }
            if (option.MemberName != "")
            {
                sql += string.Format(" and ComId in ( select ComId from Member where RealName = '{0}' ) ", option.MemberName.Trim());
            }
            if (option.Name != "")
            {
                sql += string.Format(" and Name like '%{0}%' ", option.Name);
            }
            if (option.SalesId != 0)
            {
                sql += string.Format(" and SalesId = {0} ", option.SalesId);
            }
            if (option.Telphone != "")
            {
                sql += string.Format(" and ComId in (  select ComId from Member where Telphone like '%{0}%'  or Mobile like '%{0}%' ) ", option.Telphone);
            }
            if (option.TypeId != 0)
            {
                sql += string.Format(" and TypeId={0} ", option.TypeId);
            }
            if (option.RegisterMethod != "")
            {
                sql += string.Format(" and RegisterMethod={0}", option.RegisterMethod);
            }
            if (option.ComId > 0)
            {
                sql += string.Format(" and ComId={0}", option.ComId);
            }
            sql += " order by ComPath,Name ";
            return m_dbo.GetDataSet(sql);
        }
       
        /// <summary>
        /// 按照分站读取公司客户
        /// </summary>
        /// <param name="lastUpdateTime"></param>
        /// <param name="BranchId"></param>
        /// <returns></returns>
        public DataSet ReadCompanyCache(int BranchId)
        {
            string sql = string.Format(" select ComId,Name,Company.UpdateTime ,SalesId,ServiceId,PY,Customer.Id as CustomerId,IsBanningOrders,StartOrder,PayType,InvoiceType,TaxRate,OrderMemo from Company left join Customer on Company.Id =Customer.ComId where  1=1  ");
            if (BranchId > 0)
            {
                sql += string.Format(" and branchid={0} ", BranchId);
            }
            sql += string.Format(" order by Compath,Name ");
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 检查公司名称是否已经存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int IsCompanyExsist(string name)
        {
            string sql = string.Format(" select * from Company where name = '{0}' ", name.Replace("(", "（").Replace(")", "）"));
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DBTool.GetIntFromRow(ds.Tables[0].Rows[0], "Id", 0);
            }
            else return 0;
        }
        /// <summary>
        /// 读取客户采购模式
        /// </summary>
        /// <returns></returns>
        public DataSet ReadCustomerModel()
        {
            string sql = " select * from CustomerModel  order by Id ";
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取公司已购商品（尽美平台缓存用数据）
        /// </summary>
        /// <param name="comId"></param>
        /// <returns></returns>
        public DataSet ReadBoughtGoodsCacheData(int comId)
        {
            string sql = string.Format("select distinct GoodsId as ID,'【'+convert(varchar(10),Recommend)+'】'+ convert(varchar(10),GoodsId)+' '+DisplayName as DisplayName,PY,Recommend,0 as Num,ParentId,IsShelves from view_orderDetail where comId={0}   UNION ALL select distinct g.ID,'【'+convert(varchar(10),g.Recommend)+'】'+ convert(varchar(10),g.ID)+' '+g.DisplayName as DisplayName,g.PY,g.Recommend,0 as Num,g.ParentId,g.IsShelves from Goods g left join  view_orderDetail vv on vv.GoodsId=g.ParentId where vv.ParentId =2 and vv.ComId={0} order by Recommend desc,DisplayName ", comId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取公司采购商品的明细
        /// 调用：ERP：FComGoods.cs
        /// </summary>
        /// <param name="comId"></param>
        /// <param name="keywords"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public DataSet ReadComGoods(int branchId,int comId, string keywords, DateTime startdate, DateTime enddate)
        {
            string sql = "select GoodsId,DisplayName,Model,Num,Unit,SalePrice, OrderId,PlanDate,DeptName,RealName from View_OrderDetail where IsInner=0 and IsDelete=0 and RawOrderId=0";
            sql += string.Format(" and plandate >= '{0}' and plandate < '{1}' and branchId= {2}  ", startdate.ToShortDateString(), enddate.ToShortDateString(),branchId);
            if (comId > 0)
            {
                sql += string.Format(" and ComId = {0} ", comId.ToString());
            }

            if (keywords != "")
            {
                sql += string.Format(" and  DisplayName like '%{0}%' ", keywords);
            }
            sql += "  order by DisplayName ";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// ERP FCompanyInfo.cs中用到 
        /// </summary>
        /// <param name="comId"></param>
        /// <returns></returns>
        public DataSet ReadCompanyModel(int comId)
        {
            string sql = string.Format(@"select SalesId,(select Name from Sys_Users where Id = SalesId) SalesName,ServiceId,(select Name from Sys_Users where Id = ServiceId) ServiceName,
	                                            StatementManId ,(select Name from Sys_Users where Id = StatementManId) StatementManName,
	                                            (select Address from Company where Id = ComId) Address from Customer where ComId = {0} ;
                                         select (select Name from Dept where Id = DeptId) DeptName,RealName,Telphone,Mobile,Address from  Member where ComId = {0};
                                         select CounterId from VIPCounterCompany where ComId={0} order by Id desc;
                                         select COUNT(*) from [Order] where IsInner=0 and IsDelete=0 and RawOrderId=0 and ComId = {0}", comId);
            return m_dbo.GetDataSet(sql);
        }
    }
   
}
