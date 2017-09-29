using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class NeedToPurchase
    {
        private int m_Id;
        private int m_UserId;
        private string m_Memo;
        private int m_Status;//1：已完成；0：未完成
        private DateTime m_UpdateTime;
        private int m_PrintNum;
        private int m_BranchId;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public string Memo { get { return m_Memo; } set { m_Memo = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public int Status { get { return m_Status; } set { m_Status = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public NeedToPurchase()
        {
            m_Id = 0;
            m_UserId = 0;
            m_Memo = "";
            m_UpdateTime = DateTime.Now;
            m_Status = 0;
            m_PrintNum = 0;
            m_BranchId = 1;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@UserId", m_UserId));
            arrayList.Add(new SqlParameter("@Memo", m_Memo));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            arrayList.Add(new SqlParameter("@Status", m_Status));
            arrayList.Add(new SqlParameter("@PrintNum", m_PrintNum));
            arrayList.Add(new SqlParameter("@BranchId",m_BranchId));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("NeedToPurchase", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("NeedToPurchase", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from NeedToPurchase where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_Memo = DBTool.GetStringFromRow(row, "Memo", "");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_Status = DBTool.GetIntFromRow(row, "Status", 0);
                m_PrintNum = DBTool.GetIntFromRow(row, "PrintNum", 0);
                m_BranchId = DBTool.GetIntFromRow(row, "BranchId", 1);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 增加打印次数
        /// </summary>
        /// <returns></returns>
        public bool AddPrintNum()
        {
            string sql = string.Format("update NeedToPurchase set PrintNum=PrintNum+1 where id={0}", m_Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        public bool AddStatus(int Id)
        {
            string sql = string.Format("update NeedToPurchase set Status=1 where id={0}", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 判断汇总单是否已拆单
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool NeedToPurchaseStatus(int Id)
        {
            string sql = string.Format("select Status from NeedToPurchase where Id={0};", Id);
            object o = m_dbo.ExecuteScalar(sql);
            int i = Convert.ToInt32(o.ToString());
            if (i == 0)
            {
                return true;
            }
            else return false;
        }

 
    }
}
