using Azure;
using Azure.Data.Tables;
using Coling.Shared;

namespace Coling.Api.Curriculum.Modelo
{
    public class Profesion : IProfesion, ITableEntity
    {
        public string? Nombre { get; set; }
        public string? Grado { get; set; }
        public string? Estado { get; set; }
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
