using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DDI.WebApi.Models
{
    public class RolesBindingModel
    {
        [Required]
        [Display(Name = "Roles")]
        public string[] Roles { get; set; }
    }
}