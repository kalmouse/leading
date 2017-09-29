using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace LeadingClass
{
    public class Sys_BranchDetail
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int Square { get; set; }
        public int Car { get; set; }
        public string Introduction { get; set; }
        public string EnIntroduction { get; set; }
        public int PeopleNum { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public Sys_BranchDetail()
        {
            Id = 0;
            BranchId = 0;
            Square = 0;
            Car = 0;
            Introduction = "";
            EnIntroduction = "";
            PeopleNum = 0;
            UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public Sys_BranchDetail(int branchId)
        {
            m_dbo = new DBOperate();
            string sql = string.Format("select * from Sys_BranchDetail where branchid={0}", branchId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                Square = DBTool.GetIntFromRow(row, "Square", 0);
                Car = DBTool.GetIntFromRow(row, "Car", 0);
                Introduction = DBTool.GetStringFromRow(row, "Introduction", "");
                EnIntroduction = DBTool.GetStringFromRow(row, "EnIntroduction", "");
                PeopleNum = DBTool.GetIntFromRow(row, "PeopleNum", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
            }
        }

        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@BranchId", BranchId));
            arrayList.Add(new SqlParameter("@Square", Square));
            arrayList.Add(new SqlParameter("@Car", Car));
            arrayList.Add(new SqlParameter("@Introduction", Introduction));
            arrayList.Add(new SqlParameter("@EnIntroduction",EnIntroduction));
            arrayList.Add(new SqlParameter("@PeopleNum",PeopleNum));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Sys_BranchDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Sys_BranchDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Sys_BranchDetail where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                Square = DBTool.GetIntFromRow(row, "Square", 0);
                Car = DBTool.GetIntFromRow(row, "Car", 0);
                Introduction = DBTool.GetStringFromRow(row, "Introduction", "");
                EnIntroduction = DBTool.GetStringFromRow(row,"EnIntroduction","");
                PeopleNum = DBTool.GetIntFromRow(row,"PeopleNum",0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from Sys_BranchDetail where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
}
