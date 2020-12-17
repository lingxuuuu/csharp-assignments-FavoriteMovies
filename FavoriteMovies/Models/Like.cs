using System;
using System.ComponentModel.DataAnnotations;


namespace FavoriteMovies.Models
{
    public class Like
    {
        public int LikeId { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public User UserWhoLikes { get; set; }
        public Movie Movie { get; set; }
    }

}