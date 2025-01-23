using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBlogCore.Models.ViewModels
{
    public class UserWithRolesViewModel
    {
        public ApplicationUser User { get; set; }
        public string CurrentRole { get; set; } // Rol actual del usuario
        public IEnumerable<string> AvailableRoles { get; set; } // Lista de roles disponibles
    }
}
