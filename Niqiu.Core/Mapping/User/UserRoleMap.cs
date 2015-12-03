namespace Niqiu.Core.Mapping.User
{
    public class UserRoleMap : PortalEntityTypeConfiguration<Niqiu.Core.Domain.User.UserRole>
    {
        public UserRoleMap()
        {
            ToTable("UserRoles");
            HasKey(cr => cr.Id);
            //HasKey(c => c.SystemName);
            Property(cr => cr.Name).IsRequired().HasMaxLength(255);
            Property(cr => cr.SystemName).HasMaxLength(255);
        }
    }
}
