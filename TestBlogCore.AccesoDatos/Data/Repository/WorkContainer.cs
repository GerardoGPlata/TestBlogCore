using Microsoft.AspNetCore.Identity;
using TestBlogCore.AccesoDatos.Data.Repository.IRepository;
using TestBlogCore.AccesoDatos.Data.Repository;
using TestBlogCore.Data;
using TestBlogCore.Models;

public class WorkContainer : IWorkContainer, IDisposable
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public WorkContainer(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;

        #region Aquí se deben inicializar los repositorios necesarios para la aplicación

        Category = new CategoryRepository(_db);
        Article = new ArticleRepository(_db);
        Slider = new SliderRepository(_db);
        User = new UserRepository(_db, _userManager, _roleManager);


        #endregion
    }

    #region Aquí se deben importar las interfaces de los repositorios necesarios para la aplicación

    public ICategoryRepository Category { get; private set; }
    public IArticleRepository Article { get; private set; }
    public ISliderRepository Slider { get; private set; }
    public IUserRepository User { get; private set; }

    #endregion

    /// <summary>
    /// Proveer acceso a UserManager desde el WorkContainer.
    /// </summary>
    public UserManager<ApplicationUser> UserManager => _userManager;

    /// <summary>
    /// Proveer acceso a RoleManager desde el WorkContainer.
    /// </summary>
    public RoleManager<IdentityRole> RoleManager => _roleManager;

    /// <summary>
    /// Libera los recursos utilizados por el contexto de base de datos.
    /// </summary>
    public void Dispose()
    {
        _db.Dispose();
    }

    /// <summary>
    /// Guarda los cambios realizados en los repositorios a la base de datos.
    /// </summary>
    public void Save()
    {
        _db.SaveChanges(); // Aplica todos los cambios pendientes al contexto.
    }
}
