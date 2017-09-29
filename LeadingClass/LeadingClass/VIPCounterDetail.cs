using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class VIPCounterDetail
    {
        public int Id { get; set; }
        public int CounterId { get; set; }
        public int GoodsId { get; set; }
        public double VIPPrice { get; set; }
        public string Remark { get; set; }
        public DateTime ExportDate { get; set; }
        public int Level { get; set; }
        public int IsSales { get; set; }
        public int IsSpecial { get; set; }//是否是特制的商品    默认不是 
        private DBOperate m_dbo;

        public VIPCounterDetail()
        {
            Id = 0;
            CounterId = 0;
            GoodsId = 0;
            VIPPrice = 0;
            Remark = "";
            ExportDate = Convert.ToDateTime("1900-01-01 00:00:00.000");
            Level = 1;
            IsSales = 1;
            IsSpecial = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@CounterId", CounterId));
            arrayList.Add(new SqlParameter("@GoodsId", GoodsId));
            arrayList.Add(new SqlParameter("@VIPPrice", VIPPrice));
            arrayList.Add(new SqlParameter("@Remark", Remark));
            arrayList.Add(new SqlParameter("@ExportDate", ExportDate));
            arrayList.Add(new SqlParameter("@Level", Level));
            arrayList.Add(new SqlParameter("@IsSales", IsSales));
            arrayList.Add(new SqlParameter("@IsSpecial", IsSpecial));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("VIPCounterDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("VIPCounterDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load(int counterId,int goodsId)
        {
            string sql = string.Format("select * from VIPCounterDetail where CounterId={0} and  GoodsId={1}", counterId, goodsId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                CounterId = DBTool.GetIntFromRow(row, "CounterId", 0);
                GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                VIPPrice = DBTool.GetDoubleFromRow(row, "VIPPrice", 0);
                Remark = DBTool.GetStringFromRow(row, "Remark", "");
                ExportDate = DBTool.GetDateTimeFromRow(row, "ExportDate");
                Level = DBTool.GetIntFromRow(row, "Level", 0);
                IsSales = DBTool.GetIntFromRow(row, "IsSales", 0);
                IsSpecial = DBTool.GetIntFromRow(row, "IsSpecial", 0);
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format(" delete from VIPCounterDetail where Id={0} ", this.Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 通过GoodId和ParentId 返回vip专柜关联商品（母商品与子商品）
        /// </summary>
        /// <param name="CounterId"></param>
        /// <param name="GoodsId"></param>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public DataSet GetVIPCounterDetail(int CounterId, int GoodsId, int ParentId)//2015-8-28 陈
        {
            StringBuilder stb = new StringBuilder();
            if (ParentId > 2)
            {
                stb.Append(string.Format(@"select * from  View_VIPCounterDetail where CounterId={0} and GoodsId ={1};  
select * from View_VIPCounterDetail where CounterId={0} and GoodsId ={2}", CounterId, GoodsId, ParentId));
            }
            else
            {
                stb.Append(string.Format(@"select  * from  View_VIPCounterDetail where CounterId={0} and GoodsId ={1} ;
select * from View_VIPCounterDetail where CounterId={0} and parentId={1};", CounterId, GoodsId));
            }
            return m_dbo.GetDataSet(stb.ToString());
        }

        public double ReadVIPrice(int CounterId, int GoodsId)
        {
            string sql = string.Format(" select VIPPrice from VIPCounterDetail where CounterId={0} and GoodsId={1} ", CounterId, GoodsId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return DBTool.GetDoubleFromRow(ds.Tables[0].Rows[0], "VIPPrice", 0);
            }
            else return -1;
        }

        /// <summary>
        /// 商品是否销售
        /// </summary>
        /// <param name="counterId"></param>
        /// <param name="goodsId"></param>
        /// <param name="isSales"></param>
        /// <returns></returns>
        public bool UpdateSales(int vcdId, int isSales)
        {
            string sql = string.Format("update VIPCounterDetail set IsSales={1} where Id={0}", vcdId, isSales);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 商品是否专区
        /// </summary>
        /// <param name="counterId"></param>
        /// <param name="goodsId"></param>
        /// <param name="isSales"></param>
        /// <returns></returns>
        public bool UpdateSpecial(int vcdId, int isSpecial)
        {
            string sql = string.Format("update VIPCounterDetail set IsSpecial={1} where Id={0}", vcdId, isSpecial);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}
