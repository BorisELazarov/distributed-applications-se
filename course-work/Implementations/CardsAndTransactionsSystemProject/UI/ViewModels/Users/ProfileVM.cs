using DAL.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using UI.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;

namespace UI.ViewModels.Users
{
    public class ProfileVM
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Username { get; set; }
        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }
        [Required]
        public DateOnly BirthDate { get; set; }
        [Required]
        [DefaultValue(true)]
        public bool IsMale { get; set; }
        [Required]
        [DefaultValue("0")]
        public string Balance { get; set; }
    }
}
