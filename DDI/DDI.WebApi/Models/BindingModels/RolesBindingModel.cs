using System.ComponentModel.DataAnnotations;

namespace DDI.WebApi.Models.BindingModels
{
    public class RolesBindingModel
    {
        [Required]
        [Display(Name = "Roles")]
        public string[] Roles { get; set; }
    }
}