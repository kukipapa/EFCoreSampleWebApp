using EFCoreWebApp.Interfaces;
using EFCoreWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreWebApp.Services
{
    public class DataService : IDataService
    {
        private readonly EFCoreSampleDbContext context;

        public DataService(EFCoreSampleDbContext context)
        {
            this.context = context;
        }

        public async Task<IReadOnlyCollection<User>> GetUsersAsync()
        {
            return await (from u in context.Users.Include(u => u.Codes)
                          select u).ToListAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
    }
}
