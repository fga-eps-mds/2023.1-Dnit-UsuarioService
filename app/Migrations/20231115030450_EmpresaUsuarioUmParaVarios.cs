using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app.Migrations
{
    /// <inheritdoc />
    public partial class EmpresaUsuarioUmParaVarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioEmpresa");

            migrationBuilder.DropColumn(
                name: "UFs",
                table: "Empresa");

            migrationBuilder.AddColumn<int>(
                name: "Associacao",
                table: "Usuario",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EmpresaCnpj",
                table: "Usuario",
                type: "character varying(14)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmpresaUFs",
                columns: table => new
                {
                    Uf = table.Column<int>(type: "integer", nullable: false),
                    EmpresaId = table.Column<string>(type: "character varying(14)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpresaUFs", x => new { x.EmpresaId, x.Uf });
                    table.ForeignKey(
                        name: "FK_EmpresaUFs_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Cnpj",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_EmpresaCnpj",
                table: "Usuario",
                column: "EmpresaCnpj");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Empresa_EmpresaCnpj",
                table: "Usuario",
                column: "EmpresaCnpj",
                principalTable: "Empresa",
                principalColumn: "Cnpj");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Empresa_EmpresaCnpj",
                table: "Usuario");

            migrationBuilder.DropTable(
                name: "EmpresaUFs");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_EmpresaCnpj",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Associacao",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "EmpresaCnpj",
                table: "Usuario");

            migrationBuilder.AddColumn<int[]>(
                name: "UFs",
                table: "Empresa",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);

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
    }
}
