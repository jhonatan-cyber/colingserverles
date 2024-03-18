using Azure;
using Azure.Data.Tables;
using Coling.Shared;

namespace Coling.Api.Curriculum.Modelo
{
    public class Estudio : IEstudio, ITableEntity
    {
        public string? Tipo { get; set; }
        public string? Afiliado_id { get; set; }
        public string? Grado { get; set; }
        public string? Titulo { get; set; }
        public string? Institucion_id { get; set; }
        public string? Año { get; set; }
        public string? Estado { get; set; }
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
