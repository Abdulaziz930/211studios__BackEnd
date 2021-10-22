using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.ViewModels
{
    public class UpdateUserViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Position { get; set; }

        [Required, EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public IFormFile Photo { get; set; }
    }
}
