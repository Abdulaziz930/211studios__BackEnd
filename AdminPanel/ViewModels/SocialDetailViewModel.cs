using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.ViewModels
{
    public class SocialDetailViewModel
    {
        public int Id { get; set; }

        [Required]
        public string SocialLink { get; set; }

        [Required]
        public string SocialIcon { get; set; }

        [Required]
        public string SocialName { get; set; }
    }
}
