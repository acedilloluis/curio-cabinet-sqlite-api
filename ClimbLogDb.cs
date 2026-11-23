using Microsoft.EntityFrameworkCore;

public class ClimbLogDb : DbContext
{
    public ClimbLogDb(DbContextOptions<ClimbLogDb> options)
        : base(options) {}

    public DbSet<ClimbLog> ClimbLogs => Set<ClimbLog>();
}