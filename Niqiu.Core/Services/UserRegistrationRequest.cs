using Niqiu.Core.Domain.User;

namespace Niqiu.Core.Services
{
    public class UserRegistrationRequest
    {
        public User User { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public PasswordFormat PasswordFormat { get; set; }
        public bool IsApproved { get; set; }
        public string Mobile { get; set; }


        public UserRegistrationRequest(User user, string email,string mobile, string username,
            string password, 
            PasswordFormat passwordFormat,
            bool isApproved = true)
        {
            this.User = user;
            this.Email = email;
            this.Username = username;
            this.Password = password;
            this.PasswordFormat = passwordFormat;
            this.IsApproved = isApproved;
            Mobile = mobile;
        }
    }
}
