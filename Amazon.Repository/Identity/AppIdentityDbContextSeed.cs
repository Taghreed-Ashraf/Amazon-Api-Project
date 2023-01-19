using Amazon.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Repository.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Taghreed Ashraf",
                    Email = "Taghreed.Ashraf@gmail.com",
                    UserName = "Taghreed.Ashraf",
                    PhoneNumber = "01152364865"
                };
                await userManager.CreateAsync(user , "Tt12345#");
            }
        }
    }
}
