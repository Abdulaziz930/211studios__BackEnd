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
    public class GameDetail : IEntity
    {
        public int Id { get; set; }

        public string Video { get; set; }

        public DateTime RelaseDate { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModificationDate { get; set; }

        [Required]
        public string Size { get; set; }

        [NotMapped]
        public IFormFile VideoFile { get; set; }

        [ForeignKey("Game")]
        public int GameId { get; set; }

        public Game Game { get; set; }

        public ICollection<GameDetailPlatform> GameDetailPlatforms { get; set; }
    }
}
