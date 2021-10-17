using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Social : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string SocialLink { get; set; }

        [Required]
        public string SocialIcon { get; set; }

        [Required]
        public string SocialName { get; set; }

        public bool IsDeleted { get; set; }
    }
}
