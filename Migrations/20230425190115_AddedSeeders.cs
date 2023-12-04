using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eVeterinarskaAmbulanta.Migrations
{
    public partial class AddedSeeders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Pets",
                columns: new[] { "Id", "Age", "Name", "Species", "UserId" },
                values: new object[] { 1, 3, "Slavko", "dog", 1 });

            migrationBuilder.InsertData(
                table: "Pets",
                columns: new[] { "Id", "Age", "Name", "Species", "UserId" },
                values: new object[] { 2, 7, "Mirko", "dog", 1 });

            migrationBuilder.InsertData(
                table: "Pets",
                columns: new[] { "Id", "Age", "Name", "Species", "UserId" },
                values: new object[] { 3, 10, "Luna", "turtle", 2 });

            migrationBuilder.InsertData(
                table: "Pets",
                columns: new[] { "Id", "Age", "Name", "Species", "UserId" },
                values: new object[] { 4, 4, "Maks", "cat", 2 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
