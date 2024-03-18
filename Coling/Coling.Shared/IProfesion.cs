using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public interface IProfesion
    {
        public string Nombre { get; set; }
        public string Grado { get; set; }
        public string Estado { get; set; }
    }
}
