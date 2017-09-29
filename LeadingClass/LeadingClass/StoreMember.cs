using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class StoreMember
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string RealName { get; set; }
        public string Telphone { get; set; }
        public DateTime RegisterDate { get; set; }
        public int ComId { get; set; }
        public int DeptId { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Email { get; set; }
        private DBOperate m_dbo;

        public StoreMember()
        {
            Id = 0;
            LoginName = "";
            RealName = "";
            Telphone = "";
            RegisterDate = DateTime.Now;
            ComId = 0;
            DeptId = 0;
            UpdateTime = DateTime.Now;
            Email = "";
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@LoginName", LoginName));
            arrayList.Add(new SqlParameter("@RealName", RealName));
            arrayList.Add(new SqlParameter("@Telphone", Telphone));
            arrayList.Add(new SqlParameter("@ComId", ComId));
            arrayList.Add(new SqlParameter("@DeptId", DeptId));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@Email", Email));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("StoreMember", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("StoreMember", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from StoreMember where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                LoginName = DBTool.GetStringFromRow(row, "LoginName", "");
                RealName = DBTool.GetStringFromRow(row, "RealName", "");
                Telphone = DBTool.GetStringFromRow(row, "Telphone", "");
                RegisterDate = DBTool.GetDateTimeFromRow(row, "RegisterDate");
                ComId = DBTool.GetIntFromRow(row, "ComId", 0);
                DeptId = DBTool.GetIntFromRow(row, "DeptId", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                Email = DBTool.GetStringFromRow(row, "Email", "");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from StoreMember where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        //根据公司部门查人员信息
        public DataSet showStoreMembers(int ComId, int DeptId)
        {
            string sql =string.Format( "select * from StoreMember where ComId={0} and DeptId={1}",ComId,DeptId);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 根据部门Id找到对应的人
        /// </summary>
        /// <returns></returns>
        public DataSet GetMemberByDeptId()
        {
            string sql = string.Format("select * from StoreMember where DeptId={0} and ComId={1}", DeptId, ComId);
            return m_dbo.GetDataSet(sql);
        }
    }
}
