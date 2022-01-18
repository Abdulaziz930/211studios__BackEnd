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
    public class Banner : IEntity
    {
        public int Id { get; set; }
        
        [Required, StringLength(maximumLength: 50)]
        public string Title { get; set; }

        [Required, StringLength(maximumLength: 150)]
        public string Description { get; set; }

        public string Image { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime LastModificationDate { get; set; }

        [ForeignKey("Menu")]
        public int MenuId { get; set; }

        public Menu Menu { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
