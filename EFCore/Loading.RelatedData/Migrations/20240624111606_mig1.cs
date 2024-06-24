using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Loading.RelatedData.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salary = table.Column<int>(type: "int", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "İstanbul" },
                    { 2, "Tokat" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Name", "RegionId", "Salary", "Surname" },
                values: new object[,]
                {
                    { 1, "Pinar", 1, 1500, "Aliogullari" },
                    { 2, "Ufuk", 2, 1500, "Aliogullari" },
                    { 3, "Serkan", 1, 1500, "Aliogullari" },
                    { 4, "Emre", 2, 1500, "Kaya" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "EmployeeId", "OrderDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 6, 24, 14, 16, 6, 520, DateTimeKind.Local).AddTicks(7325) },
                    { 2, 1, new DateTime(2024, 6, 24, 14, 16, 6, 520, DateTimeKind.Local).AddTicks(7340) },
                    { 3, 2, new DateTime(2024, 6, 24, 14, 16, 6, 520, DateTimeKind.Local).AddTicks(7341) },
                    { 4, 2, new DateTime(2024, 6, 24, 14, 16, 6, 520, DateTimeKind.Local).AddTicks(7342) },
                    { 5, 3, new DateTime(2024, 6, 24, 14, 16, 6, 520, DateTimeKind.Local).AddTicks(7343) },
                    { 6, 3, new DateTime(2024, 6, 24, 14, 16, 6, 520, DateTimeKind.Local).AddTicks(7344) },
                    { 7, 3, new DateTime(2024, 6, 24, 14, 16, 6, 520, DateTimeKind.Local).AddTicks(7345) },
                    { 8, 4, new DateTime(2024, 6, 24, 14, 16, 6, 520, DateTimeKind.Local).AddTicks(7346) },
                    { 9, 4, new DateTime(2024, 6, 24, 14, 16, 6, 520, DateTimeKind.Local).AddTicks(7347) },
                    { 10, 1, new DateTime(2024, 6, 24, 14, 16, 6, 520, DateTimeKind.Local).AddTicks(7347) },
                    { 11, 2, new DateTime(2024, 6, 24, 14, 16, 6, 520, DateTimeKind.Local).AddTicks(7348) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RegionId",
                table: "Employees",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EmployeeId",
                table: "Orders",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
