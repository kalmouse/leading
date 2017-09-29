using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;

namespace LeadingClass
{
    public class Sys_Updater
    {
        private int m_Id;
        private string m_Version;
        private string m_Files;
        private string m_WhatsNew;
        private DateTime m_UpdateTime;
        private DBOperate m_dbo;
        private string m_Name;

        public int Id { get { return m_Id; } set { m_Id = value; } }
        public string Version { get { return m_Version; } set { m_Version = value; } }
        public string Files { get { return m_Files; } set { m_Files = value; } }
        public string WhatsNew { get { return m_WhatsNew; } set { m_WhatsNew = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public string Name { get { return m_Name; } set { m_Name = value; } }
        public Sys_Updater()
        {
            m_Id = 0;
            m_Name = "";
            m_Version = "";
            m_Files = "";
            m_WhatsNew = "";
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
            arrayList.Add(new SqlParameter("@Version", m_Version));
            arrayList.Add(new SqlParameter("@Files", m_Files));
            arrayList.Add(new SqlParameter("@WhatsNew", m_WhatsNew));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            arrayList.Add(new SqlParameter("@Name", m_Name));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Sys_Updater", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Sys_Updater", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Sys_Updater where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_Version = DBTool.GetStringFromRow(row, "Version", "");
                m_Files = DBTool.GetStringFromRow(row, "Files", "");
                m_WhatsNew = DBTool.GetStringFromRow(row, "WhatsNew", "");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_Name = DBTool.GetStringFromRow(row, "Name", "");
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取 内部平台最新版本
        /// </summary>
        /// <returns></returns>
        public string ReadLastVersion()
        {
            string sql = "select max([version]) as [version] from Sys_Updater where Name = 'PlatForm' ";
            DataSet ds = m_dbo.GetDataSet(sql);
            return DBTool.GetStringFromRow(ds.Tables[0].Rows[0], "version", "");
        }
        /// <summary>
        /// 获取内部平台 最新版本所含的文件
        /// </summary>
        /// <returns></returns>
        public string ReadNewFiles()
        {
            string sql = " select top 1 Files from sys_updater where  Name = 'PlatForm' order by Id desc ";
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return DBTool.GetStringFromRow(ds.Tables[0].Rows[0], "Files", "");
            }
            else return "";
        }

        /// <summary>
        /// 获取 供应商 平台最新版本
        /// </summary>
        /// <returns></returns>
        public string ReadSupplierLastVersion()
        {
            string sql = "select max([version]) as [version] from Sys_Updater where Name = 'Supplier' ";
            DataSet ds = m_dbo.GetDataSet(sql);
            return DBTool.GetStringFromRow(ds.Tables[0].Rows[0], "version", "");
        }
        /// <summary>
        /// 获取 供应商 平台 最新版本所含的文件
        /// </summary>
        /// <returns></returns>
        public string ReadSupplierNewFiles()
        {
            string sql = " select top 1 Files from sys_updater where  Name = 'Supplier' order by Id desc ";
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return DBTool.GetStringFromRow(ds.Tables[0].Rows[0], "Files", "");
            }
            else return "";
        }


 
    }
}
