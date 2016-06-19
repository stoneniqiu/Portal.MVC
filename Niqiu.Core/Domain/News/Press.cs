using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Niqiu.Core.Domain.News
{
    [Serializable]
    public class Press : BaseEntity
    {
        [Required(ErrorMessage = "请填写标题")]
        [Display(Name = "标题")]
        public string Title { get; set; }
        [Display(Name = "简介")]
        public string Introduce { get; set; }
        [Required(ErrorMessage = "请填写内容")]
        [Display(Name = "内容")]
        public string Content { get; set; }
        [Display(Name = "类型")]
        public NewsType NewsType { get; set; }
        public int VisitCount { get; set; }
        public int UserId { get; set; }
        //作者
        [Display(Name = "作者")]
        public string UserName { get; set; }

        [Display(Name = "新闻图片")]
        public string ImgUrl { get; set; }

        [Display(Name = "来源")]
        public string Source { get; set; }
    }

    [Flags]
    public enum NewsType
    {
        //公司新闻
        Company,
        //所获获奖
        Reward,
        //媒体报道
        Media,
        //公司活动
        Activity
    }
}
