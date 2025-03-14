using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CLIMFinders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addingnewfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactInformation",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImpoundFees",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LocationDetails",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReasonImpoundent",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AddedOn", "ConfirmedOn", "ModifiedOn", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 3, 13, 22, 12, 36, 616, DateTimeKind.Local).AddTicks(2305), new DateTime(2025, 3, 13, 22, 12, 36, 616, DateTimeKind.Local).AddTicks(2323), new DateTime(2025, 3, 13, 22, 12, 36, 616, DateTimeKind.Local).AddTicks(2319), "uhH/Uqw0qiRszjfNtpwL2rF1Q4NA073uWVeiVrtS1MM=", "KR8ZqEMVP6GxBegj3P/BjQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactInformation",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "ImpoundFees",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "LocationDetails",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "ReasonImpoundent",
                table: "Vehicles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AddedOn", "ConfirmedOn", "ModifiedOn", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 3, 3, 23, 12, 11, 303, DateTimeKind.Local).AddTicks(9332), new DateTime(2025, 3, 3, 23, 12, 11, 303, DateTimeKind.Local).AddTicks(9349), new DateTime(2025, 3, 3, 23, 12, 11, 303, DateTimeKind.Local).AddTicks(9347), "KjJCkgaNbvbHiyjLlwE8pp/er9/sMS9wY1S3xkKd6yM=", "3UreGTnq66ZLLOnphSWcEA==" });
        }
    }
}
