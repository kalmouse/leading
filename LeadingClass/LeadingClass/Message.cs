using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Collections;
namespace LeadingClass
{
    public class MobileCode
    {
        private string mobile;

        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }
        private int message;

        public int Message
        {
            get { return message; }
            set { message = value; }
        }
    }
    public class Sms
    {
        public int Id { get; set; }
        public string Mobile { get; set; }
        public string SMSContent { get; set; }
        public int Status { get; set; }
        public DateTime Addtime { get; set; }
        private DBOperate m_dbo;

        public Sms()
        {
            Id = 0;
            Mobile = "";
            SMSContent = "";
            Status = 0;
            Addtime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@Mobile", Mobile));
            arrayList.Add(new SqlParameter("@SMSContent", SMSContent));
            arrayList.Add(new SqlParameter("@Status", Status));
            arrayList.Add(new SqlParameter("@Addtime", Addtime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Sms", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Sms", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Sms where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                Mobile = DBTool.GetStringFromRow(row, "Mobile", "");
                SMSContent = DBTool.GetStringFromRow(row, "SMSContent", "");
                Status = DBTool.GetIntFromRow(row, "Status", 0);
                Addtime = DBTool.GetDateTimeFromRow(row, "Addtime");
                return true;
            }
            return false;
        }

        public bool Delete()
        {
            string sql = string.Format("Delete from Sms where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }
    public class Smsstore
    {
        public int Id { get; set; }
        public string Mobile { get; set; }
        public string SMSCode { get; set; }
        public int SMSNumber { get; set; }
        public DateTime SendTime { get; set; }
        private DBOperate m_dbo;

        public Smsstore()
        {
            Id = 0;
            Mobile = "";
            SMSCode = "";
            SMSNumber = 0;
            SendTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@Mobile", Mobile));
            arrayList.Add(new SqlParameter("@SMSCode", SMSCode));
            arrayList.Add(new SqlParameter("@SMSNumber", SMSNumber));
            arrayList.Add(new SqlParameter("@SendTime", SendTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("SmsStore", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("SmsStore", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from SmsStore where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                Mobile = DBTool.GetStringFromRow(row, "Mobile", "");
                SMSCode = DBTool.GetStringFromRow(row, "SMSCode", "");
                SMSNumber = DBTool.GetIntFromRow(row, "SMSNumber", 0);
                SendTime = DBTool.GetDateTimeFromRow(row, "SendTime");
                return true;
            }
            return false;
        }

        public DataSet Model(string mobile)
        {
            string sql = string.Format("select * from SmsStore where Mobile='{0}'", mobile);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds;
            }
            else return null;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from SmsStore where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }

        public int GetMobile(string mobile, string code)
        {
            string sql = string.Format("select * from SmsStore where Mobile='{0}' and SMSCode='{1}'", mobile, code);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return 1;
            }
            else return 0;
        }
    }
    public static class Message
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
        ///// <summary>
        ///// 短信发送
        ///// </summary>
        ///// <param name="Content">短信内容</param>
        ///// <param name="mobile">手机号</param>
        ///// <returns></returns>
        //public static int SMSsend(string Content, string mobile)
        //{
        //    string url = "http://sdk.entinfo.cn:8061/";
        //    string sn = "SDK-BBX-010-22290";
        //    string pwd = "F1FB49873F114F8A920344623734461F";
        //    string Rrid = "9";
        //    if (Content != "")
        //    {
        //        int message = Convert.ToInt32(GetHttpPost(url + "mdgxsend.ashx?sn=" + sn + "&pwd=" + pwd + "&mobile=" + mobile + "&content=" + Content + "&ext=&stime=&rrid=" + Rrid + "&msgfmt=", "UTF-8"));
        //        Sms sms = new Sms();
        //        sms.Mobile = mobile;
        //        sms.SMSContent = Content;
        //        sms.Status = message;
        //        sms.Addtime = DateTime.Now;
        //        sms.Save();
        //        return message;
        //    }
        //    else return 0;
        //}

        ///// <summary>
        ///// 短信发送
        ///// </summary>
        ///// <param name="Content">短信内容</param>
        ///// <param name="mobile">手机号</param>
        ///// <returns></returns>
        //public static int SMSsend(string Content, string mobile)
        //{
        //    string url = "http://101.227.68.49:7891/";
        //    string un = "10690254";
        //    string pw = "Lxwl-08";
        //    string dc = "15";//15表示中文 0英文 8UCS2
        //    if (Content != "")
        //    {
        //        int message = Convert.ToInt32(GetHttpPost(url + "mt?un=" + un + "&pw=" + pw + "&da=" + mobile + "&sm=" + Content + "&dc=" + dc + "&rd=1", "UTF-8"));
        //        Sms sms = new Sms();
        //        sms.Mobile = mobile;
        //        sms.SMSContent = Content;
        //        sms.Status = message;
        //        sms.Addtime = DateTime.Now;
        //        sms.Save();
        //        return message;
        //    }
        //    else return 0;
        //}

        public static int SMSsend(string Content, string mobile)
        {
            string baseurl = "http://101.227.68.49:7891/";
            string un = "10690254";
            string pw = "Lxwl-08";
            string dc = "15";//15表示中文 0英文 8UCS2

            Encoding encoding = Encoding.GetEncoding("GB2312");
          
            if (Content != "")
            {
                var bytes = encoding.GetBytes(Content);
                string sm = ToHexString(bytes);
                string url = baseurl + "mt?un=" + un + "&pw=" + pw + "&da=" + mobile + "&sm=" + sm + "&dc=" + dc + "&rd=1";
                int message = http_get(url);
                Sms sms = new Sms();
                sms.Mobile = mobile;
                sms.SMSContent = Content;
                sms.Status = message;
                sms.Addtime = DateTime.Now;
                sms.Save();
                return message;
            }
            else return 0;                

        }

        static private int http_get(string geturl)
        {
            try
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    wc.Headers["Content-Type"] = "application/x-www-form-urlencoded;charset=GB2312";
                    string result = Encoding.GetEncoding("GB2312").GetString(wc.DownloadData(geturl));
                    if (result == "r=0" || result.Contains("id=")) {
                        return 9;
                    }
                   
                }


            }
            catch (Exception)
            {
                //Module1.log.Append(ex.ToString());
            }
            return -1;
        }

        #region  HEX编码

        private const int AllocateThreshold = 256;

        private const string UpperHexChars = "0123456789ABCDEF";

        private const string LowerhexChars = "0123456789abcdef";

        private static string[] upperHexBytes;

        private static string[] lowerHexBytes;


        public static string ToHexString(this byte[] value)
        {

            return ToHexString(value, false);

        }


        public static string ToHexString(this byte[] value, bool upperCase)
        {

            if (value == null)
            {

                throw new ArgumentNullException("value");

            }


            if (value.Length == 0)
            {

                return string.Empty;

            }


            if (upperCase)
            {

                if (upperHexBytes != null)
                {

                    return ToHexStringFast(value, upperHexBytes);

                }


                if (value.Length > AllocateThreshold)
                {

                    return ToHexStringFast(value, UpperHexBytes);

                }


                return ToHexStringSlow(value, UpperHexChars);

            }


            if (lowerHexBytes != null)
            {

                return ToHexStringFast(value, lowerHexBytes);

            }


            if (value.Length > AllocateThreshold)
            {

                return ToHexStringFast(value, LowerHexBytes);

            }


            return ToHexStringSlow(value, LowerhexChars);

        }


        private static string ToHexStringSlow(byte[] value, string hexChars)
        {

            var hex = new char[value.Length * 2];

            int j = 0;


            for (var i = 0; i < value.Length; i++)
            {

                var b = value[i];

                hex[j++] = hexChars[b >> 4];

                hex[j++] = hexChars[b & 15];

            }


            return new string(hex);

        }


        private static string ToHexStringFast(byte[] value, string[] hexBytes)
        {

            var hex = new char[value.Length * 2];

            int j = 0;


            for (var i = 0; i < value.Length; i++)
            {

                var s = hexBytes[value[i]];

                hex[j++] = s[0];

                hex[j++] = s[1];

            }


            return new string(hex);

        }


        private static string[] UpperHexBytes
        {

            get
            {

                return (upperHexBytes ?? (upperHexBytes = new[] {

                "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "0A", "0B", "0C", "0D", "0E", "0F",

                "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "1A", "1B", "1C", "1D", "1E", "1F",

                "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "2A", "2B", "2C", "2D", "2E", "2F",

                "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "3A", "3B", "3C", "3D", "3E", "3F",

                "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "4A", "4B", "4C", "4D", "4E", "4F",

                "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "5A", "5B", "5C", "5D", "5E", "5F",

                "60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "6A", "6B", "6C", "6D", "6E", "6F",

                "70", "71", "72", "73", "74", "75", "76", "77", "78", "79", "7A", "7B", "7C", "7D", "7E", "7F",

                "80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "8A", "8B", "8C", "8D", "8E", "8F",

                "90", "91", "92", "93", "94", "95", "96", "97", "98", "99", "9A", "9B", "9C", "9D", "9E", "9F",

                "A0", "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "AA", "AB", "AC", "AD", "AE", "AF",

                "B0", "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8", "B9", "BA", "BB", "BC", "BD", "BE", "BF",

                "C0", "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "CA", "CB", "CC", "CD", "CE", "CF",

                "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "DA", "DB", "DC", "DD", "DE", "DF",

                "E0", "E1", "E2", "E3", "E4", "E5", "E6", "E7", "E8", "E9", "EA", "EB", "EC", "ED", "EE", "EF",

                "F0", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "FA", "FB", "FC", "FD", "FE", "FF" }));

            }

        }


        private static string[] LowerHexBytes
        {

            get
            {

                return (lowerHexBytes ?? (lowerHexBytes = new[] {

                "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "0a", "0b", "0c", "0d", "0e", "0f",

                "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "1a", "1b", "1c", "1d", "1e", "1f",

                "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "2a", "2b", "2c", "2d", "2e", "2f",

                "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "3a", "3b", "3c", "3d", "3e", "3f",

                "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "4a", "4b", "4c", "4d", "4e", "4f",

                "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "5a", "5b", "5c", "5d", "5e", "5f",

                "60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "6a", "6b", "6c", "6d", "6e", "6f",

                "70", "71", "72", "73", "74", "75", "76", "77", "78", "79", "7a", "7b", "7c", "7d", "7e", "7f",

                "80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "8a", "8b", "8c", "8d", "8e", "8f",

                "90", "91", "92", "93", "94", "95", "96", "97", "98", "99", "9a", "9b", "9c", "9d", "9e", "9f",

                "a0", "a1", "a2", "a3", "a4", "a5", "a6", "a7", "a8", "a9", "aa", "ab", "ac", "ad", "ae", "af",

                "b0", "b1", "b2", "b3", "b4", "b5", "b6", "b7", "b8", "b9", "ba", "bb", "bc", "bd", "be", "bf",

                "c0", "c1", "c2", "c3", "c4", "c5", "c6", "c7", "c8", "c9", "ca", "cb", "cc", "cd", "ce", "cf",

                "d0", "d1", "d2", "d3", "d4", "d5", "d6", "d7", "d8", "d9", "da", "db", "dc", "dd", "de", "df",

                "e0", "e1", "e2", "e3", "e4", "e5", "e6", "e7", "e8", "e9", "ea", "eb", "ec", "ed", "ee", "ef",

                "f0", "f1", "f2", "f3", "f4", "f5", "f6", "f7", "f8", "f9", "fa", "fb", "fc", "fd", "fe", "ff" }));

            }

        }


        #endregion


        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>-1发送错误，0发送限制，1发送成功</returns>
        public static MobileCode Vcode(string mobile)
        {
            int i = 0;
            Random rd = new Random();
            string code = rd.Next(100000, 999999).ToString();
            string content = "";
            string dtime = DateTime.Today.ToString();
            string time = "";
            int number = 1;
            int Id = 0;
            MobileCode mc = new MobileCode();
            Smsstore smsstore = new Smsstore();
            DataSet ds = smsstore.Model(mobile);
            if (ds != null)
            {
                Id = Convert.ToInt32(ds.Tables[0].Rows[0]["Id"].ToString());
                time = ds.Tables[0].Rows[0]["SendTime"].ToString();
                number = Convert.ToInt32(ds.Tables[0].Rows[0]["SMSNumber"].ToString());
                if (dtime == time)
                {
                    if (number < 3 || mobile == "15101587969")
                    {
                        code = ds.Tables[0].Rows[0]["SMSCode"].ToString();
                        number = number + 1;
                        i = 1;
                    }
                    else
                    {
                        i = 0;
                        code = "";
                    }
                }
                else
                {
                    i = 1;
                }
            }
            else { i = 1; }

            int j = -1;
            if (i == 1)
            {
                content = "您的验证码：" + code + "【领先办公】";
                int message = SMSsend(content, mobile);
                if (message == 9)
                {
                    smsstore.Id = Id;
                    smsstore.SendTime = Convert.ToDateTime(dtime);
                    smsstore.SMSNumber = number;
                    smsstore.SMSCode = code;
                    smsstore.Mobile = mobile;
                    smsstore.Save();
                    j = 1;
                }
                else
                {
                    j = -1;
                }

            }
            else
            {
                j = 0;
            }
            mc.Message = j;
            mc.Mobile = mobile;
            return mc;
        }

        /// <summary>
        /// 检查短信验证码
        /// cml：20170710
        /// </summary>
        /// <param name="telnumber">手机号</param>
        /// <param name="code">短信验证码</param>
        /// <returns></returns>
        public static bool CheckSmsverifycode(string telnumber, string code)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(telnumber) && !string.IsNullOrEmpty(code))
            {
                DBOperate dbo = new DBOperate();
                string sqlStr = "select SMSCode from Smsstore where Mobile='" + telnumber + "'";
                DataTable dt = dbo.GetDataSet(sqlStr).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString() == code)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 发送手机动态验证码
        /// cml:20170614
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="sendStrStart">发送文字前面部分</param>
        /// <param name="sendStrEnd">发送文字后面部分</param>
        /// <returns></returns>
        public static SendMobileCodeState SendPhoneVerifyCode(string phoneNumber, string sendStrStart, string sendStrEnd)
        {
            SendMobileCodeState smcState = new SendMobileCodeState();
            smcState.State = 0;
            smcState.msg = "";

            //01生成随机码
            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            int iResult;
            int iUp = 99999;
            int iDown = 10000;
            iResult = ran.Next(iDown, iUp);
            string code = "8" + iResult.ToString();

            //02判断数据库是否已存在该手机号，进而判断单日已发送数量         
            Smsstore smsStore = new Smsstore();
            DataSet smsDs = smsStore.Model(phoneNumber);
            if (smsDs != null && smsDs.Tables[0] != null && smsDs.Tables[0].Rows.Count > 0)
            {
                int sendnumber = Convert.ToInt32(smsDs.Tables[0].Rows[0]["SMSNumber"]);
                string sendtime = Convert.ToDateTime(smsDs.Tables[0].Rows[0]["SendTime"]).ToString("yyyy-MM-dd");
                if (sendtime == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    if (sendnumber > 3)
                    {
                        smcState.msg = "您今天的验证码发送次数已经用完。每个手机号一天最多发送3条短信。";
                    }
                    else
                    {
                        //发送验证码
                        int result = SMSsend(sendStrStart + code + sendStrEnd, phoneNumber);
                        if (result == 9)
                        {
                            smcState.State = 1;
                            smcState.msg = "手机验证码发送成功。";
                            //更新Smsstore
                            int id = Convert.ToInt32(smsDs.Tables[0].Rows[0]["Id"]);
                            smsStore.Id = id;
                            smsStore.Load();
                            smsStore.SMSCode = code;
                            smsStore.SMSNumber = sendnumber + 1;
                            smsStore.SendTime = DateTime.Now;
                            smsStore.Save();
                        }
                    }
                }
                else
                {
                    //发送验证码
                    int result = SMSsend(sendStrStart + code + sendStrEnd, phoneNumber);
                    if (result == 9)
                    {
                        smcState.State = 1;
                        smcState.msg = "手机验证码发送成功。";
                        //更新Smsstore
                        int id = Convert.ToInt32(smsDs.Tables[0].Rows[0]["Id"]);
                        smsStore.Id = id;
                        smsStore.Load();
                        smsStore.SMSCode = code;
                        smsStore.SMSNumber = 1;
                        smsStore.SendTime = DateTime.Now;
                        smsStore.Save();
                    }
                }
            }
            else
            {
                //发送验证码
                int result = SMSsend(sendStrStart + code + sendStrEnd, phoneNumber);
                if (result == 9)
                {
                    smcState.State = 1;
                    smcState.msg = "手机验证码发送成功。";
                    //添加一条数据到Smsstore       
                    smsStore.Mobile = phoneNumber;
                    smsStore.SMSCode = code;
                    smsStore.SMSNumber = 1;
                    smsStore.SendTime = DateTime.Now;
                    smsStore.Save();
                }
            }
            return smcState;
        }
    }

    /// <summary>
    /// 发送手机动态验证码状态实体类
    /// cml:20170614
    /// </summary>
    public class SendMobileCodeState
    {
        public int State { get; set; }
        public string msg { get; set; }
    }
}
