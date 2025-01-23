using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestBlogCore.AccesoDatos.Data.Repository.IRepository;
using TestBlogCore.Models;

namespace TestBlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly IWorkContainer _workContainer;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public SlidersController(IWorkContainer workContainer, IWebHostEnvironment hostingEnvironment)
        {
            _workContainer = workContainer;
            _hostingEnvironment = hostingEnvironment;
        }

        #region Métodos para las vistas
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider) {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\sliders");
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
                    slider.URLImage = @"\images\sliders\" + fileName + extension;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "La imagen es obligatoria.");
                    return View(slider);
                }
                _workContainer.Slider.Add(slider);
                _workContainer.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(slider);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Slider slider = _workContainer.Slider.Get(id);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Slider slider)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                var sliderFromDb = _workContainer.Slider.Get(slider.Id);
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\sliders");
                    var extension_new = Path.GetExtension(files[0].FileName);
                    var imagePath = Path.Combine(webRootPath, sliderFromDb.URLImage.TrimStart('\\'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension_new), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    slider.URLImage = @"\images\sliders\" + fileName + extension_new;
                }
                else
                {
                    slider.URLImage = sliderFromDb.URLImage;
                }
                _workContainer.Slider.Update(slider);
                _workContainer.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(slider);
        }

        #endregion

        #region API CALLS   
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _workContainer.Slider.GetAll() });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _workContainer.Slider.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error al borrar el slider." });
            }
            string webRootPath = _hostingEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, objFromDb.URLImage.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            _workContainer.Slider.Remove(objFromDb);
            _workContainer.Save();
            return Json(new { success = true, message = "Slider borrado exitosamente." });
        }
        #endregion
    }
}
