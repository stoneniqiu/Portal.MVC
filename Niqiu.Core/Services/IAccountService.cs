using Niqiu.Core.Domain.User;

namespace Niqiu.Core.Services
{
   public interface IAccountService
    {
       UserLoginResults ValidateUser(string usernameOrEmail, string password);
       /// <summary>
       /// Sets a user email
       /// </summary>
       /// <param name="user">Customer</param>
       /// <param name="newEmail">New email</param>
       void SetEmail(User user, string newEmail);

       /// <summary>
       /// Sets a customer username
       /// </summary>
       /// <param name="user">Customer</param>
       /// <param name="newUsername">New Username</param>
       void SetUsername(User user, string newUsername);


       /// <summary>
       /// Register customer
       /// </summary>
       /// <param name="request">Request</param>
       /// <returns>Result</returns>
       UserRegistrationResult RegisterUser(UserRegistrationRequest request);

       /// <summary>
       /// Change password
       /// </summary>
       /// <param name="request">Request</param>
       /// <returns>Result</returns>
       PasswordChangeResult ChangePassword(ChangePasswordRequest request);

       bool ChangePassword(int userid, string password);

    }
}
