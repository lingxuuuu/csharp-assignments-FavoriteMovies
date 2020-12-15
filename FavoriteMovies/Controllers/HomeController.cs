using System;
using System.Linq; // allows us to access query methods(FirstOrDefault)
using Microsoft.AspNetCore.Mvc;
using FavoriteMovies.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

public class HomeController : Controller
{
    [HttpGet("dashboard")]
    public IActionResult Dashboard()
    {
        Console.WriteLine(HttpContext.Session.GetInt32("UserId"));
        return View();
    }
}