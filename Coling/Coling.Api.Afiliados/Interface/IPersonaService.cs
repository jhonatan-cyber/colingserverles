using Coling.Shared;

namespace Coling.Api.Afiliados.Interface
{
    public interface IPersonaService
    {
        public Task<bool> RegistarPersona(Persona persona);
        public Task<bool> ModificarPersona(Persona persona, int id);
        public Task<bool> EliminarPersona(int id);
        public Task<Persona> ListarPersonaId(int id);
        public Task<List<Persona>> ListarPersonas();

    }
}
