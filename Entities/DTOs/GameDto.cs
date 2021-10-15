using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class GameDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public CategoryDto Category { get; set; }

        public string Image { get; set; }
    }
}
