using System.Collections.Generic;
using Niqiu.Core.Domain.User;

namespace Niqiu.Core.Domain.Security
{
    public class PermissionRecord : BaseEntity
    {
        private ICollection<UserRole> _customerRoles;

        /// <summary>
        /// Gets or sets the permission name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the permission system name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the permission category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets discount usage history
        /// </summary>
        public virtual ICollection<UserRole> UserRoles
        {
            get { return _customerRoles ?? (_customerRoles = new List<UserRole>()); }
            protected set { _customerRoles = value; }
        }   
    }
}
