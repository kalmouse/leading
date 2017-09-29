using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections;
using System.Data.SqlClient;

namespace LeadingClass
{
    /// <summary>
    /// Gifts 的摘要说明
    /// </summary>
    public class Gift
    {
        private DBOperate m_dbo;
        public int Id { get; set; }
        public int GoodsId { get; set; }
        public int Point { get; set; }
        public int MaxNum { get; set; }
        public int IsValid { get; set; }
        public string GiftName { get; set; }
        public int UserId { get; set; }
        public DateTime UpdateTime { get; set; }

        public Gift()
        {
            m_dbo = new DBOperate();
            Id = 0;
            GoodsId = 0;
            Point = 0;
            MaxNum = 0;
            IsValid = 1;
            GiftName = "";
            UserId = 0;
            UpdateTime = DateTime.Now;
        }
        public int save()
        {
            ArrayList arrayList = new ArrayList();
            if (this.Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", this.Id));
            }
            arrayList.Add(new SqlParameter("@GoodsId", this.GoodsId));
            arrayList.Add(new SqlParameter("@Point", this.Point));
            arrayList.Add(new SqlParameter("@MaxNum", this.MaxNum));
            arrayList.Add(new SqlParameter("@IsValid", this.IsValid));
            arrayList.Add(new SqlParameter("@GiftName", this.GiftName));
            arrayList.Add(new SqlParameter("@UserId", this.UserId));
            arrayList.Add(new SqlParameter("@UpdateTime", this.UpdateTime));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("Gift", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Gift", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;

        }
        public bool load()
        {
            string sql = string.Format("select * from Gift where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                return LoadFromRow(row);
            }
            return false;
        }
        public bool loadFromGoodsId(int goodsId)
        {
            string sql = string.Format("select * from Gift where goodsid={0}", goodsId);
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
            GoodsId = DBTool.GetIntFromRow(row, "GoodsId", 0);
            Point = DBTool.GetIntFromRow(row, "Point", 0);
            MaxNum = DBTool.GetIntFromRow(row, "MaxNum", 0);
            IsValid = DBTool.GetIntFromRow(row, "IsValid", 1);
            GiftName = DBTool.GetStringFromRow(row, "GiftName", "");
            UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
            UserId = DBTool.GetIntFromRow(row, "UserId", 0);
            return true;
        }       

        
        public bool delete(int goodsId)
        {
            string sql = string.Format("delete from Gift where goodsId={0}", goodsId);
            return m_dbo.ExecuteNonQuery(sql);
        }

        //public DataSet GetGiftSet()
        //{
        //    string sql = "select * from Gift order by Point";
        //    return m_dbo.GetDataSet(sql);
        //}

        //public DataSet GetGiftSet(int num)
        //{
        //    string sql = string.Format("select top {0} * from Gift order by Recommend,Point desc", num);
        //    return m_dbo.GetDataSet(sql);
        //}      
    }

    public class GiftManager
    {
        public GiftOption option { get; set; }
        private DBOperate m_dbo;
       
        public GiftManager()
        {
            m_dbo= new DBOperate();
            option = new GiftOption();
        }

/// <summary>
/// 
/// </summary>
/// <param name="option">orderby:0 recommend desc,point;1:point,recommend desc;2:point desc,recommend desc</param>
/// <returns></returns>
/// 
        public DataSet ReadGift()
        {
            string sql = string.Format("select  * from View_Gift where IsValid=1");

            if (option.Code != "")
            {
                sql += string.Format(" and Code like '{0}%' ", option.Code);
            }
            if (option.TypeId > 0)
            {
                sql += string.Format(" and TypeId={0} ", option.TypeId);
            }
            switch (option.OrderBy)
            {
                case 0:
                    sql += " order by Recommend Desc,Point ";
                    break;
                case 1:
                    sql += " order by Point Desc,Recommend Desc ";
                    break;
                case 2:
                    sql += " order by Point,Recommend Desc ";
                    break;
                default: 
                    sql += " order by Recommend desc, Point";
                    break;
            }
            sql += ";select @@ROWCOUNT";
            return m_dbo.GetDataSet(sql,( option.pageModel.CurrentPage-1)*option.pageModel.PageSize, option.pageModel.PageSize);
        }
        /// <summary>
        /// 读取礼品对应的2级分类中的数量
        /// </summary>
        /// <returns></returns>
        public DataSet ReadGiftCountInType2()
        {
            string sql = "select Typename2,Code2,COUNT(*) as Num from view_gift group by Typename2,Code2 order by Code2 ";
            return m_dbo.GetDataSet(sql);
        }
    }
    public class GiftOption
    {
        public int TopNum { get; set; }
        public int TypeId { get; set; }
        public string Code { get; set; }
        public string KeyWords { get; set; }
        public int OrderBy { get; set; }
        public PageModel pageModel { get; set; }
        public GiftOption()
        {
            TopNum = 0;
            TypeId = 0;
            Code = "";
            KeyWords = "";
            OrderBy = 0;
            pageModel = new PageModel();
        }
    }
    public class SwapGift
    {
        private int m_Id;
        private int m_MemberId;
        private int m_GiftId;
        private double m_Point;
        private int m_Num;
        private int m_IsOK;
        private DBOperate m_dbo;
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public int MemberId { get { return m_MemberId; } set { m_MemberId = value; } }
        public int GiftId { get { return m_GiftId; } set { m_GiftId = value; } }
        public double Point { get { return m_Point; } set { m_Point = value; } }
        public int Num { get { return m_Num; } set { m_Num = value; } }
        public int IsOk { get { return m_IsOK; } set { m_IsOK = value; } }

        public SwapGift()
        {
            m_Id = 0;
            m_MemberId = 0;
            m_GiftId = 0;
            m_Point = 0;
            m_Num = 0;
            m_IsOK = 0;
            m_dbo = new DBOperate();
        }
        public int save()
        {
            ArrayList arrayList = new ArrayList();
            if (this.Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", this.Id));
            }
            arrayList.Add(new SqlParameter("@MemberId", this.MemberId));
            arrayList.Add(new SqlParameter("@GiftId", this.GiftId));
            arrayList.Add(new SqlParameter("@Point", this.Point));
            arrayList.Add(new SqlParameter("@Num", this.Num));
            arrayList.Add(new SqlParameter("@IsOK", this.IsOk));

            if (this.Id > 0)
            {
                m_dbo.UpdateData("Gift", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Gift", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }

    }
}