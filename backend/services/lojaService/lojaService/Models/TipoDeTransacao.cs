using System;
using System.Collections.Generic;
using System.Text;

namespace lojaService.Models
{
    public class TipoDeTransacao
    {
        public int Id { get; set; }

        public short Codigo { get; set; }

        public string Descricao { get; set; }

        public bool Entrada { get; set; }
    }
}
