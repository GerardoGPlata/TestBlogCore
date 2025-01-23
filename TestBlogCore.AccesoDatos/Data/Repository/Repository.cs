using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TestBlogCore.AccesoDatos.Data.Repository.IRepository;

namespace TestBlogCore.AccesoDatos.Data.Repository
{
    /// <summary>
    /// Clase genérica que implementa la interfaz IRepository para manejar operaciones de acceso a datos.
    /// Utiliza Entity Framework Core para interactuar con la base de datos.
    /// </summary>
    /// <typeparam name="T">El tipo de entidad con el que trabaja esta clase. Debe ser una clase.</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Contexto de base de datos de Entity Framework Core.
        /// </summary>
        protected readonly DbContext Context;

        /// <summary>
        /// Conjunto de datos (tabla) correspondiente al tipo de entidad T.
        /// </summary>
        internal DbSet<T> dbSet;

        /// <summary>
        /// Constructor que inicializa el contexto y el DbSet correspondiente.
        /// </summary>
        /// <param name="context">El contexto de base de datos.</param>
        public Repository(DbContext context)
        {
            Context = context;
            this.dbSet = context.Set<T>();
        }

        /// <summary>
        /// Agrega un nuevo registro a la base de datos.
        /// </summary>
        /// <param name="entity">La entidad que se desea agregar.</param>
        public void Add(T entity)
        {
            dbSet.Add(entity); // Agrega el registro al DbSet.
        }

        /// <summary>
        /// Obtiene un registro de la base de datos utilizando su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del registro.</param>
        /// <returns>El registro encontrado o null si no existe.</returns>
        public T Get(int id)
        {
            return dbSet.Find(id); // Busca el registro por su ID.
        }

        /// <summary>
        /// Obtiene todos los registros que cumplen con los criterios especificados.
        /// </summary>
        /// <param name="filter">Expresión para filtrar los registros.</param>
        /// <param name="orderBy">Función para ordenar los registros.</param>
        /// <param name="includeProperties">Propiedades relacionadas que deben cargarse junto con los registros.</param>
        /// <returns>Una colección de registros.</returns>
        public IEnumerable<T> GetAll(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string? includeProperties = null)
        {
            // Se crea una consulta inicial basada en el DbSet.
            IQueryable<T> query = dbSet;

            // Si se especifica un filtro, se aplica a la consulta.
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Si se especifican propiedades relacionadas, se incluyen.
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            // Si se especifica un ordenamiento, se aplica y se retorna la lista ordenada.
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }

            // Si no hay ordenamiento, se retorna la lista tal como está.
            return query.ToList();
        }

        /// <summary>
        /// Obtiene el primer registro que cumple con los criterios especificados.
        /// </summary>
        /// <param name="filter">Expresión para filtrar el registro.</param>
        /// <param name="includeProperties">Propiedades relacionadas que deben cargarse junto con el registro.</param>
        /// <returns>El primer registro encontrado o null si no hay coincidencias.</returns>
        public T GetFirstOrDefault(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null)
        {
            // Se crea una consulta inicial basada en el DbSet.
            IQueryable<T> query = dbSet;

            // Si se especifica un filtro, se aplica a la consulta.
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Si se especifican propiedades relacionadas, se incluyen.
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            // Se retorna el primer registro encontrado o null si no hay coincidencias.
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Elimina un registro de la base de datos utilizando su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del registro a eliminar.</param>
        public void Remove(int id)
        {
            // Busca la entidad por ID y la elimina si se encuentra.
            T entityToRemove = dbSet.Find(id);
            Remove(entityToRemove);
        }

        /// <summary>
        /// Elimina un registro de la base de datos utilizando la entidad misma.
        /// </summary>
        /// <param name="entity">La entidad que se desea eliminar.</param>
        public void Remove(T entity)
        {
            dbSet.Remove(entity); // Elimina el registro del DbSet.
        }
    }
}
