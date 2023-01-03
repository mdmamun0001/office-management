using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using Zdm_management.Models;

namespace Zay.Data
{
    public  class  DumyData
    {
        
        public static async Task<Boolean> Initialize( UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            
            string Name = "Zdm Information Technology";
            string userEmail = "zdm@gmail.com";
            string userPassword = "Zdm@gmail.comp@$$word007";
            int IdCardNumber = 1;
            string RoleName = "Super Admin";

            if (await userManager.FindByEmailAsync(userEmail) == null)
            {

                var user = new ApplicationUser { Name = Name, UserName = userEmail, Email = userEmail, IdCardNumber = IdCardNumber };
                IdentityResult result = await userManager.CreateAsync(user, userPassword);
                if(result.Succeeded)
                {
                    var AdminRole = await roleManager.FindByNameAsync(RoleName);
                    if (AdminRole != null)
                    {
                        result = await userManager.AddToRoleAsync(user, RoleName);
                        if (result.Succeeded) { return true; }
                    }
                    IdentityRole identityRole = new IdentityRole { Name = RoleName };
                    result = await roleManager.CreateAsync(identityRole);
                    if (result.Succeeded)
                    {
                        result =  await userManager.AddToRoleAsync(user, RoleName);
                        if (result.Succeeded) { return true; }
                    }
                }
            }
            return false;
        }
    }
}
