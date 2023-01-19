using Amazon.Core.Entities.Identity;
using Amazon.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Services
{
    public class TokenServices : ITokenServices
    {
        public  IConfiguration Configuration { get; }

        public TokenServices(IConfiguration configuration)
        {
           Configuration = configuration;
        }

        public async Task<string> CreateToken(AppUser user , UserManager<AppUser> userManager)
        {
            // Private Claims (User-Defiend) 
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email , user.Email),
                new Claim(ClaimTypes.GivenName , user.DisplayName),
            };
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]));

            var token = new JwtSecurityToken(
                // Register Claims
                issuer: Configuration["JWT:ValidIssuer"],
                audience: Configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(double.Parse(Configuration["JWT:DurationInDays"])),
                // Private Claims 
                claims : authClaims,
                signingCredentials: new SigningCredentials(authKey , SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
