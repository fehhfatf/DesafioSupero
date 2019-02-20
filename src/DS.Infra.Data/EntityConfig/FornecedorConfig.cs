using DS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Infra.Data.EntityConfig
{
    public class FornecedorConfig : EntityTypeConfiguration<Fornecedor>
    {
        public FornecedorConfig()
        {
            HasKey(c => c.FornecedorId);

            Property(c => c.Nome)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(150);
        }
    }
}
