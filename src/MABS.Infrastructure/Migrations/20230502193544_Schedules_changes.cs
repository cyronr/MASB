using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MABSAPI.Migrations
{
    /// <inheritdoc />
    public partial class Scheduleschanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Facilities_FacilityId",
                table: "Schedules");*/

            migrationBuilder.RenameColumn(
                name: "FacilityId",
                table: "Schedules",
                newName: "AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Schedules_FacilityId",
                table: "Schedules",
                newName: "IX_Schedules_AddressId");

          /*  migrationBuilder.InsertData(
                table: "ScheduleEventType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Updated" });*/

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Addresses_AddressId",
                table: "Schedules",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Addresses_AddressId",
                table: "Schedules");

            migrationBuilder.DeleteData(
                table: "ScheduleEventType",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "Schedules",
                newName: "FacilityId");

            migrationBuilder.RenameIndex(
                name: "IX_Schedules_AddressId",
                table: "Schedules",
                newName: "IX_Schedules_FacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Facilities_FacilityId",
                table: "Schedules",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
