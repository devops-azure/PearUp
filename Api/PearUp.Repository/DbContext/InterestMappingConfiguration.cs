//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using PearUp.RepositoryEntity;

//namespace PearUp.Repository
//{
//    public class InterestMappingConfiguration : IEntityTypeConfiguration<InterestMapping>
//    {
//        public void Configure(EntityTypeBuilder<InterestMapping> builder)
//        {
//            builder.ToTable("InterestMapping");
//            builder.HasOne(d => d.Interest)
//                    .WithMany(p => p.InterestMapping)
//                    .HasForeignKey(d => d.InterestId)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK__InterestM__Inter__6D0D32F4");

//            builder.HasOne(d => d.User)
//                    .WithMany(p => p.InterestMapping)
//                    .HasForeignKey(d => d.UserId)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK__InterestM__UserI__6C190EBB");
//        }
//    }
//}
