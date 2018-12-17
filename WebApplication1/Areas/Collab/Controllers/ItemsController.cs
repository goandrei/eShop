﻿using System;
using System.Collections.Generic;
using System.IO;
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
            dto.Status = false;
            dto.CategoryId = item.CategoryId;

            CategoriesDTO catDto = db.Categories.FirstOrDefault(x => x.Id == item.CategoryId);

            dto.CategoryName = catDto.Name;

            //insert and commit
            db.Items.Add(dto);
            db.SaveChanges();

            TempData["Status"] = "The item was added! <3";

            int id = dto.Id;

            var directory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            var path1 = Path.Combine(directory.ToString(), "Items");
            var path2 = Path.Combine(directory.ToString(), "Items" + id.ToString());

            if(!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
            }
            if(!Directory.Exists(path2))
            {
                Directory.CreateDirectory(path2);
            }

            if(file != null && file.ContentLength > 0)
            {
                string extension = file.ContentType.ToLower();

                if(extension != "image/jpg" && extension != "image/jpeg" && extension != "image/png")
                {
                    item.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                    ModelState.AddModelError("", "the file extension isn't accepted :(");
                    return View(item);
                }
            }

            string imageName = file.FileName;

            ItemsDTO dto1 = db.Items.Find(id);
            dto1.Image = imageName;

            db.SaveChanges();

            file.SaveAs(string.Format("{0}\\{1}", path2, imageName));

            //TODO : upload image

            return RedirectToAction("AddItem");
        }
    }
}