using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.Data;
using WebApplication1.Models.ViewModels.Items;
using WebApplication1.Models.ViewModels.Requests;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class RequestsController : Controller
    {

        Db db = new Db();

        // GET: Admin/Requests
        public ActionResult Index()
        {
            List<RequestsVM> requests;

            requests = db.Requests.
                         ToArray().
                         Select(x => new RequestsVM(x)).
                         ToList();

            return View(requests);
        }

        public ActionResult AcceptRequest(int reqId, int itemId)
        {

            ItemsVM item;

            ItemsDTO dto = db.Items.Find(itemId);
            RequestsDTO dto2 = db.Requests.Find(reqId);

            if(dto == null)
            {
                return Content("The item doesn t exist anymore :(");
            }

            //set status to true => the item will be visible
            db.Items.Attach(dto);
            db.Entry(dto).Property(x => x.Status).IsModified = true;
            db.SaveChanges();

            //remove the request
            db.Requests.Remove(dto2);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult DeclineRequest(int reqId, int itemId)
        {
            ItemsDTO dto = db.Items.Find(itemId);
            RequestsDTO dto2 = db.Requests.Find(reqId);

            db.Items.Remove(dto);
            db.Requests.Remove(dto2);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}