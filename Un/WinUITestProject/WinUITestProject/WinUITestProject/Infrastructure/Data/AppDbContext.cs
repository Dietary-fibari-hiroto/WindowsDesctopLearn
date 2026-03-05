using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinUITestProject.Core.Entities;
using WinUITestProject.Shared.Extensions;


namespace WinUITestProject.Infrastructure.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Note> Notes => Set<Note>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<Folder> Folders => Set<Folder>();
        public DbSet<NoteTag> NoteTags => Set<NoteTag>();
        public DbSet<FolderNote> FolderNotes => Set<FolderNote>();


        //マイグレートや定義に必要
        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            foreach (var entity in modelBuilder.Model.GetEntityTypes()) {

                var tablename = entity.ClrType.Name;

                var snake = tablename.ToSnakeCase();
                var plural = snake.ToPluralize();

                entity.SetTableName(plural);

                foreach (var property in entity.GetProperties()) {
                    property.SetColumnName(property.Name.ToSnakeCase());
                }
            }

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Note>(builder =>
            {
                builder.Property(n => n.Id)
                .ValueGeneratedOnAdd()
                .HasConversion(
                    id => id.Value,
                    value => new NoteId(value)
                    );
                builder.Property(n => n.Title)
                .HasMaxLength(255);
                builder.Property(n => n.IsPinned)
                .HasDefaultValue(false);
            });

            modelBuilder.Entity<Tag>(builder =>
            {
                builder.Property(t => t.Name)
                .HasMaxLength(50);
                builder.HasIndex(t => t.Name)
                .IsUnique();
            });

            modelBuilder.Entity<NoteTag>(builder =>
            {
                builder.HasKey(nt => new { nt.TagId, nt.NoteId });

                builder.HasOne(nt => nt.Note)
                .WithMany(n => n.NoteTags)
                .HasForeignKey(nt => nt.NoteId)
                .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(nt => nt.Tag)
                .WithMany(t => t.NoteTags)
                .HasForeignKey(nt => nt.TagId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Folder>(builder =>
            {
                builder.Property(f => f.Name)
                .HasMaxLength(255);
                builder.HasIndex(f => f.Name)
                .IsUnique();
            });

            modelBuilder.Entity<FolderNote>(builder =>
            {
                builder.HasKey(fn => new { fn.FolderId, fn.NoteId });

                builder.HasOne(fn => fn.Folder)
                .WithMany(f => f.FolderNotes)
                .HasForeignKey(fn => fn.FolderId)
                .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(fn => fn.Note)
                .WithMany(n => n.FolderNotes)
                .HasForeignKey(fn => fn.NoteId)
                .OnDelete(DeleteBehavior.Cascade);
            });


        }



        //DBに書き換えるときの処理
        //作成時間と更新時間をEFCoreで更新してるよ
        //どうやらSQLiteには更新する機能がないみたい
        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries<TimestampedEntity>();
            var now = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                }
            }
        }


    }
}
