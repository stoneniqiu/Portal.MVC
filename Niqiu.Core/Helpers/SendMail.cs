using System;
using System.IO;
using System.Net.Mail;

namespace Niqiu.Core.Helpers
{
    public class SendMail
    {


        /// <summary>
        /// The email gateway
        /// </summary>
        private EmailGateway _emailGateway = new EmailGateway();

        /// <summary>
        /// Gets or sets the email gateway.
        /// </summary>
        /// <value>The email gateway.</value>
        public EmailGateway EmailGateway
        {
            get { return _emailGateway; }
            set { _emailGateway = value; }
        }

        /// <summary>
        /// Gets or sets the mail host.
        /// </summary>
        /// <value>The mail host.</value>
        public string MailHost
        {
            get { return EmailGateway.MailHost; }
            set { EmailGateway.MailHost = value; }
        }

        /// <summary>
        /// Gets or sets the mail port.
        /// </summary>
        /// <value>The mail port.</value>
        public int MailPort
        {
            get { return EmailGateway.MailPort; }
            set { EmailGateway.MailPort = value; }
        }

        /// <summary>
        /// Gets or sets the mailhost username.
        /// </summary>
        /// <value>The mailhost username.</value>
        public string MailhostUsername
        {
            get { return EmailGateway.MailHostUsername; }
            set { EmailGateway.MailHostUsername = value; }
        }

        /// <summary>
        /// Gets or sets the mailhost password.
        /// </summary>
        /// <value>The mailhost password.</value>
        public string MailhostPassword
        {
            get { return EmailGateway.MailHostPassword; }
            set { EmailGateway.MailHostPassword = value; }
        }

        /// <summary>
        /// Gets or sets from address.
        /// </summary>
        /// <value>From address.</value>
        public string FromAddress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use SSL].
        /// </summary>
        /// <value><c>true</c> if [use SSL]; otherwise, <c>false</c>.</value>
        public bool UseSSL
        {
            get { return EmailGateway.UseSSL; }
            set { EmailGateway.UseSSL = value; }
        }

        /// <summary>
        /// Gets or sets the reply to address.
        /// </summary>
        /// <value>The reply to address.</value>
        public string ReplyToAddress { get; set; }

        private void SendMessage(string from, string to, string replyto, string subject, string message, string workingFolder)
        {
            try
            {
                using (var actualMessage = GetMailMessage(from, to, replyto, subject, message, workingFolder, null))
                {
                    _emailGateway.Send(actualMessage);
                }
            }
            catch (Exception e)
            {
                throw new Exception("EmailPublisher exception: " + e, e);
            }
        }

        public bool SendaMail(string toEmails, string emailText, string subject = "日志系統(SI)")
        {

            const string systermtxt = "该邮件为系统自动发送，请勿回复";
            bool isSendOk;
            try
            {
                SendMessage(FromAddress, toEmails, "", subject, emailText + systermtxt, "");
                isSendOk = true;
            }
            catch (Exception e)
            {
                isSendOk = false;
            }
            return isSendOk;
        }

        public SendMail()
        {
            MailPort = 25;
            UseSSL = false;

            MailhostUsername = "Dawn.wang";
            MailhostPassword = "Delta123456";
            FromAddress = "DAWN.WANG@DELTAWW.COM.CN";
            MailHost = "172.171.161.8";
        }

        public SendMail(string fromemail,string name, string pwd, string host)
        {
            FromAddress = fromemail;
            MailhostUsername = name;
            MailhostPassword = pwd;
            MailHost = host;
            UseSSL = false;
            MailPort = 25;
        }


        protected static MailMessage GetMailMessage(string from, string to, string replyto, string subject,
         string messageText, string workingFolder, string[] attachments)
        {
            var mailMessage = new MailMessage();
            mailMessage.To.Add(to);
            mailMessage.From = new MailAddress(from);
            if (!String.IsNullOrEmpty(replyto)) mailMessage.ReplyTo = new MailAddress(replyto);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = messageText;
            mailMessage.Priority = MailPriority.High;


            // Add any attachments
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    var fullPath = attachment;
                    if (!Path.IsPathRooted(fullPath)) fullPath = Path.Combine(workingFolder, fullPath);
                    if (File.Exists(fullPath))
                    {
                        var mailAttachment = new Attachment(fullPath);
                        mailMessage.Attachments.Add(mailAttachment);
                    }
                }
            }

            return mailMessage;
        }
        /// <summary>
        /// 异步发送邮件
        /// </summary>
        /// <param name="toMails"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        public void SyncSendMail(string toMails, string content, string subject = "DIAView")
        {
            Action invokeAction = () => SendaMail(toMails, content, subject);
            invokeAction.BeginInvoke(null, invokeAction);
        }


       
    }
}