using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class GoodsShelvesLog
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int GoodsId { get; set; }
        public int OldNum { get; set; }
        public int NewNum { get; set; }
        public string StoreZone { get; set; }
        public int UserId { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;
        public GoodsShelvesLog()
        {
            m_dbo = new DBOperate();
            StoreId = 0;
            GoodsId = 0;
            OldNum = 0;
            NewNum = 0;
            StoreZone = "";
            UserId = 0;
            UpdateTime = DateTime.Now;
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (this.Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", this.Id));
            }
            arrayList.Add(new SqlParameter("@StoreId", this.StoreId));
            arrayList.Add(new SqlParameter("@GoodsId", this.GoodsId));
            arrayList.Add(new SqlParameter("@OldNum", this.OldNum));
            arrayList.Add(new SqlParameter("@NewNum", this.NewNum));
            arrayList.Add(new SqlParameter("@StoreZone", this.StoreZone));
            arrayList.Add(new SqlParameter("@UserId", this.UserId));
            arrayList.Add(new SqlParameter("@UpdateTime", this.UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("GoodsShelvesLog", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("GoodsShelvesLog", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;

        }
        public bool Load()
        {
            string sql = string.Format("select * from GoodsShelvesLog where Id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                return LoadFromRow(row);
            }
            return false;
        }
        private bool LoadFromRow(DataRow row)
        {
            Id = DBTool.GetIntFromRow(row, "Id", 0);

            StoreId = DBTool.GetIntFromRow(row, "StoreId", 0);
            GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
            OldNum = DBTool.GetIntFromRow(row, "OldNum", 0);
            NewNum = DBTool.GetIntFromRow(row, "NewNum", 0);
            StoreZone = DBTool.GetStringFromRow(row, "StoreZone", "");
            UserId = DBTool.GetIntFromRow(row, "UserId", 0);
            UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
            return true;
        }

        public DataSet GetGoodsShelvesLog(int storeId,int userId)
        {
            string sql = "";
            if (userId == 77)
            {
                sql = string.Format(@"select top 100 gsl.StoreId,gsl.GoodsId,gsl.OldNum,gsl.NewNum,gsl.StoreZone,gsl.UserId,users.Name,gsl.UpdateTime from GoodsShelvesLog gsl inner join Sys_Users users on gsl.userId=users.Id  where StoreId in({0},2) order by UpdateTime desc", storeId);
            }
            else {
                sql = string.Format(@"select top 100 gsl.StoreId,gsl.GoodsId,gsl.OldNum,gsl.NewNum,gsl.StoreZone,gsl.UserId,users.Name,gsl.UpdateTime from GoodsShelvesLog gsl inner join Sys_Users users on gsl.userId=users.Id  where StoreId={0} order by UpdateTime desc", storeId);
            }
            return m_dbo.GetDataSet(sql);
        }
    }
}
