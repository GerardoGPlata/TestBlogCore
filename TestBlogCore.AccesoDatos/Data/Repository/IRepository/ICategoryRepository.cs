using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestBlogCore.Models;

namespace TestBlogCore.AccesoDatos.Data.Repository.IRepository
{
    /// <summary>
    /// Interfaz específica para manejar operaciones relacionadas con la entidad "Category".
    /// Hereda de la interfaz genérica IRepository y añade una operación personalizada para actualizar categorías.
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// Actualiza un registro existente en la tabla de categorías.
        /// </summary>
        /// <param name="category">La entidad de categoría con los nuevos valores.</param>
        void Update(Category category);

        IEnumerable<SelectListItem> GetCategoryListForDropDown();
    }
}
