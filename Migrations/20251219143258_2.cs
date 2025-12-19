using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CvProjekt.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Bocker",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Soppa" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bocker",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
