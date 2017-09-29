using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class VIPApply
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string Status { get; set; }
        public double SumMoney { get; set; }
        public int MemberAddressId { get; set; }
        public string Memo { get; set; }
        public DateTime UpdateTime { get; set; }
        public int ConfirmLevel { get; set; }
        public int OperatorId { get; set; }
        public int NewDeptId { get; set; }
        private DBOperate m_dbo;

        public VIPApply()
        {
            Id = 0;
            MemberId = 0;
            Status = "";
            SumMoney = 0;
            MemberAddressId = 0;
            Memo = "";
            UpdateTime = DateTime.Now;
            ConfirmLevel = 0;
            OperatorId = 0;
            NewDeptId = 0;
            m_dbo = new DBOperate();
        }
        public VIPApply(int Id)
        {
            m_dbo= new DBOperate();
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
            arrayList.Add(new SqlParameter("@MemberId", MemberId));
            arrayList.Add(new SqlParameter("@Status", Status));
            arrayList.Add(new SqlParameter("@SumMoney", SumMoney));
            arrayList.Add(new SqlParameter("@MemberAddressId", MemberAddressId));
            arrayList.Add(new SqlParameter("@Memo", Memo));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            arrayList.Add(new SqlParameter("@ConfirmLevel", ConfirmLevel));
            arrayList.Add(new SqlParameter("@OperatorId", OperatorId));
            arrayList.Add(new SqlParameter("@NewDeptId", NewDeptId));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("VIPApply", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("VIPApply", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from VIPApply where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);
                Status = DBTool.GetStringFromRow(row, "Status", "");
                SumMoney = DBTool.GetDoubleFromRow(row, "SumMoney", 0);
                MemberAddressId = DBTool.GetIntFromRow(row, "MemberAddressId", 0);
                Memo = DBTool.GetStringFromRow(row, "Memo", "");
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                ConfirmLevel = DBTool.GetIntFromRow(row, "ConfirmLevel",0);
                OperatorId = DBTool.GetIntFromRow(row, "OperatorId", 0);
                NewDeptId = DBTool.GetIntFromRow(row, "NewDeptId", 0); 
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from  VIPApplyDetail where ApplyId={0};Delete from VIPApply where id={0};", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 读取申请单明细
        /// </summary>
        /// <returns></returns>
        public DataSet ReadDetail()
        {
            string sql = string.Format(" select * from View_VIPApplyDetail where ApplyId={0} order by DisplayName", Id);
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 删除申请但明细
        /// </summary>
        /// <returns></returns>
        public bool DeleteDetail()
        {
            string sql = string.Format(" delete from VIPApplyDetail where ApplyId={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 判断是否由申请单转成订单  add  by luochunhui 10-10
        /// </summary>
        /// <returns></returns>
        public DataSet CheckIsOrer(int[] ids)
        { 
            string sql = string.Format(" select * from VIPApply where 1=1");
            if (ids != null)
            {
                sql += " and Id in ( ";
                for (int i = 0; i < ids.Length; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", ids[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", ids[i]);
                }
                sql += " ) ";
            }
            return m_dbo.GetDataSet(sql);
        }
        public DataSet ReadApplyStatus(DateTime ApplyStartTime,DateTime ApplyEndTime) 
        {
            string sql = "select * from VIPApply where 1=1";
            if (Status != "")
            {
                string[] status = Status.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                sql += " and Status in ( ";
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
            if (MemberId > 0)
            {
                sql += string.Format(" and MemberId={0}",MemberId);
            }
            if (ApplyStartTime != null)
            {
                sql += string.Format(" and UpdateTime >='{0}'", ApplyStartTime);
            }
            if (ApplyEndTime != null)
            {
                sql += string.Format(" and UpdateTime <='{0}'", ApplyEndTime);
            }
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 检查该条审核单是否已被审核
        /// </summary>
        /// <param name="applyId"></param>
        /// <param name="confirmLevel"></param>
        /// <returns></returns>
        public DataSet ReadIsConfirmOrder(int[] applyId,int confirmLevel)
        {
            string sql = "select * from VIPApply where 1=1";// and (Status='已审核' or (ConfirmLevel=0 and Status='待审核' ));
            if (applyId != null)
            {
                sql += " and Id in ( ";
                for (int i = 0; i < applyId.Length; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", applyId[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", applyId[i]);
                }
                sql += " ) ";
            }
            sql += string.Format(" and (Status='已审核' or (ConfirmLevel<{0} and Status='待审核' ));",confirmLevel);
            return m_dbo.GetDataSet(sql);
        }
    }

    public class ViewVIPApply
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int ComId { get; set; }
        public string CompanyName { get; set; }
        public int MemberId { get; set; }
        public string Memo { get; set; }
        public double SumMoney { get; set; }
        public string Mobile { get; set; }
        public string RealName { get; set; }
        public string Telphone { get; set; }
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public string Status { get; set; }
        public int MemberAddressId { get; set; }

        public ViewVIPApply(int Id)
        {
            this.Id = Id;
            DBOperate m_dbo = new DBOperate();
            string sql = string.Format(@"select Id,ReceiveAddress as Address,ComId,MemberId,Memo,SumMoney, ReceiveMobile as Mobile,ReceiveRealName as RealName,
ReceiveTelPhone as Telphone,DeptId,Status,Name as CompanyName,DeptName, MemberAddressId
from View_VIPApply where Id={0} ", Id);
            
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                ComId = DBTool.GetIntFromRow(row, "ComId", 0);
                CompanyName = DBTool.GetStringFromRow(row, "CompanyName", ""); 
                MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);

                DeliveryLineManager dlm = new DeliveryLineManager();
                DeliveryLine line = dlm.LoadDeliveryLine(MemberId);
                if (line.LineName != "")
                    Address = "[" + line.LineName + "]" + DBTool.GetStringFromRow(row, "Address", "");
                else
                    Address = DBTool.GetStringFromRow(row, "Address", "");
                Memo = DBTool.GetStringFromRow(row, "Memo", "");
                SumMoney = DBTool.GetDoubleFromRow(row, "SumMoney", 0);
                Mobile = DBTool.GetStringFromRow(row, "Mobile", "");
                RealName = DBTool.GetStringFromRow(row, "RealName", "");
                Telphone = DBTool.GetStringFromRow(row, "Telphone", "");
                DeptId = DBTool.GetIntFromRow(row, "DeptId", 0);
                DeptName = DBTool.GetStringFromRow(row, "DeptName", "");
                Status = DBTool.GetStringFromRow(row, "Status", "");
                MemberAddressId = DBTool.GetIntFromRow(row, "MemberAddressId", 0);
            }
        }
    }
}
