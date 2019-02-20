using DS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Infra.Data.EntityConfig
{
    public class ServicoConfig : EntityTypeConfiguration<Servico>
    {
        public ServicoConfig()
        {
            HasKey(c => c.ServicoId);

        }
    }
}
