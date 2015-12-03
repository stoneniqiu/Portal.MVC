using System.ComponentModel.DataAnnotations;

namespace Niqiu.Core.Domain.Common
{
    public class Company:BaseEntity
    {
        [Display(Name = "公司名称")]
        public string Name { get; set; }
        [Display(Name = "Logo")]
        public string Logo { get; set; }

        [Display(Name = "简介")]
        public string Introduce { get; set; }

        public string Image { get; set; }

        public string QQs { get; set; }

        [Display(Name = "手机")]
        public string Mobile { get; set; }

        [Display(Name = "电话")]
        public string Phone { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }

        [Display(Name = "邮件")]
        public string Email { get; set; }

        [Display(Name = "网址")]
        public string Webset { get; set; }

        [Display(Name = "联系人")]
        public string Manager { get; set; }
    }
}