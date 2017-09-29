using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace LeadingClass
{
    public class Crm_Complain
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public int GoodsLevel { get; set; }
        public int AgingLevel { get; set; }
        public int ServiceLevel { get; set; }
        public DateTime Updatetime { get; set; }
        private DBOperate m_dbo;

        public Crm_Complain()
        {
            Id = 0;
            OrderId = 0;
            Title = "";
            Content = "";
            Type = "";
            GoodsLevel = 0;
            AgingLevel = 0;
            ServiceLevel = 0;
            Updatetime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public Crm_Complain(int Id)
            : this()
        {
            this.Id = Id;
            this.Load();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@OrderId", OrderId));
            arrayList.Add(new SqlParameter("@Title", Title));
            arrayList.Add(new SqlParameter("@Content", Content));
            arrayList.Add(new SqlParameter("@Type", Type));
            arrayList.Add(new SqlParameter("@GoodsLevel", GoodsLevel));
            arrayList.Add(new SqlParameter("@AgingLevel", AgingLevel));
            arrayList.Add(new SqlParameter("@ServiceLevel", ServiceLevel));
            arrayList.Add(new SqlParameter("@Updatetime", Updatetime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Crm_Complain", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Crm_Complain", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Crm_Complain where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                OrderId = DBTool.GetIntFromRow(row, "OrderId", 0);
                Title = DBTool.GetStringFromRow(row, "Title", "");
                Content = DBTool.GetStringFromRow(row, "Content", "");
                Type = DBTool.GetStringFromRow(row, "Type", "");
                GoodsLevel = DBTool.GetIntFromRow(row, "GoodsLevel", 0);
                AgingLevel = DBTool.GetIntFromRow(row, "AgingLevel", 0);
                ServiceLevel = DBTool.GetIntFromRow(row, "ServiceLevel", 0);
                Updatetime = DBTool.GetDateTimeFromRow(row, "Updatetime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from Crm_Complain where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}
