using EFCoreWebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFCoreWebApp.Interfaces
{
    public interface IDataService
    {
        Task<IReadOnlyCollection<User>> GetUsersAsync();
        Task UpdateUserAsync(User user);
    }
}
