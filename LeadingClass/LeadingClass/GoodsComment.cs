using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace LeadingClass
{
    public class Goodscomment
    {
        public int Id { get; set; }
        public int GoodsId { get; set; }
        public int OrderId { get; set; }
        public int MemberId { get; set; }
        public int Level { get; set; }
        public string Label { get; set; }
        public string Content { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public Goodscomment()
        {
            Id = 0;
            GoodsId = 0;
            OrderId = 0;
            MemberId = 0;
            Level = 0;
            Label = "";
            Content = "";
            OrderTime = DateTime.Now;
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
            arrayList.Add(new SqlParameter("@GoodsId", GoodsId));
            arrayList.Add(new SqlParameter("@OrderId", OrderId));
            arrayList.Add(new SqlParameter("@MemberId", MemberId));
            arrayList.Add(new SqlParameter("@Level", Level));
            arrayList.Add(new SqlParameter("@Label", Label));
            arrayList.Add(new SqlParameter("@Content", Content));
            arrayList.Add(new SqlParameter("@OrderTime", OrderTime));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("GoodsComment", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("GoodsComment", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }

        /// <summary>
        /// 检查是否已经评论
        /// </summary>
        /// <param name="MemberId"></param>
        /// <param name="OrderId"></param>
        /// <param name="GoodsId"></param>
        /// <returns></returns>
        public int CheckIsSubmit(int MemberId, int OrderId, int GoodsId)
        {
            string sql = " select * from GoodsComment where 1=1";
            if (MemberId > 0)
            {
                sql += string.Format(" and MemberId={0} ", MemberId);
            }
            if (OrderId > 0)
            {
                sql += string.Format(" and OrderId={0} ", OrderId);
            }
            if (GoodsId > 0)
            {
                sql += string.Format(" and GoodsId={0} ", GoodsId);
            }

            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return 1;
            }
            return 0;
        }

        public bool Load()
        {
            string sql = string.Format("select * from GoodsComment where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
                OrderId = DBTool.GetIntFromRow(row, "OrderId", 0);
                MemberId = DBTool.GetIntFromRow(row, "MemberId", 0);
                Level = DBTool.GetIntFromRow(row, "Level", 0);
                Label = DBTool.GetStringFromRow(row, "Label", "");
                Content = DBTool.GetStringFromRow(row, "Content", "");
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from GoodsComment where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
    public class GoodsCommentManager
    {
        private DBOperate m_dbo;
        public GoodsCommentManager()
        {
            m_dbo = new DBOperate();
        }
        /// <summary>
        /// 读取商品品论
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataSet ReadGoodsComment(GoodsCommentOption option)
        {
            string sql = " select * from View_GoodsComment where 1=1 ";
            if (option.EndDate != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and UpdateTime < '{0}' ", option.EndDate.AddDays(1).ToShortDateString());
            }
            if (option.GoodsId > 0)
            {
                sql += string.Format(" and GoodsId={0} ", option.GoodsId);
            }
            if (option.KeyWords != "")
            {
                sql += string.Format(" and Content like '%{0}%' ", option.KeyWords);
            }
            if (option.Label != "")
            {
                sql += string.Format(" and Label = '{0}' ", option.Label);
            }
            if (option.Level > 0)
            {
                switch (option.Level)
                {
                    case 5:
                        sql += string.Format(" and ( Level = 5 or Level=4)", option.Level);
                        break;
                    case 3:
                        sql += string.Format(" and ( Level = 3 or Level=2)", option.Level);
                        break;
                    case 1:
                        sql += string.Format(" and  Level = 1", option.Level);
                        break;
                    default: 
                        break;
                }
            }
            if (option.MemberId > 0)
            {
                sql += string.Format(" and MemberId={0} ", option.MemberId);
            }
            if (option.StartDate != new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and UpdateTime >='{0}' ", option.StartDate.ToShortDateString());
            }

            sql += " order by UpdateTime Desc ";
            //分页控制
            if (option.recordControl.PageSize == 0)
            {
                return m_dbo.GetDataSet(sql);
            }
            else return m_dbo.GetDataSet(sql, option.recordControl.StartRecord, option.recordControl.PageSize);
        }
       
        /// <summary>
        /// 读取商品品论的数量
        /// </summary>
        /// <param name="GoodsId"></param>
        /// <returns></returns>
        public GoodsCommentAbstract ReadGoodsCommentAbstract(int GoodsId)
        {
            string sql = string.Format(" select COUNT(GoodsId) as 'Count',[Level] from view_GoodsComment where GoodsId={0} group by Level ", GoodsId);
            DataSet ds = m_dbo.GetDataSet(sql);
            GoodsCommentAbstract gca = new GoodsCommentAbstract();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int level = DBTool.GetIntFromRow(row, "Level", 0);
                int count = DBTool.GetIntFromRow(row, "Count", 0);
                switch (level)
                {
                    case 1:
                        gca.BadCommentNum += count;
                        break;
                    case 2:
                        gca.MiddelCommentNum += count;
                        break;
                    case 3:
                        gca.MiddelCommentNum += count;
                        break;
                    case 4:
                        gca.GoodCommentNum += count;
                        break;
                    case 5:
                        gca.GoodCommentNum += count;
                        break;
                    default:
                        break;
                }
            }
            gca.AllCommentNum = gca.BadCommentNum + gca.MiddelCommentNum + gca.GoodCommentNum;
            return gca;
        }
    }

    public class GoodsCommentOption
    {
        public int GoodsId { get; set; }
        public int MemberId { get; set; }
        public int Level { get; set; }
        public string Label { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string KeyWords { get; set; }
        public RecordControl recordControl { get; set; }

        public GoodsCommentOption()
        {
            GoodsId = 0;
            MemberId = 0;
            Level = 0;
            Label = "";
            StartDate = new DateTime(1900, 1, 1);
            EndDate = new DateTime(1900, 1, 1);
            KeyWords = "";
            recordControl = new RecordControl();
        }
    }

    public class GoodsCommentAbstract
    {
        public int GoodCommentNum { get; set; }
        public int MiddelCommentNum { get; set; }
        public int BadCommentNum { get; set; }
        public int AllCommentNum { get; set; }
    }
   
}
