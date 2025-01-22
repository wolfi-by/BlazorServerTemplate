using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorServerTemplate.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    //public DbSet<CustomTheme> CustomThemes { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appname = Assembly.GetExecutingAssembly().GetName().Name ?? nameof(ApplicationDbContext);
        if (!Path.Exists(Path.Combine(appdata, appname)))
        {
            Directory.CreateDirectory(Path.Combine(appdata, appname));
        }
        string dbPath = Path.Combine(appdata, appname, "app.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath};Cache=Shared");

        base.OnConfiguring(optionsBuilder);
    }
    
}
