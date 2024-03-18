using Coling.Api.Curriculum.Modelo;

namespace Coling.Api.Curriculum.Contratos.Repositorio
{
    public interface IProfesionRepositorio
    {
        public Task<bool> crear(Profesion profesion);
        public Task<List<Profesion>> listar();
        public Task<Profesion> obtener(string id);
        public Task<bool> actualizar(Profesion profesion);
        public Task<bool> eliminar(string id);
    }
}
