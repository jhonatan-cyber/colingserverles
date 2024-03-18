using Coling.Api.Curriculum.Contratos.Repositorio;
using Coling.Api.Curriculum.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Coling.Api.Curriculum.EndPoints
{
    public class EstudioFunction
    {
        private readonly ILogger<EstudioFunction> _logger;
        private readonly IEstudioRepositorio repos;

        public EstudioFunction(ILogger<EstudioFunction> logger, IEstudioRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("registrarEstudio")]
        public async Task<HttpResponseData> insertarEstudio([HttpTrigger(AuthorizationLevel.Function, "post", Route = "registrarEstudio")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {

                var registro = await req.ReadFromJsonAsync<Estudio>() ?? throw new Exception("todos los campos son requeridos");
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
        [Function("listarEstudio")]
        public async Task<HttpResponseData> ListarEstudios(
          [HttpTrigger(AuthorizationLevel.Function, "get", Route = "listarEstudio")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var estudios = await repos.listar();
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(estudios);
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }

        [Function("obtenerEstudio")]
        public async Task<HttpResponseData> ObtenerEstudio(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerEstudio/{id}")] HttpRequestData req,
           string id)
        {
            HttpResponseData respuesta;
            try
            {
                var estudio = await repos.obtener(id);
                if (estudio != null)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(estudio);
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

        [Function("eliminarEstudio")]
        public async Task<HttpResponseData> EliminarEstudio(
          [HttpTrigger(AuthorizationLevel.Function, "get", Route = "eliminarEstudio/{id}")] HttpRequestData req,
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
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }

        [Function("actualizarEstudio")]
        public async Task<HttpResponseData> ActualizarEstudio(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "actualizarEstudio/{id}")] HttpRequestData req,
       string id)
        {
            HttpResponseData respuesta;
            try
            {
                var estudio = await req.ReadFromJsonAsync<Estudio>() ?? throw new Exception("Todos los campos son requeridos");
                estudio.RowKey = id;
                estudio.PartitionKey = "Educacion";
                bool res = await repos.actualizar(estudio);
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
