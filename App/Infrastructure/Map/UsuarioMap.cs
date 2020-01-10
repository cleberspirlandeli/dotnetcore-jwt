using App.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Map
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Perfil)
                .HasDefaultValue("user");

            builder
                .Property(x => x.Ativo)
                .HasDefaultValue(1);

            builder
                .Property(x => x.EmailAtivo)
                .HasDefaultValue(0);
        }
    }
}
