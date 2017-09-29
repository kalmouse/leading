using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class TPI_Goodsparam
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string CategoryCode { get; set; }
        public string AttrName { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public TPI_Goodsparam()
        {
            Id = 0;
            TypeId = 0;
            CategoryCode = "";
            AttrName = "";
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
            arrayList.Add(new SqlParameter("@TypeId", TypeId));
            arrayList.Add(new SqlParameter("@CategoryCode", CategoryCode));
            arrayList.Add(new SqlParameter("@AttrName", AttrName));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("TPI_Goodsparam", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("TPI_Goodsparam", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from TPI_Goodsparam where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                TypeId = DBTool.GetIntFromRow(row, "TypeId", 0);
                CategoryCode = DBTool.GetStringFromRow(row, "CategoryCode", "");
                AttrName = DBTool.GetStringFromRow(row, "AttrName", "");
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from TPI_Goodsparam where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        public DataSet ReadGoodsParam(int TypeId)
        {
            string sql = string.Format("select * from TPI_Goodsparam where typeId={0} ", TypeId);
            return m_dbo.GetDataSet(sql);
        }

    }
}
