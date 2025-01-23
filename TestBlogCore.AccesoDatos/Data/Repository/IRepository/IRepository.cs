using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TestBlogCore.AccesoDatos.Data.Repository.IRepository
{
    /// <summary>
    /// Interfaz genérica para manejar operaciones básicas de acceso a datos.
    /// Esta interfaz permite realizar operaciones comunes como obtener, agregar y eliminar registros en una base de datos.
    /// Es genérica, lo que significa que puede ser utilizada con cualquier tipo de entidad (clase).
    /// </summary>
    /// <typeparam name="T">El tipo de entidad con el que trabajará esta interfaz. Debe ser una clase.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Obtiene un registro por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del registro.</param>
        /// <returns>El registro correspondiente al identificador proporcionado.</returns>
        T Get(int id);

        /// <summary>
        /// Obtiene una lista de registros.
        /// Permite aplicar filtros, ordenar los resultados y cargar propiedades relacionadas si es necesario.
        /// </summary>
        /// <param name="filter">Expresión para filtrar los registros. Ejemplo: x => x.Nombre == "Ejemplo".</param>
        /// <param name="orderBy">Función para ordenar los registros. Ejemplo: query => query.OrderBy(x => x.Nombre).</param>
        /// <param name="includeProperties">Lista de nombres de propiedades relacionadas que deben cargarse junto con los registros. Separar con comas (",") si hay más de una.</param>
        /// <returns>Una colección de registros que cumplen con los criterios especificados.</returns>
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string? includeProperties = null);

        /// <summary>
        /// Obtiene el primer registro que cumple con el criterio especificado.
        /// También permite cargar propiedades relacionadas si es necesario.
        /// </summary>
        /// <param name="filter">Expresión para filtrar el registro. Ejemplo: x => x.Id == 1.</param>
        /// <param name="includeProperties">Lista de nombres de propiedades relacionadas que deben cargarse junto con el registro. Separar con comas (",") si hay más de una.</param>
        /// <returns>El primer registro que cumple con el filtro especificado o null si no se encuentra ninguno.</returns>
        T GetFirstOrDefault(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null);

        /// <summary>
        /// Agrega un nuevo registro a la base de datos.
        /// </summary>
        /// <param name="entity">El registro que se desea agregar.</param>
        void Add(T entity);

        /// <summary>
        /// Elimina un registro de la base de datos utilizando su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del registro que se desea eliminar.</param>
        void Remove(int id);

        /// <summary>
        /// Elimina un registro de la base de datos utilizando la entidad misma.
        /// </summary>
        /// <param name="entity">El registro que se desea eliminar.</param>
        void Remove(T entity);
    }
}
