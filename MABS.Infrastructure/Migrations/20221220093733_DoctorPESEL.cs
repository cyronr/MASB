using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MABSAPI.Migrations
{
    /// <inheritdoc />
    public partial class DoctorPESEL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PESEL",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PESEL",
                table: "Doctors");
        }
    }
}
