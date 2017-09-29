using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Xml;
using System.Web;

namespace LeadingClass
{
    public class IpAddress
    {
        /// <summary>
        /// 获取http请求的结果
        /// </summary>
        /// <param name="url"></param>
        /// <param name="sEncoding"></param>
        /// <returns></returns>
        private string GetHttpPostResult(string url, string sEncoding)
        {
            try
            {
                WebClient WC = new WebClient();
                WC.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                int p = url.IndexOf("?");
                string sData = url.Substring(p + 1);
                url = url.Substring(0, p);
                byte[] postData = Encoding.GetEncoding(sEncoding).GetBytes(sData);
                byte[] responseData = WC.UploadData(url, "POST", postData);
                string ct = Encoding.GetEncoding(sEncoding).GetString(responseData);
                return ct;
            }
            catch (Exception Ex)
            {
                return Ex.Message;
            }
        }

        /// <summary>
        /// ip地址
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public Ipinfo Address(string ip)
        {
            Ipinfo ipinfo = new Ipinfo();
            try
            {
                DateTime start = DateTime.Now;
                string url = "http://ip.taobao.com/service/getIpInfo.php";
                string dz = GetHttpPostResult(url + "?ip=" + ip, "UTF-8");
                DateTime end = DateTime.Now;          
                Ipvalid ipvalid = new Ipvalid();
                ipvalid = JsonHelper.JsonDeserialize<Ipvalid>(dz);
                if (ipvalid.code == 0)
                {
                    ipvalid.data.city = ipvalid.data.city.Replace("市","");
                    return ipvalid.data;
                }
                else
                {
                    return ipinfo;
                    
                }
            }
            catch 
            {
                return ipinfo;
            }
        }

        /// <summary>
        /// 通过ip地址获取，该城市对应的供应商信息 add by quxiaoshan 2015-4-17
        /// </summary>
        /// <returns></returns>
        public Sys_Branch GetBranchCityByIp(string ip) 
        {   
            Ipinfo ipinfo= Address(ip);
            Sys_Branch branch = new Sys_Branch();
            if (!string.IsNullOrEmpty(ipinfo.city))
            {
                branch.LoadByCity(ipinfo.city);
            }
            else
            {
                branch.LoadByCity("北京");
            }
            return branch;
        }

        /// <summary>
        /// 通过City获取其对应的Branch对象 add by quxiaoshan 2015-5-4
        /// </summary>
        /// <param name="city"></param>
        public Sys_Branch SetBranchSessionByCity(string city)
        {
            Sys_Branch branch = new Sys_Branch();
            if (branch.LoadByCity(city))
            {
                return branch;
            }
            return null;
        }
    }
    public class Ipinfo
    {
        public string country { get; set; }
        public string area { get; set; }
        public string region { get; set; }
        public string city { get; set; }
        public string county { get; set; }
        public string isp { get; set; }
        public string isp_id { get; set; }
    }
    public class Ipvalid
    {
        public int code { get; set; }
        public Ipinfo data { get; set; }
    }
}
