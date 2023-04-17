using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MABSAPI.Migrations
{
    /// <inheritdoc />
    public partial class Cities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Facilites_FacilityId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorsFacilities_Facilites_FacilityId",
                table: "DoctorsFacilities");

            migrationBuilder.DropForeignKey(
                name: "FK_Facilites_FacilityStatus_StatusId",
                table: "Facilites");

            migrationBuilder.DropForeignKey(
                name: "FK_Facilites_Profiles_ProfileId",
                table: "Facilites");

            migrationBuilder.DropForeignKey(
                name: "FK_FacilityEvents_Facilites_FacilityId",
                table: "FacilityEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Facilites",
                table: "Facilites");

            migrationBuilder.RenameTable(
                name: "Facilites",
                newName: "Facilities");

            migrationBuilder.RenameIndex(
                name: "IX_Facilites_StatusId",
                table: "Facilities",
                newName: "IX_Facilities_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Facilites_ProfileId",
                table: "Facilities",
                newName: "IX_Facilities_ProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Facilities",
                table: "Facilities",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Facilities_FacilityId",
                table: "Addresses",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorsFacilities_Facilities_FacilityId",
                table: "DoctorsFacilities",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Facilities_FacilityStatus_StatusId",
                table: "Facilities",
                column: "StatusId",
                principalTable: "FacilityStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Facilities_Profiles_ProfileId",
                table: "Facilities",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityEvents_Facilities_FacilityId",
                table: "FacilityEvents",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Facilities_FacilityId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorsFacilities_Facilities_FacilityId",
                table: "DoctorsFacilities");

            migrationBuilder.DropForeignKey(
                name: "FK_Facilities_FacilityStatus_StatusId",
                table: "Facilities");

            migrationBuilder.DropForeignKey(
                name: "FK_Facilities_Profiles_ProfileId",
                table: "Facilities");

            migrationBuilder.DropForeignKey(
                name: "FK_FacilityEvents_Facilities_FacilityId",
                table: "FacilityEvents");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Facilities",
                table: "Facilities");

            migrationBuilder.RenameTable(
                name: "Facilities",
                newName: "Facilites");

            migrationBuilder.RenameIndex(
                name: "IX_Facilities_StatusId",
                table: "Facilites",
                newName: "IX_Facilites_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Facilities_ProfileId",
                table: "Facilites",
                newName: "IX_Facilites_ProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Facilites",
                table: "Facilites",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Facilites_FacilityId",
                table: "Addresses",
                column: "FacilityId",
                principalTable: "Facilites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorsFacilities_Facilites_FacilityId",
                table: "DoctorsFacilities",
                column: "FacilityId",
                principalTable: "Facilites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Facilites_FacilityStatus_StatusId",
                table: "Facilites",
                column: "StatusId",
                principalTable: "FacilityStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Facilites_Profiles_ProfileId",
                table: "Facilites",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityEvents_Facilites_FacilityId",
                table: "FacilityEvents",
                column: "FacilityId",
                principalTable: "Facilites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
