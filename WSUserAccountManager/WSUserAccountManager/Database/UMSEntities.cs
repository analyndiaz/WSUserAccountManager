using Microsoft.EntityFrameworkCore;
using WSUserAccountManager.Database.Entities;

namespace WSUserAccountManager.Database
{
    public class UMSEntities : DbContext
    {
        public UMSEntities(DbContextOptions options)
             : base(options)
        {
        }

        public DbSet<UserAccount> UserAccounts { get; set; }

        public DbSet<Password> Passwords { get; set; }

        public DbSet<VerificationCode> VerfificationCodes { get; set; }

        public DbSet<Salt> Salts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserAccount>().ToTable("UserAccount");
            modelBuilder.Entity<UserAccount>().HasKey(u => u.UserAccountId);
            modelBuilder.Entity<UserAccount>()
                .HasMany(u => u.Passwords)
                .WithOne(p => p.UserAccount);
            modelBuilder.Entity<UserAccount>()
                .HasMany(u => u.VerificationCodes)
                .WithOne(v => v.UserAccount);
            modelBuilder.Entity<UserAccount>()
                .HasMany(u => u.Salts)
                .WithOne(v => v.UserAccount);
            modelBuilder.Entity<UserAccount>()
                .HasMany(u => u.Sessions)
                .WithOne(v => v.UserAccount);

            modelBuilder.Entity<Password>().ToTable("Password");
            modelBuilder.Entity<Password>().HasKey(p => p.PasswordId);

            modelBuilder.Entity<VerificationCode>().ToTable("VerificationCode");
            modelBuilder.Entity<VerificationCode>().HasKey(v => v.VerificationCodeId);

            modelBuilder.Entity<Salt>().ToTable("Salt");
            modelBuilder.Entity<Salt>().HasKey(s => s.SaltId);

            modelBuilder.Entity<Session>().ToTable("Session");
            modelBuilder.Entity<Session>().HasKey(s => s.SessionId);
        }
    }
}
