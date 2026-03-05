using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.IO;
using WinUITestProject.Infrastructure.Data;

public class DesignTimeDbContextFactory
    : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();

        var appDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "TabSidebarDemo"
        );

        Directory.CreateDirectory(appDir);

        var dbPath = Path.Combine(appDir, "notes.db");

        builder.UseSqlite($"Data Source={dbPath}");

        return new AppDbContext(builder.Options);
    }
}