using System.ComponentModel.DataAnnotations;
using System;

namespace DDI.WebApi.Models.BindingModels
{
    public class RegisterBindingModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; } 
        [Display(Name = "Email")]
        public string Email { get; set; }
        public bool IsActive { get; set; } = false;
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "Password!1";
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = "Password!1";
        [Display(Name = "Default Business Unit")]
        public Guid? DefaultBusinessUnitId { get; set; }
        [Display(Name = "Consitituent")]
        public Guid? ConsitituentId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        
    }
}