using Coling.Api.Curriculum.Contratos.Repositorio;
using Coling.Api.Curriculum.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Coling.Api.Curriculum.EndPoints
{
    public class InstitucionFunction
    {
        private readonly ILogger<InstitucionFunction> _logger;
        private readonly IInstitucionRepositorio repos;

        public InstitucionFunction(ILogger<InstitucionFunction> logger,IInstitucionRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("registrarInstitucion")]
        public async Task<HttpResponseData> insertarInstitucion([HttpTrigger(AuthorizationLevel.Function, "post", Route = "registrarInstitucion")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {

                var registro = await req.ReadFromJsonAsync<Institucion>() ?? throw new Exception("todos los campos son requeridos");
                 registro.RowKey = Guid.NewGuid().ToString();
                registro.Timestamp = DateTime.Now;
                registro.PartitionKey = "Educacion";


                bool res = await repos.crear(registro);
                    if (res)
                    {
                        respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                    else
                    {
                         respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }

            }
            catch (Exception ex)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }

           
        }

        [Function("listarInstituciones")]
        public async Task<HttpResponseData> ListarInstituciones(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "listarInstituciones")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var instituciones = await repos.listar();
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(instituciones);
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }

        [Function("obtenerInstitucion")]
        public async Task<HttpResponseData> ObtenerInstitucion(
             [HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerInstitucion/{id}")] HttpRequestData req,
             string id)
        {
            HttpResponseData respuesta;
            try
            {
                var institucion = await repos.obtener(id);
                if (institucion != null)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(institucion);
                    return respuesta;
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.NotFound);
                    return respuesta;
                }
            }
            catch (Exception ex)
            { 
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }

        [Function("eliminarInstitucion")]
        public async Task<HttpResponseData> EliminarInstitucion(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "eliminarInstitucion/{id}")] HttpRequestData req,
           string id)
        {
            HttpResponseData respuesta;
            try
            {
                bool res = await repos.eliminar(id);
                if (res)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar institución");
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
      
        
        [Function("actualizarInstitucion")]
        public async Task<HttpResponseData> ActualizarInstitucion(
          [HttpTrigger(AuthorizationLevel.Function, "post", Route = "actualizarInstitucion/{id}")] HttpRequestData req,
         string id)
         {
            HttpResponseData respuesta;
            try
            {
                var institucion = await req.ReadFromJsonAsync<Institucion>() ?? throw new Exception("Todos los campos son requeridos");
                institucion.RowKey = id;
                institucion.PartitionKey = "Educacion";
                bool res = await repos.actualizar(institucion);
                if (res)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }

    }
}

