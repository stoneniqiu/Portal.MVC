namespace Niqiu.Core.Domain.Common
{
  public  interface IPortalContext
    {
      User.User CurrentUser { get; set; }

      bool IsAdmin { get; set; }

      bool SendMail(string toEmails, string emailText, string subject);

      void AsyncSendMail(string toEmails, string emailText, string subject);

    }
}
