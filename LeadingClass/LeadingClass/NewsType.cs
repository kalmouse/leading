using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public  class NewsType
    {
        private int m_Id;
        private string m_TypeName;
        private DateTime m_CreateDate;
        private int m_ParentId;
        private int m_IsNewsOrHelp;
        private DataRow[] m_NewsTypeRows;//子级类别
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public string TypeName { get { return m_TypeName; } set { m_TypeName = value; } }
        public DateTime CreateDate { get { return m_CreateDate; } set { m_CreateDate = value; } }
        public int ParentId { get { return m_ParentId; } set { m_ParentId = value; } }
        public int IsNewsOrHelp { get { return m_IsNewsOrHelp; } set { m_IsNewsOrHelp = value; } }
        public DataRow[] NewsTypeRows { get { return m_NewsTypeRows; } set { m_NewsTypeRows = value; } }
        public NewsType()
        {
            m_Id = 0;
            m_TypeName = "";
            m_CreateDate = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@TypeName", m_TypeName));
            arrayList.Add(new SqlParameter("@CreateDate", m_CreateDate));
            arrayList.Add(new SqlParameter("@ParentId", m_ParentId));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("NewsType", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("NewsType", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from NewsType where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_TypeName = DBTool.GetStringFromRow(row, "TypeName", "");
                m_CreateDate = DBTool.GetDateTimeFromRow(row, "CreateDate");
                m_ParentId = DBTool.GetIntFromRow(row, "ParentId", 0);
                m_IsNewsOrHelp = DBTool.GetIntFromRow(row, "IsNewsOrHelp", 0);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 读取新闻或者帮助的类别，依照参数IsNewsOrHelp，IsNewsOrHelp=0 表示的是新闻 ；IsNewsOrHelp=1
        /// </summary>
        /// <returns></returns>
        public List<NewsType> ReadNewsOrHelpType()
        {
            string sql =string.Format( "select * from dbo.NewsType where 1=1 and IsNewsOrHelp={0} ",m_IsNewsOrHelp);
            DBOperate dbOperate = new DBOperate();
            if (m_Id > 0)
            {
                sql += string.Format(" and Id=" + m_Id);
            }
            sql += " order by CreateDate ";
            DataSet ds= dbOperate.GetDataSet(sql);
            return TypeDataSetToList(ds);
        }

        private List<NewsType> TypeDataSetToList(DataSet ds)
        {
            DataRow[] parentRows= ds.Tables[0].Select("ParentId="+0);
            List<NewsType> newsTypeList = new List<NewsType>();
            foreach (DataRow typeRow in parentRows)
            {
                NewsType newsType = new NewsType();
                newsType.m_Id = DBTool.GetIntFromRow(typeRow,"Id", 0);
                newsType.m_TypeName = DBTool.GetStringFromRow(typeRow, "TypeName","");
                newsType.m_CreateDate = DBTool.GetDateTimeFromRow(typeRow, "CreateDate");
                newsType.m_ParentId = DBTool.GetIntFromRow(typeRow, "ParentId",0);
                newsType.m_IsNewsOrHelp = DBTool.GetIntFromRow(typeRow, "IsNewsOrHelp",0);
                newsType.m_NewsTypeRows = ds.Tables[0].Select("ParentId=" + newsType.m_Id); 
                newsTypeList.Add(newsType);
            }
            return newsTypeList;
        }
    }
}
