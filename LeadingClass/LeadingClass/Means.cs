using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
namespace LeadingClass
{
    public static class Means
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="sEncoding"></param>
        /// <returns></returns>
        public static string GetHttpPost(string url, string sEncoding)
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
        /// 短信发送
        /// </summary>
        /// <param name="Content">短信内容</param>
        /// <param name="mobile">手机号</param>
        /// <returns></returns>
        public static int SMSsend(string Content, string mobile)
        {
            string url = "http://sdk.entinfo.cn:8061/";
            string sn = "SDK-BBX-010-22290";
            string pwd = "F1FB49873F114F8A920344623734461F";
            string Rrid = "9";
            if (Content != "")
            {
                int message = Convert.ToInt32(GetHttpPost(url + "mdgxsend.ashx?sn=" + sn + "&pwd=" + pwd + "&mobile=" + mobile + "&content=" + Content + "&ext=&stime=&rrid=" + Rrid + "&msgfmt=", "UTF-8"));
                Sms sms = new Sms();
                sms.Mobile = "";
                sms.SMSContent = "";
                sms.Status = message;
                sms.Addtime = DateTime.Now;
                return message;
            }
            else return 0;
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>-1发送错误，0发送限制，1发送成功</returns>
        public static int Vcode(string mobile)
        {
            int i = 0;
            string code = "";
            string content = "您的验证码：" + code + "【领先办公】";
            string dtime = DateTime.Today.ToString();
            string time = "";
            int number = 1;         
            int Id = 0;
            Smsstore smsstore = new Smsstore();
            DataSet ds = smsstore.Model(mobile);
            if (ds != null)
            {
                Id = Convert.ToInt32(ds.Tables[0].Rows[0]["Id"].ToString());
                time = ds.Tables[0].Rows[0]["SendTime"].ToString();
                number = Convert.ToInt32(ds.Tables[0].Rows[0]["SMSNumber"].ToString());
                code = ds.Tables[0].Rows[0]["SMSCode"].ToString();
                if (dtime == time)
                {
                    if (number < 3)
                    {
                        number = number + 1;
                        i = 1;
                    }
                }
                else
                {
                    Random rd = new Random();
                    code = rd.Next(100000, 999999).ToString();
                    i = 1;
                }
            }
            else i = 0;
            if (i == 1)
            {
                int message = SMSsend(content, mobile);
                if (message == 9)
                {
                    smsstore.Id = Id;
                    smsstore.SendTime = Convert.ToDateTime(dtime);
                    smsstore.SMSNumber = number;
                    smsstore.SMSCode = code;
                    smsstore.Save();
                    return 1;
                }
                else return -1;
            }
            else return 0;
        }
    }
}
