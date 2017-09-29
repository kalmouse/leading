using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LeadingClass
{

    public class GoodsPoolOption
    {
        public List<int> GoodsIds { get; set; }
        public int ProjectId { get; set; }//项目编号
        public string KeyWords { get; set; }
        public string TypeCode { get; set; }//类别code用于查商品类别 赋值可能是 一级类别 二级类别或三级类别的code
        public int State { get; set; }//商品池的商品价状态
        public int PushState { get; set; }//主动推送商品的项目用到  查看推送状态 -1：没有推送或不用主动推送；0：推送但未成功； 1:推送成功
        public int IsUseTypeCompare { get; set; }//是否启用类别标识（0：没有启用，使用领先的typeId和TypeName；1：启用，使用tpi_category中的CategoryCode和Category两个字段； 2：使用领先的typeId和别的Category
        public int BranchId { get; set; }
        public DateTime StartTime { get; set; }//查询时间段
        public DateTime EndTime { get; set; }//查询时间段
        public int IsHideParentGoods { get; set; }//隐藏母商品
        public int IsHasHomeImage { get; set; }
        public int IsHasDetailPhoto { get; set; }
        public int IsHasDescription { get; set; }

        public int IsUseRecorControl { get; set; }//是否分页的开关

        public GoodsPoolOption()
        {
            GoodsIds = null;
            ProjectId = 0;
            KeyWords = "";
            TypeCode = "";
            State = -1;
            PushState = -2;
            IsUseTypeCompare = 0;
            BranchId = 0;
            StartTime = new DateTime(1900, 1, 1);
            EndTime = new DateTime(1900, 1, 1);
            IsHideParentGoods = 0;
            IsHasHomeImage = 0;
            IsHasDetailPhoto = 0;
            IsHasDescription = 0;
            IsUseRecorControl = 1;
        }
    }
    public class Goods_Iterface
    {
        private DBOperate m_dbo;
        public GoodsPoolOption SnapOption { get; set; }
        public PageModel PageModel { get; set; }
        public Goods_Iterface()
        {
            m_dbo = new DBOperate();
            SnapOption = new GoodsPoolOption();
            PageModel = new PageModel();
        }

        #region 商品池管理数据来源
        /// <summary>
        /// 接口添加商品池 Goods-->TPI_Goods  
        /// 根据BranchId 查出全部公有商品及相关网站私有商品
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllGoodsForInterface(int ProjectId, int BranchId, int IsUseTypeCompare, int IsUseBrandCompare)
        {
            string sql = @"select g.ID as GoodsId,g.DisplayName,gt.TypeName,g.SN,gt.Code,gt.PCode,g.TypeId,g.BranchId, g.Recommend,g.Description,g.HomeImage ,isnull(gb.Price,g.Price) as sysPrice ";
            if (IsUseTypeCompare >= 1)
            {
                sql += " ,Discount";
            }
            else
            {
                sql += " ,1 as Discount";
            }
            sql += string.Format("  from Goods g join GoodsType gt on g.TypeId = gt.ID left join GoodsBranch gb on (g.ID= gb.GoodsId and gb.BranchId={0} )", BranchId);
            if (IsUseTypeCompare >= 1)
            {
                sql += "  join TPI_Category tc on gt.ID=tc.TypeId ";
            }
            if (IsUseBrandCompare >= 1)
            {
                sql += "  join TPI_DSFBrand td on g.BrandId= td .BrandId  ";
            }
            sql += string.Format(" where IsShelves =1  and g.IsAllow =1 and g.IsVisible=1  and (g.IsPublic =1 or (g.IsPublic =0 and g.BranchId = {0}))", BranchId);
            if (SnapOption.IsHideParentGoods == 1)
            {
                //读取基本商品 和子商品
                sql += " and( parentId =0 or parentId >2 or ParentId =1 ) ";
            }
            else
            {
                //只读取基本商品和组合商品  
                sql += " and ParentId <=2 ";
            }
            if (ProjectId > 0)
            {
                sql += string.Format(" and g.ID not in (select GoodsId  from TPI_Goods where ProjectId={0})", ProjectId);
                if (IsUseTypeCompare >= 1)
                {
                    sql += string.Format(" and tc.ProjectId={0}", ProjectId);
                }
                if (IsUseBrandCompare >= 1)
                {
                    sql += string.Format(" and td.ProjectId={0}", ProjectId);
                }
            }
            if (SnapOption.IsHasHomeImage > 0)
            {
                sql += " and HomeImage<>'' ";
            }
            if (SnapOption.IsHasDescription > 0)
            {
                sql += " and Description like '%{%' ";
            }
            if (SnapOption.KeyWords != "" && SnapOption.KeyWords != null && SnapOption.KeyWords != "NULL")
            {
                if (SnapOption.KeyWords.Length > 2)
                {
                    SnapOption.KeyWords = CommenClass.StringTools.SplitKeyWords(SnapOption.KeyWords);
                }
                char[] sep = { ' ' };
                string[] kws = SnapOption.KeyWords.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < kws.Length; i++)
                {
                    sql += string.Format(" and ((g.DisplayName like '%{0}%') or  (g.ID like '%{0}%'))", kws[i]);
                }
            }
            if (SnapOption.TypeCode != "")
            {
                sql += string.Format(" and code like '{0}%' ", SnapOption.TypeCode);
            }

            sql += " order by code,Recommend desc; select @@ROWCOUNT as RowNum;";
            return m_dbo.GetDataSet(sql, (PageModel.CurrentPage - 1) * PageModel.PageSize, PageModel.PageSize);
        }

        /// <summary>
        /// 读取“修改商品”数据
        /// 关联数据表包含tpi_goods，TPI_Project，goodsbranch，Goods，GoodsType，TPI_Category
        /// </summary>
        /// <returns></returns>
        public DataSet GetTPIGoods()
        {
            //sql主代码
            string sql = "select g.DisplayName,gt.ID as TypeId,gt.TypeName,gt.Code,tg.AgreementPrice,tg.Sku,tg.GoodsId,tg.[State],tg.stock,tg.ProjectId,ISNULL (gb.Price,g.Price) as SysPrice";
            if (SnapOption.IsUseTypeCompare > 0)
            {
                sql += " ,tc.CategoryCode,tc.Category,tc.Discount";
            }
            sql += string.Format(@" from TPI_Goods tg  
left join GoodsBranch gb on gb.GoodsId=tg.GoodsId and gb.BranchId={0}
join Goods g on (tg.GoodsId=g.ID) 
join GoodsType gt on g.TypeId=gt.ID", SnapOption.BranchId);
            if (SnapOption.IsUseTypeCompare > 0)
            {
                sql += " join TPI_Category tc on(g.TypeId=tc.TypeId and tc.ProjectId=tg.ProjectId) ";
            }
            //sql条件部分
            sql += string.Format(" where tg.ProjectId={0} ", SnapOption.ProjectId);
            if (SnapOption.PushState >= -1)
            {
                sql += string.Format(" and tg.PushState={0}", SnapOption.PushState);
            }
            if (SnapOption.State > -1)
            {
                sql += string.Format(" and state ={0}", SnapOption.State);
            }
            if (SnapOption.KeyWords != "" && SnapOption.KeyWords != null && SnapOption.KeyWords != "NULL")
            {
                if (SnapOption.KeyWords.Length > 2)
                {
                    SnapOption.KeyWords = CommenClass.StringTools.SplitKeyWords(SnapOption.KeyWords);
                }
                char[] sep = { ' ' };
                string[] kws = SnapOption.KeyWords.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < kws.Length; i++)
                {
                    sql += string.Format(" and ((g.DisplayName like '%{0}%') or (tg.GoodsId like '%{0}%'))", kws[i]);
                }
            }
            if (SnapOption.GoodsIds != null)
            {
                sql += " and tg.GoodsId in ( ";
                for (int i = 0; i < SnapOption.GoodsIds.Count; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", SnapOption.GoodsIds[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", SnapOption.GoodsIds[i]);
                }
                sql += " ) ";
            }
            if (SnapOption.TypeCode != "" && SnapOption.TypeCode != "0")
            {
                sql += string.Format(" and Code like '{0}%'", SnapOption.TypeCode);
            }
            sql += " order by g.Id,g.typeId;select @@ROWCOUNT as RowNum;";
            if (SnapOption.IsUseRecorControl == 0)
            {
                return m_dbo.GetDataSet(sql);
            }
            else
            {
                return m_dbo.GetDataSet(sql, (PageModel.CurrentPage - 1) * PageModel.PageSize, PageModel.PageSize);
            }
        }

        #endregion

        #region 商品属性数据校验
        /// <summary>
        /// 商品属性展示页面
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public DataSet CheckGoodsPool(int ProjectId)
        {
            string sql = "with IsHaveDetailPhoto as";
            sql += string.Format(@"(select GoodsId, COUNT(distinct IsDetailPhoto)as IsDetailPhoto from GoodsPhoto group by GoodsId) 
                     select  t.GoodsId,t.SkuName,g.SN,t.ProjectId,g.HomeImage,g.Description,ParentId,IsHaveDetailPhoto.IsDetailPhoto 
                     from TPI_Goods t join Goods g on t.GoodsId=g.ID join IsHaveDetailPhoto on IsHaveDetailPhoto.GoodsId=g.ID where ProjectId={0} and t.State=1 ", ProjectId);

            if (SnapOption.IsHasHomeImage > 0)
            {
                sql += " and HomeImage=''";
            }
            else
            {
                sql += " and HomeImage<>''";
            }
            if (SnapOption.IsHasDetailPhoto > 0)
            {
                sql += " and IsHaveDetailPhoto.IsDetailPhoto<2";
            }
            else
            {
                sql += " and IsHaveDetailPhoto.IsDetailPhoto=2";
            }
            if (SnapOption.IsHasDescription > 0)
            {
                sql += " and Description not like '%{%' ";
            }
            else
            {
                sql += " and Description like '%{%' ";
            }
            sql += " group by t.GoodsId,t.SkuName,g.SN,t.ProjectId,g.HomeImage,g.Description,ParentId,IsHaveDetailPhoto.IsDetailPhoto Order by t.GoodsId";
            sql += " ;select @@rowcount as RowNum  ";
            return m_dbo.GetDataSet(sql, (PageModel.CurrentPage - 1) * PageModel.PageSize, PageModel.PageSize);
        }
        #endregion

        #region 河南商品数据处理   河南商品库，河南和领先匹配的商品数据

        /// <summary>
        /// 读取河南商城商品库
        /// </summary>
        /// <returns></returns>
        public DataSet ReadTPI_HNSCGoods()
        {
            string sql = string.Format("select * from TPI_HNSCGoods where XHMC not in ( select XHMC from TPI_GoodsCompare)");
            if (SnapOption.KeyWords != "" && SnapOption.KeyWords != null && SnapOption.KeyWords != "NULL")
            {
                if (SnapOption.KeyWords.Length > 2)
                {
                    SnapOption.KeyWords = CommenClass.StringTools.SplitKeyWords(SnapOption.KeyWords);
                }
                char[] sep = { ' ' };
                string[] kws = SnapOption.KeyWords.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < kws.Length; i++)
                {
                    sql += string.Format(" and ((XHMC like '%{0}%') or(PPMC like '%{0}%')or (PMMC like '%{0}%') )", kws[i]);
                }
            }
            sql += " order by  Id desc ;select @@ROWCOUNT as RowNum;";
            return m_dbo.GetDataSet(sql, (PageModel.CurrentPage - 1) * PageModel.PageSize, PageModel.PageSize);
        }
        /// <summary>
        /// 读取对应表数据
        /// </summary>
        /// <returns></returns>
        public DataSet ReadGoodsCompare()
        {
            string sql = string.Format("select * from TPI_GoodsCompare where 1=1");
            if (SnapOption.KeyWords == "已入库")
            {
                sql += string.Format("and IsInStore = 1");
            }
            else if (SnapOption.KeyWords == "未入库")
            {
                sql += string.Format("and IsInStore = 0");
            }
            else if (SnapOption.KeyWords != "" && SnapOption.KeyWords != null && SnapOption.KeyWords != "NULL")
            {
                if (SnapOption.KeyWords.Length > 2)
                {
                    SnapOption.KeyWords = CommenClass.StringTools.SplitKeyWords(SnapOption.KeyWords);
                }
                char[] sep = { ' ' };
                string[] kws = SnapOption.KeyWords.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < kws.Length; i++)
                {
                    sql += string.Format("and ((XHMC like '%{0}%') OR (PPMC like '%{0}%'))", kws[i]);
                }
            }
            sql += ";select @@ROWCOUNT as RowNum;";
            return m_dbo.GetDataSet(sql, (PageModel.CurrentPage - 1) * PageModel.PageSize, PageModel.PageSize);
        }
        #endregion

        #region 品目对应
        /// <summary>
        /// 领先商品品目展示
        /// </summary>
        /// <returns></returns>
        public DataSet GoodsType()
        {
            string sql = string.Format(@"select 
(select top 1  TypeName from GoodsType where Code= SUBSTRING(gt.Code,1,2))as PPTypeName,
(select top 1 TypeName from GoodsType where Code=(SELECT top 1 SUBSTRING(gt.Code,1,4)))as PTypeName,
TypeName,ID,Code,PCode,Level from GoodsType gt where IsVisible=1");
            if (SnapOption.KeyWords != "" && SnapOption.KeyWords != null && SnapOption.KeyWords != "NULL")
            {
                if (SnapOption.KeyWords.Length > 2)
                {
                    SnapOption.KeyWords = CommenClass.StringTools.SplitKeyWords(SnapOption.KeyWords);
                }
                char[] sep = { ' ' };
                string[] kws = SnapOption.KeyWords.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < kws.Length; i++)
                {
                    sql += string.Format(" and ((TypeName like '%{0}%') or ( ID like '%{0}%'))", kws[i]);
                }
            }
            sql += " and Level=3 order by Code; select @@ROWCOUNT as RowNum;";
            return m_dbo.GetDataSet(sql, (PageModel.CurrentPage - 1) * PageModel.PageSize, PageModel.PageSize);
        }
        /// <summary>
        /// 显示第三方商品品目
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public DataSet Thirdparty_GoodsType(int ProjectId)
        {
            string sql = string.Format("select * from TPI_GoodsType where  ProjectId={0} and 1=1", ProjectId);
            if (SnapOption.KeyWords != "" && SnapOption.KeyWords != null && SnapOption.KeyWords != "NULL")
            {
                if (SnapOption.KeyWords.Length > 2)
                {
                    SnapOption.KeyWords = CommenClass.StringTools.SplitKeyWords(SnapOption.KeyWords);
                }
                char[] sep = { ' ' };
                string[] kws = SnapOption.KeyWords.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < kws.Length; i++)
                {
                    sql += string.Format(" and (Category like '%{0}%' or Id like '%{0}%')", kws[i]);
                }
            }
            sql += string.Format("  order by CategoryCode; select @@ROWCOUNT as RowNum;");
            return m_dbo.GetDataSet(sql, (PageModel.CurrentPage - 1) * PageModel.PageSize, PageModel.PageSize);
        }
        /// <summary>
        /// 查询对应成功的品目商品
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataSet Tpi_GoodsTypeMatch(int ProjectId, string code, string category)
        {
            string sql = string.Format(@"select * from TPI_Category tc join GoodsType gt on tc.TypeId=gt.ID where ProjectId={0} ", ProjectId);
            if (SnapOption.KeyWords != "" && SnapOption.KeyWords != null && SnapOption.KeyWords != "NULL")
            {
                if (SnapOption.KeyWords.Length > 2)
                {
                    SnapOption.KeyWords = CommenClass.StringTools.SplitKeyWords(SnapOption.KeyWords);
                }
                char[] sep = { ' ' };
                string[] kws = SnapOption.KeyWords.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < kws.Length; i++)
                {
                    sql += string.Format(" and ((Category like '%{0}%') or  (tc.TypeName like '%{0}%') or (CategoryId like '%{0}%') or (TypeId like '%{0}%')  or (CategoryCode like '%{0}%'))", kws[i]);
                }
            }
            if (code != "")
            {
                sql += string.Format(" and Code  like '{0}%'", code);
            }
            if (category != "")
            {
                sql += string.Format(" and Category1 ='{0}'", category);
            }
            sql += string.Format("  order by code desc; select @@ROWCOUNT as RowNum;");
            return m_dbo.GetDataSet(sql, (PageModel.CurrentPage - 1) * PageModel.PageSize, PageModel.PageSize);
        }
        /// <summary>
        /// 用事务导入品目对应信息
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="ProjectId"></param>
        /// <param name="UserId"></param>
        /// <param name="success"></param>
        /// <param name="failure"></param>
        /// <returns></returns>
        public int LeadExcel(string excel, int ProjectId, int UserId, out string result, int isUseTypeCompare)
        {
            List<string> listsql = new List<string>();
            List<string> listTypeId = new List<string>();
            DataTable dt_new = ImportExcel("select 客户一级品目,客户二级品目,客户三级品目,客户池子编号,领先类别名称,领先类别编码,投标承诺折扣 from [Sheet1$]", excel);
            result = "";
            int rows = dt_new.Rows.Count;
            if (rows > 0)
            {
                for (int i = 0; i < dt_new.Rows.Count; i++)
                {
                    string categoryCode = dt_new.Rows[i]["客户池子编号"].ToString();
                    string category1 = dt_new.Rows[i]["客户一级品目"].ToString();
                    string category2 = dt_new.Rows[i]["客户二级品目"].ToString();
                    string category = dt_new.Rows[i]["客户三级品目"].ToString();
                    string typeId = dt_new.Rows[i]["领先类别编码"].ToString();
                    string typeName = dt_new.Rows[i]["领先类别名称"].ToString();
                    string discount = dt_new.Rows[i]["投标承诺折扣"].ToString();
                    bool IsNullCheck = false;
                    if (isUseTypeCompare == 1 && category1 != "" && category != "" && categoryCode != "" && typeId != "" && typeName != "")
                    {
                        IsNullCheck = true;
                    }
                    else if (isUseTypeCompare == 2 && category1 != "" && category != "" && typeId != "" && typeName != "")
                    {
                        IsNullCheck = true;
                    }

                    if (IsNullCheck)
                    {
                        if (!listTypeId.Contains(typeId))
                        {
                            string sql = "insert into TPI_Category(ProjectId,Category1,Category2,Category,TypeName,TypeId,CategoryCode,UserId,Discount) values ('" + ProjectId + "','" + category1 + "','" + category2 + "','" + category + "','" + typeName + "','" + typeId + "','" + categoryCode + "','" + UserId + "','" + discount + "')";
                            listsql.Add(sql);
                            listTypeId.Add(typeId);
                        }
                        else
                        {
                            result += "TypeId为" + typeId + "的有重复值 /r/n";//typeid  duplicates.
                        }
                    }
                    else
                    {
                        result += "客户一级品目或客户三级品目或领先类别编码或领先类别名称有空值，请详细阅读操作手册！";  //category1 || category||  typeId || typeName has null
                    }
                }
            }

            if (result != "")
            {
                return -1;
            }
            else
            {
                return m_dbo.ExecNoQuerySqlTran(listsql);
            }
        }
        /// <summary>
        /// 用事务导入品目第三方品目信息
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="ProjectId"></param>
        /// <param name="UserId"></param>
        /// <param name="success"></param>
        /// <param name="failure"></param>
        /// <returns></returns>
        public int LeadExcel(string excel, int ProjectId, out string result, int isUseTypeCompare)
        {
            List<string> listsql = new List<string>();
            List<string> listTypeId = new List<string>();
            DataTable dt_new = ImportExcel("select 客户一级品目,客户二级品目,客户三级品目,客户一级品目编号,客户二级品目编号,客户三级品目编号,商品池编号 from [Sheet1$]", excel);
            result = "";
            int rows = dt_new.Rows.Count;
            if (rows > 0)
            {
                for (int i = 0; i < dt_new.Rows.Count; i++)
                {
                    string categoryCode = dt_new.Rows[i]["商品池编号"].ToString();
                    string category1 = dt_new.Rows[i]["客户一级品目"].ToString();
                    string category2 = dt_new.Rows[i]["客户二级品目"].ToString();
                    string category = dt_new.Rows[i]["客户三级品目"].ToString();
                    string CategorysortId1 = dt_new.Rows[i]["客户一级品目编号"].ToString();
                    string CategorysortId2 = dt_new.Rows[i]["客户二级品目编号"].ToString();
                    string CategorysortId3 = dt_new.Rows[i]["客户三级品目编号"].ToString();
                    bool IsNullCheck = false;
                    if (isUseTypeCompare == 1 && category1 != "" && category != "" && categoryCode != "")
                    {
                        IsNullCheck = true;
                    }
                    else if (isUseTypeCompare == 2 && category1 != "" && category != "")
                    {
                        IsNullCheck = true;
                    }

                    if (IsNullCheck)
                    {
                        string sql = "insert into TPI_GoodsType(ProjectId,Category1,Category2,Category,CategoryCode,CategorysortId1,CategorysortId2,CategorysortId3) values ('" + ProjectId + "','" + category1 + "','" + category2 + "','" + category + "','" + categoryCode + "','" + CategorysortId1 + "','" + CategorysortId2 + "','" + CategorysortId3 + "')";
                        listsql.Add(sql);
                    }
                    else
                    {
                        result += "客户一级品目或客户三级品目或商品池编号名称有空值，请详细阅读操作手册！";  //category1 || category||  typeId || typeName has null
                    }
                }
            }

            if (result != "")
            {
                return -1;
            }
            else
            {
                return m_dbo.ExecNoQuerySqlTran(listsql);
            }
        }
        /// <summary>
        /// 导入品目对应的excel数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="excel"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static DataTable ImportExcel(string sql, string excel)
        {
            //连接字符串   //03版 Microsoft.Jet.OLEDB.4.0  
            const string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';Extended Properties='{1};HDR=Yes;IMEX=1'";
            OleDbConnection conn = new OleDbConnection(string.Format(strConn, excel, "Excel 8.0"));
            try
            {
                conn.Open();
            }
            catch
            {
                conn = new OleDbConnection(string.Format(strConn, excel, "Excel 12.0"));
                try
                {
                    conn.Open();
                }
                catch (Exception)
                { }
            }
            DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            DataSet ds = new DataSet();
            foreach (DataRow dr in dt.Rows)
            {
                sql = "select * from [" + dr["TABLE_NAME"].ToString() + "]";
                OleDbDataAdapter aper = new OleDbDataAdapter(sql, conn);
                aper.Fill(ds, dr["TABLE_NAME"].ToString());
                if (ds.Tables[dr["TABLE_NAME"].ToString()].Columns.Count < 2)
                {
                    continue;
                }
            }
            conn.Close();
            conn.Dispose();
            DataSet ExcelData = ds;
            //MVC 没有回调 弹窗，暂时不做验证，假设模板没有被改动，已经填充了数据  add by hjy 2016-12-14
            //返回表
            return ExcelData.Tables[0];
        }
        #endregion

        #region 商品池中销售额统计
        /// <summary>
        /// 全部商城统计销售额
        /// </summary>
        /// <returns></returns>
        public DataSet SalesVolume()
        {
            string sql = @"with TOrder as (select tod.*,tp.Name from TPI_Order tod join TPI_Project tp on tod.ProjectId=tp.Id) select TOrder.Name as Mall,SUM( SumMoney) as Sale 
 from View_Order vg join TOrder on vg.TPI_OrderId=TOrder.Id  and IsDelete=0  where 1=1";
            if (SnapOption.StartTime > new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and  vg.OrderTime>='{0}'", SnapOption.StartTime);
                sql += string.Format(" and vg.OrderTime<='{0}'", SnapOption.EndTime);
            }
            sql += " group by TOrder.Name order by Sale desc";
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 当前商城统计销售额
        /// </summary>
        /// <returns></returns>
        public DataSet GetMallSale()
        {
            string sql = " select DATEPART( YEAR,tp.OrderTime) as Years,DATEPART( MONTH,tp.OrderTime) as Mouth, SUM( SumMoney) as Sale  from TPI_Order tp join View_Order vo on tp.Id=vo.TPI_OrderId and IsDelete=0 where 1=1";
            DateTime dStart = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime dEnd = new DateTime(DateTime.Now.Year, 12, 30);
            if (SnapOption.StartTime > new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and  tp.OrderTime>='{0}'", SnapOption.StartTime);
                sql += string.Format(" and tp.OrderTime<='{0}'", SnapOption.EndTime);
            }
            if (SnapOption.StartTime == new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and tp.OrderTime>='{0}'", dStart);
                sql += string.Format(" and tp.OrderTime<='{0}'", dEnd);
            }
            sql += string.Format(" and tp.ProjectId='{0}'", SnapOption.ProjectId);
            sql += " group by DATEPART( YEAR,tp.OrderTime),DATEPART( MONTH,tp.OrderTime) order by Years,Mouth";
            return m_dbo.GetDataSet(sql);
        }

        /// <summary>
        /// 按商品类别统计销售额
        /// </summary>
        /// <returns></returns>
        public DataSet GetGoodsType()
        {
            string sql = string.Format(@"select (select TypeName from GoodsType where Code=SUBSTRING(vod.Code,1,2)) as Category ,SUM(Amount)as Sale
from View_OrderDetail vod where OrderId in(select Id from [Order] where TPI_OrderId in(select Id from TPI_Order where ProjectId='{0}') and IsDelete=0 and RawOrderId=0)", SnapOption.ProjectId);
            if (SnapOption.StartTime > new DateTime(1900, 1, 1))
            {
                sql += string.Format(" and vod.OrderTime>='{0}'", SnapOption.StartTime);
                sql += string.Format(" and vod.OrderTime<='{0}'", SnapOption.EndTime);
            }
            sql += " group by SUBSTRING(Code,1,2) order by Sale desc";
            return m_dbo.GetDataSet(sql);
        }
        #endregion

        /// <summary>
        /// 查询某个商城下面的所有订单
        /// </summary>
        /// <returns></returns>
        public DataSet GetOrderByProjectId()
        {
            string sql = string.Format(@"select vor.OrderId,vor.SumMoney,vor.Company,vor.PayStatus,vor.OrderTime,vor.StoreFinishTime from View_Order vor join TPI_Order 
tor on vor.TPI_OrderId=tor.Id where tor.ProjectId={0}
and IsDelete=0 
and RawOrderId=0 
order by vor.OrderTime desc ", SnapOption.ProjectId);
            sql += " ;select @@rowcount as RowNum  ";
            return m_dbo.GetDataSet(sql, (PageModel.CurrentPage - 1) * PageModel.PageSize, PageModel.PageSize);
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public DataSet GetOrderDetailByOrderId(int OrderId)
        {
            string sql = string.Format(@"select * from View_OrderDetail where OrderId like '%{0}%' and IsDelete=0 and RawOrderId=0", OrderId);
            return m_dbo.GetDataSet(sql);
        }

        #region 获取品目
        /// <summary>
        /// 根据品目获取领先方商品编号（使用接口类型：IsUseTypeCompare=0）
        /// TPI_Goods,Goods,GoodsType关联
        /// 全部都是内联一一对应
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public DataTable ReadType(int projectId)
        {
            string sql = string.Format(@"select goods.TypeId,GoodsType.TypeName from TPI_Goods inner join   goods  on TPI_Goods.GoodsId= Goods.ID inner join GoodsType on Goods.TypeId =GoodsType.ID where  TPI_Goods.ProjectId={0} group by  goods.TypeId,GoodsType.TypeName  ", projectId);
            return m_dbo.GetDataSet(sql).Tables[0];
        }

        /// <summary>
        /// 根据品目对应获取客户方品目信息（使用接口类型：IsUseTypeCompare=1）
        /// TPI_Category,Goods,TPI_Goods关联 
        /// 全部都是内联一一对应
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public DataTable ReadCategoreyCode(int ProjectId)
        {
            string sql = string.Format(@"select Category,CategoryCode from TPI_Category tc join  Goods g on tc.TypeId=g.TypeId  join TPI_Goods tg on  tg.GoodsId=g.ID 
where tg.ProjectId={0} and tc.ProjectId={0} group by Category,CategoryCode", ProjectId);
            return m_dbo.GetDataSet(sql).Tables[0];
        }
        /// <summary>
        /// 根据品目对应获取客户方品目信息（使用接口类型：IsUserTypeConpare=2）
        /// TPI_Category,Goods,TPI_Goods关联 
        /// 全部都是内联一一对应
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public DataTable ReadTypeId(int ProjectId)
        {
            string sql = string.Format(@"select Category,CategoryCode,tc.TypeId,TypeName from TPI_Category tc join  Goods g on tc.TypeId=g.TypeId  join TPI_Goods tg on  tg.GoodsId=g.ID 
where tg.ProjectId={0} and tc.ProjectId={0} group by Category,CategoryCode,tc.TypeId,TypeName", ProjectId);
            return m_dbo.GetDataSet(sql).Tables[0];
        }

        /// <summary>
        /// 根据商品编号获取池子编号（第三方提供）  （使用项目：北陆）
        /// 此方法关联表包含：TPI_Goods、Goods、TPI_Category 
        /// 关联方式都是内联
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="goodsIds"></param>
        /// <returns></returns>
        public DataTable ReadCategroyByGoodsId(int ProjectId, int[] goodsIds)
        {
            string sql = "select Category,CategoryCode,GoodsId from TPI_Goods tg join  Goods g on tg.GoodsId=g.ID join TPI_Category tc on g.TypeId=tc.TypeId where 1=1";
            if (ProjectId > 0)
            {
                sql += string.Format(" and tg.ProjectId={0} and tc.ProjectId={0} ", ProjectId);
            }
            if (goodsIds != null)
            {
                sql += " and GoodsId in ( ";
                for (int i = 0; i < goodsIds.Length; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", goodsIds[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", goodsIds[i]);
                }
                sql += " ) ";
            }
            sql += " order by  CategoryCode";
            return m_dbo.GetDataSet(sql).Tables[0];
        }

        #endregion

        #region 获取品目 one methed
        /// <summary>
        /// 获取商品目录
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="IsUseTypeCompare">获取品目类型，0：全领先；1：全客户；2：id领先，name客户</param>
        /// <returns></returns>
        public DataTable Read_Cate(int projectId, int IsUseTypeCompare)
        {
            string sqlstr = "";
            switch (IsUseTypeCompare)
            {
                case 0: sqlstr = string.Format(@"select GoodsType.TypeName as Cate_Name,goods.TypeId as Cate_Id from TPI_Goods inner join   goods  on TPI_Goods.GoodsId= Goods.ID 
inner join GoodsType on Goods.TypeId =GoodsType.ID where  TPI_Goods.ProjectId={0} group by  goods.TypeId,GoodsType.TypeName  ", projectId);
                    break;
                case 1: sqlstr = string.Format(@"select Category as Cate_Name,CategoryCode as Cate_Id from TPI_Category tc join  Goods g on tc.TypeId=g.TypeId  join TPI_Goods tg on  tg.GoodsId=g.ID 
where tg.ProjectId={0} and tc.ProjectId={0} group by Category,CategoryCode", projectId);
                    break;
                case 2: sqlstr = string.Format(@"select Category as Cate_Name,tc.TypeId as Cate_Id from TPI_Category tc join  Goods g on tc.TypeId=g.TypeId  join TPI_Goods tg on  tg.GoodsId=g.ID 
where tg.ProjectId={0} and tc.ProjectId={0} group by Category,CategoryCode,tc.TypeId,TypeName", projectId);
                    break;
            }
            return m_dbo.GetDataSet(sqlstr).Tables[0];
        }
    
        #endregion

        #region 根据品目获取商品编号

        /// <summary>
        /// 根据品目获取领先方商品编号（使用接口类型：IsUseTypeCompare=0）
        /// TPI_Goods,Goods,GoodsType关联
        /// 全部都是内联一一对应
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public DataTable ReadGoodsIdType(int projectId, int typeId)
        {
            string sql = string.Format(@"select GoodsId,goods.TypeId,Sku from TPI_Goods inner join   goods  on TPI_Goods.GoodsId= Goods.ID inner join GoodsType on Goods.TypeId =GoodsType.ID where  TPI_Goods.ProjectId={0}  and Goods.TypeId={1}", projectId, typeId);
            return m_dbo.GetDataSet(sql).Tables[0];
        }

        /// <summary>
        /// 根据品目获取领先方商品编号（使用接口类型：IsUseTypeCompare=1）
        /// TPI_Category,Goods,TPI_Goods关联 
        /// 全部都是内联一一对应
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public DataTable ReadGoodsIdByCategoreyCode(int ProjectId, string CategoryCode)
        {
            string sql = string.Format(@"select GoodsId,CategoryCode from TPI_Category tc join  Goods g on tc.TypeId=g.TypeId  join TPI_Goods tg on  tg.GoodsId=g.ID 
where CategoryCode='{0}' and tg.ProjectId={1} and tc.ProjectId={1} and State=1  group by GoodsId,CategoryCode", CategoryCode, ProjectId);
            return m_dbo.GetDataSet(sql).Tables[0];
        }

        /// <summary>
        /// 根据品目获取领先方商品编号（使用接口类型：IsUseTypeCompare=2）
        /// TPI_Category,Goods,TPI_Goods关联 
        /// 全部都是内联一一对应
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public DataTable ReadGoodsIdByTypeId(int ProjectId, int TypeId)
        {
            string sql = string.Format(@"select GoodsId,tc.TypeId from TPI_Category tc join  Goods g on tc.TypeId=g.TypeId  join TPI_Goods tg on  tg.GoodsId=g.ID 
where tc.TypeId={0} and tg.ProjectId={1} and tc.ProjectId={1} and State=1 group by GoodsId,tc.TypeId", TypeId, ProjectId);
            return m_dbo.GetDataSet(sql).Tables[0];
        }

        #endregion

        #region 获取商品详情
        /// <summary>
        /// 商品详情        （有品目对应）
        /// TPI_Goods，Goods，TPI_Category，Brand，GoodsBranch,TPI_Project关联
        /// 全部都是内联 但是有重复数据（类别对应有一对多）  
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public DataSet ReadGoodsByDetail(int projectId, List<int> goodsId)
        {
            string sql = string.Format(@"select g.SN,g.DisplayName,g.Unit,g.Feature,g.PackingList,g.ID as GoodsId,tc.CategoryId ,g.HomeImage,g.Origin,g.BarCodeNum, tg.State, tc.Category,tc.CategoryCode,b.Name as BrandName,tc.TypeName,tc.TypeId,g.GoodsName,g.Description,ParentId, g.PackingList,tg.AgreementPrice,tg.Stock, ISNULL (gb.Price,g.Price) as SysPrice ,g.Origin
    from TPI_Goods tg  
join TPI_Project tp on (tg.ProjectId =tp.Id) 
left join goodsbranch gb on(tp.BranchId=gb.BranchId and tg.GoodsId=gb.GoodsId) 
join Goods g on (tg.GoodsId=g.ID) 
join TPI_Category tc on(g.TypeId=tc.TypeId and tc.ProjectId=tg.ProjectId)
join Brand b on g.BrandId = b.Id where 1=1 ");
            if (goodsId != null)
            {
                sql += "and tg.GoodsId in (";
                for (int i = 0; i < goodsId.Count; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", goodsId[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", goodsId[i]);
                }
                sql += " ) ";
                sql += string.Format("and  tc.ProjectId={0}  and tg.ProjectId={0} ", projectId);
            }
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 商品详情  （没有品目对应）
        /// TPI_Goods，Goods，Brand ,GoodsBranch,TPI_Project关联
        /// 全部都是内联  
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public DataSet ReadGoodsByDetailTypeId(int projectId, List<int> goodsId)
        {
            string sql = string.Format(@"select g.SN,g.DisplayName,g.Unit,g.Feature,g.Price,g.TypeId ,g.PackingList,g.ID as GoodsId,g.HomeImage,g.HomeImage,g.Origin,g.BarCodeNum, tg.State,b.Name as BrandName,g.GoodsName,g.Description,ParentId ,TypeName,g.Model
 from TPI_Goods tg join Goods g on tg.GoodsId =g.ID join GoodsType on g.TypeId=GoodsType.ID 
  join Brand b on g.BrandId = b.Id where 1=1 ");
            if (goodsId != null)
            {
                sql += "and tg.GoodsId in (";
                for (int i = 0; i < goodsId.Count; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", goodsId[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", goodsId[i]);
                }
                sql += " ) ";
                sql += string.Format("and tg.ProjectId={0} ", projectId);
            }
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 商品详情   （有品目对应，品牌对应）
        /// TPI_Goods，Goods，Brand,TPI_Category,TPI_DSFBrand,GoodsBranch,TPI_Project关联
        /// 全部都是内联 但是有重复数据（类别对应有一对多）
        /// 包含第三方品牌
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public DataSet ReadGoodsByDetailHaveBrand(List<int> goodsId, int projectId)
        {
            string sql = string.Format(@"select g.SN,g.DisplayName,g.Unit,g.Feature,g.PackingList,g.ID as GoodsId,g.HomeImage,tc.CategoryId,g.HomeImage,g.Origin,g.BarCodeNum, tg.State, tc.Category,tc.CategoryCode,b.Name as BrandName,tc.TypeName,tc.TypeId,g.GoodsName,g.Description,ParentId ,td.DSFBrandId,g.PackingList,tg.AgreementPrice,tg.Stock, ISNULL (gb.Price,g.Price) as SysPrice
    from TPI_Goods tg  
join TPI_Project tp on (tg.ProjectId =tp.Id) 
left join goodsbranch gb on(tp.BranchId=gb.BranchId and tg.GoodsId=gb.GoodsId) 
join Goods g on (tg.GoodsId=g.ID) 
join TPI_Category tc on(g.TypeId=tc.TypeId and tc.ProjectId=tg.ProjectId)
join Brand b on g.BrandId = b.Id 
inner  join TPI_DSFBrand td on b.Id= td .BrandId  where 1=1 ");
            if (goodsId != null)
            {
                sql += "and tg.GoodsId in (";
                for (int i = 0; i < goodsId.Count; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", goodsId[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", goodsId[i]);
                }
                sql += " ) ";
                sql += string.Format("and tg.ProjectId={0} and td.ProjectId={0} ", projectId);
            }
            return m_dbo.GetDataSet(sql);
        }
        #endregion

        #region 读取上下架/库存数
        /// <summary>
        /// 根据goodId读取商品上下架状态
        /// TPI_Goods
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public DataTable ReadTPI_GoodsInfo(List<int> goodsId, int projectId)
        {
            string sql = string.Format(@"select * from TPI_Goods where 1=1 ");
            if (goodsId != null)
            {
                sql += " and GoodsId in ( ";
                for (int i = 0; i < goodsId.Count; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", goodsId[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", goodsId[i]);
                }
                sql += " ) ";
            }
            sql += string.Format(" and ProjectId={0} ", projectId);
            return m_dbo.GetDataSet(sql).Tables[0];
        }
        #endregion

        #region 读取商品价格（系统价格、协议价格）
        /// <summary>
        /// 根据商品编号查看商品价格  
        /// TPI_Goods，TPI_Project，goodsbranch,Goods左关联
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public DataSet ReadGoodsPrice(List<int> goodsId, int projectId)
        {
            string sql = string.Format(@" select tg.GoodsId,ISNULL(gb.Price,g.Price) as price,tg.AgreementPrice,tg.State from TPI_Goods tg 
left join TPI_Project tp on (tg.ProjectId =tp.Id)
left join goodsbranch gb on(tp.BranchId=gb.BranchId and tg.GoodsId=gb.GoodsId)
left join Goods g on (tg.GoodsId=g.ID)
where 1=1 ");
            if (goodsId != null)
            {
                sql += "and tg.GoodsId in (";
                for (int i = 0; i < goodsId.Count; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", goodsId[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", goodsId[i]);
                }
                sql += " ) ";
                sql += string.Format("and tg.ProjectId={0} ", projectId);
            }
            return m_dbo.GetDataSet(sql);
        }
        /// <summary>
        /// 读取商城售价，系统价格，折扣率
        /// TPI_Goods，TPI_Project，goodsbranch,Goods，TPI_Category左关联
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public DataSet ReadGoodsPriceDiscount(List<int> goodsId, int projectId)
        {
            string sql = string.Format(@" select tg.GoodsId,ISNULL(gb.Price,g.Price) as price,tg.AgreementPrice,tg.State,tc.Discount from TPI_Goods tg 
left join TPI_Project tp on (tg.ProjectId =tp.Id)
left join goodsbranch gb on(tp.BranchId=gb.BranchId and tg.GoodsId=gb.GoodsId)
left join Goods g on (tg.GoodsId=g.ID)   
left join TPI_Category tc on g.TypeId=tc.TypeId
where 1=1 ");
            if (goodsId != null)
            {
                sql += "and tg.GoodsId in (";
                for (int i = 0; i < goodsId.Count; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", goodsId[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", goodsId[i]);
                }
                sql += " ) ";
                sql += string.Format("and tg.ProjectId={0} and tc.ProjectId={0} ", projectId);
            }
            return m_dbo.GetDataSet(sql);
        }
        #endregion

        #region 获取图片

        /// <summary>
        /// 读取商品图片 
        /// (Path+'/'+gp.PhotoName) as imagePath
        /// GoodsPhoto,GoodsPhoto内关联
        /// </summary>
        /// <returns></returns>
        public DataSet ReadGoodsPhotoBySku(int[] GoodsId, int isHaveDetailPhoto)
        {
            string sql = string.Format(@"select g.ID as GoodsId ,(Path+'/'+gp.PhotoName) as imagePath,gp.IsHomeImage,gp.IsDetailPhoto
 from GoodsPhoto gp ,Goods  g where (gp.GoodsId =g.ID  or gp.GoodsId=g.ParentId )and gp.GoodsId !=0");
            if (GoodsId != null)
            {
                sql += " and g.ID in ( ";
                for (int i = 0; i < GoodsId.Length; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}' ", GoodsId[0]);
                    }
                    else sql += string.Format(" ,'{0}' ", GoodsId[i]);
                }
                sql += " ) ";
            }
            if (isHaveDetailPhoto == 0)
            {
                sql += " and gp.IsDetailPhoto<>1";
            }
            return m_dbo.GetDataSet(sql);
        }
        #endregion

        #region 获取订单
        /// <summary>
        /// 根据项目编号读取对应的订单数量
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public DataSet ReadOrders(string KeyWords,int projectId)
        {
            string sql = string.Format(@"select o.Id,o.ComId,o.RealName,o.SumMoney,o.Mobile,o.Address,o.SumMoney,o.OrderTime,o.Memo,TPI_Order.ThirdPartyOrderId,c.Name 
from TPI_Order inner join [Order] o  on TPI_Order.Id = o.TPI_OrderId join Company c on c.Id=o.ComId where   ProjectId ={0} ", projectId);
            if (SnapOption.KeyWords != "" && SnapOption.KeyWords != null && SnapOption.KeyWords != "NULL")
            {
                sql += string.Format(" and ( o.Id like  '%{0}%' or ThirdPartyOrderId like  '%{0}%')", KeyWords);
            }
            sql += " order by o.OrderTime desc  ;select @@ROWCOUNT as RowNum;";

            return m_dbo.GetDataSet(sql, (PageModel.CurrentPage - 1) * PageModel.PageSize, PageModel.PageSize);
        }
        #endregion
    }
}
