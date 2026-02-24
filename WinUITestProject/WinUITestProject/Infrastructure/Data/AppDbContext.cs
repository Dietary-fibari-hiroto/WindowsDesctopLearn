using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinUITestProject.Infrastructure.Entities;
using WinUITestProject.Shared.Extensions;


namespace WinUITestProject.Infrastructure.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Note> Notes => Set<Note>();
        public DbSet<Folder> Foldrs => Set<Folder>();

        protected override void OnConfiguring(DbContextOptionsBuilder opt) {
            
            var appDir = Path.Combine(//Combineで安全なパス結合
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),//Localフォルダのパスをとってる
                "TabSidebarDemo"
                );
            Directory.CreateDirectory(appDir);//directoryがなかったら作る

            var dbPath = Path.Combine(appDir, "notes.db");
            opt.UseSqlite($"Data Source{dbPath}");


            opt.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        }

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

            modelBuilder.Entity<NoteTag>(builder =>
            {
                builder.HasKey(nt => new { nt.TagId, nt.NoteId });





            });
            modelBuilder.Entity<FolderNote>().HasKey(p => new { p.FolderId, p.NoteId });

        }



        //DBに書き換えるときの処理
        //作成時間と更新時間をEFCoreで更新してるよ
        //どうやらSQLiteには更新する機能がないみたい
        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<TimestampedEntity>();
            UpdateTimestamps();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries<TimestampedEntity>();
            var now = DateTime.UtcNow;

            foreach (var entry in entries) {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                }
            }
        }

 
    }
}
