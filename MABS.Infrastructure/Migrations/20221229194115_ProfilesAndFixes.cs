using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MABSAPI.Migrations
{
    /// <inheritdoc />
    public partial class ProfilesAndFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AddressStatus_StatusId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AddressStreetType_StreetTypeId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorEvents_DoctorEventType_TypeId",
                table: "DoctorEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_DoctorStatus_StatusId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Facilites_FacilityStatus_StatusId",
                table: "Facilites");

            migrationBuilder.DropForeignKey(
                name: "FK_FacilityEvents_FacilityEventType_TypeId",
                table: "FacilityEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientEvents_PatientEventType_TypeId",
                table: "PatientEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_PatientStatus_StatusId",
                table: "Patients");

            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CallerProfileId",
                table: "PatientEvents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CallerProfileId",
                table: "FacilityEvents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "Facilites",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "Doctors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CallerProfileId",
                table: "DoctorEvents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProfileEventType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileEventType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfileStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfileType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_ProfileStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "ProfileStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Profiles_ProfileType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ProfileType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProfileEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<int>(type: "int", nullable: false),
                    AddInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CallerProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileEvents_ProfileEventType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ProfileEventType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProfileEvents_Profiles_CallerProfileId",
                        column: x => x.CallerProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProfileEvents_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ProfileEventType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Created" },
                    { 2, "Updated" },
                    { 3, "Deleted" }
                });

            migrationBuilder.InsertData(
                table: "ProfileStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Prepared" },
                    { 2, "Active" },
                    { 3, "Locked" },
                    { 4, "Deleted" }
                });

            migrationBuilder.InsertData(
                table: "ProfileType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Facility" },
                    { 2, "Patient" },
                    { 3, "Doctor" },
                    { 4, "Admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ProfileId",
                table: "Patients",
                column: "ProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientEvents_CallerProfileId",
                table: "PatientEvents",
                column: "CallerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityEvents_CallerProfileId",
                table: "FacilityEvents",
                column: "CallerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Facilites_ProfileId",
                table: "Facilites",
                column: "ProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_ProfileId",
                table: "Doctors",
                column: "ProfileId",
                unique: true,
                filter: "[ProfileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorEvents_CallerProfileId",
                table: "DoctorEvents",
                column: "CallerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileEvents_CallerProfileId",
                table: "ProfileEvents",
                column: "CallerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileEvents_ProfileId",
                table: "ProfileEvents",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileEvents_TypeId",
                table: "ProfileEvents",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_StatusId",
                table: "Profiles",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_TypeId",
                table: "Profiles",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AddressStatus_StatusId",
                table: "Addresses",
                column: "StatusId",
                principalTable: "AddressStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AddressStreetType_StreetTypeId",
                table: "Addresses",
                column: "StreetTypeId",
                principalTable: "AddressStreetType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorEvents_DoctorEventType_TypeId",
                table: "DoctorEvents",
                column: "TypeId",
                principalTable: "DoctorEventType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorEvents_Profiles_CallerProfileId",
                table: "DoctorEvents",
                column: "CallerProfileId",
                principalTable: "Profiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_DoctorStatus_StatusId",
                table: "Doctors",
                column: "StatusId",
                principalTable: "DoctorStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Profiles_ProfileId",
                table: "Doctors",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id");

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
                name: "FK_FacilityEvents_FacilityEventType_TypeId",
                table: "FacilityEvents",
                column: "TypeId",
                principalTable: "FacilityEventType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityEvents_Profiles_CallerProfileId",
                table: "FacilityEvents",
                column: "CallerProfileId",
                principalTable: "Profiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientEvents_PatientEventType_TypeId",
                table: "PatientEvents",
                column: "TypeId",
                principalTable: "PatientEventType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientEvents_Profiles_CallerProfileId",
                table: "PatientEvents",
                column: "CallerProfileId",
                principalTable: "Profiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_PatientStatus_StatusId",
                table: "Patients",
                column: "StatusId",
                principalTable: "PatientStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Profiles_ProfileId",
                table: "Patients",
                column: "ProfileId",
                principalTable: "Profiles",
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
                name: "FK_DoctorEvents_DoctorEventType_TypeId",
                table: "DoctorEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorEvents_Profiles_CallerProfileId",
                table: "DoctorEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_DoctorStatus_StatusId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Profiles_ProfileId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Facilites_FacilityStatus_StatusId",
                table: "Facilites");

            migrationBuilder.DropForeignKey(
                name: "FK_Facilites_Profiles_ProfileId",
                table: "Facilites");

            migrationBuilder.DropForeignKey(
                name: "FK_FacilityEvents_FacilityEventType_TypeId",
                table: "FacilityEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_FacilityEvents_Profiles_CallerProfileId",
                table: "FacilityEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientEvents_PatientEventType_TypeId",
                table: "PatientEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientEvents_Profiles_CallerProfileId",
                table: "PatientEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_PatientStatus_StatusId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Profiles_ProfileId",
                table: "Patients");

            migrationBuilder.DropTable(
                name: "ProfileEvents");

            migrationBuilder.DropTable(
                name: "ProfileEventType");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "ProfileStatus");

            migrationBuilder.DropTable(
                name: "ProfileType");

            migrationBuilder.DropIndex(
                name: "IX_Patients_ProfileId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_PatientEvents_CallerProfileId",
                table: "PatientEvents");

            migrationBuilder.DropIndex(
                name: "IX_FacilityEvents_CallerProfileId",
                table: "FacilityEvents");

            migrationBuilder.DropIndex(
                name: "IX_Facilites_ProfileId",
                table: "Facilites");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_ProfileId",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_DoctorEvents_CallerProfileId",
                table: "DoctorEvents");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "CallerProfileId",
                table: "PatientEvents");

            migrationBuilder.DropColumn(
                name: "CallerProfileId",
                table: "FacilityEvents");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Facilites");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "CallerProfileId",
                table: "DoctorEvents");

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
                name: "FK_DoctorEvents_DoctorEventType_TypeId",
                table: "DoctorEvents",
                column: "TypeId",
                principalTable: "DoctorEventType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_DoctorStatus_StatusId",
                table: "Doctors",
                column: "StatusId",
                principalTable: "DoctorStatus",
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
                name: "FK_FacilityEvents_FacilityEventType_TypeId",
                table: "FacilityEvents",
                column: "TypeId",
                principalTable: "FacilityEventType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientEvents_PatientEventType_TypeId",
                table: "PatientEvents",
                column: "TypeId",
                principalTable: "PatientEventType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_PatientStatus_StatusId",
                table: "Patients",
                column: "StatusId",
                principalTable: "PatientStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
