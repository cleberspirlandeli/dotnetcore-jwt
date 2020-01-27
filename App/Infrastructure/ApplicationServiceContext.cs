using App.Data.Domain;
using App.Data.Map;
using App.Data.Models;
using App.Infrastructure.Map;
using Microsoft.EntityFrameworkCore;

namespace App.Data
{
    public class ApplicationServiceContext : DbContext
    {
        public ApplicationServiceContext(DbContextOptions<ApplicationServiceContext> options) : base(options)
        {

        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProdutoMap());
            modelBuilder.ApplyConfiguration(new UsuarioMap());

            //modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
