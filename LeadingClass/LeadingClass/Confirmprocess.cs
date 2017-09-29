using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class Confirmprocess
    {
        public int Id { get; set; }
        public int ApplyId { get; set; }
        public int MemberId { get; set; }
        public string Status { get; set; }
        public string Memo { get; set; }
        public int ConfirmLevel { get; set; }
        public double Amount { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public Confirmprocess()
        {
            Id = 0;
            ApplyId = 0;
            MemberId = 0;
            Status = "";
            Memo = "";
            ConfirmLevel = 0;
            Amount = 0;
            UpdateTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@ApplyId", ApplyId));
            arrayList.Add(new SqlParameter("@MemberId", MemberId));
            arrayList.Add(new SqlParameter("@Status", Status));
            arrayList.Add(new SqlParameter("@Memo", Memo));
            arrayList.Add(new SqlParameter("@ConfirmLevel", ConfirmLevel));
            arrayList.Add(new SqlParameter("@Amount", Amount));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("ConfirmProcess", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("ConfirmProcess", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from ConfimProcess where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                ApplyId = DBTool.GetIntFromRow(row, "ApplyId", 0);
                MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);
                Status = DBTool.GetStringFromRow(row, "Status", "");
                Memo = DBTool.GetStringFromRow(row, "Memo", "");
                ConfirmLevel = DBTool.GetIntFromRow(row, "ConfirmLevel", 0);
                Amount = DBTool.GetDoubleFromRow(row, "Amount", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from ConfimProcess where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 根据申请单号查找审核的过程
        /// </summary>
        /// <returns></returns>
        public DataSet GetConfirmProcessByApplyId()
        {
            string sql = "select c.*,m.RealName from ConfirmProcess c inner join Member m on c.MemberId=m.Id where 1=1";
            if (this.ApplyId > 0)
            {
                sql +=string.Format(" and ApplyId ={0}",this.ApplyId);
            }
            sql += " order by Id desc";
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 获取我审核的申请单
        /// </summary>
        /// <returns></returns>
        public DataSet GetConfirmPassByMemberId()//,v.UpdateTime
        {
            string sql = "select v.Id,v.MemberId,v.Status,c.Amount,m.RealName,m.Address,c.Memo,c.UpdateTime from ConfirmProcess c inner join VIPApply v on c.ApplyId = v.Id inner join MemberAddress m on v.MemberAddressId =m.Id where 1=1";
            if (MemberId > 0)
            {
                sql += string.Format(" and c.MemberId={0}", MemberId);
            }
            if (Status != "")
            {
                string[] status = Status.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                sql += " and v.Status in ( ";
                for (int i = 0; i < status.Length; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", status[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", status[i]);
                }
                sql += " ) ";
            }
            sql += " order by c.UpdateTime desc";
            return m_dbo.GetDataSet(sql);
        }
    }

}
