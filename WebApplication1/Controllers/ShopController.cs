using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList;
using WebApplication1.Models.Data;
using WebApplication1.Models.ViewModels.Categories;
using WebApplication1.Models.ViewModels.Items;

namespace WebApplication1.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop/Items
        public ActionResult Items(int? page, int? catID)
        {
            //Declare list of ItemVM

            List<ItemsVM> itemList;

            //Set page number
            var pageNumber = page ?? 1;

            using (Db db = new Db())
            {
                //init the list
                itemList = db.Items.ToArray()
                    .Where(x => catID == null || catID == 0 || x.CategoryId == catID)
                    .Select(x => new ItemsVM(x))
                    .ToList();
                //populate categories select list
                ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");

                //set selected category
                ViewBag.SelectedCat = catID.ToString();
            }
            //set pagination
            var onePageOfItems = itemList.ToPagedList(pageNumber, 3);

            ViewBag.OnePageOfItems = onePageOfItems;
            //return view with list
            return View(itemList);
        }

        [HttpGet]
        public ActionResult EditItem(int id)
        {
            ItemsVM item;

            using (Db db = new Db())
            {

                ItemsDTO dto = db.Items.Find(id);
                if (dto == null)
                {
                    return Content("The item doesn't exist");
                }

                item = new ItemsVM(dto);

                List<CategoryVM> categories;

                item.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                 
               
            }

            return View(item);
        }

        [HttpPost]
        public ActionResult EditItem(ItemsVM item, HttpPostedFileBase file)
        {

            using (Db db = new Db())
            {
                item.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            }

            if (!ModelState.IsValid)
            {
                return View(item);
            }

            //unique name
            using (Db db = new Db())
            {
                if (db.Items.Where(x => x.Id != item.Id).Any(x => x.Title == item.Title))
                {
                    ModelState.AddModelError("", "That item name is already taken! :(");
                    return View(item);
                }
            }

            using (Db db = new Db())
            {
                ItemsDTO dto = db.Items.Find(item.Id);

                dto.Id = item.Id;
                dto.Title = item.Title;
                dto.Description = item.Description;
                dto.Price = item.Price;
                dto.Image = item.Image;
                int id = dto.Id;

                var directory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
                var path1 = Path.Combine(directory.ToString(), "Items");
                var path2 = Path.Combine(directory.ToString(), "Items" + id.ToString());

                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(path1);
                }
                if (!Directory.Exists(path2))
                {
                    Directory.CreateDirectory(path2);
                }

                if (file != null && file.ContentLength > 0)
                {
                    string extension = file.ContentType.ToLower();

                    if (extension != "image/jpg" && extension != "image/jpeg" && extension != "image/png")
                    {
                        item.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                        ModelState.AddModelError("", "The image was not uploaded - wrong image extension.");
                        return View(item);
                    }


                    string imageName = file.FileName;

                    ItemsDTO dto1 = db.Items.Find(id);
                    dto1.Image = imageName;

                    db.SaveChanges();

                    file.SaveAs(string.Format("{0}\\{1}", path2, imageName));

                    WebImage img = new WebImage(file.InputStream);
                    img.Resize(200, 200);
                    img.Save(path2, "png", true);
                }
                CategoriesDTO catDto = db.Categories.FirstOrDefault(x => x.Id == item.CategoryId);
                dto.CategoryName = catDto.Name;

                db.SaveChanges();

                TempData["Status"] = "Item edited! <3";
            }

            return RedirectToAction("Items");
        }

        public ActionResult DeleteItem(int id)
        {
            using (Db db = new Db())
            {
                ItemsDTO dto = db.Items.Find(id);

                //remove the item
                db.Items.Remove(dto);
                db.SaveChanges();

                TempData["Status"] = "Item deleted! <3";

                return RedirectToAction("Items");
            }
        }
    }
}