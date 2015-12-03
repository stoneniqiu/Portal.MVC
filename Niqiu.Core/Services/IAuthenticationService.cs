using Niqiu.Core.Domain.User;

namespace Niqiu.Core.Services
{
   public interface IAuthenticationService
    {
        void SignIn(User user, bool createPersistentCookie);
        void SignOut();
        User GetAuthenticatedCustomer();

        bool IsCurrentUser { get; }
    }
}
