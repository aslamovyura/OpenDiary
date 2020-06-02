using System;

namespace Application.DTO
{
    /// <summary>
    /// Author Data Transfer Object (DTO).
    /// </summary>
    public class AuthorDTO
    {
        /// <summary>
        /// Author Identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User Identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Author first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Author last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Author birth date.
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Email address.
        /// </summary>
        public string Email { get; set; }

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
        /// Author avatar.
        /// </summary>
        public byte[] Avatar { get; set; }
    }
}