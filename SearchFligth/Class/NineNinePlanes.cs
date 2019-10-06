using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchFligth
{
    public class NineNinePlanes
    {
        public string voo { get; set; }
        public string origem { get; set; }
        public string destino { get; set; }
        public DateTime data_saida { get; set; }
        public TimeSpan saida { get; set; }
        public TimeSpan chegada { get; set; }
        public decimal valor { get; set; }
    }
}
