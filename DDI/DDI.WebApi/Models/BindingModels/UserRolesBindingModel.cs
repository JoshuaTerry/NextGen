using System.ComponentModel.DataAnnotations;

namespace DDI.WebApi.Models.BindingModels
{
    public class UserRolesBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string[] Emails { get; set; }

        [Required]
        [Display(Name = "Roles")]
        public string[] Roles { get; set; }

    }
}