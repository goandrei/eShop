using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.Data;
using WebApplication1.Models.ViewModels.Items;

namespace WebApplication1.Areas.Collab.Controllers
{
    public class ItemsController : Controller
    {

        Db db = new Db();

        // GET: Collab/Items
        public ActionResult Index()
        {
            return View();
        }

        // GET : Collab/Items/AddItem
        [HttpGet]
        public ActionResult AddItem()
        {
            ItemsVM item = new ItemsVM();

            item.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");

            return View(item);
        }

        // POST : Collab/Items/AddItem
        [HttpPost]
        public ActionResult AddItem(ItemsVM item, HttpPostedFileBase file)
        {
            //rebuild the selectList
            item.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");

            if (!ModelState.IsValid)
            {
                return View(item);
            }

            ItemsDTO dto = new ItemsDTO();
            dto.Title = item.Title;
            dto.Description = item.Description;
            dto.Price = item.Price;
            dto.Score = -1;
            dto.CategoryId = item.CategoryId;

            CategoriesDTO catDto = db.Categories.FirstOrDefault(x => x.Id == item.CategoryId);

            dto.CategoryName = catDto.Name;

            //insert and commit
            db.Items.Add(dto);
            db.SaveChanges();

            TempData["Status"] = "The item was added! <3";

            //TODO : upload image

            return View(item);
        }
    }
}