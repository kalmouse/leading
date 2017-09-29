using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class TPI_HNSCGoods
    {
        public int Id { get; set; }
        public string LBBH { get; set; }
        public string LBMC { get; set; }
        public string PMBH { get; set; }
        public string PMMC { get; set; }
        public string PPBH { get; set; }
        public string PPMC { get; set; }
        public string XHBH { get; set; }
        public string XHMC { get; set; }
        public string Xyghbh { get; set; }
        public DateTime AddTime { get; set; }
        private DBOperate m_dbo;

        public TPI_HNSCGoods()
        {
            Id = 0;
            LBBH = "";
            LBMC = "";
            PMBH = "";
            PMMC = "";
            PPBH = "";
            PPMC = "";
            XHBH = "";
            XHMC = "";
            Xyghbh = "";
            AddTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@LBBH", LBBH));
            arrayList.Add(new SqlParameter("@LBMC", LBMC));
            arrayList.Add(new SqlParameter("@PMBH", PMBH));
            arrayList.Add(new SqlParameter("@PMMC", PMMC));
            arrayList.Add(new SqlParameter("@PPBH", PPBH));
            arrayList.Add(new SqlParameter("@PPMC", PPMC));
            arrayList.Add(new SqlParameter("@XHBH", XHBH));
            arrayList.Add(new SqlParameter("@XHMC", XHMC));
            arrayList.Add(new SqlParameter("@Xyghbh", Xyghbh));
            arrayList.Add(new SqlParameter("@AddTime", AddTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("TPI_HNSCGoods", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("TPI_HNSCGoods", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from TPI_HNSCGoods where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                LBBH = DBTool.GetStringFromRow(row, "LBBH", "");
                LBMC = DBTool.GetStringFromRow(row, "LBMC", "");
                PMBH = DBTool.GetStringFromRow(row, "PMBH", "");
                PMMC = DBTool.GetStringFromRow(row, "PMMC", "");
                PPBH = DBTool.GetStringFromRow(row, "PPBH", "");
                PPMC = DBTool.GetStringFromRow(row, "PPMC", "");
                XHBH = DBTool.GetStringFromRow(row, "XHBH", "");
                XHMC = DBTool.GetStringFromRow(row, "XHMC", "");
                Xyghbh = DBTool.GetStringFromRow(row, "Xyghbh", "");
                AddTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public TPI_HNSCGoods(int Id)
        {
            m_dbo = new DBOperate();
            this.Id = Id;
            this.Load();
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from TPI_HNSCGoods where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 河南商城商品对应
        /// </summary>
        /// <returns></returns>
        public DataSet GoodsCorresponding(string PPMC, string XHMC)
        {
            string sql = string.Format(@" select * from TPI_HNSCGoods inner join ( select Brand.Name,t.SN,t.ID,t.Price from Brand right join ( select ID,SN,Price,BrandId from Goods where (ParentId=0 or ParentId >2) and (SN=replace('{0}',' ','')or SN= replace('{0}','','')))
 t on Brand.Id = t.BrandId ) h on TPI_HNSCGoods.XHMC = '{0}' and TPI_HNSCGoods.PPMC = '{1}'",XHMC,PPMC);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取河南商品库商品
        /// </summary>
        /// <returns></returns>
        public DataSet Read_goods(string pmmc,string ppmc,string xhmc) 
        {
            string sql = string.Format("select * from  TPI_HNSCGoods where  PMMC ='{0}' and  PPMC ='{1}' and XHMC ='{2}'",pmmc,ppmc,xhmc);
            return m_dbo.GetDataSet(sql);
        }
    }

}
