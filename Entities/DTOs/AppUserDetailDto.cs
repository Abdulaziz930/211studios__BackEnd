using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class AppUserDetailDto
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Position { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public List<UserSocialMediaDto> UserSocialMediasDto { get; set; }
    }
}
