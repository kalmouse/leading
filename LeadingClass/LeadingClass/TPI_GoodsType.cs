using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class TPI_GoodsType
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Category1 { get; set; }
        public string Category2 { get; set; }
        public string Category { get; set; }
        public string CategoryCode { get; set; }
         public int CategorysortId1 { get; set; }
         public int CategorysortId2 { get; set; }
         public int CategorysortId3 { get; set; }
        		
        private DBOperate m_dbo;

        public TPI_GoodsType()
        {
            Id = 0;
            ProjectId = 0;
            Category1 = "";
            Category2 = "";
            Category = "";
            CategoryCode = "";
            CategorysortId1=0;
            CategorysortId2 = 0;
            CategorysortId3 = 0;
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
            arrayList.Add(new SqlParameter("@CategoryCode", CategoryCode));
            arrayList.Add(new SqlParameter("@CategorysortId1", CategorysortId1));
            arrayList.Add(new SqlParameter("@CategorysortId2", CategorysortId2));
            arrayList.Add(new SqlParameter("@CategorysortId3", CategorysortId3));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("TPI_GoodsType", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("TPI_GoodsType", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from TPI_GoodsType where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                ProjectId = DBTool.GetIntFromRow(row, "ProjectId", 0);
                Category1 = DBTool.GetStringFromRow(row, "Category1", "");
                Category2 = DBTool.GetStringFromRow(row, "Category2", "");
                Category = DBTool.GetStringFromRow(row, "Category", "");
                CategoryCode = DBTool.GetStringFromRow(row, "CategoryCode", "");
                CategorysortId1 = DBTool.GetIntFromRow(row, "CategorysortId1", 0);
                CategorysortId2 = DBTool.GetIntFromRow(row, "CategorysortId2", 0);
                CategorysortId3 = DBTool.GetIntFromRow(row, "CategorysortId3", 0);

                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from TPI_GoodsType where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 根据一级品目获取所有的子类
        /// </summary>
        /// <returns></returns>
        public DataSet Read_goodstype(string CategorysortId3, string CategoryName, string CategoryType)
        {
            string sql = "select * from TPI_GoodsType where 1=1";
            if (CategorysortId3 != "")
            {
                sql += string.Format(" and CategorysortId3={0}", CategorysortId3);
            }
            if (ProjectId > 0)
            {
                sql += string.Format(" and ProjectId={0}", ProjectId);
            }
            if (CategoryType != "" && CategoryName != "")
            {
                sql += string.Format(" and {0}='{1}'", CategoryType, CategoryName);
            }
            return m_dbo.GetDataSet(sql);

        }
    }
}
