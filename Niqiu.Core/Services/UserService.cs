using System;
using System.Collections.Generic;
using System.Linq;
using Niqiu.Core.Domain;
using Niqiu.Core.Domain.Common;
using Niqiu.Core.Domain.User;

namespace Niqiu.Core.Services
{
   public class UserService:IUserService
   {
       private readonly IRepository<User> _useRepository;
       private readonly IRepository<UserRole> _userRoleRepository;
       private readonly IRepository<Address> _addressRepository;
 
       private readonly  ICacheManager _cacheManager ;

       public UserService(IRepository<User> useRepository,IRepository<UserRole> userRoleRepository,
           IRepository<Address> addressRepository,
           ICacheManager cacheManager)
       {
           _useRepository = useRepository;
           _userRoleRepository = userRoleRepository;
           _addressRepository = addressRepository;
           _cacheManager = cacheManager;
       }


       public void DeleteUser(User user)
       {
         if(user==null) throw new ArgumentNullException("user");
         if(user.IsSystemAccount) throw new PortalException(string.Format("系统用户{0}不能删除",user.SystemName));

           user.Deleted = true;
         if (!String.IsNullOrEmpty(user.Email))
             user.Email += "-DELETED";
         if (!String.IsNullOrEmpty(user.Username))
             user.Username += "-DELETED";
            
           UpdateUser(user);
       }

       public User GetUserById(int userId)
       {
           if (userId == 0)
               return null;

           return _useRepository.GetById(userId);
       }

       public void InsertUser(User user)
       {
           if (user == null) throw new ArgumentNullException("user");

          _useRepository.Insert(user);
          //event notification
          //_eventPublisher.EntityInserted(customer);
       }

       public void UpdateUser(User user)
       {
           if (user == null)
               throw new ArgumentNullException("user");

           _useRepository.Update(user);

           //还触发了事件通知！
           //_eventPublisher.EntityUpdated(customer);
       }

       public User GetUserByEmail(string email)
       {
           if (string.IsNullOrWhiteSpace(email)) return null;

           var query = from c in _useRepository.Table
               orderby c.Id
               where c.Email == email
               select c;

           var user = query.FirstOrDefault();
           return user;
       }

       public IList<User> GetUsersByIds(int[] userIds)
       {
           if(userIds==null||userIds.Length==0)
               return new List<User>();

           var quey = _useRepository.Table.Where(n => userIds.Contains(n.Id));
           var users = quey.ToList();
           return userIds.Select(id => users.Find(x => x.Id == id)).Where(user => user != null).ToList();
       }

       public User GetUserByGuid(Guid userGuid)
       {
           return userGuid == Guid.Empty ? null :
               _useRepository.Table.FirstOrDefault(n => n.UserGuid == userGuid);
       }

       public User GetUserBySystemName(string systemName)
       {
           return string.IsNullOrWhiteSpace(systemName) ? null : _useRepository.Table.FirstOrDefault(n => n.SystemName == systemName);
       }

       public User GetUserByOpenId(string openId)
       {
           return string.IsNullOrWhiteSpace(openId) ? null : _useRepository.Table.FirstOrDefault(n => n.OpenId == openId);
       }

       public User GetUserByUsername(string username)
       {
           return string.IsNullOrWhiteSpace(username) ? null : _useRepository.Table.FirstOrDefault(n => n.Username == username);
       }


       public IPagedList<User> GetAllUsers(string email = null, string username = null, int pageIndex = 0, int pageSize = 2147483647)
       {
           var query = _useRepository.Table.Where(n=>!n.Deleted);
           if (!String.IsNullOrWhiteSpace(email))
               query = query.Where(c => c.Email.Contains(email));
           if (!String.IsNullOrWhiteSpace(username))
               query = query.Where(c => c.Email.Contains(username));
           query = query.OrderByDescending(c => c.CreateTime);
           var users = new PagedList<User>(query, pageIndex, pageSize);
           return users;
       }
      

       public IPagedList<User> GetOnlineUsers(DateTime lastActivityFromUtc, int[] userRoleIds, int pageIndex, int pageSize)
       {
           var query = _useRepository.Table;
           query = query.Where(c => lastActivityFromUtc <= c.LastActivityDateUtc);
           query = query.Where(c => !c.Deleted);
            if (userRoleIds != null && userRoleIds.Length > 0)
                query = query.Where(c => c.UserRoles.Select(cr => cr.Id).Intersect(userRoleIds).Any());

           var users = new PagedList<User>(query, pageIndex, pageSize);
           return users;
       }

       public virtual User InsertGuestUser()
       {
           var customer = new User
           {
               UserGuid = Guid.NewGuid(),
               Active = true,
               LastActivityDateUtc = DateTime.Now,
           };

           //add to 'Guests' role
           var guestRole = GetUserRoleBySystemName(SystemUserRoleNames.Guests);
           if (guestRole == null)
               throw new PortalException("'Guests' role could not be loaded");
           customer.UserRoles.Add(guestRole);

           _useRepository.Insert(customer);
  
           return customer;
       }
        

       private const string UserrolesBySystemnameKey = "Portal.userrole.systemname-{0}";
       private const string UserrroleAllKey = "Portal.userrrole.all-{0}";
       private const string UserrrolePatternKey = "Portal.userrrole.";

       #region Role

       public virtual UserRole GetUserRoleBySystemName(string systemName)
       {
           if (String.IsNullOrWhiteSpace(systemName))
               return null;
           var key = string.Format(UserrolesBySystemnameKey, systemName);
           return _cacheManager.Get(key, () =>
           {
               var query = from cr in _userRoleRepository.Table
                   orderby cr.Id
                   where cr.SystemName == systemName
                   select cr;
               var customerRole = query.FirstOrDefault();
               return customerRole;
           });
       }

       public void DeleteUserRole(UserRole role)
       {
           if (role == null)
               throw new ArgumentNullException("role");

           if (role.IsSystemRole)
               throw new PortalException("系统用户不能删除");

           _userRoleRepository.Delete(role);

          // _cacheManager.RemoveByPattern(UserrolesBySystemnameKey);

         //  _eventPublisher.EntityDeleted(customerRole);

       }

       public UserRole GetUserRoleById(int userRoleId)
       {
           if (userRoleId == 0)
               return null;

           return _userRoleRepository.GetById(userRoleId);
       }

       /// <summary>
       /// 缓存查询结果！
       /// </summary>
       /// <param name="showHidden"></param>
       /// <returns></returns>
       public IList<UserRole> GetAllUserRoles(bool showHidden = false)
       {
           var query = from cr in _userRoleRepository.Table
                       orderby cr.Name
                       where (showHidden || cr.Active)
                       select cr;
         return query.ToList();
       }

       public void InsertUserRole(UserRole userRole)
       {
           if (userRole == null)
               throw new ArgumentNullException("userRole");

           _userRoleRepository.Insert(userRole);

           //event notification
           //_eventPublisher.EntityInserted(userRole);
       }

       public void UpdateUserRole(UserRole userRole)
       {
           if (userRole == null)
               throw new ArgumentNullException("userRole");

           _userRoleRepository.Update(userRole);

           //event notification
          // _eventPublisher.EntityUpdated(customerRole);
       }


       #endregion 
   }
}
