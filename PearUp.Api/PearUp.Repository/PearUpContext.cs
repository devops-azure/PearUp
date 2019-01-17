using Microsoft.EntityFrameworkCore;
using PearUp.BusinessEntity;
using PearUp.LoggingFramework;
using PearUp.LoggingFramework.Models;
using UserStatus = PearUp.BusinessEntity.UserStatus;
//using PearUp.RepositoryEntity;

namespace PearUp.Repository
{
    public partial class PearUpContext : DbContext, ILoggerContext
    {
        public PearUpContext(DbContextOptions<PearUpContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Interest> Interests { get; set; }
        public DbSet<Log> Logs { get; set; }

        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Log>().ToTable("Log");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ForSqlServerUseSequenceHiLo("UserHiLo");
                entity.HasKey(e => e.Id);
                entity.OwnsOne(
                    e => e.Password,
                    e =>
                    {
                        e.Property(ep => ep.PasswordHash).UsePropertyAccessMode(PropertyAccessMode.Field).IsRequired().HasColumnName("PasswordHash");
                        e.Property(ep => ep.PasswordSalt).UsePropertyAccessMode(PropertyAccessMode.Field).IsRequired().HasColumnName("PasswordSalt");
                    });
                //entity.Property(e => e.Password.PasswordSalt).IsRequired();
                entity.Property(e => e.FullName).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field).HasMaxLength(512);
                entity.OwnsOne(
                    e => e.PhoneNumber,
                    e =>
                    {
                        e.Property(ep => ep.PhoneNumber).HasMaxLength(12).UsePropertyAccessMode(PropertyAccessMode.Field).IsRequired().HasColumnName("PhoneNumber");
                        e.Property(ep => ep.CountryCode).HasMaxLength(3).UsePropertyAccessMode(PropertyAccessMode.Field).IsRequired().HasColumnName("CountryCode");
                    });
                //entity.HasIndex(e => e.PhoneNumber).IsUnique();
                entity.OwnsOne(
                    e => e.Age,
                    e =>
                    {
                        e.Property(ep => ep.DateOfBirth).UsePropertyAccessMode(PropertyAccessMode.Field).IsRequired().HasColumnName("DateOfBirth");
                    });
                entity.Property(e => e.Profession)
                    .HasMaxLength(100);
                entity.OwnsOne(
                    e => e.Gender,
                    e => e.Property(ep => ep.GenderType).IsRequired().HasColumnType("int").HasColumnName("Gender"));
                entity.OwnsOne(
                    e => e.MatchPreference,
                    e =>
                    {
                        e.OwnsOne(ep => ep.LookingFor, ep => ep.Property(epp => epp.GenderType).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnType("int").HasColumnName("LookingFor"));
                        e.Property(ep => ep.Distance).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("Distance");
                        e.Property(ep => ep.MaxAge).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("MaxAge");
                        e.Property(ep => ep.MinAge).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("MinAge");
                    });
                entity.OwnsOne(
                    e => e.Location,
                    e =>
                    {
                        e.Property(ep => ep.Latitude).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnType("float").HasColumnName("Latitude");
                        e.Property(ep => ep.Longitude).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnType("float").HasColumnName("Longitude");
                    });
                entity.Property(e => e.BucketList).UsePropertyAccessMode(PropertyAccessMode.Field).UsePropertyAccessMode(PropertyAccessMode.Field).HasMaxLength(140);
                entity.Property(e => e.FunAndInterestingThings).UsePropertyAccessMode(PropertyAccessMode.Field).HasMaxLength(140);
                entity.Property(e => e.School).UsePropertyAccessMode(PropertyAccessMode.Field).HasMaxLength(100);
            });


            modelBuilder.Entity<Interest>(entity =>
            {
                entity.Property(e => e.Id).ForSqlServerUseSequenceHiLo("InterestsHiLo");
                entity.Property(e => e.InterestDescription).HasMaxLength(512);
                entity.Property(e => e.InterestName).HasMaxLength(512);

            });


            modelBuilder.Entity<UserInterest>().HasKey(i => new { i.UserId, i.InterestId });

            modelBuilder.Entity<UserPhoto>().HasKey(p => new { p.UserId,p.Order });

            modelBuilder.Entity<Admin>().ToTable("Admin");
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.Property(e => e.Id).ForSqlServerUseSequenceHiLo("AdminHiLo");
                entity.HasKey(e => e.Id);
                entity.OwnsOne(
                    e => e.Password,
                    e =>
                    {
                        e.Property(ep => ep.PasswordHash).UsePropertyAccessMode(PropertyAccessMode.Field).IsRequired().HasColumnName("PasswordHash");
                        e.Property(ep => ep.PasswordSalt).UsePropertyAccessMode(PropertyAccessMode.Field).IsRequired().HasColumnName("PasswordSalt");
                    });
                entity.Property(e => e.FullName).IsRequired().UsePropertyAccessMode(PropertyAccessMode.Field).HasMaxLength(512);
                entity.OwnsOne(
                    e => e.Email,
                    e => e.Property(em => em.EmailId).UsePropertyAccessMode(PropertyAccessMode.Field).IsRequired().HasColumnName("Email")
            );
            });
        }
    }
}