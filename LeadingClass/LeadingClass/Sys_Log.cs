using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Collections;

namespace LeadingClass
{
    public class Sys_Log
    {
        private int m_Id;
        private int m_UserId;
        private string m_OperateType;
        private string m_Operate;
        private int m_ObjectId;
        private DateTime m_UpdateTime;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public string OperateType { get { return m_OperateType; } set { m_OperateType = value; } }
        public string Operate { get { return m_Operate; } set { m_Operate = value; } }
        public int ObjectId { get { return m_ObjectId; } set { m_ObjectId = value; } }
        public DateTime UpdateTime { get { return m_UpdateTime; } set { m_UpdateTime = value; } }
        public Sys_Log()
        {
            m_Id = 0;
            m_UserId = 0;
            m_OperateType = "";
            m_Operate = "";
            m_ObjectId = 0;
            m_UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        /// <summary>
        /// 调用：ERP：FGoods.cs
        ///       ERP FOrderReceiveMoney.cs 中调用
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (m_Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", m_Id));
            }
            arrayList.Add(new SqlParameter("@UserId", m_UserId));
            arrayList.Add(new SqlParameter("@OperateType", m_OperateType));
            arrayList.Add(new SqlParameter("@Operate", m_Operate));
            arrayList.Add(new SqlParameter("@ObjectId", m_ObjectId));
            arrayList.Add(new SqlParameter("@UpdateTime", m_UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("Sys_Log", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Sys_Log", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Sys_Log where id={0}", m_Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                m_Id = DBTool.GetIntFromRow(row, "Id", 0);
                m_UserId = DBTool.GetIntFromRow(row, "UserId", 0);
                m_OperateType = DBTool.GetStringFromRow(row, "OperateType", "");
                m_Operate = DBTool.GetStringFromRow(row, "Operate", "");
                m_ObjectId = DBTool.GetIntFromRow(row, "ObjectId", 0);
                m_UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        /// <summary>
        /// 调用：ERP  FGoodsInfo.cs
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public DataSet LoadSys_LogByGoodsId(int goodsId)
        {
            string sql = string.Format(" select * from Sys_Log where ObjectId = {0} and OperateType like '%下架商品%' order by UpdateTime desc ", goodsId);
            return m_dbo.GetDataSet(sql);
        }
        
        
    }

    public class LogOption
    {
        private int m_UserId;
        private string m_OperateType;
        private DateTime m_StartDate;
        private DateTime m_EndDate;

        public int UserId { get { return m_UserId; } set { m_UserId = value; } }
        public string OperateType { get { return m_OperateType; } set { m_OperateType = value; } }
        public DateTime StartDate { get { return m_StartDate; } set { m_StartDate = value; } }
        public DateTime EndDate { get { return m_EndDate; } set { m_EndDate = value; } }
        public LogOption()
        {
            m_UserId = 0;
            m_OperateType = "";
            m_StartDate = DateTime.Now;
            m_EndDate = DateTime.Now;

        }
    }

    public class Sys_LogManager
    {
        private LogOption m_option;
        private DBOperate m_dbo;

        public LogOption option { get { return m_option; } set { m_option = value; } }
        public Sys_LogManager()
        {
            m_option = new LogOption();
            m_dbo = new DBOperate();
        }

        public DataSet ReadSysLog()
        {
            string sql = " select * from Sys_Log where 1=1 ";
            sql += string.Format(" and UpdateTime >= '{0}' and updateTime<'{1}' ", option.StartDate,option.EndDate);
            if (option.UserId > 0)
            {
                sql += string.Format(" and UserId={0} ", option.UserId);
            }
            if (option.OperateType != "")
            {
                sql += string.Format(" and OperateType = '{0}' ", option.OperateType);

            }
            sql += " order by UpdateTime desc";
            return m_dbo.GetDataSet(sql);
        }
    }
 
}
