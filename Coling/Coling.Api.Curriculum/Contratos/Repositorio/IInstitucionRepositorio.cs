using Coling.Api.Curriculum.Modelo;

namespace Coling.Api.Curriculum.Contratos.Repositorio
{
    public interface IInstitucionRepositorio
    {
        public Task<bool>crear(Institucion institucion);
        public Task<List<Institucion>>listar();
        public Task<Institucion>obtener(string id);
        public Task<bool>actualizar(Institucion institucion);
        public Task<bool>eliminar(string id);
    }
}
