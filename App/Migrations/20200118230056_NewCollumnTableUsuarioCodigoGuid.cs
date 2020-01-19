using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Migrations
{
    public partial class NewCollumnTableUsuarioCodigoGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Perfil",
                table: "Usuario",
                maxLength: 30,
                nullable: false,
                defaultValue: "user",
                oldClrType: typeof(string),
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<int>(
                name: "EmailAtivo",
                table: "Usuario",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Ativo",
                table: "Usuario",
                nullable: true,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoGuid",
                table: "Usuario",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoGuid",
                table: "Usuario");

            migrationBuilder.AlterColumn<string>(
                name: "Perfil",
                table: "Usuario",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldDefaultValue: "user");

            migrationBuilder.AlterColumn<int>(
                name: "EmailAtivo",
                table: "Usuario",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true,
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Ativo",
                table: "Usuario",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true,
                oldDefaultValue: 1);
        }
    }
}
