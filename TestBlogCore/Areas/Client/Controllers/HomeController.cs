using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestBlogCore.AccesoDatos.Data.Repository.IRepository;
using TestBlogCore.Models;
using TestBlogCore.Models.ViewModels;

namespace TestBlogCore.Areas.Client.Controllers
{

    [Area("Client")]
    public class HomeController : Controller
    {
        private readonly IWorkContainer _workContainer;

        public HomeController(IWorkContainer workContainer)
        {
            _workContainer = workContainer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                slidersList = _workContainer.Slider.GetAll(),
                articlesList = _workContainer.Article.GetAll()
            };
            ViewBag.IsHome = true;

            return View(homeViewModel);
        }

        public IActionResult Details(int id)
        {
            var objFromDb = _workContainer.Article.Get(id);
            return View(objFromDb);
        }

        [HttpGet]
        public IActionResult SearchResult(string searchString,int page=1, int pageSize=6)
        {
            var articles = _workContainer.Article.AsQueryable();

            //Filtar por nombre
            if (!string.IsNullOrEmpty(searchString))
            {
                articles = articles.Where(s => s.Name.Contains(searchString));
            }

            //Paginación
            var paginatedArticles = articles.Skip((page - 1) * pageSize).Take(pageSize);

            //Enviamos los datos a la vista
            var model = new paginatedList<Article>(paginatedArticles.ToList(), articles.Count(), page, pageSize, searchString);
            return View(model);
        }
    }
}
