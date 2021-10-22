using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class AppUser : IdentityUser, IEntity
    {
        [Required, StringLength(255)]
        public string FullName { get; set; }

        [Required, StringLength(255)]
        public string Position { get; set; }

        public string Image { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }

        public ICollection<UserSocialMedia> UserSocialMedias { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
