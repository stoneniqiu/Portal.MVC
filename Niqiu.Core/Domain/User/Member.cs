using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Niqiu.Core.Domain.User
{
    [Serializable]
    public class Member : BaseEntity
    {
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "姓名不能为空")]
        public string Name { get; set; }
        
        [Display(Name = "职位1")]
        [Required(ErrorMessage = "职位不能为空")]
        public string Position { get; set; }

        [Display(Name = "职位2")]
        public string Position1 { get; set; }

        [Display(Name = "职位3")]
        public string Position2 { get; set; }

        [Display(Name = "头像")]
        [Required(ErrorMessage = "请上传头像")]
        public string Img { get; set; }

        [Display(Name = "半身像")]
        [Required(ErrorMessage = "请上传半身像")]
        public string BodyImg { get; set; }


        [Display(Name = "是否选中")]
        public bool Selected { get; set; }

        [Display(Name = "个人简介")]
        [Required(ErrorMessage = "请填写个人简介")]
        public string Info { get; set; }

        [Display(Name = "所属团队")]
        public string Team { get; set; }

        public int TeamId { get; set; }
    }


    public class Team : BaseEntity
    {
        [Display(Name = "团队名称")]
        [Required(ErrorMessage = "名称不能为空")]
        public string Name { get; set; }

        [Display(Name = "团队描述")]
        public string Description { get; set; }

        [Display(Name = "团队负责人")]
        public virtual  User User { get; set; }

        [Display(Name = "团队图片")]
        [Required(ErrorMessage = "请上传团队图片")]
        public string Img { get; set; }
    }
}
