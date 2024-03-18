using Azure.Data.Tables;
using Coling.Api.Curriculum.Contratos.Repositorio;
using Coling.Api.Curriculum.Modelo;
using Microsoft.Extensions.Configuration;

namespace Coling.Api.Curriculum.Implementacion.Repositorio
{
    public class Institucionrepositorio : IInstitucionRepositorio
    {
        private readonly string cadenaconexion;
        private readonly string tabla;
        private readonly IConfiguration configuration;

        public Institucionrepositorio(IConfiguration conf)
        {
            configuration = conf;
            cadenaconexion = configuration.GetSection("cadenaconexion").Value;
            tabla = "institucion";
        }
        public async Task<bool> crear(Institucion institucion)
        {
            try {
                var table=new TableClient(cadenaconexion, tabla);
                table.CreateIfNotExists();
                await table.UpsertEntityAsync(institucion);
                return true;
            }catch(Exception ex) 
            {
            Console.WriteLine(ex.Message);
                return false;

            }
        }

        public async Task<bool> actualizar(Institucion institucion)
        {
            try
            {
                var tableClient = new TableClient(cadenaconexion, tabla);
                var response = await tableClient.GetEntityAsync<Institucion>(partitionKey: institucion.PartitionKey, rowKey: institucion.RowKey);
                if (response != null && response.Value != null)
                {
                    var etag = response.Value.ETag;
                    await tableClient.UpdateEntityAsync(institucion, etag);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public async Task<bool> eliminar(string id)
        {
            try
            {
                var tableClient = new TableClient(cadenaconexion, tabla);
                await tableClient.DeleteEntityAsync(partitionKey: "Educacion", rowKey: id);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<List<Institucion>> listar()
        {
            try
            {
                var tableClient = new TableClient(cadenaconexion, tabla);
                var instituciones = new List<Institucion>();

                await foreach (var institucion in tableClient.QueryAsync<Institucion>())
                {
                    instituciones.Add(institucion);
                }

                return instituciones;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<Institucion> obtener(string id)
        {
            try
            {
                var tableClient = new TableClient(cadenaconexion, tabla);
                var response = await tableClient.GetEntityAsync<Institucion>(partitionKey: "Educacion", rowKey: id);
                return response.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


    }
}
