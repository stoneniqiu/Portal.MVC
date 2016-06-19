using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Niqiu.Core.Domain.Examples
{
    [Serializable]
    public class Example : BaseEntity
    {
        [Required(ErrorMessage = "请填写标题")]
        [Display(Name = "标题")]
        public string Title { get; set; }
        [Display(Name = "案例图片")]
        public string Img { get; set; }
        [Display(Name = "类型")]
        public ExampleType ExampleType { get; set; }
        [Display(Name = "摘要")]
        public string Introduce { get; set; }
        [Required(ErrorMessage = "请填写内容")]
        [Display(Name = "内容")]
        public string Content { get; set; }

        public int VisitCount { get; set; }
        public int UserId { get; set; }
        //作者
        [Display(Name = "作者")]
        public string UserName { get; set; }

        [Display(Name = "关键字")]
        public string KeyWord { get; set; }
    }

    [Flags]
    public enum ExampleType
    {
        //私募融资
        PrivatePlacement,
        //兼并收购
        Buy,
        //证券业务
        Paper

    }
}
