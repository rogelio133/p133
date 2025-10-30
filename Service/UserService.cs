using Microsoft.AspNetCore.Identity;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service
{
    public class UserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<ApplicationUser>> GetUsersInRoleAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
                return new List<ApplicationUser>();

            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            return usersInRole.ToList();
        }
    }
}
