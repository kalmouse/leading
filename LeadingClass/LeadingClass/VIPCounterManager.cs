using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    public class VIPCounterManager
    {
        
        public VIPCounterOption option { get; set; }
        public PageModel pageModel { get; set; }
        public DBOperate m_dbo;
        public VIPCounterManager()
        {
            m_dbo = new DBOperate();
            option = new VIPCounterOption();
            pageModel = new PageModel();
        }
        /// <summary>
        /// 读取专柜明细
        /// </summary>
        /// <param name="comId"></param>
        /// <returns></returns>
        public DataSet ReadVIPCounterDetail()
        {
            string sql = "select * from view_VIPCounter where 1=1 ";
            if (option.ComId > 0)
            {
                sql += string.Format("and comId={0} ", option.ComId);
            }
            if (option.Code != "")
            {
                sql += string.Format(" and Code like '{0}%' ", option.Code);
            }
            if (option.Key != "")
            {
                if (option.Key.Length > 2)
                {
                    option.Key = CommenClass.StringTools.SplitKeyWords(option.Key);
                }
                char[] sep = { ' ' };
                string[] kws = option.Key.Split(sep, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < kws.Length; i++)
                {
                    sql += string.Format(" and ((DisplayName like '%{0}%') or  (PY like '%{0}%') ) ", kws[i]);
                }
            }
            sql += " order by DisplayName ";
            if (option.IsUseRecorControl == 1)
            {
                return m_dbo.GetDataSet(sql, option.recordContorl.StartRecord, option.recordContorl.PageSize);
            }
            else
            {
                return m_dbo.GetDataSet(sql);
            }
        }

        /// <summary>
        /// 按照专柜id读取专柜明细
        /// </summary>
        /// <returns></returns>
        public DataSet ReadVIPCounterList()
        {
            string sql = "select * from View_VIPCounterDetail where 1=1 ";
            if (option.CounterId > 0)
            {
                sql += string.Format(" and CounterId={0}", option.CounterId);
            }
            if (option.Code != "")
            {
                sql += string.Format(" and Code like '{0}%' ", option.Code);
            }
            if (option.Key != "")
            {
                if (option.Key.Length > 2)
                {
                    option.Key = CommenClass.StringTools.SplitKeyWords(option.Key);
                }
                char[] sep = { ' ' };
                string[] kws = option.Key.Split(sep, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < kws.Length; i++)
                {
                    sql += string.Format(" and ((DisplayName like '%{0}%') or  (PY like '%{0}%') ) ", kws[i]);
                }
            }
            sql += " order by GoodsId desc  ";
            if (option.IsUseRecorControl == 1)
            {
                return m_dbo.GetDataSet(sql, option.recordContorl.StartRecord, option.recordContorl.PageSize);
            }
            else
            {
                return m_dbo.GetDataSet(sql);
            }
        }

        public DataSet ReadVIPCounterDetailForCache()
        {
            StringBuilder stb = new StringBuilder();
            stb.Append("select GoodsId as Id,'【'+convert(varchar(10),Recommend)+'】'+ convert(varchar(10),GoodsId)+' '+DisplayName as DisplayName,PY,Recommend,0 as Num from view_VIPCounter where ParentId<>2 and IsShelves=1 ");
            if (option.ComId > 0)
            {
                stb.Append(string.Format(" and ComId={0} ", option.ComId));
            }
            stb.Append(" UNION ALL select distinct g.ID,'【'+convert(varchar(10),g.Recommend)+'】'+ convert(varchar(10),g.ID)+' '+g.DisplayName as DisplayName,g.PY,g.Recommend,0 as Num from Goods g left join  View_VIPCounter vv on vv.GoodsId=g.ParentId where vv.ParentId =2 and g.IsShelves=1");
            if (option.ComId > 0)
            {
                stb.Append(string.Format(" and vv.comId={0} ", option.ComId));
            }
            return m_dbo.GetDataSet(stb.ToString());
        }


       




        /// <summary>
        /// 根据GoodsId 和 ComId 读取该商品的专柜价格、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、购物车详细信息
        /// </summary>
        /// <param name="ComId"></param>
        /// <param name="GoodsId"></param>
        /// <returns>-1:专柜无此商品</returns>
        public double ReadVIPPrice(int ComId, int GoodsId, int ParentId)
        {
            string sql = string.Format("select VIPPrice from View_VIPCounter where ComId ={0} and GoodsId={1} ", ComId, GoodsId);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return DBTool.GetDoubleFromRow(ds.Tables[0].Rows[0], "VIPPrice", 0);
            }
            else
            {
                string sql2 = string.Format("select VIPPrice from View_VIPCounter where ComId ={0} and GoodsId={1} ", ComId, ParentId);
                DataSet dataset = m_dbo.GetDataSet(sql2);
                if (dataset.Tables[0].Rows.Count == 1)
                {
                    return DBTool.GetDoubleFromRow(dataset.Tables[0].Rows[0], "VIPPrice", 0);
                }
                return -1;
            }
        }
        /// <summary>
        /// 、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、商品详情
        /// </summary>
        /// <param name="ComId"></param>
        /// <param name="GoodsId"></param>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public DataSet ReadVipInfo(int ComId, int GoodsId, int ParentId)
        {
            string sql = string.Format(@"select VIPPrice,IsSales from View_VIPCounter where ComId ={0} and GoodsId={1};select VIPPrice,IsSales from View_VIPCounter where ComId ={0} and GoodsId={2} ", ComId, GoodsId, ParentId);
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 用来判断加入专柜的是子商品还是母商品 、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、、
        /// </summary>
        /// <param name="ComId"></param>
        /// <param name="GoodsId"></param>
        /// <returns></returns>
        public DataSet ReadDataRow(int ComId, int ParentId)
        {
            string sql = string.Format("select * from View_VIPCounter where ComId ={0} and GoodsId={1} ", ComId, ParentId);
            return m_dbo.GetDataSet(sql);
        }
    }
}
