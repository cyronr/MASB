using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MABSAPI.Migrations
{
    /// <inheritdoc />
    public partial class enumsAsFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adressess_Facilites_FacilitiesId",
                table: "Adressess");

            migrationBuilder.DropForeignKey(
                name: "FK_Adressess_StreetTypes_StreetTypeId",
                table: "Adressess");

            migrationBuilder.DropTable(
                name: "StreetTypes");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "FacilityEvent",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Facilites",
                newName: "StatusId");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "DoctorEvents",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Adressess",
                newName: "StatusId");

            migrationBuilder.RenameColumn(
                name: "FacilitiesId",
                table: "Adressess",
                newName: "FacilityId");

            migrationBuilder.RenameIndex(
                name: "IX_Adressess_FacilitiesId",
                table: "Adressess",
                newName: "IX_Adressess_FacilityId");

            migrationBuilder.CreateTable(
                name: "AddressStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AddressStreetType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressStreetType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoctorEventType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorEventType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FacilityEventType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityEventType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FacilityStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityStatus", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AddressStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Active" },
                    { 2, "Deleted" }
                });

            migrationBuilder.InsertData(
                table: "AddressStreetType",
                columns: new[] { "Id", "Name", "ShortName" },
                values: new object[,]
                {
                    { 1, "", "Street" },
                    { 2, "", "Avenue" }
                });

            migrationBuilder.InsertData(
                table: "DoctorEventType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Created" },
                    { 2, "Updated" },
                    { 3, "Deleted" }
                });

            migrationBuilder.InsertData(
                table: "FacilityEventType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Created" },
                    { 2, "Updated" },
                    { 3, "Deleted" }
                });

            migrationBuilder.InsertData(
                table: "FacilityStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Active" },
                    { 2, "Deleted" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacilityEvent_TypeId",
                table: "FacilityEvent",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Facilites_StatusId",
                table: "Facilites",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorEvents_TypeId",
                table: "DoctorEvents",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Adressess_StatusId",
                table: "Adressess",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adressess_AddressStatus_StatusId",
                table: "Adressess",
                column: "StatusId",
                principalTable: "AddressStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Adressess_AddressStreetType_StreetTypeId",
                table: "Adressess",
                column: "StreetTypeId",
                principalTable: "AddressStreetType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Adressess_Facilites_FacilityId",
                table: "Adressess",
                column: "FacilityId",
                principalTable: "Facilites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorEvents_DoctorEventType_TypeId",
                table: "DoctorEvents",
                column: "TypeId",
                principalTable: "DoctorEventType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Facilites_FacilityStatus_StatusId",
                table: "Facilites",
                column: "StatusId",
                principalTable: "FacilityStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityEvent_FacilityEventType_TypeId",
                table: "FacilityEvent",
                column: "TypeId",
                principalTable: "FacilityEventType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adressess_AddressStatus_StatusId",
                table: "Adressess");

            migrationBuilder.DropForeignKey(
                name: "FK_Adressess_AddressStreetType_StreetTypeId",
                table: "Adressess");

            migrationBuilder.DropForeignKey(
                name: "FK_Adressess_Facilites_FacilityId",
                table: "Adressess");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorEvents_DoctorEventType_TypeId",
                table: "DoctorEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_Facilites_FacilityStatus_StatusId",
                table: "Facilites");

            migrationBuilder.DropForeignKey(
                name: "FK_FacilityEvent_FacilityEventType_TypeId",
                table: "FacilityEvent");

            migrationBuilder.DropTable(
                name: "AddressStatus");

            migrationBuilder.DropTable(
                name: "AddressStreetType");

            migrationBuilder.DropTable(
                name: "DoctorEventType");

            migrationBuilder.DropTable(
                name: "FacilityEventType");

            migrationBuilder.DropTable(
                name: "FacilityStatus");

            migrationBuilder.DropIndex(
                name: "IX_FacilityEvent_TypeId",
                table: "FacilityEvent");

            migrationBuilder.DropIndex(
                name: "IX_Facilites_StatusId",
                table: "Facilites");

            migrationBuilder.DropIndex(
                name: "IX_DoctorEvents_TypeId",
                table: "DoctorEvents");

            migrationBuilder.DropIndex(
                name: "IX_Adressess_StatusId",
                table: "Adressess");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "FacilityEvent",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Facilites",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "DoctorEvents",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Adressess",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "FacilityId",
                table: "Adressess",
                newName: "FacilitiesId");

            migrationBuilder.RenameIndex(
                name: "IX_Adressess_FacilityId",
                table: "Adressess",
                newName: "IX_Adressess_FacilitiesId");

            migrationBuilder.CreateTable(
                name: "StreetTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreetTypes", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Adressess_Facilites_FacilitiesId",
                table: "Adressess",
                column: "FacilitiesId",
                principalTable: "Facilites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Adressess_StreetTypes_StreetTypeId",
                table: "Adressess",
                column: "StreetTypeId",
                principalTable: "StreetTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
