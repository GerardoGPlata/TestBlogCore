using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TestBlogCore.AccesoDatos.Data.Repository.IRepository;
using TestBlogCore.Models.ViewModels;

namespace TestBlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IWorkContainer _workContainer;

        public UsersController(IWorkContainer workContainer)
        {
            _workContainer = workContainer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var actualUser = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var users = _workContainer.User.GetAll(u => u.Id != actualUser);

            var roles = _workContainer.RoleManager.Roles.Select(r => r.Name).ToList();

            var viewModel = users.Select(user => new UserWithRolesViewModel
            {
                User = user,
                CurrentRole = _workContainer.UserManager.GetRolesAsync(user).Result.FirstOrDefault(),
                AvailableRoles = roles
            });

            return View(viewModel);
        }



        [HttpGet]
        public IActionResult Block(string id)
        {
           if(id == null)
            {
                return NotFound();
            }
            _workContainer.User.blockUser(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult UnBlock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _workContainer.User.unBlockUser(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ChangeRole(string userId, string newRole)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(newRole))
            {
                return BadRequest("Datos inválidos.");
            }

            try
            {
                _workContainer.User.changeRole(userId, newRole);
                _workContainer.Save();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Manejo de errores
                TempData["Error"] = $"Error al cambiar el rol: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
