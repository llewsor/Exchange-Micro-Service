using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExchangeService.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trades",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseAmount = table.Column<decimal>(type: "decimal(12,10)", precision: 12, scale: 10, nullable: false),
                    TargetCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(12,10)", precision: 12, scale: 10, nullable: false),
                    TargetAmount = table.Column<decimal>(type: "decimal(12,10)", precision: 12, scale: 10, nullable: false),
                    TradeTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trades", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trades");
        }
    }
}
