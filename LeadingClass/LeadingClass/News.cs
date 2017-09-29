using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public  class News
    {
        private int m_Id;
        private string m_Title;
        private int m_NewsTypeId;
        private string m_NewsKeywords;
        private string m_Author;
        private DateTime m_PublishDate;
        private string m_ImageUrl;
        private string m_Content;
        private int m_Recommend;
        private string m_Describe;
        private int m_BranchId;
        private int m_IsCheck;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public string Title { get { return m_Title; } set { m_Title = value; } }
        public int NewsTypeId { get { return m_NewsTypeId; } set { m_NewsTypeId = value; } }
        public string NewsKeywords { get { return m_NewsKeywords; } set { m_NewsKeywords = value; } }
        public string Author { get { return m_Author; } set { m_Author = value; } }
        public DateTime PublishDate { get { return m_PublishDate; } set { m_PublishDate = value; } }
        public string ImageUrl { get { return m_ImageUrl; } set { m_ImageUrl = value; } }
        public string Content { get { return m_Content; } set { m_Content = value; } }
        public int Recommend { get { return m_Recommend; } set { m_Recommend = value; } }
        public string Describe { get { return m_Describe; } set { m_Describe = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public int IsCheck { get { return m_IsCheck; } set { m_IsCheck = value; } }
        public News() 
        { 
            m_Id = 0 ; 
            m_Title = "" ;
            m_NewsTypeId = 0;
            m_NewsKeywords =""; 
            m_Author = "" ; 
            m_PublishDate = DateTime.Now ; 
            m_ImageUrl = "" ; 
            m_Content = "" ; 
            m_Recommend = 0 ; 
            m_Describe = "" ;
            m_IsCheck = 0;
            m_dbo = new DBOperate(); 
        }

        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@Title", m_Title));
            arrayList.Add(new SqlParameter("@NewsTypeId",m_NewsTypeId));
            arrayList.Add(new SqlParameter("@NewsKeywords", m_NewsKeywords));
            arrayList.Add(new SqlParameter("@Author", m_Author));
            arrayList.Add(new SqlParameter("@PublishDate", m_PublishDate));
            arrayList.Add(new SqlParameter("@ImageUrl", m_ImageUrl));
            arrayList.Add(new SqlParameter("@Content", m_Content));
            arrayList.Add(new SqlParameter("@Recommend", m_Recommend));
            arrayList.Add(new SqlParameter("@Describe", m_Describe));
            arrayList.Add(new SqlParameter("@BranchId", m_BranchId));
            arrayList.Add(new SqlParameter("@IsCheck", m_IsCheck));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("News", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("News", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from News where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_Title = DBTool.GetStringFromRow(row, "Title", "");
                m_NewsTypeId =  DBTool.GetIntFromRow(row, "NewsTypeId", 0) ;
                m_NewsKeywords = DBTool.GetStringFromRow(row, "NewsKeywords", "");
                m_Author = DBTool.GetStringFromRow(row, "Author", "");
                m_PublishDate = DBTool.GetDateTimeFromRow(row, "PublishDate");
                m_ImageUrl = DBTool.GetStringFromRow(row, "ImageUrl", "");
                m_Content = DBTool.GetStringFromRow(row, "Content", "");
                m_Recommend = DBTool.GetIntFromRow(row, "Recommend", 0);
                m_Describe = DBTool.GetStringFromRow(row, "Describe", "");
                m_BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                m_IsCheck = DBTool.GetIntFromRow(row, "IsCheck", 0);

                return true;
            }
            return false;
        }
        public bool Delete() 
        {

            DBOperate dbOperate = new DBOperate();
            if (m_Id > 0)
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("Id",m_Id)
                };
                return dbOperate.DeleteAt("News", param);
            }
            else return  false;
        }
    }

    public  class NewsOption
    {
        public int IsNewsOrHelp { get; set; }//1:帮助 0:新闻
        public int TypeId { get; set; }
        public string NewsKeywords { get; set; }
        public int Recommend { get; set; }
        public int IsCheck { get; set; }
        public int BranchTarget { get; set; }//发布新闻 0：全国 1：北京

        public NewsOption() 
        {
            IsNewsOrHelp = 0;
            TypeId = 0;
            NewsKeywords = "";
            Recommend = 0;
            IsCheck = 0;
            BranchTarget = 1;//默认北京
        }
    }

    public class NewsManager
    {
        public DBOperate  dbOperate { get; set; }
        public NewsOption newsOption { get; set; }
        public PageModel pageModel { get; set; }
        public NewsManager()
        {
            dbOperate = new DBOperate();
            newsOption = new NewsOption();
            pageModel = new PageModel();
        }

        /// <summary>
        /// 分页读取新闻列表
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        public DataTable  PageReadNews(int branchId)
        {
            string sql = "";
            string sqlWhere="";
            if (branchId == -2)
            {
                sqlWhere = GetNewsWhere();
            }
            else
            {
                sqlWhere = GetNewsSqlWhere();
            }
            string orderby = " order by PublishDate desc ";
            if (pageModel != null)
            {
                sql += string.Format("select top {0} * from dbo.View_News where 1=1 ",   pageModel.PageSize);
                sql += string.Format("  and Id not in ( select top {0} Id from dbo.View_News where 1=1 {1} {2})", (pageModel.CurrentPage - 1) * pageModel.PageSize, sqlWhere,orderby);
            }
            else 
            {
                sql = "select * from dbo.View_News where 1=1 {0} {1} "+sqlWhere+orderby;
            }
            if (branchId >= 0)
            {
                sql += " and BranchId=" + branchId + "" + sqlWhere + orderby;
            }
            else
            {
                sql +=  sqlWhere + orderby;
            }
            
            return dbOperate.GetDataSet(sql).Tables[0];
        }
        /// <summary>
        /// 网站首页展示新闻  add by hjy 2016-9-8
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public DataSet ShowNews(int branchId)
        {
            string sql = string.Format(@"select top (8-(case when (select COUNT(Id)  from News where BranchId={0} and IsCheck =1) >=4 then 4 when (select COUNT(Id)  from News where BranchId={0} and IsCheck =1)<4 then (select COUNT(Id) from News where BranchId={0} and IsCheck =1) end)) * from News where BranchId=0 and IsCheck=1 order by Recommend desc,PublishDate desc;
                select top 4 * from News where BranchId ={0} and IsCheck =1 order by Recommend desc,PublishDate desc
            ", branchId);
                                         
            return dbOperate.GetDataSet(sql);
        }
        /// <summary>
        /// 展示所有未审核的新闻列表 add by hjy 2016-9-22
        /// </summary>
        /// <returns></returns>
        public DataTable AllowNews()
        {
            string sql = "select * from news where IsCheck=0";
            return dbOperate.GetDataSet(sql).Tables[0];
        }
        /// <summary>
        /// 读取所有帮助
        /// </summary>
        /// <returns></returns>
        public DataTable ReadAllHelps()
        {
            string sql = " select * from view_news where IsNewsOrHelp=1 order by TypeName,Recommend ";
            return dbOperate.GetDataSet(sql).Tables[0];
        }

        /// <summary>
        /// 查询News时的where条件
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        private string GetNewsSqlWhere() 
        { 
            string sql =string.Format(" and IsNewsOrHelp="+ newsOption.IsNewsOrHelp);
            if (newsOption.TypeId > 0)
            {
                sql += string.Format(" and NewsTypeId=" + newsOption.TypeId);
            }
            if (newsOption.NewsKeywords != "")
            {
                sql += string.Format(" and NewsKeywords like '%" + newsOption.NewsKeywords + "%' ");
            }
            if (newsOption.Recommend > 0)
            {
                sql += string.Format(" and Recommend ="+newsOption.Recommend);
            }
            if (newsOption.IsCheck >= -1)
            {
                sql += string.Format(" and IsCheck =" + newsOption.IsCheck);
            }
            return sql;
        }
        //新闻首页点击新闻动态，展示新闻列表--已经审核通过的。
        private string GetNewsWhere()
        {
            string sql = string.Format(" and IsNewsOrHelp=" + newsOption.IsNewsOrHelp);
            if (newsOption.TypeId > 0)
            {
                sql += string.Format(" and NewsTypeId=" + newsOption.TypeId);
            }
            if (newsOption.NewsKeywords != "")
            {
                sql += string.Format(" and NewsKeywords like '%" + newsOption.NewsKeywords + "%' ");
            }
            if (newsOption.Recommend > 0)
            {
                sql += string.Format(" and Recommend =" + newsOption.Recommend);
            }
            
            sql += string.Format(" and IsCheck =1");
           
            return sql;
        }

        /// <summary>
        /// 读取分页的总条数
        /// </summary>
        /// <returns></returns>
        public int PageNewsListTotalRows(int branchId)
        {
            string sql="";
            if (branchId == -2)
            {
                //仅网站首页展示新闻信息用到 GetNewsWhere（）
                sql += string.Format("select COUNT(*) as count from dbo.View_News where 1=1 {0} ", GetNewsWhere());
            }
            else
            {
                 sql+= string.Format("select COUNT(*) as count from dbo.View_News where 1=1 {0} ", GetNewsSqlWhere());
            }
            if (branchId >= 0)
            {
                sql += string.Format(" and branchId={0}", branchId);
            }
            DataRowCollection collection= dbOperate.GetDataSet(sql).Tables[0].Rows;
            if (collection.Count > 0)
            {
                return DBTool.GetIntFromRow(collection[0], "count", 0);
            }
            return 0;
        }
    }
}
