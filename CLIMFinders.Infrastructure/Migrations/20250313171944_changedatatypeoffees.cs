using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CLIMFinders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedatatypeoffees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ImpoundFees",
                table: "Vehicles",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AddedOn", "ConfirmedOn", "ModifiedOn", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 3, 13, 22, 19, 43, 517, DateTimeKind.Local).AddTicks(3614), new DateTime(2025, 3, 13, 22, 19, 43, 517, DateTimeKind.Local).AddTicks(3626), new DateTime(2025, 3, 13, 22, 19, 43, 517, DateTimeKind.Local).AddTicks(3624), "Tfn3Js+pF9AWxZf+OK65bV2HRHErWxA7MTJ4LKw85cI=", "Ftft8pIUs21aEEHBGHSDsg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImpoundFees",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AddedOn", "ConfirmedOn", "ModifiedOn", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 3, 13, 22, 12, 36, 616, DateTimeKind.Local).AddTicks(2305), new DateTime(2025, 3, 13, 22, 12, 36, 616, DateTimeKind.Local).AddTicks(2323), new DateTime(2025, 3, 13, 22, 12, 36, 616, DateTimeKind.Local).AddTicks(2319), "uhH/Uqw0qiRszjfNtpwL2rF1Q4NA073uWVeiVrtS1MM=", "KR8ZqEMVP6GxBegj3P/BjQ==" });
        }
    }
}
