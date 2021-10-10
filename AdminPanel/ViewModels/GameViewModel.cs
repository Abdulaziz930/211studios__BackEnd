using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.ViewModels
{
    public class GameViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string CategoryName { get; set; }
    }
}
