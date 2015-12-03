using System;
using System.Collections.Generic;
using System.Linq;
using Niqiu.Core.Domain.Common;
using Niqiu.Core.Domain.Security;
using Niqiu.Core.Domain.User;

namespace Niqiu.Core.Services
{
    /// <summary>
    /// Permission service
    /// </summary>
    public class PermissionService : IPermissionService
    {
        #region Constants
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer role ID
        /// {1} : permission system name
        /// </remarks>
        private const string PERMISSIONS_ALLOWED_KEY = "Portal.permission.allowed-{0}-{1}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string PERMISSIONS_PATTERN_KEY = "Portal.permission.";

        #endregion


        #region Fields

        private readonly IRepository<PermissionRecord> _permissionPecordRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IUserService _userService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public PermissionService(IRepository<PermissionRecord> permissionPecordRepository,
            IUserService userService, ICacheManager cacheManager,IWorkContext workContext)
        {
            _permissionPecordRepository = permissionPecordRepository;
            _cacheManager = cacheManager;
            _userService = userService;
            _workContext = workContext;
        }

        #endregion

        public void DeletePermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            _permissionPecordRepository.Delete(permission);
            //这个是干嘛的  
            _cacheManager.RemoveByPattern(PERMISSIONS_PATTERN_KEY);
        }

        public PermissionRecord GetPermissionRecordById(int permissionId)
        {
            if (permissionId == 0)
                return null;

            return _permissionPecordRepository.GetById(permissionId);
        }

        public PermissionRecord GetPermissionRecordBySystemName(string systemName)
        {
            if (String.IsNullOrWhiteSpace(systemName))
                return null;

            var query = from pr in _permissionPecordRepository.Table
                        where pr.SystemName == systemName
                        orderby pr.Id
                        select pr;

            var permissionRecord = query.FirstOrDefault();
            return permissionRecord;
        }

        public IList<PermissionRecord> GetAllPermissionRecords()
        {
            var query = from pr in _permissionPecordRepository.Table
                        orderby pr.Name
                        select pr;
            var permissions = query.ToList();
            return permissions;
        }

        public void InsertPermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            _permissionPecordRepository.Insert(permission);

            _cacheManager.RemoveByPattern(PERMISSIONS_PATTERN_KEY);
        }

        public void UpdatePermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            _permissionPecordRepository.Update(permission);

            _cacheManager.RemoveByPattern(PERMISSIONS_PATTERN_KEY);
        }

        public void InstallPermissions(IPermissionProvider permissionProvider)
        {
            //install new permissions
            var permissions = permissionProvider.GetPermissions();
            foreach (var permission in permissions)
            {
                var permission1 = GetPermissionRecordBySystemName(permission.SystemName);
                if (permission1 == null)
                {
                    //new permission (install it)
                    permission1 = new PermissionRecord
                    {
                        Name = permission.Name,
                        SystemName = permission.SystemName,
                        Category = permission.Category,
                    };

                    //这里有点不合理 难道每次都要循环一次？
                    //default customer role mappings
                    var defaultPermissions = permissionProvider.GetDefaultPermissions();
                    foreach (var defaultPermission in defaultPermissions)
                    {
                        var customerRole = _userService.GetUserRoleBySystemName(defaultPermission.UserRoleSystemName);
                        if (customerRole == null)
                        {
                            //new role (save it)
                            customerRole = new UserRole
                            {
                                Name = defaultPermission.UserRoleSystemName,
                                Active = true,
                                SystemName = defaultPermission.UserRoleSystemName
                            };
                            _userService.InsertUserRole(customerRole);
                        }


                        var defaultMappingProvided = (from p in defaultPermission.PermissionRecords
                                                      where p.SystemName == permission1.SystemName
                                                      select p).Any();
                        var mappingExists = (from p in customerRole.PermissionRecords
                                             where p.SystemName == permission1.SystemName
                                             select p).Any();
                        if (defaultMappingProvided && !mappingExists)
                        {
                            permission1.UserRoles.Add(customerRole);
                        }
                    }

                    //save new permission
                    InsertPermissionRecord(permission1);

                    //save localization 本地化
                   // permission1.SaveLocalizedPermissionName(_localizationService, _languageService);
                }
            }
        }

        public void UninstallPermissions(IPermissionProvider permissionProvider)
        {
            var permissions = permissionProvider.GetPermissions();
            foreach (var permission in permissions)
            {
                var permission1 = GetPermissionRecordBySystemName(permission.SystemName);
                if (permission1 != null)
                {
                    DeletePermissionRecord(permission1);
                    //delete permission locales
                    // permission1.DeleteLocalizedPermissionName(_localizationService, _languageService);
                }
            }
        }

        public bool Authorize(PermissionRecord permission)
        {
            return Authorize(permission, _workContext.CurrentUser);
        }

        public bool Authorize(PermissionRecord permission, User user)
        {
            if (permission == null)
                return false;

            if (user == null)
                return false;
            return Authorize(permission.SystemName, user);
        }

        public bool Authorize(string permissionRecordSystemName)
        {
            return Authorize(permissionRecordSystemName, _workContext.CurrentUser);
        }

        public bool Authorize(string permissionRecordSystemName, User user)
        {
            if (String.IsNullOrEmpty(permissionRecordSystemName))
                return false;

            var customerRoles = user.UserRoles.Where(cr => cr.Active);
            foreach (var role in customerRoles)
                if (Authorize(permissionRecordSystemName, role))
                    return true;

            //no permission found
            return false;
        }

        public bool Authorize(string permissionRecordSystemName, UserRole role)
        {
            if (String.IsNullOrEmpty(permissionRecordSystemName))
                return false;
            var key = string.Format(PERMISSIONS_ALLOWED_KEY, role.Id, permissionRecordSystemName);
            return _cacheManager.Get(key, () =>
            {
                foreach (var pr in role.PermissionRecords)
                    if (pr.SystemName.Equals(permissionRecordSystemName, StringComparison.InvariantCultureIgnoreCase))
                        return true;

                return false;
            });
        }
    }
}