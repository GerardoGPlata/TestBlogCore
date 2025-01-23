using Microsoft.AspNetCore.Identity;
using TestBlogCore.Models;

namespace TestBlogCore.AccesoDatos.Data.Repository.IRepository
{
    /// <summary>
    /// Interfaz que define el contenedor de trabajo (Unit of Work) para la aplicación.
    /// Centraliza el acceso a los repositorios y maneja la persistencia de cambios.
    /// </summary>
    public interface IWorkContainer : IDisposable
    {
        #region Repositorios

        ICategoryRepository Category { get; }
        IArticleRepository Article { get; }
        ISliderRepository Slider { get; }
        IUserRepository User { get; }

        #endregion

        /// <summary>
        /// Proporciona acceso a UserManager para gestionar usuarios.
        /// </summary>
        UserManager<ApplicationUser> UserManager { get; }

        /// <summary>
        /// Proporciona acceso a RoleManager para gestionar roles.
        /// </summary>
        RoleManager<IdentityRole> RoleManager { get; }

        /// <summary>
        /// Guarda los cambios realizados en los repositorios a la base de datos.
        /// </summary>
        void Save();
    }
}
