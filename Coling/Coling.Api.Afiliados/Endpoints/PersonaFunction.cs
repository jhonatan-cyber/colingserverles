using Coling.Api.Afiliados.Implementacion;
using Coling.Api.Afiliados.Interface;
using Coling.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Coling.Api.Afiliados.Endpoints
{
    public class PersonaFunction
    {
        private readonly ILogger<PersonaFunction> _logger;
        private readonly IPersonaService personaLogic;

        public PersonaFunction(ILogger<PersonaFunction> logger,IPersonaService personaLogic)
        {
            _logger = logger;
            this.personaLogic = personaLogic;
        }

        [Function("PersonaFunction")]
        public async Task<HttpResponseData> ListarPersonas([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listarPersonas")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando: Azure Functions listar Persona");
            try
            {
                var personas = personaLogic.ListarPersonas();
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(personas.Result);
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                var error = req.CreateResponse(HttpStatusCode.BadRequest);
                await error.WriteStringAsync(e.Message);
                return error;
            }
        }
        
        [Function("PersonaFunction")]
        public async Task<HttpResponseData> RegistrarPersona([HttpTrigger(AuthorizationLevel.Function, "post", Route = "registrarPersona")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando: Azure Functions registrar Persona");
            try
            {
                var persona= await req.ReadFromJsonAsync<Persona>() ?? throw new Exception("Debe ingresar una persona con todos los datos.");
                bool resultado = await personaLogic.RegistarPersona(persona);
                if (resultado)
                {
                    var response = req.CreateResponse(HttpStatusCode.Created);
                    return response;
                }
                    var error = req.CreateResponse(HttpStatusCode.BadRequest);
                    await error.WriteStringAsync("Error al registrar la persona.");
                    return error;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteStringAsync(e.Message);
                return error;
            }
        }
        
        [Function("PersonaFunction")]
        public async Task<HttpResponseData> ModificarPersona([HttpTrigger(AuthorizationLevel.Function, "post", Route = "modificarPersona/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"Ejecutando: Azure Functions modificar Persona - ID: {id}");
            try
            {
                var persona = await req.ReadFromJsonAsync<Persona>();
                bool resultado = await personaLogic.ModificarPersona(persona, id);

                if (resultado)
                {
                    var response = req.CreateResponse(HttpStatusCode.OK);
                    return response;
                }
                else
                {
                    var error = req.CreateResponse(HttpStatusCode.NotFound);
                    await error.WriteStringAsync($"No se encontró la persona con ID {id}.");
                    return error;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                var error = req.CreateResponse(HttpStatusCode.BadRequest);
                await error.WriteStringAsync(e.Message);
                return error;
            }
        }

        [Function("PersonaFunction")]
        public async Task<HttpResponseData> EliminarPersona([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarPersona/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"Ejecutando: Azure Functions eliminar Persona - ID: {id}");
            try
            {
                bool resultado = await personaLogic.EliminarPersona(id);

                if (resultado)
                {
                    var response = req.CreateResponse(HttpStatusCode.NoContent);
                    return response;
                }
                else
                {
                    var error = req.CreateResponse(HttpStatusCode.NotFound);
                    await error.WriteStringAsync($"No se encontró la persona con ID {id}.");
                    return error;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                var error = req.CreateResponse(HttpStatusCode.BadRequest);
                await error.WriteStringAsync(e.Message);
                return error;
            }
        }

        [Function("PersonaFunction")]
        public async Task<HttpResponseData> ListarPersonaPorId([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listarPersona/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation($"Ejecutando: Azure Functions listar Persona por ID - ID: {id}");
            try
            {
                // Llamar al servicio para obtener la persona por ID
                var persona = await personaLogic.ListarPersonaId(id);

                if (persona != null)
                {
                    var response = req.CreateResponse(HttpStatusCode.OK);
                    await response.WriteAsJsonAsync(persona);
                    return response;
                }
                else
                {
                    var error = req.CreateResponse(HttpStatusCode.NotFound);
                    await error.WriteStringAsync($"No se encontró la persona con ID {id}.");
                    return error;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                var error = req.CreateResponse(HttpStatusCode.BadRequest);
                await error.WriteStringAsync(e.Message);
                return error;
            }
        }


    }
}
