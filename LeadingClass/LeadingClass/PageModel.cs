using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LeadingClass
{
    public enum PageType { 首页 = 1, 上一页 = 2, 下一页 = 3, 尾页 = 4,默认=5}
    public  class PageModel
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalRows { get; set; }
        public int TotalPage { get; set; }
        public DataSet Dataset { get; set; }
        public string DatasetJson { get; set; }

        public PageModel() 
        {
            CurrentPage = 0;
            PageSize = 0;
            TotalRows = 0;
            TotalPage = 0;
            Dataset = new DataSet();
            DatasetJson = "";
        }

        public string ToUrl()
        {
            return "";
        }

        /// <summary>
        /// 新闻的url地址
        /// </summary>
        /// <returns></returns>
        public string ToUrlForNewsList()
        {
            return "";
        }

        public string ToUrlForPriceOff(int typeId,PageType type)
        {
            switch (type)
            {
                case PageType.首页:
                    return string.Format("priceoff-{0}-{1}", typeId, 1);
                case PageType.上一页:
                    if (this.CurrentPage > 1)
                    {
                        return string.Format("priceoff-{0}-{1}", typeId, CurrentPage - 1);
                    }
                    else
                    {
                        return string.Format("priceoff-{0}-{1}", typeId, 1);
                    }
                case PageType.下一页:
                    if (this.CurrentPage < TotalPage)
                    {
                        return string.Format("priceoff-{0}-{1}", typeId, CurrentPage + 1);
                    }
                    else
                    {
                        return string.Format("priceoff-{0}-{1}", typeId, TotalPage);
                    }
                case PageType.尾页:
                    return string.Format("priceoff-{0}-{1}", typeId, TotalPage);
                default :
                    return string.Format("priceoff-{0}-{1}", typeId, CurrentPage);
            }

        }

        public string ToUrlForHot(int typeId, PageType type)
        {
            switch (type)
            {
                case PageType.首页:
                    return string.Format("hot-{0}-{1}", typeId, 1);
                case PageType.上一页:
                    if (this.CurrentPage > 1)
                    {
                        return string.Format("hot-{0}-{1}", typeId, CurrentPage - 1);
                    }
                    else
                    {
                        return string.Format("hot-{0}-{1}", typeId, 1);
                    }
                case PageType.下一页:
                    if (this.CurrentPage < TotalPage)
                    {
                        return string.Format("hot-{0}-{1}", typeId, CurrentPage + 1);
                    }
                    else
                    {
                        return string.Format("hot-{0}-{1}", typeId, TotalPage);
                    }
                case PageType.尾页:
                    return string.Format("hot-{0}-{1}", typeId, TotalPage);
                default:
                    return string.Format("hot-{0}-{1}", typeId, CurrentPage);
            }
        }

        /// <summary>
        /// 专柜商品列表的url
        /// </summary>
        /// <param name="code"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string ToUrlForCounterGoods(string code, PageType type)
        {
            string url = "VipManage/CounterGoodsList?code={0}&currentpage={1}";
            switch (type)
            {
                case PageType.首页:
                    return string.Format(url, code, 1);
                case PageType.上一页:
                    if (this.CurrentPage > 1)
                    {
                        return string.Format(url, code, CurrentPage - 1);
                    }
                    else
                    {
                        return string.Format(url, code, 1);
                    }
                case PageType.下一页:
                    if (this.CurrentPage < TotalPage)
                    {
                        return string.Format(url, code, CurrentPage + 1);
                    }
                    else
                    {
                        return string.Format(url, code, TotalPage);
                    }
                case PageType.尾页:
                    return string.Format(url, code, TotalPage);
                default:
                    return string.Format(url, code, CurrentPage);
            }
        }

        /// <summary>
        /// 品牌专区的url,ppcode,  pcode,  code的初始值都为"0",不然重写url是出现brand-7----1,然后无法匹配action的多个参数
        /// </summary>
        /// <param name="brandid"></param>
        /// <param name="ppcode"></param>
        /// <param name="pcode"></param>
        /// <param name="code"></param>
        /// <param name="currentpage"></param>
        /// <returns></returns>
        public string ToUrlBrand(int brandid, string ppcode, string pcode, string code, int currentpage)
        {
            return string.Format("brand-{0}-{1}-{2}-{3}-{4}", brandid, ppcode, pcode, code, currentpage);
        }
    }
}
