using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Position { get; set; }

        public string Image { get; set; }

        public string Role { get; set; }

        public bool IsActive { get; set; }
    }
}
