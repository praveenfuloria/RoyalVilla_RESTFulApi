using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoyalVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class CorrectTheTableNameVillaAmenties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VillaAmeities");

            migrationBuilder.CreateTable(
                name: "VillaAmenties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    VillaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VillaAmenties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VillaAmenties_Villa_VillaId",
                        column: x => x.VillaId,
                        principalTable: "Villa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VillaAmenties_VillaId",
                table: "VillaAmenties",
                column: "VillaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VillaAmenties");

            migrationBuilder.CreateTable(
                name: "VillaAmeities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VillaId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VillaAmeities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VillaAmeities_Villa_VillaId",
                        column: x => x.VillaId,
                        principalTable: "Villa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VillaAmeities_VillaId",
                table: "VillaAmeities",
                column: "VillaId");
        }
    }
}
