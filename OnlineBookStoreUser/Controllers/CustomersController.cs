﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStoreUser.Helper;
using OnlineBookStoreUser.Models;

namespace OnlineBookStoreUser.Controllers
{
    public class CustomersController : Controller
    {
        Book_Store_DbContext context = new Book_Store_DbContext();



        [HttpGet]
        public ViewResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Customers cust)
        {
            context.Customers.Add(cust);
            context.SaveChanges();

            return RedirectToAction("Login", "Customers");
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Login(Customers cust)

        {
            ViewBag.bookcategoryAuthor = context.Books.Include(c => c.Author).ToList();
            ViewBag.bookcategoryPublication = context.Books.Include(p => p.Publication).ToList();
            ViewBag.bookcategoryBook = context.Books.Include(b => b.BookCategory).ToList();

            var user = context.Customers.Where(x => x.UserName == cust.UserName && x.Password.Equals(cust.Password)).SingleOrDefault();
            if (user == null)
            {
                ViewBag.Error = "Invalid Credential";
                return View("Login");
            }
            else
            {
                int custId = user.CustomerId;
                //var obj = context.Customers.Where(a => a.UserName.Equals(cust.UserName) && a.Password.Equals(cust.Password)).FirstOrDefault();
                if (user != null)
                {
                    HttpContext.Session.SetString("uname", cust.UserName);
                    //return RedirectToAction("Details", "Customers", new { @id = custId });
                    //return RedirectToAction("CheckOut", "Index", new { @id = custId });
                    return RedirectToAction("Index", "Home", new { @id = custId });
                }
                else
                {
                    ViewBag.Error = "Invalid Credential";
                    return View("Index");
                }

            }

            

        }
        public ActionResult Details(int id)
        {
            Customers cust = context.Customers.Where(x => x.CustomerId == id).SingleOrDefault();
            //Books bk = context.Books.Where(x => x.BookId == id).SingleOrDefault();
            context.SaveChanges();
            return View(cust);
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("uname");
            return RedirectToAction("Index","Home");
        }

        public ActionResult Profile(int id)
        {
            Customers cust = context.Customers.Where(x => x.CustomerId == id).SingleOrDefault();
            //Books bk = context.Books.Where(x => x.BookId == id).SingleOrDefault();
            context.SaveChanges();
            return View(cust);

            
        }
    }
}
