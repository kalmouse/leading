using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class TPI_Authority
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public DateTime UpdateTime { get; set; }
        public int IsAdmin { get; set; }
        private DBOperate m_dbo;

        public TPI_Authority()
        {
            Id = 0;
            UserId = 0;
            ProjectId = 0;
            UpdateTime = DateTime.Now;
            IsAdmin = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@UserId", UserId));
            arrayList.Add(new SqlParameter("@ProjectId", ProjectId));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@IsAdmin", IsAdmin));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("TPI_Authority", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("TPI_Authority", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from TPI_Authority where UserId ={0} and ProjectId={1} ", UserId,ProjectId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                ProjectId = DBTool.GetIntFromRow(row, "ProjectId", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                IsAdmin = DBTool.GetIntFromRow(row, "IsAdmin", 0);
                return true;
            }
            return false;
        }
        public bool Delete( int Id)
        {
            string sql = string.Format("Delete from TPI_Authority where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 读取自己可用的项目
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet ReadMyProject(int userId)
        {
            string sql = string.Format("select * from dbo.View_TPIAuthority where UserId={0}", userId);
            return m_dbo.GetDataSet(sql);
        }

        public DataSet ReadProject(int userId)
        {
            string sql = string.Format("select top 1 * from TPI_Authority where UserId ={0}", userId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 读取权限（根据用户编号和项目编号）
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="ProjectId">项目编号</param>
        /// <returns></returns>
        public DataSet ReadAuthority(int UserId, int ProjectId)
        {
            string sqlStr = string.Format("select * from TPI_Authority where UserId={0} and ProjectId={1}", UserId, ProjectId);
            return m_dbo.GetDataSet(sqlStr);
        }

   }
}
