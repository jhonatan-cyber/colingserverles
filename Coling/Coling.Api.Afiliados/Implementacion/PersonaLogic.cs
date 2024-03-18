using Coling.Api.Afiliados.Interface;
using Coling.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Api.Afiliados.Implementacion
{
    internal class PersonaLogic : IPersonaService
    {
        private readonly Contexto contexto;

        public PersonaLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }


        public async Task<List<Persona>> ListarPersonas()
        {
         var personas = await contexto.Personas.ToListAsync();
         return personas;
        }

        public async Task<bool> EliminarPersona(int id)
        {
            var persona = await contexto.Personas.FindAsync(id);
            if (persona == null)
                return false;

            contexto.Personas.Remove(persona);
            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<Persona> ListarPersonaId(int id)
        {
            return await contexto.Personas.FindAsync(id);
        }

        public async Task<bool> ModificarPersona(Persona persona, int id)
        {
            var personaExistente = await contexto.Personas.FindAsync(id);
            if (personaExistente == null)
                return false;

            contexto.Entry(personaExistente).CurrentValues.SetValues(persona);
            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RegistarPersona(Persona persona)
        {
            contexto.Personas.Add(persona);
            await contexto.SaveChangesAsync();
            return true;
        }
    }
}
