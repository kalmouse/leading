using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Data;

namespace LeadingClass
{
    public class TransferDetail
    {
        public int Id { get; set; }
        public int TransferId { get; set; }
        public int GoodsId { get; set; }
        public string Model { get; set; }
        public string Unit { get; set; }
        public double AC { get; set; }
        public int Num { get; set; }
        public double Amount { get; set; }
        public string Memo { get; set; }
        public DateTime UpdateDate { get; set; }
        private DBOperate m_dbo;

        public TransferDetail()
        {
            Id = 0;
            TransferId = 0;
            GoodsId = 0;
            Model = "";
            Unit = "";
            AC = 0;
            Num = 0;
            Amount = 0;
            Memo = "";
            UpdateDate = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@TransferId", TransferId));
            arrayList.Add(new SqlParameter("@GoodsId", GoodsId));
            arrayList.Add(new SqlParameter("@Model", Model));
            arrayList.Add(new SqlParameter("@Unit", Unit));
            arrayList.Add(new SqlParameter("@AC", AC));
            arrayList.Add(new SqlParameter("@Num", Num));
            arrayList.Add(new SqlParameter("@Amount", Amount));
            arrayList.Add(new SqlParameter("@Memo", Memo));
            arrayList.Add(new SqlParameter("@UpdateDate", UpdateDate));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("TransferDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("TransferDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from TransferDetail where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                TransferId = DBTool.GetIntFromRow(row, "TransferId", 0);
                GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                Model = DBTool.GetStringFromRow(row, "Model", "");
                Unit = DBTool.GetStringFromRow(row, "Unit", "");
                AC = DBTool.GetDoubleFromRow(row, "AC", 0);
                Num = DBTool.GetIntFromRow(row, "Num", 0);
                Amount = DBTool.GetDoubleFromRow(row, "Amount", 0);
                Memo = DBTool.GetStringFromRow(row, "Memo", "");
                UpdateDate = DBTool.GetDateTimeFromRow(row, "UpdateDate");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from TransferDetail where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        public bool DeleteTransferId(int tId)
        {
            string sql = string.Format("Delete from TransferDetail where TransferId={0} ", tId);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 通过Id读取调拨单和调拨单的明细 quxiaoshan 2015-6-15
        /// </summary>
        /// <returns></returns>
        public DataSet ReadTransferDetail(int transferId)
        {
            string sql = string.Format(" select * from dbo.TransferDetail where TransferId={0}; ", transferId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取本站点的申请单明细 
        /// </summary>
        /// <returns></returns>
        public DataSet ReadTransferDetailByTransferId(int transferId)
        {
            string sql = string.Format(@"select dbo.[TransferDetail].*,dbo.Goods.DisplayName,dbo.Goods.Unit,dbo.Goods.Model,dbo.GoodsStore.AC  from dbo.TransferDetail
                                        join dbo.Goods on dbo.TransferDetail.GoodsId=dbo.Goods.ID
                                        join dbo.GoodsStore on dbo.GoodsStore.GoodsId=dbo.Goods.ID
                                        where transferId ={0} ", transferId);
            return m_dbo.GetDataSet(sql);
        }
    }

}
