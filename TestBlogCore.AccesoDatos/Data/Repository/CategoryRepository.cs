using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using TestBlogCore.AccesoDatos.Data.Repository.IRepository;
using TestBlogCore.Data;
using TestBlogCore.Models;

namespace TestBlogCore.AccesoDatos.Data.Repository
{
    /// <summary>
    /// Repositorio específico para manejar operaciones de datos relacionadas con la entidad "Category".
    /// Hereda de la clase genérica Repository y utiliza el contexto de la base de datos ApplicationDbContext.
    /// </summary>
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        /// <summary>
        /// Contexto de la base de datos utilizado para interactuar con las tablas.
        /// </summary>
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Constructor que inicializa el repositorio con el contexto de la base de datos.
        /// </summary>
        /// <param name="db">El contexto de la base de datos ApplicationDbContext.</param>
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetCategoryListForDropDown()
        {
            return _db.Category.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        /// <summary>
        /// Actualiza un registro existente en la tabla de categorías.
        /// </summary>
        /// <param name="category">La entidad de categoría con los nuevos valores.</param>
        public void Update(Category category)
        {
            // Busca la categoría en la base de datos utilizando su ID.
            var objFromDb = _db.Category.FirstOrDefault(s => s.Id == category.Id);

            // Si se encuentra la categoría, actualiza los valores.
            if (objFromDb != null)
            {
                objFromDb.Name = category.Name; // Actualiza el nombre de la categoría.
                objFromDb.Order = category.Order; // Actualiza el orden de la categoría.
            }

            // Guarda los cambios en la base de datos.
            _db.SaveChanges();
        }
    }
}
