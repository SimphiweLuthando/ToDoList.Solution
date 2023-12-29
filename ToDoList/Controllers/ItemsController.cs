// using Microsoft.AspNetCore.Mvc;
// using ToDoList.Models;
// using System.Collections.Generic;

// namespace ToDoList.Controllers
// {
//   public class ItemsController : Controller
//   {

//     [HttpGet("/categories/{categoryId}/items/new")]
//     public ActionResult New(int categoryId)
//     {
//        Category category = Category.Find(categoryId);
//        return View(category);
//     }

//     [HttpGet("/categories/{categoryId}/items/{itemId}")]
//     public ActionResult Show(int categoryId, int itemId)
//     {
//       Item item = Item.Find(itemId);
//       Category category = Category.Find(categoryId);
//       Dictionary<string, object> model = new Dictionary<string, object>();
//       model.Add("item", item);
//       model.Add("category", category);
//       return View(model);
//     }
//   }
// }

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ToDoListContext _db;

        public ItemsController(ToDoListContext db)
        {
            _db = db;
        }

        public ActionResult Index()
        {
            List<Item> model = _db.Items.ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Item item)
        {
            _db.Items.Add(item);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
            return View(thisItem);
        }
    }
}
