using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;


namespace LeadingClass
{
    public class GoodsBarCode
    {
        private int m_Id;
        private int m_GoodsId;
        private string m_BarCode;
        private string m_Model;
        private int m_Count;
        private string m_Unit;
        private int m_UserId;
        private DateTime m_UpdateTime;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int GoodsId { get { return m_GoodsId; } set { m_GoodsId = value; } }
        public string BarCode { get { return m_BarCode; } set { m_BarCode = value; } }
        public string Model { get { return m_Model; } set { m_Model = value; } }
        public int Count { get { return m_Count; } set { m_Count = value; } }
        public string Unit { get { return m_Unit; } set { m_Unit = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public GoodsBarCode()
        {
            m_Id = 0;
            m_GoodsId = 0;
            m_BarCode = "";
            m_Model = "";
            m_Count = 0;
            m_Unit = "";
            m_UserId = 0;
            m_UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@GoodsId", m_GoodsId));
            arrayList.Add(new SqlParameter("@BarCode", m_BarCode));
            arrayList.Add(new SqlParameter("@Model", m_Model));
            arrayList.Add(new SqlParameter("@Count", m_Count));
            arrayList.Add(new SqlParameter("@Unit", m_Unit));
            arrayList.Add(new SqlParameter("@UserId", m_UserId));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("GoodsBarCode", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("GoodsBarCode", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from GoodsBarCode where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                m_BarCode = DBTool.GetStringFromRow(row, "BarCode", "");
                m_Model = DBTool.GetStringFromRow(row, "Model", "");
                m_Count = DBTool.GetIntFromRow(row, "Count", 0);
                m_Unit = DBTool.GetStringFromRow(row, "Unit", "");
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }

        public DataSet ReadGoodsBarCode(int GoodsId)
        {
            string sql = string.Format("select * from GoodsBarCode where GoodsId={0}", GoodsId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 如果 条形码 存在 返回商品的ID
        /// </summary>
        /// <param name="BarCode"></param>
        /// <returns></returns>
        public GoodsBarCode CheckBarCode(string BarCode)
        {
            string sql = string.Format(" select * from GoodsBarCode where BarCode ='{0}' ", BarCode);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                this.Id = DBTool.GetIntFromRow(ds.Tables[0].Rows[0], "Id", 0);
                this.Load();
                return this;
            }

        }

        public bool CheckBarCode(int GoodsId, string Model, int Count)
        {
            string sql = string.Format(" select * from GoodsBarCode where GoodsId={0} and Model='{1}' and count =  {2}", GoodsId, Model, Count);
            if (m_dbo.GetDataSet(sql).Tables[0].Rows.Count == 0)
            {
                return true;
            }
            else return false;
        }
        /// <summary>
        /// 删除条形码数据
        /// </summary>
        /// <param name="BarCode"></param>
        /// <returns></returns>
        public bool DeleteBarCode(string BarCode)
        {
            string sql0 = string.Format(" select goodsId from GoodsBarCode where BarCode = '{0}' ", BarCode);
            DataSet ds = m_dbo.GetDataSet(sql0);
            int goodsId = DBTool.GetIntFromRow(ds.Tables[0].Rows[0], "goodsId", 0);

            string sql = string.Format(" delete from GoodsBarCode where BarCode='{0}' ", BarCode);
            bool result = m_dbo.ExecuteNonQuery(sql);
            return result;
        }
    }
}
