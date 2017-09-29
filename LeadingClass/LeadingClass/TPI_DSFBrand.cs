using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class TPI_Dsfbrand
    {
        public int Id { get; set; }
        public string DSFBrandId { get; set; }
        public string DSFBrandName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int ProjectId { get; set; }
        private DBOperate m_dbo;

        public TPI_Dsfbrand()
        {
            Id = 0;
            DSFBrandId = "";
            DSFBrandName = "";
            BrandId = 0;
            BrandName = "";
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
            arrayList.Add(new SqlParameter("@DSFBrandId", DSFBrandId));
            arrayList.Add(new SqlParameter("@DSFBrandName", DSFBrandName));
            arrayList.Add(new SqlParameter("@BrandId", BrandId));
            arrayList.Add(new SqlParameter("@BrandName", BrandName));
            arrayList.Add(new SqlParameter("@ProjectId", ProjectId));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("TPI_DSFBrand", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("TPI_DSFBrand", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from TPI_DSFBrand where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                DSFBrandId = DBTool.GetStringFromRow(row, "DSFBrandId", "");
                DSFBrandName = DBTool.GetStringFromRow(row, "DSFBrandName", "");
                BrandId = DBTool.GetIntFromRow(row, "BrandId", 0);
                BrandName = DBTool.GetStringFromRow(row, "BrandName", "");
                ProjectId = DBTool.GetIntFromRow(row, "ProjectId", 0);
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from TPI_DSFBrand where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 读取用友电商平台品牌id
        /// </summary>
        /// <returns></returns>
        public DataSet Read_DSFbrand(string brandname)
        {
            string sql = string.Format(" select * from  TPI_DSFBrand where BrandName like '%{0}%'", brandname);
            return m_dbo.GetDataSet(sql);
        
        }
        /// <summary>
        /// 读取品牌ID和名称
        /// </summary>
        /// <returns></returns>
        public DataSet Read_YYBrand(string dsfBrandId,int projectId)
        {
            string sql = string.Format("select * from  TPI_DSFBrand where DSFBrandId ='{0}' and  ProjectId={1} ", dsfBrandId, projectId);
            return m_dbo.GetDataSet(sql);
        }
    }
}
  