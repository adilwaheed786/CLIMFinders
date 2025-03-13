using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CLIMFinders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 🔥 Drop the existing unique index
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Users_SubRoleId ON Users;");

            // ✅ Recreate the index without the unique constraint
            migrationBuilder.CreateIndex(
                name: "IX_Users_SubRoleId",
                table: "Users",
                column: "SubRoleId"
            );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
