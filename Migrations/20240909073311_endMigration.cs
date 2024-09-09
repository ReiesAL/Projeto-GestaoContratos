using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto_GestaoContratos.Migrations
{
    /// <inheritdoc />
    public partial class endMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UsuarioEmail",
                table: "Contratos",
                newName: "UsuarioInclusao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UsuarioInclusao",
                table: "Contratos",
                newName: "UsuarioEmail");
        }
    }
}
