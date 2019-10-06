using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchFligth.Class
{
    public class FlightList
    {
        public string origem { get; set; }
        public string destino { get; set; }
        public string saida { get; set; }
        public string chegada { get; set; }		
        public List<Trechos> trechos { get; set; }
        
    }

    public class Trechos
    {
        
	    public string origem { get; set; }
        public string destino { get; set; }
        public DateTime saida { get; set; }
        public DateTime chegada { get; set; }
        public string operadora { get; set; }
        public decimal preco { get; set; }

    }
}
