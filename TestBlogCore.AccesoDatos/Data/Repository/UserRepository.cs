using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestBlogCore.AccesoDatos.Data.Repository.IRepository;
using TestBlogCore.Data;
using TestBlogCore.Models;

namespace TestBlogCore.AccesoDatos.Data.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRepository(
    ApplicationDbContext db,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager
) : base(db)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void blockUser(string userId)
        {
            var user = _db.ApplicationUser.FirstOrDefault(u => u.Id == userId);
            user.LockoutEnd = DateTime.Now.AddYears(1000);
            _db.SaveChanges();
        }

        public void changeRole(string userId, string newRole)
        {
            var user = _db.ApplicationUser.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new ArgumentException("Usuario no encontrado.");
            }

            var currentRoles = _userManager.GetRolesAsync(user).Result;
            _userManager.RemoveFromRolesAsync(user, currentRoles).Wait();

            if (!_roleManager.RoleExistsAsync(newRole).Result)
            {
                throw new ArgumentException("El rol especificado no existe.");
            }

            _userManager.AddToRoleAsync(user, newRole).Wait();
            _db.SaveChanges();
        }

        public void unBlockUser(string userId)
        {
            var user = _db.ApplicationUser.FirstOrDefault(u => u.Id == userId);
            user.LockoutEnd = DateTime.Now;
            _db.SaveChanges();
        }
    }
}
