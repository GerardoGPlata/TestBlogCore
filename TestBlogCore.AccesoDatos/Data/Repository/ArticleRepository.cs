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
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        private readonly ApplicationDbContext _db;
        public ArticleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IQueryable<Article> AsQueryable()
        {
            return _db.Set<Article>().AsQueryable();
        }

        public void Update(Article article)
        {
            var objFromDb = _db.Article.FirstOrDefault(s => s.Id == article.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = article.Name;
                objFromDb.Description = article.Description;
                objFromDb.CreatedAt = article.CreatedAt;
                objFromDb.URLImage = article.URLImage;
                objFromDb.CategoryId = article.CategoryId;
            }
            _db.SaveChanges();
        }

    }
}
