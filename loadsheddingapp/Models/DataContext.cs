
using loadsheddingapp.Services;
using Microsoft.EntityFrameworkCore;

namespace loadsheddingapp.Models
{
    public class DataContext : DbContext
    {

        private ISecretsManagerService ssmService;
        private IConfiguration configuration;
        public DataContext(ISecretsManagerService ssmService, IConfiguration configuration)  
        {
            this.ssmService = ssmService;
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }
            else
            {
                string connectionString = getConnString();
                optionsBuilder.UseSqlServer(
                   connectionString
                );
                base.OnConfiguring(optionsBuilder);
            }
        }

        private string getConnString()
        {
       
                var secretID = configuration.GetSection("DatabaseSecretID").Value.ToString();
                DbSecretModel secretModel = this.ssmService.getDatabaseCredential(secretID);
                return $"Server='{secretModel.Host}';" +
                   $" Database='{secretModel.Database}';" +
                   $" User Id='{secretModel.Username}'; " +
                   $"Password='{secretModel.Password}';";


        }

        public DbSet<Joke> Jokes { get; set; }
    }
}
