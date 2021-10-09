using Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Game : IEntity
    {
        public int Id { get; set; }

        [Required,StringLength(150)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string Image { get; set; }

        public bool IsDeleted { get; set; }

        public GameDetail GameDetail { get; set; }

        public ICollection<GameCategory> GameCategories { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
