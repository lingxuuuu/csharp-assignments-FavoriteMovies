using System;
using System.Linq; // allows us to access query methods(FirstOrDefault)
using Microsoft.AspNetCore.Mvc;
using FavoriteMovies.Models; 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http; 

namespace FavoriteMovies.Controllers   
{
    public class UsersController : Controller   
    {
        
        //DB setup
        private readonly MyContext _context;

        //dependency injection
        public UsersController(MyContext myContext)
        {
            _context = myContext;
        }


        //for each route this controller is to handle:
        [HttpGet]       //type of request
        [Route("")]     //associated route string (exclude the leading /)
        public IActionResult LoginReg()
        {
            return View();
        }

        [HttpPost("users")]     
        public IActionResult Register(User userToCreate)
        {

            if(!ModelState.IsValid)
            {
                return View("LoginReg");
            }

            var existingUser = _context
                .Users
                .FirstOrDefault(user => user.Email == userToCreate.Email );   


            if (existingUser != null) //if there is a existing user
            {
                ModelState.AddModelError("Email", "Email already registered");
                return View("LoginReg");
            }

            //hash the password
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            userToCreate.Password = Hasher.HashPassword(userToCreate, userToCreate.Password);
            
            //Save the user to the DB
            _context.Add(userToCreate);
            _context.SaveChanges();

            //Save the user ID to session
            HttpContext.Session.SetInt32("UserId", userToCreate.UserId);

            // note that we'are sending the user to a different controller
            return RedirectToAction("Dashboard", "Home");

                
        }
    
    }
}