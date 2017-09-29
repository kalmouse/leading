using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeadingClass
{
    /// <summary>
    /// 接口所有用到的枚举类型
    /// </summary>
    public class TPI_Enum
    {
        /// <summary>
        /// 这是北京市采订单接口用到的枚举和Dictionary类型
        /// 备注：Dictionary类型如果没有对应的键值,会返回null值
        /// </summary>
        public class TPI_ZD
        {
            //是否开发票 
            public enum InvoiceState { 开 = 1, 不开 = 0 }
            //发票类型
            public enum InvoiceType { 不开发票 = 0, 增值发票 = 1, 普通发票 = 2 }
            //发票内容
            public enum InvoiceContent { 不开发票 = 0, 明细 = 1, 电脑配件 = 1, 耗材 = 19, 办公用品 = 22 }
            //是否审核 
            public enum ReviewId { 审核 = 1, 不审核 = 0 }
            //订单行状态码,其实就是商品的状态   
            public enum statusNum { 审核中 = 1, 待发货 = 2, 已发货 = 3, 部分已发货 = 4, 已完成 = 5, 已取消 = 6, 已退货 = 7, 订单异常 = 8, 审核不通过订单已取消 = 9 }
            //付款方式：Dictionary类型                
            public readonly static Dictionary<string, string> payment = new Dictionary<string, string> { { "04", "货到付款" } };
            //商品上下架状态
            public enum listState { 在售 = 1, 下架 = 0 }

            //订单状态
            public readonly static Dictionary<string, int> orderStatus = new Dictionary<string, int>
            {
                {"审核中",1}, {"待发货",2}, {"已发货",3}, {"部分已发货",4}, {"已完成",5}, {"已取消",6}, {"已退货",7}, {"订单异常",8}, {"审核不通过订单已取消",9}
            };
            //（市采）-（领先未来）订单状态对应：Dictionary类型
            public readonly static Dictionary<string, string> orderStatusKey = new Dictionary<string, string> { 
            { "未处理", orderStatus.FirstOrDefault(d => d.Key=="审核中").Value.ToString()},
            { "已接受", orderStatus.FirstOrDefault(d => d.Key=="待发货").Value.ToString()},
            { "配送中", orderStatus.FirstOrDefault(d => d.Key=="已发货").Value.ToString()},
            { "已完成", orderStatus.FirstOrDefault(d => d.Key=="已完成").Value.ToString()}
            };
        }
        public class TPI_YGGC
        {
            public enum payment { 货到付款 = 1, 转帐 = 2, 在线支付 = 3, 支票 = 4, 账期 = 5 }
        }
        /// <summary>
        /// 友云采订单支付方式
        /// </summary>
        public class YYCPayment
        {
            public enum payment { 在线支付 = 0, 货到付款 = 1, 赊账账期 = 2, 供应商代收 = 3 }
        }
        /// <summary>
        /// 政采云订单状态
        /// </summary>
        public class ZCYorderPayment
        {
            public enum orderPayment { 等待处理 = 0, 已接收待发货 = 1, 部分发货 = 2, 全部发货 = 3, 确认收货 = 4, 待安装 = 5, 已安装待验收 = 6, 交易完成 = 7 }
        }
        /// <summary>
        /// 国网订单状态
        /// </summary>
        public class GWoederPayment
        {
            public enum orderPayment { 待付款 = 01, 取消 = 02, 已付款 = 03, 交易完成 = 04, 关闭 = 05, 删除 = 06, 禁止付款 = 99, 部分付款 = 07, 部分退款 = 08, 部分确认付款 = 09 }
        }

    }
}
