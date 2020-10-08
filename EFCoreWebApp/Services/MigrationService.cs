using EFCoreWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EFCoreWebApp.Services
{
    public class MigrationService : IHostedService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IConfiguration configuration;

        public MigrationService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this.serviceProvider = serviceProvider;
            this.configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var serviceScope = serviceProvider.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<EFCoreSampleDbContext>())
                {
                    await MigrateAsync(context);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public static User CreateUser()
        {
            var code1 = new Code
            {
                CityCode = 1111
            };

            var code2 = new Code
            {
                CityCode = 2222
            };

            return new User
            {
                Name = "My User",
                Codes = new[] { code1, code2 }
            };
        }

        private async Task MigrateAsync(EFCoreSampleDbContext context)
        {
            await context.Database.EnsureCreatedAsync();
            await context.Database.MigrateAsync();

            if (!context.Users.Any())
            {
                var user = CreateUser();

                context.Users.Add(user);

                await context.SaveChangesAsync();
            }
        }
    }
}
