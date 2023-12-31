using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ToDoListContext _db;

        public CategoriesController(ToDoListContext context)
        {
            _db = context;
        }

        // GET: Categories
        public IActionResult Index()
        {
            List<Category> model = _db.Categories.ToList();
            return View(model);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        public IActionResult Create(Category category)
        {
            _db.Categories.Add(category);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            Category thisCategory = _db.Categories.FirstOrDefault(category => category.CategoryId == id);
            return View(thisCategory);
        }

        // GET: Categories/Edit/5
        public IActionResult Edit(int id)
        {
            Category thisCategory = _db.Categories.FirstOrDefault(
                category => category.CategoryId == id
            );
            return View(thisCategory);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            _db.Update(category);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Categories/Delete/5
        public IActionResult Delete(int id)
        {
            Category category = _db.Categories.FirstOrDefault(
                category => category.CategoryId == id
            );
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            Category category = _db.Categories.FirstOrDefault(
                category => category.CategoryId == id
            );
            _db.Categories.Remove(category);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
