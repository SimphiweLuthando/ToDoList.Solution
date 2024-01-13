using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class TagsController : Controller
    {
        private readonly ToDoListContext _db;

        public TagsController(ToDoListContext db)
        {
            _db = db;
        }

        public ActionResult Index()
        {
            return View(_db.Tags.ToList());
        }

        public ActionResult Details(int id)
        {
            Tag thisTag = _db.Tags.Include(tag => tag.JoinEntities)
                .ThenInclude(join => join.Item)
                .FirstOrDefault(tag => tag.TagId == id);
            return View(thisTag);
        }
    }
}
