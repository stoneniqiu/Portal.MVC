using System.ComponentModel.DataAnnotations;

namespace Niqiu.Core.Domain.Common
{
    public class Company:BaseEntity
    {
        [Display(Name = "公司名称")]
        [Required(ErrorMessage = "请输入公司名称")]
        public string Name { get; set; }
        [Display(Name = "Logo")]
        public string Logo { get; set; }

        [Display(Name = "简介")]
        public string Introduce { get; set; }

        [Display(Name = "公司照片")]
        public string Img { get; set; }

        [Display(Name = "QQ")]
        public string QQs { get; set; }

        [Display(Name = "手机")]
        public string Mobile { get; set; }

        [Display(Name = "电话")]
        [Required(ErrorMessage = "请输入公司电话")]
        public string Phone { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }

        [Display(Name = "邮箱")]
        [Required(ErrorMessage = "请输入公司邮箱")]
        public string Email { get; set; }

        [Display(Name = "传真")]
        public string Fax { get; set; }

        [Display(Name = "邮编")]
        public string ZipCode { get; set; }


        [Display(Name = "网址")]
        public string Webset { get; set; }

        [Display(Name = "联系人")]
        public string Manager { get; set; }
    }
}