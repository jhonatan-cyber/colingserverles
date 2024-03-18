using Coling.Api.Curriculum.Modelo;

namespace Coling.Api.Curriculum.Contratos.Repositorio
{
    public interface IEstudioRepositorio
    {
        public Task<bool> crear(Estudio estudio);
        public Task<List<Estudio>> listar();
        public Task<Estudio> obtener(string id);
        public Task<bool> actualizar(Estudio estudio);
        public Task<bool> eliminar(string id);
    }
}
