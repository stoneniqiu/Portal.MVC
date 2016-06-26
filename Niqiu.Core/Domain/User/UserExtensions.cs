using System;
using System.Linq;
using Niqiu.Core.Domain.Common;

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

       public static bool IsAdmin(this User customer, bool onlyActiveCustomerRoles = true)
       {
           return IsInCustomerRole(customer, SystemUserRoleNames.Administrators, onlyActiveCustomerRoles);
       }

       /// <summary>
       /// Gets a value indicating whether customer is guest
       /// </summary>
       /// <param name="customer">Customer</param>
       /// <param name="onlyActiveCustomerRoles">A value indicating whether we should look only in active customer roles</param>
       /// <returns>Result</returns>
       public static bool IsGuest(this User customer, bool onlyActiveCustomerRoles = true)
       {
           return IsInCustomerRole(customer, SystemUserRoleNames.Guests, onlyActiveCustomerRoles);
       }

       #region Addresses

       public static void RemoveAddress(this User customer, Address address)
       {
           if (customer.Addresses.Contains(address))
           {
             //  if (customer.BillingAddress == address) customer.BillingAddress = null;
              // if (customer.ShippingAddress == address) customer.ShippingAddress = null;

               customer.Addresses.Remove(address);
           }
       }

       #endregion
    }
}
