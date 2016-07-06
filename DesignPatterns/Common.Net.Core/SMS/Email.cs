/***************************************************************************** 
*        filename :MailHelper 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.42000 
*        新建项输入的名称:   MailHelper 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Common.Net.Core 
*        文件名:             MailHelper 
*        创建系统时间:       2016/2/3 10:13:10 
*        创建年份:           2016 
/*****************************************************************************/
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Common.Net.Core
{
    /// <summary>
    /// 发送邮件类型
    /// </summary>
    public enum EmailSysEnum
    {
        /// <summary>
        /// 用户注册
        /// </summary>
        Register,
        /// <summary>
        /// 系统（内部使用，发送系统通知用）
        /// </summary>
        Sys,
        /// <summary>
        /// 系统（外部用户，发送系统邮件，如找回密码等）
        /// </summary>
        Service,
        /// <summary>
        /// 推广邮件（广告推送）
        /// </summary>
        Pub
    }

    /// <summary>
    /// 邮件发送对象
    /// </summary>
    public class Email
    {
        /// <summary>发送邮件的方法
        /// 发送邮件的方法
        /// </summary>
        /// <param name="toemail">收件人的邮件箱</param>
        /// <param name="mailtitle">发送邮件的标题</param>
        /// <param name="mailcontent">发送邮件的内容</param>
        /// <param name="addfiles">发送邮件的内容</param>
        /// <param name="emailsys">邮件类型</param>
        /// <returns>成功返回 ok  失败返回失败的原因</returns>
        public static string SendEmail(string toemail, string mailtitle, string mailcontent, string addfiles, EmailSysEnum emailsys)
        {
            string result = string.Empty;
            try
            {
                //发件人邮箱地址，收件人邮箱地址，邮件标题，邮件内容
                MailMessage myMail = null;
                SmtpClient client = null;

                myMail = EmailCompose(emailsys.ToString() + "@mail.mofangge.com", toemail, mailtitle, mailcontent, addfiles);//new MailMessage();
                //System.Web .Mail .MailMessage                    
                client = new SmtpClient("mail.mofangge.com");//指定SMTP服务器
                client.Credentials = new NetworkCredential(emailsys.ToString() + "@mail.mofangge.com", "Mfg1314..");//指定用户名和密码  

                myMail.SubjectEncoding = System.Text.Encoding.GetEncoding("gb2312");//邮件标题的编码
                myMail.BodyEncoding = System.Text.Encoding.GetEncoding("gb2312");//邮件内容的编码
                myMail.IsBodyHtml = true;//邮件的内容为Html格式
                client.EnableSsl = false;//是否加密连接
                client.Send(myMail);//发送指定邮件
                result = "ok";
            }
            catch (SmtpFailedRecipientsException e)
            {
                result = e.Message;
            }
            catch (SmtpException e)
            {
                result = e.Message;
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }

        /// <summary>
        /// 用于组装邮件
        /// </summary>
        /// <param name="fromMail">发送人地址</param>
        /// <param name="toMail">收件人地址</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="attachmentFiles">附件</param>
        /// <returns></returns>
        private static MailMessage EmailCompose(string fromMail, string toMail, string subject, string body, string attachmentFiles)
        {
            MailMessage message;
            MailAddress from = new MailAddress(fromMail, "学习社区", System.Text.Encoding.GetEncoding("gb2312"));
            MailAddress to = new MailAddress(toMail);
            message = new MailMessage(from, to);
            message.Body = body.Length > 0 ? body : "云教学平台";
            // Include some non-ASCII characters in body and subject.
            string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });//这是四个方向箭头，用于装饰
            //message.Body += Environment.NewLine + someArrows;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = subject;
            message.SubjectEncoding = System.Text.Encoding.GetEncoding("gb2312");
            if (attachmentFiles.Length > 0)
            {
                char[] splitarr = { '*' };
                string[] attachmentCollection = attachmentFiles.Split(splitarr, System.StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < attachmentCollection.Length; i++)
                {
                    if (attachmentCollection[i] != null && File.Exists(attachmentCollection[i]))
                        message.Attachments.Add(new Attachment(attachmentCollection[i]));
                }
            }
            return message;

        }
    }
}
