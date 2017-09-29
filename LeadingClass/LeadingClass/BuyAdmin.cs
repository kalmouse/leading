using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class BuyAdmin
    {
        public int Id { get; set; }
        public int ComId { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public int ChooseDeptId { get; set; }
        public string ChooseDeptName { get; set; }
        private DBOperate m_dbo;

        public BuyAdmin()
        {
            Id = 0;
            ComId = 0;
            MemberId = 0;
            MemberName = "";
            DeptId = 0;
            DeptName = "";
            ChooseDeptId = 0;
            ChooseDeptName = "";
            m_dbo = new DBOperate();
        }

        public BuyAdmin(int ComId)
        {
            m_dbo = new DBOperate();
            this.Id = Id;
            this.Load();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@ComId", ComId));
            arrayList.Add(new SqlParameter("@MemberId", MemberId));
            arrayList.Add(new SqlParameter("@MemberName", MemberName));
            arrayList.Add(new SqlParameter("@DeptId", DeptId));
            arrayList.Add(new SqlParameter("@DeptName", DeptName));
            arrayList.Add(new SqlParameter("@ChooseDeptId", ChooseDeptId));
            arrayList.Add(new SqlParameter("@ChooseDeptName", ChooseDeptName));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("BuyAdmin", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("BuyAdmin", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from BuyAdmin where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                Id = DBTool.GetIntFromRow(row, "ComId", 0);
                MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);
                MemberName = DBTool.GetStringFromRow(row, "MemberName", "");
                DeptId = DBTool.GetIntFromRow(row, "DeptId", 0);
                DeptName = DBTool.GetStringFromRow(row, "DeptName", "");
                ChooseDeptId = DBTool.GetIntFromRow(row, "ChooseDeptId", 0);
                ChooseDeptName = DBTool.GetStringFromRow(row, "ChooseDeptName", "");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from BuyAdmin where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 通过ComId找到本公司下面所有人员
        /// </summary>
        /// <returns></returns>
        public DataSet SearchByComId(int ComId)
        {
            string sql = string.Format("select * from BuyAdmin where ComId={0} Order by MemberId ", ComId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 通过ComId找到本公司下面所有人员
        /// </summary>
        /// <returns></returns>
        public DataSet SearchChooseNept(int SeeionMemberId)
        {
            string sql = string.Format("select * from BuyAdmin  where MemberId ={0}", SeeionMemberId);
            //string sql = string.Format("select ComId,MemberId,MemberName from BuyAdmin where ComId={0} group by comId,MemberId,MemberName", ComId );
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 通过ComId 和采购专员的 MemberId 找到绑定的部门
        /// </summary>
        /// <param name="ComId"></param>
        /// <param name="MemberId"></param>
        /// <returns></returns>
        public DataSet SearchByDeptId(int ComId, int MemberId)
        {
            string sql = string.Format("select * from BuyAdmin where ComId={0} and MemberId={1} ", ComId, MemberId);
            return m_dbo.GetDataSet(sql);
        }
    }
}
