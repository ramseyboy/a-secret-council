using ASecretCouncil.Model.Application;
using ASecretCouncil.Model.Person;
using ASecretCouncil.Model.Resume;

namespace ASecretCouncil.Model.Data;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class ApplicationContext : DbContext
{
    public DbSet<ApplicationEntity> Application { get; set; }
    public DbSet<PersonEntity> Person { get; set; }
    public DbSet<ResumeEntity> Resume { get; set; }

    private string DbPath { get; } = Path.Join(Storage.LocalSqlitePath, "application.db");

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        Directory.CreateDirectory(Storage.LocalSqlitePath);
        options.UseSqlite($"Data Source={DbPath}");
    }
}
