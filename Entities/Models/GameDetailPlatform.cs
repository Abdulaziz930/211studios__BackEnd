using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class GameDetailPlatform : IEntity
    {
        public int Id { get; set; }

        public int GameDetailId { get; set; }

        public GameDetail GameDetail { get; set; }

        public int PlatformId { get; set; }

        public Platform Platform { get; set; }
    }
}
