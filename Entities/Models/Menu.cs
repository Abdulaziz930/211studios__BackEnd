using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Menu : IEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string PageName { get; set; }

        public Banner Banner { get; set; }
    }
}
