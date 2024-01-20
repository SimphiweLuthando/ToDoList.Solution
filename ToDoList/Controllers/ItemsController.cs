using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {
        private readonly ToDoListContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        // Updated constructor
        public ItemsController(UserManager<ApplicationUser> userManager, ToDoListContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<ActionResult> Index()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
            List<Item> userItems = _db.Items.Where(entry => entry.User.Id == currentUser.Id)
                .Include(item => item.Category)
                .ToList();
            return View(userItems);
        }

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Item item, int CategoryId)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
                return View(item);
            }
            else
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
                item.User = currentUser;
                _db.Items.Add(item);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            Item thisItem = _db.Items.Include(item => item.User)
                .Include(item => item.Category)
                .Include(item => item.JoinEntities)
                .ThenInclude(join => join.Tag)
                .FirstOrDefault(item => item.ItemId == id);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (thisItem.User == null || thisItem.User.Id != userId)
            {
                return Unauthorized(); // This will trigger the 401 error
            }

            return View(thisItem);
        }

        public async Task<ActionResult> Edit(int id)
        {
            Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");

            string userId = _userManager.GetUserId(User);
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

            if (thisItem.User == null || thisItem.User.Id != currentUser.Id)
            {
                return Unauthorized(); // or return NotFound() I'll see what works best
            }

            return View(thisItem);
        }

        // [HttpPost]
        // public ActionResult Edit(Item item)
        // {
        //     _db.Items.Update(item);
        //     _db.SaveChanges();
        //     return RedirectToAction("Index");
        // }

        // public ActionResult Delete(int id)
        // {
        //     Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
        //     return View(thisItem);
        // }

        // [HttpPost, ActionName("Delete")]
        // public ActionResult DeleteConfirmed(int id)
        // {
        //     Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
        //     _db.Items.Remove(thisItem);
        //     _db.SaveChanges();
        //     return RedirectToAction("Index");
        // }

        [HttpPost]
        public ActionResult Edit(Item item)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (item.User == null || item.User.Id != userId)
            {
                return Unauthorized(); // or return NotFound() if you want to hide the existence of the item
            }

            _db.Items.Update(item);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (thisItem.User == null || thisItem.User.Id != userId)
            {
                return Unauthorized(); // or return NotFound() if you want to hide the existence of the item
            }

            _db.Items.Remove(thisItem);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddTag(int id)
        {
            Item thisItem = _db.Items.FirstOrDefault(items => items.ItemId == id);
            ViewBag.TagId = new SelectList(_db.Tags, "TagId", "Title");
            return View(thisItem);
        }

        [HttpPost]
        public ActionResult AddTag(Item item, int tagId)
        {
#nullable enable
            ItemTag? joinEntity = _db.ItemTags.FirstOrDefault(
                join => (join.TagId == tagId && join.ItemId == item.ItemId)
            );
#nullable disable
            if (joinEntity == null && tagId != 0)
            {
                _db.ItemTags.Add(new ItemTag() { TagId = tagId, ItemId = item.ItemId });
                _db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = item.ItemId });
        }

        [HttpPost]
        public ActionResult DeleteJoin(int joinId)
        {
            ItemTag joinEntry = _db.ItemTags.FirstOrDefault(entry => entry.ItemTagId == joinId);
            _db.ItemTags.Remove(joinEntry);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
