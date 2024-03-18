using Coling.Api.Curriculum.Contratos.Repositorio;
using Coling.Api.Curriculum.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Coling.Api.Curriculum.EndPoints
{
    public class ProfesionFuction
    {
        private readonly ILogger<ProfesionFuction> _logger;
        private readonly IProfesionRepositorio repos;

        public ProfesionFuction(ILogger<ProfesionFuction> logger, IProfesionRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("registrarProfesion")]
        public async Task<HttpResponseData> InsertarProfesion([HttpTrigger(AuthorizationLevel.Function, "post", Route = "registrarProfesion")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {

                var registro = await req.ReadFromJsonAsync<Profesion>() ?? throw new Exception("todos los campos son requeridos");
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
        [Function("listarProfesiones")]
        public async Task<HttpResponseData> ListarProfesiones(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "listarProfesiones")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var profesiones = await repos.listar();
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(profesiones);
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }

        [Function("obtenerProfesion")]
        public async Task<HttpResponseData> ObtenerProfesion(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerProfesion/{id}")] HttpRequestData req,
            string id)
        {
            HttpResponseData respuesta;
            try
            {
                var profesion = await repos.obtener(id);
                if (profesion != null)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(profesion);
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

        [Function("eliminarProfesion")]
        public async Task<HttpResponseData> EliminarProfesion(
         [HttpTrigger(AuthorizationLevel.Function, "get", Route = "eliminarProfesion/{id}")] HttpRequestData req,
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


        [Function("actualizarProfesion")]
        public async Task<HttpResponseData> ActualizarProfesion(
          [HttpTrigger(AuthorizationLevel.Function, "post", Route = "actualizarProfesion/{id}")] HttpRequestData req,
         string id)
        {
            HttpResponseData respuesta;
            try
            {
                var profesion = await req.ReadFromJsonAsync<Profesion>() ?? throw new Exception("Todos los campos son requeridos");
                profesion.RowKey = id;
                profesion.PartitionKey = "Educacion";
                bool res = await repos.actualizar(profesion);
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
