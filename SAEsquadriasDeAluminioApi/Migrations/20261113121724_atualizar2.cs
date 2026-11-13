using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAEsquadriasDeAluminioApi.Migrations
{
    /// <inheritdoc />
    public partial class atualizar2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VendedorId",
                table: "Orcamentos",
                newName: "UsuarioId");

            migrationBuilder.RenameColumn(
                name: "VendedorId",
                table: "LiberacoesInstalacaos",
                newName: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Orcamentos",
                newName: "VendedorId");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "LiberacoesInstalacaos",
                newName: "VendedorId");
        }
    }
}
