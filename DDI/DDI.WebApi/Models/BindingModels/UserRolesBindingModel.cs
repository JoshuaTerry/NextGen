using System.ComponentModel.DataAnnotations;

namespace DDI.WebApi.Models.BindingModels
{
    public class UserRolesBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Roles")]
        public string[] Roles { get; set; }

    }
}