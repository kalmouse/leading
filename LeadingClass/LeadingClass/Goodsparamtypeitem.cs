using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class Goodsparamtypeitem
    {
        public int Id { get; set; }
        public int GoodsParamTypeId { get; set; }
        public string ItemKey { get; set; }
        private DBOperate m_dbo;

        public Goodsparamtypeitem()
        {
            Id = 0;
            GoodsParamTypeId = 0;
            ItemKey = "";
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@GoodsParamTypeId", GoodsParamTypeId));
            arrayList.Add(new SqlParameter("@ItemKey", ItemKey));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("GoodsParamTypeItem", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("GoodsParamTypeItem", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from GoodsParamTypeItem where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                GoodsParamTypeId = DBTool.GetIntFromRow(row, "GoodsParamTypeId", 0);
                ItemKey = DBTool.GetStringFromRow(row, "ItemKey", "");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from GoodsParamTypeItem where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}
