using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestBlogCore.Models;

namespace TestBlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IArticleRepository : IRepository<Article>
    {
        void Update(Article article);

        IQueryable<Article> AsQueryable();
    }
}
