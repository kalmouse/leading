using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace LeadingClass
{
    public class EmailServ
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PassWord { get; set; }
        public DateTime LastSendTime { get; set; }
        private DBOperate m_dbo;

        public EmailServ()
        {
            Id = 0;
            Email = "";
            PassWord = "";
            LastSendTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public EmailServ(int Id)
            : this()
        {
            this.Id = Id;
            this.Load();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@Email", Email));
            arrayList.Add(new SqlParameter("@PassWord", PassWord));
            arrayList.Add(new SqlParameter("@LastSendTime", LastSendTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("EmailServ", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("EmailServ", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from EmailServ where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                return true;
            }
            return false;
        }
        private void LoadFromRow(DataRow row)
        {
            Id = DBTool.GetIntFromRow(row, "Id", 0);
            Email = DBTool.GetStringFromRow(row, "Email", "");
            PassWord = DBTool.GetStringFromRow(row, "PassWord", "");
            LastSendTime = DBTool.GetDateTimeFromRow(row, "LastSendTime");

        }
        public bool Delete()
        {
            string sql = string.Format("Delete from EmailServ where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        public bool GetEmailServ()
        {
            string sql = "select top 1 * from EmailServ order by LastSendTime ";
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                LoadFromRow(row);
                this.LastSendTime = DateTime.Now;//更新此邮箱的最新发件时间
                this.Save();
                return true;
            }
            return false;
        }
    }

    public class Email
    {
        public int Id { get; set; }
        public string MailFrom { get; set; }
        public string MailTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int IsOK { get; set; }
        public DateTime SendTime { get; set; }
        private DBOperate m_dbo;

        public Email()
        {
            Id = 0;
            MailFrom = "";
            MailTo = "";
            Subject = "";
            Body = "";
            IsOK = 0;
            SendTime = DateTime.Now;
            m_dbo = new DBOperate();
        }
        public Email(int Id)
            : this()
        {
            this.Id = Id;
            this.Load();
        }
        public int Save()
        {
            ArrayList arrayList = new ArrayList();
            if (Id > 0)
            {
                arrayList.Add(new SqlParameter("@Id", Id));
            }
            arrayList.Add(new SqlParameter("@MailFrom", MailFrom));
            arrayList.Add(new SqlParameter("@MailTo", MailTo));
            arrayList.Add(new SqlParameter("@Subject", Subject));
            arrayList.Add(new SqlParameter("@Body", Body));
            arrayList.Add(new SqlParameter("@IsOK", IsOK));
            arrayList.Add(new SqlParameter("@SendTime", SendTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("Email", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("Email", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from Email where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                MailFrom = DBTool.GetStringFromRow(row, "MailFrom", "");
                MailTo = DBTool.GetStringFromRow(row, "MailTo", "");
                Subject = DBTool.GetStringFromRow(row, "Subject", "");
                Body = DBTool.GetStringFromRow(row, "Body", "");
                IsOK = DBTool.GetIntFromRow(row, "IsOK", 0);
                SendTime = DBTool.GetDateTimeFromRow(row, "SendTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from Email where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
    }

    public class EmailCheck
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int EmailCode { get; set; }
        public int EmailNum { get; set; }
        public DateTime UpdateTime { get; set; }
        private DBOperate m_dbo;

        public EmailCheck()
        {
            Id = 0;
            Email = "";
            EmailCode = 0;
            EmailNum = 0;
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
            arrayList.Add(new SqlParameter("@Email", Email));
            arrayList.Add(new SqlParameter("@EmailCode", EmailCode));
            arrayList.Add(new SqlParameter("@EmailNum", EmailNum));
            arrayList.Add(new SqlParameter("@UpdateTime", UpdateTime));
            if (this.Id > 0)
            {
                m_dbo.UpdateData("EmailCheck", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            else
            {
                this.Id = m_dbo.InsertData("EmailCheck", (SqlParameter[])arrayList.ToArray(typeof(SqlParameter)));
            }
            return this.Id;
        }
        public bool Load()
        {
            string sql = string.Format("select * from EmailCheck where id={0}", Id);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Id = DBTool.GetIntFromRow(row, "Id", 0);
                Email = DBTool.GetStringFromRow(row, "Email", "");
                EmailCode = DBTool.GetIntFromRow(row, "EmailCode", 0);
                EmailNum = DBTool.GetIntFromRow(row, "EmailNum", 0);
                UpdateTime = DBTool.GetDateTimeFromRow(row, "UpdateTime");
                return true;
            }
            return false;
        }
        public bool Delete()
        {
            string sql = string.Format("Delete from EmailCheck where id={0} ", Id);
            return m_dbo.ExecuteNonQuery(sql);
        }
        public DataSet Model(string email)
        {
            string sql = string.Format("select * from EmailCheck where Email='{0}'", email);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds;
            }
            else return null;
        }
        public int GetEmail(string email, string code)
        {
            string sql = string.Format("select * from EmailCheck where Email='{0}' and EmailCode='{1}'", email, code);
            DataSet ds = m_dbo.GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return 1;
            }
            else return 0;
        }
    }

    public class LeadingEmail
    {
        public string mailFrom { get; set; }//发送者
        public string[] mailToArray { get; set; }// 收件人
        public string[] mailCcArray { get; set; }// 抄送
        public string mailSubject { get; set; }// 标题
        public string mailBody { get; set; }// 正文
        public string mailPwd { get; set; }// 发件人密码
        public string host { get; set; }// SMTP邮件服务器
        public bool isbodyHtml { get; set; }// 正文是否是html格式
        public string[] attachmentsPath { get; set; }// 附件

        public LeadingEmail()
        {
            EmailServ serv = new EmailServ();
            if (serv.GetEmailServ())
            {
                mailFrom = serv.Email;
                mailPwd = serv.PassWord;
                host = "smtp.ym.163.com";
            }
            isbodyHtml = true;
        }

        public bool Send()
        {
            //使用指定的邮件地址初始化MailAddress实例
            MailAddress maddr = new MailAddress(mailFrom);
            //初始化MailMessage实例
            MailMessage myMail = new MailMessage();
            myMail.IsBodyHtml = true;
            //向收件人地址集合添加邮件地址
            string strto = "";
            if (mailToArray != null)
            {
                for (int i = 0; i < mailToArray.Length; i++)
                {
                    myMail.To.Add(mailToArray[i].ToString());
                    strto += mailToArray[i] + ";";
                }
            }

            //向抄送收件人地址集合添加邮件地址
            if (mailCcArray != null)
            {
                for (int i = 0; i < mailCcArray.Length; i++)
                {
                    myMail.CC.Add(mailCcArray[i].ToString());
                }
            }
            //发件人地址
            myMail.From = maddr;

            //电子邮件的标题
            myMail.Subject = mailSubject;

            //电子邮件的主题内容使用的编码
            myMail.SubjectEncoding = Encoding.UTF8;

            //电子邮件正文
            myMail.Body = mailBody;

            //电子邮件正文的编码
            myMail.BodyEncoding = Encoding.Default;

            myMail.Priority = MailPriority.High;

            myMail.IsBodyHtml = isbodyHtml;

            //在有附件的情况下添加附件
            try
            {
                if (attachmentsPath != null && attachmentsPath.Length > 0)
                {
                    Attachment attachFile = null;
                    foreach (string path in attachmentsPath)
                    {
                        attachFile = new Attachment(path);
                        myMail.Attachments.Add(attachFile);
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception("在添加附件时有错误:" + err);
            }

            SmtpClient smtp = new SmtpClient();
            //指定发件人的邮件地址和密码以验证发件人身份
            smtp.Credentials = new System.Net.NetworkCredential(mailFrom, mailPwd);

            //设置SMTP邮件服务器
            smtp.Host = host;

            Email email = new Email();
            email.Body = this.mailBody;
            email.MailFrom = this.mailFrom;
            email.MailTo = strto;
            email.Subject = this.mailSubject;

            try
            {
                //将邮件发送到SMTP邮件服务器
                smtp.Send(myMail);
                email.IsOK = 1;
                email.Save();//记录邮件发送日志

                return true;
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                email.IsOK = 0;
                int id = email.Save();

                Sys_ErrorLog errorlog = new Sys_ErrorLog();
                errorlog.TableName = "SendEmail";
                errorlog.Memo = ex.Message;
                errorlog.RelationId = id.ToString();
                errorlog.Save();
                return false;
            }

        }

        #region 邮件审核
        /// <summary>
        /// 发送申请单邮件（可审核）
        /// </summary>
        /// <param name="applyId"></param>
        /// <param name="dt">该申请单是被谁审核的 管理员的表 </param>
        /// <returns></returns>
        public bool SendEmail(int applyId, int isPass)
        {
            if (applyId > 0)//申请单添加成功，审核消息发送给对应的管理员
            {
                //1、找到要发邮件的对应的人
                //通过ApplyId找到（新申请单生成）申请人，（审核过一级的）审核了一次的审核人
                VIPApply apply = new VIPApply(applyId);
                if (apply.Status == "待审核")
                {
                    int memberId = apply.OperatorId > 0 ? apply.OperatorId : apply.MemberId;
                    VIPApplyOption option = new VIPApplyOption();
                    Member member = new Member();
                    DataSet ds = member.GetDeptComIdByMemberId(memberId);
                    option.ComId = Convert.ToInt32(ds.Tables[0].Rows[0]["ComId"]);
                    option.MemberId = memberId;
                    option.RoleId = 2;//成本中心管理员
                    option.Code = Convert.ToString(ds.Tables[0].Rows[0]["Code"]);
                    VIPApplyManager vam = new VIPApplyManager();
                    DataSet dsAdmin = vam.GetAdmin(option);
                    List<int> memberIds = new List<int>();
                    if (dsAdmin.Tables[0].Rows.Count > 0)
                    {
                        int SendmemberId = Convert.ToInt32(dsAdmin.Tables[0].Rows[0]["MemberId"]);//上一级成本中心
                        memberIds.Add(SendmemberId);
                    }
                    //越级是 admin+sys_admin  逐级是admin
                    if (isPass == 1)//越级
                    {
                        MemberRole role = new MemberRole();
                        DataSet dsMemberIds = role.GetSysAdmin(option.ComId);//找到系统管理
                        if (dsMemberIds.Tables[0].Rows.Count > 0)
                        {
                            memberIds.Add(Convert.ToInt32(dsMemberIds.Tables[0].Rows[0]["MemberId"]));
                        }
                    }
                    if (memberIds.Count >0)
                    {
                        Memberinfocfg configure = new Memberinfocfg();
                        DataSet dsEmail = configure.GetIsPassAudit(memberIds);
                        if (dsEmail.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsEmail.Tables[0].Rows.Count; i++)
                            {
                                if (dsEmail.Tables[0].Rows[i]["Type"].ToString() == "实时发送" && Convert.ToInt32(dsEmail.Tables[0].Rows[i]["Value"]) == 1)
                                {
                                    string emailss = dsEmail.Tables[0].Rows[i]["Email"].ToString();
                                    if (emailss != "")
                                    {
                                        int mId = Convert.ToInt32(dsEmail.Tables[0].Rows[i]["MemberId"]);
                                        mailToArray = new string[] { emailss };
                                        mailSubject = "订单审核：您有新订单需要审核 来自 领先办公(Stationery Order: You have one new order need to approve _ from Leading Future)";
                                        mailBody = EmailBodyStr(applyId, mId);
                                        isbodyHtml = true;
                                        Send();
                                    }
                                }
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 邮件拼接的字符串
        /// </summary>
        /// <param name="applyId"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        private static string EmailBodyStr(int applyId, int memberId)
        {
            //申请单明细
            VIPApplyDetail vad = new VIPApplyDetail();
            List<int> applyIds = new List<int>();
            applyIds.Add(applyId);
            DataSet dsDetail = vad.GetApplyDetailByApplyId(applyIds);
            StringBuilder strTable = new StringBuilder();
            //申请单明细  添加英文
            strTable.Append(@"<table style='border-collapse:collapse;'>
<tr style='border:1px solid #aac1de;'>
<td style='border:1px solid #aac1de; padding:5px;'>序号（No.）</td>
<td style='border:1px solid #aac1de; padding:5px;'>商品编号（Id）</td>
<td style='border:1px solid #aac1de; padding:5px; text-align:center;'>商品名称（Product ）</td>
<td style='border:1px solid #aac1de; padding:5px;'>单位（Unit）</td>
<td style='border:1px solid #aac1de; padding:5px;'>数量（Num）</td>
<td style='border:1px solid #aac1de; padding:5px;'>单价（Price）</td>
<td style='border:1px solid #aac1de; padding:5px;'>金额（Amount）</td>
</tr>");
            int index = 1;
            foreach (DataRow row in dsDetail.Tables[0].Rows)
            {
                decimal amount = Convert.ToDecimal(row["VIPPrice"]) * Convert.ToInt32(row["Num"]);
                strTable.Append("<tr style='border:1px solid #aac1de;'>");
                strTable.Append("<td style='border:1px solid #aac1de; padding:5px; text-align:center;'>" + index.ToString() + "</td>");
                strTable.Append("<td style='border:1px solid #aac1de; padding:5px; text-align:center;'>" + row["GoodsId"] + "</td>");
                strTable.Append("<td style='border:1px solid #aac1de; padding:5px;'>" + row["DisplayName"] + "</td>");
                strTable.Append("<td style='border:1px solid #aac1de; padding:5px; text-align:center;'>" + row["Unit"] + "</td>");
                strTable.Append("<td style='border:1px solid #aac1de; padding:5px; text-align:center;'>" + row["num"] + "</td>");
                strTable.Append("<td style='border:1px solid #aac1de; padding:5px; text-align:center;'>" + string.Format("{0:F2}", row["VIPPrice"]) + "</td>");
                strTable.Append("<td style='border:1px solid #aac1de; padding:5px; text-align:center;'>" + amount.ToString("0.00") + "</td>");
                strTable.Append("</tr>");
                index += 1;
            }
            strTable.Append("</table>");
            string applyId1s = CommenClass.Encrypt.EncryptDES(applyId.ToString(), "leading1");
            //获取token
            Emailapply ea = new Emailapply();
            ea.ApplyId = applyId;
            ea.MemberId = memberId;
            ea.Token = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
            ea.StartTime = DateTime.Now;
            ea.EndTime = DateTime.Now.AddDays(30); //DateTime.Now.AddHours(48);
            ea.Save();

            VIPApplyManager vam = new VIPApplyManager();
            VIPApplyOption option = new VIPApplyOption();
            option.Id = applyId;
            option.ConfirmLevel = -1;
            DataRow row1 = vam.ReadApply(option).Tables[0].Rows[0];
            string DeptName = DBTool.GetStringFromRow(row1, "DeptName", "");
            string RealName = DBTool.GetStringFromRow(row1, "RealName", "");
            double SumMoney = DBTool.GetDoubleFromRow(row1, "SumMoney", 0);
            DateTime UpdateTime = DBTool.GetDateTimeFromRow(row1, "UpdateTime");
            string ApplyInfo = string.Format("Id：{0}&nbsp;&nbsp;部门(Dept)：{1}&nbsp;&nbsp;申请人(Applicant）：{2}&nbsp;&nbsp;总金额（Amount）：{3}&nbsp;&nbsp;日期（Date）：{4}", applyId, DeptName, RealName, SumMoney, UpdateTime);
            string url = CommenClass.SiteUrl.HomeUrl();
            //string url ="http://localhost:2568/";
            //审核通过
            string strConfirm = string.Format("<a href='" + url + "Vip/EmailConfirmOrders?Id1={0}&Id2={1}' style='font-size:14pt;text-decoration:none;'>审核通过(Pass)</a>", applyId1s, ea.Token);
            //查看详情/Vip/Apply/id
            string applydetail = string.Format("<a href='" + url + "Account/AutoLogin?Id1={0}&Id2={1}' style='font-size:14pt;text-decoration:none;'>查看详情(Detail)</a>",
                "/Vip/Apply/" + applyId + "", ea.Token);
            //回到审核下单/Vip/ConfirmOrder
            string strToConfirmOrder = string.Format("<a href='" + url + "Account/AutoLogin?Id1={0}&Id2={1}' style='font-size:14pt;text-decoration:none;'>更多申请单(More...)</a>", "/Vip/ConfirmOrder", ea.Token);

            string str = string.Format(@"
系统邮件，请勿回复！（Don't reply this email.）<br/><br/>
您好，您有1张订单需要审批，明细如下（Hello, There is an application form need you to approve）:<br/><br/>
{0}<br/><br/>
{1}<br/><br/> {2}&nbsp;&nbsp;&nbsp;&nbsp;{3}&nbsp;&nbsp;&nbsp;&nbsp;{4}<br/>",
               ApplyInfo, strTable, strConfirm, applydetail, strToConfirmOrder);
            return str;
        }
        #endregion

        #region 邮件发送驳回、审核通过通知
        /// <summary>
        /// 被驳回的邮件
        /// </summary>
        /// <param name="applyId">被驳回的申请单</param>
        /// <param name="memberId">申请人</param>
        /// <returns></returns>
        public bool EmailToReject(int applyId, int memberId)
        {
            Memberinfocfg mic = new Memberinfocfg();
            List<int> memberIds = new List<int>();
            memberIds.Add(memberId);
            if (memberIds != null)
            {
                DataSet ds = mic.GetIsPassAudit(memberIds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Type"].ToString() == "实时发送" && Convert.ToInt32(ds.Tables[0].Rows[0]["Value"]) == 1)
                    {
                        mailToArray = new string[] { ds.Tables[0].Rows[0]["Email"].ToString() };
                        mailSubject = "申请单驳回：您有申请单被驳回。 来自 领先办公(Reject Order: You have one order be reject _ from Leading Future)";
                        mailBody = EmailBodyStrToReject(applyId, memberId);
                        Send();
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 邮件内容
        /// </summary>
        /// <param name="applyId"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        private static string EmailBodyStrToReject(int applyId, int memberId)
        {
            //获取token
            Emailapply ea = new Emailapply();
            ea.ApplyId = applyId;
            ea.MemberId = memberId;
            ea.Token = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
            ea.StartTime = DateTime.Now;
            ea.EndTime = DateTime.Now.AddDays(30); //DateTime.Now.AddHours(48);
            ea.Save();
            string url = CommenClass.SiteUrl.HomeUrl();
            string applydetail = string.Format("<a href='" + url + "Account/AutoLogin?Id1={0}&Id2={1}' style='font-size:14pt;text-decoration:none;'>查看详情(Detail)</a>","/Vip/Apply/" + applyId + "", ea.Token);

            string str = string.Format(@"
系统邮件，请勿回复！（Don't reply this email.） &nbsp;&nbsp;您好，您有申请单被驳回，申请单号是{0}，请及时处理。（Hello, Your application are dismissed,the No. is {0},Please deal with.）:<br/><br/>{1}", applyId, applydetail);
            return str;
        }

        /// <summary>
        /// 申请单审核通过的通知
        /// </summary>
        /// <param name="applyId">通过的申请单</param>
        /// <param name="memberId">申请人</param>
        /// <returns></returns>
        public bool EmailToPass(int applyId, int memberId)
        {
            Memberinfocfg mic = new Memberinfocfg();
            List<int> memberIds = new List<int>();
            memberIds.Add(memberId);
            if (memberIds != null)
            {
                DataSet ds = mic.GetIsPassAudit(memberIds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Type"].ToString() == "实时发送" && Convert.ToInt32(ds.Tables[0].Rows[0]["Value"]) == 1)
                    {
                        mailToArray = new string[] { ds.Tables[0].Rows[0]["Email"].ToString() };
                        mailSubject = "申请单通过：您的申请单通过了。 来自 领先办公";
                        mailBody = EmailBodyStrToPass(applyId, memberId);
                        Send();
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 审核通过的邮件内容
        /// </summary>
        /// <param name="applyId"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        private static string EmailBodyStrToPass(int applyId, int memberId)
        {
            //获取token
            Emailapply ea = new Emailapply();
            ea.ApplyId = applyId;
            ea.MemberId = memberId;
            ea.Token = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
            ea.StartTime = DateTime.Now;
            ea.EndTime = DateTime.Now.AddDays(30); 
            ea.Save();
            string url = CommenClass.SiteUrl.HomeUrl();
            string applydetail = string.Format("<a href='" + url + "Account/AutoLogin?Id1={0}&Id2={1}' style='font-size:14pt;text-decoration:none;'>查看详情(Detail)</a>", "/Vip/Apply/" + applyId + "", ea.Token);
            string str = string.Format(@"系统邮件，请勿回复！（Don't reply this email.）&nbsp;&nbsp;  恭喜，您的申请单通过了，申请单号是{0}。（Congratulations, your application has been approved,the No. is {0}.）:<br/><br/>{1}", applyId, applydetail);
            return str;
        }
        /// <summary>
        /// 消息推送给上级管理员的通知
        /// </summary>
        /// <param name="applyId"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public bool NoticeToApplyMember(int applyId, int memberId)
        {
            Memberinfocfg mic = new Memberinfocfg();
            List<int> memberIds = new List<int>();
            memberIds.Add(memberId);
            if (memberIds != null)
            {
                DataSet ds = mic.GetIsPassAudit(memberIds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Type"].ToString() == "实时发送" && Convert.ToInt32(ds.Tables[0].Rows[0]["Value"]) == 1)
                    {
                        mailToArray = new string[] { ds.Tables[0].Rows[0]["Email"].ToString() };
                        mailSubject = "申请单通知：申请单推送给您的上司。 来自 领先办公";
                        mailBody = EmailBodyStrToNotice(applyId, memberId);
                        Send();
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 通知的内容
        /// </summary>
        /// <param name="applyId"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        private static string EmailBodyStrToNotice(int applyId, int memberId)
        {
            //获取token
            Emailapply ea = new Emailapply();
            ea.ApplyId = applyId;
            ea.MemberId = memberId;
            ea.Token = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
            ea.StartTime = DateTime.Now;
            ea.EndTime = DateTime.Now.AddDays(30);
            ea.Save();
            string url = CommenClass.SiteUrl.HomeUrl();
            string applydetail = string.Format("<a href='" + url + "Account/AutoLogin?Id1={0}&Id2={1}' style='font-size:14pt;text-decoration:none;'>查看详情(Detail)</a>", "/Vip/Apply/" + applyId + "", ea.Token);
            string str = string.Format(@"系统邮件，请勿回复(仅供参考)！（Don't reply this email.）&nbsp;&nbsp; 您提交的申请单，单号是{0}的，邮件已发送给您的上司，祝您好运。（You submit the application,the No. is {0},has been send to your boss.Good Luck!）:<br/><br/>{1}", applyId,applydetail);
            return str;
        }
        #endregion
    }
}
