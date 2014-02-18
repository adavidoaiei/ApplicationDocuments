using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ActeAuto.Models
{
    public class Document
    {
        [Key]
        public int id;
       
        public string Nume { get; set; }
        public string PrenumeMama { get; set; }
        public string PrenumeTata { get; set; }
        public string DataNasterii { get; set; }
        public string Domiciliul { get; set; }
        public string Strada { get; set; }
        public string Nr { get; set; }
        public string Sc { get; set; }
        public string Bl { get; set; }
        public string Ap { get; set; }
        public string Judet { get; set; }
        public string Marca { get; set; }
        public string NrIdentificare { get; set; }
        public string Culoare { get; set; }
        public string AnFabricatie { get; set; }
        public string Operatie { get; set; }
        public string DeLa { get; set; }
        public string Data { get; set; }
        
    }
}
