using System;
using Niqiu.Core.Domain;
using Niqiu.Core.Domain.User;
using Niqiu.Core.Helpers;

namespace Niqiu.Core.Services
{
   public class AccoutService : IAccountService
   {
       private readonly IUserService _userService;

       public AccoutService(IUserService userService)
       {
           _userService = userService;
       }

       public UserLoginResults ValidateUser(string usernameOrEmail, string password)
       {
           User user=null;


            user = _userService.GetUserByUsername(usernameOrEmail);
           if (user == null && usernameOrEmail.Contains("@"))
           {
               user = _userService.GetUserByEmail(usernameOrEmail);
           }


           if (user == null)
               return UserLoginResults.UserNotExist;
           if (user.Deleted)
               return UserLoginResults.Deleted;
           if (!user.Active)
               return UserLoginResults.NotActive;
           //only registered can login
           //if (!user.IsRegistered())
           //    return UserLoginResults.NotRegistered;

           string pwd = "";
           //这个地方要注意 日后加强的时候要更改。
           switch (user.PasswordFormat)
           {
               case PasswordFormat.Encrypted:
                   pwd = Encrypt.EncryptString(password);
                 break;
               case PasswordFormat.Hashed:
                   pwd = Encrypt.CreatePasswordHash(password, user.PasswordSalt);
                   break;
               default:
                   pwd = password;
                   break;
           }
           bool isValid = Encrypt.GetMd5Code(pwd) == user.Password;
           if (!isValid)
               return UserLoginResults.WrongPassword;

           //save last login date
           user.LastLoginDateUtc = DateTime.UtcNow;
           _userService.UpdateUser(user);
           return UserLoginResults.Successful;
       }

       public void SetEmail(User user, string newEmail)
       {
            
       }

       public void SetUsername(User user, string newUsername)
       {
           if (user == null)
               throw new ArgumentNullException("user");
              newUsername = newUsername.Trim();

           if (newUsername.Length > 100)
               throw new PortalException("用户名太长");

           var user2 = _userService.GetUserByUsername(newUsername);
           if (user2 != null && user.Id != user2.Id)
               throw new PortalException("用户名已经存在");

           user.Username = newUsername;
           _userService.UpdateUser(user);
       }

       public UserRegistrationResult RegisterUser(UserRegistrationRequest request)
       {
           if (request == null)
               throw new ArgumentNullException("request");

           if (request.User == null)
               throw new ArgumentException("当前用户为空");

           var result = new UserRegistrationResult();

           if (request.User.IsRegistered())
           {
               result.AddError("当前用户已经注册");
               return result;
           }
           if (String.IsNullOrEmpty(request.Email))
           {
               result.AddError("邮箱不能为空");
               return result;
           }
           if (!CommonHelper.IsValidEmail(request.Email))
           {
               result.AddError("邮件格式错误");
               return result;
           }
           if (String.IsNullOrWhiteSpace(request.Password))
           {
               result.AddError("密码不能为空");
               return result;
           }
           if (String.IsNullOrWhiteSpace(request.Mobile))
           {
               result.AddError("手机号码不能为空");
               return result;
           }
           if (_userService.GetUserByUsername(request.Username) != null)
           {
               result.AddError("用户名已经存在");
               return result;
           }

           request.User.Username = request.Username;
           request.User.Email = request.Email;
           request.User.PasswordFormat = request.PasswordFormat;
           request.User.Mobile = request.Mobile;
           request.User.ImgUrl = "/Content/user_img.jpg";
           switch (request.PasswordFormat)
           {
               case PasswordFormat.Clear:
                   {
                       request.User.Password = request.Password;
                   }
                   break;
               case PasswordFormat.Encrypted:
                   {
                       request.User.Password = Encrypt.GetMd5Code(request.Password);
                   }
                   break;
               case PasswordFormat.Hashed:
                   {
                       string saltKey = Encrypt.CreateSaltKey(5);
                       request.User.PasswordSalt = saltKey;
                       request.User.Password = Encrypt.CreatePasswordHash(request.Password, saltKey);
                   }
                   break;
               default:
                   break;
           }
           request.User.Active = request.IsApproved;

           // 添加基本角色。
           //var registeredRole = _userService.GetUserRoleBySystemName(SystemUserRoleNames.Registered);
           //if (registeredRole == null)
           //    throw new PortalException("'Registered' 角色加载失败");
           if (request.User.Id == 0)
           {

               _userService.InsertUser(request.User);
               request.User = _userService.GetUserByUsername(request.Username);
           }
           //request.User.UserRoles.Add(registeredRole);
           //_userService.UpdateUser(request.User);

           return result;
       }

       public PasswordChangeResult ChangePassword(ChangePasswordRequest request)
       {
           if (request == null)
               throw new ArgumentNullException("request");

           var result = new PasswordChangeResult();
           if (String.IsNullOrWhiteSpace(request.Email))
           {
               result.AddError("邮件不能为空");
               return result;
           }
           if (String.IsNullOrWhiteSpace(request.NewPassword))
           {
               result.AddError("密码不能为空");
               return result;
           }

           var customer =_userService.GetUserByEmail(request.Email);
           if (customer == null)
           {
               result.AddError("邮件不存在");
               return result;
           }

           var requestIsValid = false;
           if (request.ValidateRequest)
           {
               //password
               string oldPwd = "";
               switch (customer.PasswordFormat)
               {
                   case PasswordFormat.Encrypted:
                       oldPwd = Encrypt.GetMd5Code(request.OldPassword);
                       break;
                   case PasswordFormat.Hashed:
                       oldPwd = Encrypt.CreatePasswordHash(request.OldPassword, customer.PasswordSalt);
                       break;
                   default:
                       oldPwd = request.OldPassword;
                       break;
               }

               bool oldPasswordIsValid = oldPwd == customer.Password;
               if (!oldPasswordIsValid)
                   result.AddError("旧密码错误");

               if (oldPasswordIsValid)
                   requestIsValid = true;
           }
           else
               requestIsValid = true;

           if (requestIsValid)
           {
               switch (request.NewPasswordFormat)
               {
                   case PasswordFormat.Clear:
                       {
                           customer.Password = request.NewPassword;
                       }
                       break;
                   case PasswordFormat.Encrypted:
                       {
                           customer.Password = Encrypt.GetMd5Code(request.NewPassword);
                       }
                       break;
                   case PasswordFormat.Hashed:
                       {
                           string saltKey = Encrypt.CreateSaltKey(5);
                           customer.PasswordSalt = saltKey;
                           customer.Password = Encrypt.CreatePasswordHash(request.NewPassword, saltKey);
                       }
                       break;
                   default:
                       break;
               }
               customer.PasswordFormat = request.NewPasswordFormat;
               _userService.UpdateUser(customer);
           }

           return result;
       }

       public bool ChangePassword(int userid,string password)
       {
           var rawuser = _userService.GetUserById(userid);
           if (rawuser != null)
           {
               rawuser.Password = Encrypt.EncryptString(password);
               _userService.UpdateUser(rawuser);
               return true;
           }
           return false;
       }
    }
}
