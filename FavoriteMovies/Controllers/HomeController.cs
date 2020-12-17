using System;
using System.Linq; //allows us to access query methods(FirstOrDefault)
using Microsoft.AspNetCore.Mvc;
using FavoriteMovies.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;//allow us to use include


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

        if (userId == null)
        {
            //no user present
            return RedirectToAction("LoginReg", "Users");
        }

        ViewBag.User = _context
        .Users
        .Find(userId);

        // need movie data for view
        ViewBag.AllMovies = _context
        .Movies
        // one to many relationship need include to access the related Model in the view
        //meaning give me the PostedBy as well so I can use it in the view
        .Include(movie => movie.PostedBy)
        // many to many relationship, need to have .Include() to access the Likes in Dashboard.cshtml
        .Include(movie => movie.Likes)
        .OrderByDescending(movie => movie.Likes.Count)
        .ToList();

        return View();
    }


    [HttpGet("movies/new")] //this is the actual route in the address bar (action without asp points to this url)
    public IActionResult NewMoviePage() //the action name can be used with asp-action on cshtml page
    {

        int? userId = HttpContext.Session.GetInt32("UserId"); //int or null

        if (userId == null)
        {
            return RedirectToAction("LoginReg", "Users"); //action: go to LoginReg page, controller: UsersController//
        }

        ViewBag.User = _context
            .Users
            .Find(userId);

        return View();
    }

    [HttpPost("movies")] //this is the actual route in the address bar (action without asp points to this url)
    public IActionResult CreateMovie(Movie movieToCreate) //the action name can be used with asp-action on cshtml page
    {
        if (movieToCreate.ReleaseDate > DateTime.Now)
        {   //if the movie released after the created date, add a Error to ReleaseDate (This is part of the validation)
            ModelState.AddModelError("ReleaseDate", "You must specity a date in the past");
        }

        if (!ModelState.IsValid) // 2 ways to write this code (another one is like in UserController) -- if (ModelState.IsValid)
        {
            return View("NewMoviePage");
        }

        //we need to get UserId from the Session
        //Error: Cannot implicitly convert type 'int?' to 'int'. An explicit conversion exists (are you missing a cast?)
        //Error can be fixed using GetValueOrDefault(); if user is not in session, return the default 0
        movieToCreate.UserId = HttpContext.Session.GetInt32("UserId").GetValueOrDefault();

        _context.Add(movieToCreate);
        _context.SaveChanges();

        // need movie Id to show the movie detail in another page
        // after we add the movieToCreate in the _context and save the change, then the movie Id will get created
        // to send to the movie page
        return Redirect($"/movies/{movieToCreate.MovieId}");
    }

    //This line of code is responding for the Dashboard.cshtml MoviePage to show individual movie
    [HttpGet("movies/{id}")] //post is submitting the form, get is retriving the info
    public IActionResult MoviePage(int id)
    {
        //retrieve the movie first
        ViewBag.Movie = _context
            .Movies
            //one to many relationship: each movie I find, I want to return the movie that is PostedBy
            .Include(movie => movie.PostedBy)
            //populate the likes first
            .Include(movie => movie.Likes)
                //and then populate the users
                .ThenInclude(like => like.UserWhoLikes)
            .FirstOrDefault(movie => movie.MovieId == id); //take a movie, see if the the MovieId is the same with provided id

        //we want to have a delete button on MoviePage, so we need to get the User in the cshtml as well
        ViewBag.User = _context
            .Users
            .Find(HttpContext.Session.GetInt32("UserId"));

        return View(); //this view goes to MoivePage.cshtml
    }

    // User in session add a like to the movie
    [HttpPost("movies/{id}/likes")]
    public IActionResult AddLikeToMovie(int id)
    {
        var likeToAdd = new Like();
        likeToAdd.UserId = HttpContext.Session.GetInt32("UserId").GetValueOrDefault();
        likeToAdd.MovieId = id;

        _context.Add(likeToAdd);
        _context.SaveChanges();

        return RedirectToAction("Dashboard");
    }

    // User in session  unlike to the movie
    [HttpPost("movies/{id}/likes/delete")]
    public IActionResult RemoveLikeFromMovie(int id)
    {
        //get the userId
        var userId = HttpContext.Session.GetInt32("UserId").GetValueOrDefault();
        //get the like that need to remove
        var likeToRemove = _context
            .Likes
        //get the first like where the movie id matches the id we took in the form, and the user id in session matches the user id
            .FirstOrDefault(like => like.MovieId == id && like.UserId == userId);

        _context.Likes.Remove(likeToRemove);
        _context.SaveChanges();

        return RedirectToAction("Dashboard");
    }

    // User who posted can delte
    [HttpPost("movies/{id}/delete")]
    public IActionResult DeleteMovie(int id)
    {
        //get the movie that need to remove
        var movieToDelete = _context
            .Movies
        //get the first like where the movie id matches the id we took in the form, and the user id in session matches the user id
            .Find(id);

        _context.Movies.Remove(movieToDelete);
        _context.SaveChanges();

        return RedirectToAction("Dashboard");
    }

}