using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public  class PointType
    {
        private int m_Id;
        private int m_TypeId;
        private string m_TypeName;
        private int m_Point;
        private int m_ParentId;
        private DateTime m_UpdateTime;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int TypeId { get { return m_TypeId; } set { m_TypeId = value; } }
        public string TypeName { get { return m_TypeName; } set { m_TypeName = value; } }
        public int Point { get { return m_Point; } set { m_Point = value; } }
        public int ParentId { get { return m_ParentId; } set { m_ParentId = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public PointType()
        {
            m_Id = 0;
            m_TypeId = 0;
            m_TypeName = "";
            m_Point = 0;
            m_ParentId = 0;
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
            arrayList.Add(new SqlParameter("@TypeId", m_TypeId));
            arrayList.Add(new SqlParameter("@TypeName", m_TypeName));
            arrayList.Add(new SqlParameter("@Point", m_Point));
            arrayList.Add(new SqlParameter("@ParentId", m_ParentId));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("PointType", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("PointType", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from PointType where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_TypeId = DBTool.GetIntFromRow(row, "TypeId", 0);
                m_TypeName = DBTool.GetStringFromRow(row, "TypeName", "");
                m_Point = DBTool.GetIntFromRow(row, "Point", 0);
                m_ParentId = DBTool.GetIntFromRow(row, "ParentId", 0);
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        } 
 
    }
}
