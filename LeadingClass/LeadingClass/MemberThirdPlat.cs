using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class Memberthirdplat
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string PlatName { get; set; }
        public string Account { get; set; }
        public DateTime RegisterDate { get; set; }
        private DBOperate m_dbo;

        public Memberthirdplat()
        {
            Id = 0;
            MemberId = 0;
            PlatName = "";
            Account = "";
            RegisterDate = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@MemberId", MemberId));
            arrayList.Add(new SqlParameter("@PlatName", PlatName));
            arrayList.Add(new SqlParameter("@Account", Account));
            arrayList.Add(new SqlParameter("@RegisterDate", RegisterDate));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("MemberThirdPlat", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("MemberThirdPlat", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from MemberThirdPlat where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);
                PlatName = DBTool.GetStringFromRow(row, "PlatName", "");
                Account = DBTool.GetStringFromRow(row, "Account", "");
                RegisterDate = DBTool.GetDateTimeFromRow(row, "RegisterDate");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from MemberThirdPlat where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}
