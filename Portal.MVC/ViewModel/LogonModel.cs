using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Niqiu.Core.Domain.User;

namespace Portal.MVC.ViewModel
{
    public class LogOnModel
    {
        [Required(ErrorMessage = "请输入用户名")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "请输入原密码")]
        [DataType(DataType.Password)]
        [Display(Name = "原密码")]
        [Remote("CheckPassword", "Account", ErrorMessage = "密码输入错误")]
        public string Password { get; set; }

        [Required(ErrorMessage = "请输入新密码")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }


    }
    public class EmailModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "当前密码:")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [Remote("CheckMail", "User", ErrorMessage = "该邮箱已经存在！")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "请输入正确的email")]
        public string Email { get; set; }
    }
    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "当前密码:")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码:")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码:")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "新密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }
    public class RegisterModel
    {
        public RegisterModel(int id)
        {
            Id = id;
        }

        public RegisterModel()
        {
            
        }

        public RegisterModel(User user)
        {
            if (user != null)
            {
                Id = user.Id;
                UserName = user.Username;
                 Mobile = user.Mobile;
                 Password = "";
                Email = user.Email;

                if (user.UserRoles.Any())
                {
                    RoleId = user.UserRoles.FirstOrDefault().Id;
                }

            }


        }

        public void Empty()
        {
            UserName = "";
            Password = "";
            Email = "";
            Mobile = "";
        }

        public int Id { get; set; }

        [Required(ErrorMessage="用户名不能为空")]
        [Display(Name = "用户名")]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 2)]
        [Remote("CheckUserName", "User", ErrorMessage = "该用户名已经存在")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "请输入正确的email")]
        [Remote("CheckMail", "User", ErrorMessage = "该邮箱已经存在")]
        public string Email { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [StringLength(20, ErrorMessage = "{0}由6到20个字符或数字组成。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }


        [Display(Name = "手机号码")]
        [Required(ErrorMessage = "手机号不能为空")]
        [RegularExpression(@"^1[3458][0-9]{9}$", ErrorMessage = "手机号格式不正确")]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }

        [Display(Name = "角色")]
        [Required(ErrorMessage = "角色不能为空")]
        public int  RoleId { get; set; }
    }

}