//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using PearUp.RepositoryEntity;

//namespace PearUp.Repository
//{
//    public class InterestConfiguration: IEntityTypeConfiguration<Interest>
//    {
//        public void Configure(EntityTypeBuilder<Interest> builder)
//        {
//            builder.ToTable("Interest");
//            builder.Property(e => e.InterestDescription).HasMaxLength(512);
//            builder.Property(e => e.InterestName).HasMaxLength(512);
//        }
//    }
//}
