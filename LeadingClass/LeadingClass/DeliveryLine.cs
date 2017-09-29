using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace LeadingClass
{
    public  class DeliveryLine
    {
        private int m_Id;
        private string m_LineName;
        private string m_LineRange;
        private DateTime m_UpdateTime;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public string LineName { get { return m_LineName; } set { m_LineName = value; } }
        public string LineRange { get { return m_LineRange; } set { m_LineRange = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public DeliveryLine()
        {
            m_Id = 0;
            m_LineName = "";
            m_LineRange = "";
            m_UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@LineName", m_LineName));
            arrayList.Add(new SqlParameter("@LineRange", m_LineRange));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("DeliveryLine", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("DeliveryLine", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from DeliveryLine where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_LineName = DBTool.GetStringFromRow(row, "LineName", "");
                m_LineRange = DBTool.GetStringFromRow(row, "LineRange", "");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        } 
 
    }

    public class DeliveryCompanyLine 
    {
        private int m_Id;
        private int m_CompanyId;
        private int m_LineId;
        private string m_Remark;
        private DateTime m_UpdateTime;
        private int m_Distance;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int CompanyId { get { return m_CompanyId; } set { m_CompanyId = value; } }
        public int LineId { get { return m_LineId; } set { m_LineId = value; } }
        public string Remark { get { return m_Remark; } set { m_Remark = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public int Distance { get { return m_Distance; } set { m_Distance = value; } }//送货距离，公里
        public DeliveryCompanyLine()
        {
            m_Id = 0;
            m_CompanyId = 0;
            m_LineId = 0;
            m_Remark = "";
            m_UpdateTime = DateTime.Now;
            m_Distance = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@CompanyId", m_CompanyId));
            arrayList.Add(new SqlParameter("@LineId", m_LineId));
            arrayList.Add(new SqlParameter("@Remark", m_Remark));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            arrayList.Add(new SqlParameter("@Distance", m_Distance));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("DeliveryCompanyLine", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("DeliveryCompanyLine", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from DeliveryCompanyLine where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_CompanyId = DBTool.GetIntFromRow(row, "CompanyId", 0);
                m_LineId = DBTool.GetIntFromRow(row, "LineId", 0);
                m_Remark = DBTool.GetStringFromRow(row, "Remark", "");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "Update");
                m_Distance = DBTool.GetIntFromRow(row, "Distance", 0);
                return true;
            }
            return false;
        } 
 
    }

    public class DeliveryMemberLine 
    {
        private int m_Id;
        private int m_MemberId;
        private int m_LineId;
        private string m_Remark;
        private DateTime m_UpdateTime;
        private int m_Distance;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int MemberId { get { return m_MemberId; } set { m_MemberId = value; } }
        public int LineId { get { return m_LineId; } set { m_LineId = value; } }
        public string Remark { get { return m_Remark; } set { m_Remark = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public int Distance { get { return m_Distance; } set { m_Distance = value; } }
        public DeliveryMemberLine()
        {
            m_Id = 0;
            m_MemberId = 0;
            m_LineId = 0;
            m_Remark = "";
            m_UpdateTime = DateTime.Now;
            m_Distance = 0;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@MemberId", m_MemberId));
            arrayList.Add(new SqlParameter("@LineId", m_LineId));
            arrayList.Add(new SqlParameter("@Remark", m_Remark));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            arrayList.Add(new SqlParameter("@Distance", m_Distance));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("DeliveryMemberLine", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("DeliveryMemberLine", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from DeliveryMemberLine where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);
                m_LineId = DBTool.GetIntFromRow(row, "LineId", 0);
                m_Remark = DBTool.GetStringFromRow(row, "Remark", "");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_Distance = DBTool.GetIntFromRow(row, "Distance", 0);
                return true;
            }
            return false;
        } 
 
    }
}
