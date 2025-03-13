using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CLIMFinders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialv1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleNanme = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleNanme = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleColors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleMakes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleMakes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanServices_SubscriptionPlans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "SubscriptionPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    SubRoleId = table.Column<int>(type: "int", nullable: true),
                    TierId = table.Column<int>(type: "int", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: true),
                    ConfirmedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Photo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ConfirmationCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubscriptionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddedById = table.Column<int>(type: "int", nullable: false),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<int>(type: "int", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_SubRoles_SubRoleId",
                        column: x => x.SubRoleId,
                        principalTable: "SubRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_SubscriptionPlans_TierId",
                        column: x => x.TierId,
                        principalTable: "SubscriptionPlans",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VehicleModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MakeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleModels_VehicleMakes_MakeId",
                        column: x => x.MakeId,
                        principalTable: "VehicleMakes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedById = table.Column<int>(type: "int", nullable: false),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<int>(type: "int", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Searches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VIN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SearchDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Paid = table.Column<bool>(type: "bit", nullable: false),
                    AddedById = table.Column<int>(type: "int", nullable: false),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<int>(type: "int", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Searches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Searches_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAddress",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedById = table.Column<int>(type: "int", nullable: false),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<int>(type: "int", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAddress_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VIN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MakeId = table.Column<int>(type: "int", nullable: false),
                    ModelId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    ColorId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PickedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BusinessId = table.Column<int>(type: "int", nullable: false),
                    AddedById = table.Column<int>(type: "int", nullable: false),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<int>(type: "int", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_UserAddress_BusinessId",
                        column: x => x.BusinessId,
                        principalSchema: "dbo",
                        principalTable: "UserAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleColors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "VehicleColors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleMakes_MakeId",
                        column: x => x.MakeId,
                        principalTable: "VehicleMakes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleModels_ModelId",
                        column: x => x.ModelId,
                        principalTable: "VehicleModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    MatchedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notified = table.Column<bool>(type: "bit", nullable: false),
                    AddedById = table.Column<int>(type: "int", nullable: false),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<int>(type: "int", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleNanme" },
                values: new object[,]
                {
                    { 1, "Users" },
                    { 2, "Business" },
                    { 3, "SuperAdmin" }
                });

            migrationBuilder.InsertData(
                table: "SubRoles",
                columns: new[] { "Id", "RoleNanme" },
                values: new object[,]
                {
                    { 1, "Tow" },
                    { 2, "Impound" }
                });

            migrationBuilder.InsertData(
                table: "SubscriptionPlans",
                columns: new[] { "Id", "Amount", "Duration", "Tier" },
                values: new object[,]
                {
                    { 1, 10m, 1, "User Registration (Car Owners)" },
                    { 2, 10m, 1, "Business Registration (Tow or Impound)" }
                });

            migrationBuilder.InsertData(
                table: "VehicleColors",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Black" },
                    { 2, "White" },
                    { 3, "Gray" },
                    { 4, "Silver" },
                    { 5, "Red" },
                    { 6, "Blue" },
                    { 7, "Green" },
                    { 8, "Yellow" },
                    { 9, "Orange" },
                    { 10, "Brown" },
                    { 11, "Gold" },
                    { 12, "Beige" },
                    { 13, "Purple" },
                    { 14, "Pink" },
                    { 15, "Turquoise" }
                });

            migrationBuilder.InsertData(
                table: "VehicleMakes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Acura" },
                    { 2, "Alfa Romeo" },
                    { 3, "Aston Martin" },
                    { 4, "Audi" },
                    { 5, "Bentley" },
                    { 6, "BMW" },
                    { 7, "Bugatti" },
                    { 8, "Buick" },
                    { 9, "Cadillac" },
                    { 10, "Chevrolet" },
                    { 11, "Chrysler" },
                    { 12, "Dodge" },
                    { 13, "Ferrari" },
                    { 14, "Fiat" },
                    { 15, "Ford" },
                    { 16, "Genesis" },
                    { 17, "GMC" },
                    { 18, "Honda" },
                    { 19, "Hyundai" },
                    { 20, "Infiniti" },
                    { 21, "Jaguar" },
                    { 22, "Jeep" },
                    { 23, "Kia" },
                    { 24, "Lamborghini" },
                    { 25, "Land Rover" },
                    { 26, "Lexus" },
                    { 27, "Lincoln" },
                    { 28, "Lotus" },
                    { 29, "Maserati" },
                    { 30, "Mazda" },
                    { 31, "McLaren" },
                    { 32, "Mercedes-Benz" },
                    { 33, "Mini" },
                    { 34, "Mitsubishi" },
                    { 35, "Nissan" },
                    { 36, "Peugeot" },
                    { 37, "Porsche" },
                    { 38, "Ram" },
                    { 39, "Renault" },
                    { 40, "Rolls-Royce" },
                    { 41, "Saab" },
                    { 42, "Subaru" },
                    { 43, "Suzuki" },
                    { 44, "Tesla" },
                    { 45, "Toyota" },
                    { 46, "Volkswagen" },
                    { 47, "Volvo" }
                });

            migrationBuilder.InsertData(
                table: "PlanServices",
                columns: new[] { "Id", "Name", "PlanId" },
                values: new object[,]
                {
                    { 1, "User Registration.", 1 },
                    { 2, "Vehicle Search by VIN.", 1 },
                    { 3, "Access the search results.", 1 },
                    { 4, "Business registration", 2 },
                    { 5, "Vehicle Management", 2 },
                    { 6, "Notification System", 2 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AddedById", "AddedOn", "ConfirmationCode", "ConfirmedOn", "Email", "FullName", "IsConfirmed", "IsDeleted", "ModifiedById", "ModifiedOn", "PasswordHash", "PasswordSalt", "Photo", "RoleId", "SessionId", "SubRoleId", "SubscriptionId", "TierId" },
                values: new object[] { 1, 1, new DateTime(2025, 3, 2, 14, 47, 21, 443, DateTimeKind.Local).AddTicks(3230), "", new DateTime(2025, 3, 2, 14, 47, 21, 443, DateTimeKind.Local).AddTicks(3247), "admin@admin.com", "Super Admin", true, false, 1, new DateTime(2025, 3, 2, 14, 47, 21, 443, DateTimeKind.Local).AddTicks(3245), "fPkom/mVXggTnQneTrAyKFBcGZuUQIi4S1Fs+AriaUI=", "YHm6SCi7KwU2po6/CQBYnQ==", null, 3, "", null, "", null });

            migrationBuilder.InsertData(
                table: "VehicleModels",
                columns: new[] { "Id", "MakeId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "MDX" },
                    { 2, 1, "RDX" },
                    { 3, 1, "TLX" },
                    { 4, 2, "Giulia" },
                    { 5, 2, "Stelvio" },
                    { 6, 3, "DB11" },
                    { 7, 3, "Vantage" },
                    { 8, 4, "A3" },
                    { 9, 4, "A4" },
                    { 10, 4, "Q5" },
                    { 11, 4, "Q7" },
                    { 12, 5, "Continental GT" },
                    { 13, 5, "Bentayga" },
                    { 14, 6, "3 Series" },
                    { 15, 6, "5 Series" },
                    { 16, 6, "X5" },
                    { 17, 6, "X7" },
                    { 18, 7, "Chiron" },
                    { 19, 7, "Veyron" },
                    { 20, 8, "Enclave" },
                    { 21, 8, "Encore" },
                    { 22, 9, "Escalade" },
                    { 23, 9, "XT5" },
                    { 24, 10, "Silverado" },
                    { 25, 10, "Malibu" },
                    { 26, 10, "Camaro" },
                    { 27, 11, "300" },
                    { 28, 11, "Pacifica" },
                    { 29, 12, "Charger" },
                    { 30, 12, "Challenger" },
                    { 31, 13, "488" },
                    { 32, 13, "Roma" },
                    { 33, 14, "500" },
                    { 34, 14, "Panda" },
                    { 35, 15, "F-150" },
                    { 36, 15, "Mustang" },
                    { 37, 16, "G70" },
                    { 38, 16, "G90" },
                    { 39, 17, "Sierra" },
                    { 40, 17, "Yukon" },
                    { 41, 18, "Civic" },
                    { 42, 18, "Accord" },
                    { 43, 19, "Elantra" },
                    { 44, 19, "Santa Fe" },
                    { 45, 20, "Q50" },
                    { 46, 20, "QX80" },
                    { 47, 21, "F-PACE" },
                    { 48, 21, "XE" },
                    { 49, 22, "Wrangler" },
                    { 50, 22, "Grand Cherokee" },
                    { 51, 44, "Model S" },
                    { 52, 44, "Model 3" },
                    { 53, 44, "Model X" },
                    { 54, 44, "Model Y" },
                    { 55, 45, "Corolla" },
                    { 56, 45, "Camry" },
                    { 57, 45, "RAV4" },
                    { 58, 46, "Golf" },
                    { 59, 46, "Passat" },
                    { 60, 47, "XC90" },
                    { 61, 47, "S60" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_UserId",
                table: "Matches",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_VehicleId",
                table: "Matches",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanServices_PlanId",
                table: "PlanServices",
                column: "PlanId",
                unique: false);

            migrationBuilder.CreateIndex(
                name: "IX_Searches_UserId",
                table: "Searches",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_UserId",
                schema: "dbo",
                table: "UserAddress",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SubRoleId",
                table: "Users",
                column: "SubRoleId",
                unique: true,
                filter: "[SubRoleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TierId",
                table: "Users",
                column: "TierId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleModels_MakeId",
                table: "VehicleModels",
                column: "MakeId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_BusinessId",
                table: "Vehicles",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ColorId",
                table: "Vehicles",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_MakeId",
                table: "Vehicles",
                column: "MakeId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ModelId",
                table: "Vehicles",
                column: "ModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PlanServices");

            migrationBuilder.DropTable(
                name: "Searches");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "UserAddress",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VehicleColors");

            migrationBuilder.DropTable(
                name: "VehicleModels");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "VehicleMakes");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "SubRoles");

            migrationBuilder.DropTable(
                name: "SubscriptionPlans");
        }
    }
}
