using DatingApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Infrastructure.Database.Configs
{
    public class LikeMap : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.ToTable("like");

            builder.HasKey(k => new { k.SenderId, k.ReceiverId })
                .HasName("uk_like");

            builder.Property(p => p.SenderId)
                .HasColumnName("sender_id")
                .IsRequired();
            builder.HasOne(e => e.Sender)
                .WithMany(e => e.LikesReceived)
                .HasForeignKey(e => e.SenderId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_like_user_sender");

            builder.Property(p => p.ReceiverId)
                .HasColumnName("receiver_id")
                .IsRequired();
            builder.HasOne(e => e.Receiver)
                .WithMany(e => e.LikesSent)
                .HasForeignKey(e => e.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_like_user_receiver");
        }
    }
}
