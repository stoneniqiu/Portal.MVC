using Niqiu.Core.Domain.User;

namespace Niqiu.Core.Services
{
    /// <summary>
    /// Work context
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// Gets or sets the current customer
        /// </summary>
        User CurrentUser { get; set; }
        /// <summary>
        /// Gets or sets the original customer (in case the current one is impersonated)
        /// </summary>
      
        /// <summary>
        /// Get or set value indicating whether we're in admin area
        /// </summary>
        bool IsAdmin { get; set; }
        bool IsCurrentUser { get; }

        bool SendMail(string toEmails, string emailText, string subject);

        void AsyncSendMail(string toEmails, string emailText, string subject);
    }
}
