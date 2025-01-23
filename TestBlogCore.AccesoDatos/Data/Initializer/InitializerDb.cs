using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestBlogCore.Data;
using TestBlogCore.Models;
using TestBlogCore.Utils;

namespace TestBlogCore.AccesoDatos.Data.Initializer
{
    public class InitializerDb : IInitializerDb
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public InitializerDb(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initialize()
        {
            try
            {
                if(_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {
                throw;
            }

            if (_db.Roles.Any(roles => roles.Name == CNT.Admin)) return;

            // Crear roles
            _roleManager.CreateAsync(new IdentityRole(CNT.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(CNT.User)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(CNT.Guest)).GetAwaiter().GetResult();


            // Crear Admin
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "gerardogplata@gmail.com",
                Email = "gerardogplata@gmail.com",
                EmailConfirmed = true,
                FirstName = "Gerardo"
            },"Admin94*").GetAwaiter().GetResult();

            ApplicationUser user = _db.ApplicationUser.Where(u => u.Email == "gerardogplata@gmail.com").FirstOrDefault();
            _userManager.AddToRoleAsync(user, CNT.Admin).GetAwaiter().GetResult();
        }
    }
}
