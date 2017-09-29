using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace LeadingClass
{
    public  class GoodsStandingInventory
    {
        private int m_Id;
        private int m_BranchId;
        private int m_StoreId;
        private int m_GoodsId;
        private string m_Model;
        private int m_MinStock;
        private int m_MaxStock;
        private string m_Memo;
        private DateTime m_UpdateTime;
        private int m_UserId;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public int StoreId { get { return m_StoreId; } set { m_StoreId = value; } }
        public int GoodsId { get { return m_GoodsId; } set { m_GoodsId = value; } }
        public string Model { get { return m_Model; } set { m_Model = value; } }
        public int MinStock { get { return m_MinStock; } set { m_MinStock = value; } }
        public int MaxStock { get { return m_MaxStock; } set { m_MaxStock = value; } }
        public string Memo { get { return m_Memo; } set { m_Memo = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public GoodsStandingInventory()
        {
            m_Id = 0;
            m_BranchId = 1;
            m_StoreId = 1;
            m_GoodsId = 0;
            m_Model = "";
            m_MinStock = 0;
            m_MaxStock = 0;
            m_Memo = "";
            m_UpdateTime = DateTime.Now;
            m_UserId = 0;
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
            arrayList.Add(new SqlParameter("@StoreId", m_StoreId));
            arrayList.Add(new SqlParameter("@GoodsId", m_GoodsId));
            arrayList.Add(new SqlParameter("@Model", m_Model));
            arrayList.Add(new SqlParameter("@MinStock", m_MinStock));
            arrayList.Add(new SqlParameter("@MaxStock", m_MaxStock));
            arrayList.Add(new SqlParameter("@Memo", m_Memo));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            arrayList.Add(new SqlParameter("@UserId", m_UserId));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("GoodsStandingInventory", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("GoodsStandingInventory", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from GoodsStandingInventory where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                m_StoreId = DBTool.GetIntFromRow(row, "StoreId", 0);
                m_GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                m_Model = DBTool.GetStringFromRow(row, "Model", "");
                m_MinStock = DBTool.GetIntFromRow(row, "MinStock", 0);
                m_MaxStock = DBTool.GetIntFromRow(row, "MaxStock", 0);
                m_Memo = DBTool.GetStringFromRow(row, "Memo", "");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                return true;
            }
            return false;
        }

        public bool Delete() 
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Id", m_Id)
            };
            return  m_dbo.DeleteAt("GoodsStandingInventory",param);
        }

        

        public DataSet LoadStandingInventoryByGoodsId(int goodsId) 
        {
            string sql = string.Format("select * from dbo.GoodsStandingInventory where GoodsId={0}", goodsId);
            DataSet ds = m_dbo.GetDataSet(sql);
            return ds;
            //while (ds.Tables[0].Rows.Count > 0)
            //{
            //    DataRow row = new DataRow();// ds.Tables[0].Rows[0];
            //    m_Id = DBTool.GetIntFromRow(row, "Id", 0);
            //    m_BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
            //    m_StoreId = DBTool.GetIntFromRow(row, "StoreId", 0);
            //    m_GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
            //    m_Model = DBTool.GetStringFromRow(row, "Model", "");
            //    m_MinStock = DBTool.GetIntFromRow(row, "MinStock", 0);
            //    m_MaxStock = DBTool.GetIntFromRow(row, "MaxStock", 0);
            //    m_Memo = DBTool.GetStringFromRow(row, "Memo", "");
            //    m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
            //    m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
            //    ds.Tables[0].Rows.Add(row);
            //    return ds;
            //}
        }
    }


    /// <summary>
    /// add by quxiaoshan 2014-12-02 商品库存数量
    /// </summary>
    public class GoodsInventoryOption
    {
        private DateTime m_PlanDate;
        private string m_Model;
        private int m_PrepareDay;
        private int m_GoodsId;
        public DateTime PlanDate { get { return m_PlanDate; } set { m_PlanDate = value; } }
        public string Model { get { return m_Model; } set { m_Model = value; } }
        public int PrepareDay { get { return m_PrepareDay; } set { m_PrepareDay = value; } }
        public int GoodsId { get { return m_GoodsId; } set { m_GoodsId = value; } }
        public int BranchId { get; set; }
        public GoodsInventoryOption() 
        {
            m_PlanDate = DateTime.Parse("1900-01-01");
            m_Model = "";
            m_PrepareDay = 0;
            m_GoodsId = 0;
            BranchId = 1;
        }
    }

    public class GoodsInventoryManager
    {

        private DBOperate m_dbo;
        private GoodsInventoryOption m_option;
        public GoodsInventoryOption option { get { return m_option; } set { m_option = value; } }
        public GoodsInventoryManager()
        {
            m_dbo = new DBOperate();
            m_option = new GoodsInventoryOption();
        }
        /// <summary>
        /// 读取 相应天数 商品的销售数量
        /// </summary>
        /// <returns></returns>
        public DataSet ReadGoodsSaleNum()
        {
            string sql = string.Format("select  GoodsId,SUM(Num) as SaleNum from dbo.[Order] inner join dbo.OrderDetail on dbo.[Order].Id=dbo.OrderDetail.OrderId where 1=1");

            if (option.GoodsId > 0)
            {
                sql += string.Format(" and dbo.OrderDetail.GoodsId={0}",option.GoodsId);
            }
            if (option.PlanDate > DateTime.Parse("1900-01-01"))
            {
                sql += string.Format(" and PlanDate >'{0}' and PlanDate<'{1}'", option.PlanDate.AddDays(-option.PrepareDay), option.PlanDate);
            }
            if (option.Model != "")
            {
                sql += string.Format(" and model='{0}' ", option.Model);
            }
            sql += " group by dbo.OrderDetail.GoodsId order by SaleNum desc";
            return m_dbo.GetDataSet(sql);
        }


        public DataSet ReadGoodsStandingInventory()
        {
            string sql = string.Format(@"select * ,
(select SUM(Num) from View_GoodsStore gs where gs.GoodsId=vgsi.GoodsId  and IsAvalible=1 and BranchId={0} ) as 可用库存,
(select SUM(Num) from View_GoodsStore gs where gs.GoodsId=vgsi.GoodsId  and IsAvalible=0 and BranchId={0} ) as 可调库存,
(select SUM(num) from View_OrderDetail vod where vod.GoodsId=vgsi.GoodsId  and BranchId={0} 
	and StoreStatus <> '已完成'  and Num >0 and IsDelete=0 and IsInner=0 and RawOrderId=0 ) as 预售数量
from view_GoodsStandingInventory vgsi
where BranchId={0} ", option.BranchId);
            if(option.GoodsId >0)
            {
                sql += string.Format(" and GoodsId={0} ",option.GoodsId);
            }
            sql += " order by DisplayName ";
            return m_dbo.GetDataSet(sql);
        }
    }
}
