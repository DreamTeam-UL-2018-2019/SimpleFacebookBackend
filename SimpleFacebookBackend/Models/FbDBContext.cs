using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SimpleFacebookBackend.Models
{
    public partial class FbDBContext : DbContext
    {
        public FbDBContext()
        {
        }

        public FbDBContext(DbContextOptions<FbDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Friend> Friend { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Unread> Unread { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserGroup> UserGroup { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=FbDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friend>(entity =>
            {
                entity.Property(e => e.IdUser1).HasColumnName("Id_user1");

                entity.Property(e => e.IdUser2).HasColumnName("Id_user2");

                entity.HasOne(d => d.IdUser1Navigation)
                    .WithMany(p => p.FriendIdUser1Navigation)
                    .HasForeignKey(d => d.IdUser1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Friend_User1");

                entity.HasOne(d => d.IdUser2Navigation)
                    .WithMany(p => p.FriendIdUser2Navigation)
                    .HasForeignKey(d => d.IdUser2)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Friend_User2");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.File).IsUnicode(false);

                entity.Property(e => e.IdGroup).HasColumnName("Id_group");

                entity.Property(e => e.IdReceiver).HasColumnName("Id_receiver");

                entity.Property(e => e.IdSender).HasColumnName("Id_sender");

                entity.Property(e => e.Message1)
                    .HasColumnName("Message")
                    .IsUnicode(false);

                entity.HasOne(d => d.IdGroupNavigation)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.IdGroup)
                    .HasConstraintName("FK_Message_Group");

                entity.HasOne(d => d.IdReceiverNavigation)
                    .WithMany(p => p.MessageIdReceiverNavigation)
                    .HasForeignKey(d => d.IdReceiver)
                    .HasConstraintName("FK_Message_User2");

                entity.HasOne(d => d.IdSenderNavigation)
                    .WithMany(p => p.MessageIdSenderNavigation)
                    .HasForeignKey(d => d.IdSender)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Message_User1");
            });

            modelBuilder.Entity<Unread>(entity =>
            {
                entity.Property(e => e.IdMessage).HasColumnName("Id_message");

                entity.Property(e => e.IdUser).HasColumnName("Id_user");

                entity.HasOne(d => d.IdMessageNavigation)
                    .WithMany(p => p.Unread)
                    .HasForeignKey(d => d.IdMessage)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Unread_Message");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Unread)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Unread_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("FIrst_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Image).IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("Last_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.ToTable("User_group");

                entity.Property(e => e.IdGroup).HasColumnName("Id_group");

                entity.Property(e => e.IdUser).HasColumnName("Id_user");

                entity.HasOne(d => d.IdGroupNavigation)
                    .WithMany(p => p.UserGroup)
                    .HasForeignKey(d => d.IdGroup)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_group_group");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.UserGroup)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_group_user");
            });
        }
    }
}
