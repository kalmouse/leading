using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
namespace LeadingClass
{
    public class GoodsAC
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int GoodsId { get; set; }
        public double Goods_AC { get; set; }
        public int UserId { get; set; }
        public DateTime GoodsACTime { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public GoodsAC()
        {
            Id = 0;
            BranchId = 0;
            GoodsId = 0;
            Goods_AC = 0;
            UserId = 0;
            GoodsACTime = DateTime.Now;
            UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@BranchId", BranchId));
            arrayList.Add(new SqlParameter("@GoodsId", GoodsId));
            arrayList.Add(new SqlParameter("@Goods_AC", Goods_AC));
            arrayList.Add(new SqlParameter("@UserId", UserId));
            arrayList.Add(new SqlParameter("@GoodsACTime", GoodsACTime));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("GoodsAC", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("GoodsAC", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from GoodsAC where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                Goods_AC = DBTool.GetDoubleFromRow(row, "Goods_AC", 0);
                UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                GoodsACTime = DBTool.GetDateTimeFromRow(row, "GoodsACTime");
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Load(int BranchId, int GoodsId)
        {
            string sql = string.Format("select * from GoodsAC where BranchId={0} and GoodsId={1}", BranchId,GoodsId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                Goods_AC = DBTool.GetDoubleFromRow(row, "Goods_AC", 0);
                UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                GoodsACTime = DBTool.GetDateTimeFromRow(row, "GoodsACTime");
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;

        }
        public bool Delete()
        {
            string sql = string.Format("Delete from GoodsAC where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        public DataSet GoodsACList(int Goodsid)
        {
            GoodsAC goodsac = new GoodsAC();
            string sql = string.Format("select * from View_GoodsAC where GoodsId={0}", Goodsid);
            return m_dbo.GetDataSet(sql);

        }

        public DataSet GoodsACList()
        {
            GoodsAC goodsac = new GoodsAC();
            string sql = "select * from View_GoodsAC";
            return m_dbo.GetDataSet(sql);
        }
    }
    public class GoodsACModify
    {
        public int Id { get; set; }
        public int GoodsACId { get; set; }
        public double OldAC { get; set; }
        public double NewAC { get; set; }
        public int UserId { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public GoodsACModify()
        {
            Id = 0;
            GoodsACId = 0;
            OldAC = 0;
            NewAC = 0;
            UserId = 0;
            UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@GoodsACId", GoodsACId));
            arrayList.Add(new SqlParameter("@OldAC", OldAC));
            arrayList.Add(new SqlParameter("@NewAC", NewAC));
            arrayList.Add(new SqlParameter("@UserId", UserId));
            arrayList.Add(new SqlParameter("@UpdateTime", DateTime.Now));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("GoodsACModify", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("GoodsACModify", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from GoodsACModify where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                GoodsACId = DBTool.GetIntFromRow(row, "GoodsACId", 0);
                OldAC = DBTool.GetDoubleFromRow(row, "OldAC", 0);
                NewAC = DBTool.GetDoubleFromRow(row, "NewAC", 0);
                UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from GoodsACModify where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}
