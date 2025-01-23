using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestBlogCore.Models;

namespace TestBlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        void blockUser(string userId);
        void unBlockUser(string userId);
        void changeRole(string userId, string role);
    }
}
