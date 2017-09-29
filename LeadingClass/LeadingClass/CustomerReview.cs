using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;


namespace LeadingClass
{
    public class CustomerReview
    {
        public int Id { get; set; }
        public int FK_CustomerId { get; set; }
        public int FK_UsersId { get; set; }
        public string Contents { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int UpdatePerson { get; set; }
        public DateTime NextTime { get; set; }
        public string ReviewPerson { get; set; }
        public string ReviewPhone { get; set; }
        private DBOperate m_dbo;

        public CustomerReview()
        {
            Id = 0;
            FK_CustomerId = 0;
            FK_UsersId = 0;
            Contents = "";
            CreateTime = DateTime.Now;
            UpdateTime = DateTime.Now;
            UpdatePerson = 0;
            NextTime = DateTime.Now;
            ReviewPerson = "";
            ReviewPhone = "";
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@FK_CustomerId", FK_CustomerId));
            arrayList.Add(new SqlParameter("@FK_UsersId", FK_UsersId));
            arrayList.Add(new SqlParameter("@Contents", Contents));
            arrayList.Add(new SqlParameter("@CreateTime", CreateTime));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@UpdatePerson", UpdatePerson));
            arrayList.Add(new SqlParameter("@NextTime", NextTime));
            arrayList.Add(new SqlParameter("@ReviewPerson", ReviewPerson));
            arrayList.Add(new SqlParameter("@ReviewPhone", ReviewPhone));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("CustomerReview", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("CustomerReview", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from CustomerReview where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                FK_CustomerId = DBTool.GetIntFromRow(row, "FK_CustomerId", 0);
                FK_UsersId = DBTool.GetIntFromRow(row, "FK_UsersId", 0);
                Contents = DBTool.GetStringFromRow(row, "Contents", "");
                CreateTime = DBTool.GetDateTimeFromRow(row, "CreateTime");
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                UpdatePerson = DBTool.GetIntFromRow(row, "UpdatePerson", 0);
                NextTime = DBTool.GetDateTimeFromRow(row, "NextTime");
                ReviewPerson = DBTool.GetStringFromRow(row, "ReviewPerson", "");
                ReviewPhone = DBTool.GetStringFromRow(row, "ReviewPhone", "");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from CustomerReview where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 按公司 Id 读取 回访记录
        /// ERP：FReceivableReminder.cs
        /// 用了 Company（客户公司信息表） CustomerReview（客户回访）Sys_Users（系统用户表）
        /// </summary>
        /// <returns></returns>
        public DataSet ReadCustomerReviewByComId()
        {
            string sql = string.Format(" select * from View_CustomerReview where FK_CustomerId={0}  order by Id desc ", this.FK_CustomerId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 按公司 Id 读取 回访记录
        /// 调用：ERP：FReceivableReminder.cs
        /// </summary>
        /// <returns></returns>
        public DataSet ReadCustomerReviewReminder(DateTime nextTime,int UserId)
        {
            string sql = string.Format(@"select * from 
       (select rank() over(partition by FK_CustomerId order by NextTime desc) idd,
       *from View_CustomerReview ) t1 
       where idd=1 and NextTime<'{0}' and FK_UsersId ={1} ",nextTime,UserId );
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 回访统计
        /// </summary>
        /// <returns></returns>
        public DataSet ReadCustomerReviewReport(String StarTime,String EndTime,int userId,int comId)
        {
            string sql = "select (select COUNT(Id) from View_CustomerReview where FK_CustomerId=View_Company.ComId";
            if (StarTime != "" && StarTime != null) {
                sql += string.Format(" and CreateTime>='{0}' ",StarTime);
            }
            if (EndTime != "" && EndTime != null)
            {
                sql +=string.Format(" and CreateTime<='{0}' ",EndTime);
            }

            sql += string.Format(" ) as ReNum,*  from View_Company where (SalesId={0} or ServiceId={0}) ", userId);
            if (comId > 0) {

                sql += string.Format(" and ComId={0}", comId);
            }
            sql += " order by ReNum";

           
            return m_dbo.GetDataSet(sql);
        }
    }
}
