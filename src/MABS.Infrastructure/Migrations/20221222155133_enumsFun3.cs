using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MABSAPI.Migrations
{
    /// <inheritdoc />
    public partial class enumsFun3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_DoctorStatus_DoctorStatusId",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "DoctorStatusId",
                table: "DoctorStatus",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "DoctorStatusId",
                table: "Doctors",
                newName: "StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Doctors_DoctorStatusId",
                table: "Doctors",
                newName: "IX_Doctors_StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_DoctorStatus_StatusId",
                table: "Doctors",
                column: "StatusId",
                principalTable: "DoctorStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_DoctorStatus_StatusId",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "DoctorStatus",
                newName: "DoctorStatusId");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Doctors",
                newName: "DoctorStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Doctors_StatusId",
                table: "Doctors",
                newName: "IX_Doctors_DoctorStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_DoctorStatus_DoctorStatusId",
                table: "Doctors",
                column: "DoctorStatusId",
                principalTable: "DoctorStatus",
                principalColumn: "DoctorStatusId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
