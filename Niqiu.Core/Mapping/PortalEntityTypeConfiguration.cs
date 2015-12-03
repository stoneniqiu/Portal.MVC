using System.Data.Entity.ModelConfiguration;

namespace Niqiu.Core.Mapping
{
    public abstract class PortalEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : class
    {
        protected PortalEntityTypeConfiguration()
        {
            PostInitialize();
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        private void PostInitialize()
        {
            
        }
    }
}