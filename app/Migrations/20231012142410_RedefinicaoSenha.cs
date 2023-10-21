using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace app.Migrations
{
    /// <inheritdoc />
    public partial class RedefinicaoSenha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "senha",
                table: "Usuario",
                newName: "Senha");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "Usuario",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Usuario",
                newName: "Email");

            migrationBuilder.CreateTable(
                name: "RedefinicaoSenha",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uuid = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedefinicaoSenha", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RedefinicaoSenha_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RedefinicaoSenha_IdUsuario",
                table: "RedefinicaoSenha",
                column: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RedefinicaoSenha");

            migrationBuilder.RenameColumn(
                name: "Senha",
                table: "Usuario",
                newName: "senha");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Usuario",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Usuario",
                newName: "email");
        }
    }
}
