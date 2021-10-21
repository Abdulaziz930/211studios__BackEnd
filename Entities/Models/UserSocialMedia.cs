using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class UserSocialMedia : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Link { get; set; }

        [Required]
        public string Icon { get; set; }

        public bool IsDeleted { get; set; }

        public int AppUserDetailId { get; set; }

        public AppUserDetail AppUserDetail { get; set; }
    }
}
