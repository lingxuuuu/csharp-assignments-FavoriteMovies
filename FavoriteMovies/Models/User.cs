using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //let us use [NotMapped] on PSconfirmation


namespace FavoriteMovies.Models
{
    public class User
    {
        [Key] //signifies that this is the unique identifier(example: Like a ssn number)
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please provide your first name.")]
        [Display(Name="First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please provide your last name.")]
        [Display(Name="Last Name")]
        public string LastName { get; set; }
        
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please provide your email.")]
        [EmailAddress(ErrorMessage="Please providea valid email address")]
        public string Email { get; set; }
        
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter your password.")]
        [MinLength(8, ErrorMessage="Please enter a password of at least 8 charactors")]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please re-enter your password.")]
        [NotMapped] // we not not want the passwordconfirmation goes into DB
        [Compare("Password", ErrorMessage="Please ensure that the confirmation matches the password")]
        public string PasswordConfirmation { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}