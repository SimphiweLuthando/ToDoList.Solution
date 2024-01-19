using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ToDoListContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager, ToDoListContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        // [HttpGet("/")]
        // public async Task<ActionResult> Index()
        // {
        //   Category[] cats = _db.Categories.ToArray();
        //   Dictionary<string,object[]> model = new Dictionary<string, object[]>();
        //   model.Add("categories", cats);
        //   string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //   ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        //   if (currentUser != null)
        //   {
        //     Item[] items = _db.Items
        //                 .Where(entry => entry.User.Id == currentUser.Id)
        //                 .ToArray();
        //     model.Add("items", items);
        //   }
        //   return View(model);
        // }

        [HttpGet("/")]
        public async Task<ActionResult> Index()
        {
            Category[] cats = _db.Categories.ToArray();
            Dictionary<string, object[]> model = new Dictionary<string, object[]>();
            model.Add("categories", cats);
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
            Item[] items;
            if (currentUser != null)
            {
                items = _db.Items.Where(entry => entry.User.Id == currentUser.Id).ToArray();
            }
            else
            {
                items = new Item[0]; // Add an empty array if currentUser is null
            }
            model.Add("items", items);
            return View(model);
        }
    }
}
