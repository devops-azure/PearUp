//using Microsoft.EntityFrameworkCore;
//using UserStatus = PearUp.BusinessEntity.UserStatus;
//using PearUp.RepositoryEntity;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace PearUp.Repository
//{
//    public class UserConfiguration : IEntityTypeConfiguration<User>
//    {
//        public void Configure(EntityTypeBuilder<User> builder)
//        {
//            builder.ToTable("User");
//            builder.HasKey(e => e.Id);
//            builder.Property(e => e.Email).HasMaxLength(512);
//            builder.Property(e => e.PasswordHash).IsRequired();
//            builder.Property(e => e.PasswordSalt).IsRequired();
//            builder.Property(e => e.FullName).HasMaxLength(512).IsRequired();
//            builder.Property(e => e.PhoneNumber).HasMaxLength(512).IsRequired();
//            builder.HasIndex(e => e.PhoneNumber).IsUnique();
//            builder.Property(e => e.DateOfBirth).HasColumnType("datetime");
//            builder.Property(e => e.Profession)
//                .IsRequired()
//                .HasMaxLength(100);
//            builder.Property(e => e.School).HasMaxLength(100);
//            builder.Property(e => e.Status).HasDefaultValue(UserStatus.NotVerified);
//            builder.Property(e => e.OnboardingStep).IsRequired(false);

//        }
//    }
//}
