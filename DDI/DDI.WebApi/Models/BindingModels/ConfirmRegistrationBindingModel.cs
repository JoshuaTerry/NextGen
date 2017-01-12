using System.ComponentModel.DataAnnotations;

namespace DDI.WebApi.Models.BindingModels
{
    public class ConfirmRegistrationBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
    }
}