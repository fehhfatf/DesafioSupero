using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Domain.Entities
{
    public class Cliente
    {
        public Cliente()
        {
            Servicos = new List<Servico>();
        }

        public int ClienteId { get; set; }
        public string Nome { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        public virtual ICollection<Servico> Servicos { get; set; }
    }
}
