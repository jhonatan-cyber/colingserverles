using Azure.Data.Tables;
using Coling.Api.Curriculum.Contratos.Repositorio;
using Coling.Api.Curriculum.Modelo;
using Coling.Shared;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Api.Curriculum.Implementacion.Repositorio
{
    public class ProfesionRepositorio : IProfesionRepositorio
    {
        private readonly string cadenaconexion;
        private readonly string tabla;
        private readonly IConfiguration configuration;

        public ProfesionRepositorio(IConfiguration conf)
        {
            configuration = conf;
            cadenaconexion = configuration.GetSection("cadenaconexion").Value;
            tabla = "profesion";
        }


        public async Task<bool> actualizar(Profesion profesion)
        {
            try
            {
                var tableClient = new TableClient(cadenaconexion, tabla);
                var response = await tableClient.GetEntityAsync<Institucion>(partitionKey: profesion.PartitionKey, rowKey: profesion.RowKey);
                if (response != null && response.Value != null)
                {
                    var etag = response.Value.ETag;
                    await tableClient.UpdateEntityAsync(profesion, etag);
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

        public async Task<bool> crear(Profesion profesion)
        {
            try
            {
                var table = new TableClient(cadenaconexion, tabla);
                table.CreateIfNotExists();
                await table.UpsertEntityAsync(profesion);
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

        public async Task<List<Profesion>> listar()
        {
            try
            {
                var tableClient = new TableClient(cadenaconexion, tabla);
                var profesiones = new List<Profesion>();

                await foreach (var profesion in tableClient.QueryAsync<Profesion>())
                {
                    profesiones.Add(profesion);
                }

                return profesiones;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Profesion> obtener(string id)
        {
            try
            {
                var tableClient = new TableClient(cadenaconexion, tabla);
                var response = await tableClient.GetEntityAsync<Profesion>(partitionKey: "Educacion", rowKey: id);
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
