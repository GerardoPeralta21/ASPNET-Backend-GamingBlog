using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiGames.Migrations
{
    /// <inheritdoc />
    public partial class relacionUsuarioTienda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TiendaId",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_TiendaId",
                table: "Usuarios",
                column: "TiendaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Tiendas_TiendaId",
                table: "Usuarios",
                column: "TiendaId",
                principalTable: "Tiendas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Tiendas_TiendaId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_TiendaId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "TiendaId",
                table: "Usuarios");
        }
    }
}
