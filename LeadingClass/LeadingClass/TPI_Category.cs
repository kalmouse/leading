using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class TPI_Category
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Category1 { get; set; }
        public string Category2 { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public string TypeName { get; set; }
        public int TypeId { get; set; }
        public DateTime UpdateTime { get; set; }
        public string CategoryCode { get; set; }
        public decimal Discount { get; set; }
        public int UserId { get; set; }
        private DBOperate m_dbo;

        public TPI_Category()
        {
            Id = 0;
            ProjectId = 0;
            Category1 = "";
            Category2 = "";
            Category = "";
            CategoryId = 0;
            TypeName = "";
            TypeId = 0;
            UpdateTime = DateTime.Now;
            CategoryCode = "";
            Discount = 0;
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
            arrayList.Add(new SqlParameter("@ProjectId", ProjectId));
            arrayList.Add(new SqlParameter("@Category1", Category1));
            arrayList.Add(new SqlParameter("@Category2", Category2));
            arrayList.Add(new SqlParameter("@Category", Category));
            arrayList.Add(new SqlParameter("@CategoryId", CategoryId));
            arrayList.Add(new SqlParameter("@TypeName", TypeName));
            arrayList.Add(new SqlParameter("@TypeId", TypeId));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@CategoryCode", CategoryCode));
            arrayList.Add(new SqlParameter("@Discount", Discount));
            arrayList.Add(new SqlParameter("@UserId", UserId));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("TPI_Category", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("TPI_Category", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from TPI_Category where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                ProjectId = DBTool.GetIntFromRow(row, "ProjectId", 0);
                Category1 = DBTool.GetStringFromRow(row, "Category1", "");
                Category2 = DBTool.GetStringFromRow(row, "Category2", "");
                Category = DBTool.GetStringFromRow(row, "Category", "");
                CategoryId = DBTool.GetIntFromRow(row, "CategoryId", 0);
                TypeName = DBTool.GetStringFromRow(row, "TypeName", "");
                TypeId = DBTool.GetIntFromRow(row, "TypeId", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                CategoryCode = DBTool.GetStringFromRow(row, "CategoryCode", "");
                Discount = DBTool.GetDecimalFromRow(row, "Discount", 0);
                UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from TPI_Category where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 批量修改折扣率
        /// </summary>
        /// <param name="TypeIds"></param>
        /// <returns></returns>
        public bool UpdateDiscount(int[] TypeIds)
        {
            string sql = string.Format("Update TPI_Category set Discount={0} where  ProjectId={1} ", Discount, ProjectId);
            if (TypeIds != null)
            {
                sql += " and TypeId in ( ";
                for (int i = 0; i < TypeIds.Length; i++)
                {
                    if (i == 0)
        {
                        sql += string.Format(" '{0}' ", TypeIds[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", TypeIds[i]);
                }
                sql += " ) ";
            }
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 待整理
        /// </summary>
        /// <param name="TypeId"></param>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public DataSet SelectCategoryId(int TypeId, int ProjectId)
        {
            string sql = string.Format("select * from TPI_Category where ProjectId={0} and TypeId={1} ", ProjectId, TypeId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 根据projectId读取第三方的类别编号类别名称          --------------------------------------需要修改
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public DataTable GetCategoryCode()
        {
            string sql = string.Format(" select Category,CategoryCode,TypeId,TypeName  from TPI_Category where ProjectId={0} and TypeId>1  group by Category,CategoryCode,TypeId,TypeName ", ProjectId);
            return m_dbo.GetDataSet(sql).Tables[0];
        }
    }
}
