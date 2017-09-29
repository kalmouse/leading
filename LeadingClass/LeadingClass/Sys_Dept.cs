using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using CommenClass;

using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;

namespace LeadingClass
{
    public class Sys_Dept
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BranchId { get; set; }
        public string Memo { get; set; }
        public string Code { get; set; }
        public string PCode { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public Sys_Dept()
        {
            Id = 0;
            Name = "";
            BranchId = 0;
            Memo = "";
            Code = "";
            PCode = "";
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
            arrayList.Add(new SqlParameter("@Name", Name));
            arrayList.Add(new SqlParameter("@BranchId", BranchId));
            arrayList.Add(new SqlParameter("@Memo", Memo));
            arrayList.Add(new SqlParameter("@Code", Code));
            arrayList.Add(new SqlParameter("@PCode", PCode));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("sys_dept", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("sys_dept", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }

        public bool Load()
        {
            string sql = string.Format("select * from sys_dept where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                Name = DBTool.GetStringFromRow(row, "Name", "");
                BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                Memo = DBTool.GetStringFromRow(row, "Memo", "");
                Code = DBTool.GetStringFromRow(row, "Code", "");
                PCode = DBTool.GetStringFromRow(row, "PCode", "");
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }

        public bool Delete()
        {
            string sql = string.Format("update Sys_users set DeptId=0 where DeptId={0}; delete from sys_dept where Id = {0}; ", this.Id);
            return m_dbo.ExecuteNonQuery(sql);
        }


        public DataSet ReadUsers(int branchId, int deptId)
        {
            this.Id = deptId;
            this.Load();
            string sql = string.Format("select *  from view_sysusers where branchId={0} ", branchId);
            if (deptId > 0)
            {
                if (this.Code != "")
                {
                    sql += string.Format(" and code like '{0}%' ", this.Code);
                }
                else
                {
                    sql += string.Format(" and deptId = {0} ", deptId);
                }
            }
            sql += " order by code,name";
            return  m_dbo.GetDataSet(sql);
        }
    }
}
