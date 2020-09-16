using Microsoft.AspNet.Identity;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MyShop.WebUI.Controllers
{
    [Authorize]
    public class UserOrdersController : Controller
    {
        IOrderService orderService;

        public UserOrdersController(IOrderService OrderService)
        {
            this.orderService = OrderService;
        }


        
        // GET: OrderManager
        public ActionResult Index()
        {
            List<Order> orders = orderService.GetOrderList();
            //Get the current users Id
            string Id = System.Web.HttpContext.Current.User.Identity.Name;

            //Make a new list to present
            List<Order> userBasedOrders = new List<Order>();

            //Iterate through the database of orders
            foreach (Order order in orders)
            {
                //If the orders email is equal to the current users email it will add it to the new list
                if (order.Email == Id)
                {
                    userBasedOrders.Add(order);
                }
            }
            return View(userBasedOrders);
        }

        public ActionResult UpdateOrder(string Id)
        {
            Order order = orderService.GetOrder(Id);
            return View(order);
        }


        [HttpPost]
        public ActionResult UpdateOrder(Order updatedOrder, string Id)
        {
            Order order = orderService.GetOrder(Id);
            order.OrderStatus = updatedOrder.OrderStatus;
            orderService.UpdateOrder(order);

            return RedirectToAction("Index");
        }
    }
}