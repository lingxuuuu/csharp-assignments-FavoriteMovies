using System;
using System.ComponentModel.DataAnnotations; //let us use [key]
using System.ComponentModel.DataAnnotations.Schema; //let us use [NotMapped] on PSconfirmation
using System.Collections.Generic;


namespace FavoriteMovies.Models
{
    public class Movie
    {
        [Key] //signifies that this is the unique identifier(example: Like a ssn number)

        public int MovieId { get; set; }

        [Required(ErrorMessage = "The title is required")]
        [MinLength(3, ErrorMessage = "Please ensure the title is at least 3 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The star is required")]
        [MinLength(2, ErrorMessage = "Please ensure the star is at least 2 characters")]
        public string Star { get; set; }

        [Required(ErrorMessage = "The release date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "The URL is required")]
        [MinLength(10, ErrorMessage = "Please ensure the image URL is at least 10 characters")]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        //the user ID
        public int UserId { get; set; }

        // the user object, get populated by .NET(not in the db originally)
        // Movie does not have a User PostedBy field. We need to use include() to connect the UserId with PostedBy
        public User PostedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        
        //navigation property to get all the likes
        public List<Like> Likes {get; set;}
        

    }
}