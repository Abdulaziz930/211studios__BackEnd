using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.ViewModels
{
    public class GameDetailViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public string Video { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastModificationDate { get; set; }

        public string Size { get; set; }

        public string Platform { get; set; }

        public string GameLink { get; set; }

        public List<string> Categories { get; set; }
    }
}
