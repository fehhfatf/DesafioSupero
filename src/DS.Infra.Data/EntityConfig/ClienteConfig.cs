using DS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Infra.Data.EntityConfig
{
    public class ClienteConfig : EntityTypeConfiguration<Cliente>
    {
        public ClienteConfig()
        {
            HasKey(c => c.ClienteId);

            Property(c => c.Nome)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(150);

            Property(c => c.Bairro)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(100);

            Property(c => c.Cidade)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(100);

            Property(c => c.Estado)
                .IsRequired()
                .HasColumnType("char")
                .HasMaxLength(2);                
        }
    }
}
