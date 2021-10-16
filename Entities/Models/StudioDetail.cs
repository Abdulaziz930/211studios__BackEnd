using Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class StudioDetail : IEntity
    {
        public int Id { get; set; }

        public string IntroDescription { get; set; }

        public string DetailImage { get; set; }

        [ForeignKey("Studio")]
        public int StudioId { get; set; }

        public Studio Studio { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
