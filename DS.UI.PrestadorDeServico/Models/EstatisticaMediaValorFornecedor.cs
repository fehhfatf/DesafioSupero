using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DS.UI.PrestadorDeServico.Models
{
    public class EstatisticaMediaValorFornecedor
    {
        public string NomeDoFornecedor { get; set; }
        public string TipoDeServico { get; set; }
        public decimal ValorMedio { get; set; }
    }
}