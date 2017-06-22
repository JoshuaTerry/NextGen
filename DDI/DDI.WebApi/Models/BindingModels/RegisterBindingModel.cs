using System.ComponentModel.DataAnnotations;
using System;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Client.Security;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.WebApi.Models.BindingModels
{
    public class RegisterBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Default Business Unit")]
        public Guid? DefaultBusinessUnitId { get; set; }
        public string UserName { get; internal set; }
        public string FullName { get; internal set; }
    }
}