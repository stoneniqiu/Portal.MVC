using System;
using System.Linq;

namespace Niqiu.Core.Domain.User
{
   public static class UserExtensions
    {
        /// <summary>
        /// Gets a value indicating whether customer is registered
        /// </summary>
        /// <param name="user">Customer</param>
        /// <param name="onlyActiveCustomerRoles">A value indicating whether we should look only in active customer roles</param>
        /// <returns>Result</returns>
        public static bool IsRegistered(this User user, bool onlyActiveCustomerRoles = true)
        {
            return IsInCustomerRole(user, SystemUserRoleNames.Registered, onlyActiveCustomerRoles);
        }

       public static string RoleName(this User user)
       {
           return user.UserRoles.Aggregate("", (current, role) => current + (role.Name + " "));
       }

       public static bool IsInCustomerRole(this User user, string userRoleSystemName, bool onlyActiveCustomerRoles = true)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (String.IsNullOrEmpty(userRoleSystemName))
                throw new ArgumentNullException("userRoleSystemName");

            var result = user.UserRoles
                .FirstOrDefault(cr => (!onlyActiveCustomerRoles || cr.Active) && (cr.SystemName == userRoleSystemName)) != null;
            return result;
        }
    }
}
