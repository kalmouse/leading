using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class TPI_Messagepush
    {
        public int Id { get; set; }
        public int GoodsId { get; set; }
        public string CategoryCode { get; set; }
        public int ProjectId { get; set; }
        public int IsUserType { get; set; }
        public DateTime Updatetime { get; set; }
        public int Type { get; set; }
        public string Note { get; set; }
        private DBOperate m_dbo;

        public TPI_Messagepush()
        {
            Id = 0;
            GoodsId = 0;
            CategoryCode = "";
            ProjectId = 0;
            IsUserType = 0;
            Updatetime = DateTime.Now;
            Type = 0;
            Note = "";
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@GoodsId", GoodsId));
            arrayList.Add(new SqlParameter("@CategoryCode", CategoryCode));
            arrayList.Add(new SqlParameter("@ProjectId", ProjectId));
            arrayList.Add(new SqlParameter("@IsUserType", IsUserType));
            arrayList.Add(new SqlParameter("@Updatetime", Updatetime));
            arrayList.Add(new SqlParameter("@Type", Type));
            arrayList.Add(new SqlParameter("@Note", Note));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("TPI_MessagePush", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("TPI_MessagePush", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from TPI_MessagePush where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                CategoryCode = DBTool.GetStringFromRow(row, "CategoryCode", "");
                ProjectId = DBTool.GetIntFromRow(row, "ProjectId", 0);
                IsUserType = DBTool.GetIntFromRow(row, "IsUserType", 0);
                Updatetime = DBTool.GetDateTimeFromRow(row, "Updatetime");
                Type = DBTool.GetIntFromRow(row, "Type", 0);
                Note = DBTool.GetStringFromRow(row, "Note", "");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from TPI_MessagePush where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        //批量保存
        public int Save_message(int[] ids,string CategoryCode, int projectId)
        {
            int result = 0;
            for (int i = 0; i < ids.Length; i++)
            {
                if (ids[i] > 0)
                {
                    Goods goods = new Goods(ids[i]);
                    //TPI_Goods tpiGoods = new TPI_Goods();
                    TPI_Messagepush tm = new TPI_Messagepush();
                    tm.GoodsId = ids[i];
                    tm.ProjectId = ProjectId;
                    tm.CategoryCode = CategoryCode;
                    tm.Type = 6;
                    tm.Save();
                    result++;
                }
            }
            return result;
        }
        //通过type来查看商品的状态
        public DataSet ReadType( int[] type) 
        {
            string sql = string.Format("select* from  TPI_MessagePush where 1=1");
            if (type != null)
            {
                sql += "and Type in (";
                for (int i = 0; i < type.Length; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", type[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", type[i]);                  
                }
                sql += ")";
            }
            return m_dbo.GetDataSet(sql);
        
        }
        //删除推送的消息
        public DataSet ReadTypeId(int id)
        {
            string sql = string.Format("delete  from  TPI_MessagePush where Id ={0} ", id);
            return m_dbo.GetDataSet(sql);

        }
    }
}
