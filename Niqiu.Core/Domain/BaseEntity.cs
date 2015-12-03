using System;
using System.ComponentModel.DataAnnotations;

namespace Niqiu.Core.Domain
{
    public abstract class BaseEntity
    {

        [Key]
        public int Id { get; set; }
        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }
        [Display(Name = "更新时间")]
        public DateTime ModifyTime { get; set; }

        protected BaseEntity()
        {
            CreateTime = DateTime.Now;
            ModifyTime = DateTime.Now;
        }
    }
}
