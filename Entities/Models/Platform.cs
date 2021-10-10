using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Platform : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Logo { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<GameDetailPlatform> GameDetailPlatforms { get; set; }
    }
}
