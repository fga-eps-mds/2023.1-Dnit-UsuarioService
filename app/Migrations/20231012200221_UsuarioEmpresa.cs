using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app.Migrations
{
    /// <inheritdoc />
    public partial class UsuarioEmpresa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsuarioEmpresa",
                columns: table => new
                {
                    CnpjEmpresa = table.Column<string>(type: "character varying(14)", nullable: false),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioEmpresa", x => new { x.CnpjEmpresa, x.IdUsuario });
                    table.ForeignKey(
                        name: "FK_UsuarioEmpresa_Empresa_CnpjEmpresa",
                        column: x => x.CnpjEmpresa,
                        principalTable: "Empresa",
                        principalColumn: "Cnpj",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioEmpresa_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioEmpresa_IdUsuario",
                table: "UsuarioEmpresa",
                column: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioEmpresa");
        }
    }
}
