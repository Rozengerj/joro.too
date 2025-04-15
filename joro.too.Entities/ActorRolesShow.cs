using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace joro.too.Entities
{
    public class ActorRolesShows
    {
        public int ActorId { get; set; }
        public int ShowId { get; set; }
        public Actor Actor { get; set; }
        public Show Show { get; set; }
        public List<string> Roles { get; set; }
    }
}
