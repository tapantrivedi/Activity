using Microsoft.EntityFrameworkCore;

namespace Activity.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
        public DbSet<User> UsersTable {get;set;}

        public DbSet<ActivitySchedule> ActivityTable {get;set;}
        public DbSet<Guest> Guest {get;set;}

    }
}