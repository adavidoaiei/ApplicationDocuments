using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainClasses
{
    public class Document
    {
        public string Nume { get; set; }
        public string PrenumeMama { get; set; }
        public string PrenumeTata { get; set; }
        public DateTime DataNasterii { get; set; }
        public string Marca { get; set; }
        public string NrIdentificare { get; set; }
        public string Culoare { get; set; }
        public DateTime AnFabricatie { get; set; }
    }
}
