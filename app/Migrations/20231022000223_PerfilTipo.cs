using api;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app.Migrations
{
    /// <inheritdoc />
    public partial class PerfilTipo : Migration
    {
        /// <inheritdoc />
        /// Warning: this migration has custom code
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "Perfis",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            // #############
            // ## Begin custom code
            // #############
            
            var idPerfilBasico = Guid.NewGuid();

            migrationBuilder.InsertData
            (
                table: "Perfis",
                columns: new[] { "Id", "Nome", "Tipo" },
                values: new object[] { idPerfilBasico, "Básico", (int) TipoPerfil.Basico }
            );

            migrationBuilder.InsertData
            (
                table: "PerfilPermissoes",
                columns: new[] { "Id", "PerfilId", "Permissao" },
                values: new object[] { Guid.NewGuid(), idPerfilBasico, (int) Permissao.PerfilCadastrar }
            );

            migrationBuilder.InsertData
            (
                table: "PerfilPermissoes",
                columns: new[] { "Id", "PerfilId", "Permissao" },
                values: new object[] { Guid.NewGuid(), idPerfilBasico, (int) Permissao.PerfilRemover }
            );

            migrationBuilder.InsertData
            (
                table: "PerfilPermissoes",
                columns: new[] { "Id", "PerfilId", "Permissao" },
                values: new object[] { Guid.NewGuid(), idPerfilBasico, (int) Permissao.PerfilVisualizar }
            );

            migrationBuilder.InsertData
            (
                table: "PerfilPermissoes",
                columns: new[] { "Id", "PerfilId", "Permissao" },
                values: new object[] { Guid.NewGuid(), idPerfilBasico, (int) Permissao.PerfilEditar }
            );            

            migrationBuilder.InsertData
            (
                table: "Perfis",
                columns: new[] { "Id", "Nome", "Tipo" },
                values: new object[] { Guid.NewGuid(), "Administrador", (int) TipoPerfil.Administrador }
            );           

            // #############
            // ## End custom code
            // #############
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Perfis");
        }
    }
}
