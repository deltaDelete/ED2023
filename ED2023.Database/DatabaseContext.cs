using System.Runtime.CompilerServices;
using Avalonia.Data;
using ED2023.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ED2023.Database;

/// <inheritdoc cref="DbContext"/>
public class DatabaseContext : DbContext {
    public static readonly string
        ConnectionString = 
            // "server=localhost;user=dev;password=devPassword;database=ed2023";
            "server=10.10.1.24;user=user_01;password=user01pro;database=pro1_2";

    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<ClientExperience> ClientExperiences { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<LanguageLevel> LanguageLevels { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    
    /// <inheritdoc cref="DbContext"/>
    public DatabaseContext() {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseMySql(
            ConnectionString,
            ServerVersion.AutoDetect(ConnectionString)
        );
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.LogTo(Console.WriteLine);
    }

    private static readonly Lazy<DatabaseContext> LazyInstance = new Lazy<DatabaseContext>(() => new DatabaseContext());
    public static DatabaseContext Instance => LazyInstance.Value;
    
    [MethodImpl(MethodImplOptions.Synchronized)]
    public static DatabaseContext InstanceFor(object context) {
        if (Instances.TryGetValue(context, out var instance)) {
            return instance;
        }
        Instances.Add(context, new());
        return Instances[context];
    }

    public static DatabaseContext NewInstance() {
        return new DatabaseContext();
    }

    public static Dictionary<object, DatabaseContext> Instances { get; } = new Dictionary<object, DatabaseContext>();
}
