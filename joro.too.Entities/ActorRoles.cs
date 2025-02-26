using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace joro.too.Entities
{
    public class ActorRoles
    {
        public int ActorId { get; set; }
        public int MediaId { get; set; }
        public Actor Actor { get; set; }
        public Media Media { get; set; }
        public string Role { get; set; }
    }
}
