using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DS.UI.PrestadorDeServico.Models
{
    public class EstatisticaClientePorMesViewModel
    {
        public string Mes { get; set; }
        public string NomeDoCliente { get; set; }
        public decimal Valor { get; set; }
    }
}