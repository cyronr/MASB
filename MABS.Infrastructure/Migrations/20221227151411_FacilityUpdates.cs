using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MABSAPI.Migrations
{
    /// <inheritdoc />
    public partial class FacilityUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacilityEvent_Facilites_FacilityId",
                table: "FacilityEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_FacilityEvent_FacilityEventType_TypeId",
                table: "FacilityEvent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FacilityEvent",
                table: "FacilityEvent");

            migrationBuilder.RenameTable(
                name: "FacilityEvent",
                newName: "FacilityEvents");

            migrationBuilder.RenameIndex(
                name: "IX_FacilityEvent_TypeId",
                table: "FacilityEvents",
                newName: "IX_FacilityEvents_TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_FacilityEvent_FacilityId",
                table: "FacilityEvents",
                newName: "IX_FacilityEvents_FacilityId");

            migrationBuilder.AlterColumn<int>(
                name: "FlatNumber",
                table: "Adressess",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FacilityEvents",
                table: "FacilityEvents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityEvents_Facilites_FacilityId",
                table: "FacilityEvents",
                column: "FacilityId",
                principalTable: "Facilites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityEvents_FacilityEventType_TypeId",
                table: "FacilityEvents",
                column: "TypeId",
                principalTable: "FacilityEventType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacilityEvents_Facilites_FacilityId",
                table: "FacilityEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_FacilityEvents_FacilityEventType_TypeId",
                table: "FacilityEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FacilityEvents",
                table: "FacilityEvents");

            migrationBuilder.RenameTable(
                name: "FacilityEvents",
                newName: "FacilityEvent");

            migrationBuilder.RenameIndex(
                name: "IX_FacilityEvents_TypeId",
                table: "FacilityEvent",
                newName: "IX_FacilityEvent_TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_FacilityEvents_FacilityId",
                table: "FacilityEvent",
                newName: "IX_FacilityEvent_FacilityId");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Countries",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(2)");

            migrationBuilder.AlterColumn<int>(
                name: "FlatNumber",
                table: "Adressess",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CountryId",
                table: "Adressess",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(2)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FacilityEvent",
                table: "FacilityEvent",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityEvent_Facilites_FacilityId",
                table: "FacilityEvent",
                column: "FacilityId",
                principalTable: "Facilites",
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
    }
}
