using System.Net;
using System.Net.Mail;

namespace Niqiu.Core.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class EmailGateway
    {
        private string _mailhostUsername;
        private string _mailhostPassword;
        private readonly SmtpClient _smtpServer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailGateway" /> class.	
        /// </summary>
        /// <remarks></remarks>
        public EmailGateway()
        {
            _smtpServer = new SmtpClient();
        }

        /// <summary>
        /// Gets or sets the mail host.	
        /// </summary>
        /// <value>The mail host.</value>
        /// <remarks></remarks>
        public virtual string MailHost
        {
            get { return _smtpServer.Host; }
            set { _smtpServer.Host = value; }
        }

        /// <summary>
        /// Gets or sets the mail port.	
        /// </summary>
        /// <value>The mail port.</value>
        /// <remarks></remarks>
        public virtual int MailPort
        {
            get { return _smtpServer.Port; }
            set { _smtpServer.Port = value; }
        }

        /// <summary>
        /// Gets or sets the use SSL.	
        /// </summary>
        /// <value>The use SSL.</value>
        /// <remarks></remarks>
        public bool UseSSL
        {
            get { return _smtpServer.EnableSsl; }
            set { _smtpServer.EnableSsl = value; }
        }

        /// <summary>
        /// Gets or sets the mail host username.	
        /// </summary>
        /// <value>The mail host username.</value>
        /// <remarks></remarks>
        public string MailHostUsername
        {
            get { return _mailhostUsername; }
            set { _mailhostUsername = value; }
        }

        /// <summary>
        /// Gets or sets the mail host password.	
        /// </summary>
        /// <value>The mail host password.</value>
        /// <remarks></remarks>
        public string MailHostPassword
        {
            get { return _mailhostPassword; }
            set { _mailhostPassword = value; }
        }

        /// <summary>
        /// Sends the specified mail message.	
        /// </summary>
        /// <param name="mailMessage">The mail message.</param>
        /// <remarks></remarks>
        public virtual void Send(MailMessage mailMessage)
        {
            if (MailHostUsername != null && MailHostPassword != null)
            {
                _smtpServer.Credentials = new NetworkCredential(_mailhostUsername, _mailhostPassword);
            }
            _smtpServer.Send(mailMessage);
        }
    }
}
