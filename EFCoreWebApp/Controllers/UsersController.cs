using EFCoreWebApp.Interfaces;
using EFCoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EFCoreWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IDataService dataService;

        public UsersController(IDataService dataService)
        {
            this.dataService = dataService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await dataService.GetUsersAsync();
            return Ok(users);
        }

        [HttpPost]
        [Route("user")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] User user)
        {
            await dataService.UpdateUserAsync(user);
            return Ok();
        }
    }
}
