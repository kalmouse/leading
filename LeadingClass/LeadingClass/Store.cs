using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class Store
    {
        private int m_Id;
        private int m_BranchId;
        private string m_Name;
        private int m_IsAvalible;
        private string m_Place;
        private DateTime m_UpdateTime;
        private int m_IsDefault;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int BranchId { get { return m_BranchId; } set { m_BranchId = value; } }
        public string Name { get { return m_Name; } set { m_Name = value; } }
        public int IsAvalible { get { return m_IsAvalible; } set { m_IsAvalible = value; } }
        public string Place { get { return m_Place; } set { m_Place = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public int IsDefault { get { return m_IsDefault; } set { m_IsDefault = value; } }
        public Store()
        {
            m_Id = 0;
            m_BranchId = 0;
            m_Name = "";
            m_IsAvalible = 0;
            m_Place = "";
            m_UpdateTime = DateTime.Now;
            m_IsDefault = 0;
            m_dbo = new DBOperate();
        }
        public Store(int Id)
            : this()
        {
            this.Id = Id;
            this.Load();
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
            arrayList.Add(new SqlParameter("@IsAvalible", m_IsAvalible));
            arrayList.Add(new SqlParameter("@Place", m_Place));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));
            arrayList.Add(new SqlParameter("@IsDefault", m_IsDefault));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("Store", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Store", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Store where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_BranchId = DBTool.GetIntFromRow(row, "BranchId", 0);
                m_Name = DBTool.GetStringFromRow(row, "Name", "");
                m_IsAvalible = DBTool.GetIntFromRow(row, "IsAvalible", 0);
                m_Place = DBTool.GetStringFromRow(row, "Place", "");
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                m_IsDefault = DBTool.GetIntFromRow(row, "IsDefault", 0);
                return true;
            }
            return false;
        } 
 
    }

    public class StoreManager
    {
        private DBOperate m_dbo;
        public StoreManager()
        {
            m_dbo = new DBOperate();
        }

        /// <summary>
        /// 读取branchId加盟商站点对应仓库
        /// 调用;FPurchase.cs
        /// </summary>
        /// <param name="branchId">branchId加盟商Id,branchId==0时，读取的是所有的仓库</param>
        /// <returns></returns>
        public DataSet ReadStores(int branchId)
        {
            string sql =string.Format( "select * from Store where 1=1 ");
            if (branchId > 0)
            {
                sql += string.Format(" and BranchId={0} " ,branchId);
            }
            sql += " order by IsDefault desc,Name ";
            return m_dbo.GetDataSet(sql);            
        }
        /// <summary>
        /// 获取分支机构的默认主仓库
        /// 应用;ERP;FNeedToBuy.cs
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public int GetDefaultStore(int branchId)
        {
            string sql = string.Format(" select Id from Store where BranchId={0} and IsDefault=1 ", branchId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return DBTool.GetIntFromRow(ds.Tables[0].Rows[0], "Id", 0);
            }
            else return 0;
        }
        /// <summary>
        /// 根据用户获取 自己的默认仓库，以后可能会修改
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public int GetUserDefaultStore(int UserId)
        {
            Sys_Users user = new Sys_Users(UserId);
            return GetDefaultStore(user.BranchId);
        }
    }
}
