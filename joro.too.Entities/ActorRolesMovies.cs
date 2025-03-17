using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace joro.too.Entities
{
    public class ActorRolesMovies
    {
        public int ActorId { get; set; }
        public int MovieId { get; set; }
        public Actor Actor { get; set; }
        public Movie Movie { get; set; }
        
        public string Role { get; set; }
    }
}
