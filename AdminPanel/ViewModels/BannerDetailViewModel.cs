using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.ViewModels
{
    public class BannerDetailViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public string PageName { get; set; }

        public DateTime LastModificationDate { get; set; }
    }
}
