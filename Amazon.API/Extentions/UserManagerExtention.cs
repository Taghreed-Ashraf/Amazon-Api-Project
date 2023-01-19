using Amazon.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Amazon.API.Extentions
{
    public static class UserManagerExtention
    {
        public static async Task<AppUser> FindUserWithAdderssByEmailAsync(this UserManager<AppUser> userManager , ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var userwithAddress = await userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email);

            return userwithAddress;
        }
    }
}
