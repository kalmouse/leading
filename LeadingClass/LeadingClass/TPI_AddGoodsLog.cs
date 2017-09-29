using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class TPI_Addgoodslog
    {
        public int Id { get; set; }
        public int GoodsId { get; set; }
        public int ProjectId { get; set; }
        public int TheState { get; set; }
        public string Memo { get; set; }
        public DateTime Ctime { get; set; }
        private DBOperate m_dbo;

        public TPI_Addgoodslog()
        {
            Id = 0;
            GoodsId = 0;
            ProjectId = 0;
            TheState = 0;
            Memo = "";
            Ctime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@GoodsId", GoodsId));
            arrayList.Add(new SqlParameter("@ProjectId", ProjectId));
            arrayList.Add(new SqlParameter("@TheState", TheState));
            arrayList.Add(new SqlParameter("@Memo", Memo));
            arrayList.Add(new SqlParameter("@Ctime", Ctime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("TPI_AddGoodsLog", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("TPI_AddGoodsLog", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from TPI_AddGoodsLog where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                ProjectId = DBTool.GetIntFromRow(row, "ProjectId", 0);
                TheState = DBTool.GetIntFromRow(row, "TheState", 0);
                Memo = DBTool.GetStringFromRow(row, "Memo", "");
                Ctime = DBTool.GetDateTimeFromRow(row, "Ctime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from TPI_AddGoodsLog where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}
