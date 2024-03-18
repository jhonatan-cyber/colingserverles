﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coling.Shared;

namespace Coling.Api.Afiliados
{
    public class Contexto:DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) 
        { 

        }
        public virtual DbSet<Persona> Personas { get; set; }
    }
}
