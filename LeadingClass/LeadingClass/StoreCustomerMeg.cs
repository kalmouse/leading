using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class StoreCustomerMeg
    {
        public int Id { get; set; }
        public int GoodsId { get; set; }
        public string DisplayName { get; set; }
        public int Num { get; set; }
        public double Price { get; set; }
        public DateTime UpdateTime { get; set; }
        public int DeptId { get; set; }
        public int ComId { get; set; }
        public int SDeptId { get; set; }
        public int SMemberId { get; set; }
        public int Allow { get; set; }
        private DBOperate m_dbo;

       

        public StoreCustomerMeg()
        {
            Id = 0;
            GoodsId = 0;
            DisplayName = "";
            Num = 0;
            Price = 0;
            UpdateTime = DateTime.Now;
            DeptId = 0;
            ComId = 0;
            SDeptId = 0;
            SMemberId = 0;
            Allow = 0;
            m_dbo = new DBOperate();
            m_pageModel = new PageModel();//分页
        }
        
        //库存管理分页
        private string m_KeyWords;//根据关键字搜索
        public string KeyWords { get { return m_KeyWords; } set { m_KeyWords = value; } }
        private PageModel m_pageModel;
        public PageModel pageModel { get { return m_pageModel; } set { m_pageModel = value; } }
        

        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@GoodsId", GoodsId));
            arrayList.Add(new SqlParameter("@DisplayName", DisplayName));
            arrayList.Add(new SqlParameter("@Num", Num));
            arrayList.Add(new SqlParameter("@Price", Price));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@DeptId", DeptId));
            arrayList.Add(new SqlParameter("@ComId", ComId));
            arrayList.Add(new SqlParameter("@SDeptId", SDeptId));
            arrayList.Add(new SqlParameter("@SMemberId", SMemberId));
            arrayList.Add(new SqlParameter("@Allow", Allow));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("StoreCustomerMeg", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("StoreCustomerMeg", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from StoreCustomerMeg where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }
        public int LoadFromGoodsId()
        {
            int num = 0;
            string sql = string.Format("select * from StoreCustomerMeg where GoodsId={0}", GoodsId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                num = Num;
            }
            return num;
        }
        private void LoadFromRow(DataRow row)
        {
            Id = DBTool.GetIntFromRow(row, "Id", 0);
            GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
            DisplayName = DBTool.GetStringFromRow(row, "DisplayName", "");
            Num = DBTool.GetIntFromRow(row, "Num", 0);
            Price = DBTool.GetDoubleFromRow(row, "Price", 0);
            UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
            DeptId = DBTool.GetIntFromRow(row, "DeptId", 0);
            ComId = DBTool.GetIntFromRow(row, "ComId", 0);
            SDeptId = DBTool.GetIntFromRow(row, "SDeptId", 0);
            SMemberId = DBTool.GetIntFromRow(row, "SMemberId", 0);
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from StoreCustomerMeg where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        //出入库统计
        public DataSet showStoreDetial(int ComId)
        {
            string sql = string.Format(@" 
select GoodsId,DisplayName,SUM(Num) as num,scm.UpdateTime,sm.RealName,sd.SDeptName,Allow from  StoreCustomerMeg scm 
left join StoreMember sm on scm.SMemberId =sm.Id 
left join StoreDept sd on scm.SDeptId=sd.Id
where scm.Num>0 and scm.ComId={0}
group by GoodsId,DisplayName,scm.UpdateTime,sm.RealName,sd.SDeptName,Allow order by scm.UpdateTime desc,GoodsId;select @@ROWCOUNT as RowNum;

select GoodsId,DisplayName,SUM(Num) as num,scm.UpdateTime,sm.RealName,sd.SDeptName,Allow from  StoreCustomerMeg scm 
left join StoreMember sm on scm.SMemberId =sm.Id 
left join StoreDept sd on scm.SDeptId=sd.Id
where scm.Num<0 and scm.ComId={0}
group by GoodsId,DisplayName,scm.UpdateTime,sm.RealName,sd.SDeptName,Allow order by scm.UpdateTime desc,GoodsId;select @@ROWCOUNT as RowNum;

", ComId);
            return m_dbo.GetDataSet(sql, (pageModel.CurrentPage - 1) * pageModel.PageSize, pageModel.PageSize);
        }

        //库存查询
        public DataSet showStoreAll(int ComId)
        {
            string sql = string.Format(@" select GoodsId,DisplayName,SUM(Num) as num from  StoreCustomerMeg where ComId={0} group by GoodsId,DisplayName;select @@ROWCOUNT as RowNum; ",ComId);
            return m_dbo.GetDataSet(sql, (pageModel.CurrentPage - 1) * pageModel.PageSize, pageModel.PageSize);
        }
        //入库商品
        public DataSet ShowInStore(int ComId)
        {
            string sql = string.Format(@"select GoodsId,DisplayName,SUM(Num) as num,scm.UpdateTime,sm.RealName,sd.SDeptName,Allow from  StoreCustomerMeg scm 
left join StoreMember sm on scm.SMemberId =sm.Id 
left join StoreDept sd on scm.SDeptId=sd.Id
where scm.Num>0 and scm.ComId={0}
group by GoodsId,DisplayName,scm.UpdateTime,sm.RealName,sd.SDeptName,Allow order by scm.UpdateTime desc,GoodsId",ComId);
            return m_dbo.GetDataSet(sql);
        }
        //出库领用商品
        public DataSet ShowOutStore(int ComId)
        {
            string sql = string.Format(@"select GoodsId,DisplayName,SUM(Num) as num,scm.UpdateTime,sm.RealName,sd.SDeptName,Allow from  StoreCustomerMeg scm 
left join StoreMember sm on scm.SMemberId =sm.Id 
left join StoreDept sd on scm.SDeptId=sd.Id
where scm.Num<0 and scm.ComId={0}
group by GoodsId,DisplayName,scm.UpdateTime,sm.RealName,sd.SDeptName,Allow order by scm.UpdateTime desc,GoodsId",ComId);
            return m_dbo.GetDataSet(sql);
        }
        //审核商品领用
        public DataSet checkGoodsUsing(int ComId)
        {
            string sql = string.Format(@" select scm.Id,GoodsId,DisplayName,scm.Num,scm.UpdateTime,sm.RealName,sd.SDeptName,Allow from  StoreCustomerMeg scm  
left join StoreMember sm on scm.SMemberId =sm.Id 
left join StoreDept sd on scm.SDeptId=sd.Id 
where scm.Num<0 and Allow=0 and scm.ComId={0} order by scm.UpdateTime desc",ComId);
            return m_dbo.GetDataSet(sql);
        }

        
    }
    //操作数据类
    public class StoreCusOptipon
    {
        private DBOperate m_dbo;
        private StoreCustomerMeg m_option;
        public StoreCustomerMeg option { get { return m_option; } set { m_option = value; } }

        public StoreCusOptipon()
        {
            m_option = new StoreCustomerMeg();
            m_dbo = new DBOperate();
        }
        private string GetSQLWhere()
        {
            string sql = "";
            if (option.SDeptId > 0)
            {
                sql += string.Format(" and SDeptId={0} ", option.SDeptId);
            }
            if (option.SMemberId > 0)
            {
                sql += string.Format(" and SMemberId={0} ", option.SMemberId);
            }
            if (option.GoodsId > 0)
            {
                sql += string.Format(" and GoodsId ={0} ", option.GoodsId);
            }
            if (option.DisplayName != "")
            {
                sql += string.Format(" and DisplayName like '%{0}%' ", option.DisplayName);
            }
            return sql;
        }
        //领用结果查询
        public DataSet searchGoodsUsing()
        {
            string sql = string.Format(@"select scm.Id,GoodsId,DisplayName,scm.Num,scm.UpdateTime,sm.RealName,sd.SDeptName,sd.Id as SDeptId,sm.Id as SMemberId,Allow from  StoreCustomerMeg scm  
left join StoreMember sm on scm.SMemberId =sm.Id 
left join StoreDept sd on scm.SDeptId=sd.Id 
where scm.Num<0 and scm.ComId={0} ", option.ComId);
            sql += GetSQLWhere() + " order by scm.UpdateTime desc;select @@ROWCOUNT as RowNum";
            return m_dbo.GetDataSet(sql);
        }
        
    }
}
