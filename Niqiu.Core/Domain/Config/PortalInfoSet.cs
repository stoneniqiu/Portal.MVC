using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Niqiu.Core.Domain.Config
{
    public class PortalInfoSet : BaseEntity
    {
        [Display(Name = "站点名称")]
        [Required(ErrorMessage = "请填写名称")]
        public string Name { get; set; }

        [Display(Name = "简介")]
        [Required(ErrorMessage = "请填写简介")]
        public string Brife { get; set; }

        [Display(Name = "Logo")]
        [Required(ErrorMessage = "请上传logo")]
        public string Logo { get; set; }

    }

    public class SliderImg : BaseEntity
    {
        [Display(Name = "图片")]
        [Required(ErrorMessage = "请上传图片")]
        public string Img { get; set; }

        [Display(Name = "跳转Url")]
        public string Url { get; set; }

        [Display(Name = "缩略图")]
        public string Thumbnail { get; set; }

        [Display(Name = "描述")]
        public string Description { get; set; }

    }

    
}
