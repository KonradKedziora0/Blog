using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GitBlog.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        [Display(Name = "Nick")]
        public string FullName { get; set; }
        public string ImagePath { get; set; }
    }
}
