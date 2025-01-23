using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestBlogCore.AccesoDatos.Data.Repository.IRepository;
using TestBlogCore.Models.ViewModels;

namespace TestBlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ArticlesController : Controller
    {
        private readonly IWorkContainer _workContainer;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ArticlesController(IWorkContainer workContainer, IWebHostEnvironment hostingEnvironment)
        {
            _workContainer = workContainer;
            _hostingEnvironment = hostingEnvironment;
        }

        #region Métodos para las vistas

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ArticleViewModel articleVM = new ArticleViewModel()
            {
                Article = new TestBlogCore.Models.Article(),
                categoriesList = _workContainer.Category.GetCategoryListForDropDown()
            };

            return View(articleVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArticleViewModel articleVM)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\articles");
                    var extension = Path.GetExtension(files[0].FileName);

                    // Crear carpeta si no existe
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    articleVM.Article.URLImage = @"\images\articles\" + fileName + extension;
                }
                else
                {
                    ModelState.AddModelError("Article.URLImage", "Debe seleccionar una imagen.");
                    articleVM.categoriesList = _workContainer.Category.GetCategoryListForDropDown();
                    return View(articleVM);
                }

                articleVM.Article.CreatedAt = DateTime.Now.ToString();
                _workContainer.Article.Add(articleVM.Article);
                _workContainer.Save();
                return RedirectToAction(nameof(Index));
            }

            articleVM.categoriesList = _workContainer.Category.GetCategoryListForDropDown();
            return View(articleVM);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ArticleViewModel articleVM = new ArticleViewModel()
            {
                Article = new TestBlogCore.Models.Article(),
                categoriesList = _workContainer.Category.GetCategoryListForDropDown()
            };
            if(id != null)
            {
                articleVM.Article = _workContainer.Article.Get(id.GetValueOrDefault());
            }
            return View(articleVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArticleViewModel articleVM)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                var objFromDb = _workContainer.Article.Get(articleVM.Article.Id);

                //** Validar si se subió una nueva imagen
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\articles");
                    var extension_new = Path.GetExtension(files[0].FileName);
                    // Borrar imagen anterior
                    var imagePath = Path.Combine(webRootPath, objFromDb.URLImage.TrimStart('\\'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    // Subir nueva imagen
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension_new), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    articleVM.Article.URLImage = @"\images\articles\" + fileName + extension_new;
                }

                //** Si no se subió una nueva imagen, mantener la imagen anterior
                else
                {
                    articleVM.Article.URLImage = objFromDb.URLImage;
                }
                articleVM.Article.CreatedAt = objFromDb.CreatedAt;
                _workContainer.Article.Update(articleVM.Article);
                _workContainer.Save();
                return RedirectToAction(nameof(Index));
            }
            articleVM.categoriesList = _workContainer.Category.GetCategoryListForDropDown();
            return View(articleVM);
        }

        #endregion

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _workContainer.Article.GetAll(includeProperties:"Category") });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _workContainer.Article.Get(id);
            string webRootPath = _hostingEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, objFromDb.URLImage.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error al borrar el artículo." });
            }
            _workContainer.Article.Remove(objFromDb);
            _workContainer.Save();
            return Json(new { success = true, message = "Artículo borrado exitosamente." });

        }

        #endregion
    }
}
