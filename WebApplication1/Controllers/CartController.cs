using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.Data;
using WebApplication1.Models.ViewModels.Cart;

namespace WebApplication1.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            //init the cart list
            var cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();

            //check if the cart is empty
            if (cart.Count == 0 || Session["cart"] == null)
            {
                ViewBag.Message = "Your cart is empty.";
                return View();
            }
            // calculate total and save to ViewBag
            decimal total = 0m;

            foreach(var item in cart)
            {
                total += item.Total;
                ViewBag.GrandTotal = total;
            }

            return View(cart);
        }

        public ActionResult CartPartial()
        {
            //init cart VM
            CartVM model = new CartVM();
            //init quantity
            int qty = 0;
            //init price
            decimal price = 0m;
            //check for cart session
            if (Session["cart"] != null)
            {
                var list = (List<CartVM>)Session["cart"];

                foreach (var item in list)
                {
                    qty += item.Quantity;
                    price += item.Quantity * item.Price;
                }

                model.Quantity = qty;
                model.Price = price;
            }
            else
            {
                model.Quantity = 0;
                model.Price = 0m;
            }

            return PartialView(model);
        }

        public ActionResult AddToCartPartial(int id)
        {
            List<CartVM> cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();

            CartVM model = new CartVM();
            
            //check item is in the cart
            using (Db db = new Db())
            {
                ItemsDTO item = db.Items.Find(id);

                var itemInCart = cart.FirstOrDefault(x => x.ItemID == id);
                
                //if not, add a new one
                if (itemInCart == null)
                {
                    cart.Add(new CartVM()
                    {
                        ItemID = item.Id,
                        ItemName = item.Title,
                        Quantity = 1,
                        Price = item.Price,
                        Image = item.Image
                        
                    });
                }
                //if it is, increment quantity
                else
                {
                    itemInCart.Quantity++;
                }
            }

            //get grand quantity and price and add to model
            int qty = 0;
            decimal price = 0m;

            foreach (var item in cart)
            {
                qty += item.Quantity;
                price += item.Quantity * item.Price;
            }

            model.Quantity = qty;
            model.Price = price;

            //Save cart to session
            Session["cart"] = cart;

            return PartialView(model);
        }

        // GET: Cart/IncrementProduct
        public JsonResult IncrementProduct(int itemID)
        {
            //Init cart list
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            using (Db db = new Db())
            {
                //Get cartVM from list
                CartVM model = cart.FirstOrDefault(x => x.ItemID == itemID);

                model.Quantity++;

                //Store needed data
                var result = new { qty = model.Quantity, price = model.Price};

                //Return JSON
                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }

        // GET: Cart/DecrementProduct
        public JsonResult DecrementProduct(int itemID)
        {
            //Init cart list
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            using (Db db = new Db())
            {
                //Get cartVM from list
                CartVM model = cart.FirstOrDefault(x => x.ItemID == itemID);

                model.Quantity--;

                //Store needed data
                var result = new { qty = model.Quantity, price = model.Price };

                //Return JSON
                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }

        // GET: Cart/RemoveProduct
        public JsonResult RemoveProduct(int itemID)
        {
            //Init cart list
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            using (Db db = new Db())
            {
                //Get cartVM from list
                CartVM model = cart.FirstOrDefault(x => x.ItemID == itemID);

                
                //Store needed data
                var result = new { qty = model.Quantity, price = model.Price };

                //Return JSON
                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }

    }

}