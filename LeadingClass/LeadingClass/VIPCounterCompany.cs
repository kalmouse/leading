using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{

    public class VIPCounterCompany
    {
        public int Id { get; set; }
        public int ComId { get; set; }
        public int CounterId { get; set; }
        public DateTime UpdateTime { get; set; }
        public decimal Discount { get; set; }
        public int UserId { get; set; }
        public DBOperate m_dbo;
        public VIPCounterCompany()
        {
            Id = 0;
            ComId = 0;
            CounterId = 0;
            UpdateTime = DateTime.Now;
            Discount = 1;
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
            arrayList.Add(new SqlParameter("@ComId", ComId));
            arrayList.Add(new SqlParameter("@CounterId", CounterId));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@Discount", Discount));
            arrayList.Add(new SqlParameter("@UserId", UserId));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("VIPCounterCompany", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("VIPCounterCompany", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from VIPCounterCompany where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                ComId = DBTool.GetIntFromRow(row, "ComId", 0);
                CounterId = DBTool.GetIntFromRow(row, "CounterId", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                Discount = DBTool.GetDecimalFromRow(row, "Discount", 1);
                UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                return true;
            }
            return false;
        }

        public bool LoadFromComId(int ComId)
        {
            string sql = string.Format(" select * from VIPCounterCompany where ComId={0} order by Id desc", ComId);//如果不小心为一个公司插入了多个专柜，读取最新的一个专柜 quxiaoshan 2015-5-21
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                ComId = DBTool.GetIntFromRow(row, "ComId", 0);
                CounterId = DBTool.GetIntFromRow(row, "CounterId", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                Discount = DBTool.GetDecimalFromRow(row, "Discount", 1);
                return true;
            }
            return false;

        }

        public bool Delete(int CounterId, int ComId)
        {
            string sql = string.Format(" delete from VIPCounterCompany where CounterId={0} and ComId={1} ", CounterId, ComId);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 查询专柜对应公司
        /// </summary>
        /// <param name="CounterId"> 专柜编号</param>
        /// <returns></returns>
        public DataSet VIPCounterCompanyList(int CounterId)
        {
            string sql = string.Format("select Id, ComId,Name,CounterId,CompanyName,ShortName,branchId,(Discount*100) as Discount from View_VIPCounterCompany");
            if (CounterId > 0)
            {
                sql += string.Format(" where CounterId={0}", CounterId);
            }
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 查询专柜对应公司
        /// </summary>
        /// <param name="CounterId">专柜编号</param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public DataSet VIPCounterCompanyList(int CounterId, int branchId)
        {
            string sql = string.Format("select Id, ComId,Name,CounterId,CompanyName,ShortName,VCCBranchId,(Discount*100) as Discount from View_VIPCounterCompany where BranchId=0");
            if (CounterId > 0)
            {
                sql += string.Format(" and CounterId={0}", CounterId);
            }
            if (branchId > 0)
            {
                sql += string.Format(" and VCCBranchId={0}", branchId);
            }
            return m_dbo.GetDataSet(sql);
        }

    }
}
