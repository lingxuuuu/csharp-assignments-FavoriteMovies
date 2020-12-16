using System;
using System.Linq; // allows us to access query methods(FirstOrDefault)
using Microsoft.AspNetCore.Mvc;
using FavoriteMovies.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

public class HomeController : Controller
{

    private readonly MyContext _context;

    public HomeController(MyContext myContext)
    {
        _context = myContext;
    }

    [HttpGet("dashboard")]
    public IActionResult Dashboard()
    {
        //Console.WriteLine(HttpContext.Session.GetInt32("UserId"));
        int? userId = HttpContext.Session.GetInt32("UserId"); //? meaning we are not sure if there is a userId in session

        if(userId == null)
        {
            //no user present
            return RedirectToAction("LoginReg", "Users");
        }

        ViewBag.User = _context
        .Users
        .Find(userId);

        return View();
    }
}