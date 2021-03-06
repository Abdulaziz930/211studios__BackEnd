using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.ViewModels
{
    public class StudioDetailViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string BannerTitle { get; set; }

        public string BannerDescription { get; set; }

        public string Image { get; set; }

        public string BannerImage { get; set; }
    }
}
