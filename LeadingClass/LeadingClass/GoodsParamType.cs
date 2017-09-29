using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class Goodsparamtype
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GoodsTypeId { get; set; }
        private DBOperate m_dbo;

        public Goodsparamtype()
        {
            Id = 0;
            Name = "";
            GoodsTypeId = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@Name", Name));
            arrayList.Add(new SqlParameter("@GoodsTypeId", GoodsTypeId));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("GoodsParamType", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("GoodsParamType", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from GoodsParamType where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                Name = DBTool.GetStringFromRow(row, "Name", "");
                GoodsTypeId = DBTool.GetIntFromRow(row, "GoodsTypeId", 0);
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from GoodsParamType where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }


        #region  自定义方法

        /// <summary>
        /// 获取商品参数类型
        /// </summary>
        /// <param name="goodstypeid"></param>
        /// <returns></returns>
        public DataTable GetGoodsParamType(int goodstypeid)
        {
            string sql = string.Format("select  gpt.Id ,  Name ,  gpti.Id ,   ItemKey  from Goodsparamtype gpt inner join GoodsParamTypeItem gpti on gpt.Id=gpti.GoodsParamTypeId  where gpt.GoodsTypeId=" + goodstypeid);
            return m_dbo.GetDataSet(sql).Tables[0];
        }


        #endregion

    }
}
