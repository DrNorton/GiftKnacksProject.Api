using System.Data.Entity;

namespace GiftKnacksProject.Api.EfDao
{
    public class EfContext : DbContext
    {
        public EfContext()
            : base("name=giftKnacksConnectionString")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profile>().HasRequired(a => a.User).WithRequiredDependent(b => b.Profile);
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }
    }
}
