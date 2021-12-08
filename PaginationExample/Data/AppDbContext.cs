using Microsoft.EntityFrameworkCore;
using PaginationExample.Models;

namespace PaginationExample.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Todo> Todos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlite("DataSource=app.db;Cache=Shared");
    }
}
