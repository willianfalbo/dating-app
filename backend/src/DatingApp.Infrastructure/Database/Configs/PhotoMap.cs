using DatingApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Infrastructure.Database.Configs
{
    public class PhotoMap : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.HasQueryFilter(p => p.IsApproved);

            builder.ToTable("photo");

            builder.HasKey(p => p.Id)
                .HasName("pk_photo");
            builder.Property(p => p.Id)
                .HasColumnName("id");

            builder.Property(p => p.Url)
                .HasColumnName("url")
                .IsRequired();

            builder.Property(p => p.Description)
                .HasColumnName("description")
                .HasMaxLength(100);

            builder.Property(p => p.DateAdded)
                .HasColumnName("added_at")
                .IsRequired();

            builder.Property(p => p.IsMain)
                .HasColumnName("is_main")
                .IsRequired();

            builder.Property(p => p.PublicId)
                .HasColumnName("public_id");

            builder.Property(p => p.UserId)
                .HasColumnName("user_id")
                .IsRequired();
            builder.HasOne(e => e.User)
                .WithMany(e => e.Photos)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_photo_user");

            builder.Property(p => p.IsApproved)
                .HasColumnName("is_approved")
                .HasDefaultValue(false)
                .IsRequired();
        }
    }
}
