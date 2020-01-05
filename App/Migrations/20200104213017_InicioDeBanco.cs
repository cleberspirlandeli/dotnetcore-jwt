using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Migrations
{
    public partial class InicioDeBanco : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Produto",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(maxLength: 50, nullable: false),
                    Descricao = table.Column<string>(maxLength: 100, nullable: false),
                    ValorPago = table.Column<decimal>(type: "decimal(9, 2)", nullable: true),
                    ValorVenda = table.Column<decimal>(type: "decimal(9, 2)", nullable: false),
                    Quantidade = table.Column<int>(nullable: false),
                    DataCompra = table.Column<DateTime>(nullable: true),
                    Ativo = table.Column<int>(nullable: true),
                    CriadoEm = table.Column<DateTime>(nullable: false),
                    EditadoEm = table.Column<DateTime>(nullable: false),
                    Usuario = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produto");
        }
    }
}
