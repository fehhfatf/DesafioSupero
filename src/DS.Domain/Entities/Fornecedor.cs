using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Domain.Entities
{
    public class Fornecedor
    {
        public Fornecedor()
        {
            Servicos = new List<Servico>();
        }

        public int FornecedorId { get; set; }
        public string Nome { get; set; }

        public ICollection<Servico> Servicos { get; set; }
    }
}
