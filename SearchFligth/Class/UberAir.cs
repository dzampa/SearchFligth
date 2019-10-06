using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchFligth
{
    public class UberAir
    {
        public string numero_voo { get; set; }
        public string aeroporto_origem { get; set; }
        public string aeroporto_destino { get; set; }
        public DateTime data { get; set; }
        public TimeSpan horario_saida { get; set; }
        public TimeSpan horario_chegada { get; set; }
        public decimal preco { get; set; }
    }
}
