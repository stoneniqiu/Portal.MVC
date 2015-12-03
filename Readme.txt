1.这个小框架是MVC4+NInject+EF+Matrix_Admin（bootstarp）包含 用户，角色，权限三个功能。是模拟的NOP的架构。原本的NOP太大了。我搞了个Min版本。
2.Excel导入用的NPIO
3.基本模型、常用对象、帮助类、数据映射、Service都在Niqiu.Core里面。

权限的初始化需要调用下面的方法。 默认数据库已经包含了 系统管理员，管理员和 操作员三个角色。
权限验证通过标签来处理。
//_permissionService.InstallPermissions(new StandardPermissionProvider());