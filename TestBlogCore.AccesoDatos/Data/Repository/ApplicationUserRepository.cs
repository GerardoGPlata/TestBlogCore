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

    public class ApplicationUserRepository: Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ApplicationUser applicationUser)
        {
            throw new NotImplementedException();
        }
    }
}
