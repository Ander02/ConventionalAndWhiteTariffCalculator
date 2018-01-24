using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ConventionalAndWhiteTariffCalculatorAPI.Migrations
{
    public partial class powerDistribuitor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tariff_Concessionary_ConcessionaryId",
                table: "Tariff");

            migrationBuilder.DropTable(
                name: "Concessionary");

            migrationBuilder.RenameColumn(
                name: "ConcessionaryId",
                table: "Tariff",
                newName: "PowerDistribuitorId");

            migrationBuilder.RenameIndex(
                name: "IX_Tariff_ConcessionaryId",
                table: "Tariff",
                newName: "IX_Tariff_PowerDistribuitorId");

            migrationBuilder.CreateTable(
                name: "PowerDistribuitor",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerDistribuitor", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Tariff_PowerDistribuitor_PowerDistribuitorId",
                table: "Tariff",
                column: "PowerDistribuitorId",
                principalTable: "PowerDistribuitor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tariff_PowerDistribuitor_PowerDistribuitorId",
                table: "Tariff");

            migrationBuilder.DropTable(
                name: "PowerDistribuitor");

            migrationBuilder.RenameColumn(
                name: "PowerDistribuitorId",
                table: "Tariff",
                newName: "ConcessionaryId");

            migrationBuilder.RenameIndex(
                name: "IX_Tariff_PowerDistribuitorId",
                table: "Tariff",
                newName: "IX_Tariff_ConcessionaryId");

            migrationBuilder.CreateTable(
                name: "Concessionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concessionary", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Tariff_Concessionary_ConcessionaryId",
                table: "Tariff",
                column: "ConcessionaryId",
                principalTable: "Concessionary",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
