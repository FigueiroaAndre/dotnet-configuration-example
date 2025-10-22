using ConfigurationExample.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationExample.Data;

public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    {
    }

    public DbSet<Settings> Settings => Set<Settings>();
}
