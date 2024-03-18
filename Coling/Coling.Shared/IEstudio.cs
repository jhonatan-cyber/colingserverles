using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public interface IEstudio
    {
        public string Tipo { get; set; }
        public string Afiliado_id { get; set; }
        public string Grado { get; set; }
        public string Titulo { get; set; }
        public string Institucion_id { get; set; }
        public string Año { get; set; }
        public string Estado { get; set; }  
    }
}
