using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ConventionalAndWhiteTariffCalculator.Migrations
{
    public partial class ConcessionaryAndTariffTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.CreateTable(
                name: "Concessionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concessionary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipament",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DefaultPower = table.Column<double>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipament", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tariff",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BaseValue = table.Column<double>(nullable: false),
                    ConcessionaryId = table.Column<Guid>(nullable: false),
                    FinishTime = table.Column<TimeSpan>(nullable: false),
                    InitTime = table.Column<TimeSpan>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tariff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tariff_Concessionary_ConcessionaryId",
                        column: x => x.ConcessionaryId,
                        principalTable: "Concessionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tariff_ConcessionaryId",
                table: "Tariff",
                column: "ConcessionaryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equipament");

            migrationBuilder.DropTable(
                name: "Tariff");

            migrationBuilder.DropTable(
                name: "Concessionary");

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DefaultPower = table.Column<double>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });
        }
    }
}
