using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MABSAPI.Migrations
{
    /// <inheritdoc />
    public partial class enumsFun : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Doctors",
                newName: "DoctorStatusId");

            migrationBuilder.CreateTable(
                name: "DoctorStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorStatus", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "DoctorStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Active" },
                    { 2, "Deleted" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_DoctorStatusId",
                table: "Doctors",
                column: "DoctorStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_DoctorStatus_DoctorStatusId",
                table: "Doctors",
                column: "DoctorStatusId",
                principalTable: "DoctorStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_DoctorStatus_DoctorStatusId",
                table: "Doctors");

            migrationBuilder.DropTable(
                name: "DoctorStatus");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_DoctorStatusId",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "DoctorStatusId",
                table: "Doctors",
                newName: "Status");
        }
    }
}
