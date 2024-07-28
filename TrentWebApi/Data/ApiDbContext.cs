using Microsoft.EntityFrameworkCore;
using TrentWebApi.Models;

namespace TrentWebApi.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> optons) : base(optons)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Member> Members { get; set; }
    }
}
