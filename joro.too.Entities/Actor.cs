﻿using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace joro.too.Entities
{
    public class Actor
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
        public string imgsrc { get; set; }

        public List<ActorRolesMovies>? RolesInMovies { get; set; }
        public List<ActorRolesShows>? RolesInShows { get; set; }
    }
}
