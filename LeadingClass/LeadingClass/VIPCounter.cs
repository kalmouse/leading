using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using LeadingClass;

namespace LeadingClass
{
    public class VIPCounter
    {
        private int m_Id;
        private int m_BranchId;
        private string m_Name;
        private string m_Memo;
        private int m_UserId;
        private DateTime m_AddTime;
        private DateTime m_UpdateTime;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public string Name { get { return m_Name; } set { m_Name = value; } }
        public string Memo { get { return m_Memo; } set { m_Memo = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public DateTime AddTime { get { return m_AddTime; } set { m_AddTime = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }   
        public VIPCounter()
        {
            m_Id = 0;
            m_BranchId = 1;
            m_Name = "";
            m_Memo = "";
            m_UserId = 0;
            m_AddTime = DateTime.Now;
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
            arrayList.Add(new SqlParameter("@BranchId", m_BranchId));
            arrayList.Add(new SqlParameter("@Name", m_Name));
            arrayList.Add(new SqlParameter("@Memo", m_Memo));
            arrayList.Add(new SqlParameter("@UserId", m_UserId));
            arrayList.Add(new SqlParameter("@AddTime", m_AddTime));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("VIPCounter", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("VIPCounter", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from VIPCounter where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                m_Name = DBTool.GetStringFromRow(row, "Name", "");
                m_Memo = DBTool.GetStringFromRow(row, "Memo", "");
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_AddTime = DBTool.GetDateTimeFromRow(row, "AddTime");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete(int CounterId)
        {
            string sql = string.Format(" delete from VIPCounter where Id ={0} ", CounterId);
            return m_dbo.ExecuteNonQuery(sql);
        }
        public DataSet VIPCounterList(string VIPCounterName, int branchId)//chenshaolin 6-17
        {
            StringBuilder stb = new StringBuilder();
            stb.Append("select  v.Id ,v.Name, (select COUNT(Id) from VIPCounterCompany as vc where vc .CounterId =v.Id) as Companycount ,v.Memo from VIPCounter as v where BranchId=0;");
            if (branchId > 0)
            {
                stb.Append(string.Format("select  v.Id ,v.Name,(select COUNT(Id) from VIPCounterCompany as vc where vc .CounterId =v.Id) as Companycount,v.Memo from VIPCounter as v where BranchId={0}", branchId));
            }
            if (VIPCounterName != "")
            {
                stb.Append(string.Format(" and Name like '%{0}%'", VIPCounterName));
            }
            return m_dbo.GetDataSet(stb.ToString());
        }

    }






    public class VIPCounterOption
    {
        private string m_Key;
        private string m_Code;
        private int m_ComId;
        private int m_IsUseRecordControl;//add by wangpl
        private RecordControl m_recordControl;
        private int m_CounterId; //add by quxiaoshan 2015-1-30
        private int m_IsVisible;//add by quxiaoshan 2015-4-2
        private string m_KeyWords;//add by quxiaoshan 2015-4-4
        private int m_BranchId;

        public string Key { get { return m_Key; } set { m_Key = value; } }
        public string Code { get { return m_Code; } set { m_Code = value; } }
        public int ComId { get { return m_ComId; } set { m_ComId = value; } }
        public int IsUseRecorControl { get { return m_IsUseRecordControl; } set { m_IsUseRecordControl = value; } }
        public RecordControl recordContorl { get { return m_recordControl; } set { m_recordControl = value; } }
        public int CounterId { get { return m_CounterId; } set { m_CounterId = value; } }
        public int IsVisible { get { return m_IsVisible; } set { m_IsVisible = value; } }
        public string KeyWords { get { return m_KeyWords; } set { m_KeyWords = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }

        public VIPCounterOption()
        {
            m_Key = "";
            m_Code = "";
            m_ComId = 0;
            m_IsUseRecordControl = 0;
            m_CounterId = 0;
            m_IsVisible = 1;
            m_KeyWords = "";
            m_recordControl = new RecordControl();
            m_BranchId = 0;
        }

    }


}

