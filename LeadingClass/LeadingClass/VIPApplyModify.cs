using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class VIPApplyModify
    {
        public int Id { get; set; }
        public int ApplyId { get; set; }
        public int GoodsId { get; set; }
        public string Model { get; set; }
        public int OldNum { get; set; }
        public int NewNum { get; set; }
        public int OperaterId { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public VIPApplyModify()
        {
            Id = 0;
            ApplyId = 0;
            GoodsId = 0;
            Model = "";
            OldNum = 0;
            NewNum = 0;
            OperaterId = 0;
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
            arrayList.Add(new SqlParameter("@GoodsId", GoodsId));
            arrayList.Add(new SqlParameter("@Model", Model));
            arrayList.Add(new SqlParameter("@OldNum", OldNum));
            arrayList.Add(new SqlParameter("@NewNum", NewNum));
            arrayList.Add(new SqlParameter("@OperaterId", OperaterId));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("VIPApplyModify", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("VIPApplyModify", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from VIPApplyModify where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                ApplyId = DBTool.GetIntFromRow(row, "ApplyId", 0);
                GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                Model = DBTool.GetStringFromRow(row, "Model", "");
                OldNum = DBTool.GetIntFromRow(row, "OldNum", 0);
                NewNum = DBTool.GetIntFromRow(row, "NewNum", 0);
                OperaterId = DBTool.GetIntFromRow(row, "OperaterId", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from VIPApplyModify where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 获取对应申请单号和操作人的对申请单明细的修改记录
        /// </summary>
        /// <returns></returns>
        public DataSet GetApplyModifyDetail()
        {
            string sql = "select v.*,m.RealName,g.DisplayName from VIPApplyModify v inner join Member m on v.OperaterId=m.Id inner join Goods g on v.GoodsId=g.ID where 1=1";
            if (this.ApplyId > 0)
            {
                sql += string.Format(" and ApplyId={0}",this.ApplyId);
            }
            if (this.OperaterId > 0)
            {
                sql += string.Format(" and OperaterId ={0}",this.OperaterId);
            }
            sql += " order by Id";
            return m_dbo.GetDataSet(sql);
        }
    }
}
