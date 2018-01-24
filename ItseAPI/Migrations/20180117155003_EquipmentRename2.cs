using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ConventionalAndWhiteTariffCalculatorAPI.Migrations
{
    public partial class EquipmentRename2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Equipament",
                table: "Equipament");

            migrationBuilder.RenameTable(
                name: "Equipament",
                newName: "Equipment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Equipment",
                table: "Equipment",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Equipment",
                table: "Equipment");

            migrationBuilder.RenameTable(
                name: "Equipment",
                newName: "Equipament");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Equipament",
                table: "Equipament",
                column: "Id");
        }
    }
}
