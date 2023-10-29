using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace app.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaMunicipioERelacionamentoComUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MunicipioId",
                table: "Usuario",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Municipio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Uf = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipio", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_MunicipioId",
                table: "Usuario",
                column: "MunicipioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Municipio_MunicipioId",
                table: "Usuario",
                column: "MunicipioId",
                principalTable: "Municipio",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Municipio_MunicipioId",
                table: "Usuario");

            migrationBuilder.DropTable(
                name: "Municipio");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_MunicipioId",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "MunicipioId",
                table: "Usuario");
        }
    }
}
