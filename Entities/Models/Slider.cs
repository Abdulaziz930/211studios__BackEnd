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
    public class Slider : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Image { get; set; }

        public bool IsDeleted { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
