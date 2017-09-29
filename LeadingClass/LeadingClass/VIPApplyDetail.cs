using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class VIPApplyDetail
    {
        public int Id { get; set; }
        public int ApplyId { get; set; }
        public int GoodsId { get; set; }
        public string Model { get; set; }
        public int Num { get; set; }
        public double VIPPrice { get; set; }
        private DBOperate m_dbo;

        public VIPApplyDetail()
        {
            Id = 0;
            ApplyId = 0;
            GoodsId = 0;
            Model = "";
            Num = 0;
            VIPPrice = 0;
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
            arrayList.Add(new SqlParameter("@Num", Num));
            arrayList.Add(new SqlParameter("@VIPPrice", VIPPrice));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("VIPApplyDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("VIPApplyDetail", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from VIPApplyDetail where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                ApplyId = DBTool.GetIntFromRow(row, "ApplyId", 0);
                GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                Model = DBTool.GetStringFromRow(row, "Model", "");
                Num = DBTool.GetIntFromRow(row, "Num", 0);
                VIPPrice = DBTool.GetDoubleFromRow(row, "VIPPrice", 0);
                return true;
            }
            return false;
        }
        public bool GetAllApplyDetailByApplyId()
        { 
            string sql =string.Format("select * from VIPApplyDetail where ApplyId={0}", ApplyId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                ApplyId = DBTool.GetIntFromRow(row, "ApplyId", 0);
                GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                Model = DBTool.GetStringFromRow(row, "Model", "");
                Num = DBTool.GetIntFromRow(row, "Num", 0);
                VIPPrice = DBTool.GetDoubleFromRow(row, "VIPPrice", 0);
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from VIPApplyDetail where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 根据applyId得到申请单明细
        /// </summary>
        /// <returns></returns>
        public DataSet GetApplyDetailByApplyId()
        { 
            string sql =string.Format("select * from VIPApplyDetail where ApplyId={0}", ApplyId);     
            return m_dbo.GetDataSet(sql);
        }
        public DataSet GetApplyDetailByApplyId(List<int> applyIds)
        {
            string sql = "select * from View_VIPApplyDetail where 1=1";
            if (applyIds != null)
            {
                sql += " and ApplyId in ( ";
                for (int i = 0; i < applyIds.Count; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", applyIds[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", applyIds[i]);
                }
                sql += " ) ";
            }
            sql += " Order by ApplyId";
            return m_dbo.GetDataSet(sql);
        }
    }
}
