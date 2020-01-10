using App.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Map
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.CriadoEm)
                .HasDefaultValueSql("getdate()");

            builder
                .Property(x => x.EditadoEm)
                .HasDefaultValueSql("getdate()");

            builder
                .Property(x => x.Ativo)
                .HasDefaultValue(1);
        }
    }
}
