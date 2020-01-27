
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Map
{
    public class ServerErrorMap : IEntityTypeConfiguration<ServerError>
    {
        public void Configure(EntityTypeBuilder<ServerError> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql("getdate()");

        }
    }
}
