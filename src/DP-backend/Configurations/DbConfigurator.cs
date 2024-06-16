using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DP_backend.Configurations
{
    public static class DbConfigurator
    {
        public static void AddDb<T>(this WebApplicationBuilder builder, string? connectionString = null) where T : DbContext
        {
            var npgsqlDataSource = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString(connectionString ?? "DefaultConnection"))
                .EnableDynamicJson()
                .Build();

            builder.Services.AddDbContext<T>(options =>
            {
                options.UseNpgsql(npgsqlDataSource);
            });
        }
        public static void MigrateDBWhenNecessary<T>(this WebApplication app) where T : DbContext
        {
            using (var scope = app.Services.CreateScope())
            {
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                if (bool.TryParse(config["RuntimeMigrations"], out bool migrate) && migrate)
                {
                    var dbcontext = scope.ServiceProvider.GetRequiredService<T>();
                    dbcontext.Database.Migrate();
                }
            }
        }
    }
}
