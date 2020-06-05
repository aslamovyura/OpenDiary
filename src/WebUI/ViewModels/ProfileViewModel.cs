using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Application.DTO;
using Microsoft.AspNetCore.Http;

namespace WebUI.ViewModels
{
    public class ProfileViewModel
    {
        /// <summary>
        /// Author Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User first name.
        /// </summary>
        [Required(ErrorMessage = "FirstNameRequired")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// User last name.
        /// </summary>
        [Required(ErrorMessage = "LastNameRequired")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        /// <summary>
        /// User's email.
        /// </summary>
        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "EmailTypeRequired")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// User's birth date.
        /// </summary>
        [Required(ErrorMessage = "BirthDateRequired")]
        [DataType(DataType.Date, ErrorMessage = "DateTypeRequired")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "BirthDate")]
        public string BirthDate { get; set; }

        /// <summary>
        /// User age.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// About informations.
        /// </summary>
        public string About { get; set; }

        /// <summary>
        /// Hobbies informations.
        /// </summary>
        public string Hobbies { get; set; }

        /// <summary>
        /// Profession informations.
        /// </summary>
        public string Profession { get; set; }

        /// <summary>
        /// Current reader Id.
        /// </summary>
        public int CurrentReaderId { get; set; }

        /// <summary>
        /// Total number of user posts.
        /// </summary>
        public int TotalPostsNumber { get; set; }

        /// <summary>
        /// Total number of number comments.
        /// </summary>
        public int TotalCommentsNumber { get; set; }

        /// <summary>
        /// Author avatar.
        /// </summary>
        public byte[] Avatar { get; set; } = null;

        /// <summary>
        /// Uploaded data.
        /// </summary>
        public IFormFile UploadedData { get; set; } = null;

        /// <summary>
        /// Latest posts
        /// </summary>
        public ICollection<PostDTO> Posts { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ProfileViewModel()
        {
            Posts = new List<PostDTO>();
        }
    }
}