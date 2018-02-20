using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adultus.Models
{
    public class Users
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Minimum length is 8")]
        [RegularExpression(@"^(?=.{6,})(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W).*$", ErrorMessage = "Must contain an upper and lower case letter, at least 1 number and at least 1 symbol")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm is required")]
        [Compare("Password", ErrorMessage = "Must be the same as Password")]
        public string ConfirmPassword { get; set; }

        public string PasswordHash { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public string Gender { get; set; }

        public string GenderPreference { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime JoinDate { get; set; }

        public string ProfilePic { get; set; }

        public string GalleryPic { get; set; }

        public string PrivateGalleryPic { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string PostCode { get; set; }

        public bool OnlineStatus { get; set; }

        public int Ranking { get; set; }

        public string ProfileId { get; set; }

        public int AccountBalance { get; set; }

        public DateTime LastLogin { get; set; }
    }
}