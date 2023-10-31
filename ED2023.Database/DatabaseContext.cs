using ED2023.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ED2023.Database;

/// <inheritdoc cref="DbContext"/>
public class DatabaseContext : DbContext {
    public static readonly string
        ConnectionString = "server=10.10.1.24;user=user_01;password=user01pro;database=pro1_2";

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
    public DbSet<Teather> Teathers { get; set; }
    
    /// <inheritdoc cref="DbContext"/>
    public DatabaseContext() {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseMySql(
            ConnectionString,
            ServerVersion.AutoDetect(ConnectionString)
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
    }

    private static readonly Lazy<DatabaseContext> LazyInstance = new Lazy<DatabaseContext>(() => new DatabaseContext());
    public static DatabaseContext Instance => LazyInstance.Value;
}
