using Coling.Api.Afiliados.Interface;
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
        private readonly IPersonaService persona;

        public PersonaFunction(ILogger<PersonaFunction> logger,IPersonaService persona)
        {
            _logger = logger;
            this.persona = persona;
        }

        [Function("PersonaFunction")]
        public async Task<HttpResponseData> ListarPersonas([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listarPersonas")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando: Azure Functions listar Persona");
            try
            {
                var personas = persona.ListarPersonas();
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
    }
}
