using Microsoft.EntityFrameworkCore;

namespace loadsheddingapp.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Joke> Jokes { get; set; }
    }
}
