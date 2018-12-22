using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebApplication1.Models.Data;
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
    }
}