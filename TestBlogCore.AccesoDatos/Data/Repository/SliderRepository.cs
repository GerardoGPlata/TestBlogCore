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
    public class SliderRepository : Repository<Slider>, ISliderRepository
    {
        private readonly ApplicationDbContext _db;
        public SliderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Slider slider)
        {
            var objFromDb = _db.Slider.FirstOrDefault(s => s.Id == slider.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = slider.Name;
                objFromDb.State = slider.State;
                objFromDb.URLImage = slider.URLImage;
            }
            _db.SaveChanges();
        }
    }
}
