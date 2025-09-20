using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoDeEstacionamento.Infraestrutura.Orm.Migrations
{
    /// <inheritdoc />
    public partial class UsuarioIdForVaga : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vaga_Veiculos_VeiculoId",
                table: "Vaga");

            migrationBuilder.DropIndex(
                name: "IX_Vaga_Id",
                table: "Vaga");

            migrationBuilder.DropIndex(
                name: "IX_Vaga_VeiculoId",
                table: "Vaga");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Vaga");

            migrationBuilder.AlterColumn<string>(
                name: "Zona",
                table: "Vaga",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Identificador",
                table: "Vaga",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "Ocupada",
                table: "Vaga",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ocupada",
                table: "Vaga");

            migrationBuilder.AlterColumn<string>(
                name: "Zona",
                table: "Vaga",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Identificador",
                table: "Vaga",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Vaga",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Vaga_Id",
                table: "Vaga",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vaga_VeiculoId",
                table: "Vaga",
                column: "VeiculoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vaga_Veiculos_VeiculoId",
                table: "Vaga",
                column: "VeiculoId",
                principalTable: "Veiculos",
                principalColumn: "Id");
        }
    }
}
