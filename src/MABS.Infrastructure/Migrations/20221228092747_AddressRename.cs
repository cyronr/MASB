using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MABSAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddressRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adressess_AddressStatus_StatusId",
                table: "Adressess");

            migrationBuilder.DropForeignKey(
                name: "FK_Adressess_AddressStreetType_StreetTypeId",
                table: "Adressess");

            migrationBuilder.DropForeignKey(
                name: "FK_Adressess_Countries_CountryId",
                table: "Adressess");

            migrationBuilder.DropForeignKey(
                name: "FK_Adressess_Facilites_FacilityId",
                table: "Adressess");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Adressess",
                table: "Adressess");

            migrationBuilder.RenameTable(
                name: "Adressess",
                newName: "Addresses");

            migrationBuilder.RenameIndex(
                name: "IX_Adressess_StreetTypeId",
                table: "Addresses",
                newName: "IX_Addresses_StreetTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Adressess_StatusId",
                table: "Addresses",
                newName: "IX_Addresses_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Adressess_FacilityId",
                table: "Addresses",
                newName: "IX_Addresses_FacilityId");

            migrationBuilder.RenameIndex(
                name: "IX_Adressess_CountryId",
                table: "Addresses",
                newName: "IX_Addresses_CountryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Id");  

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AddressStatus_StatusId",
                table: "Addresses",
                column: "StatusId",
                principalTable: "AddressStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AddressStreetType_StreetTypeId",
                table: "Addresses",
                column: "StreetTypeId",
                principalTable: "AddressStreetType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Countries_CountryId",
                table: "Addresses",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Facilites_FacilityId",
                table: "Addresses",
                column: "FacilityId",
                principalTable: "Facilites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AddressStatus_StatusId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AddressStreetType_StreetTypeId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Countries_CountryId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Facilites_FacilityId",
                table: "Addresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.DeleteData(
                table: "FacilityEventType",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "FacilityStatus",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "Adressess");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_StreetTypeId",
                table: "Adressess",
                newName: "IX_Adressess_StreetTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_StatusId",
                table: "Adressess",
                newName: "IX_Adressess_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_FacilityId",
                table: "Adressess",
                newName: "IX_Adressess_FacilityId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_CountryId",
                table: "Adressess",
                newName: "IX_Adressess_CountryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Adressess",
                table: "Adressess",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "FacilityEventType",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Deleted");

            migrationBuilder.UpdateData(
                table: "FacilityStatus",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Active");

            migrationBuilder.UpdateData(
                table: "FacilityStatus",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Deleted");

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
                name: "FK_Adressess_Countries_CountryId",
                table: "Adressess",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Adressess_Facilites_FacilityId",
                table: "Adressess",
                column: "FacilityId",
                principalTable: "Facilites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
