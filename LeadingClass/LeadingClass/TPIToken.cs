using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class TPIToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; } 
        public DateTime CallDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime AccessTime { get; set; } 
        //DateTime dat = new DateTime();  
        private DBOperate m_dbo;
        
        public TPIToken()
        {
            Token = "";
            UserId = 0;
            ProjectId = 0;
            CallDate = DateTime.Now;         
            EndDate = DateTime.Now;
            AccessTime = DateTime.Now;
            m_dbo = new DBOperate();

        }
        /// <summary>
        /// 检查Token的有效性
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool Check(string token)
        {
            bool result = false;
            string sql = string.Format("select * from TPI_Users where State=1  and token='{0}'", token);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count ==1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Token = DBTool.GetStringFromRow(row, "Token", "");
                ProjectId = DBTool.GetIntFromRow(row, "ProjectId", 0);
                CallDate = DBTool.GetDateTimeFromRow(row, "CallDate");
                EndDate = DBTool.GetDateTimeFromRow(row, "EndDate");
                if (EndDate > DateTime.Now)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }
        public int Save() 
        {
            ArrayList list = new ArrayList();
            if (Id > 0)
            {
                list.Add(new SqlParameter("@Id",Id ));
            }
          
            list.Add(new SqlParameter("@Token", Token));
            list.Add(new SqlParameter("@CallDate", CallDate));
            list.Add(new SqlParameter("@EndDate", EndDate));
            if (this.Id >0 )
            {
                m_dbo.UpdateData("TPIToken",(SqlParameter[])list.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }

       
    }

}
