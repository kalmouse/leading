using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class Goodsparamgroup
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public DateTime UpdateTime { get; set; }
        public int UserId { get; set; }
        private DBOperate m_dbo;

        public Goodsparamgroup()
        {
            Id = 0;
            GroupName = "";
            UpdateTime = DateTime.Now;
            UserId = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@GroupName", GroupName));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@UserId", UserId));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("GoodsParamGroup", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("GoodsParamGroup", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from GoodsParamGroup where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                GroupName = DBTool.GetStringFromRow(row, "GroupName", "");
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from GoodsParamGroup where id={0} ", Id);
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
            string sql = string.Format("select  TypeId , gpg.Id , gpg.GroupName , gp.Id  , gp.Name as ItemName    from GoodsParam gp  inner  join GoodsParamGroup gpg on gp.GoodsParamGroupId=gpg.Id where gp.TypeId="+goodstypeid+"  order by gpg.Id ");
            return m_dbo.GetDataSet(sql).Tables[0];
        }


        #endregion
    }
}
