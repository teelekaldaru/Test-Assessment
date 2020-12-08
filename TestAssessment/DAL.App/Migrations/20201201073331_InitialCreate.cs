using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.App.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ShipmentNumber = table.Column<string>(nullable: false),
                    Airport = table.Column<int>(nullable: false),
                    FlightNumber = table.Column<string>(nullable: false),
                    FlightDate = table.Column<DateTime>(nullable: false),
                    IsFinalized = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BagWithLetterses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BagNumber = table.Column<string>(maxLength: 15, nullable: false),
                    CountOfLetters = table.Column<int>(nullable: false),
                    Weight = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    ShipmentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BagWithLetterses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BagWithLetterses_Shipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BagWithParcelses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BagNumber = table.Column<string>(maxLength: 15, nullable: false),
                    ShipmentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BagWithParcelses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BagWithParcelses_Shipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parcels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ParcelNumber = table.Column<string>(nullable: false),
                    RecipientName = table.Column<string>(maxLength: 100, nullable: false),
                    DestinationCountry = table.Column<string>(maxLength: 2, nullable: false),
                    Weight = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    BagWithParcelsId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parcels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parcels_BagWithParcelses_BagWithParcelsId",
                        column: x => x.BagWithParcelsId,
                        principalTable: "BagWithParcelses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BagWithLetterses_BagNumber",
                table: "BagWithLetterses",
                column: "BagNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BagWithLetterses_ShipmentId",
                table: "BagWithLetterses",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_BagWithParcelses_BagNumber",
                table: "BagWithParcelses",
                column: "BagNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BagWithParcelses_ShipmentId",
                table: "BagWithParcelses",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_BagWithParcelsId",
                table: "Parcels",
                column: "BagWithParcelsId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_ParcelNumber",
                table: "Parcels",
                column: "ParcelNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_ShipmentNumber",
                table: "Shipments",
                column: "ShipmentNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BagWithLetterses");

            migrationBuilder.DropTable(
                name: "Parcels");

            migrationBuilder.DropTable(
                name: "BagWithParcelses");

            migrationBuilder.DropTable(
                name: "Shipments");
        }
    }
}
