using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class AppUserDetail : IEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }

        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }

        public AppUser AppUser { get; set; }

        public ICollection<UserSocialMedia> UserSocialMedias { get; set; }
    }
}
