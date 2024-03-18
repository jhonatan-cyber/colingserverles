using Azure.Data.Tables;
using Coling.Api.Curriculum.Contratos.Repositorio;
using Coling.Api.Curriculum.Modelo;
using Microsoft.Extensions.Configuration;

namespace Coling.Api.Curriculum.Implementacion.Repositorio
{
    public class EstudioRepositorio : IEstudioRepositorio
    {
        private readonly string cadenaconexion;
        private readonly string tabla;
        private readonly IConfiguration configuration;

        public EstudioRepositorio(IConfiguration conf)
        {
            configuration = conf;
            cadenaconexion = configuration.GetSection("cadenaconexion").Value;
            tabla = "estudio";
        }

        public async Task<bool> actualizar(Estudio estudio)
        {
            try
            {
                var tableClient = new TableClient(cadenaconexion, tabla);
                var response = await tableClient.GetEntityAsync<Estudio>(partitionKey: estudio.PartitionKey, rowKey: estudio.RowKey);
                if (response != null && response.Value != null)
                {
                    var etag = response.Value.ETag;
                    await tableClient.UpdateEntityAsync(estudio, etag);
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

        public async Task<bool> crear(Estudio estudio)
        {
            try
            {
                var table = new TableClient(cadenaconexion, tabla);
                table.CreateIfNotExists();
                await table.UpsertEntityAsync(estudio);
                return true;
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

        public async Task<List<Estudio>> listar()
        {
            try
            {
                var tableClient = new TableClient(cadenaconexion, tabla);
                var estudios = new List<Estudio>();

                await foreach (var estudio in tableClient.QueryAsync<Estudio>())
                {
                    estudios.Add(estudio);
                }

                return estudios;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Estudio> obtener(string id)
        {
            try
            {
                var tableClient = new TableClient(cadenaconexion, tabla);
                var response = await tableClient.GetEntityAsync<Estudio>(partitionKey: "educacion", rowKey: id);
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
