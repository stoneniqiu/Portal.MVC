// ***********************************************************************
// Assembly         : Portal.Services
// Author           : RJ-Stone
// Created          : 01-28-2015
//
// Last Modified By : RJ-Stone
// Last Modified On : 02-09-2015
// ***********************************************************************
// <copyright file="IUserService.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Niqiu.Core.Domain;
using Niqiu.Core.Domain.User;

namespace Niqiu.Core.Services
{
    /// <summary>
    /// Interface IUserService
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="user">The user.</param>
        void DeleteUser(User user);
        /// <summary>
        /// Gets the user by id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>User.</returns>
        User GetUserById(int userId);
        /// <summary>
        /// Inserts the user.
        /// </summary>
        /// <param name="user">The user.</param>
        void InsertUser(User user);
        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        void UpdateUser(User user);
        /// <summary>
        /// Gets the user by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>User.</returns>
        User GetUserByEmail(string email);
        /// <summary>
        /// Get Users by identifiers
        /// </summary>
        /// <param name="userIds">User identifiers</param>
        /// <returns>Users</returns>
        IList<User> GetUsersByIds(int[] userIds);

        /// <summary>
        /// Gets a User by GUID
        /// </summary>
        /// <param name="userGuid">User GUID</param>
        /// <returns>A User</returns>
        User GetUserByGuid(Guid userGuid);

        /// <summary>
        /// Get User by system role
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>User</returns>
        User GetUserBySystemName(string systemName);
        User GetUserByOpenId(string openId);

        /// <summary>
        /// Get User by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User</returns>
        User GetUserByUsername(string username);
        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="username">The username.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>IPagedList{User}.</returns>
        IPagedList<User> GetAllUsers(string email = null, string username = null, int pageIndex = 0, int pageSize = 2147483647);
        /// <summary>
        /// Gets the online users.
        /// </summary>
        /// <param name="lastActivityFromUtc">The last activity from UTC.</param>
        /// <param name="userRoleIds">The user role ids.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>IPagedList{User}.</returns>
        IPagedList<User> GetOnlineUsers(DateTime lastActivityFromUtc,
          int[] userRoleIds, int pageIndex, int pageSize);

        #region Roles

        UserRole GetUserRoleBySystemName(string systemName);

        void DeleteUserRole(UserRole role);

        UserRole GetUserRoleById(int userRoleId);
        
        IList<UserRole> GetAllUserRoles(bool showHidden = false);
        /// <summary>
        /// Inserts a customer role
        /// </summary>
        /// <param name="userRole">Customer role</param>
        void InsertUserRole(UserRole userRole);

        /// <summary>
        /// Updates the customer role
        /// </summary>
        /// <param name="userRole">Customer role</param>
        void UpdateUserRole(UserRole userRole);


        User InsertGuestUser();

        #endregion
    }
}


