using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LeadingClass
{
    public class DeliveryLineManager
    {
        private DBOperate m_dbo;

        public DeliveryLineManager()
        {
            m_dbo = new DBOperate();
        }

        public DataSet ReadDeliveryLine()
        {
            string sql = " select * from dbo.DeliveryLine  order by Id ";
            return m_dbo.GetDataSet(sql);
        }

        public DataSet ReadDeliveryLineByBranch(int branchId)       {
           

            string sql = string.Format(" select * from dbo.DeliveryLine where BranchId={0} order by Id ", branchId);
            return m_dbo.GetDataSet(sql);
        }


        /// <summary>
        /// 读取公司联系人的配送路线
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataSet ReadDeliveryMemberLine(int companyId)
        {
            string sql = string.Format(@" select * from View_MemberDeliveryLine  where ComId={0}", companyId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 通过公司Id，获取之前是否为公司分配过路线
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DeliveryCompanyLine LoadDeliveryCompyLinebyComId(int companyId)
        {
            DeliveryCompanyLine dcl = new DeliveryCompanyLine();
            string sql = "select * from dbo.DeliveryCompanyLine where CompanyId =" + companyId;
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                dcl.Id = DBTool.GetIntFromRow(row, "Id", 0);
                dcl.CompanyId = DBTool.GetIntFromRow(row, "CompanyId", 0);
                dcl.LineId = DBTool.GetIntFromRow(row, "LineId", 0);
                dcl.Remark = DBTool.GetStringFromRow(row, "Remark", "");
                dcl.UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                dcl.Distance = DBTool.GetIntFromRow(row, "Distance", 0);
                return dcl;
            }
            else
            {
                return null;
            }
        }

        //通过联系人Id，获取之前是否为联系人分配过路线
        public DeliveryMemberLine LoadDeliveryMemLinebyMemId(int memberId)
        {
            DeliveryMemberLine dml = new DeliveryMemberLine();
            string sql = "select * from dbo.DeliveryMemberLine where MemberId =" + memberId;
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                dml.Id = DBTool.GetIntFromRow(row, "Id", 0);
                dml.MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);
                dml.LineId = DBTool.GetIntFromRow(row, "LineId", 0);
                dml.Remark = DBTool.GetStringFromRow(row, "Remark", "");
                dml.UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                dml.Distance = DBTool.GetIntFromRow(row, "Distance", 0);
                return dml;
            }
            return null;
        }

        /// <summary>
        /// 获取联系人或者公司对应的线路
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="companyId"></param>
        public DeliveryLine LoadDeliveryLine(int memberId)
        {
            DeliveryLine line = new DeliveryLine();
            DeliveryMemberLine dml = LoadDeliveryMemLinebyMemId(memberId);
            if (dml != null)
            {
                line.Id = dml.LineId;
                line.Load();
            }
            else//读取公司路线
            {
                Member member = new Member();
                member.Id = memberId;
                member.Load();
                DeliveryCompanyLine dcl = LoadDeliveryCompyLinebyComId(member.ComId);
                if (dcl != null)
                {
                    line.Id = dcl.LineId;
                    line.Load();
                }
            }
            return line;
        }
    }
}
