using System;
using DatingApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.Infrastructure.Database.Configs
{
    public class MessageMap : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("message");

            builder.HasKey(p => p.Id)
                .HasName("pk_message");
            builder.Property(p => p.Id)
                .HasColumnName("id");

            builder.Property(p => p.SenderId)
                .HasColumnName("sender_id")
                .IsRequired();
            builder.HasOne(e => e.Sender)
                .WithMany(e => e.MessagesSent)
                .HasForeignKey(e => e.SenderId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_message_user_sender");

            builder.Property(p => p.RecipientId)
                .HasColumnName("recipient_id")
                .IsRequired();
            builder.HasOne(e => e.Recipient)
                .WithMany(e => e.MessagesReceived)
                .HasForeignKey(e => e.RecipientId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_message_user_recipient");

            builder.Property(p => p.Content)
                .HasColumnName("content")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(p => p.IsRead)
                .HasColumnName("is_read")
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(p => p.DateRead)
                .HasColumnName("read_at");

            builder.Property(p => p.MessageSent)
                .HasColumnName("sent_at")
                .HasDefaultValue(DateTimeOffset.UtcNow)
                .IsRequired();

            builder.Property(p => p.SenderDeleted)
                .HasColumnName("sender_deleted")
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(p => p.RecipientDeleted)
                .HasColumnName("recipient_deleted")
                .HasDefaultValue(false)
                .IsRequired();
        }
    }
}
