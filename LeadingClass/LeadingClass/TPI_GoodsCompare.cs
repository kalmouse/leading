using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class TPI_GoodsCompare
    {
        public int Id { get; set; }
        public int HNSCGoodsId { get; set; }
        public string LBBH { get; set; }
        public string LBMC { get; set; }
        public string PMBH { get; set; }
        public string PMMC { get; set; }
        public string PPBH { get; set; }
        public string PPMC { get; set; }
        public string XHBH { get; set; }
        public string XHMC { get; set; }
        public string Xyghbh { get; set; }
        public int GoodsId { get; set; }
        public DateTime UpdateTime { get; set; }
        public decimal Price { get; set; }
        public decimal sjjg { get; set; }
        public int IsInStore { get; set; }
        public int State { get; set; }
        public int IsShelves { get; set; }
        public string Note { get; set; }
        public string Reason { get; set; }
        
        private DBOperate m_dbo;

        public TPI_GoodsCompare()
        {
            Id = 0;
            HNSCGoodsId = 0;
            LBBH = "";
            LBMC = "";
            PMBH = "";
            PMMC = "";
            PPBH = "";
            PPMC = "";
            XHBH = "";
            XHMC = "";
            Xyghbh = "";
            GoodsId = 0;
            UpdateTime = DateTime.Now;
            Price = 0;
            sjjg = 0;
            IsInStore = 0;
            State = 1;
            IsShelves = 1;
            Note = "";
            Reason = "";
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }

            arrayList.Add(new SqlParameter("@HNSCGoodsId", HNSCGoodsId));
            arrayList.Add(new SqlParameter("@LBBH", LBBH));
            arrayList.Add(new SqlParameter("@LBMC", LBMC));
            arrayList.Add(new SqlParameter("@PMBH", PMBH));
            arrayList.Add(new SqlParameter("@PMMC", PMMC));
            arrayList.Add(new SqlParameter("@PPBH", PPBH));
            arrayList.Add(new SqlParameter("@PPMC", PPMC));
            arrayList.Add(new SqlParameter("@XHBH", XHBH));
            arrayList.Add(new SqlParameter("@XHMC", XHMC));
            arrayList.Add(new SqlParameter("@Xyghbh", Xyghbh));
            arrayList.Add(new SqlParameter("@GoodsId", GoodsId));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@Price", Price));
            arrayList.Add(new SqlParameter("@sjjg", sjjg));
            arrayList.Add(new SqlParameter("@IsInStore", IsInStore));
            arrayList.Add(new SqlParameter("@State", State));
            arrayList.Add(new SqlParameter("@IsShelves", IsShelves));          
            arrayList.Add(new SqlParameter("@Note", Note));
            arrayList.Add(new SqlParameter("@Reason", Reason));
            

            if (this.Id > 0)
            {
                m_dbo.UpdateData("TPI_GoodsCompare", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("TPI_GoodsCompare", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from TPI_GoodsCompare where 1=1");
            if (GoodsId > 0)
            {
                sql += string.Format(" and GoodsId={0}", GoodsId);
            }
            if (Id > 0)
            {
                sql += string.Format(" and Id={0}", Id);
            }
            if (XHMC != "" && XHMC!=null) 
            {
                sql += string.Format("and XHMC='{0}'", XHMC);
            }
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                HNSCGoodsId = DBTool.GetIntFromRow(row, "HNSCGoodsId",0);
                LBBH = DBTool.GetStringFromRow(row, "LBBH", "");
                LBMC = DBTool.GetStringFromRow(row, "LBMC", "");
                PMBH = DBTool.GetStringFromRow(row, "PMBH", "");
                PMMC = DBTool.GetStringFromRow(row, "PMMC", "");
                PPBH = DBTool.GetStringFromRow(row, "PPBH", "");
                PPMC = DBTool.GetStringFromRow(row, "PPMC", "");
                XHBH = DBTool.GetStringFromRow(row, "XHBH", "");
                XHMC = DBTool.GetStringFromRow(row, "XHMC", "");
                Xyghbh = DBTool.GetStringFromRow(row, "Xyghbh", "");
                GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                IsInStore = DBTool.GetIntFromRow(row, "IsInStore", 0);
                State = DBTool.GetIntFromRow(row, "State", 0);
                IsShelves = DBTool.GetIntFromRow(row, "IsShelves", 0);
                Note = DBTool.GetStringFromRow(row, "Note", "");
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                Price = DBTool.GetDecimalFromRow(row, "Price", 0);
                sjjg = DBTool.GetDecimalFromRow(row, "sjjg", 0);
                Reason = DBTool.GetStringFromRow(row, "Reason", "");
                return true;
            }
            return false;
        }
        public TPI_GoodsCompare(string XHMC)
        {
            m_dbo = new DBOperate();
            this.XHMC = XHMC;
            this.Load();
        }
        public TPI_GoodsCompare(int goodsId)
        {
            m_dbo = new DBOperate();
            this.GoodsId = goodsId;
            this.Load();
        
        }
        /// <summary>
        ///取消商品对应
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public bool  Delete(int[] Id)
        {
            string sql = string.Format("Delete from TPI_GoodsCompare where 1=1");
            if (Id != null)
            {
                sql += " and Id in ( ";
                for (int i = 0; i < Id.Length; i++)
                {
                    if (i == 0)
        {
                        sql += string.Format(" '{0}' ", Id[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", Id[i]);
                }
                sql += " ) ";
            }
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 入库商品
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public DataSet GetAllTPI_Goods(int[] goodsId)
        {
            string sql = "select * from dbo.TPI_GoodsCompare where 1=1";
            if (goodsId != null)
            {
                sql += "and GoodsId in (";
                for (int i = 0; i < goodsId.Length; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", goodsId[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", goodsId[i]);
                }
                sql += " ) ";
            }
            return m_dbo.GetDataSet(sql);
        }
        //读取对应表数据
        public bool ModifyIsInStoreByGoodsId(int goodsId) 
        {
            string sql = string.Format("update TPI_GoodsCompare set Isinstore = 1 where GoodsId = {0}",goodsId);
            return m_dbo.ExecuteNonQuery(sql);
        }
        public DataSet ReadGoods(int goodsid) 
        {
            string sql = string.Format("select * from TPI_GoodsCompare where GoodsId={0}", goodsid);
            return m_dbo.GetDataSet(sql);
        }
    }
}
