using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ServerCenter.Models;

namespace ServerCenter.Data
{
    public class ApplicationDbContextSeed
    {
        private UserManager<User> _userManager;
        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider service)
        {
            using(var scope = service.CreateScope())
            {
                if (!context.Users.Any())
                {
                    _userManager = scope.ServiceProvider.GetService<UserManager<User>>();
                    var defaultUser = new User
                    {
                        UserName = "admin"
                    };
                    var result = await _userManager.CreateAsync(defaultUser, "admin");
                    if (!result.Succeeded)
                    {
                        throw new Exception("初始化数据失败：" + result.Errors.First().Description);
                    }
                }
            }
        }
    }
}
