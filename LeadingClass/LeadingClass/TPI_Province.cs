using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class TPI_Province
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
        public int Type { get; set; }
        public int ProjectId { get; set; }
        private DBOperate m_dbo;

        public TPI_Province()
        {
            Id = 0;
            Code = "";
            Name = "";
            Parent = "";
            Type = 0;
            ProjectId = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@Code", Code));
            arrayList.Add(new SqlParameter("@Name", Name));
            arrayList.Add(new SqlParameter("@Parent", Parent));
            arrayList.Add(new SqlParameter("@Type", Type));
            arrayList.Add(new SqlParameter("@ProjectId", ProjectId));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("TPI_Province", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("TPI_Province", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from TPI_Province where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Code = DBTool.GetStringFromRow(row, "Code", "");
                Name = DBTool.GetStringFromRow(row, "Name", "");
                Parent = DBTool.GetStringFromRow(row, "Parent", "");
                Type = DBTool.GetIntFromRow(row, "Type", 0);
                ProjectId = DBTool.GetIntFromRow(row, "ProjectId", 0);

                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from TPI_Province where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

       /// <summary>
       /// 读取第三方地址
       /// </summary>
       /// <returns></returns>
        public DataSet ReadProvince()
        {
            string sql = " select * from  dbo.TPI_Province where 1=1 ";
            if (Type != 0)
            {
                sql += string.Format(" and Type={0}", Type);
            }
            if (Parent != "")
            {
                sql += string.Format(" and Parent = '{0}' ", Parent);
            }
            if (ProjectId > 0)
            {
                sql += string.Format("and  projectId={0}", ProjectId);
            }
            sql += string.Format("order by Code");
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 根据Code获取名称
        /// </summary>
        /// <returns></returns>
        public string GetNameByCode(string code)
        {
            string name = "";
            if (code != "")
            {
                string sql = string.Format("select Name from TPI_Province where Code='{0}'", code);
                DataSet ds = m_dbo.GetDataSet(sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    name = DBTool.GetStringFromRow(row, "Name", "");
                }
            }
            return name;
        }
    }
}
