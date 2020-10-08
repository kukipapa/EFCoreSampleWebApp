using EFCoreWebApp;
using EFCoreWebApp.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;

namespace EFCoreWebAppTests
{
    public class DataServiceTests
    {
        public static IConfigurationRoot GetIConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        private readonly DbContextOptions<EFCoreSampleDbContext> options = new DbContextOptionsBuilder<EFCoreSampleDbContext>()
            .UseSqlServer(GetIConfigurationRoot().GetConnectionString("DefaultConnection")).Options;

        public DataServiceTests()
        {
            using (var context = new EFCoreSampleDbContext(options))
            {
                context.Database.EnsureCreated();
                context.Database.Migrate();
            }
        }

        [Fact]
        public async Task GetUsersAsync_ValidateUser()
        {
            var user = MigrationService.CreateUser();

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var context = new EFCoreSampleDbContext(options))
                {
                    context.Users.Add(user);

                    await context.SaveChangesAsync();
                }

                using (var context = new EFCoreSampleDbContext(options))
                {
                    var dataService = new DataService(context);

                    var users = await dataService.GetUsersAsync();
                    var dbUser = users.Single(u => u.Id == user.Id);

                    dbUser.Name.Should().Be(user.Name);
                    dbUser.Codes.Count.Should().Be(user.Codes.Count);
                }
            }
        }

        [Fact]
        public async Task UpdateUserAsync_WhenPassingUpdatedUser_ShouldUpdateProperly()
        {
            var user = MigrationService.CreateUser();

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var context = new EFCoreSampleDbContext(options))
                {
                    context.Users.Add(user);

                    await context.SaveChangesAsync();
                }

                var code = user.Codes.First();
                code.CityCode = 3333;

                using (var context = new EFCoreSampleDbContext(options))
                {
                    var dataService = new DataService(context);

                    await dataService.UpdateUserAsync(user);
                }

                using (var context = new EFCoreSampleDbContext(options))
                {
                    var dataService = new DataService(context);

                    var users = await dataService.GetUsersAsync();
                    var dbUser = users.Single(u => u.Id == user.Id);

                    dbUser.Codes.Single(c => c.Id == code.Id).CityCode.Should().Be(3333);
                }
            }
        }
    }
}
