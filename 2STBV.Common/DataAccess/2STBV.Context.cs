using System.Data.Entity;

namespace _2STBV.Common.DataAccess
{
    public class _2STBVContext : DbContext
    {
        public _2STBVContext()
            : base("name=2STBVContext")
        {
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }       

        public virtual DbSet<UserTelegramAccount> UserTelegramAccounts { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            Database.SetInitializer<_2STBVContext>(null);

            base.OnModelCreating(modelBuilder);

        }
    }
}
