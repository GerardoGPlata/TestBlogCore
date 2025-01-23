﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestBlogCore.AccesoDatos.Data.Repository.IRepository;
using TestBlogCore.Models;

namespace TestBlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IWorkContainer _workContainer;

        public CategoriesController(IWorkContainer workContainer)
        {
            _workContainer = workContainer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //[AllowAnonymous] // Permite que la acción sea accesible sin autenticación
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _workContainer.Category.Add(category);
                _workContainer.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Category category = _workContainer.Category.Get(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _workContainer.Category.Update(category);
                _workContainer.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _workContainer.Category.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _workContainer.Category.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error al borrar la categoría" });
            }
            _workContainer.Category.Remove(objFromDb);
            _workContainer.Save();
            return Json(new { success = true, message = "Categoría borrada exitosamente" });
        }

        #endregion
    }
}
