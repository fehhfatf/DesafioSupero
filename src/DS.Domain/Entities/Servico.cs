using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Domain.Entities
{
    public class Servico
    {
        public int ServicoId { get; set; }
        public int ClienteId { get; set; }
        public int FornecedorId { get; set; }
        public string Descricao { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataDeAtendimento { get; set; }
        public decimal ValorDoAtendimento { get; set; }
        public string TipoDeServico { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
    }
}
