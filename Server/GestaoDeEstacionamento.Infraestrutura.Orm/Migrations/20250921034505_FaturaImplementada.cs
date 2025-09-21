using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoDeEstacionamento.Infraestrutura.Orm.Migrations
{
    /// <inheritdoc />
    public partial class FaturaImplementada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fatura_TBTicket_TicketId",
                table: "Fatura");

            migrationBuilder.DropForeignKey(
                name: "FK_Fatura_Veiculos_VeiculoId",
                table: "Fatura");

            migrationBuilder.RenameColumn(
                name: "NumeroDiarias",
                table: "Fatura",
                newName: "Diarias");

            migrationBuilder.RenameColumn(
                name: "DataSaida",
                table: "Fatura",
                newName: "DataHoraSaida");

            migrationBuilder.RenameColumn(
                name: "DataEntrada",
                table: "Fatura",
                newName: "DataHoraEntrada");

            migrationBuilder.AddColumn<string>(
                name: "CPFHospede",
                table: "Fatura",
                type: "character varying(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "CheckInId",
                table: "Fatura",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "CorVeiculo",
                table: "Fatura",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataPagamento",
                table: "Fatura",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorVaga",
                table: "Fatura",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeloVeiculo",
                table: "Fatura",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NumeroTicket",
                table: "Fatura",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PlacaVeiculo",
                table: "Fatura",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ZonaVaga",
                table: "Fatura",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fatura_DataHoraSaida",
                table: "Fatura",
                column: "DataHoraSaida");

            migrationBuilder.CreateIndex(
                name: "IX_Fatura_NumeroTicket",
                table: "Fatura",
                column: "NumeroTicket",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fatura_Pago",
                table: "Fatura",
                column: "Pago");

            migrationBuilder.CreateIndex(
                name: "IX_Fatura_PlacaVeiculo",
                table: "Fatura",
                column: "PlacaVeiculo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Fatura_DataHoraSaida",
                table: "Fatura");

            migrationBuilder.DropIndex(
                name: "IX_Fatura_NumeroTicket",
                table: "Fatura");

            migrationBuilder.DropIndex(
                name: "IX_Fatura_Pago",
                table: "Fatura");

            migrationBuilder.DropIndex(
                name: "IX_Fatura_PlacaVeiculo",
                table: "Fatura");

            migrationBuilder.DropColumn(
                name: "CPFHospede",
                table: "Fatura");

            migrationBuilder.DropColumn(
                name: "CheckInId",
                table: "Fatura");

            migrationBuilder.DropColumn(
                name: "CorVeiculo",
                table: "Fatura");

            migrationBuilder.DropColumn(
                name: "DataPagamento",
                table: "Fatura");

            migrationBuilder.DropColumn(
                name: "IdentificadorVaga",
                table: "Fatura");

            migrationBuilder.DropColumn(
                name: "ModeloVeiculo",
                table: "Fatura");

            migrationBuilder.DropColumn(
                name: "NumeroTicket",
                table: "Fatura");

            migrationBuilder.DropColumn(
                name: "PlacaVeiculo",
                table: "Fatura");

            migrationBuilder.DropColumn(
                name: "ZonaVaga",
                table: "Fatura");

            migrationBuilder.RenameColumn(
                name: "Diarias",
                table: "Fatura",
                newName: "NumeroDiarias");

            migrationBuilder.RenameColumn(
                name: "DataHoraSaida",
                table: "Fatura",
                newName: "DataSaida");

            migrationBuilder.RenameColumn(
                name: "DataHoraEntrada",
                table: "Fatura",
                newName: "DataEntrada");

            migrationBuilder.AddForeignKey(
                name: "FK_Fatura_TBTicket_TicketId",
                table: "Fatura",
                column: "TicketId",
                principalTable: "TBTicket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fatura_Veiculos_VeiculoId",
                table: "Fatura",
                column: "VeiculoId",
                principalTable: "Veiculos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
