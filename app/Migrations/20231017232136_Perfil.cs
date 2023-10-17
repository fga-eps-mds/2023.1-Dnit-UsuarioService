using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app.Migrations
{
    /// <inheritdoc />
    public partial class Perfil : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PerfilId",
                table: "Usuario",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Perfis",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PerfilPermissoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PerfilId = table.Column<Guid>(type: "uuid", nullable: false),
                    Permissao = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilPermissoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerfilPermissoes_Perfis_PerfilId",
                        column: x => x.PerfilId,
                        principalTable: "Perfis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Email",
                table: "Usuario",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_PerfilId",
                table: "Usuario",
                column: "PerfilId");

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_Cnpj",
                table: "Empresa",
                column: "Cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PerfilPermissoes_PerfilId",
                table: "PerfilPermissoes",
                column: "PerfilId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Perfis_PerfilId",
                table: "Usuario",
                column: "PerfilId",
                principalTable: "Perfis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Perfis_PerfilId",
                table: "Usuario");

            migrationBuilder.DropTable(
                name: "PerfilPermissoes");

            migrationBuilder.DropTable(
                name: "Perfis");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_Email",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_PerfilId",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Empresa_Cnpj",
                table: "Empresa");

            migrationBuilder.DropColumn(
                name: "PerfilId",
                table: "Usuario");
        }
    }
}
