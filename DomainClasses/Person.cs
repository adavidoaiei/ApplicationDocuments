using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainClasses
{
    public class Person
    {
        public string Nume { get; set; }     
        public string PrenumeMama { get; set; }
        public string PrenumeTata { get; set; }
        public DateTime DataNasterii { get; set; }
    }
}
