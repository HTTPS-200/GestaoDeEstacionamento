using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoDeEstacionamento.Infraestrutura.Orm.Migrations
{
    /// <inheritdoc />
    public partial class FixTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataAtualizacaoSequencial",
                table: "TBTicket");

            migrationBuilder.DropColumn(
                name: "UltimoNumeroSequencial",
                table: "TBTicket");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataAtualizacaoSequencial",
                table: "TBTicket",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UltimoNumeroSequencial",
                table: "TBTicket",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
