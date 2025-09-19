using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoDeEstacionamento.Infraestrutura.Orm.Migrations
{
    /// <inheritdoc />
    public partial class AtualizadoSoftware : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fatura_Tickets_TicketId",
                table: "Fatura");

            migrationBuilder.DropForeignKey(
                name: "FK_Saida_Tickets_TicketId",
                table: "Saida");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets");

            migrationBuilder.RenameTable(
                name: "Tickets",
                newName: "TBTicket");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_VeiculoId",
                table: "TBTicket",
                newName: "IX_TBTicket_VeiculoId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_NumeroTicket",
                table: "TBTicket",
                newName: "IX_TBTicket_NumeroTicket");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_Id",
                table: "TBTicket",
                newName: "IX_TBTicket_Id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCriacao",
                table: "TBTicket",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAtualizacaoSequencial",
                table: "TBTicket",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TBTicket",
                table: "TBTicket",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TBRegistroCheckIn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VeiculoId = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataHoraCheckIn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    NumeroTicket = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBRegistroCheckIn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBRegistroCheckIn_TBTicket_TicketId",
                        column: x => x.TicketId,
                        principalTable: "TBTicket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TBRegistroCheckIn_Veiculos_VeiculoId",
                        column: x => x.VeiculoId,
                        principalTable: "Veiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBRegistroCheckIn_Id",
                table: "TBRegistroCheckIn",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TBRegistroCheckIn_NumeroTicket",
                table: "TBRegistroCheckIn",
                column: "NumeroTicket",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TBRegistroCheckIn_TicketId",
                table: "TBRegistroCheckIn",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TBRegistroCheckIn_VeiculoId",
                table: "TBRegistroCheckIn",
                column: "VeiculoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fatura_TBTicket_TicketId",
                table: "Fatura",
                column: "TicketId",
                principalTable: "TBTicket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Saida_TBTicket_TicketId",
                table: "Saida",
                column: "TicketId",
                principalTable: "TBTicket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fatura_TBTicket_TicketId",
                table: "Fatura");

            migrationBuilder.DropForeignKey(
                name: "FK_Saida_TBTicket_TicketId",
                table: "Saida");

            migrationBuilder.DropTable(
                name: "TBRegistroCheckIn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TBTicket",
                table: "TBTicket");

            migrationBuilder.RenameTable(
                name: "TBTicket",
                newName: "Tickets");

            migrationBuilder.RenameIndex(
                name: "IX_TBTicket_VeiculoId",
                table: "Tickets",
                newName: "IX_Tickets_VeiculoId");

            migrationBuilder.RenameIndex(
                name: "IX_TBTicket_NumeroTicket",
                table: "Tickets",
                newName: "IX_Tickets_NumeroTicket");

            migrationBuilder.RenameIndex(
                name: "IX_TBTicket_Id",
                table: "Tickets",
                newName: "IX_Tickets_Id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCriacao",
                table: "Tickets",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAtualizacaoSequencial",
                table: "Tickets",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fatura_Tickets_TicketId",
                table: "Fatura",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Saida_Tickets_TicketId",
                table: "Saida",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
