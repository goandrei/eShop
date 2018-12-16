using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.Data;
using WebApplication1.Models.ViewModels.Categories;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {

        Db db = new Db();

        // GET: Admin/Categories
        public ActionResult Index()
        {
            List<CategoriesVM> categories;

            categories = db.Categories.
                         ToArray().
                         Select(x => new CategoriesVM(x)).
                         ToList();

            return View(categories);
        }

        // GET: Admin/Categories/CreateCategory
        [HttpGet]
        public ActionResult CreateCategory()
        {
            return View();
        }

        // POST: Admin/Categories/CreateCategory
        [HttpPost]
        public ActionResult CreateCategory(CategoriesVM cat)
        {
            if(!ModelState.IsValid)
            {
                return View(cat);
            }

            CategoriesDTO dto = new CategoriesDTO();

            dto.Name = cat.Name;
            dto.Description = cat.Description;

            if(db.Categories.Any(x => x.Name == dto.Name))
            {
                ModelState.AddModelError("", "The category already exists!");
                return View(cat);
            }

            //insert and commit
            db.Categories.Add(dto);
            db.SaveChanges();

            TempData["Status"] = "Category created! <3";

            return RedirectToAction("CreateCategory");
        }

        // GET: Admin/Categories/EditCategory/id
        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            CategoriesVM cat;

            CategoriesDTO dto = db.Categories.Find(id);
            if(dto == null)
            {
                return Content("The content doesn't exist");
            }

            cat = new CategoriesVM(dto);

            return View(cat);
        }

        // POST: Admin/Categories/EditCategory/id
        [HttpPost]
        public ActionResult EditCategory(CategoriesVM cat)
        {
            CategoriesDTO dto = db.Categories.Find(cat.Id);

            if(db.Categories.Any(x => x.Name == cat.Name))
            {
                ModelState.AddModelError("", "The category already exists!");
                return View(cat);
            }

            dto.Id = cat.Id;
            dto.Name = cat.Name;
            dto.Description = cat.Description;

            db.SaveChanges();

            TempData["Status"] = "Category edited! <3";

            return RedirectToAction("EditCategory");
        }

        // POST : Admin/Categories/DeleteCategory/id
        [HttpGet]
        public ActionResult DeleteCategory(int id)
        {
            CategoriesDTO dto = db.Categories.Find(id);
            db.Categories.Remove(dto);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        
    }
}