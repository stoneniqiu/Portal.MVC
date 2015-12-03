using Niqiu.Core.Domain.Security;

namespace Niqiu.Core.Mapping.Security
{
  public  class PermissionRecordMap : PortalEntityTypeConfiguration<PermissionRecord>
    {

    public  PermissionRecordMap()
      {
          HasKey(pr => pr.Id);
          Property(p => p.Name).IsRequired();
          Property(p => p.SystemName).IsRequired().HasMaxLength(255);
          Property(p => p.Category).IsRequired().HasMaxLength(255);
          HasMany(pr => pr.UserRoles).WithMany(c => c.PermissionRecords) 
           .Map(m => m.ToTable("PermissionRecord_Role_Mapping"));
      }
    }
}
